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

public partial class Teacher_CInfoExperimentResource : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Master.ShowLiControl(this.Page, "liFragment6");
            //删除实验
            int erid = 0;
            if (Request["op"] == "delete")
            {

                if (CommonUtility.SafeCheckByParams<string>(Request["experimentResourceId"], ref erid))
                {

                    DalOperationAboutExperimentResources daloAexper = new DalOperationAboutExperimentResources();
                   
                    daloAexper.DelExperimentResources(erid);
                    
                }
            }

            
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        DataSet coursesInfo = null;
        coursesInfo = DalOperationAboutCourses.GetCoursesInfo(Master.courseNo.ToString().Trim(),Master.classID,Master.termtag, "6");

        dlstExperimentResource.DataSource = coursesInfo.Tables[0];
        dlstExperimentResource.DataBind();
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