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

public partial class Teacher_EditNotifyInfo : CheckUserWithCommonPageBase
{
    public CoursesNotifyInfo CoursesNotifyInfo;
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    public int fileFolderType = (int)FileFolderType.courseNotify;

    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    int courseNotifyId = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!CommonUtility.SafeCheckByParams<string>(Request["courseNotifyId"], ref courseNotifyId))
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        if (!IsPostBack)
        {
            DalOperationAboutCourseNotifyInfo DalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
            CoursesNotifyInfo = DalOperationAboutCourseNotifyInfo.GetCourseNotifyInfoById(courseNotifyId);
            txtNotifyTitle.Text = CoursesNotifyInfo.courseNotifyInfoTitle;
            Textarea1.Value = CoursesNotifyInfo.courseNotifyInfoContent;

            hidAttachmentId.Value = CoursesNotifyInfo.attachmentIds;
            if (CoursesNotifyInfo.attachmentIds.Length > 0)
            {
                DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(CoursesNotifyInfo.attachmentIds, ref iframeCount, true, string.Empty);
            }

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (txtNotifyTitle.Text.Trim().Length != 0)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            CoursesNotifyInfo CoursesNotifyInfo1 = new CoursesNotifyInfo
            {
                courseNotifyInfoId = courseNotifyId,
                courseNo = Request["courseNo"].ToString().Trim(),
                classID = Server.UrlDecode(Request["classId"].Trim()),
                termTag = Request["termtag"].Trim(),
                courseNotifyInfoTitle = CommonUtility.JavascriptStringFilter(txtNotifyTitle.Text),
                courseNotifyInfoContent = Textarea1.Value,
                isTop = 0,
                notifyType = 0,//默认是通知类型
                publishUserNo = UserCookiesInfo.userName,
                updateTime = DateTime.Now,
                attachmentIds = hidAttachmentId.Value
            };
            DalOperationAboutCourseNotifyInfo DalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
            DalOperationAboutCourseNotifyInfo.UpdateCourseNotifyInfo(CoursesNotifyInfo1);
            Javascript.RefreshParentWindow("修改成功!", "CInfoCourseNotify.aspx?page="+pageIndex+"&courseNo=" + CoursesNotifyInfo1.courseNo+"&classID="+Server.UrlEncode(Server.UrlDecode(Request["classID"])), Page);
        }
        else { Javascript.Alert("标题不能为空！", Page); }
    }
}