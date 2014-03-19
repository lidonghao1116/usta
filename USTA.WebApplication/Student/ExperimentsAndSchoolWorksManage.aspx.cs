using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Bll;
using USTA.Model;
using USTA.Common;

public partial class Student_ExperimentsAndSchoolWorksManage : System.Web.UI.Page
{
    public string courseNoTermTagClassID = HttpContext.Current.Request["courseNoTermTagClassID"] != null ? HttpContext.Current.Request["courseNoTermTagClassID"] : string.Empty;
    int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                , divFragment1, divFragment2, divFragment3);

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

            DalOperationAboutStudent dalstudentCourse = new DalOperationAboutStudent();
            DataTable dt = dalstudentCourse.GetallCourseByStudentNo(UserCookiesInfo.userNo).Tables[0];
            if (fragmentFlag.Equals("1"))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddltcourseSchoolwork.Items.Add(new ListItem(dt.Rows[i]["courseName"].ToString().Trim() + "(" + dt.Rows[i]["termTag"].ToString().Trim() + ")", dt.Rows[i]["courseNo"].ToString().Trim() + dt.Rows[i]["termTag"].ToString().Trim() + dt.Rows[i]["ClassID"].ToString().Trim()));
                }

                if (!string.IsNullOrEmpty(courseNoTermTagClassID))
                {
                    for (int i = 0; i < ddltcourseSchoolwork.Items.Count; i++)
                    {
                        if (ddltcourseSchoolwork.Items[i].Value == courseNoTermTagClassID)
                        {
                            ddltcourseSchoolwork.SelectedIndex = i;
                            ShoolworkBind(UserCookiesInfo.userNo, courseNoTermTagClassID);
                        }
                    }
                }
                else
                {
                    if (ddltcourseSchoolwork.Items.Count > 0)
                    {
                        ShoolworkBind(UserCookiesInfo.userNo, ddltcourseSchoolwork.SelectedValue);
                    }
                }
            }

            if (fragmentFlag.Equals("2"))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddltExperimentCourse.Items.Add(new ListItem(dt.Rows[i]["courseName"].ToString().Trim() + "(" + dt.Rows[i]["termTag"].ToString().Trim() + ")", dt.Rows[i]["courseNo"].ToString().Trim() + dt.Rows[i]["termTag"].ToString().Trim() + dt.Rows[i]["ClassID"].ToString().Trim()));
                }

                if (!string.IsNullOrEmpty(courseNoTermTagClassID))
                {
                    for (int i = 0; i < ddltExperimentCourse.Items.Count; i++)
                    {
                        if (ddltExperimentCourse.Items[i].Value == courseNoTermTagClassID)
                        {
                            ddltExperimentCourse.SelectedIndex = i;
                            ExperimentBind(UserCookiesInfo.userNo, courseNoTermTagClassID);
                        }
                    }
                }
                else
                {
                    if (ddltExperimentCourse.Items.Count > 0)
                    {
                        ExperimentBind(UserCookiesInfo.userNo, ddltExperimentCourse.SelectedValue);
                    }
                }
            }
        }
    }
    private void ShoolworkBind(string user, string courseNoTermTagClassID)
    {
        DalOperationAboutSchoolWorks dalw = new DalOperationAboutSchoolWorks();
        DataSet dsschoolwork = dalw.GetSchoolWorksByStudentNo(user, courseNoTermTagClassID);
        dlstMySchoolworks.DataSource = dsschoolwork.Tables[0];
        dlstMySchoolworks.DataBind();
    }
    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref iframeCount, false, string.Empty);
    }
    public string DeadLine(DateTime time)
    {
        string re = string.Empty;
        if (time.CompareTo(DateTime.Now) > 0)
        {
            re = "未截止";
        }
        else
        {
            re = "已过期";
        }
        return re;
    }
    public string isCheck(string check)
    {
        string re = string.Empty;
        if (check.Equals("True"))
        {
            re = "已批改";
        }
        else
        {
            re = "未批改";
        }
        return re;
    }
    private void ExperimentBind(string user, string courseNoTermTagClassID)
    {
        DalOperationAboutExperiment dale = new DalOperationAboutExperiment();
        DataSet dsexperiment = dale.GetExperimentByStudentNo(user, courseNoTermTagClassID);
        dlstMyExperiments.DataSource = dsexperiment.Tables[0];
        dlstMyExperiments.DataBind();
    }

    protected void ddltcourseSchoolwork_Click(object sender, EventArgs e)
    {
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        ShoolworkBind(UserCookiesInfo.userNo, ddltcourseSchoolwork.SelectedValue);
    }
    protected void ddltExperimentCourse_Click(object sender, EventArgs e)
    {
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        ExperimentBind(UserCookiesInfo.userNo, ddltExperimentCourse.SelectedValue);
    }
}
