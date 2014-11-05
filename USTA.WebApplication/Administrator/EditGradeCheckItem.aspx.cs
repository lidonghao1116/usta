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
    public partial class EditGradeCheckItem : CheckUserWithCommonPageBase
    {
        int gradeCheckId = (HttpContext.Current.Request["gradeCheckId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["gradeCheckId"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tryParseInt = 0;
                if (CommonUtility.SafeCheckByParams<string>(Request["gradeCheckId"], ref tryParseInt))
                {
                    //获取Url中的参数
                    gradeCheckId = tryParseInt;
                }

                InitialNotifyEdit(gradeCheckId);
            }
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int englishExamNotifyInfoId)
        {
            DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
            DataTable dt = dalOperationAboutGradeCheck.GetGradeCheckItemById(gradeCheckId).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txtGradeCheckItemName.Text = dt.Rows[i]["gradeCheckItemName"].ToString().Trim();

                txtGradeCheckItemDefaultValue.Text = dt.Rows[i]["gradeCheckItemDefaultValue"].ToString().Trim();
                txtDisplayOrder.Text = dt.Rows[i]["displayOrder"].ToString().Trim();
            }

            DataTable dt1 = dalOperationAboutGradeCheck.GetTermYear().Tables[0];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    ddlTermYear.Items.Add(new ListItem("20" + dt.Rows[i]["termYear"].ToString().Trim() + "学年", dt.Rows[i]["termYear"].ToString().Trim()));
            //}

            string termYears = dt.Rows[0]["termYears"].ToString().Trim();


            dt1.Columns.Add("termYearFormat", typeof(string), "'20'+termYear+'学年'");
            ddlTermYears.DataSource = dt1;
            ddlTermYears.DataTextField = "termYearFormat";
            ddlTermYears.DataValueField = "termYear";
            ddlTermYears.DataBind();

            string[] items = termYears.Split(",".ToCharArray());
            for (int i = 0; i < ddlTermYears.Items.Count; i++)
            {
                ListItem _item = ddlTermYears.Items[i];
                for (int j = 0; j < items.Length; j++)
                {
                    if (_item.Value == items[j].Trim())
                    {
                        _item.Selected = true;
                        break;
                    }
                }
            }

        }
        //修改
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
            string _name = txtGradeCheckItemName.Text.Trim();
            if (dalOperationAboutGradeCheck.CheckIsExistGradeCheckItemName(_name, gradeCheckId).Tables[0].Rows.Count > 0)
            {
                Javascript.GoHistory(-1, "已经存在此名称的规则，请更换规则名或者在已有规则上修改所应用的学年即可！", Page);
                return;
            }

            if (txtGradeCheckItemName.Text.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "请输入成绩审核单项名称！", Page);
            }
            else
            {
                StudentsGradeCheck model = new StudentsGradeCheck();
                model.gradeCheckItemName = txtGradeCheckItemName.Text.Trim();
                model.gradeCheckItemDefaultValue = txtGradeCheckItemDefaultValue.Text.Trim();
                model.gradeCheckId = gradeCheckId;
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
                    dalOperationAboutGradeCheck.UpdateGradeCheckItemById(model);//修改
                    Javascript.RefreshParentWindow("修改成绩审核单项成功！", "/Administrator/StudentManager.aspx?fragment=5", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "修改失败,请检查格式是否有误！", Page);
                }

            }
        }
    }
}