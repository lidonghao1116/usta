using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Dal;

public partial class Teacher_EditCourseIntroduction : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["courseNo"] == null)
            {
                Javascript.GoHistory(-1, Page);
                return;
            }
            else
            {
                DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
                this.Textarea1.Value = DalOperationAboutCourses.GetCoursesByNo(Request["courseNo"].Trim(),Server.UrlDecode(Request["classID"].Trim()),Request["termtag"].Trim()).courseIntroduction;
            }
        }
    }
    protected void btnCommit_Click(object sender, EventArgs e)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        DalOperationAboutCourses.UpdateCourseIntroduction(Request["courseNo"].Trim(), Server.UrlDecode(Request["classID"].Trim()), Request["termtag"].Trim(), Textarea1.Value);
        Javascript.AlertAndRedirect("修改成功！", "CInfoCourseIntro.aspx?courseNo=" + Request["courseNo"].Trim(), Page);
    }
}
