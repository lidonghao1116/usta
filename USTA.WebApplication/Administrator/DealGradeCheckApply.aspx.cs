using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Transactions;
using USTA.Model;
using USTA.Bll;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class DealGradeCheckApply : CheckUserWithCommonPageBase
    {
        public int gradeCheckApplyId = HttpContext.Current.Request["gradeCheckApplyId"] == null ? 0 : int.Parse(HttpContext.Current.Request["gradeCheckApplyId"].Trim());

        public string studentNo = HttpContext.Current.Request["studentNo"] == null ? string.Empty : HttpContext.Current.Request["studentNo"].Trim();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
                DataTable dt = dal.GetStudentGradeCheckApplyById(gradeCheckApplyId).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ltlTermTag.Text = CommonUtility.ChangeTermToString(dt.Rows[i]["termTag"].ToString().Trim());
                    ltlCourseName.Text = dt.Rows[i]["courseName"].ToString().Trim() + "(" + dt.Rows[i]["ClassID"].ToString().Trim() + ")";

                    if (dt.Rows[i]["gradeCheckApplyType"].ToString().Trim() == "重修")
                    {
                        ddlGradeCheckApplyType.Items.Add(new ListItem("重修", "重修"));
                        ddlGradeCheckApplyType.Items.Add(new ListItem("重考", "重考"));
                    }
                    else
                    {
                        ddlGradeCheckApplyType.Items.Add(new ListItem("重考", "重考"));
                        ddlGradeCheckApplyType.Items.Add(new ListItem("重修", "重修"));
                    }
                    txtapplyChecKSuggestion.Text = dt.Rows[i]["applyChecKSuggestion"].ToString().Trim();
                }
            }
        }

        protected void btnAccord_Click(object sender, EventArgs e)
        {
            try
            {
                DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

                dal.UpdateStudentGradeCheckApplyState("符合", txtapplyChecKSuggestion.Text.Trim(), gradeCheckApplyId);
                Javascript.RefreshParentWindow("修改重修重考申请状态成功！", "/Administrator/StudentManager.aspx?fragment=2", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "修改重修重考申请状态失败,请检查格式是否有误！", Page);
            }
        }

        protected void btnNotAccord_Click(object sender, EventArgs e)
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            try
            {
                dal.UpdateStudentGradeCheckApplyState("不符合", txtapplyChecKSuggestion.Text.Trim(), gradeCheckApplyId);
                Javascript.RefreshParentWindow("修改重修重考申请状态成功！", "/Administrator/StudentManager.aspx?fragment=2", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "修改重修重考申请状态失败,请检查格式是否有误！", Page);
            }
        }
    }
}