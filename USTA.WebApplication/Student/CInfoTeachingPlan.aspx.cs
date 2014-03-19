using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_CousrInfo_4_TeachingPlan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment4");
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Master.DsCourse != null)
        {
            dlstTeachingPlan.DataSource = Master.DsCourse.Tables[0];
            dlstTeachingPlan.DataBind();
        }   
    }
}