using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Model;
using USTA.Common;

public partial class Teacher_VExpRExperimentUnSubed : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment3");
        if (CommonUtility.SafeCheckByParams<string>(Request["experimentResourceId"], ref Master.experimentResourceId))
        {
            //未提交列表
            DataBindNoExperments(Master.experimentResourceId);
        }
           
        else
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
    }

    //绑定未提交实验列表
    public void DataBindNoExperments(int experimentResourceId)
    {
        DalOperationAboutExperiment dalOperationAboutNoExperiment = new DalOperationAboutExperiment();
        string studentName = txtNameSearch.Text;
        DataView dv = dalOperationAboutNoExperiment.GetNoExperimentsByResourcesId(experimentResourceId,studentName).Tables[0].DefaultView;

        this.AspNetPager1.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        this.dlNoExp.DataSource = pds;
        this.dlNoExp.DataBind();

        if (pds.Count > 0)
        {
            this.dlNoExp.ShowFooter = false;
        }
    }
    //未提交实验分页
    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        DataBindNoExperments(Master.experimentResourceId);
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