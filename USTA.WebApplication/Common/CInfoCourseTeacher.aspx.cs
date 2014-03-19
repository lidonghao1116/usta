using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_CInfoCourseTeacher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment5");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Master.DsCourse != null)
        {
            dlstCourseTeacher.DataSource = Master.DsCourse.Tables[0];
            dlstCourseTeacher.DataBind();
        }
    }
}