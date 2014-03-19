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
using System.Configuration;

public partial class Teacher_VSCheckExperiments : System.Web.UI.Page
{
    public int attachmentIds = 0;
    public int fileFolderType = (int)FileFolderType.remarkExperimentsAndSchoolWorks;
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment4");
        ///批阅实验-评语与打分
        int experimentId = 0;
        if (!CommonUtility.SafeCheckByParams<string>(Request["experimentId"], ref experimentId))
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        if (!IsPostBack)
        {
            DataBindExperment(experimentId);
        }
    }
    //绑定实验信息
    public void DataBindExperment(int experimentId)
    {
        DalOperationAboutExperiment doae = new DalOperationAboutExperiment();
        DataTable dt = doae.FindExperimentsByexperimentIdForCheck(experimentId).Tables[0];
        lblNo.Text = dt.Rows[0]["studentNo"].ToString().Trim();
        lblName.Text = dt.Rows[0]["studentName"].ToString().Trim();
        txtRemark.Text = dt.Rows[0]["remark"].ToString().Trim();
        txtScore.Text = dt.Rows[0]["score"].ToString().Trim();

        if (int.TryParse(dt.Rows[0]["returnAttachmentId"].ToString().Trim(), out attachmentIds))
        {
            attachmentIds = int.Parse(dt.Rows[0]["returnAttachmentId"].ToString().Trim());
        }
    }
    //提交批阅信息--评语和实验分数
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        float score = 0;
        if (CommonUtility.SafeCheckByParams<string>(txtScore.Text.Trim(), ref score))
        {
            if (!CommonUtility.CheckScoreScope(ref score))
            {
                Javascript.Alert("请输入0-100之间的分数", Page);
                return;
            }
            int attachmentId = 0;
            if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
            {
                attachmentId = int.Parse(hidAttachmentId.Value);
            }
            DalOperationAboutExperiment doae = new DalOperationAboutExperiment();
            doae.CheckExperimentByexperimentId(int.Parse(Request["experimentId"].Trim()), CommonUtility.JavascriptStringFilter(txtRemark.Text.Trim()), score.ToString(), attachmentId);

            Response.Redirect("VExpRExperimentSubed.aspx?page="+pageIndex+"&experimentResourceId=" + Request["experimentResourceId"] + "&courseNo=" + Request["courseNo"] + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"])) + "&termtag=" + Request["termtag"]);
        }
        else
        {
            Javascript.Alert("请输入数字！", Page);
        }
    }

    public string GetURL(string aids)
    {
        int a = 0;
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref a, false,string.Empty);
    }
}