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
    public partial class ViewReimDetail : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string reimId = Request["reimId"];
                if (string.IsNullOrWhiteSpace("reimId"))
                {
                    Javascript.Alert("您未指定要查看的报销项!", Page);
                    Javascript.RefreshParentWindowReload(Page);
                }
                else
                {
                    DalOperationAboutReim dalReim = new DalOperationAboutReim();
                    Reim reim = dalReim.GetReim(int.Parse(reimId.Trim()));
                    if (reim == null)
                    {
                        Javascript.Alert("您指定的报销项不存在!", Page);
                        Javascript.RefreshParentWindowReload(Page);
                    }
                    else
                    {
                        this.ViewReimName.Text = reim.name;
                        this.ViewReimDesc.Text = reim.comment;
                        this.ViewReimCreatedTime.Text = reim.createdTime.ToString();
                    }
                }

            }
        }
    }
}