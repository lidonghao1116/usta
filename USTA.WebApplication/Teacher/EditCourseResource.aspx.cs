using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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

public partial class Teacher_EditCourseResource : CheckUserWithCommonPageBase
{
    int courseResourceId = -1;

    public int fileFolderType = (int)FileFolderType.courseResources;

    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        if (!IsPostBack)
        {
            if (CommonUtility.SafeCheckByParams<string>(Request["courseResourceId"], ref courseResourceId))
            {
                DalOperationAboutCourseResources DalOperationAboutCourseResources = new DalOperationAboutCourseResources();
                CourseResources CourseResources = DalOperationAboutCourseResources.GetCourseResourcesbyId(courseResourceId);

                txtTitle.Text = CourseResources.courseResourceTitle;

                hidAttachmentId.Value = CourseResources.attachmentIds;

                if (CourseResources.attachmentIds.Length > 0)
                {
                    DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                    ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(CourseResources.attachmentIds, ref iframeCount, true,string.Empty);
                }
            }
            else
            {
                Javascript.GoHistory(-1, Page);
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Length != 0)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

            CourseResources CourseResources = new CourseResources
            {
                courseResourceId = int.Parse(Request["courseResourceId"]),
                courseNo = Request["courseNo"],
                attachmentIds = hidAttachmentId.Value,
                updateTime = DateTime.Now,
                courseResourceTitle = CommonUtility.JavascriptStringFilter(txtTitle.Text)
            };

            DalOperationAboutCourseResources DalOperationAboutCourseResources = new DalOperationAboutCourseResources();

            DalOperationAboutCourseResources.UpdateCourseResources(CourseResources);
            Javascript.RefreshParentWindow("修改成功!", "CInfoCourseResource.aspx?courseNo=" + CourseResources.courseNo+"&classID="+Server.UrlEncode(Server.UrlDecode(Request["classID"])), Page);
        }
        else
        {
            Javascript.Alert("标题不能为空！", Page);
        }
    }
}