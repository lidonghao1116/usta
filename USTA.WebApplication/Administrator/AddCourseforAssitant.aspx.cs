using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.Bll;
using USTA.PageBase;
using System.Configuration;

public partial class Administrator_AddCourseforAssitant : CheckUserWithCommonPageBase
{
    public string assistantNo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["assistantNo"] != null && Request["courseNo"] != null && Request["classID"] != null && Request["termTag"] != null)
            {
                assistantNo = Request["assistantNo"];
                String courseNo = Request["courseNo"].ToString().Trim();
                String ClassID = Server.UrlDecode(Request["classID"].ToString().Trim());
                String termTag = Request["termTag"].ToString().Trim();

                DalOperationAboutAssistant DalOperationAboutAssistant = new DalOperationAboutAssistant();
                DalOperationAboutAssistant.AddCourseOfAssistant(assistantNo, courseNo, ClassID, termTag);
                Javascript.RefreshParentWindow("AssistantManager.aspx?fragment=4&assistantNo=" + assistantNo, Page);
                return;
            }
            if (Request["assistantNo"] != null)
            {
                assistantNo = Request["assistantNo"];
                DalOperationAboutAssistant DalOperationAboutAssistant = new DalOperationAboutAssistant();
                DataSet ds = DalOperationAboutAssistant.GetOtherCoursesByAssistantNo(assistantNo);

                dlstCourses.DataSource = ds.Tables[0];
                dlstCourses.DataBind();
            }
            else
            {
                Javascript.GoHistory(-1, Page);
            }
        }
    }
}
