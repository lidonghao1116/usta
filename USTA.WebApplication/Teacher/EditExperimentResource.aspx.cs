using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

public partial class Teacher_EditExperimentResource : CheckUserWithCommonPageBase
{
    ExperimentResources experimentResources;

    public int fileFolderType = (int)FileFolderType.experimentResources;

    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    int experimentResourceId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (CommonUtility.SafeCheckByParams<string>(Request["experimentResourceId"], ref experimentResourceId))
            {
                if (Request["courseNo"] == null)
                {
                    Javascript.GoHistory(-1, Page);
                    return;
                }
                DalOperationAboutExperimentResources DalOperationAboutExperimentResources = new DalOperationAboutExperimentResources();
                experimentResources = DalOperationAboutExperimentResources.GetExperimentResourcesbyId(experimentResourceId);
                txtTitle.Text = experimentResources.experimentResourceTitle;
                Textarea1.Value = experimentResources.experimentResourceContent;
                datepicker.Value = experimentResources.deadLine.ToString("yyyy-MM-dd HH:mm:ss");


                hidAttachmentId.Value = experimentResources.attachmentIds;
                if (experimentResources.attachmentIds.Length > 0)
                {
                    DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                    ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(experimentResources.attachmentIds, ref iframeCount, true,string.Empty);
                }
            }
        }
    }


    protected void btnEditExperimentResource_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Length != 0)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DateTime deadline_t = Convert.ToDateTime(datepicker.Value);

            ExperimentResources ExperimentResources = new ExperimentResources
            {
                courseNo = Request["courseNo"],
                experimentResourceTitle = CommonUtility.JavascriptStringFilter(txtTitle.Text),
                experimentResourceContent = Textarea1.Value,
                deadLine = deadline_t,
                updateTime = DateTime.Now,
                attachmentIds = hidAttachmentId.Value,
                experimentResourceId = (Request["experimentResourceId"] != null ? int.Parse(Request["experimentResourceId"]) : 0)

            };

            DalOperationAboutExperimentResources DalOperationAboutExperimentResources = new DalOperationAboutExperimentResources();
            DalOperationAboutExperimentResources.UpdateExperimentResources(ExperimentResources);
            Javascript.RefreshParentWindow("修改成功!", "CInfoExperimentResource.aspx?courseNo=" + ExperimentResources.courseNo, Page);
        }
        else
        {
            Javascript.Alert("标题不能为空！", Page);
        }
    }
}
