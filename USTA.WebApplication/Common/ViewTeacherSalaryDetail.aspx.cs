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
using USTA.PageBase;

namespace USTA.WebApplication.Common
{
    public partial class WebForm1 : CheckUserWithCommonPageBase
    {
        private static string pageName = "Teacher_SalaryManage";
        private bool isAuth(string teacherNo) 
        {
            bool isAuth = false;
            DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
            UserAuth userAuth = dalua.GetUserAuth(pageName);
            if (userAuth != null) {
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
                string salaryEntryId = Request["salaryEntryId"];
                if (salaryEntryId == null || salaryEntryId.Trim().Length == 0)
                {
                    Javascript.Alert("请提供准确的薪酬主键", Page);
                }
                else {
                    DalOperationAboutSalaryEntry dal = new DalOperationAboutSalaryEntry();
                    SalaryEntry salaryEntry = dal.GetSalaryEntry(int.Parse(salaryEntryId.Trim()));

                    if (salaryEntry == null)
                    {

                        Javascript.Alert("你查看的记录不存在!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                    else {
                        UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
                        if (!(salaryEntry.teacher.teacherNo == userCookiesInfo.userNo || isAuth(userCookiesInfo.userNo)))
                        {
                            Javascript.Alert("你无权查看该此记录", Page);
                            Javascript.RefreshParentWindowReload(Page);
                        }
                        else
                        {

                            List<SalaryItemElement> inSalaryItemElement = salaryEntry.GetSalaryInItemElements();
                            List<SalaryItemElement> outSalaryItemElement = salaryEntry.GetSalaryOutItemElements();
                            FullFillSalaryItemElements(inSalaryItemElement);
                            FullFillSalaryItemElements(outSalaryItemElement);

                            this.ShowSalaryInItems.DataSource = inSalaryItemElement;
                            this.ShowSalaryInItems.DataBind();

                            this.ShowSalaryOutItems.DataSource = outSalaryItemElement;
                            this.ShowSalaryOutItems.DataBind();

                            this.TeacherName.Text = salaryEntry.teacher.teacherName;
                            this.TeacherSalaryCostWithTax.Text = salaryEntry.teacherCostWithTax.ToString();
                            this.TeacherSalaryCostWithoutTax.Text = salaryEntry.teacherCostWithoutTax.ToString();
                            this.TeacherTotalSalaryCost.Text = salaryEntry.teacherTotalCost.ToString();
                            this.SalaryMonth.Text = salaryEntry.salaryMonth;
                            this.TermTag.Text = CommonUtility.ChangeTermToString(salaryEntry.termTag);
                            if (salaryEntry.course != null)
                            {
                                this.TeacherCourse.Text = salaryEntry.course.courseName;
                                this.TeacherCourse_TR.Visible = true;
                            }
                            this.TeacherPosition.Text = CommonUtility.ConvertTeacherType2String(salaryEntry.teacherType);

                            this.SalaryEntryId.Value = "" + salaryEntry.salaryEntryId;
                            this.SalaryEntryMemo.Text = salaryEntry.memo;

                            if (salaryEntry.salaryEntryStatus == 2 && salaryEntry.teacher.teacherNo == userCookiesInfo.userNo)
                            {
                                this.btn_TeacherSalaryConfirm.Visible = true;
                                this.btn_TeacherSalaryQA.Visible = true;
                                this.hf_salaryId.Value = salaryEntryId;
                            }
                        }
                    
                    }
                }
            
            }
        }

        protected void TeacherSalaryConfirm_Click(object sender, EventArgs e) {
            DalOperationAboutSalaryEntry dal = new DalOperationAboutSalaryEntry();
            string salaryEntryId = this.SalaryEntryId.Value;
            dal.updateSalaryEntryStatus(int.Parse(salaryEntryId), 3);
            
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
                    itemElement.hasTax = item.hasTax;
                }
            }
        }
    }
}