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

public partial class Teacher_Courses : System.Web.UI.Page
{
    public string tags;
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
            DataTable dt = doac.GetHistoryTagsByTeacherNo(UserCookiesInfo.userNo).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddltTerms.Items.Add(new ListItem(CommonUtility.ChangeTermToString((dt.Rows[i]["termTag"].ToString())), dt.Rows[i]["termTag"].ToString()));
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
        DataSet ds = new DataSet();

        DataSet dsA = new DataSet();

        DalOperationAboutTeacher DalOperationAboutTeacher = new DalOperationAboutTeacher();
        DalOperationAboutAssistant DalOperationAboutAssistant = new DalOperationAboutAssistant();
        ds = DalOperationAboutTeacher.GetCoursesByTeachertNo(UserCookiesInfo.userNo, tag);
        dsA = DalOperationAboutAssistant.GetCoursesByAssistantNo(UserCookiesInfo.userNo, tag);

        tags = TagToString(tag);

        dlstAssistantCourses.DataSource = dsA.Tables[0];
        dlstAssistantCourses.DataBind();
        dlstCourses.DataSource = ds.Tables[0];
        dlstCourses.DataBind();

        if (dlstAssistantCourses.Items.Count == 0)
        {
            dlstAssistantCourses.ShowFooter = true;
        }
        else
        {
            dlstAssistantCourses.ShowFooter = false;
        }


        if (dlstCourses.Items.Count == 0)
        {
            dlstCourses.ShowFooter = true;
        }
        else
        {
            dlstCourses.ShowFooter = false;
        }
    }

    public string TagToString(string tag)
    {
        return CommonUtility.ChangeTermToString(tag);
    }

    protected void ddltTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        DatalistBind(ddltTerms.SelectedValue);
    }
}


