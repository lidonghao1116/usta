using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;

public partial class Student_ViewStudentInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutStudent dalOperationAboutStudent = new DalOperationAboutStudent();
            StudentsList student = dalOperationAboutStudent.GetStudentById(UserCookiesInfo.userNo);
            this.spanPhone.InnerText = student.mobileNo;
            this.spanEmail.InnerText = student.emailAddress;
        }
    }
}
