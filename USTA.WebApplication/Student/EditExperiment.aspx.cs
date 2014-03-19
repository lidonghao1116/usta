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

public partial class Student_EditExperiment : CheckUserWithCommonPageBase
{
    public Experiments experiment;
    int experimentId;
    public int fileFolderType = (int)FileFolderType.experiments;
    int experimentResourcesIdt;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
            experimentId = 0;
            if (CommonUtility.SafeCheckByParams<string>(Request["experimentId"], ref experimentId))
            {
                DalOperationAboutExperiment dalOperationAboutExperiment = new DalOperationAboutExperiment();
                experiment = dalOperationAboutExperiment.GetExperimentById(experimentId);
                experimentResourcesIdt = experiment.experimentResourceId;
            }
            else
            {
                Javascript.GoHistory(-1, Page);
            }
        
    }
    protected void commit_Click(object sender, EventArgs e)
    {
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

        Experiments experiments = new Experiments
        {
            experimentId = experimentId,
            updateTime=DateTime.Now,
            experimentResourceId = experimentResourcesIdt,
            studentNo=UserCookiesInfo.userNo,
            isCheck=false,
            isExcellent=false,
            remark="",
            checkTime=DateTime.Now,
            excellentTime=DateTime.Now

        };

        if (hidAttachmentId.Value.Length != 0)
        {
            experiments.attachmentId = hidAttachmentId.Value;
        }
        else
        {
            Javascript.Alert("请上传附件！", Page);
            return;
        }
        DalOperationAboutExperiment dalOperationAboutExperiment=new DalOperationAboutExperiment();
         
        dalOperationAboutExperiment.UpdateExperiment(experiments);
        Javascript.RefreshParentWindow("修改成功！", "CInfoExperiment.aspx?experimentResourceId=" + experiments.experimentResourceId + "courseNo=" + Request["courseNo"], Page);
         
    }
}
