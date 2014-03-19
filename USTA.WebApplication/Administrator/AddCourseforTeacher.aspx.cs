using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.Bll;
using System.Configuration;
using USTA.PageBase;

public partial class Administrator_AddCourseforTeacher : CheckUserWithCommonPageBase
{
    public string teacherNo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["teacherNo"] != null && Request["courseNo"] != null && Request["classID"] != null && Request["termTag"] != null)
            {
                teacherNo = Request["teacherNo"];
                String courseNo = Request["courseNo"].ToString().Trim();
                String ClassID = Server.UrlDecode(Request["classID"].ToString().Trim());
                String termTag = Request["termTag"].ToString().Trim();

                DalOperationAboutTeacher dalOperationAboutTeacher = new DalOperationAboutTeacher();
               
                dalOperationAboutTeacher.AddCourseOfTeacher(teacherNo, courseNo, ClassID, termTag);
                Javascript.RefreshParentWindow("TeacherManager.aspx?fragment=4&teacherNo=" + teacherNo, Page);
                return;
            }
            if (Request["teacherNo"] != null)
            {
                teacherNo = Request["teacherNo"];
                DalOperationAboutTeacher dalOperationAboutTeacher = new DalOperationAboutTeacher();
                DataSet ds = dalOperationAboutTeacher.GetOtherCoursesByTeacherNo(teacherNo);
                courses.DataSource = ds.Tables[0];
                courses.DataBind();
 
            }
            else
            {
                Javascript.GoHistory(-1, Page);
            }
        }
    }
}
