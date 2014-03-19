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
using USTA.PageBase;

public partial class Student_EditStudentinfo : CheckUserWithCommonPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                , divFragment1, divFragment2, divFragment3);


            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutStudent dalOperationAboutStudent = new DalOperationAboutStudent();
            StudentsList student = dalOperationAboutStudent.GetStudentById(UserCookiesInfo.userNo);
            ltlEmail.Text = student.emailAddress;
            ltlPhone.Text = student.mobileNo;
            ltlRemark.Text = student.remark.Trim().Length == 0 ? "暂无" : student.remark.Trim();
        }
    }
}