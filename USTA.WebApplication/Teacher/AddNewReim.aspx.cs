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
    public partial class AddNewReim : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) {
                string operation = Request["op"];
                if (!string.IsNullOrWhiteSpace(operation) && "edit" == operation) {
                    string reimId = Request["reimId"];
                    bool isError = false;
                    if (!string.IsNullOrWhiteSpace(reimId))
                    {
                        DalOperationAboutReim dalReim = new DalOperationAboutReim();
                        Reim reim = dalReim.GetReim(int.Parse(reimId));
                        if (reim == null)
                        {
                            isError = true;
                        }
                        else
                        {
                            this.ReimName.Text = reim.name;
                            this.ReimDesc.Text = reim.comment;
                            this.btn_NewReim.Text = "修改";
                            this.hf_ReimId.Value = reim.id.ToString();
                        }
                    }
                    else {
                        isError = true;
                    }

                    if (isError) {
                        Javascript.Alert("您所执行操作的数据不存在或已被删除!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                }
            
            }

        }

        protected void NewReim_Click(object sender, EventArgs e) 
        {
            string reimName = this.ReimName.Text.Trim();
            string reimDesc = this.ReimDesc.Text.Trim();
            DalOperationAboutReim dalReim = new DalOperationAboutReim();
            if (this.btn_NewReim.Text == "修改")
            {
                string reimId = this.hf_ReimId.Value;
                Reim reim = dalReim.GetReim(int.Parse(reimId));
                if (reim != null)
                {
                    reim.name = reimName;
                    reim.comment = reimDesc;
                    dalReim.UpdateReim(reim);
                }
                else {
                    Javascript.Alert("您所操作的报销项不存在或已被删除!", Page);
                }

            }
            else {
                Reim reim = new Reim()
                {
                    name = reimName,
                    comment = reimDesc
                };
                dalReim.AddReim(reim);
            }
            

            Javascript.Alert("操作成功!", Page);
            Javascript.RefreshParentWindowReload(Page);
        }
    }
}