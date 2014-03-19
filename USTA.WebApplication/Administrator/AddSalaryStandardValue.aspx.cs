using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class AddSalaryStandardValue : CheckUserWithCommonPageBase
    {
        DalOperationAboutSalaryStandardValue dalssv = new DalOperationAboutSalaryStandardValue();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                string salaryItemId = Request["salaryItemId"];
                if (salaryItemId == null || salaryItemId.Trim().Length == 0)
                {
                    Javascript.Alert("请指定一个薪酬项再进行此项操作", Page);
                    Javascript.RefreshParentWindowReload(Page);
                }
                else {
                    this.hf_SalaryItemId.Value = salaryItemId;
                    BindStandardValueList(int.Parse(salaryItemId));
                }
            }
        }

        private void BindStandardValueList(int salaryItemId) {

            List<SalaryStandardValue> valueList = dalssv.GetStandardValueBySalaryItemId(salaryItemId);
            this.StandardValueList.DataSource = valueList;
            this.StandardValueList.DataBind();
            if (valueList != null && valueList.Count > 0)
            {
                this.StandardValueList.ShowHeader = true;
                this.StandardValueList.ShowFooter = false;
            }
            else {
                this.StandardValueList.ShowHeader = false;
                this.StandardValueList.ShowFooter = true;
            }
        }

        protected void AddSalaryStandardValue_Click(object source, EventArgs e) 
        {

            SalaryStandardValue standardValue = new SalaryStandardValue();
            standardValue.SalaryItemId = int.Parse(this.hf_SalaryItemId.Value.Trim());
            standardValue.SalaryItemValue = CommonUtility.ConvertFormatedFloat("{0:F2}", this.SalaryStandardValue.Text.Trim());
            
            dalssv.AddStandardValue(standardValue);

            Javascript.Alert("添加标准金额成功!", Page);
            BindStandardValueList(int.Parse(this.hf_SalaryItemId.Value.Trim()));
        }

        protected void StandardValueList_Command(object source, DataListCommandEventArgs e) 
        {   
            int standardValue = int.Parse(StandardValueList.DataKeys[e.Item.ItemIndex].ToString());
            if ("delValue".Equals(e.CommandName))
            {
                dalssv.DelStandardValue(standardValue);
                Javascript.Alert("删除标准金额成功!", Page);
                BindStandardValueList(int.Parse(this.hf_SalaryItemId.Value.Trim()));
            }
        }
    }
}