using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using USTA.Bll;
using System.IO;
using System.Configuration;

namespace USTA.BaseHttpHandler
{
    /// <summary>
    /// BaseHttpHandlerControl类，用于处理特定文件类型
    /// </summary>
    public class BaseHttpHandlerControl : IHttpHandler
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context">当前上下文环境</param>
        public void ProcessRequest(HttpContext context)
        {
            //if (context.Request.Url.ToString().LastIndexOf("DataBaseBak", StringComparison.OrdinalIgnoreCase) != -1)
            //{
            //    if (BllOperationAboutUser.GetUserCookiesInfo().userType != 0)
            //    {
            //        context.Response.ContentType = "text/html";
            //        context.Response.WriteFile(ConfigurationManager.AppSettings["errorPage"]);
            //    }
            //    else
            //    {
            //        FileInfo DownloadFile = new FileInfo(context.Server.MapPath(context.Request.Url.AbsolutePath));

            //        context.Response.Clear();
            //        context.Response.ClearHeaders();
            //        context.Response.Buffer = false;
            //        context.Response.ContentType = "application/octet-stream";
            //        context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.FullName, System.Text.Encoding.ASCII));
            //        context.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            //        context.Response.WriteFile(DownloadFile.FullName);
            //        context.Response.Flush();
            //    }
            //}
        }

        /// <summary>
        /// 指明是否可重复使用
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}