using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;

public partial class Teacher_VSWorkNOnlineWorkSubed : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment2");    
        //提交列表
        if (Request["excellent"] != null || Request["studentNo"] != null)
        {
            DalOperationAboutSchoolWorks doas = new DalOperationAboutSchoolWorks();
            if (Request["excellent"].ToString().Trim().Equals("true"))
            {
                doas.UpdateSchoolWorkByschoolWorkNofityIdAndStudentNo(Master.schoolworkNotifyId, Request["studentNo"].ToString().Trim(), 1);//设置为1：优秀实验
            }
            else if (Request["excellent"].ToString().Trim().Equals("false"))
            {
                doas.UpdateSchoolWorkByschoolWorkNofityIdAndStudentNo(Master.schoolworkNotifyId, Request["studentNo"].ToString().Trim(), 0);//设置为0：非优秀实验
            }
        }

        if (!CommonUtility.SafeCheckByParams<string>(Request["schoolworkNotifyId"], ref Master.schoolworkNotifyId))
        {
            Javascript.GoHistory(-1, Page);
            return;
        }

        DataBindSchoolWorks(Master.schoolworkNotifyId);
        
    }


    //绑定提交作业列表
    public void DataBindSchoolWorks(int schoolworkNotifyId)
    {
        DalOperationAboutSchoolWorks dalOperationAboutschoolwork = new DalOperationAboutSchoolWorks();
        string studentName = txtNameSearch.Text;
       
        DataView dv = null;
        if ((txtlow.Text != null && txtlow.Text.Length > 0) || (txthigh.Text != null && txthigh.Text.Length > 0))
        {
            float low = (txtlow.Text == null || txtlow.Text.Equals("")) ? float.MinValue : float.Parse(txtlow.Text.Trim());
            float high = (txthigh.Text == null || txtlow.Text.Equals("")) ? float.MaxValue : float.Parse(txthigh.Text.Trim());
            dv = dalOperationAboutschoolwork.FindSchoolWorksByschoolWorkNofityId(schoolworkNotifyId, studentName, low, high).Tables[0].DefaultView;
        }
        else
        {
            dv = dalOperationAboutschoolwork.FindSchoolWorksByschoolWorkNofityId(schoolworkNotifyId, studentName).Tables[0].DefaultView;
        }
        this.AspNetPager2.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize;

        this.ddlstSchoolWork.DataSource = pds;
        this.ddlstSchoolWork.DataBind();

        if (pds.Count > 0)
        {
            this.ddlstSchoolWork.ShowFooter = false;
        }
    }

    //提交作业分页
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataBindSchoolWorks(Master.schoolworkNotifyId);
    }

    //获取附件
    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false,string.Empty);
    }
    protected void btnSubmitSearch_Click(object sender, EventArgs e)
    {

    }

}