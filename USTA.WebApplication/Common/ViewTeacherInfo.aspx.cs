using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;
using USTA.Model;
using USTA.PageBase;
using System.Configuration;

public partial class Common_ViewTeacherInfo : CheckUserWithCommonPageBase
{
    public string email = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["email"] == null)
            {
                Javascript.GoHistory(-1, Page);
                return;
            }
            else
            {
                email = Request["email"].Trim();
                PageDataBind();
                DalOperationAboutTeacher DalOperationAboutTeacher = new DalOperationAboutTeacher();

                String teacherNo = DalOperationAboutTeacher.GetTeacherNoByAddressEmail(email);
                DataSet ds = DalOperationAboutTeacher.GetCoursesByTeacherAssistant(teacherNo);
                courses.DataSource = ds.Tables[0];
                courses.DataBind();
            }
        }
    }

    public void PageDataBind()
    {
        DalOperationUsers dou = new DalOperationUsers();
        TeachersList teacher = dou.FindTeacherByEmail(email);
        if (teacher == null)
        {
            Javascript.Alert("对不起，您要查看的教师不存在！", Page);
        }
        else
        {

            lblName.Text = teacher.teacherName;
            lblOffice.Text = teacher.officeAddress;
            lblEmail.Text = teacher.emailAddress;
            lblRemark.Text = teacher.remark;
        }
    }
}
