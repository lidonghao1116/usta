using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;

public partial class Teacher_CInfoCourseIntro : System.Web.UI.Page
{
    public string editCourseintroductionAtag = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment1");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Master.DsCourse != null)
        {
            dlstCourseIntro.DataSource = Master.DsCourse.Tables[0];
            dlstCourseIntro.DataBind();
        }
    }
}