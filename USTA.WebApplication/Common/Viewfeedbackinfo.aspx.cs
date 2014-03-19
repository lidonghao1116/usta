using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using USTA.Dal;
using USTA.Model;
using USTA.Common;
using USTA.Bll;
using System.Configuration;
using USTA.PageBase;

public partial class Common_Viewfeedbackinfo : CheckUserWithCommonPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int feedbackId = 0;
        if (CommonUtility.SafeCheckByParams<string>(Request["feedbackId"], ref feedbackId))
        {
            DalOperationFeedBack dal = new DalOperationFeedBack();
            dlstfeedback.DataSource = dal.GetFeedById(feedbackId).Tables[0];
            dlstfeedback.DataBind();
           // dal.UpdateFeedBackIsReadById(feedbackId);
        }
        else
        {
            Javascript.GoHistory(-1, Page);
        }
    }
}
