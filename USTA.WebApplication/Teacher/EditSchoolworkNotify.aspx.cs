using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

public partial class Teacher_EditSchoolworkNotify : CheckUserWithCommonPageBase
{
    SchoolWorkNotify schoolWorkNotify;

    public int fileFolderType = (int)FileFolderType.schoolWorks;

    int schoolworkNotifyId = -1;

    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null||!CommonUtility.SafeCheckByParams<string>(Request["schoolworkNotifyId"], ref schoolworkNotifyId))
        {
            Javascript.GoHistory(-1, Page);
            return;
        }

        if (!IsPostBack)
        {
            DalOperationAboutCourses dalOperationAboutCourses = new DalOperationAboutCourses();
            schoolWorkNotify = dalOperationAboutCourses.GetSchoolworkNotifyById(schoolworkNotifyId);
            this.txtTitle.Text = schoolWorkNotify.schoolWorkNotifyTitle;
            this.Textarea1.Value = schoolWorkNotify.schoolWorkNotifyContent;
            datepicker.Value = schoolWorkNotify.deadline.ToString("yyyy-MM-dd HH:mm:ss");
            if (schoolWorkNotify.isOnline)
                this.ddltOnline.SelectedValue = "true";
            else
                this.ddltOnline.SelectedValue = "false";

            hidAttachmentId.Value = schoolWorkNotify.attachmentIds;
            if (schoolWorkNotify.attachmentIds.Length > 0)
            {
                DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(schoolWorkNotify.attachmentIds, ref iframeCount, true,string.Empty);
            }
        }
    }

    protected void btnEditSchoolworkNotify_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Length != 0 && Textarea1.Value.ToString().Trim().Length != 0)
        {
            DateTime deadline_t = Convert.ToDateTime(datepicker.Value);

            SchoolWorkNotify schoolWorkNotify = new SchoolWorkNotify
            {
                courseNo = Request["courseNo"],
                schoolWorkNotifyTitle = CommonUtility.JavascriptStringFilter(txtTitle.Text),
                schoolWorkNotifyContent = Textarea1.Value,
                deadline = deadline_t,
                updateTime = DateTime.Now,
                attachmentIds = hidAttachmentId.Value,
                schoolWorkNotifyId = schoolworkNotifyId,
                isOnline = Convert.ToBoolean(ddltOnline.SelectedValue)

            };

            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            doac.UpdateSchoolworkNotify(schoolWorkNotify);
            Javascript.RefreshParentWindow("修改成功!", "CInfoSchoolworkNotify.aspx?courseNo=" + schoolWorkNotify.courseNo+"&classID="+Server.UrlEncode(Server.UrlDecode(Request["classID"]))+"&termtag="+Request["termtag"].Trim(), Page);
        }
        else
        {
            Javascript.Alert("标题和内容不能为空！", Page);
        }
    }
}
