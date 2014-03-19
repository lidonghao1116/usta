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

public partial class Student_AddExperiment : CheckUserWithCommonPageBase
{
    int experimentResourceId = 0;

    public int fileFolderType = (int)FileFolderType.experiments;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        if (CommonUtility.SafeCheckByParams<string>(Request["experimentResourceId"], ref experimentResourceId))
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutExperimentResources DalOperationAboutExperimentResources = new DalOperationAboutExperimentResources();
            ExperimentResources experimentResources = DalOperationAboutExperimentResources.GetExperimentResourcesbyId(experimentResourceId);
        }
        else
        {
            Javascript.GoHistory(-1, Page);
        }
    }

    protected void submitExpriment_Click(object sender, EventArgs e)
    {
        
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutExperiment DalOperationAboutExperiment = new DalOperationAboutExperiment();
        
        Experiments experiment = new Experiments
        {
            studentNo = UserCookiesInfo.userNo,
            experimentResourceId = experimentResourceId,
            updateTime = DateTime.Now,
        };
        if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
        {
            experiment.attachmentId = hidAttachmentId.Value;
        }
        else
        {
            Javascript.Alert("请上传附件！", Page);
            return;
        }

        DalOperationAboutExperiment.SubmitExperiment(experiment);
        Javascript.RefreshParentWindow("提交成功！","haha", Page);

    }
}
