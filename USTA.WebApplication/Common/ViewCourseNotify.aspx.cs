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

public partial class Common_ViewCourseNotify : CheckUserWithCommonPageBase
{
    public CoursesNotifyInfo CoursesNotifyInfo;

    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        int courseNotifyId = -1;
        if (CommonUtility.SafeCheckByParams<string>(Request["courseNotifyId"], ref courseNotifyId))
        {
            DalOperationAboutCourseNotifyInfo DalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
            
            
            DataSet ds = DalOperationAboutCourseNotifyInfo.GetCourseNotifyInfobyId(courseNotifyId);
            dlstcNotify.DataSource = ds.Tables[0];
            dlstcNotify.DataBind();
             
            DalOperationAboutCourseNotifyInfo.AddScanCount(courseNotifyId);

        }
        else
        {
            Javascript.GoHistory(-1, Page);
        }

    }

    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref iframeCount, false,string.Empty);
    }
}
