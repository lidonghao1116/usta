using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;
using USTA.Model;


public partial class Teacher_CInfoCourseNotify : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Master.ShowLiControl(this.Page, "liFragment3");

            //课程通知
            int notifyId = 0;
            if (CommonUtility.SafeCheckByParams<string>(Request["notifyId"], ref notifyId))
            {
                DalOperationAboutCourseNotifyInfo dalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
                if (Request["op"] == "delete")
                {
                    dalOperationAboutCourseNotifyInfo.DelCourseNotifyInfoById(notifyId);
                }
                else if (Request["op"] == "toTop")
                {
                    DalOperationAboutCourses doac = new DalOperationAboutCourses();

                    CoursesNotifyInfo coursesNotify = doac.FindCourseNotifyById(notifyId);
                    if (coursesNotify.isTop > 0)
                        doac.Canceltop(notifyId);
                    else
                        doac.Addtop(notifyId);
                }
            }

            
            DataListBindCourseNotify();
        }
       
    }

    protected void DataListBindCourseNotify()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataView dv = doac.GetCoursesInfo(Master.courseNo,Master.classID,Master.termtag, "3").Tables[0].DefaultView;//第3个标签，绑定课程通知信息

        this.AspNetPager2.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        this.dlstCourseNotify.DataSource = pds;
        this.dlstCourseNotify.DataBind();

        if (this.dlstCourseNotify.Items.Count == 0)
        {
            btnDelete.Visible = false;
        }
        else
        {
            btnDelete.Visible = true;
        }

        if (pds.Count > 0) 
        {
            this.dlstCourseNotify.ShowFooter = false;
        }
    }

    //课程通知分页
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataListBindCourseNotify();
    }

    ////绑定课程通知
    //protected void dlstCourseNotify_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    DalOperationAboutCourseNotifyInfo dalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
    //    string NotifyId = dlstCourseNotify.DataKeys[e.Item.ItemIndex].ToString();
    //    if (e.CommandName == "delete")
    //    {
    //        dalOperationAboutCourseNotifyInfo.DelCourseNotifyInfoById(int.Parse(NotifyId));

    //        Javascript.JavaScriptLocationHref("CInfoCourseNotify.aspx?courseNo=" + Request["courseNo"], Page);
    //    }
    //    else if (e.CommandName == "editIstop")
    //    {
    //        DalOperationAboutCourses doac = new DalOperationAboutCourses();
    //        string courseNotifyInfoId = dlstCourseNotify.DataKeys[e.Item.ItemIndex].ToString();
    //        CoursesNotifyInfo coursesNotify = doac.FindCourseNotifyById(int.Parse(courseNotifyInfoId));
    //        if (coursesNotify.isTop > 0)
    //            doac.Canceltop(int.Parse(courseNotifyInfoId));
    //        else
    //            doac.Addtop(int.Parse(courseNotifyInfoId));
    //        Javascript.JavaScriptLocationHref("CInfoCourseNotify.aspx?courseNo=" + Request["courseNo"], Page);
    //        //Javascript.JavaScriptLocationHref("CourseInfo.aspx?courseNo=" + Request["courseNo"] + "&fragment=3", Page);
    //    }
    //}

    //删除选定的课程通知
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        int courseNotifyInfoId;
        foreach (DataListItem item in this.dlstCourseNotify.Items)
        {

            CheckBox chkItem = (CheckBox)item.FindControl("ChkBox");
            Label lbl = (Label)item.FindControl("lblcourseNotifyInfoId");
            if (chkItem.Checked)
            {
                //被勾选的要删除
                courseNotifyInfoId = int.Parse(lbl.Text);
                doac.DeleteCourseNotifyById(courseNotifyInfoId);
            }
        }
        DataListBindCourseNotify();
    }


    public bool isNew(string date)
    {
        return DateTime.Now.AddDays(-CommonUtility.GetNewDays()).CompareTo(Convert.ToDateTime(date)) < 0;
    }
}