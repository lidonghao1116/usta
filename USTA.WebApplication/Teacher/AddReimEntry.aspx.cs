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

namespace USTA.WebApplication.Teacher
{
    public partial class AddReimEntry : CheckUserWithCommonPageBase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string projectId = Request["projectId"];
                
                if (projectId == null || projectId.Trim().Length == 0)
                {
                    Javascript.Alert("您未指定项目不能进行此操作!", Page);
                    Javascript.RefreshParentWindowReload(Page);
                }
                else 
                {
                    DalOperationAboutProject dalProject = new DalOperationAboutProject();
                    Project project = dalProject.GetProject(int.Parse(projectId));
                    if (project == null)
                    {
                        Javascript.Alert("未找到指定项目!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                        
                    }
                    else 
                    {
                        this.literal_ProjectName.Text = project.name;
                        this.hf_ProjectId.Value = project.id.ToString();
                        BindDataItemReim();   
                    }
                }
            }
        }

        protected void ReimListChanged(object sender, EventArgs e) 
        {
            int reimId = int.Parse(this.ddl_ReimList.SelectedValue.Trim());
            if (reimId == 0)
            {
                this.reimRuleDiv.Visible = false;

            }
            else {
                int projectId = int.Parse(this.hf_ProjectId.Value.Trim());
                DalOperationAboutProjectReimRule dalRule = new DalOperationAboutProjectReimRule();
                ProjectReimRule rule = dalRule.GetProjectReimRule(projectId, reimId);
                if (rule != null)
                {
                    this.reimRuleDiv.Visible = true;
                    this.literal_ReimValue.Text = rule.reimValue.ToString();
                    this.literal_MaxReimValue.Text = rule.maxReimValue.ToString();
                }
                else {
                    this.reimRuleDiv.Visible = false;
                }
            }
        
        }

        private void BindDataItemReim() 
        {
            DalOperationAboutReim dalReim = new DalOperationAboutReim();
            List<Reim> reimList = dalReim.GetAllReims();
            if (reimList == null || reimList.Count == 0)
            {
                Javascript.Alert("您尚未添加报销项，不能进行此操作!", Page);
                Javascript.RefreshParentWindowReload(Page);
            }
            else 
            {
                ListItemCollection itemCollection = this.ddl_ReimList.Items;
                foreach (Reim reim in reimList) 
                {
                    itemCollection.Add(new ListItem(reim.name, reim.id.ToString()));
                }
            }
        
        }

        protected void ReimEntry_Click(object sender, EventArgs e) 
        {
            string reimIdString = this.ddl_ReimList.SelectedValue;
            if (reimIdString == null || reimIdString == "0")
            {
                Javascript.Alert("您未指定报销项，请核对后再完成此操作!", Page);
            }
            else 
            {
                int projectId = int.Parse(this.hf_ProjectId.Value);
                int reimId = int.Parse(reimIdString);
                float reimingValue = CommonUtility.ConvertFormatedFloat("{0:F2}", this.tb_ReimValue.Text.Trim());
                DalOperationAboutReimItem dalReimItem = new DalOperationAboutReimItem();
                DalOperationAboutProjectReimRule dalRule = new DalOperationAboutProjectReimRule();

                float reimedValue = dalReimItem.GetReimItemValue(projectId, reimId);

                ProjectReimRule projectRule = dalRule.GetProjectReimRule(projectId, reimId);

                bool isError = false;
                if (projectRule != null)
                {
                    if (reimingValue > projectRule.reimValue)
                    {
                        isError = true;
                        Javascript.Alert("该项目选择的报销项单次最大金额不超过"　+ projectRule.reimValue+ "元", Page);
                    }
                    else if (reimingValue + reimedValue > projectRule.maxReimValue)
                    {
                        isError = true;
                        Javascript.Alert("该项目选择的报销项最大报销总金额不超过" + projectRule.maxReimValue + "元，当前您已报销了" + reimedValue + "元", Page);
                    }
                }

                if (!isError) {
                    ReimItem reimItem = new ReimItem()
                    {
                        project = new Project() { id = projectId },
                        reim = new Reim() { id = reimId },
                        value = reimingValue,
                        memo = this.ReimEntryMemo.Text.Trim()
                    };

                    dalReimItem.AddReimItem(reimItem);

                    Javascript.Alert("操作成功!", Page);
                    Javascript.RefreshParentWindowReload(Page);
                }
            }
        
        }
    }
}