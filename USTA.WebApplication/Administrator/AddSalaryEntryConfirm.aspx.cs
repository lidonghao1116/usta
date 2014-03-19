using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class AddSalaryEntryConfirm : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                string formatedTeacherSalaryCostWithTax = CommonUtility.ConvertFormatedFloat("{0:F2}", Request["withTax"].Trim()).ToString();

                string formatedTeacherSalaryCostWithoutTax = CommonUtility.ConvertFormatedFloat("{0:F2}", Request["withoutTax"].Trim()).ToString();

                string formatedTeacherTotalCost = CommonUtility.ConvertFormatedFloat("{0:F2}", Request["totalCost"].Trim()).ToString();

                this.TeacherSalaryCostWithTax.Text = formatedTeacherSalaryCostWithTax;
                this.TeacherSalaryCostWithoutTax.Text = formatedTeacherSalaryCostWithoutTax;
                this.TeacherTotalCost.Text = formatedTeacherTotalCost;
                this.InSalaryItemValueList.Value = Request["inValueList"];
                this.OutSalaryItemValueList.Value = Request["outValueList"];

                SalaryEntry salaryEntry = new SalaryEntry();
                salaryEntry.SetSalaryInItemValueList(Request["inValueList"], true);
                salaryEntry.SetSalaryOutItemValueList(Request["outValueList"], true);

                List<SalaryItemElement> inItemElements = salaryEntry.GetSalaryInItemElements();
                List<SalaryItemElement> outItemElements = salaryEntry.GetSalaryOutItemElements();

                FullFillSalaryItemElements(inItemElements);
                FullFillSalaryItemElements(outItemElements);

                this.ShowSalaryInItems.DataSource = inItemElements;
                this.ShowSalaryOutItems.DataSource = outItemElements;

                this.ShowSalaryInItems.DataBind();
                this.ShowSalaryOutItems.DataBind();

                DataBindSearchTermTagList();

                this.TeacherId.Value = Request["tid"];
                DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
                TeachersList teacher = teacherDal.GetTeacherById(Request["tid"].Trim());
                this.TeacherName.Text = teacher.teacherName;

                DateTime lastMonth = DateTime.Now.Date.AddMonths(-1);
                string lastMonthString = lastMonth.Month < 10 ? "0" + lastMonth.Month.ToString() : lastMonth.Month.ToString();
                this.SalaryMonth.Value = lastMonth.Year + "-" + lastMonthString;

                this.CourseId.Value = Request["cid"];
                this.atCourseType.Value = Request["acType"];
                this.teacherType.Value = Request["teacherType"];
            }
        }

        //绑定学期标识下拉列表
        public void DataBindSearchTermTagList()
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.FindAllTermTags().Tables[0];
            this.SalaryTermTag.Items.Clear();
            string termTag = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                termTag = dt.Rows[i]["termTag"].ToString().Trim();
                ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
                this.SalaryTermTag.Items.Add(li);
            }
        }

        private void FullFillSalaryItemElements(List<SalaryItemElement> itemElements) 
        {
            DalOperationAboutSalaryItem dal = new DalOperationAboutSalaryItem();
            if (itemElements != null && itemElements.Count > 0) 
            {
                foreach(SalaryItemElement itemElement in itemElements)
                {
                    SalaryItem item = dal.GetSalaryItemById(int.Parse(itemElement.salaryItemId));
                    itemElement.salaryItemName = item.salaryItemName;
                    itemElement.itemUnit = item.salaryItemUnit;
                    itemElement.hasTax = item.hasTax;
                }
            }
        }

        

        protected void AddSalary_Submit(object sender, EventArgs e) 
        {
            DalOperationAboutSalaryEntry dal = new DalOperationAboutSalaryEntry();
            DalOperationAboutTeacher dalt = new DalOperationAboutTeacher();
            DalOperationAboutCourses dalc = new DalOperationAboutCourses();

            string teacherNo = this.TeacherId.Value;
            string termTag = this.SalaryTermTag.SelectedValue;
            string salaryMonth = this.SalaryMonth.Value;
            int teacherType = int.Parse(this.teacherType.Value.Trim());
            int salaryEntryStatus = 0;
            
            List<SalaryEntry> salaryEntries = dal.GetSalaryEntrys(teacherNo, termTag, salaryMonth, teacherType, salaryEntryStatus);
            if (salaryEntries != null && salaryEntries.Count != 0)
            {
                Javascript.Alert("本月已为该教师添加过薪酬信息，请核对信息后再次录入!", Page);
            }
            else {
                SalaryEntry salaryEntry = new SalaryEntry();

                TeachersList teacher = new TeachersList
                {
                    teacherNo = teacherNo
                };

                Courses course = new Courses
                {
                    courseNo = this.CourseId.Value
                };

                salaryEntry.teacher = teacher;
                salaryEntry.course = course;
                salaryEntry.atCourseType = atCourseType.Value;
                salaryEntry.termTag = termTag;
                salaryEntry.salaryMonth = salaryMonth;
                salaryEntry.teacherCostWithTax = float.Parse(this.TeacherSalaryCostWithTax.Text);
                salaryEntry.teacherCostWithoutTax = float.Parse(this.TeacherSalaryCostWithoutTax.Text);
                salaryEntry.teacherTotalCost = float.Parse(this.TeacherTotalCost.Text);

                salaryEntry.SetSalaryInItemValueList(this.InSalaryItemValueList.Value, false);
                salaryEntry.SetSalaryOutItemValueList(this.OutSalaryItemValueList.Value, false);
                salaryEntry.memo = this.SalaryEntryMemo.Text.Trim();
                salaryEntry.teacherType = teacherType;

                dal.AddSalaryEntry(salaryEntry);
                Javascript.RefreshParentWindow("/Administrator/SalaryManage.aspx?fragment=5", Page);
            }
        }
    }
}