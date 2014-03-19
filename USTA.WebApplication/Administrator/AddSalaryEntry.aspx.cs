using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Model;
using USTA.Dal;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class AddSalaryEntry : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
string inSalaryItemIds = Request["inIds"];
                string outSalaryItemIds = Request["outIds"];
                string teacherNo = Request["tid"];
                string cid = Request["cid"];
                string acType = Request["acType"];
                string teacherType = Request["teacherType"];
                DalOperationAboutTeacherSalary dalts = new DalOperationAboutTeacherSalary();
                string[] courseInfo = cid.Split('-');
                cid = courseInfo[0];
                TeacherSalary teacherSalary = dalts.GetTeacherSalaryByTidAndCidAndTermTag(teacherNo, cid, int.Parse(teacherType), null);

                List<SalaryItem> inSalaryItemList = GetInSalaryItemListByIdString(inSalaryItemIds, teacherSalary);
                List<SalaryItem> outSalaryItemList = GetOutSalaryItemListByIdString(outSalaryItemIds);

                if (inSalaryItemList.Count == 0)
                {
                    Javascript.AlertAndRedirect("请至少选择一项待发薪酬项", "/Administrator/SelectSalaryItem.aspx?teacherNO=" + teacherNo, Page);
                }
                else
                {

                    DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
                    TeachersList teacher = teacherDal.GetTeacherById(teacherNo);
                    this.teacherNo.Value = teacher.teacherNo;
                    this.teacherName.Text = teacher.teacherName;
                    this.courseNo.Value = cid;
                    this.atCourseType.Value = acType;
                    this.teacherType.Value = teacherType;

                    this.TeacherType_Literal.Text = CommonUtility.ConvertTeacherType2String(int.Parse(teacherType));
                    this.InSalaryItemList.DataSource = inSalaryItemList;
                    this.InSalaryItemList.DataBind();

                    this.OutSalaryItemList.DataSource = outSalaryItemList;
                    this.OutSalaryItemList.DataBind();
                    if (!string.IsNullOrWhiteSpace(cid)) {
                        DalOperationAboutCourses dalc = new DalOperationAboutCourses();
                        
                        Courses course = dalc.GetCoursesByNo(cid, null);
                        this.Course_TR.Visible = true;
                        this.CourseName_Literal.Text = course.courseName;

                        this.Course_TR.Visible = true;
                        this.CoursePeriod_TR.Visible = true;
                        if (string.IsNullOrWhiteSpace(course.TestHours) || "0" == course.TestHours)
                        {
                            this.CoursePeriod_Literal.Text = course.period;
                        }
                        else {
                            this.CoursePeriod_Literal.Text = course.period + "/" + course.TestHours;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 根据薪酬项id字符串获得相应的薪酬项信息列表 
        /// </summary>
        /// <param name="salaryItemIds"></param>
        /// <returns></returns>
        private List<SalaryItem> GetInSalaryItemListByIdString(string salaryItemIds, TeacherSalary teacherSalary)
        {
            string[] salaryItemIdArray = ConvertSalaryItemIdString2Array(salaryItemIds);
            List<SalaryItem> salaryItemList = new List<SalaryItem>();
            if (salaryItemIdArray != null)
            {
                DalOperationAboutSalaryItem dal = new DalOperationAboutSalaryItem();
                foreach (string salaryItemId in salaryItemIdArray)
                {
                    SalaryItem salaryItem = dal.GetSalaryItemById(int.Parse(salaryItemId));
                    SetSalaryItemElement(salaryItem, teacherSalary);
                    salaryItemList.Add(salaryItem);
                }
            }
            return salaryItemList;
        }

        private void SetSalaryItemElement(SalaryItem salaryItem, TeacherSalary teacherSalary)
        {
            if (teacherSalary != null && teacherSalary.GetSalaryInItemElements() != null) 
            {
                foreach (SalaryItemElement salaryItemElement in teacherSalary.GetSalaryInItemElements()) 
                {
                    if (salaryItem.salaryItemId.ToString() == salaryItemElement.salaryItemId) {
                        SalaryItemElement element = new SalaryItemElement();
                        if (int.Parse(string.Format("{0:0}", salaryItemElement.times)) != 0)
                        {
                            element.times = salaryItemElement.times / salaryItemElement.MonthNum;
                            element.times = CommonUtility.ConvertFormatedFloat("{0:F2}", element.times.ToString());      
                        }
                        if (salaryItemElement.salaryStandard != 0) 
                        {
                            element.salaryStandard = salaryItemElement.salaryStandard;
                        }
                        else if (salaryItemElement.itemCost != 0) {
                            element.salaryStandard = salaryItemElement.itemCost / salaryItemElement.MonthNum;
                            element.salaryStandard = CommonUtility.ConvertFormatedFloat("{0:F2}", element.salaryStandard.ToString());
                        }

                        salaryItem.salaryItemElement = element;
                        break;
                    }
                }
            }
            if (salaryItem.salaryItemElement == null)
            {
                SalaryItemElement element = new SalaryItemElement();
                element.salaryStandard = 1;
                element.times = 1;
            }
        }

        /// <summary>
        /// 根据薪酬项id字符串获得相应的薪酬项信息列表 
        /// </summary>
        /// <param name="salaryItemIds"></param>
        /// <returns></returns>
        private List<SalaryItem> GetOutSalaryItemListByIdString(string salaryItemIds)
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

        /// <summary>
        /// 页面点击下一步时候触发动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddSalaryValue_Forward(object sender, EventArgs e)
        {
            SalaryEntry salaryEntry = new SalaryEntry();
            BuildInSalaryItemValueList(salaryEntry);
            BuildOutSalaryItemValueList(salaryEntry);
            string tid = this.teacherNo.Value;
            string cid = this.courseNo.Value;
            string acType = this.atCourseType.Value;
            string teacherType = this.teacherType.Value;
            
            if (salaryEntry.teacherTotalCost <= 0)
            {
                Javascript.Alert("该老师本月发放薪酬应该为大于0的数值，请核对后再次录入", Page);
            }
            else {
                Javascript.JavaScriptLocationHref("/Administrator/AddSalaryEntryConfirm.aspx?tid=" + tid + "&inValueList=" + salaryEntry.GetSalaryInItemValueList() + "&outValueList=" + salaryEntry.GetSalaryOutItemValueList() + "&totalCost=" + salaryEntry.teacherTotalCost + "&withTax=" + salaryEntry.teacherCostWithTax + "&withoutTax=" + salaryEntry.teacherCostWithoutTax + "&cid=" + cid + "&acType=" + acType + "&teacherType=" + teacherType, Page);
            }
        }

        private void BuildInSalaryItemValueList(SalaryEntry salaryEntry)
        {
            DataListItemCollection itemCollection = this.InSalaryItemList.Items;
            string salaryItemValueList = "";
            float totalSalaryValue = 0;
            float teacherCostWithoutTax = 0;
            float teacherCostWithTax = 0;
            foreach (DataListItem item in itemCollection)
            {
                string itemId = ((HiddenField)item.FindControl("in" + "SalaryItemId")).Value;

                string salaryStandard;
                if (((DropDownList)item.FindControl("InSalaryItemStandard_DropDownList")).Visible)
                {
                    salaryStandard = ((DropDownList)item.FindControl("InSalaryItemStandard_DropDownList")).SelectedValue;
                }
                else {
                    salaryStandard = ((TextBox)item.FindControl("in" + "SalaryStandard")).Text;
                }
                    
                
                string salaryUnit = ((TextBox)item.FindControl("in" + "SalaryUnit")).Text;

                string salaryAdjust = ((TextBox)item.FindControl("in" + "salaryAdjust")).Text;

                float salaryValue = float.Parse(salaryStandard) * float.Parse(salaryUnit) * float.Parse(salaryAdjust);

                bool hasTax = bool.Parse(((HiddenField)item.FindControl("InSalaryItemHasTax")).Value.Trim());

                salaryItemValueList += (itemId + ":" + salaryStandard + "," + salaryUnit + "," + salaryAdjust + ";");
                if (hasTax)
                {
                    teacherCostWithTax += salaryValue;
                }
                else
                {
                    teacherCostWithoutTax += salaryValue;
                }

            }

            if (salaryItemValueList.Length > 0)
            {
                salaryItemValueList = salaryItemValueList.Substring(0, salaryItemValueList.Length - 1);
            }


            salaryEntry.SetSalaryInItemValueList(salaryItemValueList, false);
            salaryEntry.teacherCostWithoutTax += teacherCostWithoutTax;
            salaryEntry.teacherCostWithTax += teacherCostWithTax;
            totalSalaryValue = teacherCostWithoutTax + (teacherCostWithTax - float.Parse(CommonUtility.CalculateTax(decimal.Parse(teacherCostWithTax.ToString())).ToString()));
            salaryEntry.teacherTotalCost += totalSalaryValue;

            salaryEntry.teacherCostWithoutTax = CommonUtility.ConvertFormatedFloat("{0:F2}", salaryEntry.teacherCostWithoutTax.ToString());

            salaryEntry.teacherCostWithTax = CommonUtility.ConvertFormatedFloat("{0:F2}", salaryEntry.teacherCostWithTax.ToString());

            salaryEntry.teacherTotalCost = CommonUtility.ConvertFormatedFloat("{0:F2}", salaryEntry.teacherTotalCost.ToString());
        }

        private void BuildOutSalaryItemValueList(SalaryEntry salaryEntry)
        {
            DataListItemCollection itemCollection = this.OutSalaryItemList.Items;
            string salaryItemValueList = "";
            float totalSalaryValue = 0;
            foreach (DataListItem item in itemCollection)
            {
                string itemId = ((HiddenField)item.FindControl("out" + "SalaryItemId")).Value;

                string salaryStandard = ((TextBox)item.FindControl("out" + "SalaryStandard")).Text;

                string salaryUnit = ((TextBox)item.FindControl("out" + "SalaryUnit")).Text;

                string salaryAdjust = ((TextBox)item.FindControl("out" + "salaryAdjust")).Text;

                float salaryValue = float.Parse(salaryStandard) * float.Parse(salaryUnit) * float.Parse(salaryAdjust);

                salaryItemValueList += (itemId + ":" + salaryStandard + "," + salaryUnit + "," + salaryAdjust + ";");
                totalSalaryValue += salaryValue;
            }

            if (salaryItemValueList.Length > 0)
            {
                salaryItemValueList = salaryItemValueList.Substring(0, salaryItemValueList.Length - 1);
            }
            salaryEntry.SetSalaryOutItemValueList(salaryItemValueList, false);
            salaryEntry.teacherTotalCost -= totalSalaryValue;

            salaryEntry.teacherTotalCost = CommonUtility.ConvertFormatedFloat("{0:F2}", salaryEntry.teacherTotalCost.ToString());

        }

        protected void SalaryEntryItemList_DataBound(object sender, DataListItemEventArgs e) 
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                DataList dataList = (DataList)e.Item.FindControl("InSalaryItemList");
                SalaryItem salaryItem = (SalaryItem)e.Item.DataItem;
                int salaryItemId = salaryItem.salaryItemId;
                string defaultStandardValue = ((TextBox)e.Item.FindControl("InSalaryStandard")).Text;
                if (string.IsNullOrWhiteSpace(defaultStandardValue)) 
                {
                    DalOperationAboutSalaryStandardValue dalssv = new DalOperationAboutSalaryStandardValue();
                    List<SalaryStandardValue> standardValueList = dalssv.GetStandardValueBySalaryItemId(salaryItemId);
                    if (standardValueList == null || standardValueList.Count == 0)
                    {
                        ((TextBox)e.Item.FindControl("InSalaryStandard")).Visible = true;
                        ((DropDownList)e.Item.FindControl("InSalaryItemStandard_DropDownList")).Visible = false;

                    }
                    else
                    {
                        ((TextBox)e.Item.FindControl("InSalaryStandard")).Visible = false;
                        DropDownList dropDownList = (DropDownList)e.Item.FindControl("InSalaryItemStandard_DropDownList");
                        dropDownList.Visible = true;
                        foreach (SalaryStandardValue standardValue in standardValueList)
                        {
                            dropDownList.Items.Add(new ListItem(standardValue.SalaryItemValue.ToString(), standardValue.SalaryItemValue.ToString()));

                        }
                    }
                }
                TextBox InSalaryUnitTextBox = (TextBox)e.Item.FindControl("InSalaryUnit");
                if (string.IsNullOrWhiteSpace(InSalaryUnitTextBox.Text)) 
                {
                    InSalaryUnitTextBox.Text = "1";
                }
            } 
        
        }
    }
}