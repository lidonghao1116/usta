using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using System.Data;

namespace USTA.WebApplication.Student
{
    public partial class EnglishExamSignUp : System.Web.UI.Page
    {
        public string fragmentFlag = (HttpContext.Current.Request["fragmentFlag"] != null ? HttpContext.Current.Request["fragmentFlag"] : "1");
        protected void Page_Load(object sender, EventArgs e)
        {
            //控制Tab的显示


            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                , divFragment1, divFragment2, divFragment3);

            if (!IsPostBack)
            {
                if (fragmentFlag == "1")
                {
                    DalOperationAboutEnglishExam dal = new DalOperationAboutEnglishExam();
                    DataTable _dt = dal.GetLocaleByStudentNo(UserCookiesInfo.userNo).Tables[0];

                    DataSet ds = dal.GetEnglishExamNotifyByLocale(_dt.Rows.Count > 0 ? _dt.Rows[0]["locale"].ToString().Trim() : string.Empty);
                    DataRowCollection drc = ds.Tables[0].Rows;
                    
                    if (drc.Count == 0)
                    {
                        Javascript.AlertAndRedirect("您好，当前无四六级报名信息！", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
                        return;
                    }

                    for (int i = 0; i < 1; i++)
                    {
                        ddlEnglishExamNotify.Items.Add(new ListItem(drc[i]["englishExamNotifyTitle"].ToString().Trim(), drc[i]["englishExamNotifyId"].ToString().Trim()));
                    }

                    if (dal.CheckHasSignUpInfo(UserCookiesInfo.userNo, int.Parse(ddlEnglishExamNotify.SelectedValue)) > 0)
                    {
                        Javascript.AlertAndRedirect("您好，当前已经报名，点击确定查看报名信息", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
                        return;
                    }

                    StudentsList studentList = dal.GetEnglishExamSignUpStudentInfoByStudentNo(UserCookiesInfo.userNo);

                    ltlName.Text = studentList.studentName;
                    ltlSex.Text = (studentList.Sex == 1 ? "女" : "男");
                    ltlStudentNo.Text = studentList.studentNo;
                    ltlCardType.Text = studentList.CardType;
                    ltlCardNum.Text = studentList.CardNum;
                    ltlMatriculationDate.Text = studentList.MatriculationDate.ToString("yyyy-MM-dd");
                    ltlMajor.Text = studentList.studentSpeciality;
                    ltlSchoolClass.Text = studentList.SchoolClassName;

                }

                if (fragmentFlag == "2")
                {
                    DalOperationAboutEnglishExam dal = new DalOperationAboutEnglishExam();
                    dlstEnglishExamSignUp.DataSource = dal.GetAllEnglishExamSignUpInfoByStudentNo(UserCookiesInfo.userNo);
                    dlstEnglishExamSignUp.DataBind();
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
                UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

                if (dalOperationAboutEnglishExam.CheckHasSignUpInfo(user.userNo,int.Parse(ddlEnglishExamNotify.SelectedValue)) > 0)
                {
                    Javascript.AlertAndRedirect("您好，当前已经报名，点击确定查看报名信息", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
                    return;
                }

                dalOperationAboutEnglishExam.AddEnglishExamSignUp(user.userNo, ddlEnglishExamPlace.SelectedValue, ddlEnglishExamType.SelectedValue
                    , int.Parse(ddlEnglishExamNotify.SelectedValue)); //保存反馈意见
                Javascript.AlertAndRedirect("报名成功！", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "报名失败,请检查格式是否有误！", Page);
            }
        }
    }
}