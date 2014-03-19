using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using USTA.PageBase;

public partial class Administrator_ViewStudentInfo : CheckUserWithCommonPageBase
{
    public string studentNo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["studentNo"] == null)
            {
                Javascript.GoHistory(-1, Page);
                return;
            }
            else
            {
                studentNo = Request["studentNo"].Trim();
                PageDataBind();
            }
        }
    }

    public void PageDataBind()
    {
        DalOperationUsers dou = new DalOperationUsers();
        StudentsList student = dou.FindStudentByNo(studentNo);
        if (student == null)
        {
            Javascript.AlertAndRedirect("对不起，您要查看的学生不存在！", "/Administrator/StudentManager.aspx", Page);
        }
        else
        {
            lblNo.Text = student.studentNo;
            lblName.Text = student.studentName;
            lblSpeciality.Text = student.studentSpeciality;
            lblClass.Text = student.classNo;
            lblMobileNo.Text = student.mobileNo;
            lblEmail.Text = student.emailAddress;
            lblRemark.Text = student.remark;
        }
    }
}
