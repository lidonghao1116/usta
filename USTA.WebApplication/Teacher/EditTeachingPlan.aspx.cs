using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Common;
using USTA.PageBase;
using System.Configuration;

public partial class Teacher_EidtTeachingPlan : CheckUserWithCommonPageBase
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
                this.Textarea1.Value = DalOperationAboutCourses.GetCoursesByNo(Request["courseNo"].Trim(),Server.UrlDecode(Request["classID"].Trim()),Request["termtag"].Trim()).teachingPlan;
            }
        }
    }
    protected void btnCommit_Click(object sender, EventArgs e)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        DalOperationAboutCourses.UpdateCourseTeachingPlan(Request["courseNo"].Trim(),Server.UrlDecode(Request["classID"].Trim()),Request["termTag"].Trim(), Textarea1.Value);
        Javascript.RefreshParentWindow("修改成功！", "CInfoTeachingPlan.aspx?courseNo=" + Request["courseNo"].Trim() + "&classID=" + Server.UrlEncode(Server.UrlDecode(Request["classID"].ToString().Trim())) + "&termtag=" + Request["termtag"].ToString().Trim(), Page);
    }
}
