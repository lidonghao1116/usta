using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Model;
using USTA.Common;
using USTA.PageBase;
using USTA.Bll;

namespace USTA.WebApplication.Teacher
{
    public partial class ViewTeacherSalarySummaryDetail : CheckUserWithCommonPageBase
    {
        private static string pageName = "Teacher_SalaryManage";
        private bool isAuth(string teacherNo)
        {
            bool isAuth = false;
            DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
            UserAuth userAuth = dalua.GetUserAuth(pageName);
            if (userAuth != null)
            {
                string userIds = userAuth.userIds;
                if (!(userIds == null || userIds.Trim().Length == 0))
                {
                    if (userIds.Contains(teacherNo))
                    {
                        isAuth = true;
                    }
                }
            }

            return isAuth;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string teacherSalaryId = Request["teacherSalaryId"];
                if (teacherSalaryId == null || teacherSalaryId.Trim().Length == 0)
                {
                    Javascript.Alert("请提供准确的薪酬主键", Page);
                }
                else
                {
                    DalOperationAboutTeacherSalary dal = new DalOperationAboutTeacherSalary();
                    TeacherSalary teacherSalary = dal.GetTeacherSalaryBySalaryId(int.Parse(teacherSalaryId.Trim()));
                    if (teacherSalary == null)
                    {
                        Javascript.Alert("您查看的薪酬记录不存在!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                    else {
                        UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
                        if (isAuth(userCookiesInfo.userNo) || teacherSalary.teacher.teacherNo == userCookiesInfo.userNo)
                        {
                            List<SalaryItemElement> inSalaryItemElement = teacherSalary.GetSalaryInItemElements();

                            FullFillSalaryItemElements(inSalaryItemElement);

                            this.ShowSalarySummaryItems.DataSource = inSalaryItemElement;
                            this.ShowSalarySummaryItems.DataBind();



                            this.TeacherName.Text = teacherSalary.teacher.teacherName;
                            this.TeacherTotalSummaryCost.Text = teacherSalary.totalTeachCost.ToString();
                            this.TermTag.Text = CommonUtility.ChangeTermToString(teacherSalary.termTag);
                            if (teacherSalary.teacherType != 1)
                            {
                                this.TeacherCourse.Text = teacherSalary.course.courseName;
                                this.TeacherCoursePeriod.Text = teacherSalary.teachPeriod + "/" + teacherSalary.experPeriod;
                                this.TeacherCourse_TR.Visible = true;
                                this.TeacherCoursePeriod_TR.Visible = true;
                            }
                            this.TeacherPosition.Text = CommonUtility.ConvertTeacherType2String(teacherSalary.teacherType);

                            this.TeacherSalaryId.Value = "" + teacherSalary.teacherSalaryId;
                            this.SalaryEntryMemo.Text = teacherSalary.memo;

                            if (!teacherSalary.isConfirm && teacherSalary.teacher.teacherNo == userCookiesInfo.userNo)
                            {
                                this.btn_TeacherSalaryConfirm.Visible = true;
                                this.btn_TeacherSalaryQA.Visible = true;
                                this.hf_salaryId.Value = teacherSalaryId;
                            }

                        }
                        else {
                            Javascript.Alert("您无权限查看此条记录!", Page);
                            Javascript.RefreshParentWindowReload(Page);
                        
                        }
                    }
                }
            }
        }

        protected void TeacherSalaryConfirm_Click(object sender, EventArgs e)
        {
            DalOperationAboutTeacherSalary dal = new DalOperationAboutTeacherSalary();
            string teacherSalaryId = this.TeacherSalaryId.Value;
            dal.confirmTeacherSalary(int.Parse(teacherSalaryId));

            Javascript.RefreshParentWindowReload(Page);
        }

        protected void TeacherSalaryQA_Click(object sender, EventArgs e)
        {
            string salaryId = this.hf_salaryId.Value;
            string salaryType = this.hf_salaryType.Value;

            Javascript.JavaScriptLocationHref("/Teacher/TeacherSalaryQA.aspx?salaryId=" + salaryId + "&salaryType=" + salaryType, Page);

        }


        private void FullFillSalaryItemElements(List<SalaryItemElement> itemElements)
        {
            DalOperationAboutSalaryItem dal = new DalOperationAboutSalaryItem();
            if (itemElements != null && itemElements.Count > 0)
            {
                foreach (SalaryItemElement itemElement in itemElements)
                {
                    SalaryItem item = dal.GetSalaryItemById(int.Parse(itemElement.salaryItemId));
                    itemElement.salaryItemName = item.salaryItemName;
                    itemElement.itemUnit = item.salaryItemUnit;
                }
            }
        }
    }
}