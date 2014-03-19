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

public partial class Student_Courses : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                , divFragment1, divFragment2, divFragment3);

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.GetHistoryTagsByStudentNo(UserCookiesInfo.userNo).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddltTerms.Items.Add(new ListItem(CommonUtility.ChangeTermToString((dt.Rows[i]["Year"].ToString())), dt.Rows[i]["Year"].ToString()));
            }

            if (ddltTerms.Items.Count > 0)
            {
                DatalistBind(ddltTerms.SelectedValue);
            }
        }       
    }

    protected void DatalistBind(string tag)
    {
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutStudent DalOperationAboutStudent = new DalOperationAboutStudent();
        DataSet ds = new DataSet();
        ds = DalOperationAboutStudent.GetCoursesByStudentNo(UserCookiesInfo.userNo, tag);

        dlstCourses.DataSource = ds.Tables[0];
        dlstCourses.DataBind();

        if (dlstCourses.Items.Count == 0)
        {
            dlstCourses.ShowFooter = true;
        }
        else
        {
            dlstCourses.ShowFooter = false;
        }
    }

    protected void ddltTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        DatalistBind(ddltTerms.SelectedValue);
    }
}
