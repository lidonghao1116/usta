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
using System.Collections;

public partial class bbs_BBSTopicList : System.Web.UI.Page
{
    public string forumId = HttpContext.Current.Request["forumId"] != null ? HttpContext.Current.Request["forumId"] : string.Empty;
    public string classID = HttpContext.Current.Request["classID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["classID"]) : string.Empty;
    public string termTag = HttpContext.Current.Request["termTag"] != null ? HttpContext.Current.Request["termTag"] : string.Empty;
    public string tag = HttpContext.Current.Request["tag"] != null ? HttpContext.Current.Request["tag"] : string.Empty;
    public string tagName = string.Empty;
    //= HttpUtility.UrlDecode(HttpContext.Current.Request["tagName"]);
    public string forumName = string.Empty;
    public bool hasControl = false;
    public Hashtable ht0 = new Hashtable();
    public Hashtable ht1 = new Hashtable();
    public Hashtable ht2 = new Hashtable();
    public Hashtable ht3 = new Hashtable();

    #region 用于提交话题，设定上传文件夹类型
    public int fileFolderType = (int)FileFolderType.bbs;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        switch (Request["tag"])
        {
            case "1": Master.ShowLiControl(this.Page, "liFragment1");
                if (Request["forumId"] == null || Request["classID"] == null || Request["termtag"] == null)
                {
                    Javascript.Alert("参数错误", Page);
                    Javascript.GoHistory(-1, Page);
                    return;
                }
                break;
            case "2": Master.ShowLiControl(this.Page, "liFragment2");
                break;
            case "3": Master.ShowLiControl(this.Page, "liFragment3");
                break;
            default: break;
        }
        //控制Tab的显示
        hasControl = hasControls(forumId);
        if (!IsPostBack)
        {
            ///删除话题
            int topicId = 0;
            if (hasControl && CommonUtility.SafeCheckByParams<string>(Request["topicId"], ref topicId) && Request["del"] == "true")
            {

                deltopic(topicId);
                Javascript.Alert("删除成功！", Page);
                // Javascript.AlertAndRedirect("删除成功！", "BBSTopicList.aspx?forumId=" + Request["forumId"] + "&tag=" + tag, Page);

            }

            /// 设置置顶
            if (hasControl && CommonUtility.SafeCheckByParams<string>(Request["toTopId"], ref topicId))
            {
                setTop(topicId);
            }


            /// 取消置顶
            int canceltopId = 0;
            if (hasControl && CommonUtility.SafeCheckByParams<string>(Request["cancelTopId"], ref canceltopId))
            {
                cancelTop(canceltopId);
            }
        }
        if (Request["tag"] != null && Request["forumId"] != null)
        {
            string tag = Request["tag"];
            if (tag == "1")
            {

                DalOperationAboutCourses dal1 = new DalOperationAboutCourses();
                forumId = Request["forumId"].Trim() + Server.UrlDecode(Request["classID"].Trim()) + Request["termtag"].Trim();
                tagName = dal1.GetCoursesByNo(Request["forumId"].Trim(), Server.UrlDecode(Request["classID"].Trim()), Request["termtag"].Trim()).courseName;
            }
            else
            {
                DalOperationAboutBbsManage dal3 = new DalOperationAboutBbsManage();
                DalOperationAboutBbs dal2 = new DalOperationAboutBbs();
                tagName = dal3.GetForumById(Request["forumId"]).forumTitle;
            }
        }
        DataListBind();
    }
    /// <summary>
    /// 设置置顶
    /// </summary>
    /// <param name="topicId"></param>
    private void setTop(int topicId)
    {
        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        dalOperationAboutBbs.SetTopicOnTop(topicId);
    }
    private void deltopic(int topicId)
    {
        DalOperationAboutBbsManage DalOperationAboutBbsManage = new DalOperationAboutBbsManage();
        DalOperationAboutBbsManage.DeleteTopicByTopicId(topicId);
    }
    /// <summary>
    /// 取消置顶
    /// </summary>
    /// <param name="topicId"></param>
    private void cancelTop(int topicId)
    {
        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        dalOperationAboutBbs.CancelTopicOnTop(topicId);
    }

    /// <summary>
    /// 判断是否有此版面的管理权限
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    public bool hasControls(string course)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        return UserCookiesInfo.userType == (int)UserType.AdminRole || DalOperationAboutCourses.IsTeacherAtCourse(UserCookiesInfo.userNo, course, classID, termTag);
    }

    protected void DataListBind()
    {
        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet ds = dalOperationAboutBbs.GetAllTopicsByForumId(forumId);
        DataSet ds1 = dalOperationAboutBbs.GetTopicsTopByForumId(forumId);
        dlstTopTopic.DataSource = ds1.Tables[0];
        dlstTopTopic.DataBind();


        //填充HASHTABLE
        DataSet ds2 = dalOperationAboutBbs.GetLastPost();

        DataTable dt0 = ds2.Tables["0"];

        DataTable dt1 = ds2.Tables["1"];

        foreach (DataRow dr0 in dt0.Rows)
        {
            ht0.Add(dr0["topicId"].ToString(), dr0["postsCount"].ToString());
        }

        foreach (DataRow dr1 in dt1.Rows)
        {
            ht1.Add(dr1["topicId"].ToString(), dr1["postUserName"].ToString());
            ht2.Add(dr1["topicId"].ToString(), dr1["updateTime"].ToString());
        }

        DalOperationPatch dal = new DalOperationPatch();
        DataSet dsNew = dal.GetLatestTopic();
        DataSet dsTopicList = dal.GetLatestTopicAndPostsUpdateTime();
        foreach (DataRow dr2 in dsTopicList.Tables[0].Rows)
        {
            ht3.Add(dr2["topicId"].ToString().Trim(), dr2["updateTime"].ToString().Trim());
        }

        dlsttopics.DataSource = ds.Tables[0];
        dlsttopics.DataBind();


    }

    #region 提交话题标签功能代码
    // /// <summary>
    ///// 提交话题标签功能代码
    // /// </summary>
    // /// <param name="sender"></param>
    // /// <param name="e"></param>

    protected void btnCommit_Click(object sender, EventArgs e)
    {
        if (txtTilte.Text.Length == 0)
        {
            Javascript.GoHistory(-1, "请填写标题！", Page);
            return;
        }

        if (Textarea1.Value.Length == 0)
        {
            Javascript.GoHistory(-1, "请填写回复内容！", Page);
            return;
        }
        string tag = Request["tag"];
        if (tag == "1")
        {
            forumId = Request["forumId"].Trim() + Server.UrlDecode(Request["classID"].Trim()) + Request["termtag"].Trim();
        }
        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        BbsTopics topic = new BbsTopics
        {
            courseNo = forumId,
            topicTitle = CommonUtility.JavascriptStringFilterAll(txtTilte.Text),
            topicContent = CommonUtility.JavascriptStringFilterAll(Textarea1.Value),
            topicUserName = (UserCookiesInfo.userType != 3) ? UserCookiesInfo.userName : UserCookiesInfo.userNo + "  " + UserCookiesInfo.userName,
            updateTime = DateTime.Now,
            topicUserNo = UserCookiesInfo.userNo,
            topicUserType = UserCookiesInfo.userType
        };
        if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
        {
            topic.attachmentIds = hidAttachmentId.Value;
        }
        dalOperationAboutBbs.AddTopicByForumId(topic);//添加新话题
        Javascript.AlertAndRedirect("添加主题成功！", "BBSTopicList.aspx?forumId=" + Request["forumId"] + "&classID=" + Server.UrlDecode(Request["classID"]) + "&termTag=" + Request["termTag"] + "&tag=" + tag, Page);
    }
    #endregion
}