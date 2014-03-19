using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using USTA.Dal;
using USTA.Model;
using USTA.Common;

public partial class MasterPage_ViewExperimentForTeacher : System.Web.UI.MasterPage
{
    public ExperimentResources ExperimentResources;

    //
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["classID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["classID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;
    //保存当前实验编号
    public int experimentResourceId;
    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }

        CommonUtility.SafeCheckByParams<string>(HttpContext.Current.Request["experimentResourceId"] != null ? HttpContext.Current.Request["experimentResourceId"] : string.Empty, ref experimentResourceId);
    }

    public void ShowLiControl(Page page, string ShowLiId)
    {
        if (ShowLiId == "liFragment4") liFragment4.Visible = true;
        HtmlControl liControl = (HtmlControl)page.Master.Master.FindControl("ContentPlaceHolder1").FindControl(ShowLiId);
        liControl.Attributes.Add("class", "ui-tabs-selected");
    }
}
