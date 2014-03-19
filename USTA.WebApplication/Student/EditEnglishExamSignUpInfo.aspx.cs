using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Student
{
    public partial class EditEnglishExamSignUpInfo : CheckUserWithCommonPageBase
    {
        int englishExamId = (HttpContext.Current.Request["englishExamId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["englishExamId"]);
        int englishExamNotifyId = (HttpContext.Current.Request["englishExamNotifyId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["englishExamNotifyId"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tryParseInt = 0;
                if (CommonUtility.SafeCheckByParams<string>(Request["englishExamId"], ref tryParseInt))
                {
                    //获取Url中的参数
                    englishExamId = tryParseInt;
                }
                if (CommonUtility.SafeCheckByParams<string>(Request["englishExamNotifyId"], ref tryParseInt))
                {
                    //获取Url中的参数
                    englishExamNotifyId = tryParseInt;
                }
            }
            InitialNotifyEdit(englishExamId);
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int englishExamId)
        {

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutEnglishExam dal = new DalOperationAboutEnglishExam();

            if (dal.CheckIsOverDateOrSignUpConfirm(UserCookiesInfo.userNo, englishExamId))
            {
                Javascript.AlertAndRedirect("您好，当前报名信息不能修改，可能的原因为：\n1. 已经过报名截止日期。2. 报名信息已经确认。\n如有疑问请发送疑问反馈，谢谢！", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
                return;
            }

            DataSet ds = dal.GetEnglishExamNotifyIngById(englishExamNotifyId);
            DataRowCollection drc = ds.Tables[0].Rows;

            if (drc.Count == 0)
            {
                Javascript.RefreshParentWindow("您好，当前无四六级报名信息！", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
                return;
            }

            for (int i = 0; i < 1; i++)
            {
                ddlEnglishExamNotify.Items.Add(new ListItem(drc[i]["englishExamNotifyTitle"].ToString().Trim(), drc[i]["englishExamNotifyId"].ToString().Trim()));
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

            EnglishExam englishExam =  dal.GetEnglishExamSignUpInfoByStudentNo(UserCookiesInfo.userNo, englishExamId);

            if (englishExam.examType.Trim() == "四级")
            {
                ddlEnglishExamType.Items.Add(new ListItem("四级", "四级"));
                ddlEnglishExamType.Items.Add(new ListItem("六级", "六级"));
            }
            else if (englishExam.examType.Trim() == "六级")
            {
                ddlEnglishExamType.Items.Add(new ListItem("六级", "六级"));
                ddlEnglishExamType.Items.Add(new ListItem("四级", "四级"));
            }


            if (englishExam.examPlace.Trim() == "合肥")
            {
                ddlEnglishExamPlace.Items.Add(new ListItem("合肥", "合肥"));
                ddlEnglishExamPlace.Items.Add(new ListItem("苏州", "苏州"));
            }
            else if (englishExam.examPlace.Trim() == "苏州")
            {
                ddlEnglishExamPlace.Items.Add(new ListItem("苏州", "苏州"));
                ddlEnglishExamPlace.Items.Add(new ListItem("合肥", "合肥"));
            }
        }
        //修改
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
                UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

                if (dalOperationAboutEnglishExam.CheckIsOverDateOrSignUpConfirm(user.userNo, englishExamId))
                {
                    Javascript.AlertAndRedirect("您好，当前报名信息不能修改，可能的原因为：\n1. 已经过报名截止日期。2. 报名信息已经确认。\n如有疑问请发送疑问反馈，谢谢！", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
                    return;
                }

                dalOperationAboutEnglishExam.UpdateEnglishExamSignUp(user.userNo, ddlEnglishExamPlace.SelectedValue, ddlEnglishExamType.SelectedValue
                    , englishExamId); //保存反馈意见
                Javascript.RefreshParentWindow("修改报名信息成功！", "/Student/EnglishExamSignUp.aspx?fragment=2", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "修改报名信息失败,请检查格式是否有误！", Page);
            }
        }
    }
}