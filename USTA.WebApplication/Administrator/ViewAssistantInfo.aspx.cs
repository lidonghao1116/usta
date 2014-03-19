using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using USTA.PageBase;

public partial class Administrator_ViewAssistantInfo : CheckUserWithCommonPageBase
{
    public string assistantNo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["assistantNo"] == null)
            {
                Javascript.GoHistory(-1, Page);
                return;
            }
            else
            {
                assistantNo = Request["assistantNo"].Trim();
                PageDataBind();
            }
        }
    }

    public void PageDataBind()
    {
        DalOperationUsers dou = new DalOperationUsers();
        AssistantsList assistant = dou.FindAssistantByNo(assistantNo);
        if (assistant == null)
        {
            Javascript.AlertAndRedirect("对不起，您要查看的助教不存在！", "/Administrator/AssistantManager.aspx", Page);
        }
        else
        {
            lblName.Text = assistant.assistantName;
            lblOffice.Text = assistant.officeAddress;
            lblEmail.Text = assistant.emailAddress;
            lblRemark.Text = assistant.remark;
        }
    }
}
