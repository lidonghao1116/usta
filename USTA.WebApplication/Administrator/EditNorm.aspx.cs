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
    public partial class EditNorm : CheckUserWithCommonPageBase
    {
        public String parentName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DalOperationNorm dalOperationNorm = new DalOperationNorm();
                if (Request["normId"] != null)
                {
                    int normId = Convert.ToInt32(Request["normId"]);
                    Norm norm = dalOperationNorm.getNormById(normId);
                    if (norm != null)
                    {
                        this.hidNormId.Value = Convert.ToString(norm.normId);
                        this.hidParentId.Value = Convert.ToString(norm.parentId);
                        this.ddltNormType.SelectedValue = Convert.ToString(norm.type);
                        Norm parent = null;
                        if ((parent = dalOperationNorm.getNormById(norm.parentId)) != null)
                        {
                            this.parentName = parent.name;
                        }
                        this.TextNormName.Text = norm.name;
                        this.TextComment.Text = norm.comment;
                    }

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Norm norm = new Norm();
            norm.normId = Convert.ToInt32(hidNormId.Value);
            norm.name = TextNormName.Text;
            norm.comment = TextComment.Text;
            norm.parentId = Convert.ToInt32(hidParentId.Value);
            norm.type = Convert.ToInt32(ddltNormType.SelectedValue);
            DalOperationNorm dalOperationNorm = new DalOperationNorm();
            dalOperationNorm.EditNorm(norm);
            Javascript.RefreshParentWindow("/Administrator/NormManager.aspx?fragment=2&term=" + Request["year"], Page);
        }

    }
}