using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using USTA.Model;
using System.Configuration;
using USTA.PageBase;

public partial class bbs_AddBigTopic : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.bbs;
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCommit_Click(object sender, EventArgs e)
    {
         
        if (txtTilte.Text.Length == 0)
        {
            Javascript.GoHistory(-1, "请填写标题！", Page);
            return;
        }

        if (Textarea1.Value.Length == 0)
        {
            Javascript.GoHistory(-1, "请填写内容！", Page);
            return;
        }

        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        BbsTopics topic = new BbsTopics
        {
            courseNo = Request["forumId"],
            topicTitle = CommonUtility.JavascriptStringFilterAll(txtTilte.Text),
            topicContent = CommonUtility.JavascriptStringFilterAll(Textarea1.Value),
            topicUserName = (UserCookiesInfo.userType != 3) ? UserCookiesInfo.userName : UserCookiesInfo.userNo + "  " + UserCookiesInfo.userName,
            updateTime = DateTime.Now,
            topicUserNo = UserCookiesInfo.userNo,
            topicUserType = UserCookiesInfo.userType,
            isbigTop=1
        };
        if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
        {
            topic.attachmentIds = hidAttachmentId.Value;
        }
        if (hasControls(Request["forumId"]))
        {
            dalOperationAboutBbs.AddTopicByForumId(topic);//添加新话题
        }
        else
        {
            Javascript.GoHistory(-1, "您没有权限！", Page);
        }
        Javascript.RefreshParentWindow("BBSTopicList.aspx?tagName=" + Request["tagName"] + "&tag=2&forumId=" + Request["forumId"], Page);
    }
    public bool hasControls(string course)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        return UserCookiesInfo.userType == (int)UserType.AdminRole || DalOperationAboutCourses.IsTeacherAtCourse(UserCookiesInfo.userNo, course,classID,termtag);
    }
}
