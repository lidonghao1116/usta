using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;

public partial class Administrator_CurrentTermCourseList : System.Web.UI.Page
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

            //绑定学期标识下拉列表
            DataBindTermTagList();
            //绑定课程列表--学期标识(termTag)
            DataBindCourseList(ddlTermTags.SelectedValue);
        }
    }

    //绑定学期标识下拉列表
    public void DataBindTermTagList()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataTable dt = doac.FindAllTermTags().Tables[0];
        string termTag = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            termTag = dt.Rows[i]["termTag"].ToString().Trim();
            ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
            ddlTermTags.Items.Add(li);
        }
    }
    //下拉列表事件
    protected void ddlTermTags_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定课程列表--学期标识(termTag)
        DataBindCourseList(ddlTermTags.SelectedValue);
    }

    //定课程列表--学期标识(termTag)
    public void DataBindCourseList(string termTag)
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataSet ds = doac.FindTermCoursesList(termTag);
        dlstCourses.DataSource = ds.Tables[0];
        dlstCourses.DataBind();
    }

}
