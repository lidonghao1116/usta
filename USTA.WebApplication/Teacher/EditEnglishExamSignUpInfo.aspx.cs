using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using USTA.Bll;
using USTA.PageBase;

namespace USTA.WebApplication.Teacher
{
    public partial class EditEnglishExamSignUpInfo : CheckUserWithCommonPageBase
    {
        int englishExamId = (HttpContext.Current.Request["englishExamId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["englishExamId"]);
        int englishExamNotifyId = (HttpContext.Current.Request["englishExamNotifyId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["englishExamNotifyId"]);
        string studentNo = (HttpContext.Current.Request["studentNo"] == null) ? "00000000" : HttpContext.Current.Request["studentNo"];

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
                InitialNotifyEdit(englishExamId);
            }
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int englishExamId)
        {
            DalOperationAboutEnglishExam dal = new DalOperationAboutEnglishExam();

            DataSet ds = dal.GetEnglishExamNotifyIngById(englishExamNotifyId);
            DataRowCollection drc = ds.Tables[0].Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                ddlEnglishExamNotify.Items.Add(new ListItem(drc[i]["englishExamNotifyTitle"].ToString().Trim(), drc[i]["englishExamNotifyId"].ToString().Trim()));
            }
            StudentsList studentList = dal.GetEnglishExamSignUpStudentInfoByStudentNo(studentNo);

            ltlName.Text = studentList.studentName;
            ltlSex.Text = (studentList.Sex == 1 ? "女" : "男");
            ltlStudentNo.Text = studentList.studentNo;
            ltlCardType.Text = studentList.CardType;
            ltlCardNum.Text = studentList.CardNum;
            ltlMatriculationDate.Text = studentList.MatriculationDate.ToString("yyyy-MM-dd");
            ltlMajor.Text = studentList.studentSpeciality;
            ltlSchoolClass.Text = studentList.SchoolClassName;

            EnglishExam englishExam = dal.GetEnglishExamSignUpInfoByStudentNo(studentNo, englishExamId);

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

            if (englishExam.englishExamSignUpConfirm == 1)
            {
                btnConfirm.Text = "取消确认报名信息";
                btnConfirm.Attributes.Add("onclick", "return confirm('是否取消确认报名信息？')");
                ddlEnglishExamType.Attributes.Add("disabled", "disabled");
                ddlEnglishExamPlace.Attributes.Add("disabled", "disabled");
            }
            else
            {
                btnConfirm.Attributes.Add("onclick", "return confirm('是否确认报名信息（包括基本信息、考试类型、考试地点等）？')");
            }
            hidExamType.Value = englishExam.examType;
            hidExamPlace.Value = englishExam.examPlace;
        }
        //修改
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();

            dalOperationAboutEnglishExam.ConfirmOrCancelSignUpInfo(englishExamId.ToString(), studentNo, ddlEnglishExamType.SelectedValue, ddlEnglishExamPlace.SelectedValue, (btnConfirm.Text == "确认报名信息" ? false : true));
            Javascript.RefreshParentWindow((btnConfirm.Text == "确认报名信息" ? "确认报名信息成功！" : "取消确认报名信息成功！"), "/Teacher/EnglishExamManage.aspx?fragment=1", Page);
        }
    }
}