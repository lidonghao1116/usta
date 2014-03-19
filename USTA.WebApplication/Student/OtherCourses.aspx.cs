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

public partial class Student_OtherCourses : System.Web.UI.Page
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



            DatalistBind("当前学期");


            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.GetHistoryTags().Tables[0];
            ddltTerms.Items.Add(new ListItem("当前学期", "当前学期"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddltTerms.Items.Add(new ListItem(CommonUtility.ChangeTermToString((dt.Rows[i]["termTag"].ToString())), dt.Rows[i]["termTag"].ToString()));
            }

            
        }
    }
    protected void DatalistBind(string tag)
    {
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutStudent DalOperationAboutStudent = new DalOperationAboutStudent();
        DataSet ds = new DataSet();
        if (tag == "当前学期")
        {
            ds = DalOperationAboutStudent.GetOtherCoursesByStudentNo(UserCookiesInfo.userNo);
        }
        else
        {
            ds = DalOperationAboutStudent.GetOtherCoursesByStudentNo(UserCookiesInfo.userNo, tag);
        }
        dlstCourses.DataSource = ds.Tables[0];
        dlstCourses.DataBind();
    }

    protected void ddltTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddltTerms.SelectedValue == "当前学期")
        {
            DatalistBind("当前学期");
        }
        else
        {
            DatalistBind(ddltTerms.SelectedValue);
        }
    }
}
