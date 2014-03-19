using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Teacher
{
    public partial class ViewTeacherInfo : CheckUserWithCommonPageBase
    {
        public string teacherNo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["teacherNo"] == null)
                {
                    Javascript.GoHistory(-1, Page);
                    return;
                }
                else
                {
                    teacherNo = Request["teacherNo"].Trim();
                    PageDataBind();
                }
            }
        }

        public void PageDataBind()
        {
            DalOperationUsers dou = new DalOperationUsers();
            TeachersList teacher = dou.FindTeacherByNo(teacherNo);
            if (teacher == null)
            {
                Javascript.AlertAndRedirect("对不起，您要查看的教师不存在！", "/Administrator/TeacherManager.aspx", Page);
            }
            else
            {
                //lblNo.Text = teacher.teacherNo;
                lblName.Text = teacher.teacherName;
                lblOffice.Text = teacher.officeAddress;
                lblEmail.Text = teacher.emailAddress;
                lblRemark.Text = teacher.remark;
            }
        }
    }
}