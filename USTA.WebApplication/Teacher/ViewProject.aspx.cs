using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.PageBase;

using USTA.Dal;
using USTA.Model;
using USTA.Common;
using USTA.Bll;

namespace USTA.WebApplication.Teacher
{
    public partial class ViewProject : CheckUserWithCommonPageBase
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
                if (string.IsNullOrWhiteSpace(projectId))
                {
                    Javascript.Alert("您未指定要查看的项目!", Page);
                    Javascript.RefreshParentWindowReload(Page);
                }
                else 
                {
                    DalOperationAboutProject dalProject = new DalOperationAboutProject();
                    
                    Project project = dalProject.GetProject(int.Parse(projectId));
                    if (project == null)
                    {
                        Javascript.Alert("您查看的项目不存在!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                    else {

                        UserCookiesInfo userCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
                        string cookieUserNo = userCookiesInfo.userNo;

                        if (project.userNo == cookieUserNo || isAuth(cookieUserNo))
                        {

                            this.literal_ViewProjectName.Text = project.name;
                            this.literal_UserName.Text = project.userName;


                            DalOperationAboutProjectCategory dalProCate = new DalOperationAboutProjectCategory();
                            List<ProjectCategory> categoryList = dalProCate.GetProjectCategoryPathById(project.category.id);
                            string categoryPath = "";
                            bool hasPrefix = false;
                            foreach (ProjectCategory category in categoryList)
                            {
                                categoryPath += ((hasPrefix ? " -> " : "") + category.name);
                                hasPrefix = true;
                            }

                            this.literal_ViewProjectCategory.Text = categoryPath;

                            this.literal_CreatedTime.Text = project.createdTime.ToString();
                            this.literal_ProjectMemo.Text = project.memo;
                        }
                        else {
                            Javascript.Alert("您无权查看此页面!", Page);
                            Javascript.RefreshParentWindowReload(Page);
                        }
                    }
                }
            
            }
        }
    }
}