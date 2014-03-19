using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Dal;

public partial class Administrator_ViewChooseCourse : System.Web.UI.Page
{
    public string studentNo;
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["studentNo"] != null)
            {
                studentNo = Request["studentNo"];

                //删除
                if (Request["del"]=="true" && Request["courseNo"] != null)
                {
                    DalOperationAboutStudent dalw = new DalOperationAboutStudent();
                    dalw.DelChooseCourse(studentNo, Request["courseNo"]);
                }

                //提交
                if (Request["add"] =="true" && Request["courseNo"] != null)
                {
                    DalOperationAboutStudent dala = new DalOperationAboutStudent();
                    dala.AddChooseCourse(studentNo, courseNo, termtag, classID);
                    Javascript.RefreshParentWindow("ViewChooseCourse.aspx?studentNo="+studentNo, Page);
                    return;
                }

                DalOperationAboutStudent dal = new DalOperationAboutStudent();
                lblstudentName.Text = dal.GetStudentById(Request["studentNo"]).studentName;
                DalOperationAboutStudent dal1= new DalOperationAboutStudent();
                DataSet ds = dal1.GetCoursesByStudentNo(Request["studentNo"]);
                dlstcourses.DataSource = ds.Tables[0];
                dlstcourses.DataBind();
            }
        }
    }
}
