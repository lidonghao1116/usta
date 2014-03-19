using System;
using System.Web;
using System.IO;

namespace USTA.BaseHttpModule
{
    using USTA.Bll;
    using USTA.Dal;
    using USTA.Model;
    using USTA.Common;

    /// <summary>
    /// BaseHttpModuleControl类，用于处理需要在管道中完成的任务
    /// </summary>
    public class BaseHttpModuleControl : IHttpModule
    {

        #region IHttpModule 成员


        /// <summary>
        /// 资源清理
        /// </summary>
        public void Dispose()

        { }


        /// <summary>
        /// 初始化管道处理
        /// </summary>
        /// <param name="application">HttpApplication实例</param>
        public void Init(HttpApplication application)
        {

            //application.BeginRequest += new EventHandler(application_BeginRequest);

            //application.Error += new EventHandler(ShowError);

            //application.EndRequest += new EventHandler(application_EndRequest);

            //application.PreRequestHandlerExecute += new EventHandler(application_PreRequestHandlerExecute);

            //application.PostRequestHandlerExecute += new EventHandler(application_PostRequestHandlerExecute);

            //application.ReleaseRequestState += new EventHandler(application_ReleaseRequestState);

            //application.AcquireRequestState += new EventHandler(application_AcquireRequestState);

            //application.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);

            //application.AuthorizeRequest += new EventHandler(application_AuthorizeRequest);

            //application.ResolveRequestCache += new EventHandler(application_ResolveRequestCache);

            //application.PreSendRequestHeaders += new EventHandler(application_PreSendRequestHeaders);

            //application.PreSendRequestContent += new EventHandler(application_PreSendRequestContent);

        }

        //void ShowError(object sender, EventArgs e)
        //{

        //    Exception ex = HttpContext.Current.Server.GetLastError();
        //    HttpContext.Current.Response.Redirect("/fds.htm");
        //    if (ex is System.Web.HttpException)
        //    {
        //        HttpContext.Current.Response.Write("error<br/>");
        //    }


        //}

        //void application_PreSendRequestContent(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_PreSendRequestContent<br/>");

        //}



        //void application_PreSendRequestHeaders(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_PreSendRequestHeaders<br/>");

        //}



        //void application_ResolveRequestCache(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_ResolveRequestCache<br/>");

        //}



        //void application_AuthorizeRequest(object sender, EventArgs e)
        //{
        //HttpApplication application = (HttpApplication)sender;
        ////application.Context.Response.Write(application.Context.Request.RawUrl.ToLower().IndexOf("javascript"));

        //if ((application.Context.Request.RawUrl.ToLower().IndexOf("/teacher") == 0 || application.Context.Request.RawUrl.ToLower().IndexOf("/student") == 0) && application.Context.Request["courseNo"] != null && application.Context.Request["classID"] != null && application.Context.Request["termTag"] != null)
        //{
        //    //application.Context.Response.Write(application.Context.Request["courseNo"]);
        //    string courseNo = application.Context.Request["courseNo"];
        //    string classID = application.Server.UrlDecode(application.Context.Request["classID"]);
        //    string termTag = application.Context.Request["termTag"];

        //    DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();

        //    UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        //    bool hasCourse = false;


        //    switch (UserCookiesInfo.userType)
        //    {
        //        case 2:
        //            //IsAssistantAtCourse有问题，需要改进，已经改进
        //            hasCourse = DalOperationAboutCourses.IsAssistantAtCourse(UserCookiesInfo.userNo, courseNo,classID,termTag);
        //            break;
        //        case 1:
        //            //IsTeacherAtCourse有问题，需要改进，已经改进
        //            hasCourse = DalOperationAboutCourses.IsTeacherAtCourse(UserCookiesInfo.userNo, courseNo,classID,termTag);
        //            break;
        //        case 3:
        //            hasCourse = DalOperationAboutCourses.IsStudentHasCourse(UserCookiesInfo.userNo, courseNo, classID, termTag);
        //            break;
        //        default:
        //            break;
        //    }

        //    if (!hasCourse)
        //    {
        //        CommonUtility.RedirectUrl();
        //        return;
        //    }
        //application.Context.Response.Write("application_AuthorizeRequest<br/>");
        // }

        //application.Context.Response.Write("application_AuthorizeRequest<br/>");

        // }



        //void application_AuthenticateRequest(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_AuthenticateRequest<br/>");

        //}



        //void application_AcquireRequestState(object sender, EventArgs e)
        //{

        //HttpApplication application = (HttpApplication)sender;
        //if (application.Context.Request.Form != null)
        //{
        //    for (int i = 0; i < application.Context.Request.Form.Count; i++)
        //    {
        //        string key = application.Context.Request.Form.Keys[i];
        //        if (key == "__VIEWSTATE") continue;
        //        if (application.Context.Request.Form.Keys[i].ToLower().IndexOf("<script")!=-1)
        //        {
        //            application.Context.Response.Write("sql");

        //           // return;
        //        }
        //    }
        //}
        //application.Context.Response.Write("application_AcquireRequestState<br/>");

        //}



        //void application_ReleaseRequestState(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_ReleaseRequestState<br/>");

        //}



        //void application_PostRequestHandlerExecute(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_PostRequestHandlerExecute<br/>");

        //}



        //void application_PreRequestHandlerExecute(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_PreRequestHandlerExecute<br/>");

        //}



        //void application_EndRequest(object sender, EventArgs e)
        //{

        //    HttpApplication application = (HttpApplication)sender;

        //    application.Context.Response.Write("application_EndRequest<br/>");

        //}



        //void application_BeginRequest(object sender, EventArgs e)
        //{


        //HttpApplication application = (HttpApplication)sender;

        //HttpContext.Current.Response.Write(application.Context.Request.CurrentExecutionFilePath);
        //HttpContext.Current.Response.End();

        //if (application.Context.Request.PhysicalApplicationPath != "/")
        //{
        //    if (!File.Exists(application.Context.Request.PhysicalApplicationPath))
        //    {
        //        HttpContext.Current.Response.Redirect("/404.htm", true);
        //    }
        //}


        //if (application.Context.Request.RawUrl.ToLower().IndexOf("javascript:") != -1)
        //{
        //application.Context.Response.Write(application.Context.Request.RawUrl.ToLower().IndexOf("javascript:"));
        //  CommonUtility.RedirectUrl();
        //  return;
        // }
        //}

        #endregion

    }
}

