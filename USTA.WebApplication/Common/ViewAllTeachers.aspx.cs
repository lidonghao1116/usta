using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;

public partial class Common_ViewAllTeachers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //TeacherDataBind("");
        }
    }

    protected void TeacherDataBind(string s)
    {
        //DalOperationUsers daluser = new DalOperationUsers();
        //DataSet ds = daluser.SearchTeacherAndAssitant(s);
        //dlstTeacher.DataSource = ds.Tables[0];
        //dlstTeacher.DataBind();
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //TeacherDataBind(CommonUtility.JavascriptStringFilter(txtSearchstring.Text));
        //fieldsetTeacher.Visible = true;
        //legendTeacher.InnerText = "查找结果";
    }
}
