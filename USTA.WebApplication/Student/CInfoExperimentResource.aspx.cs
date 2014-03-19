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

public partial class Student_CousrInfo_6_ExperimentResource : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment6");
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        DataSet coursesInfo = null;
        coursesInfo = DalOperationAboutCourses.GetCoursesInfo(UserCookiesInfo.userNo, Master.courseNo.ToString().Trim(), Master.classID.Trim(), Master.termtag.Trim(), "6");

        dlstExperimentResource.DataSource = coursesInfo.Tables[0];
        dlstExperimentResource.DataBind();
    }

    public bool isNew(string date)
    {
        return DateTime.Now.AddDays(-CommonUtility.GetNewDays()).CompareTo(Convert.ToDateTime(date)) < 0;
    }
}