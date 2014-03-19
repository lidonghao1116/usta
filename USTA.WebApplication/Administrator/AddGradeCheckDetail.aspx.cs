using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Transactions;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class AddGradeCheckDetail : CheckUserWithCommonPageBase
    {
        public string studentNo = (HttpContext.Current.Request.QueryString["studentNo"] == null ? string.Empty : HttpContext.Current.Request.QueryString["studentNo"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            DalOperationAboutStudent stu = new DalOperationAboutStudent();

            StudentsList model = stu.GetStudentById(studentNo);

            DataSet ds = dal.GetGradeCheckItemsByTermYear(model.SchoolClassName.Trim().Substring(0, 2));

            if (ds.Tables[0].Rows.Count == 0)
            {
                Javascript.ExcuteJavascriptCode("alert('未找到20" + model.SchoolClassName.Trim().Substring(0, 2) + "学年成绩审核规则，请先添加:)');parent.tb_remove();", Page);
                return;
            }

            List<string> listColumn = new List<string>();

            //for (int i=0;i<ds.Tables[0].Rows.Count;i++)
            //{
            //    listColumn.Add(ds.Tables[0].Columns[i].ToString());
            //}
            Table tb = new Table();
            tb.CssClass = "tableAddStyleNone";
            tb.Width = Unit.Percentage(100);


            TableRow tr1 = new TableRow();
            TableCell td1 = new TableCell();
            td1.HorizontalAlign = HorizontalAlign.Left;
            td1.CssClass = "border";
            td1.Width = Unit.Percentage(25);
            td1.Text = "当前学生所属班级：";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.CssClass = "border";
            td1.Text = model.SchoolClassName + "&nbsp;&nbsp;所应用的成绩审核规则为：20" + model.SchoolClassName.Trim().Substring(0, 2) + "学年成绩审核规则";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Left;
                td.CssClass = "border";
                td.Width = Unit.Percentage(25);

                LiteralControl ltl = new LiteralControl();
                ltl.Text = "<br />" + ds.Tables[0].Rows[i]["gradeCheckItemName"].ToString().Trim();

                td.Controls.Add(ltl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Left;
                td.CssClass = "border";

                TextBox txt = new TextBox();
                txt.ID = "txt_" + ds.Tables[0].Rows[i]["gradeCheckId"].ToString().Trim();
                txt.Text = ds.Tables[0].Rows[i]["gradeCheckItemDefaultValue"].ToString().Trim();
                td.Controls.Add(txt);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
            PlaceHolder1.Controls.Add(tb);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
            string dtime = DateTime.Now.ToString();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (Control ctlTable in PlaceHolder1.Controls)
                    {
                        foreach (Control ctlTableRow in ctlTable.Controls)
                        {
                            foreach (Control ctlTableCell in ctlTableRow.Controls)
                            {
                                foreach (Control ctl in ctlTableCell.Controls)
                                {
                                    if (ctl.GetType().ToString().Trim() == "System.Web.UI.WebControls.TextBox")
                                    {
                                        StudentsGradeCheckDetail model = new StudentsGradeCheckDetail();
                                        model.studentNo = studentNo;
                                        model.updateTime = Convert.ToDateTime(dtime);
                                        model.gradeCheckDetailValue = ((TextBox)ctl).Text.Trim();
                                        model.gradeCheckId = int.Parse(ctl.ID.Split("_".ToCharArray())[1]);
                                        dal.AddGradeCheckDetailByStudentNo(model);
                                    }
                                }
                            }
                        }
                    }

                    StudentsGradeCheckConfirm model1 = new StudentsGradeCheckConfirm { studentNo = studentNo, updateTime = Convert.ToDateTime(dtime), isAccord = int.Parse(ddlIsAccord.SelectedValue), remark = remark.Text.Trim() };
                    dal.AddStudentGradeCheckConfirm(model1);
                    scope.Complete();
                    Javascript.RefreshParentWindow("添加成绩审核记录成功！", "/Administrator/StudentManager.aspx?fragment=7&studentNo=" + studentNo, Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "添加成绩审核记录失败！", Page);
                }
                finally
                {
                    dal.conn.Close();
                }
            }
        }
    }
}