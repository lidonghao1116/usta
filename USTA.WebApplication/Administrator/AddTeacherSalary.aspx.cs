using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Bll;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.PageBase;
using System.Transactions;

namespace USTA.WebApplication.Administrator
{
    public partial class AddTeacherSalary : CheckUserWithCommonPageBase
    {
        private DalOperationAboutSalaryStandardValue dalssv = new DalOperationAboutSalaryStandardValue();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string tid = Request["teacherNo"];
                string courseNo = Request["cid"];
                string teacherType = Request["teacherType"];
                string atCourseType = Request["acType"];
                
                if (tid == null || tid.Trim().Length == 0)
                {
                    Javascript.Alert("请先选择教师再进行此项操作", Page);
                    Javascript.RefreshParentWindow("/Administrator/SalaryManage.aspx?fragment=1", Page);
                }
                else
                {
                    DalOperationAboutTeacher dal = new DalOperationAboutTeacher();

                    TeachersList teacher = dal.GetTeacherById(tid);
                    if (teacher == null)
                    {
                        Javascript.AlertAndRedirect("请正确选择教师后再进行此操作", "/Administrator/SalaryManage.aspx?fragmentFlag=1", Page);
                    }
                    else
                    {
                        this.teacherName.Text = teacher.teacherName;
                        this.teacherNo.Value = teacher.teacherNo;
                        this.teacherType.Value = teacherType;
                    }

                    DataBindSearchTermTagList(this.TeacherSalary_TermTag.Items);
                    if (!string.IsNullOrWhiteSpace(courseNo)) {
                        DalOperationAboutCourses dalc = new DalOperationAboutCourses();
                        string[] courseInfo = courseNo.Split('-');
                        Courses course = dalc.GetCoursesByNo(courseInfo[0], null);
                        this.Course_TR.Visible = true;
                        string courseName = course.courseName + (atCourseType == "1" ? "（教师）" : "（助教）");
                        this.CourseName_Literal.Text = courseName;
                        this.CourseId_hf.Value = course.courseNo;
                        this.teachPeriod_TR.Visible = true;
                        this.experPeriod_TR.Visible = true;
                        this.atCourseType.Value = atCourseType;
                        DataBindTeacherSalaryCourse(course);
                    }

                    this.teacherType_Literal.Text = CommonUtility.ConvertTeacherType2String(int.Parse(teacherType.Trim()));

                    string inSalaryItemIds = Request["inIds"];
                    List<SalaryItem> inSalaryItemList = GetSalaryItemListByIdString(inSalaryItemIds);

                    TeacherSalary_ItemList.DataSource = inSalaryItemList;
                    TeacherSalary_ItemList.DataBind();

                }
            }
        }

        protected void TeacherSalaryItemList_DataBound(object sender, DataListItemEventArgs e) 
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {

                DataList dataList = (DataList)e.Item.FindControl("TeacherSalary_ItemList");
                SalaryItem salaryItem = (SalaryItem)e.Item.DataItem;
                int salaryItemId = salaryItem.salaryItemId;
                List<SalaryStandardValue> standardValueList = dalssv.GetStandardValueBySalaryItemId(salaryItemId);
                if (standardValueList == null || standardValueList.Count == 0)
                {
                    ((TextBox)e.Item.FindControl("salaryItemStandard")).Visible = true;
                    ((DropDownList)e.Item.FindControl("SalaryItemStandard_DropDownList")).Visible = false;
                    
                }
                else {
                    ((TextBox)e.Item.FindControl("salaryItemStandard")).Visible = false;
                    DropDownList dropDownList = (DropDownList)e.Item.FindControl("SalaryItemStandard_DropDownList");
                    dropDownList.Visible = true;
                    foreach(SalaryStandardValue standardValue in standardValueList){
                        dropDownList.Items.Add(new ListItem(standardValue.SalaryItemValue.ToString(), standardValue.SalaryItemValue.ToString()));

                    }
                }
            } 
        }

        /// <summary>
        /// 根据薪酬项id字符串获得相应的薪酬项信息列表 
        /// </summary>
        /// <param name="salaryItemIds"></param>
        /// <returns></returns>
        private List<SalaryItem> GetSalaryItemListByIdString(string salaryItemIds)
        {
            string[] salaryItemIdArray = ConvertSalaryItemIdString2Array(salaryItemIds);
            List<SalaryItem> salaryItemList = new List<SalaryItem>();
            if (salaryItemIdArray != null)
            {
                DalOperationAboutSalaryItem dal = new DalOperationAboutSalaryItem();
                foreach (string salaryItemId in salaryItemIdArray)
                {
                    salaryItemList.Add(dal.GetSalaryItemById(int.Parse(salaryItemId)));
                }
            }
            return salaryItemList;
        }

        /// <summary>
        /// 把","隔开的薪酬项id列表转化为id数组
        /// </summary>
        /// <param name="salaryItemIds"></param>
        /// <returns></returns>
        private string[] ConvertSalaryItemIdString2Array(string salaryItemIds)
        {
            string[] salaryItemIdArray = null;
            if (salaryItemIds != null && salaryItemIds.Trim().Length > 0)
            {
                salaryItemIdArray = salaryItemIds.Split(',');
            }
            return salaryItemIdArray;
        }

        protected void btn_TeacherSalary_Click(object sender, EventArgs e)
        {
            DalOperationAboutTeacherSalary teacherSalaryDal = new DalOperationAboutTeacherSalary();
            if (this.btn_TeacherSalary.Text == "添加")
            {
                string teacherNo = this.teacherNo.Value;
                string termTag = this.TeacherSalary_TermTag.SelectedValue;
                int teacherType = int.Parse(this.teacherType.Value.Trim());

                List<TeacherSalary> teacherSalaries = teacherSalaryDal.GetTeacherSalarys(teacherNo, teacherType, termTag, 0);
                if (teacherSalaries != null && teacherSalaries.Count != 0)
                {
                    Javascript.Alert("本学期已为该教师添加过薪酬预算，请核对信息后再次录入！", Page);
                }
                else {
                    TeachersList teacherList = new TeachersList
                    {
                        teacherNo = this.teacherNo.Value
                    };



                    TeacherSalary salary = new TeacherSalary();
                    salary.teacher = teacherList;
                    if (Course_TR.Visible)
                    {
                        string atCourseType = this.atCourseType.Value;
                        Courses course = new Courses
                        {
                            courseNo = this.CourseId_hf.Value
                        };
                        salary.course = course;
                        salary.atCourseType = int.Parse(atCourseType);
                        salary.teachPeriod = int.Parse(this.teachPeriod.Text.Trim());
                        if (this.experPeriod.Text == null || this.experPeriod.Text.Trim().Length == 0)
                        {
                            salary.experPeriod = 0;
                        }
                        else
                        {
                            salary.experPeriod = int.Parse(this.experPeriod.Text.Trim());
                        }
                    }

                    if (BuildInSalaryItemValueList(salary)) {
                        salary.teacherType = int.Parse(this.teacherType.Value.Trim());
                        salary.termTag = TeacherSalary_TermTag.SelectedValue;
                        salary.memo = teacherSalary_Memo.Text.Trim();
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                teacherSalaryDal.AddTeacherSalary(salary);
                                AddSalaryEntryDefault(salary);
                                scope.Complete();
                                Javascript.RefreshParentWindow("/Administrator/SalaryManage.aspx?fragment=3", Page);
                            }
                            catch (Exception ex)
                            {
                                MongoDBLog.LogRecord(ex);
                                Javascript.GoHistory(-1, "添加薪酬预算和月发放薪酬记录失败！", Page);
                            }
                        }
                    }
                }
            }
        }

        private void AddSalaryEntryDefault(TeacherSalary teacherSalary) 
        {
            List<SalaryItemElement> teacherSalaryElements = teacherSalary.GetSalaryInItemElements();
            int maxMonth = 1;
            foreach(SalaryItemElement teacherSalaryElement in teacherSalaryElements){
                if (teacherSalaryElement.MonthNum > maxMonth) {
                    maxMonth = teacherSalaryElement.MonthNum;
                }
            }
            List<string> salaryInValueListForMonth = new List<string>();
            List<float> salaryWithTax = new List<float>();
            List<float> salaryWithoutTax = new List<float>();

            string salaryInValueStringForMonth;
            string singleSalaryItemValue;
            float salaryWithTaxForMonth;
            float salaryWithoutTaxForMonth;
            for (int i = 1; i <= maxMonth; i++) {
                salaryInValueStringForMonth = "";
                salaryWithTaxForMonth = 0f;
                salaryWithoutTaxForMonth = 0f;
                foreach (SalaryItemElement teacherSalaryElement in teacherSalaryElements) {
                    singleSalaryItemValue = "";

                    if (i <= teacherSalaryElement.MonthNum) 
                    {
                        if (teacherSalaryElement.salaryStandard != 0)
                        {
                            singleSalaryItemValue = (teacherSalaryElement.salaryItemId + ":" + teacherSalaryElement.salaryStandard + "," + CommonUtility.ConvertFormatedFloat("{0:F2}", (teacherSalaryElement.times / teacherSalaryElement.MonthNum).ToString()) + ",1;");
                        }
                        else {
                            singleSalaryItemValue = (teacherSalaryElement.salaryItemId + ":" + 
                                CommonUtility.ConvertFormatedFloat("{0:F2}", (teacherSalaryElement.itemCost / teacherSalaryElement.MonthNum).ToString()) + ",1,1;");
                            if (teacherSalaryElement.hasTax)
                            {
                                salaryWithTaxForMonth += CommonUtility.ConvertFormatedFloat("{0:F2}", (teacherSalaryElement.itemCost / teacherSalaryElement.MonthNum).ToString());
                            }
                            else {
                                salaryWithoutTaxForMonth += CommonUtility.ConvertFormatedFloat("{0:F2}", (teacherSalaryElement.itemCost / teacherSalaryElement.MonthNum).ToString());
                            }
                        }
                        
                        salaryInValueStringForMonth += singleSalaryItemValue;
                        if (teacherSalaryElement.hasTax) { 
                            salaryWithTaxForMonth += (teacherSalaryElement.salaryStandard * teacherSalaryElement.times / teacherSalaryElement.MonthNum);
                            }else{
                            salaryWithoutTaxForMonth += (teacherSalaryElement.salaryStandard * teacherSalaryElement.times / teacherSalaryElement.MonthNum);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(salaryInValueStringForMonth)) {
                    salaryInValueStringForMonth = salaryInValueStringForMonth.Substring(0, salaryInValueStringForMonth.Length - 1);
                    salaryInValueListForMonth.Add(salaryInValueStringForMonth);
                    salaryWithTax.Add(salaryWithTaxForMonth);
                    salaryWithoutTax.Add(salaryWithoutTaxForMonth);
                }
            }

            DalOperationAboutSalaryEntry dalse = new DalOperationAboutSalaryEntry();
            SalaryEntry salaryEntry = new SalaryEntry();
            salaryEntry.teacher = teacherSalary.teacher;
            salaryEntry.teacherType = teacherSalary.teacherType;
            salaryEntry.course = teacherSalary.course;
            salaryEntry.atCourseType = "" + teacherSalary.atCourseType;
            salaryEntry.termTag = teacherSalary.termTag;
            salaryEntry.salaryEntryStatus = 1;
            salaryEntry.memo = "本列表中的税后所得仅为假定你在苏州研究院的所有收入只有此项而计算(因为是兼职，财务默认您的基础收入已经为3500元)，仅供参考！实际税后金额，以财务发放为准，有问题可以及时联系教学管理部(0512-68839206)或者财务(0512-87161163)";

            DateTime monthDateTime = DateTime.Now;

            for (int i = 0; i < salaryInValueListForMonth.Count; i++) {
                salaryEntry.SetSalaryInItemValueList(salaryInValueListForMonth[i], false);

                salaryEntry.teacherCostWithTax = CommonUtility.ConvertFormatedFloat("{0:F2}", salaryWithTax[i].ToString());
                salaryEntry.teacherCostWithoutTax = CommonUtility.ConvertFormatedFloat("{0:F2}", salaryWithoutTax[i].ToString());
                salaryEntry.teacherTotalCost = CommonUtility.ConvertFormatedFloat("{0:F2}", ((salaryEntry.teacherCostWithTax - float.Parse(CommonUtility.CalculateTax(decimal.Parse(salaryEntry.teacherCostWithTax.ToString())).ToString())) + salaryEntry.teacherCostWithoutTax).ToString());

                int monthValue = monthDateTime.Month;
                
                salaryEntry.salaryMonth = (monthDateTime.Year + "-" + (monthValue < 10 ? "0" + monthValue.ToString() : monthValue.ToString()));
                        
                dalse.AddSalaryEntry(salaryEntry);
                monthDateTime = monthDateTime.AddMonths(1);
            }
        }

        /// <summary>
        /// 返回操作是否成功，成功为true，否则为false
        /// </summary>
        /// <param name="teacherSalary"></param>
        /// <returns></returns>
        private bool BuildInSalaryItemValueList(TeacherSalary teacherSalary)
        {
            DataListItemCollection itemCollection = this.TeacherSalary_ItemList.Items;
            string salaryItemValueList = "";
            float totalSalaryValue = 0;

            bool isError = false;
            foreach (DataListItem item in itemCollection)
            {
                string itemId = ((HiddenField)item.FindControl("salaryItemId")).Value;
                string salaryStandard;
                if (((DropDownList)item.FindControl("SalaryItemStandard_DropDownList")).Visible)
                {
                    salaryStandard = ((DropDownList)item.FindControl("SalaryItemStandard_DropDownList")).SelectedValue;
                }
                else {
                    salaryStandard = CommonUtility.ConvertFormatedFloat("{0:F2}", ((TextBox)item.FindControl("salaryItemStandard")).Text.Trim()).ToString();
                }

                string salaryUnit = ((TextBox)item.FindControl("salaryItemUnit")).Text;
                string itemValue = CommonUtility.ConvertFormatedFloat("{0:F2}", ((TextBox)item.FindControl("salaryItemTotal")).Text.Trim()).ToString();
                
                string MonthNum = ((TextBox)item.FindControl("MonthNum")).Text;
                bool hasTax = bool.Parse(((HiddenField)item.FindControl("salaryItemHasTax")).Value.Trim());

                float salaryValue = float.Parse(salaryStandard) * float.Parse(salaryUnit);
                if (salaryValue == 0) {
                    salaryValue = float.Parse(itemValue);
                }

                if (salaryValue == 0) {
                    isError = true;
                    break;
                }
                salaryValue = CommonUtility.ConvertFormatedFloat("{0:F2}", salaryValue.ToString());
                
                salaryItemValueList += (itemId + ":" + salaryStandard + "," + salaryUnit + "," + salaryValue + "," + MonthNum + "," + (hasTax ? "1" : "0") + ";");

                totalSalaryValue += salaryValue;
            }
            
            if (isError)
            {
                Javascript.Alert("薪酬项的标准与总额不能同时为0，请核对后再次输入", Page);
                
            }
            else {
                if (totalSalaryValue <= 0)
                {
                    Javascript.Alert("该老师的薪酬预算总和应该为大于0的数值，请核对后再次录入", Page);
                    isError = true;
                }
                else {
                    if (salaryItemValueList.Length > 0)
                    {
                        salaryItemValueList = salaryItemValueList.Substring(0, salaryItemValueList.Length - 1);
                    }

                    teacherSalary.SetSalaryInItemValueList(salaryItemValueList, true);
                    teacherSalary.totalTeachCost = CommonUtility.ConvertFormatedFloat("{0:0.00}", totalSalaryValue.ToString());
                }
            }
            return !isError;
        }

        //绑定学期标识下拉列表
        public void DataBindSearchTermTagList(ListItemCollection itemCollection)
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.FindAllTermTags().Tables[0];
            string termTag = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                termTag = dt.Rows[i]["termTag"].ToString().Trim();
                ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
                itemCollection.Add(li);
            }
        }

        private void DataBindTeacherSalaryCourse(Courses course)
        {
            this.teachPeriod.Text = course.period;
            this.experPeriod.Text = course.TestHours;
            
        }
    }
}