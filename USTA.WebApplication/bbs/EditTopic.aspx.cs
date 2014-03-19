using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using System.Configuration;
using USTA.PageBase;

public partial class bbs_EditTopic : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.experimentResources;

    public string classID = HttpContext.Current.Request["classID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["classID"]) : string.Empty;
    public string termTag = HttpContext.Current.Request["termTag"] != null ? HttpContext.Current.Request["termTag"] : string.Empty; 
    public string course = HttpContext.Current.Request["course"] != null ? HttpContext.Current.Request["course"] : string.Empty;
    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!hasControls(course))
        {
            CommonUtility.RedirectLoginUrl();
            return;
        }
        if (!IsPostBack)
        {
            int topicId=0;
            if (CommonUtility.SafeCheckByParams<string>(Request["topicId"], ref topicId))
            {
                DalOperationAboutBbs dal = new DalOperationAboutBbs();
                DataSet ds = dal.GetTopicAndPostsByTopicId(topicId);
                txtTitle.Text = ds.Tables["1"].Rows[0]["topicTitle"].ToString().Trim();
                Textarea1.Value = ds.Tables["1"].Rows[0]["topicContent"].ToString().Trim();
                hidAttachmentId.Value = ds.Tables["1"].Rows[0]["attachmentIds"].ToString().Trim();
                if ( ds.Tables["1"].Rows[0]["attachmentIds"].ToString().Length > 0)
                {
                    DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                    ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(ds.Tables["1"].Rows[0]["attachmentIds"].ToString(), ref iframeCount, true,string.Empty);
                }
            }
            else
            {
                Javascript.GoHistory(-1, Page);
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DalOperationPatch dal = new DalOperationPatch();
        dal.UpdateBBSTopic(int.Parse(Request["topicId"]), txtTitle.Text, Textarea1.Value, hidAttachmentId.Value);
        Response.Write("<script>window.top.location.reload();</script>");
    }
    public bool hasControls(string course)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        return UserCookiesInfo.userType == (int)UserType.AdminRole || DalOperationAboutCourses.IsTeacherAtCourse(UserCookiesInfo.userNo, course, classID, termTag);
    }
}
