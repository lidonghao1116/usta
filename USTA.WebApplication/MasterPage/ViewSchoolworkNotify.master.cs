using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

using USTA.Dal;
using USTA.Model;
using USTA.Common;

public partial class MasterPage_ViewSchoolworkNotify : System.Web.UI.MasterPage
{
    public SchoolWorkNotify schoolworkNotify;
    public bool isOnline;
    //
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["classID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["classID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;
    //保存当前作业编号
    public int schoolworkNotifyId;
    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    private DataSet dsSchoolNotify;
    public DataSet DsSchoolNotify
    {
        get { return dsSchoolNotify; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        if (!CommonUtility.SafeCheckByParams<string>(Request["schoolworkNotifyId"], ref schoolworkNotifyId))
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
 
        //提取作业信息--schoolworkNotifyId
        DalOperationAboutCourses dalOperationAboutCourses = new DalOperationAboutCourses();
        dsSchoolNotify = dalOperationAboutCourses.GetSchoolworkNotifybyId(schoolworkNotifyId);

        isOnline = bool.Parse(dsSchoolNotify.Tables[0].Rows[0]["isOnline"].ToString());
        //如果是在线提交作业,隐藏最后一个也就是书面作业标签,否则隐藏在线作业标签
        if (isOnline)
        {
            liFragment4.Visible = false;
        }
        else
        {
            liFragment2.Visible = false;
            liFragment3.Visible = false;
        }
    }

    public void ShowLiControl(Page page, string ShowLiId)
    {
        if (ShowLiId == "liFragment5") liFragment5.Visible = true;
        HtmlControl liControl = (HtmlControl)page.Master.Master.FindControl("ContentPlaceHolder1").FindControl(ShowLiId);
        liControl.Attributes.Add("class", "ui-tabs-selected");       
    }
}
