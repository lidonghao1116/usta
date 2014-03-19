using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Model;
using USTA.Common;
using USTA.Bll;
using USTA.PageBase;

namespace USTA.WebApplication.Teacher
{
    public partial class TeacherSalaryQA : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                int salaryType = int.Parse(Request["salaryType"].ToString().Trim());
                int salaryId = int.Parse(Request["salaryId"].ToString().Trim());
                DalOperationAboutSalaryQA dalsqa = new DalOperationAboutSalaryQA();
                List<SalaryQA> salaryQas = dalsqa.GetSalaryQA(salaryId, salaryType);
           
                
                this.TeacherSalaryQAList.DataSource = salaryQas;
                this.TeacherSalaryQAList.DataBind();

                this.hf_SalaryId.Value = salaryId.ToString();
                this.hf_SalaryType.Value = salaryType.ToString();
                UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
                this.teacherName.Text = userCookiesInfo.userName;

            }
        }

        protected void TeacherSalaryQA_Click(object sender, EventArgs e) 
        {
            SalaryQA salaryQa = new SalaryQA();
            salaryQa.qaContent = this.newTeacherSalaryQA.Text.Trim();
            salaryQa.salaryId = int.Parse(this.hf_SalaryId.Value.Trim());
            salaryQa.salaryType = int.Parse(this.hf_SalaryType.Value.Trim());
            TeachersList teacher = new TeachersList();
            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            teacher.teacherNo = userCookiesInfo.userNo;
            salaryQa.teacher = teacher;

            DalOperationAboutSalaryQA dalqa = new DalOperationAboutSalaryQA();
            dalqa.AddSalaryQA(salaryQa);

            Javascript.JavaScriptLocationHref("/Teacher/TeacherSalaryQA.aspx?salaryType=" + salaryQa.salaryType + "&salaryId=" + salaryQa.salaryId, Page);

        }
    }
}