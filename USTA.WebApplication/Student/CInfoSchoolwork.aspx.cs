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

public partial class Student_CInfoSchoolwork : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";

        if (!IsPostBack)
        {
            Javascript.ExcuteJavascriptCode("window.onbeforeunload= function(){if($.trim($('#hidOriginalAttachmentId').val())!=$.trim($('#hidAttachmentId').val())){return '温馨提示：作业数据可能未保存哟~（此为提示，并不代表您真正未保存作业数据，请在完成附件上传后点击提交，确保顺利提交作业）';}}", Page);
            HtmlControl liControl = (HtmlControl)Master.Master.FindControl("ContentPlaceHolder1").FindControl("liFragment9");
            liControl.Visible = true;
            liControl.Attributes.Add("class", "ui-tabs-selected");

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            int schoolworkNotifyId = 0;
            if (CommonUtility.SafeCheckByParams<String>(Request["schoolworkNotifyId"], ref schoolworkNotifyId))
            {

                DalOperationAboutCourses dalOperationAboutCourses = new DalOperationAboutCourses();
                DataSet dsSchoolworkNotify = dalOperationAboutCourses.GetSchoolworkNotifybyId(schoolworkNotifyId);
                dlstSchoolWork.DataSource = dsSchoolworkNotify.Tables[0];
                dlstSchoolWork.DataBind();

                bool isOnline = bool.Parse(dsSchoolworkNotify.Tables[0].Rows[0]["isOnline"].ToString());

                DateTime deadline = Convert.ToDateTime(dsSchoolworkNotify.Tables[0].Rows[0]["deadline"].ToString());

                DalOperationAboutSchoolWorks dalOperationAboutSchoolworks = new DalOperationAboutSchoolWorks();
                int SchoolworkId = 0;
                bool checkOR = false;
                if (isOnline)
                {
                    if (checkOR)
                    {
                        isSchoolcommitspan.InnerHtml += "已经批阅！";
                        btnSchoolworkCommit.Visible = false;
                        return;
                    }
                    else if (deadline.CompareTo(DateTime.Now) <= 0)
                    {
                        isSchoolcommitspan.InnerHtml += "已过截止日期！";
                        btnSchoolworkCommit.Visible = false;
                        return;
                    }

                    if (dalOperationAboutSchoolworks.SchoolworkIsCommit(UserCookiesInfo.userNo, schoolworkNotifyId, ref SchoolworkId, ref checkOR))
                    {

                        DalOperationAboutSchoolWorks dalOperationAboutSchoolWorks1 = new DalOperationAboutSchoolWorks();
                        string attachementids = dalOperationAboutSchoolWorks1.GetSchoolWorkById(SchoolworkId).attachmentId.ToString();


                        hidAttachmentId.Value = attachementids;

                        hidOriginalAttachmentId.Value = attachementids;

                        isSchoolcommitspan.InnerHtml = "已提交作业文件列表<br /><br />" + this.GetSchoolAttachmentsURL(attachementids);
                    }

                    isSchoolcommitspan.InnerHtml += "<input id=\"Button3\" type=\"button\" value=\"点击添加一个附件，可添加多个\" onclick=\"addIframe(" + (int)FileFolderType.schoolWorks + ");\" />&nbsp;&nbsp;<b>上传文件大小不超过</b>" + ConfigurationManager.AppSettings["uploadFileLimit"] + "<div id=\"iframes\"></div>";
                }
                else
                {
                    isSchoolcommitspan.InnerHtml += "此为书面作业，不需要在线提交！";
                    btnSchoolworkCommit.Visible = false;
                }
            }
        }
    }

    protected void btnSchoolworkCommit_Click(object sender, EventArgs e)
    {
        int SchoolNotifyId = int.Parse(Request["schoolworkNotifyId"]);
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        SchoolWorks schoolwork = new SchoolWorks
        {
            updateTime = DateTime.Now,
            schoolWorkNofityId = SchoolNotifyId,
            studentNo = UserCookiesInfo.userNo,
            isCheck = false,
            isExcellent = false,
            remark = "",
            checkTime = DateTime.Now,
            excellentTime = DateTime.Now,

        };
        if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
        {
            schoolwork.attachmentId = hidAttachmentId.Value;
        }
        else
        {
            Javascript.GoHistory(-1, "请上传附件！", Page);
            return;
        }
        DalOperationAboutSchoolWorks dalOperationAboutSchoolworks = new DalOperationAboutSchoolWorks();
        dalOperationAboutSchoolworks.SubmitSchoolWork(schoolwork);

        Javascript.AlertAndRedirect("提交成功！", "CInfoSchoolwork.aspx?schoolworkNotifyId=" + schoolwork.schoolWorkNofityId + "&courseNo=" + Request["courseNo"] + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"])) + "&termtag=" + Request["termtag"], Page);
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