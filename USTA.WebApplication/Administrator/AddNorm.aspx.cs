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
    public partial class AddNorm : CheckUserWithCommonPageBase
    {
        public String parentName;
        protected void Page_Load(object sender, EventArgs e)
        {
            
                DalOperationNorm dalOperationNorm = new DalOperationNorm();
                if (Request["parentId"] != null && Request["parentId"]!="")
                {
                    int parentId = Convert.ToInt32(Request["parentId"]);
                    Norm norm = dalOperationNorm.getNormById(parentId);
                    if (norm != null)
                    {
                        this.parentId.Value = Convert.ToString(norm.normId);
                        parentName = norm.name;
                    }

                }
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        { 
            DalOperationNorm dalOperationNorm = new DalOperationNorm();
       
            Norm norm = new Norm();
            norm.name = TextNormName.Text.Trim();
            norm.comment = TextComment.Text.Trim();
            norm.type = int.Parse(ddltNormType.SelectedValue);
            //if (parentId.Value != null && parentId.Value != "")
            //{
            //    norm.parentId = Convert.ToInt32(parentId.Value);
            //}
            //else
            //{
            //    norm.parentId = 0;
            //}
            if (Request["parentId"] == null || Request["parentId"]=="")
            {
                norm.parentId = 0;
            }
            else
            {
                norm.parentId = Convert.ToInt32(Request["parentId"]);
            }
            
            if (Request["year"] != null && !Request["year"].Trim().Equals(""))
            {
                norm.year = Request["year"].Trim();
                if (norm.parentId == -1)
                {
                    if ("课程名称".Equals(norm.name) || "学期".Equals(norm.name) || "类型".Equals(norm.name) || "理论课时".Equals(norm.name) || "实验课时".Equals(norm.name))
                    {
                        Javascript.Alert("此指标名称在硕士教学中已经存在，不能重复添加！", Page);

                        return;
                    }
                }
                if (dalOperationNorm.ExistNormName(TextNormName.Text.Trim(),norm.parentId , norm.year))
                {
                    Javascript.Alert("存在此指标名称，不能重复输入", Page);
                    
                    return;
                }
                dalOperationNorm.AddNorm(norm);
                Javascript.RefreshParentWindow("/Administrator/NormManager.aspx?fragment=2&term=" + Request["year"], Page);
            }
            else
            {
                Javascript.RefreshParentWindow("/Administrator/NormManager.aspx?fragment=2", Page);
            }
        }
    }
}