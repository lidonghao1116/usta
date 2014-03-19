using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Data;
using System.IO;

public partial class Administrator_AssistantManager : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 
    public string assistantNo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //控制Tab的显示
            string fragmentFlag = "3";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, liFragment4
                , divFragment1, divFragment2, divFragment3, divFragment4);
         
            switch (fragmentFlag)
            {
                case "3":
                    DataListBind();
                    break;
                case "4":
                    assistantNo = Request["assistantNo"];
                    DalOperationAboutAssistant DalOperationAboutAssistant = new DalOperationAboutAssistant();
                    if (Request["del"] == "true" && Request["courseNo"] != null && Request["assistantNo"] != null && Request["coursesTeachersCorrelationId"] != null)
                    {
                        string coursesTeachersCorrelationId = Request["coursesTeachersCorrelationId"].ToString().Trim();
                        DalOperationAboutAssistant.DelCourseOfAssistantByCoursesTeachersCorrelationId(coursesTeachersCorrelationId);
                    }
                  
                    liFragment4.Visible = true;
                    lblassistamtName.Text= DalOperationAboutAssistant.GetAssistantbyId(assistantNo).assistantName;
                    
                    DataSet ds = DalOperationAboutAssistant.GetCoursesByAssistantNo(assistantNo);
                    dlstcoursesOfAssistamt.DataSource = ds.Tables[0];
                    dlstcoursesOfAssistamt.DataBind();
                    break;

            }
        }
    }

    //模糊查询
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataListBind();
    }
    //绑定搜索到的助教数据
    protected void DataListBind()
    {
        DalOperationUsers dos = new DalOperationUsers();
        DataView dv = dos.SearchUserByTypeAndKeywod(2, txtKeyword.Text.Trim()).DefaultView;

        this.AspNetPager2.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        this.dlSearchAssistant.DataSource = pds;
        this.dlSearchAssistant.DataBind();

        if (pds.Count > 0)
        {
            this.dlSearchAssistant.ShowFooter = false;
        }
    }
    //搜索到的助教信息分页
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataListBind();
    }
    protected void dlSearchAssistant_ItemCommand(object source, DataListCommandEventArgs e)
    {
        DalOperationAboutTeacher dalOperationAboutTeacher = new DalOperationAboutTeacher();

        if (e.CommandName == "cancelAssistant")
        {
            string assistantNo = this.dlSearchAssistant.DataKeys[e.Item.ItemIndex].ToString();//取选中行教师编号  

            dalOperationAboutTeacher.ChangeTeacherType(assistantNo, false);
            Javascript.AlertAndRedirect("取消助教身份成功！", "/Administrator/AssistantManager.aspx?page="+pageIndex, Page);
            DataListBind();
        }
    }
}
