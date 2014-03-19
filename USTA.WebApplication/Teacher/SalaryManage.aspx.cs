using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Model;
using USTA.Common;
using System.Data;
using USTA.Bll;

namespace USTA.WebApplication.Teacher
{
    public partial class SalaryManage : System.Web.UI.Page
    {
        private static string pageName = "Teacher_SalaryManage";
        public List<string> authIds = new List<string>();
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 
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
            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            if (!isAuth(userCookiesInfo.userNo))
            {
                Javascript.Alert("你无权查看此页面", Page);
                Javascript.RefreshParentWindow("/Common/NotifyList.aspx", Page);
            }
            else {
                //控制Tab的显示
                string fragmentFlag = "2";

                if (Request["fragment"] != null)
                {
                    fragmentFlag = Request["fragment"];
                }

                CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                    , liFragment4, liFragment5, divFragment1, divFragment2, divFragment3, divFragment4, divFragment5);

                if (fragmentFlag.Equals("2"))
                {
                    if (!IsPostBack)
                    {
                        DataListBindSalaryItem();
                    }
                }
                if (fragmentFlag.Equals("3"))
                {
                    if (!IsPostBack)
                    {
                        string tid = Request["teacherNo"];
                        if (tid != null && tid.Trim().Length != 0)
                        {
                            DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
                            TeachersList teacher = teacherDal.GetTeacherById(tid);
                            this.TeacherSalary_TeacherName.Text = teacher.teacherName;
                        }
                        DataListBindTeacherSalary();
                    }

                }
                if (fragmentFlag.Equals("4"))
                {
                    if (!IsPostBack)
                    {
                        string tid = Request["teacherNo"];
                        if (tid == null || tid.Trim().Length == 0)
                        {
                            Javascript.AlertAndRedirect("请先选择教师再进行此项操作", "/Administrator/SalaryManage.aspx?fragmentFlag=1", Page);
                        }
                        else
                        {
                            Javascript.JavaScriptLocationHref("SelectSalaryItem.aspx?keepThis=true&tid=" + tid + "&TB_iframe=true&height=300&width=500", Page);
                        }
                    }
                }
                if (fragmentFlag.Equals("5"))
                {
                    if (!IsPostBack)
                    {
                        string tid = Request["teacherNo"];
                        if (tid != null && tid.Trim().Length != 0)
                        {
                            DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
                            TeachersList teacher = teacherDal.GetTeacherById(tid);
                            this.SalaryQuery_Name.Text = teacher.teacherName;
                        }
                        DataListBindSalaryEntry();

                    }
                }
            
            }

            
        }

        //模糊查询
        protected void SearchTeacher_Click(object sender, EventArgs e)
        {
            string teacherName = this.txtKeyword.Text;
            string termTag = this.searchTeacherTermTag.SelectedValue;
            Javascript.JavaScriptLocationHref("/Teacher/SalaryManage.aspx?teacherName=" + teacherName + "&termTag=" + termTag, Page);
        }

        protected void seacherTeacher_SelectChanged(object sender, EventArgs e)
        {
            this.SearchTeacher_Click(sender, e);
        }

        //绑定搜索到的教师数据
        protected void TeacherDataListBind()
        {
            DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
            UserAuth auth = dalua.GetUserAuth(pageName);
            if (auth != null)
            {
                string ids = auth.userIds;
                string[] _ids = ids.Split(',');
                for (int i = 0; i < _ids.Length; i++)
                {
                    authIds.Add(_ids[i]);
                }
            }
            DalOperationUsers dal = new DalOperationUsers();
            string termValue = Request["termTag"];
            string teacherName = Request["teacherName"];
            DataBindSearchTermTagList(searchTeacherTermTag.Items, termValue);
            if (termValue == "all")
            {
                termValue = "";
            }
            else if(termValue == null || termValue.Trim().Length == 0) {
                termValue = searchTeacherTermTag.SelectedValue;
            }

            if (teacherName != null) {
                teacherName = teacherName.Trim();
                this.txtKeyword.Text = teacherName;
            }


            DataView dv = dal.SearchTeacherByTermTagAndKeyword(termValue, teacherName).DefaultView;

            this.TeacherListPager.RecordCount = dv.Count;
            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dv;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = pageIndex - 1;
            pds.PageSize = CommonUtility.pageSize;

            this.dlSearchTeacher.DataSource = pds;
            this.dlSearchTeacher.DataBind();

            if (pds.Count > 0)
            {
                dlSearchTeacher.ShowFooter = false;
            }
        }
        //搜索到的教师数据分页
        protected void TeacherSearch_PageChanged(object sender, EventArgs e)
        {
            TeacherDataListBind();
        }

        protected void dlSearchTeacher_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("dlstSecondNorm");
                DataRowView rowv = (DataRowView)e.Item.DataItem;
                string teacherNo = rowv["teacherNo"].ToString().Trim();

                if (this.isAuth(teacherNo))
                {
                    e.Item.FindControl("LinkButton_RemoveSalaryPermission").Visible = true;
                    e.Item.FindControl("LinkButton_AddSalaryPermission").Visible = false;
                }
                else
                {
                    e.Item.FindControl("LinkButton_RemoveSalaryPermission").Visible = false;
                    e.Item.FindControl("LinkButton_AddSalaryPermission").Visible = true;
                }
            }
        }

        protected void DataListBindSalaryEntry()
        {
            DalOperationAboutSalaryEntry dal = new DalOperationAboutSalaryEntry();
            string teacherNo = null;
            string teacherName = Request["teacherName"];
            this.SalaryQuery_Name.Text = teacherName;
            if (teacherName != null && teacherName.Trim().Length > 0)
            {
                teacherNo = SearchTeacherIds(teacherName.Trim());

            }
            else
            {
                teacherName = null;
            }
            string termTag = Request["termTag"];
            string teacherType = Request["teacherType"];
            string salaryEntryStatus = Request["status"];
            DataBindSearchTermTagList(this.SalaryQuery_TermTag.Items, termTag);
            DataBindTeacherTypeList(this.SalaryQuery_TeacherType.Items, teacherType);
            DataBindSalaryStatus(this.SalaryQuery_SalaryEntryStatus.Items, salaryEntryStatus);

            if (termTag == null || termTag.Trim().Length == 0)
            {
                termTag = this.SalaryQuery_TermTag.SelectedValue;
            }
            else if (termTag.Trim() == "all") {
                termTag = null;
            }

            if (teacherType == null || teacherType.Trim().Length == 0)
            {
                teacherType = "0";
            }
            else {
                teacherType = teacherType.Trim();
            }

            if (string.IsNullOrWhiteSpace(salaryEntryStatus) || "0" == salaryEntryStatus.Trim())
            {
                salaryEntryStatus = "0";
            }
            else
            {
                salaryEntryStatus = salaryEntryStatus.Trim();
            }

            string salaryMonth = Request["salaryMonth"] != null && Request["salaryMonth"].Trim().Length > 0 ? Request["salaryMonth"].Trim() : null;
            this.SalaryQuery_SalaryMonth.Value = salaryMonth;

            List<SalaryEntry> salaryEntries = new List<SalaryEntry>();


            if (!(teacherName != null && (teacherNo == null || teacherNo.Trim().Length == 0)))
            {
                if (teacherNo == null && termTag == null && salaryMonth == null && teacherType == "0" && salaryEntryStatus == "0")
                {
                    salaryEntries = dal.GetAllSalaryEntry();
                }
                else {
                    salaryEntries = dal.GetSalaryEntrys(teacherNo, termTag, salaryMonth, int.Parse(teacherType), int.Parse(salaryEntryStatus));
                }
                
            }
            TotalSalaryModel selectedTotalSalaryModel = dal.GetTotalSalaryEntryValues(teacherNo, termTag, salaryMonth, int.Parse(teacherType), int.Parse(salaryEntryStatus));
            TotalSalaryModel allTotalSalaryModel = dal.GetTotalSalaryEntryValues(null, null, null, 0, 0);

            this.literal_SelectedSalaryWithTax.Text = selectedTotalSalaryModel.salaryWithTax.ToString();
            this.literal_SelectedSalaryWithoutTax.Text = selectedTotalSalaryModel.salaryWithoutTax.ToString();
            this.literal_SelectedTotalSalary.Text = selectedTotalSalaryModel.salaryTotal.ToString();

            this.literal_AllSalaryWithTax.Text = allTotalSalaryModel.salaryWithTax.ToString();
            this.literal_AllSalaryWithoutTax.Text = allTotalSalaryModel.salaryWithoutTax.ToString();
            this.literal_AllTotalSalary.Text = allTotalSalaryModel.salaryTotal.ToString();
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
        private void DataBindSalaryStatus(ListItemCollection listItemCollection, string salaryEntryStatus)
        {
            if (salaryEntryStatus != null)
            {
                salaryEntryStatus = salaryEntryStatus.Trim();
                foreach (ListItem li in listItemCollection)
                {
                    if (li.Value == salaryEntryStatus)
                    {
                        li.Selected = true;
                        break;
                    }
                }

            }
        }
        private void DataListBindSalaryItem()
        {
            DalOperationAboutSalaryItem dal = new DalOperationAboutSalaryItem();
            List<SalaryItem> items = dal.GetAllSalaryItem();

            if (items != null)
            {
                this.SalaryItemPager.RecordCount = items.Count;
                SalaryItemPager.PageSize = CommonUtility.pageSize;

                PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
                pds.DataSource = items;
                pds.AllowPaging = true;

                pds.CurrentPageIndex = pageIndex - 1;
                pds.PageSize = SalaryItemPager.PageSize;

                this.SalaryItemList.DataSource = pds;
                this.SalaryItemList.DataBind();

            }
            else
            {
                this.SalaryItemPager.RecordCount = 0;
            }

            if (SalaryItemPager.RecordCount > 0)
            {
                this.SalaryItemList.ShowFooter = false;
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
            if (termValue == "all")
            {
                itemAll.Selected = true;
            }
            itemCollection.Add(itemAll);
        }

        public void DataBindTeacherTypeList(ListItemCollection itemCollection, string teacherType) 
        {
            itemCollection.Clear();
            ListItem[] listItems = new ListItem[]
            {
              new ListItem("所有教师", "0"),
              new ListItem("院内教师/助教", "1"),
              new ListItem("院外教师", "2"),
              new ListItem("院外助教", "3")
            };
            if(teacherType != null && teacherType.Trim().Length != 0)
            foreach(ListItem listItem in listItems)
            {
                if(listItem.Value == teacherType.Trim())
                {
                    listItem.Selected = true;
                    break;
                }
            }
            itemCollection.AddRange(listItems);
        }

        private string SearchTeacherIds(string keyWord)
        {

            DalOperationUsers dos = new DalOperationUsers();
            DataView dv = dos.SearchUserByTypeAndKeywod(1, keyWord).DefaultView;
            DataRowCollection drCollection = dv.Table.Rows;
            List<string> tidList = new List<string>();

            for (var i = 0; i < drCollection.Count; i++)
            {
                string s = drCollection[i]["teacherNo"].ToString();
                tidList.Add("'" + s + "'");
            }

            string tids = string.Join(",", tidList.ToArray());
            return tids;
        }

        private void DataListBindTeacherSalary()
        {
            DalOperationAboutTeacherSalary dal = new DalOperationAboutTeacherSalary();
            string teacherNo = null;
            string teacherName = Request["teacherName"];
            if (teacherName != null && teacherName.Trim().Length > 0)
            {
                teacherName = teacherName.Trim();
                teacherNo = SearchTeacherIds(teacherName);
                this.TeacherSalary_TeacherName.Text = teacherName;
            }
            else
            {
                teacherName = null;
            }
            string termTag = Request["termTag"];
            string teacherType = Request["teacherType"];
            string status = Request["status"];

            DataBindSearchTermTagList(this.TeacherSalary_TermTag.Items, termTag);
            DataBindTeacherTypeList(this.TeacherSalary_TeacherType.Items, teacherType);
            DataBindSalaryStatus(this.TeacherSalary_Status.Items, status);

            if (string.IsNullOrWhiteSpace(termTag))
            {
                termTag = this.TeacherSalary_TermTag.SelectedValue;
            }
            else if (termTag.Trim() == "all") 
            {
                termTag = null;
            }

            if (string.IsNullOrWhiteSpace(teacherType))
            {
                teacherType = "0";
            }
            else
            {
                teacherType = teacherType.Trim();
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                status = "0";
            }
            else
            {
                status = status.Trim();
            }

            List<TeacherSalary> teacherSalaries = new List<TeacherSalary>();

            if (!(teacherName != null && string.IsNullOrWhiteSpace(teacherNo)))
            {
                if (teacherNo == null && teacherType == "0" && termTag == null && status == "0")
                {
                    teacherSalaries = dal.GetAllTeacherSalary();
                }
                else {
                    teacherSalaries = dal.GetTeacherSalarys(teacherNo, int.Parse(teacherType), termTag, int.Parse(status));
                }
            }

            double totalTeacherSummary = dal.GetTeacherSalarysValue(null, 0, null, 0);
            double selectedTeacherSummary = dal.GetTeacherSalarysValue(teacherNo, int.Parse(teacherType), termTag, int.Parse(status));

            this.literal_TotalTeacherSummay.Text = totalTeacherSummary.ToString();
            this.literal_SelectedTeacherSummay.Text = selectedTeacherSummary.ToString();

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

        protected void SalaryManage_ItemBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("SalaryItemList");
                SalaryItem salaryItem = (SalaryItem)e.Item.DataItem;
                int salaryItemId = salaryItem.salaryItemId;

                if (salaryItem.salaryItemStatus == 1)
                {
                    e.Item.FindControl("LinkButton_SalaryItemHidden").Visible = true;
                    e.Item.FindControl("LinkButton_SalaryItemShow").Visible = false;
                }
                else
                {
                    e.Item.FindControl("LinkButton_SalaryItemHidden").Visible = false;
                    e.Item.FindControl("LinkButton_SalaryItemShow").Visible = true;
                }

                ((Literal)e.Item.FindControl("SalaryItem_Literal")).Text = CommonUtility.ConvertTeacherType2String(salaryItem.useFor);
            }
        }

        protected void SalaryEntryManage_ItemBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("dlstSecondNorm");
                SalaryEntry salaryEntry = (SalaryEntry)e.Item.DataItem;

                ((Literal)e.Item.FindControl("literal_SalaryEntryStatus")).Text = CommonUtility.ConvertSalaryEntryStatus(salaryEntry.salaryEntryStatus);
            }

        }

        protected void SalaryQuery_Click(object source, EventArgs e)
        {
            string teacherName = null;
            if (this.SalaryQuery_Name.Text != null && this.SalaryQuery_Name.Text.Trim().Length > 0)
            {
                teacherName = this.SalaryQuery_Name.Text.Trim();
            }
            string termTag = this.SalaryQuery_TermTag.SelectedValue;
            string teacherType = this.SalaryQuery_TeacherType.SelectedValue;
            string salaryMonth = this.SalaryQuery_SalaryMonth.Value != null && this.SalaryQuery_SalaryMonth.Value.Trim().Length > 0 ? this.SalaryQuery_SalaryMonth.Value.Trim() : null;

            string salaryStatus = this.SalaryQuery_SalaryEntryStatus.SelectedValue;

            Javascript.JavaScriptLocationHref("/Teacher/SalaryManage.aspx?fragment=5&teacherName=" + teacherName + "&teacherType=" + teacherType + "&termTag=" + termTag + "&salaryMonth=" + salaryMonth + "&status=" + salaryStatus, Page);
        }

        protected void SalaryQuery_TermTagChanged(object source, EventArgs e)
        {
            this.SalaryQuery_Click(source, e);
        }

        protected void SalaryItemPager_PageChanged(object sender, EventArgs e)
        {
            DataListBindSalaryItem();
        }

        protected void TeacherSalaryPager_PageChanged(object sender, EventArgs e)
        {
            DataListBindTeacherSalary();
        }

        protected void SalaryEntryPager_PageChanged(object sender, EventArgs e)
        {
            DataListBindSalaryEntry();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        protected void TeacherSalaryQuery_Click(object sender, EventArgs e)
        {
            string teacherName = null;
            if (this.TeacherSalary_TeacherName.Text != null && this.TeacherSalary_TeacherName.Text.Trim().Length > 0)
            {
                teacherName = this.TeacherSalary_TeacherName.Text.Trim();
            }
            string termTag = this.TeacherSalary_TermTag.SelectedValue;
            string teacherType = this.TeacherSalary_TeacherType.SelectedValue;
            string status = this.TeacherSalary_Status.SelectedValue;
            Javascript.JavaScriptLocationHref("/Teacher/SalaryManage.aspx?fragment=3&teacherName=" + teacherName + "&teacherType=" + teacherType + "&termTag=" + termTag + "&status=" + status, Page);
        }

        protected void TeacherSalary_TermTagChanged(object sender, EventArgs e)
        {
            this.TeacherSalaryQuery_Click(sender, e);

        }
    }
}