using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


using USTA.Dal;
using USTA.Common;
using USTA.Model;
using USTA.Bll;
using System.Collections;

public partial class MasterPage_BBSindex : System.Web.UI.MasterPage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
       

    }
    public void DataSolution()
    {
    }

    public void ShowLiControl(Page page, string ShowLiId)
    {
        HtmlControl liControl = (HtmlControl)page.Master.Master.FindControl("ContentPlaceHolder1").FindControl(ShowLiId);
        liControl.Attributes.Add("class", "ui-tabs-selected");
    }
}
