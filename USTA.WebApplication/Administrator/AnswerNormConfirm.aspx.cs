using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Dal;
using USTA.Common;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class AnswerNormConfirm : CheckUserWithCommonPageBase
    {
        DalOperationNorm dalnorm = new DalOperationNorm();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = int.Parse(Request["id"].Trim());
                NormConfirm nc = dalnorm.GetNormConfirm(id);
                lblquestion.Text = nc.question;
            }
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            int id = int.Parse(Request["id"].Trim());
            NormConfirm nc = dalnorm.GetNormConfirm(id);
            string answer = this.txtAnswer.Text.Trim();
            dalnorm.SetAnswer(id, answer);
            Javascript.RefreshParentWindow("/Administrator/NormManager.aspx?fragment=3&teacherNo="+nc.teacherNo, Page);
        }
    }
}