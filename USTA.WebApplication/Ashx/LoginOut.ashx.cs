using System;
using System.Web;
using System.Configuration;
using System.Web.SessionState;

public class LoginOut : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        //System.Web.Security.FormsAuthentication.SignOut();
        context.Session[ConfigurationManager.AppSettings["sessionKey"]] = null;
        //UpdateRedirect
        context.Response.Redirect(ConfigurationManager.AppSettings["sseweb"], false);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}