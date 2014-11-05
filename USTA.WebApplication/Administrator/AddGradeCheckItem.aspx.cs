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

                //检测是否有重名规则
                string _name = txtGradeCheckItemName.Text.Trim();
                if (doan.CheckIsExistGradeCheckItemName(_name).Tables[0].Rows.Count > 0)
                {
                    Javascript.GoHistory(-1, "已经存在此名称的规则，请更换规则名或者在已有规则上修改所应用的学年即可！", Page);
                    return;
                }

                DataTable dt = doan.GetTermYear().Tables[0];

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    ddlTermYear.Items.Add(new ListItem("20" + dt.Rows[i]["termYear"].ToString().Trim() + "学年", dt.Rows[i]["termYear"].ToString().Trim()));
                //}
                if (dt.Rows[0]["termYear"].ToString().Trim().Length > 0)
                {

                    dt.Columns.Add("termYearFormat", typeof(string), "'20'+termYear+'学年'");
                    ddlTermYears.DataSource = dt;
                    ddlTermYears.DataTextField = "termYearFormat";
                    ddlTermYears.DataValueField = "termYear";
                    ddlTermYears.DataBind();
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

                List<string> items = new List<string>();
                for (int i = 0; i < ddlTermYears.Items.Count; i++)
                {
                    ListItem _item = ddlTermYears.Items[i];
                    if (_item.Selected)
                    {
                        items.Add(_item.Value);
                    }
                }

                model.termYears = string.Join(",", items).Trim();

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