using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class AddGradeCheckItem : CheckUserWithCommonPageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DalOperationAboutGradeCheck doan = new DalOperationAboutGradeCheck();
                DataTable dt = doan.GetTermYear().Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTermYear.Items.Add(new ListItem("20" + dt.Rows[i]["termYear"].ToString().Trim() + "学年", dt.Rows[i]["termYear"].ToString().Trim()));
                }
            }
        }

        //修改
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtGradeCheckItemName.Text.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "请输入成绩审核单项名称！", Page);
            }
            else
            {
                DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
                StudentsGradeCheck model = new StudentsGradeCheck();
                model.gradeCheckItemName = txtGradeCheckItemName.Text.Trim();
                model.gradeCheckItemDefaultValue = txtGradeCheckItemDefaultValue.Text.Trim();
                model.displayOrder = int.Parse(txtDisplayOrder.Text.Trim());
                model.termYear = ddlTermYear.SelectedValue;

                try
                {

                    dalOperationAboutGradeCheck.AddGradeCheckItems(model);//修改
                    Javascript.RefreshParentWindow("添加成绩审核单项成功！", "/Administrator/StudentManager.aspx?fragment=5", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "添加成绩审核单项失败,请检查格式是否有误！", Page);
                }

            }
        }
    }
}