using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Dal;
using USTA.PageBase;
using System.Configuration;

public partial class Teacher_EditTeacherResume : CheckUserWithCommonPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["courseNo"] == null && Request["fragment"] != null)
            {
                Javascript.GoHistory(-1, Page);
                return;
            }
            else
            {
                DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
                this.Textarea1.Value = DalOperationAboutCourses.GetCoursesByNo(Request["courseNo"].Trim(),Server.UrlDecode(Request["classID"].Trim()),Request["termtag"].Trim()).teacherResume;
            }
        }
    }


    protected void btnCommit_Click(object sender, EventArgs e)
    {
        if (Request["courseNo"] != null)
        {
            DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
            DalOperationAboutCourses.UpdateTeacherResume(Request["courseNo"], Server.UrlDecode(Request["classID"].Trim()), Request["termtag"].Trim(), Textarea1.Value);
            Javascript.RefreshParentWindow("修改成功！", "CInfoCourseTeacher.aspx?courseNo=" + Request["courseNo"] + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"])) + "&termtag=" + Request["termtag"], Page);
        }
    }
}
