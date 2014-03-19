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
using System.Transactions;
using USTA.PageBase;

public partial class Teacher_AddSchoolworkNotify : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.schoolWorks;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
    }

    protected void btnAddSchoolworkNotify_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Length != 0)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DateTime deadline_t = Convert.ToDateTime(datepicker.Value);
            if (deadline_t.CompareTo(DateTime.Now) > 0)
            {
                SchoolWorkNotify SchoolWorkNotify = new SchoolWorkNotify
                {
                    schoolWorkNotifyTitle=CommonUtility.JavascriptStringFilter(txtTitle.Text),
                    updateTime=DateTime.Now,
                    schoolWorkNotifyContent = Textarea1.Value,
                    deadline=deadline_t,
                    courseNo = Request["courseNo"],
                    isOnline = Convert.ToBoolean(ddltOnline.SelectedValue),
                    attachmentIds = hidAttachmentId.Value,
                    classID=Server.UrlDecode(Request["classID"].Trim()),
                    termTag=Request["termtag"].Trim()
                    
                };

                using (TransactionScope scope = new TransactionScope())
                {

                    try
                    {
                        //添加课程作业并返回作业的自增长主键
                        DalOperationAboutCourses dalOperationAboutCourses = new DalOperationAboutCourses();
                        SchoolWorkNotify = dalOperationAboutCourses.AddSchoolworkNotify(SchoolWorkNotify);

                        //添加了课程作业以后,往作业提交表中添加记录
                        DalOperationAboutSchoolWorks dalOperationAboutSchoolworks = new DalOperationAboutSchoolWorks();


                        //查询出选课学生记录
                        DataTable dt = dalOperationAboutCourses.FindStudentInfoByCourseNo(Request["courseNo"].ToString().Trim(),Server.UrlDecode(Request["classID"]),Request["termtag"]).Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SchoolWorks schoolwork = new SchoolWorks
                            {
                                updateTime = DateTime.Now,
                                schoolWorkNofityId = SchoolWorkNotify.schoolWorkNotifyId,
                                studentNo = dt.Rows[i]["studentNo"].ToString().Trim(),
                                isCheck = false,
                                isExcellent = false,
                                remark = "",
                                checkTime = DateTime.Now,
                                excellentTime = DateTime.Now,
                                attachmentId = "0"
                                
                            };
                            dalOperationAboutSchoolworks.AddSchoolWork(schoolwork);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        MongoDBLog.LogRecord(ex);
                        CommonUtility.RedirectUrl();
                    }
                }
                Javascript.RefreshParentWindow(Page);
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
