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

public partial class Common_NotifyList : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    public string isAdmin = "0";
    public int notifyTypeParentId = HttpContext.Current.Request["pid"] == null ? -3 : int.Parse(HttpContext.Current.Request["pid"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DalOperationAboutAdminNotifyType dalOperationAboutAdminNotifyType = new DalOperationAboutAdminNotifyType();
            DataSet _ds = dalOperationAboutAdminNotifyType.FindAllParentAdminNotifyType();

            string strLi = string.Empty;

            for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
            {
                strLi += "<li id=\"liFragment" + _ds.Tables[0].Rows[i]["notifyTypeId"].ToString().Trim() + "\" pid=\"" + _ds.Tables[0].Rows[i]["notifyTypeId"].ToString().Trim() + "\"><a href=\"?pid=" + _ds.Tables[0].Rows[i]["notifyTypeId"].ToString().Trim() + "\"><span>" + _ds.Tables[0].Rows[i]["notifyTypeName"].ToString().Trim() + "</span></a></li>";

                if (i == 0 && notifyTypeParentId == -3)
                {
                    notifyTypeParentId = int.Parse(_ds.Tables[0].Rows[i]["notifyTypeId"].ToString().Trim());
                }
            }

            ltlNotifyTypeParent.Text = strLi;

            if (notifyTypeParentId > 0 || notifyTypeParentId == -3)
            {

                divFragment1.Attributes.Add("pid", notifyTypeParentId.ToString());
                DataListBindNotifyType(notifyTypeParentId);
                divFragment1.Visible = true;
            }

            if (notifyTypeParentId == -1)
            {
                ViewAdminNotify();
            }
            if (notifyTypeParentId == -2)
            {
                if (Request["notifyTypeId"] != null)
                {
                    int typeId = -1;
                    if (CommonUtility.SafeCheckByParams<string>(Request["notifyTypeId"], ref typeId))
                    {
                        DataListBindNotifyByTypeId(typeId);
                    }
                    else
                    {
                        Javascript.GoHistory(-1, Page);
                    }
                }
            }



            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

            isAdmin = UserCookiesInfo.userType.ToString();

            if (UserCookiesInfo.userType == 3)
            {
                DalOperationUsers DalOperationUsers = new DalOperationUsers();
                DataSet ds = DalOperationUsers.StudentTips(UserCookiesInfo.userNo);

                int examCount = ds.Tables[0].Rows.Count;

                int experimentsCount = ds.Tables[1].Rows.Count;

                int schoolWorksCount = ds.Tables[2].Rows.Count;

                int schoolworkpaperCount = ds.Tables[3].Rows.Count;

                int feedBackCount = ds.Tables[4].Rows.Count;

                DalOperationPatch dal = new DalOperationPatch();
                DataSet ds1 = dal.GetLatestCourseNotify(UserCookiesInfo.userNo);

                int courseNotifyCount = ds1.Tables[0].Rows.Count;


                tbTip.Visible = true;

                if (feedBackCount == 0)
                {
                    divFeedBack.Visible = false;
                    tdFeedBack.Visible = false;
                }
                else
                {
                    ltlFeedBack.Text = feedBackCount.ToString();
                    dlistFeedBack.DataSource = ds.Tables[4];
                    dlistFeedBack.DataBind();
                }

                if (examCount == 0)
                {
                    divExam.Visible = false;
                    tdExam.Visible = false;
                }
                else
                {
                    ltlExamTip.Text = examCount.ToString();
                    dlstExam.DataSource = ds.Tables[0];
                    dlstExam.DataBind();
                }

                if (experimentsCount == 0)
                {
                    divExperiments.Visible = false;
                    tdExperiments.Visible = false;
                }
                else
                {
                    ltlExperimentsTip.Text = experimentsCount.ToString();
                    dlstExpriment.DataSource = ds.Tables[1];
                    dlstExpriment.DataBind();
                }

                if (schoolWorksCount == 0)
                {
                    divSchoolWorks.Visible = false;
                    tdSchoolWorks.Visible = false;
                }
                else
                {
                    ltlSchoolWorksTip.Text = schoolWorksCount.ToString() + "次在线作业待提交";

                    dlstSchoolwork.DataSource = ds.Tables[2];
                    dlstSchoolwork.DataBind();
                }

                if (schoolworkpaperCount == 0)
                {
                    divSchoolWorksPaper.Visible = false;
                    tdSchoolWorksPaper.Visible = false;
                }
                else
                {
                    ltlschoolworkpaper.Text = "近期有" + schoolworkpaperCount.ToString() + "次书面作业待提交(此为提醒功能，并不表示未提交作业)";

                    dlstSchoolworkpa.DataSource = ds.Tables[3];
                    dlstSchoolworkpa.DataBind();
                }

                if (courseNotifyCount == 0)
                {
                    divNotify.Visible = false;
                    tdNotify.Visible = false;
                }
                else
                {
                    ltlnotify.Text = courseNotifyCount.ToString();
                    DataList1.DataSource = ds1.Tables[0];
                    DataList1.DataBind();
                }
            }
            if (UserCookiesInfo.userType == 0 || UserCookiesInfo.userType == 1 || UserCookiesInfo.userType == 2)
            {
                DalOperationAboutArchivesConfig dalArchivesConfig = new DalOperationAboutArchivesConfig();

                if (dalArchivesConfig.CheckArchivesNotifyTime())
                {
                    tdArchivesNotify.Visible = true;
                    divArchivesNotify.Visible = true;
                }
                else
                {
                    tdArchivesNotify.Visible = false;
                    divArchivesNotify.Visible = false;
                }

                DalOperationAboutGameCategory dal = new DalOperationAboutGameCategory();

                if (dal.GetGameCategoryIng(DateTime.Now).Tables[0].Rows.Count>0)
                {
                    tdGameCategory.Visible = true;
                    divGameCategory.Visible = true;
                }
                else
                {
                    tdGameCategory.Visible = false;
                    divGameCategory.Visible = false;
                }


            }
        }
    }

    //依据文章类型，绑定第类的文章列表
    protected void dlstNotifyType_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList dataList = (DataList)e.Item.FindControl("dlstNotify");
            DataRowView rowv = (DataRowView)e.Item.DataItem;
            int mainID = Convert.ToInt32(rowv["notifyTypeId"]);
            DalOperationAboutAdminNotify DalOperationAboutAdminNotify = new DalOperationAboutAdminNotify();
            DataSet ds = DalOperationAboutAdminNotify.FindTheTop5NotifyByTypeId(mainID);
            dataList.DataSource = ds.Tables[0].DefaultView;
            dataList.DataBind();

            if (ds.Tables[0].Rows.Count == 0)
            {
                dataList.ShowFooter = true;
            }
            else
            {
                dataList.ShowFooter = false;
            }
        }
    }

    //绑定文章类型列表
    protected void DataListBindNotifyType(int notifyTypeParentId)
    {
        DalOperationAboutAdminNotifyType dalOperationAboutAdminNotifyType = new DalOperationAboutAdminNotifyType();
        DataSet ds = dalOperationAboutAdminNotifyType.FindAllAdminNotifyTypeByParentId(notifyTypeParentId);

        this.dlstNotifyType.DataSource = ds;
        this.dlstNotifyType.DataBind();
    }


    //***第3个标签：显示某一类型的文章信息－－－－－－－－－－－－－－－开始

    //绑定文章类型列表
    protected void DataListBindNotifyByTypeId(int type)
    {
        DalOperationAboutAdminNotify DalOperationAboutAdminNotify = new DalOperationAboutAdminNotify();
        DataSet ds = DalOperationAboutAdminNotify.FindNotifyByTypeId(type);

        this.AspNetPager1.RecordCount = ds.Tables[0].DefaultView.Count;
        this.AspNetPager1.PageSize = CommonUtility.pageSize;

        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = ds.Tables[0].DefaultView;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = this.AspNetPager1.PageSize;

        this.dlistNotifyList.DataSource = pds;
        this.dlistNotifyList.DataBind();

        if (pds.Count > 0) {
            this.dlistNotifyList.ShowFooter = false;
        }
    }

    //protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    //{
    //    int notifyTypeId = 0;
    //    if (CommonUtility.SafeCheckByParams<string>(Request["notifyTypeId"], ref notifyTypeId))
    //    {
    //        DataListBindNotifyByTypeId(notifyTypeId);
    //    }
    //}
    //***第3个标签：显示某一类型的文章信息－－－－－－－－－－－－－－－结束

    #region 查看标签部分功能函数
    protected void ViewAdminNotify()
    {
        int notifyId = -1;
        if (CommonUtility.SafeCheckByParams<string>(Request["adminNotifyInfoId"], ref notifyId))
        {
            DalOperationAboutAdminNotify dal = new DalOperationAboutAdminNotify();

            //浏览次数加1
            dal.AddScanCount(notifyId);

            DataSet adminNotify = dal.FindNotifybyNo(notifyId);
            news.DataSource = adminNotify.Tables[0];
            news.DataBind();

            if (adminNotify.Tables[0].Rows[0]["attachmentIds"].ToString().Length > 0)//有附件则显示
            {
                DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
            }

        }
        else
        {
            Javascript.GoHistory(-1, Page);
        }
    }
    public string GetURL(string aids)
    {
        int iframeCount = 0;
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref iframeCount, false,string.Empty);
    }
    /// <summary>
    /// 判断是否为新
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool isNew(string date)
    {
        return DateTime.Now.AddDays(-CommonUtility.GetNewDays()).CompareTo(Convert.ToDateTime(date)) < 0;
    }
    #endregion
}
