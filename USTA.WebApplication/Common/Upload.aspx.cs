using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using USTA.PageBase;

public partial class Common_Upload : System.Web.UI.Page
{
    public string fileExtension = ConfigurationManager.AppSettings["fileExtension"];

    public string imageFileExtension = ConfigurationManager.AppSettings["imageFileExtension"];

    public string iframeId = HttpContext.Current.Request["iframeId"] == null ? string.Empty : HttpContext.Current.Request["iframeId"];


    public string hiddenId = HttpContext.Current.Request["hiddenId"] == null ? string.Empty : HttpContext.Current.Request["hiddenId"];


    protected void Page_PreLoad(object sender, EventArgs e)
    {
        CommonFunction.CheckUser();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
