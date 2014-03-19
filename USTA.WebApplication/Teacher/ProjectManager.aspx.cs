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

using Wuqi.Webdiyer;
using USTA.Bll;

namespace USTA.WebApplication.Teacher
{
    public partial class ProjectManager : System.Web.UI.Page
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 

        DataTable categorydt;
        DalOperationAboutReim dalreim = new DalOperationAboutReim();
        DalOperationAboutProjectCategory dalProCate = new DalOperationAboutProjectCategory();

        private static string pageName = "ProjectManager";
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
            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            if (!isAuth(userCookiesInfo.userNo))
            {
                Javascript.Alert("你无权查看此页面", Page);
                Javascript.RefreshParentWindow("/Common/NotifyList.aspx", Page);
            }
            else {
                string fragmentFlag = "1";

                if (Request["fragment"] != null)
                {
                    fragmentFlag = Request["fragment"];
                }
                CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, liFragment4, liFragment5, liFragment6
                   , divFragment1, divFragment2, divFragment3, divFragment4, divFragment5, divFragment6);

                if (!IsPostBack)
                {
                    if (fragmentFlag.Equals("1"))
                    {
                        BindDataListProjectFragment();
                    }
                    else if (fragmentFlag.Equals("2"))
                    {
                    }
                    else if (fragmentFlag.Equals("3"))
                    {
                        BindDataListProjectCategoryFragment();

                    }
                    else if (fragmentFlag.Equals("4"))
                    {
                        BindDataListReimFragment();
                    }
                    else if (fragmentFlag.Equals("5"))
                    {

                        BindDataListReimEntryFragment();
                    }
                    else if (fragmentFlag.Equals("6"))
                    {
                        BindDataListReimRuleFragment();
                    }
                }
            
            }
        }

        private void BindDataDropList(ListItemCollection items, List<ProjectCategory> categories, int selectedIndex) 
        {
            ListItem li;
            foreach(ProjectCategory category in categories)
            {
                li = new ListItem(category.name, category.id.ToString());
                if(category.id == selectedIndex){
                    li.Selected = true;
                }
                items.Add(li);
            }
        }

        private void PagerCommon<T>(AspNetPager pager, DataList dataList, List<T> list) 
        {
            if (list != null)
            {

                pager.RecordCount = list.Count;
                pager.PageSize = CommonUtility.pageSize;

                PagedDataSource pds = new PagedDataSource();
                pds.DataSource = list;
                pds.AllowPaging = true;

                pds.CurrentPageIndex = pageIndex - 1;
                pds.PageSize = pager.PageSize;

                dataList.DataSource = pds;
                dataList.DataBind();
            }
        
        }

        private void BindDataListReimFragment() 
        {
            DalOperationAboutReim dalReim = new DalOperationAboutReim();
            List<Reim> reimList = dalReim.GetAllReims();

            PagerCommon(this.ReimPager, this.ReimDataList, reimList);

        
        }

        private void BindDataListReimEntryFragment() 
        {
            DalOperationAboutReimItem dalReimItem = new DalOperationAboutReimItem();
            string projectId = Request["projectId"];
            string projectName = Request["pname"];
            List<ReimItem> reimItemList;
            float currentReimValue = 0.0f;
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                reimItemList = dalReimItem.GetReimItemSummaryValues(null, int.Parse(projectId.Trim()), 0, null);
                DalOperationAboutProject dalProject = new  DalOperationAboutProject();
                Project project = dalProject.GetProject(int.Parse(projectId));
                if (project != null) {
                    this.tb_projectName.Text = project.name;
                    currentReimValue = dalReimItem.GetReimItemValue(project.id, 0);
                }

            }
            else if (!string.IsNullOrWhiteSpace(projectName)) {
                reimItemList = dalReimItem.GetReimItemSummaryValues(projectName.Trim(), 0, 0, null);
                this.tb_projectName.Text = projectName;
                currentReimValue = dalReimItem.GetReimItemValue(projectName.Trim(), 0);
            }
            else
            {
                reimItemList = dalReimItem.GetAllReimItems();
                currentReimValue = dalReimItem.GetReimItemValue(0, 0);
            }

            this.literal_CurrentReimValue.Text = currentReimValue.ToString();
            PagerCommon(this.ReimEntryPager, this.ReimEntryDataList, reimItemList);
            
        }

        private void BindDataListReimRuleFragment() 
        {
            DalOperationAboutProjectReimRule dalRule = new DalOperationAboutProjectReimRule();
            string projectId = Request["projectId"];
            string projectName = Request["pname"];
            List<ProjectReimRule> reimRuleList;
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                reimRuleList = dalRule.GetProjectReimRules(null, int.Parse(projectId.Trim()), 0);
                DalOperationAboutProject dalProject = new DalOperationAboutProject();
                Project project = dalProject.GetProject(int.Parse(projectId));
                if (project != null)
                {
                    this.tb_projectName_Rule.Text = project.name;
                }

            }
            else if (!string.IsNullOrWhiteSpace(projectName))
            {
                reimRuleList = dalRule.GetProjectReimRules(projectName.Trim(), 0, 0);
                this.tb_projectName_Rule.Text = projectName;
            }
            else
            {
                reimRuleList = dalRule.GetProjectReimRules(null, 0, 0);
            }


            PagerCommon(this.ReimEntryPager, this.ddlProjectRuleList, reimRuleList);
        
        }

        private void BindDataListProjectFragment() 
        {
            DalOperationAboutProject dalProject = new DalOperationAboutProject();
            string projectName = Request["pname"];
            string userName = Request["uname"];
            string rootCategoryId = Request["rc"];
            string subCategoryId = Request["sc"];
            string thirdCategoryId = Request["tc"];

            BindRootProjectCategory(this.SearchProject_RootCategory.Items, string.IsNullOrWhiteSpace(rootCategoryId) ? 0 : int.Parse(rootCategoryId));
            BindSubProjectCategory(this.SearchProject_SubCategory.Items, string.IsNullOrWhiteSpace(subCategoryId) ? 0 : int.Parse(subCategoryId), string.IsNullOrWhiteSpace(rootCategoryId) ? 0 : int.Parse(rootCategoryId));
            BindThirdProjectCategory(this.SearchProject_ThirdCategory.Items, string.IsNullOrWhiteSpace(thirdCategoryId) ? 0 : int.Parse(thirdCategoryId), string.IsNullOrWhiteSpace(subCategoryId) ? 0 : int.Parse(subCategoryId));

            List<ProjectCategory> queryCategoryList = new List<ProjectCategory>();
            if (!(string.IsNullOrWhiteSpace(thirdCategoryId) || "0" == thirdCategoryId.Trim())) 
            {
                queryCategoryList.Add(dalProCate.GetProjectCategoryById(int.Parse(thirdCategoryId.Trim())));
            }
            else if (!(string.IsNullOrWhiteSpace(subCategoryId) || "0" == subCategoryId.Trim())) 
            {
                queryCategoryList.AddRange(dalProCate.GetAllLastProjectCategoryByParentId(int.Parse(subCategoryId.Trim())));
            }
            else if (!(string.IsNullOrWhiteSpace(rootCategoryId) || "0" == rootCategoryId.Trim())) {

                queryCategoryList.AddRange(dalProCate.GetAllLastProjectCategoryByParentId(int.Parse(rootCategoryId.Trim())));
            }

            string categoryIds = null;

            if (queryCategoryList.Count != 0) {
                List<int> categoryIdList = new List<int>();
                foreach (ProjectCategory category in queryCategoryList) 
                {
                    categoryIdList.Add(category.id);
                }

                categoryIds = string.Join(",", categoryIdList.ToArray());
            }


            List<Project> projectList;
            if (string.IsNullOrWhiteSpace(projectName) && string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(categoryIds))
            {
                projectList = dalProject.GetAllProjects();
            }
            else 
            {
                projectList = dalProject.GetPrjects(categoryIds, userName.Trim(), projectName.Trim());
                this.tb_ProjectName_ProjectFrag.Text = projectName.Trim();
                this.tb_UserName_ProjectFrag.Text = userName.Trim();
            }
            
            
            PagerCommon(this.ProjectDataList_Pager, this.ProjectDataList, projectList);
            
        }

        private void BindRootProjectCategory(ListItemCollection itemCollection, int selectedIndex) 
        {
            itemCollection.Clear();
            itemCollection.Add(new ListItem("选择一级类目", "0"));
            List<ProjectCategory> rootCategoryList = dalProCate.GetProjectCategoryByParendId(0);
            BindDataDropList(itemCollection, rootCategoryList, selectedIndex);
        
        }

        private void BindSubProjectCategory(ListItemCollection itemCollection, int selectedIndex, int parendId) 
        {
            BindSubAndThirdCategoryCommon(itemCollection, selectedIndex, parendId, "选择二级类目");
        }

        private void BindThirdProjectCategory(ListItemCollection itemCollection, int selectedIndex, int parendId) 
        {
            BindSubAndThirdCategoryCommon(itemCollection, selectedIndex, parendId, "选择三级类目");
        }

        private void BindSubAndThirdCategoryCommon(ListItemCollection itemCollection, int selectedIndex, int parendId, string text) 
        {
            itemCollection.Clear();
            itemCollection.Add(new ListItem(text, "0"));
            if (parendId != 0)
            {
                List<ProjectCategory> subCategoryList = dalProCate.GetProjectCategoryByParendId(parendId);
                BindDataDropList(itemCollection, subCategoryList, selectedIndex);
            }
        
        
        }


        private void BindDataListProjectCategoryFragment() {

            string categoryId = Request["categoryId"];
            string categoryLevel = Request["cl"];
            string rootCategoryId = Request["rc"];
            string subCategoryId = Request["sc"];

            BindRootProjectCategory(this.RootProjectCategory.Items, string.IsNullOrWhiteSpace(rootCategoryId) ? 0 : int.Parse(rootCategoryId.Trim()));
            
            List<ProjectCategory> categoryList = null;

            if (string.IsNullOrWhiteSpace(rootCategoryId) || "0" == rootCategoryId.Trim())
            {
                BindSubProjectCategory(this.SubProjectCategory.Items, 0, 0);

               if (string.IsNullOrWhiteSpace(categoryLevel) || "0" == categoryLevel.Trim())
                {
                    categoryList = dalProCate.GetAllProjectCategory();
                }
                else
                {
                    categoryLevel = categoryLevel.Trim();
                    categoryList = dalProCate.GetProjectCategoryByLevel(int.Parse(categoryLevel));
                    ListItemCollection items = this.ProjectCategoryDdList.Items;
                    foreach (ListItem item in items)
                    {
                        if (item.Value == categoryLevel)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
            else {
                
                BindSubProjectCategory(this.SubProjectCategory.Items, string.IsNullOrWhiteSpace(subCategoryId) ? 0 : int.Parse(subCategoryId.Trim()), int.Parse(rootCategoryId.Trim()));

                if (string.IsNullOrWhiteSpace(subCategoryId) || "0" == subCategoryId.Trim())
                {
                    categoryList = dalProCate.GetAllProjectCategoryByParendId(int.Parse(rootCategoryId.Trim()));
                }
                else {
                    categoryList = dalProCate.GetProjectCategoryByParendId(int.Parse(subCategoryId.Trim()));
                }
            }

            PagerCommon(this.ProjectCategoryPager, this.dlstCategory, categoryList);

        }

        protected void QueryReimEntry_Click(object source, EventArgs e) 
        {
            string projectName = this.tb_projectName.Text.Trim();
            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=5&pname=" + projectName, Page);
        }

        protected void QueryProjectRule_Click(object source, EventArgs e) 
        {
            string projectName = this.tb_projectName_Rule.Text.Trim();
            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=6&pname=" + projectName, Page);
        }

        protected void ProjectCategoryDdList_Changed(object source, EventArgs e)
        {
            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=3&cl=" + this.ProjectCategoryDdList.SelectedValue, Page);
        }

        protected void RootCategory_Changed(object source, EventArgs e)
        {
            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=3&rc=" + this.RootProjectCategory.SelectedValue, Page);
        }

        protected void SubCategory_Changed(object source, EventArgs e)
        {
            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=3&rc=" + this.RootProjectCategory.SelectedValue + "&sc=" + this.SubProjectCategory.SelectedValue, Page);
        }

        private void SearchProjectCommon(int categoryLevel) {
            string projectName = this.tb_ProjectName_ProjectFrag.Text.Trim();
            string userName = this.tb_UserName_ProjectFrag.Text.Trim();
            string rootCategoryId = null;
            string subCategoryId = null;
            string thirdCategoryId = null;

            if (categoryLevel >= 1)
            {
                rootCategoryId = this.SearchProject_RootCategory.SelectedValue.Trim();
            }

            if(categoryLevel >= 2) {
                subCategoryId = this.SearchProject_SubCategory.SelectedValue.Trim();
            }

            if (categoryLevel >= 3) 
            {
                thirdCategoryId = this.SearchProject_ThirdCategory.SelectedValue.Trim();
            }
            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=1&pname=" + projectName + "&uname=" + userName + "&rc=" + rootCategoryId + "&sc=" + subCategoryId + "&tc=" + thirdCategoryId, Page);
        
         }

        protected void SearchProject_Click(object source, EventArgs e) 
        {
            SearchProjectCommon(3);
        
        }

        protected void SearchProject_RootCategory_Changed(object source, EventArgs e) 
        {

            SearchProjectCommon(1);
        }

        protected void SearchProject_SubCategory_Changed(object source, EventArgs e)
        {
            SearchProjectCommon(2);
        }

        protected void SearchProject_ThirdCategory_Changed(object source, EventArgs e)
        {
            SearchProjectCommon(3);
        }



        protected void ProjectCategoryPager_PageChanged(object sender, EventArgs e)
        {
            BindDataListProjectCategoryFragment();
            
        }

        protected void ProjectDataListPager_PageChanged(object sender, EventArgs e) 
        {
            //TODO
        }

        protected void ReimEntryPager_PageChanged(object sender, EventArgs e) 
        {
            //TODO
        }

        protected void ReimPager_PageChanged(object sender, EventArgs e) 
        {
            //TODO
        }

        protected void ProjectRulePager_PageChanged(object sender, EventArgs e) 
        {
            //TODO
        }

        protected void ProjectRuleCommand_Click(object source, DataListCommandEventArgs e) 
        {

            if (e.CommandName == "deleteRule") {
                DalOperationAboutProjectReimRule dalRule = new DalOperationAboutProjectReimRule();
                string ruleId = this.ddlProjectRuleList.DataKeys[e.Item.ItemIndex].ToString();
                dalRule.DelProjectReimRule(int.Parse(ruleId));
                Javascript.Alert("操作成功", Page);
                Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=6&projectName=" + this.tb_projectName_Rule.Text.Trim(), Page);
            }
        }
        
    }
}