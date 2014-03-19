using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;

public partial class Common_CInfoCourseNotify : System.Web.UI.Page
{
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        Master.ShowLiControl(this.Page, "liFragment3");
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DataSet coursesInfo = null;
        if (UserCookiesInfo.userType == (int)UserType.StudentRole)
        {
            coursesInfo = DalOperationAboutCourses.GetCoursesInfo(UserCookiesInfo.userNo, Master.courseNo.ToString().Trim(), classID, termtag,  "3");
        }
        else
        {
            coursesInfo = DalOperationAboutCourses.GetCoursesInfo(UserCookiesInfo.userNo, Master.courseNo.ToString().Trim(), classID, termtag, "3");
        }


        dlstCourseNotify.DataSource = coursesInfo.Tables[0];
        dlstCourseNotify.DataBind();
    }

    public bool isNew(string date)
    {
        return DateTime.Now.AddDays(-CommonUtility.GetNewDays()).CompareTo(Convert.ToDateTime(date)) < 0;
    }
}

   