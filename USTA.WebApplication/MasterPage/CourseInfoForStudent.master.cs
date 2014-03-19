using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;

public partial class Student_CousrInfo_CourseInfo : System.Web.UI.MasterPage
{
    public int iframeCount = 0;
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;
    public string courseName;
    private DataSet dsCourse;
    public DataSet DsCourse
    {
        get { return dsCourse; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null||termtag==string.Empty||classID==string.Empty)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }

        if (Request["courseNo"] != null)
        {
            UserCookiesInfo User = BllOperationAboutUser.GetUserCookiesInfo();
            //获得课程的基本信息
            DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();

            if (User.userType == 3)
            {
                DalOperationAboutCourses.UpdateCoursesAttention(courseNo, classID, termtag, User.userNo);
            }

            dsCourse = DalOperationAboutCourses.GetCourseInfoTeacherAndAssistant(courseNo,classID,termtag);

            string ltlStringCourseName = string.Empty;
            string ltlStringLessonTimeAndAddress = string.Empty;

            //其实有一行,因为couserNo之传过来一个值
            for (int i = 0; i < dsCourse.Tables[0].Rows.Count; i++)
            {
                ltlStringCourseName += dsCourse.Tables[0].Rows[i]["courseName"].ToString();
                ltlStringLessonTimeAndAddress += dsCourse.Tables[0].Rows[i]["lessonTimeAndAddress"].ToString();
            }
            courseName = ltlStringCourseName;
            if (ltlStringCourseName != string.Empty)
            {
                ltlCourseName.Text = ltlStringCourseName;
            }
            if (ltlStringLessonTimeAndAddress != string.Empty)
            {
                ltlLessonTimeAndAddress.Text = ltlStringLessonTimeAndAddress;
            }

            //绑定任课教师数据,一门课的任课教师可能有多个
            string ltlStringTeacherName = string.Empty;

            for (int i = 0; i < dsCourse.Tables["1"].Rows.Count; i++)
            {
                ltlStringTeacherName += "<a href=\"/Common/ViewTeacherInfo.aspx?keepThis=true&email=" + dsCourse.Tables["1"].Rows[i]["emailAddress"].ToString().Trim() + "&TB_iframe=true&height=300&width=600\" title=\"查看教师信息\" class=\"thickbox\">" + dsCourse.Tables["1"].Rows[i]["teacherName"].ToString().Trim() + "</a>&nbsp;&nbsp;";
            }

            if (ltlStringTeacherName != string.Empty)
            {
                ltlTeacherName.Text = ltlStringTeacherName;
            }

            //绑定助教数据，一门课的任课助教可能有多个
            string ltlStringAssistantName = string.Empty;

            for (int i = 0; i < dsCourse.Tables["2"].Rows.Count; i++)
            {
                ltlStringAssistantName += "<a href=\"/Common/ViewTeacherInfo.aspx?keepThis=true&email=" + dsCourse.Tables["2"].Rows[i]["emailAddress"].ToString().Trim() + "&TB_iframe=true&height=300&width=600\" title=\"查看助教信息\" class=\"thickbox\">" + dsCourse.Tables["2"].Rows[i]["assistantName"].ToString().Trim() + "</a>&nbsp;&nbsp;";

            }

            if (ltlStringAssistantName != string.Empty)
            {
                ltlAssistantName.Text = ltlStringAssistantName;
            }

        }
    }

    public void ShowLiControl(Page page, string ShowLiId)
    {
        HtmlControl liControl = (HtmlControl)page.Master.Master.FindControl("ContentPlaceHolder1").FindControl(ShowLiId);
        liControl.Attributes.Add("class", "ui-tabs-selected");
    }
}
