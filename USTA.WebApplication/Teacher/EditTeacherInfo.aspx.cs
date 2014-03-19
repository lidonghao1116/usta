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
using System.Transactions;

public partial class Teacher_EditTeacherInfo : System.Web.UI.Page
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
            if (UserCookiesInfo.userType == 1 || UserCookiesInfo.userType == 2)
            {
                DalOperationAboutTeacher DalOperationAboutTeacher = new DalOperationAboutTeacher();
                TeachersList teacher = DalOperationAboutTeacher.GetTeacherById(UserCookiesInfo.userNo);
                ltlEmail.Text = teacher.emailAddress;
                txtOffice.Text = teacher.officeAddress;
                txtRemark.Text = teacher.remark;
            }
        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        if (UserCookiesInfo.userType == 1 || UserCookiesInfo.userType == 2)
        {
           
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    DalOperationAboutTeacher DalOperationAboutTeacher = new DalOperationAboutTeacher();
                    DalOperationAboutTeacher.UpdateEmail(UserCookiesInfo.userNo, CommonUtility.JavascriptStringFilter(ltlEmail.Text), CommonUtility.JavascriptStringFilter(txtOffice.Text), CommonUtility.JavascriptStringFilter(txtRemark.Text));
                    scope.Complete();       
                }
                catch (Exception ex)
                {
                   MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "修改失败,错误提示：您输入的Email重复,请重新输入后提交！", Page);
                }
                finally
                {
                   
                }
            }
        }
        Javascript.AlertAndRedirect("修改成功：）", "EditTeacherInfo.aspx", Page);
    }
}
