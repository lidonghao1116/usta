using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Common;
using USTA.Model;
using USTA.Bll;



public partial class Teacher_CInfoCourseTeacher : System.Web.UI.Page
{
    public string editTeacherAtag = string.Empty;
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment10");
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutCourses dal = new DalOperationAboutCourses();
        if (dal.IsTeacherAtCourse(UserCookiesInfo.userNo, Master.courseNo,classID,termtag))
        {

            editTeacherAtag = "<a href=\"EditTeacherResume.aspx?keepThis=true&courseNo=" + Master.courseNo + "&classID=" + Server.UrlEncode(Master.classID )+ "&termtag=" + Master.termtag + "&fragment=9&TB_iframe=true&height=380&width=800\" title=\"编辑教师简介\" class=\"thickbox\">编辑</a>";
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Master.DsCourse != null)
        {
            dlstCourseTeacher.DataSource = Master.DsCourse.Tables[0];
            dlstCourseTeacher.DataBind();
        }
    }
}