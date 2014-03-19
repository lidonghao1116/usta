using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Configuration;
using USTA.Common;

namespace USTA.PageBase
{
    /// <summary>
    /// 动态添加Css和Js文件的页面基类
    /// </summary>
    public class CommonPageBase : System.Web.UI.Page
    {
        /// <summary>
        /// PageBase初始化
        /// </summary>
        public CommonPageBase()
        {

        }
        /// <summary>
        /// 普通页面的初始化
        /// </summary>
        /// <param name="e">初始化相关参数</param>
        protected override void OnInit(EventArgs e)
        {
            CommonFunction.CheckUser();

            CommonUtility.AddCssAndJs(Page.Header, new string[] { ConfigurationManager.AppSettings["mainCSS"], 
            ConfigurationManager.AppSettings["commonJS"],
            ConfigurationManager.AppSettings["uploadJS"], ConfigurationManager.AppSettings["thickboxJS"]});
        }
    }
}
