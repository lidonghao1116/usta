using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Dal;
using USTA.Model;
using USTA.Common;
using System.Data;

namespace USTA.WebApplication.Teacher
{
    public partial class SalaryView : System.Web.UI.Page
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 
        protected void Page_Load(object sender, EventArgs e)
        {
            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            string tid = userCookiesInfo.userNo;
            if (tid == null || tid.Trim().Length == 0)
            {

                Javascript.AlertAndRedirect("你尚未登陆，请先登陆!", "/Common/NotifyList.aspx", Page);

            }
            else
            {

                //控制Tab的显示
                string fragmentFlag = "1";

                if (Request["fragment"] != null)
                {
                    fragmentFlag = Request["fragment"];
                }

                CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, divFragment1, divFragment2);

                if (fragmentFlag.Equals("1"))
                {
                    if (!IsPostBack)
                    {
                        DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
                        TeachersList teacher = teacherDal.GetTeacherById(tid);
                        this.TeacherSalary_Name.Text = teacher.teacherName;
                        DataListBindTeacherSalary(tid);
                    }
                }

                if (fragmentFlag.Equals("2"))
                {
                    if (!IsPostBack)
                    {
                        DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
                        TeachersList teacher = teacherDal.GetTeacherById(tid);
                        this.SalaryEntry_TeacherName.Text = teacher.teacherName;

                        DataListBindSalaryEntry(tid);

                    }
                }

            }
        }

        private void DataListBindTeacherSalary(string teacherNo)
        {
            DalOperationAboutTeacherSalary dal = new DalOperationAboutTeacherSalary();

            string termTag = Request["termTag"];
            DataBindSearchTermTagList(this.TeacherSalary_TermTag.Items, termTag);
            
            if (termTag == null || termTag.Trim().Length == 0)
            {
                termTag = this.TeacherSalary_TermTag.SelectedValue;
            }
            else if(termTag.Trim() == "all") {
                termTag = null;
            }
            List<TeacherSalary> teacherSalaries = new List<TeacherSalary>();

            teacherSalaries = dal.GetTeacherSalarys(teacherNo, 0, termTag, 0);
            

            if (teacherSalaries != null)
            {
                this.TeacherSalaryPager.RecordCount = teacherSalaries.Count;
                TeacherSalaryPager.PageSize = CommonUtility.pageSize;

                PagedDataSource pds = new PagedDataSource();
                pds.DataSource = teacherSalaries;
                pds.AllowPaging = true;

                pds.CurrentPageIndex = pageIndex - 1;
                pds.PageSize = TeacherSalaryPager.PageSize;

                this.TeacherSalaryList.DataSource = pds;
                this.TeacherSalaryList.DataBind();
            }
            else
            {
                this.TeacherSalaryPager.RecordCount = 0;
            }

            if (TeacherSalaryPager.RecordCount > 0)
            {
                this.TeacherSalaryList.ShowFooter = false;
            }
            
        }

        //绑定学期标识下拉列表
        public void DataBindSearchTermTagList(ListItemCollection itemCollection, string termValue)
        {
            itemCollection.Clear();
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.FindAllTermTags().Tables[0];
            string termTag = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                termTag = dt.Rows[i]["termTag"].ToString().Trim();
                ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
                if (termValue == li.Value)
                {
                    li.Selected = true;
                }
                itemCollection.Add(li);
            }
            ListItem itemAll = new ListItem("全部学期", "all");
            if ("all" == termValue) {
                itemAll.Selected = true;
            }
            itemCollection.Add(itemAll);
        }

        protected void DataListBindSalaryEntry(string teacherNo)
        {
            DalOperationAboutSalaryEntry dal = new DalOperationAboutSalaryEntry();

            string termTag = Request["termTag"];
            DataBindSearchTermTagList(this.SalaryQuery_TermTag.Items, termTag);
            if (termTag == null || termTag.Trim().Length == 0)
            {
                termTag = this.SalaryQuery_TermTag.SelectedValue;        
            }
            else if (termTag.Trim() == "all") {
                termTag = null;
            }


            string salaryMonth = Request["salaryMonth"] != null && Request["salaryMonth"].Trim().Length > 0 ? Request["salaryMonth"].Trim() : null;
            this.SalaryQuery_SalaryMonth.Value = salaryMonth;

            List<SalaryEntry> salaryEntries = new List<SalaryEntry>();


            salaryEntries = dal.GetSalaryEntrys(teacherNo, termTag, salaryMonth, 0, 4); //salaryEntryStatus=4：只看到已发放的

            if (salaryEntries != null)
            {
                this.SalaryEntryPager.RecordCount = salaryEntries.Count;
                SalaryEntryPager.PageSize = CommonUtility.pageSize;

                PagedDataSource pds = new PagedDataSource();
                pds.DataSource = salaryEntries;
                pds.AllowPaging = true;

                pds.CurrentPageIndex = pageIndex - 1;
                pds.PageSize = SalaryEntryPager.PageSize;

                this.SalaryEntryList.DataSource = pds;
                this.SalaryEntryList.DataBind();
            }
            else
            {
                this.SalaryEntryPager.RecordCount = 0;
            }

            if (SalaryEntryPager.RecordCount > 0)
            {
                this.SalaryEntryList.ShowFooter = false;
            }
            

        }

        protected void TeacherSalary_TermTagChanged(object sender, EventArgs e)
        {
            this.TeacherSalaryQuery_Click(sender, e);

        }

        protected void TeacherSalaryQuery_Click(object sender, EventArgs e)
        {

            string termTag = this.TeacherSalary_TermTag.SelectedValue;
            Javascript.JavaScriptLocationHref("/Teacher/SalaryView.aspx?fragment=1&termTag=" + termTag, Page);
        }

        protected void TeacherSalaryPager_PageChanged(object sender, EventArgs e)
        {
            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            string tid = userCookiesInfo.userNo;
            DataListBindTeacherSalary(tid);
        }

        protected void SalaryEntryPager_PageChanged(object sender, EventArgs e)
        {
            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            string tid = userCookiesInfo.userNo;
            DataListBindSalaryEntry(tid);
        }

        protected void SalaryQuery_TermTagChanged(object source, EventArgs e)
        {
            this.SalaryQuery_Click(source, e);
        }

        protected void SalaryQuery_Click(object source, EventArgs e)
        {
            string termTag = this.SalaryQuery_TermTag.SelectedValue;
            
            string salaryMonth = this.SalaryQuery_SalaryMonth.Value != null && this.SalaryQuery_SalaryMonth.Value.Trim().Length > 0 ? this.SalaryQuery_SalaryMonth.Value.Trim() : null;

            Javascript.JavaScriptLocationHref("/Teacher/SalaryView.aspx?fragment=2&termTag=" + termTag + "&salaryMonth=" + salaryMonth, Page);
        }

        protected void TeacherSalaryManage_ItemCommand(object source, DataListCommandEventArgs e) 
        {
            DalOperationAboutTeacherSalary dal = new DalOperationAboutTeacherSalary();
            if (e.CommandName == "confirm")
            {
                string teacherSalaryId = TeacherSalaryList.DataKeys[e.Item.ItemIndex].ToString();
                dal.confirmTeacherSalary(int.Parse(teacherSalaryId));
                Javascript.AlertAndRedirect("确认成功", "/Teacher/SalaryView.aspx?fragment=1&page=" + pageIndex, Page);
            }
        }

        protected void TeacherSalaryManage_ItemBound(object sender, DataListItemEventArgs e) 
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("TeacherSalaryList");
                TeacherSalary teacherSalary = (TeacherSalary)e.Item.DataItem;
                
                if(!teacherSalary.isConfirm){
                    e.Item.FindControl("LinkButton_TeacherSalary_Confirm").Visible = true;
                    e.Item.FindControl("TeacherSalary_feedback_Link").Visible = true;
                    ((HyperLink)e.Item.FindControl("TeacherSalary_feedback_Link")).NavigateUrl = "/Teacher/TeacherSalaryQA.aspx?keepThis=true&salaryId=" + teacherSalary.teacherSalaryId + "&salaryTYpe=1&TB_iframe=true&height=300&width=500&page=" + pageIndex;
                }
                ((Literal)e.Item.FindControl("TeacherSalary_TeacherType_Literal")).Text = CommonUtility.ConvertTeacherType2String(teacherSalary.teacherType);
            }
        }

        protected void SalaryEntryManage_ItemBound(object sender, DataListItemEventArgs e) 
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("SalaryEntryList");
                SalaryEntry teacherSalary = (SalaryEntry)e.Item.DataItem;

                ((Literal)e.Item.FindControl("literal_SalaryEntryStatus")).Text = CommonUtility.ConvertSalaryEntryStatus(teacherSalary.salaryEntryStatus);

                if (teacherSalary.salaryEntryStatus == 2)
                {
                   e.Item.FindControl("SalaryEntry_feedback_Link").Visible = true;
                }
            }
        
        }

    }
}