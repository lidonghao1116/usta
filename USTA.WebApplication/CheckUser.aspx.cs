using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Dal;
using USTA.Model;
using USTA.Common;
using USTA.Bll;
using System.Web.Security;
using System.Configuration;

public partial class CheckUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if((Request.UrlReferrer!=null) && Request.UrlReferrer.ToString().ToLower().IndexOf("enroll.sse.ustc.edu.cn")!=-1 && Request.UrlReferrer.ToString().ToLower().IndexOf("sseweb")!=-1)
        //{
        string USID = string.Empty;
        if (Request["USID"] == null)
        {
            Response.Redirect("http://enroll.sse.ustc.edu.cn/sseweb/", false);
            return;
        }

        USID = Request["USID"].Trim();

        DalOperationAboutUser DalOperationAboutUser = new DalOperationAboutUser();

        UserCookiesInfo UserCookiesInfoTeacher = null;
        UserCookiesInfo UserCookiesInfoStudent = null;


        CheckUserLogin CheckUserLoginTeacher = BllOperationAboutUser.ReturnSqlJudgeByUserType(1, USID);
        UserCookiesInfoTeacher = DalOperationAboutUser.CheckUserLogin(CheckUserLoginTeacher);

        CheckUserLogin CheckUserLoginStudent = BllOperationAboutUser.ReturnSqlJudgeByUserType(3, USID);
        UserCookiesInfoStudent = DalOperationAboutUser.CheckUserLogin(CheckUserLoginStudent);

        //1:教师(isAssistant值为1表示有助教身份，isAdmin值为1表示为管理员);3:学生;

        UserCookiesInfo UserCookiesInformation = null;//登陆的用户Cookie对象

        if (UserCookiesInfoTeacher != null)
        {
            UserCookiesInformation = UserCookiesInfoTeacher;
        }
        else if (UserCookiesInfoStudent != null)
        {
            UserCookiesInformation = UserCookiesInfoStudent;
        }

        if (UserCookiesInformation != null)
        {
            //FormsAuthenticationTicket Ticket;

            //Ticket = new FormsAuthenticationTicket(3, UserCookiesInformation.userType.ToString().Trim(), DateTime.Now, DateTime.Now.AddMinutes(1440), false, SerializeCookies.SerializeCookiesMethod<UserCookiesInfo>(UserCookiesInformation)); //建立身份验证票对象

            Session[ConfigurationManager.AppSettings["sessionKey"]] = SerializeCookies.SerializeCookiesMethod<UserCookiesInfo>(UserCookiesInformation);

            //string HashTicket = FormsAuthentication.Encrypt(Ticket); //加密序列化验证票为字符串

            //HttpCookie UserCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HashTicket);
            //UserCookie.HttpOnly = true;
            ////生成Cookie
            //Context.Response.Cookies.Add(UserCookie); //输出Cookie
            // 定位到管理页面
            Response.Redirect("/Common/NotifyList.aspx", false);
        }
        else
        {
            Javascript.AlertAndRedirect("用户名或密码错误！请重新填写再登陆！","http://enroll.sse.ustc.edu.cn/sseweb/", Page);
        }
    }
}