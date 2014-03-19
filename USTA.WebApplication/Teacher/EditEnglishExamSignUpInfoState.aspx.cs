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
    public partial class EditEnglishExamSignUpInfoState : CheckUserWithCommonPageBase
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


            EnglishExam englishExam = dal.GetEnglishExamSignUpInfoByStudentNo(studentNo, englishExamId);

            //txtIsPaid.Text = englishExam.isPaid.ToString();
            txtIsPaidRemark.Text = englishExam.isPaidRemark;
            //txtExamCertificateState.Text = englishExam.examCertificateState;
            txtExamCertificateStateRemark.Text = englishExam.examCertificateRemark;
            txtGrade.Text = englishExam.grade;
            //txtGradeCertificateState.Text = englishExam.gradeCertificateState;
            txtGradeCertificateStateRemark.Text = englishExam.gradeCertificateRemark;

            for (int i = 0; i < ddlIspaid.Items.Count; i++)
            {
                if (int.Parse(ddlIspaid.Items[i].Value.Trim()) == englishExam.isPaid)
                {
                    ddlIspaid.SelectedIndex = i;
                }
            }

            for (int i = 0; i < ddlExamCertificate.Items.Count; i++)
            {
                if (ddlExamCertificate.Items[i].Value.Trim() == englishExam.examCertificateState.Trim())
                {
                    ddlExamCertificate.SelectedIndex = i;
                }
            }

            for (int i = 0; i < ddlGradeCertificate.Items.Count; i++)
            {
                if (ddlGradeCertificate.Items[i].Value.Trim() == englishExam.gradeCertificateState.Trim())
                {
                    ddlGradeCertificate.SelectedIndex = i;
                }
            }

        }
        //修改
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();

                EnglishExam model = new EnglishExam { studentNo = studentNo, englishExamId = englishExamId, englishExamNotifyId=englishExamNotifyId, examCertificateRemark=txtExamCertificateStateRemark.Text.Trim(), gradeCertificateRemark=txtGradeCertificateStateRemark.Text.Trim(), isPaidRemark=txtIsPaidRemark.Text.Trim(),
                     grade=txtGrade.Text.Trim(), 
                     gradeCertificateState= ddlGradeCertificate.SelectedValue,
                     examCertificateState=ddlExamCertificate.SelectedValue,
                     isPaid = int.Parse(ddlIspaid.SelectedValue)
                 };
                dalOperationAboutEnglishExam.UpdateEnglishExamSignUpState(model);
                Javascript.RefreshParentWindow("修改报名相关状态信息成功！", "/Teacher/EnglishExamManage.aspx?fragment=1", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "修改报名相关状态信息失败,请检查格式是否有误！", Page);
            }
        }
    }
}