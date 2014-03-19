using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;

public partial class Teacher_VSWorkNOnlineWorkUnSubed : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment3");
        //未提交列表

        if (!CommonUtility.SafeCheckByParams<string>(Request["schoolworkNotifyId"], ref Master.schoolworkNotifyId))
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        DataBindNoSchoolWorks(Master.schoolworkNotifyId);
    }

    //获取附件
    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false, string.Empty);
    }

    //绑定未提交作业列表
    public void DataBindNoSchoolWorks(int schoolworkNotifyId)
    {
        DalOperationAboutSchoolWorks dalOperationAboutschoolwork = new DalOperationAboutSchoolWorks();
        string studentName = txtNameSearch.Text;
        DataView dv = dalOperationAboutschoolwork.FindNoSchoolWorksByschoolWorkNofityId(schoolworkNotifyId, studentName).Tables[0].DefaultView;

        this.AspNetPager1.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        this.dlSchoolWork.DataSource = pds;
        this.dlSchoolWork.DataBind();

        if (pds.Count > 0) {
            this.dlSchoolWork.ShowFooter = false;
        }
    }

    //提交作业分页
    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        DataBindNoSchoolWorks(Master.schoolworkNotifyId);
    }

    protected void btnSubmitSearch_Click(object sender, EventArgs e)
    {

    }


}