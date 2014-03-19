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
using System.Data;

namespace USTA.WebApplication.Administrator
{
    public partial class EditSecondNotifyType : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
                AdminNotifyType type = dalNotifyType.FindAdminNotifyTypeById(int.Parse(Request["notifyTypeId"].ToString().Trim()));
                txtTypeName.Text = type.notifyTypeName.ToString().Trim();
                txtSequence.Text = type.sequence.ToString().Trim();

                txtTypeName.Attributes.Add("class", "required");
                txtSequence.Attributes.Add("class", "required number");

                DataTable dt = dalNotifyType.FindAllParentAdminNotifyType().Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlNotifyType.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
                }

                int parentId = dalNotifyType.GetParentIdById(int.Parse(Request["notifyTypeId"].ToString().Trim()));

                for (int i = 0; i < ddlNotifyType.Items.Count; i++)
                {
                    if (ddlNotifyType.Items[i].Value == parentId.ToString())
                    {
                        ddlNotifyType.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //若为浮点数则返回去
            if (txtSequence.Text.Trim().IndexOf(".") != -1)
            {
                Javascript.GoHistory(-1, "显示顺序，请输入整数！", Page);
                return;
            }

            DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();


            AdminNotifyType type = new AdminNotifyType();
            type.notifyTypeName = txtTypeName.Text.Trim();
            type.sequence = int.Parse(txtSequence.Text.Trim());
            type.parentId = int.Parse(ddlNotifyType.SelectedValue.Trim());
            dalNotifyType.AddAdminNotifyType(type);
            Javascript.RefreshParentWindow("修改成功", "/Administrator/NotifyInfoManage.aspx?fragment=3", Page);

        }
    }
}