using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Model;
using USTA.Dal;
using System.Data;
using USTA.PageBase;
using USTA.Common;
using System.Collections;
using System.Threading;
using System.Globalization;

namespace USTA.WebApplication.Teacher
{
    public partial class AddProject : CheckUserWithCommonPageBase
    {

        private DalOperationAboutProjectCategory dalCategory = new DalOperationAboutProjectCategory();
        private DalOperationAboutTeacher dalTeacher = new DalOperationAboutTeacher();
        private DalOperationAboutProject dalProject = new DalOperationAboutProject();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string operation = Request["op"];
                if (!string.IsNullOrWhiteSpace(operation) && "edit" == operation)
                {
                    string pid = Request["pid"];
                    bool isError = false;
                    if (string.IsNullOrWhiteSpace(pid))
                    {
                        isError = true;
                    }
                    else {

                        Project project = dalProject.GetProject(int.Parse(pid.Trim()));
                        if (project == null)
                        {
                            isError = true;
                        }
                        else {
                            this.hf_ProjectId.Value = project.id.ToString();
                            this.NewProjectName.Text = project.name;
                            this.ProjectDesc.Text = project.memo;
                            List<ProjectCategory> categoryPath = dalCategory.GetProjectCategoryPathById(project.category.id);

                            BindProjectCategory(this.RootProjectCategory.Items, dalCategory.GetProjectCategoryByParendId(0), categoryPath[0].id);
                            BindProjectCategory(this.SubProjectCategory.Items, dalCategory.GetProjectCategoryByParendId(categoryPath[0].id), categoryPath[1].id);
                            BindProjectCategory(this.ThirdProjectCategory.Items, dalCategory.GetProjectCategoryByParendId(categoryPath[1].id), categoryPath[2].id);

                            BindProjectUser(project.userNo);
                            this.btn_AddNewProject.Text = "修改";
                        }
                    }
                    if (isError) {
                        Javascript.Alert("您所操作的数据不存在或已被删除!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                }
                else {
                    BindProjectCategory(this.RootProjectCategory.Items, dalCategory.GetProjectCategoryByParendId(0), 0);
                    BindProjectUser("0");
                }
                
            }
        }

        private void ClearCategoryAndDefault(ListItemCollection itemCollection) 
        {
            itemCollection.Clear();
            itemCollection.Add(new ListItem("-请选择-", "0"));
        }

        private void BindProjectUser(string selectedTeacherNo) 
        {
            DataView dv = dalTeacher.GetTeachers().Tables[0].DefaultView;
            List<TeachersList> userTeachers = new List<TeachersList>();
            for (int i = 0; i < dv.Count; i++) 
            {
                TeachersList teacher = new TeachersList()
                {
                    teacherNo = dv[i]["teacherNo"].ToString(),
                    teacherName = dv[i]["teacherName"].ToString() + "（" + (string.IsNullOrWhiteSpace(dv[i]["employeeNum"].ToString()) ? "暂无工号" : dv[i]["employeeNum"].ToString()) + "）"

                };
                userTeachers.Add(teacher);
            }

            userTeachers.Sort(new UserNameComparor());

            ListItemCollection itemCollections = this.ProjectUserName.Items;
            for (int i = 0; i < userTeachers.Count; i++) 
            {
                ListItem li = new ListItem(userTeachers[i].teacherName, userTeachers[i].teacherNo);
                if(selectedTeacherNo == userTeachers[i].teacherNo){
                    li.Selected = true;
                }
                itemCollections.Add(li);
            }
        }

        private void BindProjectCategory(ListItemCollection itemCollection, List<ProjectCategory> categoryList, int selectedValue) 
        {
            ClearCategoryAndDefault(itemCollection);
            foreach(ProjectCategory category in categoryList)
            {
                ListItem li = new ListItem(category.name, category.id.ToString());
                if (selectedValue == category.id) 
                {
                    li.Selected = true;
                }
                itemCollection.Add(li);
            }
        }

        protected void RootCategoryChanged(object sender, EventArgs e) 
        {
            string selectedValue = this.RootProjectCategory.SelectedValue;
            if (selectedValue == null || selectedValue.Trim() == "0")
            {
                ClearCategoryAndDefault(this.SubProjectCategory.Items);
                ClearCategoryAndDefault(this.ThirdProjectCategory.Items);
            }
            else 
            {
                BindProjectCategory(this.SubProjectCategory.Items, dalCategory.GetProjectCategoryByParendId(int.Parse(selectedValue)), 0);
            }
        }



        protected void SubCategoryChanged(object sender, EventArgs e) 
        {
            string selectedValue = this.SubProjectCategory.SelectedValue;
            if (selectedValue == null || selectedValue.Trim() == "0")
            {
                ClearCategoryAndDefault(this.ThirdProjectCategory.Items);
            }
            else
            {
                BindProjectCategory(this.ThirdProjectCategory.Items, dalCategory.GetProjectCategoryByParendId(int.Parse(selectedValue)), 0);
            }
        
        }

        protected void ThirdCategoryChanged(object sender, EventArgs e) 
        {
        
        
        }

        protected void AddNewProject_Click(object sender, EventArgs e) 
        {
            string teacherNo = this.ProjectUserName.SelectedValue;
            string categoryId = this.ThirdProjectCategory.SelectedValue;
            bool isError = false;
            if (teacherNo == null || teacherNo.Trim() == "0") 
            {
                isError = true;
            }
            if (categoryId == null || categoryId.Trim() == "0")
            {
                isError = true;
            }

            if (!isError)
            {
                if ("修改" == this.btn_AddNewProject.Text)
                {
                    Project project = dalProject.GetProject(int.Parse(this.hf_ProjectId.Value.Trim()));
                    project.name = this.NewProjectName.Text.Trim();
                    project.memo = this.ProjectDesc.Text.Trim();
                    project.category.id = int.Parse(categoryId.Trim());
                    project.userNo = teacherNo;
                    project.userName = this.ProjectUserName.SelectedItem.Text;

                    dalProject.UpdateProject(project);

                }
                else {
                    Project project = new Project();
                    project.name = this.NewProjectName.Text.Trim();
                    project.memo = this.ProjectDesc.Text.Trim();
                    ProjectCategory category = new ProjectCategory()
                    {
                        id = int.Parse(categoryId.Trim())
                    };
                    project.category = category;
                    project.userName = this.ProjectUserName.SelectedItem.Text;
                    project.userNo = teacherNo;
                    dalProject.AddProject(project);
                
                }
                
                Javascript.Alert("操作成功!", Page);
                Javascript.RefreshParentWindowReload(Page);

            }
            else 
            {
                Javascript.Alert("您未选择项目的类目或项目的负责人，请核对后再完成操作!", Page);
            }
        }

        private class UserNameComparor : IComparer<TeachersList> {

            public int Compare(TeachersList t1, TeachersList t2) 
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
                return t1.teacherName.CompareTo(t2.teacherName);
            }
        }
    }
}