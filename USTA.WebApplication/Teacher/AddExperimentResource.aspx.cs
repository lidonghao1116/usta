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
using System.Transactions;


using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

public partial class Teacher_AddExperimentResource : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.experimentResources;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
    }
    protected void btnAddExperimentResource_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Length != 0)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DateTime deadline_t = Convert.ToDateTime(datepicker.Value);
            if (deadline_t.CompareTo(DateTime.Now) > 0)
            {
                ExperimentResources ExperimentResources = new ExperimentResources
                {
                    courseNo = Request["courseNo"].Trim(),
                    classID = Server.UrlDecode(Request["classID"].Trim()),
                    termTag = Request["termtag"].Trim(),
                    experimentResourceTitle = CommonUtility.JavascriptStringFilter(txtTitle.Text),
                    experimentResourceContent = Textarea1.Value,
                    deadLine = deadline_t,
                    updateTime = DateTime.Now,
                    attachmentIds = hidAttachmentId.Value
                };
                //添加课程实验并返回作业的自增长主键

                using (TransactionScope scope = new TransactionScope())
                {
                    DalOperationAboutExperimentResources DalOperationAboutExperimentResources = new DalOperationAboutExperimentResources();
                    ExperimentResources = DalOperationAboutExperimentResources.InsertExperimentResources(ExperimentResources);

                    //添加了课程实验以后,往提交表中添加记录               

                    //查询出选课学生记录
                    DalOperationAboutExperiment doae = new DalOperationAboutExperiment();
                    DalOperationAboutCourses dalOperationAboutCourses = new DalOperationAboutCourses();
                    DataTable dt = dalOperationAboutCourses.FindStudentInfoByCourseNo(Request["courseNo"].ToString().Trim(),Server.UrlDecode(Request["classID"]),Request["termtag"]).Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Experiments experiment = new Experiments
                        {
                            experimentResourceId = ExperimentResources.experimentResourceId,
                            studentNo = dt.Rows[i]["studentNo"].ToString().Trim(),
                            updateTime = DateTime.Now,
                            checkTime = DateTime.Now,
                            isExcellent = false,
                            remark = "",
                            excellentTime = DateTime.Now,
                            attachmentId = "0"
                        };
                        doae.InsertExperiment(experiment);
                    }
                    scope.Complete();
                }
                Javascript.RefreshParentWindow("添加成功!", "CInfoExperimentResource.aspx?courseNo=" + Request["courseNo"]+"&classID="+Server.UrlEncode(Server.UrlDecode(Request["classID"]))+"&termtag="+Request["termtag"], Page);
                
            }
            else
            {
                Javascript.Alert("截止时间不能在当前时间之前", Page);
            }
        }
        else
        {
            Javascript.Alert("标题不能为空！", Page);
        }
    }
}
