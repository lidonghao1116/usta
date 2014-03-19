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
public partial class Common_CInfoCourseResource : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        Master.ShowLiControl(this.Page, "liFragment4");
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DataSet coursesInfo = null;
        if (UserCookiesInfo.userType == (int)UserType.StudentRole)
        {
            coursesInfo = DalOperationAboutCourses.GetCoursesInfo( Master.courseNo.ToString().Trim(),Master.classID,Master.termtag, "5");
        }
        else
        {
            coursesInfo = DalOperationAboutCourses.GetCoursesInfo(Master.courseNo.ToString().Trim(), Master.classID, Master.termtag, "5");
        }

        dlstCourseResource.DataSource = coursesInfo.Tables[0];
        dlstCourseResource.DataBind();
    }

    public bool isNew(string date)
    {
        return DateTime.Now.AddDays(-CommonUtility.GetNewDays()).CompareTo(Convert.ToDateTime(date)) < 0;
    }

    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false, string.Empty);
    }
}