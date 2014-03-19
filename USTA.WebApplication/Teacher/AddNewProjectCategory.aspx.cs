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

namespace USTA.WebApplication.Teacher
{
    public partial class AddNewProjectCategory : CheckUserWithCommonPageBase
    {
        private DalOperationAboutProjectCategory dalpc = new DalOperationAboutProjectCategory();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {

                string operation = Request["op"];
                if (!string.IsNullOrWhiteSpace(operation) && "edit" == operation)
                {
                    string cid = Request["cid"];
                    bool isError = false;
                    if (!string.IsNullOrWhiteSpace(cid))
                    {
                        ProjectCategory pcategory = dalpc.GetProjectCategoryById(int.Parse(cid));
                        if (pcategory == null)
                        {
                            isError = true;
                        }
                        else {
                            this.CatagoryName.Text = pcategory.name;
                            this.CategoryDesc.Text = pcategory.memo;
                            this.hf_CategoryId.Value = pcategory.id.ToString();
                            List<ProjectCategory> parentCategoryList = dalpc.GetProjectCategoryPathById(int.Parse(cid));

                            string parentPath = "";
                            bool hasPrefix = false;
                            foreach(ProjectCategory category in parentCategoryList)
                            {

                                parentPath += ((hasPrefix ? "->" : "") + category.name);
                                hasPrefix = true;
                            }
                            this.literal_CategoryPath.Text = "类目路径：" + parentPath;
                            this.literal_CategoryPath.Visible = true;
                            this.RootCategoryList.Visible = false;
                            this.btn_AddProjectCategory.Text = "修改";
                        }
                    }
                    else {
                        isError = true;
                    }

                    if (isError) {

                        Javascript.Alert("您所操作的数据不存在或已被删除!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                }
                else {
                    List<ProjectCategory> categoryList = dalpc.GetProjectCategoryByParendId(0);

                    if (categoryList != null && categoryList.Count != 0)
                    {
                        BindCategoryList(this.RootCategoryList.Items, categoryList, 0);
                    }
                }


               
            }
        }

        protected void RootCategoryList_Changed(Object sender, EventArgs e)
        {

            if (this.RootCategoryList.SelectedValue == "0")
            {
                CategoryListDisableVisible(this.SubCategoryList);
            }
            else
            {

                List<ProjectCategory> categoryList = dalpc.GetProjectCategoryByParendId(int.Parse(this.RootCategoryList.SelectedValue.Trim()));

                this.SubCategoryList.Items.Clear();
                this.SubCategoryList.Visible = true;
                this.SubCategoryList.Items.Add(new ListItem("新建二级类目", "0"));
                BindCategoryList(this.SubCategoryList.Items, categoryList, 0);
            }
        }

        private void CategoryListDisableVisible(DropDownList dropDownList) {

            dropDownList.Items.Clear();
            dropDownList.Visible = false;
        }

        private void BindCategoryList(ListItemCollection itemCollection, List<ProjectCategory> categoryList, int selectedIndex) {

            ListItem item;
            foreach(ProjectCategory category in categoryList){
                
                item = new ListItem(category.name, category.id.ToString());
                if (selectedIndex == category.id) {
                    item.Selected = true;
                }
                itemCollection.Add(item);
            }
        }

        protected void btn_AddProjectCategory_Click(object sender, EventArgs e)
        {
            string categoryName = this.CatagoryName.Text.Trim();
            string categoryDesc = this.CategoryDesc.Text.Trim();

            if ("修改" == this.btn_AddProjectCategory.Text.Trim())
            {
                int cid = int.Parse(this.hf_CategoryId.Value.Trim());
                ProjectCategory projectCategory = dalpc.GetProjectCategoryById(cid);
                projectCategory.name = categoryName;
                projectCategory.memo = categoryDesc;

                dalpc.UpdateProjectCategory(projectCategory);
            }
            else
            {
                string parendId;
                int categoryLevel;
                if (this.RootCategoryList.SelectedValue == "0")
                {
                    parendId = "0";
                    categoryLevel = 1;
                }
                else {
                    if (this.SubCategoryList.SelectedValue == "0")
                    {

                        parendId = this.RootCategoryList.SelectedValue;
                        categoryLevel = 2;
                    }
                    else {
                        parendId = this.SubCategoryList.SelectedValue;
                        categoryLevel = 3;
                    }
                }
           
                ProjectCategory projectCategory = new ProjectCategory();
                projectCategory.name = categoryName;
                projectCategory.memo = categoryDesc;
                projectCategory.parentId = int.Parse(parendId);
                projectCategory.categoryLevel = categoryLevel;

                dalpc.addProjectCategory(projectCategory);
            }

            Javascript.Alert("操作成功", Page);
            Javascript.RefreshParentWindowReload(Page);
        }
    }
}