using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using USTA.Model;
using System.Configuration;

public partial class bbs_BBSViewTopic : System.Web.UI.Page
{
    public int topicId = 0;
    public bool hascontrol = false;
    public string topicName = string.Empty;
    public string tag = HttpContext.Current.Request["tag"] != null ? HttpContext.Current.Request["tag"] : string.Empty;
    public string tagName = string.Empty;
    public string forumId = HttpContext.Current.Request["forumId"] != null ? HttpContext.Current.Request["forumId"] : string.Empty;
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;

    public int floor = 1;
    #region 提交回复所需文件夹变量
    public int fileFolderType = (int)FileFolderType.bbs;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (tag)
        {
            case "1": Master.ShowLiControl(this.Page, "liFragment1");
                break;
            case "2": Master.ShowLiControl(this.Page, "liFragment2");
                break;
            case "3": Master.ShowLiControl(this.Page, "liFragment3");
                break;
            default: break;
        }

        PageDataBinds();
    }

    protected void PageDataBinds()
    {
        if (CommonUtility.SafeCheckByParams<string>(Request["topicId"], ref topicId))
        {
            DalOperationAboutBbs dal = new DalOperationAboutBbs();
            dal.AddTopicHits(topicId.ToString());
            DataSet ds = dal.GetTopicAndPostsByTopicId(topicId);

            if (ds.Tables["1"].Rows.Count > 0 && ds.Tables["1"].Rows.Count > 0)
            {
                courseNo = ds.Tables["1"].Rows[0]["courseNo"].ToString();
                topicName = ds.Tables["1"].Rows[0]["topicTitle"].ToString();
            }

            if (Request["tag"] != null)
            {
                if (tag == "1")
                {
                    courseNo = Request["courseNo"];
                    DalOperationAboutCourses dal1 = new DalOperationAboutCourses();
                    tagName = dal1.GetCoursesByNo(Request["courseNo"],Server.UrlDecode(Request["classID"]),Request["termtag"]).courseName;
                }
                else
                {
                    DalOperationAboutBbsManage dal3 = new DalOperationAboutBbsManage();

                    BbsForum f = dal3.GetForumById(courseNo);
                    tagName = f.forumTitle;
                }
            }

            hascontrol = this.hasControl(courseNo);

            if (Request["del"] == "true")
            {
                int postId = 0;
                if (hascontrol && CommonUtility.SafeCheckByParams<string>(Request["postId"], ref postId))
                {
                    delpost(postId);
                    Javascript.AlertAndRedirect("删除成功！", "BBSViewTopic.aspx?tag=" + tag + "&topicId=" + Request["topicId"], Page);
                    return;
                }
                int topicd = 0;
                if (hascontrol && CommonUtility.SafeCheckByParams<string>(Request["topicId"], ref topicd) && Request["postId"] == null)
                {

                    //deltopic(topicd);
                    //Javascript.AlertAndRedirect("删除成功！", "BBSTopicList.aspx?forumId=" + courseNo +"&classID="+Server.UrlDecode(Request["classID"])+"termtag="+Request["termtag"]+ "&tag=" + tag, Page);
                    // Javascript.JavaScriptLocationHref("BBSTopicList.aspx?del=true&forumId=" + courseNo + "&classID=" + Server.UrlDecode(Request["classID"]) + "&termtag="+Request["termtag"]+"&topicId=166&tag=1", Page);
                    return;
                }
                else
                {
                    Javascript.Alert("不能删除！", Page);
                    return;
                }
            }

            dlsttopic.DataSource = ds.Tables["1"];
            dlsttopic.DataBind();
            dlstposts.DataSource = ds.Tables["0"];
            dlstposts.DataBind();
        }
    }

    public bool hasControl(string course)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        return UserCookiesInfo.userType == (int)UserType.AdminRole || DalOperationAboutCourses.IsTeacherAtCourse(UserCookiesInfo.userNo, course,classID,termtag);
    }
    public string GetURL(string aids)
    {
        int a = 0;
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref a, false,string.Empty);
    }
    private void deltopic(int topicId)
    {
        DalOperationAboutBbsManage DalOperationAboutBbsManage = new DalOperationAboutBbsManage();
        DalOperationAboutBbsManage.DeleteTopicByTopicId(topicId);
    }
    private void delpost(int postId)
    {
        DalOperationAboutBbsManage DalOperationAboutBbsManage = new DalOperationAboutBbsManage();
        DalOperationAboutBbsManage.DeletePostByPostId(postId);
    }
    #region 提交回复所需功能



    protected void btnCommit_Click(object sender, EventArgs e)
    {
        if (Textarea1.Value.Length == 0)
        {
            Javascript.GoHistory(-1, "请填写回复内容！", Page);
            return;
        }
        int lastPostId = 0;

        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        BbsPosts post = new BbsPosts
        {
            topicId = topicId,
            postContent = CommonUtility.JavascriptStringFilterAll(Textarea1.Value),
            postUserName = (UserCookiesInfo.userType != 3) ? UserCookiesInfo.userName : UserCookiesInfo.userNo + "  " + UserCookiesInfo.userName,
            updateTime = DateTime.Now,
            attachmentIds = hidAttachmentId.Value,
            postUserNo = UserCookiesInfo.userNo,
            postUserType = UserCookiesInfo.userType,
            courseNo = forumId
        };
        lastPostId = dalOperationAboutBbs.AddPostByTopicId(post);
        PageDataBinds();
        hidAttachmentId.Value = string.Empty;
        Textarea1.Value = string.Empty;
        Javascript.JavaScriptLocationHref("#post" + lastPostId.ToString(), Page);
    }

    #endregion



    /// <summary>
    /// 获得头像
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="userType"></param>
    /// <returns></returns>
    public string GetAvatar(string userNo, int userType)
    {
        DalOperationPatch dal = new DalOperationPatch();
        return dal.GetAvatar(userNo, userType);
    }

    public bool IsMyself(string userNo, int userType)
    {
        UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

        return userNo.Trim() == user.userNo.Trim() && userType == user.userType;
    }

}