using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using USTA.Common;
using USTA.Dal;
using USTA.Bll;
using USTA.Model;
using System.Configuration;

namespace USTA.PageBase
{
    public sealed class CommonFunction
    {
        public CommonFunction()
        {

        }

        public static bool CheckUserIsLogin()
        {
            HttpContext Context = HttpContext.Current;

            return (Context.Session[ConfigurationManager.AppSettings["sessionKey"]] != null);
        }

        public static void CheckUser()
        {
            HttpContext Context = HttpContext.Current;
            //获取要访问的路径
            string path = Context.Request.Path.ToLower();

            if (Context.Session[ConfigurationManager.AppSettings["sessionKey"]] == null && (!path.StartsWith("/checkuser.aspx")))
            {
                CommonUtility.RedirectLoginUrl();
                return;
            }

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            
            if (path.StartsWith("/administrator"))
            {
                if (UserCookiesInfo.userType != 0)
                {
                    CommonUtility.RedirectLoginUrl();
                    return;
                }
            }

            if (path.StartsWith("/teacher"))
            {
                //复用Excel导出功能，使用的角色为：管理员、教师、助教
                if (path.StartsWith("/teacher/outputenglishexamsignupexcel.ashx"))
                {
                    if (!(UserCookiesInfo.userType == 0 || UserCookiesInfo.userType == 1 || UserCookiesInfo.userType == 2))
                    {
                        CommonUtility.RedirectLoginUrl();
                        return;
                    }
                }
                else
                {
                    if (UserCookiesInfo.userType != 1 && UserCookiesInfo.userType != 2)
                    {
                        CommonUtility.RedirectLoginUrl();
                        return;
                    }
                }
            }


            if (path.StartsWith("/student"))
            {
                if (UserCookiesInfo.userType != 3)
                {
                    CommonUtility.RedirectLoginUrl();
                    return;
                }
            }

            //Context.Response.Write(Context.Request.RawUrl.ToLower().IndexOf("javascript"));

            if ((Context.Request.RawUrl.ToLower().IndexOf("/teacher") == 0 || Context.Request.RawUrl.ToLower().IndexOf("/student") == 0) && Context.Request["courseNo"] != null && Context.Request["classID"] != null && Context.Request["termTag"] != null)
            {
                //Context.Response.Write(Context.Request["courseNo"]);
                string courseNo = Context.Request["courseNo"];
                string classID = Context.Server.UrlDecode(Context.Request["classID"]);
                string termTag = Context.Request["termTag"];

                DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();




                bool hasCourse = false;


                switch (UserCookiesInfo.userType)
                {
                    case 2:
                        //IsAssistantAtCourse有问题，需要改进，已经改进
                        hasCourse = DalOperationAboutCourses.IsAssistantAtCourse(UserCookiesInfo.userNo, courseNo, classID, termTag);
                        break;
                    case 1:
                        //IsTeacherAtCourse有问题，需要改进，已经改进
                        hasCourse = DalOperationAboutCourses.IsTeacherAtCourse(UserCookiesInfo.userNo, courseNo, classID, termTag);
                        break;
                    case 3:
                        hasCourse = DalOperationAboutCourses.IsStudentHasCourse(UserCookiesInfo.userNo, courseNo, classID, termTag);
                        break;
                    default:
                        break;
                }

                if (!hasCourse)
                {
                    CommonUtility.RedirectLoginUrl();
                    return;
                }


                if (Context.Request.RawUrl.ToLower().IndexOf("javascript:") != -1)
                {
                    //Context.Response.Write(Context.Request.RawUrl.ToLower().IndexOf("javascript:"));
                    CommonUtility.RedirectUrl();
                    return;
                }
            }
        }
    }
}
