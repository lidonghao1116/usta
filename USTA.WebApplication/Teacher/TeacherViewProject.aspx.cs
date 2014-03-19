using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Dal;
using USTA.Common;
using System.Data;
using USTA.Model;
using Wuqi.Webdiyer;
using USTA.Bll;

namespace USTA.WebApplication.Teacher
{
    public partial class TeacherViewProject : System.Web.UI.Page
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);

        DalOperationAboutReim dalreim = new DalOperationAboutReim();
        DalOperationAboutProjectCategory dalProCate = new DalOperationAboutProjectCategory();
        protected void Page_Load(object sender, EventArgs e)
        {
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }
            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2,
                divFragment1, divFragment2);

            if (!IsPostBack)
            {
                if (fragmentFlag.Equals("1"))
                {
                    BindDataListProjectFragment();
                }
                else if (fragmentFlag.Equals("2"))
                {
                    BindDataListReimEntryFragment();
                }
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



        private void BindDataListReimEntryFragment()
        {
            DalOperationAboutReimItem dalReimItem = new DalOperationAboutReimItem();
            string projectId = Request["projectId"];
            string projectName = Request["pname"];

            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            string userId = userCookiesInfo.userNo;
            
            List<ReimItem> reimItemList;
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                reimItemList = dalReimItem.GetReimItemSummaryValues(null, int.Parse(projectId.Trim()), 0, userId.Trim());
                DalOperationAboutProject dalProject = new DalOperationAboutProject();
                Project project = dalProject.GetProject(int.Parse(projectId));
                if (project != null)
                {
                    this.tb_projectName.Text = project.name;
                }

            }
            else
            {
                reimItemList = dalReimItem.GetReimItemSummaryValues(null, 0, 0, userId.Trim());
                this.tb_projectName.Text = projectName;
            }


            PagerCommon(this.ReimEntryPager, this.ReimEntryDataList, reimItemList);

        }
        private void BindDataListProjectFragment()
        {
            DalOperationAboutProject dalProject = new DalOperationAboutProject();
            string projectName = Request["pname"];

            UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            string userId = userCookiesInfo.userNo;
            List<Project> projectList = dalProject.GetProjects(userId, projectName);

            PagerCommon(this.ProjectDataList_Pager, this.ProjectDataList, projectList);
        }


        protected void QueryReimEntry_Click(object source, EventArgs e)
        {
            string projectName = this.tb_projectName.Text.Trim();
            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=5&pname=" + projectName, Page);
        }


        protected void SearchProject_Click(object source, EventArgs e)
        {
            string projectName = this.tb_ProjectName_ProjectFrag.Text.Trim();

            Javascript.JavaScriptLocationHref("/Teacher/ProjectManager.aspx?fragment=1&pname=" + projectName, Page);

        }

        protected void ProjectDataListPager_PageChanged(object sender, EventArgs e)
        {
            //TODO
        }

        protected void ReimEntryPager_PageChanged(object sender, EventArgs e)
        {
            //TODO
        }

       

    }
}