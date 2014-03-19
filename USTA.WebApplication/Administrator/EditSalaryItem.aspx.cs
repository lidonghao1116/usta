using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class EditSalaryItem : CheckUserWithCommonPageBase
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                string pageIndex = Request["page"];
                
                if (pageIndex == null || pageIndex.Trim().Length == 0)
                {
                    pageIndex = "1";
                }
                else {
                    pageIndex = Request["page"].Trim();
                }

                string operation = Request["op"];
                if ("edit".Equals(operation))
                {
                    string itemID = Request["salaryItemId"];
                    
                    if (itemID == null || itemID.Trim().Length == 0)
                    {
                        Javascript.Alert("请提供正确的薪酬项Id", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                    else
                    {
                        DalOperationAboutSalaryItem dal = new DalOperationAboutSalaryItem();
                        SalaryItem item = dal.GetSalaryItemById(int.Parse(Request["salaryItemId"].ToString().Trim()));
                        salaryItemName.Text = item.salaryItemName;
                        salaryItemUnit.Text = item.salaryItemUnit;
                        salaryItemDesc.Text = item.salaryItemDesc;
                        cb_hasTax.Checked = item.hasTax;
                        cb_defaultChecked.Checked = item.isDefaultChecked;


                        SalaryVisible_TR.Visible = false;
                        UserFor_TR.Visible = false;
                        this.btnSubmit.Text = "修改";
                    }
                }
                else 
                {
                    this.btnSubmit.Text = "添加";
                    cb_hasTax.Checked = true;
                    this.UserFor_TR.Visible = true;
                }
                this.hf_page.Value = pageIndex;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
           DalOperationAboutSalaryItem dal = new DalOperationAboutSalaryItem();
           string pageIndex = this.hf_page.Value;
            if (this.btnSubmit.Text == "添加") 
            {
                SalaryItem salaryItem = new SalaryItem();
                salaryItem.salaryItemName = salaryItemName.Text.Trim();
                salaryItem.salaryItemUnit = salaryItemUnit.Text.Trim();
                salaryItem.salaryItemDesc = salaryItemDesc.Text.Trim();
                if (SalaryItemIsHidden.Checked)
                {
                    salaryItem.salaryItemStatus = 2;        //默认隐藏
                }
                else {
                    salaryItem.salaryItemStatus = 1;        //默认展示
                }

                if (SalaryItemForOutTeacher.Checked)
                {
                    salaryItem.useFor = 2;                  //适用于院外教师
                }
                else if (SalaryItemForOutAssistant.Checked)
                {
                    salaryItem.useFor = 3;                  //适用于院外助教
                }
                else {
                    salaryItem.useFor = 1;                  //适用于院内教师/助教
                }

                salaryItem.hasTax = cb_hasTax.Checked;
                salaryItem.isDefaultChecked = cb_defaultChecked.Checked;    
                
                dal.AddSalaryItem(salaryItem);

                Javascript.RefreshParentWindow("/Administrator/SalaryManage.aspx?fragment=2&page=" + pageIndex,  Page);
            }
            else if (this.btnSubmit.Text == "修改") 
            {
                if (Request["salaryItemId"] != null) 
                {
                    string salaryItemId = Request["salaryItemId"].ToString().Trim();

                    SalaryItem item = dal.GetSalaryItemById(int.Parse(salaryItemId));
                    item.salaryItemName = salaryItemName.Text;
                    item.salaryItemUnit = salaryItemUnit.Text;
                    item.salaryItemDesc = salaryItemDesc.Text;
                    item.hasTax = cb_hasTax.Checked;
                    item.isDefaultChecked = cb_defaultChecked.Checked;
                    
                    dal.updateSalaryItem(item);

                    Javascript.RefreshParentWindow("/Administrator/SalaryManage.aspx?fragment=2&page=" + pageIndex, Page);
                }
            
            }
        }
    }
}