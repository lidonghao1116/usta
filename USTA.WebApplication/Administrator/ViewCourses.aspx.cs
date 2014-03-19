using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

using USTA.Model;
using USTA.Dal;
using USTA.Common;

public partial class Administrator_ViewCourses : System.Web.UI.Page
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


            DataListBind("当前学期");

            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.GetHistoryTags().Tables[0];
            ddltTerms.Items.Add(new ListItem("当前学期", "当前学期"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddltTerms.Items.Add(new ListItem(CommonUtility.ChangeTermToString((dt.Rows[i]["termTag"].ToString())), dt.Rows[i]["termTag"].ToString()));
            }
        }
    }

    //绑定课程表信息到DataList,是当前学期的课程
    public void DataListBind(string tag)
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        string termTag = DalCommon.GetTermTag(new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString));//获取当前学期的标识
        DataTable dt = new DataTable();
        if (tag == "当前学期")
        {
            dt = doac.FindCurrentTermCoursesList().Tables[0];
        }
        else
        {
            dt=doac.FindCourseByTermTage(tag).Tables[0];
        }
        this.dlCourse.DataSource = dt;
        this.dlCourse.DataBind();
    }

    protected void ddltTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddltTerms.SelectedValue == "当前学期")
        {
            DataListBind("当前学期");
        }
        else
        {
            DataListBind(ddltTerms.SelectedValue);
        }
    }

}
