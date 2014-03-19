using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

using USTA.Common;
using USTA.PageBase;

// USTA.WebServices命名空间
namespace USTA.WebServices
{
    /// <summary>
    ///WebServicesAboutGetTimeStamp 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
    // [System.Web.Script.Services.ScriptService]
    public class WebServicesAboutGetTimeStamp : System.Web.Services.WebService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WebServicesAboutGetTimeStamp()
        {

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }

        /// <summary>
        /// 跨域获取教学周显示信息
        /// </summary>
        /// <returns>返回正则匹配后的教学周信息字符串</returns>
        [WebMethod(EnableSession = true)]
        public string GetTimeStamp()
        {
            if (!CommonFunction.CheckUserIsLogin())
            {
                return string.Empty;
            }

            string timeStamp = ConfigurationManager.AppSettings["jwcUrl"];
            try
            {
                HttpWebRequest wrequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["jwcUrl"]);

                HttpWebResponse wresponse = (HttpWebResponse)wrequest.GetResponse();

                StreamReader sr = new StreamReader(wresponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));

                Regex reg = new Regex("<div id=\"time_stamp\">.*?</div>");
                timeStamp = reg.Match(sr.ReadToEnd()).ToString().Replace("<div id=\"time_stamp\">", "").Replace("</div>", "").Replace("<em>", "").Replace("</em>", "");
                return timeStamp;
            }
            catch (Exception ex)
            {
                return timeStamp;
            }
        }

    }
}