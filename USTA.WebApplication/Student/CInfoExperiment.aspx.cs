using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Configuration;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;

public partial class Student_CousrInfo_9_Experiment : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";

        if (!IsPostBack)
        {
            Javascript.ExcuteJavascriptCode("window.onbeforeunload= function(){if($.trim($('#hidOriginalAttachmentId').val())!=$.trim($('#hidAttachmentId').val())){return '温馨提示：实验数据可能未保存哟~（此为提示，并不代表您真正未保存实验数据，请在完成附件上传后点击提交，确保顺利提交实验）';}}", Page);
            HtmlControl liControl = (HtmlControl)Master.Master.FindControl("ContentPlaceHolder1").FindControl("liFragment9");
            liControl.Visible = true;
            liControl.Attributes.Add("class", "ui-tabs-selected");
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            //设置实验资源ID
            int experimentResourceId = -1;

            if (CommonUtility.SafeCheckByParams<string>(Request["experimentResourceId"], ref experimentResourceId))
            {

                DalOperationAboutExperimentResources DalOperationAboutExperimentResources = new DalOperationAboutExperimentResources();
                DataSet dsExperimentResource = DalOperationAboutExperimentResources.GetExperimentResourcesById(experimentResourceId);
                dlstExperiment.DataSource = dsExperimentResource.Tables[0];
                dlstExperiment.DataBind();
                DateTime deadLine = Convert.ToDateTime(dsExperimentResource.Tables[0].Rows[0]["deadLine"].ToString());

                DalOperationAboutExperiment DalOperationAboutExperiment = new DalOperationAboutExperiment();
                int experimentId = 0;
                bool checkOR = false;

                if (checkOR)
                {
                    experimentspan.InnerHtml += "已经批阅！";
                    btnExperiment.Visible = false;
                    return;
                }
                else if (deadLine.CompareTo(DateTime.Now) <= 0)
                {
                    experimentspan.InnerHtml += "已过截止日期！";
                    btnExperiment.Visible = false;
                    return;
                }


                if (DalOperationAboutExperiment.ExperimentIsCommit(UserCookiesInfo.userNo, experimentResourceId, ref experimentId, ref checkOR))
                {

                    string attachmentids = DalOperationAboutExperiment.GetExperimentById(experimentId).attachmentId.ToString();

                    hidAttachmentId.Value = attachmentids;
                    hidOriginalAttachmentId.Value = attachmentids;

                    experimentspan.InnerHtml = "已提交实验文件列表<br/><br />" + GetSchoolAttachmentsURL(attachmentids);
                }
                experimentspan.InnerHtml += "<input id=\"Button3\" type=\"button\" value=\"点击添加一个附件，可添加多个\" onclick=\"addIframe(" + (int)FileFolderType.experiments + ");\" />&nbsp;&nbsp;<b>上传文件大小不超过</b>" + ConfigurationManager.AppSettings["uploadFileLimit"] + "<div id=\"iframes\"></div>";
            }
        }
    }

    protected void btnExperiment_Click(object sender, EventArgs e)
    {
        int experimentResourceId = int.Parse(Request["experimentResourceId"]);
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutExperiment DalOperationAboutExperiment = new DalOperationAboutExperiment();

        Experiments experiment = new Experiments
        {
            studentNo = UserCookiesInfo.userNo,
            experimentResourceId = experimentResourceId,
            updateTime = DateTime.Now,
        };
        if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
        {
            experiment.attachmentId = hidAttachmentId.Value;
        }
        else
        {
            Javascript.Alert("请上传附件！", Page);
            return;
        }

        DalOperationAboutExperiment.SubmitExperiment(experiment);

        Javascript.AlertAndRedirect("提交成功！", "CInfoExperiment.aspx?experimentResourceId=" + experiment.experimentResourceId + "&courseNo=" + Request["courseNo"] + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"])) + "&termtag=" + Request["termtag"], Page);

    }

    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false,string.Empty);
    }

    public string GetSchoolAttachmentsURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, true,string.Empty);
    }
}