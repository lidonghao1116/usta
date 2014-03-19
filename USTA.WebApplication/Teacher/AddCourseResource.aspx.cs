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

public partial class Teacher_AddCourseResource : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.courseResources;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Length > 0)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            CourseResources CourseResources = new CourseResources
            {
                classID = Server.UrlDecode(Request["classID"]),
                termTag=Request["termtag"],
                courseNo = Request["courseNo"],
                updateTime = DateTime.Now,
                courseResourceTitle = CommonUtility.JavascriptStringFilter(txtTitle.Text)
            };

            if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
            {
                CourseResources.attachmentIds = hidAttachmentId.Value;
            }

            DalOperationAboutCourseResources DalOperationAboutCourseResources = new DalOperationAboutCourseResources();
            DalOperationAboutCourseResources.InsertCourseResources(CourseResources);
            Javascript.RefreshParentWindow("添加成功!", "CInfoCourseResource.aspx?courseNo=" + CourseResources.courseNo + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"].ToString().Trim())) + "&termtag=" + Request["termtag"].ToString().Trim(), Page);
        }
        else
        {
            Javascript.Alert("标题不能为空!", Page);
        }
    }
}
