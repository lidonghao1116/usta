using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Model;
using USTA.Common;

public partial class Teacher_VExpRExperimentReq : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment1");
        if (CommonUtility.SafeCheckByParams<string>(Request["experimentResourceId"], ref Master.experimentResourceId))
        {
            //提取实验信息(experimentResourceId)
            DalOperationAboutExperimentResources DalOperationAboutExperimentResources = new DalOperationAboutExperimentResources();
            DataSet dsExperimentResources = DalOperationAboutExperimentResources.GetExperimentResourcesById(Master.experimentResourceId);

            dlstExperimentResource.DataSource = dsExperimentResources.Tables[0];
            dlstExperimentResource.DataBind();
        }

        else
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        
    }

    //获取附件
    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false, string.Empty);
    }
}