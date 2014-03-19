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

namespace USTA.WebApplication.Administrator
{
    public partial class AddFirstNotifyType : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        //****第3个标签：管理文章类型－－－－－－－开始－－－－－－－－－－－－

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
            type.parentId = 0;
            dalNotifyType.AddAdminNotifyType(type);
            Javascript.RefreshParentWindow("添加成功", "/Administrator/NotifyInfoManage.aspx?fragment=3", Page);

        }
    }
}