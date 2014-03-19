using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Model;
using USTA.Common;

public partial class Teacher_VSWorkNWorkReq: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment1");    
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Master.DsSchoolNotify!= null)
        {
            dlstSchoolworkNotify.DataSource = Master.DsSchoolNotify.Tables[0];
            dlstSchoolworkNotify.DataBind();
        }      
    }

    //获取附件
    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false,string.Empty);
    }

}