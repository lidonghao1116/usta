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

public partial class Teacher_AddNotifyInfo : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.courseNotify;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
    }
    protected void btnNotifyAdd_Click(object sender, EventArgs e)
    {
        if (txtNotifyTitle.Text.Trim().Length != 0)
        {

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            CoursesNotifyInfo CoursesNotifyInfo = new CoursesNotifyInfo
            {
                courseNo = Request["courseNo"],
                termTag = Request["termtag"],
                classID = Server.UrlDecode(Request["classID"]),
                courseNotifyInfoTitle = CommonUtility.JavascriptStringFilter(txtNotifyTitle.Text),
                courseNotifyInfoContent = Textarea1.Value,
                isTop = 0,
                notifyType = 0,//通知与作业已分开，这里为了不为空，默认存值为0
                publishUserNo = UserCookiesInfo.userName,
                updateTime = DateTime.Now,
                attachmentIds = hidAttachmentId.Value
            };
            DalOperationAboutCourseNotifyInfo DalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
            DalOperationAboutCourseNotifyInfo.InsertCourseNotifyInfo(CoursesNotifyInfo);
            Javascript.RefreshParentWindow("添加成功!", "CInfoCourseNotify.aspx?courseNo=" + CoursesNotifyInfo.courseNo + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"].ToString().Trim())) + "&termtag=" + Request["termtag"].Trim(), Page);
        }
        else
        {
            Javascript.Alert("请提交标题！", Page);
        }
    }
}
