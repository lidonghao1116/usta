using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using USTA.Dal;
using USTA.Common;
using System.Configuration;

namespace USTA.WebApplication
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            //在应用程序启动时运行的代码
            //RouteTable.Routes.AddCombresRoute("Combres");
        }

        void Application_End(object sender, EventArgs e)
        {
            //在应用程序关闭时运行的代码
        }

        void Application_Error(object sender, EventArgs e)
        {
            //Server.Transfer(ConfigurationManager.AppSettings["errorPage"]);
        }

        void Session_Start(object sender, EventArgs e)
        {
            //在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            //在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式 
            //设置为 StateServer 或 SQLServer，则不会引发该事件。

        }
    }
}
