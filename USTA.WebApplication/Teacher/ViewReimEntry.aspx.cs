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
    public partial class ViewReimEntry : CheckUserWithCommonPageBase
    {
        private string pageName = "ProjectManager";

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
                string projectId = Request["projectId"];
                string reimId = Request["reimId"];
                if (string.IsNullOrWhiteSpace(projectId) || string.IsNullOrWhiteSpace(reimId))
                {
                    Javascript.Alert("您未指定要查看的报销记录!", Page);
                    Javascript.RefreshParentWindowReload(Page);
                }
                else 
                {
                    
                    DalOperationAboutReim dalReim = new DalOperationAboutReim();
                    DalOperationAboutProject dalProject = new DalOperationAboutProject();
                    DalOperationAboutReimItem dalReimItem = new DalOperationAboutReimItem();

                    Project project = dalProject.GetProject(int.Parse(projectId.Trim()));
                    Reim reim = dalReim.GetReim(int.Parse(reimId.Trim()));

                    
                    if (project == null || reim == null)
                    {
                        Javascript.Alert("您指定的项目或报销项不存在!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                    else {

                        this.ReimEntry_ProjectValue.Text = dalReimItem.GetReimItemValue(int.Parse(projectId), 0).ToString();
                        this.ReimEntry_ProjectReimValue.Text = dalReimItem.GetReimItemValue(int.Parse(projectId.Trim()), int.Parse(reimId.Trim())).ToString();

                        UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
                        string cookieUserNo = userCookiesInfo.userNo;

                        if (project.userNo == cookieUserNo || isAuth(cookieUserNo))
                        {
                            List<ReimItem> reimItems = dalReimItem.GetReimItemsForProjectAndReim(int.Parse(projectId.Trim()), int.Parse(reimId.Trim()));

                            this.ReimEntry_ProjectName.Text = project.name;
                            this.ReimEntry_ReimName.Text = reim.name;
                            if (reimItems == null || reimItems.Count == 0)
                            {
                                this.ReimEntry_ReimItemList.ShowFooter = true;
                            }
                            else
                            {
                                this.ReimEntry_ReimItemList.DataSource = reimItems;
                                this.ReimEntry_ReimItemList.DataBind();
                                this.ReimEntry_ReimItemList.ShowFooter = false;
                            }
                        }
                        else {

                            Javascript.Alert("您无权查看此页面!", Page);
                            Javascript.RefreshParentWindowReload(Page);
                        }
                    }
                }
            
            }
        }

        protected void ReimItemList_Command(object source, DataListCommandEventArgs e)
        {
            int reimItemSelected = int.Parse(this.ReimEntry_ReimItemList.DataKeys[e.Item.ItemIndex].ToString()); //取选中行报销记录
            DalOperationAboutReimItem dalReimItem = new DalOperationAboutReimItem();
            ReimItem reimItem = dalReimItem.GetReimItem(reimItemSelected);
            if (reimItem == null)
            {
                Javascript.Alert("您所操作的数据不存在!", Page);
            }
            else {
                if (e.CommandName == "delReimItem")
                {

                    dalReimItem.DelReimItem(reimItemSelected);
                    Javascript.JavaScriptLocationHref("/Teacher/ViewReimEntry.aspx?projectId=" + reimItem.project.id + "&reimId=" + reimItem.reim.id, Page);
                }
            }
            
        }
    }
}