using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using System.Data;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class SelectSalaryItemForSummary : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string tid = Request["teacherNO"];
                string courseNo = Request["courseNo"];
                if (tid == null || tid.Trim().Length == 0)
                {

                    Javascript.Alert("请先选择教师后再进行此项操作", Page);
                    Javascript.RefreshParentWindow("/Administrator/SalaryManage.aspx?fragment=1", Page);
                }
                else
                {
                    DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
                    TeachersList teacher = teacherDal.GetTeacherById(tid);
                    if (teacher == null)
                    {
                        Javascript.AlertAndRedirect("请先确定教师后再进行此项操作", "/Administrator.aspx?fragment=1", Page);
                    }
                    else
                    {
                        this.teacherName.Text = teacher.teacherName;
                        this.teacherNo.Value = teacher.teacherNo;
                    }
                    int teacherType = 1;
                   
                    if ("本院" == teacher.teacherType)
                    {
                        teacherType = 1;
                    }
                    else {
                        SelectCourse.Visible = true;
                        SelectCourse_Literal.Visible = true;
                        DalOperationAboutTeacher dalt = new DalOperationAboutTeacher();

                        DataTable dsTeacher = dalt.GetCoursesByTeacherAssistant(tid).Tables[0];
                        if (dsTeacher.Rows.Count == 0)
                        {
                            Javascript.Alert("该教师非院内教师并且本学期未待任何课程！", Page);
                            Javascript.RefreshParentWindow("/Administrator/SalaryManage.aspx?fragment=1", Page);
                        }
                        else
                        {
                            DataBindTeacherSalaryCourse(dsTeacher, courseNo);

                            if (string.IsNullOrWhiteSpace(courseNo))
                            {

                                courseNo = this.SelectCourse.SelectedValue;
                            }

                            string[] courseInfo = courseNo.Split('-');
                            int atCourseType = 1;
                            if (courseInfo.Length > 1)
                            {
                                atCourseType = int.Parse(courseInfo[1].Trim());
                            }
                            teacherType = CommonUtility.CheckTeacherType(teacher.teacherType, atCourseType);
                        }
                    }

                    if (teacherType == 2) {
                        this.teacherType_Literal.Text = "院外教师";
                    }
                    else if (teacherType == 3)
                    {
                        this.teacherType_Literal.Text = "院外助教";
                    }
                    else {
                        this.teacherType_Literal.Text = "院内教师/助教";
                    }
                    this.teacherType.Value = teacherType.ToString();

                    DalOperationAboutSalaryItem dalsi = new DalOperationAboutSalaryItem();
                    List<SalaryItem> salaryItems = dalsi.GetAllSalaryItem(teacherType, 1);

                    this.TeacherInSalaryItemList.DataSource = salaryItems;
                    this.TeacherInSalaryItemList.DataBind();
                }
                
            }
        }

        private void DataBindTeacherSalaryCourse(DataTable dt, string selectedCouseNo)
        {

            string courseName = null;
            string courseNo = null;

            bool isSelected = false;
            string currentSelectedCourseNo = null;
            int currentAtCourseType = 1;
            string currentCoursePeriod = null;
            if (selectedCouseNo != null)
            {
                selectedCouseNo = selectedCouseNo.Trim();
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                courseName = dt.Rows[i]["courseName"].ToString().Trim();
                courseNo = dt.Rows[i]["courseNo"].ToString().Trim();

                int atCourseType = int.Parse(dt.Rows[i]["atCourseType"].ToString().Trim());
                string coursePeriod = dt.Rows[i]["period"].ToString().Trim();
                courseNo = courseNo + "-" + atCourseType;
                ListItem li = new ListItem(courseName + "-" + (atCourseType == 1 ? "教师" : "助教"), courseNo);
                if (selectedCouseNo == courseNo)
                {
                    li.Selected = true;
                    isSelected = true;
                    currentAtCourseType = atCourseType;
                    currentCoursePeriod = coursePeriod;
                }
                else
                {
                    if (i == 0)
                    {
                        currentAtCourseType = atCourseType;
                        currentCoursePeriod = coursePeriod;
                    }
                }
                SelectCourse.Items.Add(li);
            }

            if (selectedCouseNo == null || selectedCouseNo.Trim().Length == 0 || isSelected == false)
            {
                currentSelectedCourseNo = SelectCourse.Items[0].Value;
            }
            else
            {
                currentSelectedCourseNo = selectedCouseNo.Trim();
            }

            this.atCourseType.Value = "" + currentAtCourseType;
        }

        protected void SelectSalaryItem_SelectCourse(object sender, EventArgs e)
        {
            string teacherNo = this.teacherNo.Value;
            string courseNo = this.SelectCourse.SelectedValue;
            Javascript.JavaScriptLocationHref("/Administrator/SelectSalaryItemForSummary.aspx?teacherNo=" + teacherNo + "&courseNo=" + courseNo, Page);

        }

        protected void AddSalary_Forwad(object sender, EventArgs e)
        {
            string inSalaryItemIds = GetSelectedInSalaryItemIds();
            string teacherNo = this.teacherNo.Value;
            string courseNo = this.SelectCourse.SelectedValue;
            string atCourseType = this.atCourseType.Value;
            string teacherType = this.teacherType.Value;

            if (inSalaryItemIds == null || inSalaryItemIds.Trim().Length == 0)
            {
                Javascript.AlertAndRedirect("请至少指定一项待发薪酬项", "/Administrator/SelectSalaryItemForSummary.aspx?teacherNo=" + teacherNo, Page);
            }
            
            Javascript.JavaScriptLocationHref("/Administrator/AddTeacherSalary.aspx?teacherNo=" + teacherNo + "&inIds=" + inSalaryItemIds + "&cid=" + courseNo + "&acType=" + atCourseType + "&teacherType=" + teacherType, Page);
        }

        /// <summary>
        /// private 方法，获得选中的薪酬收入项id列表字符串
        /// </summary>
        /// <returns></returns>
        private string GetSelectedInSalaryItemIds()
        {
            List<string> selectedInSalaryItemList = GetSelectedSalaryItemIdList(this.TeacherInSalaryItemList.Items, "salaryItemInChkBox", "salaryItemInId");
            return ConvertSalaryItemIdList2String(selectedInSalaryItemList);
        }

        /// <summary>
        /// private 方法，根据页面中选中的CheckBox项生成薪酬项id列表
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <param name="chkBoxID"></param>
        /// <param name="hiddenFieldID"></param>
        /// <returns></returns>
        private List<string> GetSelectedSalaryItemIdList(DataListItemCollection itemCollection, string chkBoxID, string hiddenFieldID)
        {
            List<string> itemList = new List<string>();
            foreach (DataListItem item in itemCollection)
            {

                CheckBox chkBox = (CheckBox)item.FindControl(chkBoxID);
                if (chkBox.Checked)
                {
                    string itemId = ((HiddenField)item.FindControl(hiddenFieldID)).Value.Trim();
                    itemList.Add(itemId);
                }
            }
            return itemList;
        }

        /// <summary>
        /// private 方法，把List的薪酬项id转化为字符串
        /// </summary>
        /// <param name="salaryItemIdList"></param>
        /// <returns></returns>
        private string ConvertSalaryItemIdList2String(List<string> salaryItemIdList)
        {
            string salaryItemIds = "";
            if (salaryItemIdList.Count > 0)
            {
                foreach (string itemId in salaryItemIdList)
                {
                    salaryItemIds += (itemId + ",");
                }

                salaryItemIds = salaryItemIds.Substring(0, salaryItemIds.Length - 1);
            }

            return salaryItemIds;
        }
    }
}