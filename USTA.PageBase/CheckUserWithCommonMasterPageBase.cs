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
    public class CheckUserWithCommonMasterPageBase : System.Web.UI.MasterPage
    {
        /// <summary>
        /// PageBase初始化
        /// </summary>
        public CheckUserWithCommonMasterPageBase()
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
