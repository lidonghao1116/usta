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
using System.Data.SqlClient;
using System.Configuration;

public partial class CourseManager : System.Web.UI.Page
{
    //public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 
    string fragmentFlag = "3";

    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //控制Tab的显示

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                , divFragment1, divFragment2, divFragment3);

            if (fragmentFlag.Equals("3"))
            {
                //绑定学期标识下拉列表
                DataBindSearchTermTagList();
                //绑定课程列表
                DataBindSearchCourse();
            }
        }
    }
  
    //绑定学期标识下拉列表
    public void DataBindSearchTermTagList()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataTable dt = doac.FindAllTermTags().Tables[0];
        string termTag = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            termTag = dt.Rows[i]["termTag"].ToString().Trim();
            ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
            this.ddlSerachTermTags.Items.Add(li);
        }
    }

    //搜索课程列表
    protected void btnCommit_Click(object sender, EventArgs e)
    {
        //绑定搜索的课程信息
        DataBindSearchCourse();
    }

    //绑定搜索的课程信息
    public void DataBindSearchCourse()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataView dv = doac.SearchCourses(ddlSerachTermTags.SelectedValue, txtKeyword.Text.Trim()).Tables[0].DefaultView;

        this.AspNetPager2.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = AspNetPager2.CurrentPageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        this.dlSearchCourse.DataSource = pds;
        this.dlSearchCourse.DataBind();

        if (pds.Count > 0)
        {
            this.dlSearchCourse.ShowFooter = false;
        }
    }

    protected void ddlSerachTermTags_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBindSearchCourse();
    }

    //搜索的学期列表分页
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataBindSearchCourse();
    }
    //protected void delCourse()
    //{
    //    if (Request["action"] != null && Request["action"].Trim() == "delete")
    //    {
    //        DalOperationAboutCourses doac = new DalOperationAboutCourses();
    //        doac.DeleteCourseByNo(courseNo, termtag, classID);//删除课程与学生,教师,助教等表的关联，最后删除此课程 
    //        Javascript.AlertAndRedirect("删除成功！", "/Administrator/CourseManager.aspx?fragment=3", Page);
    //    }
    //}
}
