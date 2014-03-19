using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using USTA.Model;


public partial class Teacher_CInfoSchoolworkNotify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Master.ShowLiControl(this.Page, "liFragment7");
            //删除课程作业
            int schoolworkNotifyId = 0;
            if (CommonUtility.SafeCheckByParams<string>(Request["schoolworkNotifyId"], ref schoolworkNotifyId))
            {
                //DalOperationAboutCourseNotifyInfo dalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
                if (Request["op"] == "delete")
                {
                    DalOperationAboutSchoolWorks dalOperationAboutCourseResources = new DalOperationAboutSchoolWorks();

                    dalOperationAboutCourseResources.DeleteSchoolworkNotify(schoolworkNotifyId);
                }
            }

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
            DataSet coursesInfo = null;
            coursesInfo = DalOperationAboutCourses.GetCoursesInfo(Master.courseNo.ToString().Trim(),Master.classID,Master.termtag, "7");

            dlstSchoolworkNotify.DataSource = coursesInfo.Tables[0];
            dlstSchoolworkNotify.DataBind();
        }

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

}