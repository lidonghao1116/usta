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

public partial class bbs_BBSTopicSearch : System.Web.UI.Page
{
    public string forumId = HttpContext.Current.Request["forumId"] != null ? HttpContext.Current.Request["forumId"] : string.Empty;
    public string classID = HttpContext.Current.Request["classID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["classID"]) : string.Empty;
    public string termTag = HttpContext.Current.Request["termTag"] != null ? HttpContext.Current.Request["termTag"] : string.Empty;
    public string tag = HttpContext.Current.Request["tag"] != null ? HttpContext.Current.Request["tag"] : string.Empty;
    public string tagName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["tag"] != null && Request["forumId"] != null)
        {
            string tag = Request["tag"];
            if (tag == "1")
            {
                DalOperationAboutCourses dal1 = new DalOperationAboutCourses();
                tagName = dal1.GetCoursesByNo(Request["forumId"], Server.UrlDecode(Request["classID"]), Request["termtag"]).courseName;
            }
            else
            {
                DalOperationAboutBbsManage dal3 = new DalOperationAboutBbsManage();
                DalOperationAboutBbs dal2 = new DalOperationAboutBbs();
                tagName = dal3.GetForumById(Request["forumId"]).forumTitle;
            }
        }
        else
        {
            Javascript.GoHistory(-1, Page);
        }
    }
    /// <summary>
    /// 搜索话题
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public DataView dv;
    protected void btnsearch_Click(object sender, EventArgs e)
    {

        dlsttopicresult.DataSource = null;
        dlsttopicresult.DataBind();
        //dlstpostresult.DataSource = null;
        //dlstpostresult.DataBind();
        DalOperationAboutBbs DalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet ds = new DataSet();
        
            //dlsttopicresult.Visible = true;

            //dlstpostresult.Visible = false;

            DataResultTopic();

       
    }

    protected void DataResultTopic()
    {
        DalOperationAboutBbs DalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet ds = DalOperationAboutBbs.SearchTopic(txtSearchString.Text, forumId+classID+termTag);
        dv = ds.Tables[0].DefaultView;


        dlsttopicresult.DataSource = dv;
        dlsttopicresult.DataBind();
    }

    protected void DataResultPost()
    {
        DalOperationAboutBbs DalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet ds = DalOperationAboutBbs.SearchPost(txtSearchString.Text, forumId);

        dv = ds.Tables[0].DefaultView;



        //dlstpostresult.DataSource = dv;
        //dlstpostresult.DataBind();
    }
}