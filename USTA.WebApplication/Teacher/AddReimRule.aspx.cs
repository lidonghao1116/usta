using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.PageBase;
using USTA.Common;
using USTA.Dal;
using USTA.Model;

namespace USTA.WebApplication.Teacher
{
    public partial class AddReimRule : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string projectId = Request["projectId"];
                if (string.IsNullOrWhiteSpace(projectId))
                {
                    Javascript.Alert("请指定您要添加报销规则的项目!", Page);
                    Javascript.RefreshParentWindowReload(Page);
                }
                else {

                    DalOperationAboutProject dalProject = new DalOperationAboutProject();
                    DalOperationAboutReim dalReim = new DalOperationAboutReim();


                    Project project = dalProject.GetProject(int.Parse(projectId.Trim()));

                    List<Reim> reimList = dalReim.GetAllReims();

                    if (project == null || reimList.Count == 0)
                    {
                        Javascript.Alert("您要添加报销规则的项目不存在或者您未添加任何报销项，请核对后再次操作!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                    else {

                        this.literal_ProjectName.Text = project.name;
                        this.hf_ProjectId.Value = project.id.ToString();
                        foreach (Reim reim in reimList) 
                        {
                            this.ddlReimLists.Items.Add(new ListItem(reim.name, reim.id.ToString()));
                        }
                    }
                }
            }
        }

        protected void AddReimRule_Click(object sender, EventArgs e) 
        {
            int projectId = int.Parse(this.hf_ProjectId.Value.Trim());
            int reimId = int.Parse(this.ddlReimLists.SelectedValue.Trim());
            float reimValue = CommonUtility.ConvertFormatedFloat("{0:0.00}", this.tb_reimValue.Text);

            float maxReimValue = CommonUtility.ConvertFormatedFloat("{0:0.00}", this.tb_maxReimValue.Text);

            ProjectReimRule reimRule = new ProjectReimRule()
            {
                project = new Project() { id = projectId },
                reim = new Reim() { id = reimId },
                reimValue = reimValue,
                maxReimValue = maxReimValue
            };

            DalOperationAboutProjectReimRule dalRule = new DalOperationAboutProjectReimRule();
            dalRule.AddProjectReimRule(reimRule);
            Javascript.Alert("操作成功", Page);

            Javascript.RefreshParentWindowReload(Page);
        }
    }
}