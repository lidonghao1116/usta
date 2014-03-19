using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Configuration;
using USTA.Dal;
using USTA.Model;
using USTA.Bll;
using USTA.Common;
using USTA.Cache;
using System.Web.Security;
using System.Web;

namespace USTA.WebServices
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WebServicesAboutUsers
    { 
        #region  添加主题
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public int CheckLogin(string userName,string userPwd)
        {
            int isLoginSuccess = 0;

            DalOperationAboutUser DalOperationAboutUser = new DalOperationAboutUser();

            //0:管理员;1:教师;2:助教;3:学生;
            CheckUserLogin CheckUserLoginAdmin = BllOperationAboutUser.ReturnSqlJudgeByUserType(0, userName, CommonUtility.EncodeUsingMD5(userPwd));
            CheckUserLogin CheckUserLoginTeacher = BllOperationAboutUser.ReturnSqlJudgeByUserType(1, userName, CommonUtility.EncodeUsingMD5(userPwd));
            CheckUserLogin CheckUserLoginAssistant = BllOperationAboutUser.ReturnSqlJudgeByUserType(2, userName, CommonUtility.EncodeUsingMD5(userPwd));
            CheckUserLogin CheckUserLoginStudent = BllOperationAboutUser.ReturnSqlJudgeByUserType(3, userName, CommonUtility.EncodeUsingMD5(userPwd));

            UserCookiesInfo UserCookiesInfoAdmin = DalOperationAboutUser.CheckUserLogin(CheckUserLoginAdmin);
            UserCookiesInfo UserCookiesInfoTeacher = DalOperationAboutUser.CheckUserLogin(CheckUserLoginTeacher);
            UserCookiesInfo UserCookiesInfoAssistant = DalOperationAboutUser.CheckUserLogin(CheckUserLoginAssistant);
            UserCookiesInfo UserCookiesInfoStudent = DalOperationAboutUser.CheckUserLogin(CheckUserLoginStudent);

            UserCookiesInfo UserCookiesInformation = null;//登陆的用户Cookie对象

            if (UserCookiesInfoAdmin != null)
            {
                UserCookiesInformation = UserCookiesInfoAdmin;
            }
            else if (UserCookiesInfoTeacher != null)
            {
                UserCookiesInformation = UserCookiesInfoTeacher;
            }
            else if (UserCookiesInfoAssistant != null)
            {
                UserCookiesInformation = UserCookiesInfoAssistant;
            }
            else if (UserCookiesInfoStudent != null)
            {
                UserCookiesInformation = UserCookiesInfoStudent;
            }

            if (UserCookiesInformation != null)
            {
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(3, UserCookiesInformation.userType.ToString().Trim(), DateTime.Now, DateTime.Now.AddMinutes(1440), false, SerializeCookies.SerializeCookiesMethod<UserCookiesInfo>(UserCookiesInformation)); //建立身份验证票对象
                string HashTicket = FormsAuthentication.Encrypt(Ticket); //加密序列化验证票为字符串
                HttpCookie UserCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HashTicket);
                UserCookie.HttpOnly = true;
                //生成Cookie
                HttpContext.Current.Response.Cookies.Add(UserCookie); //输出Cookie
                //// 定位到管理页面
                //HttpContext.Current.Response.Redirect("/Common/NotifyList.aspx", false);
                isLoginSuccess = 1;
            }
            return isLoginSuccess;
        }
        #endregion
    }
}
