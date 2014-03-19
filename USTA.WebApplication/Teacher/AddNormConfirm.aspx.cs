using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Model;
using USTA.Bll;
using USTA.Common;
using USTA.PageBase;

namespace USTA.WebApplication.Teacher
{
    public partial class AddNormConfirm : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            DalOperationNorm daln = new DalOperationNorm();
            NormConfirm ncom = new NormConfirm
            {
                teacherNo = Request["teacherNo"].Trim(),
                term = Request["term"].Trim(),
                question = txtproblem.Text,
                type = 1,
                value = Request["value"].Trim(),
                answer = ""
            };
            daln.AddNormConfirm(ncom);
            Javascript.RefreshParentWindow("/Teacher/NormView.aspx", Page);
        }
        
    }
}