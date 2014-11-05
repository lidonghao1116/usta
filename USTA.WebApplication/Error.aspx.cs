using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.PageBase;

namespace USTA.WebApplication.Common
{
    public partial class Error : System.Web.UI.Page
    {
        public string errorUri = (HttpContext.Current.Request.Url != null ? HttpContext.Current.Request.Url.ToString() : "/");

        protected void Page_Load(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            //记录异常信息
            if (ex != null)
            {
                MongoDBLog.LogRecord(ex);
            }
            Server.ClearError();
        }
    }
}