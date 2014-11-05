using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using System.Collections;

namespace USTA.WebApplication.Administrator
{
    public partial class ExamSeatArrange : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, divFragment1, divFragment2, divFragment3);

            if (fragmentFlag.Equals("1"))
            {
                if (!IsPostBack)
                {
                    DataBindCourses();
                }
            }
        }

        //绑定课程下拉列表
        public void DataBindCourses()
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.FindCurrentCoursesByLocale(ddlGradeCheckLocale.SelectedValue).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListItem li = new ListItem(dt.Rows[i]["courseName"].ToString().Trim() + "(" + dt.Rows[i]["classID"].ToString().Trim() + ")", dt.Rows[i]["termTag"].ToString().Trim() + dt.Rows[i]["courseNo"].ToString().Trim() + dt.Rows[i]["classID"].ToString().Trim());
                ddlCourses.Items.Add(li);
            }
        }

        protected void ddlGradeCheckLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (ddlCourses.Items.Count > 0)
            {
                ddlCourses.Items.RemoveAt(0);
            }
            DataBindCourses();
        }

        protected void btnExamSeatArrange_Click(object sender, EventArgs e)
        {
            List<string> listStudent = new List<string>();

            if (ddlCourses.SelectedValue != "-1")
            {
                DalOperationUsers dou = new DalOperationUsers();
                DataTable dt1 = dou.SearchStudentByCourseNo(ddlCourses.SelectedValue.Trim()).Tables[0];

                Session["examSeatArrange_dt1"] = dt1;

                for (int i = 0; i < dt1.Rows.Count;i++ )
                {
                    listStudent.Add(dt1.Rows[i]["studentNo"].ToString().Trim() + "_" + dt1.Rows[i]["studentName"].ToString().Trim());
                }

                DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
                DataTable dt2 = dal.GetStudentGradeCheckApplyAccordByCourse(ddlCourses.SelectedValue).Tables[0];

                Session["examSeatArrange_dt2"] = dt2;

                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    listStudent.Add(dt2.Rows[j]["studentNo"].ToString().Trim() + "_" + dt2.Rows[j]["studentName"].ToString().Trim());
                }

                //总学生人数
                int studentsNum = dt1.Rows.Count + dt2.Rows.Count;

                //座位行数
                int rows = int.Parse(txtRows.Text.Trim());
                //座位列数
                int cols = -1;

                if (studentsNum % rows == 0)
                {
                    cols = studentsNum / rows;
                }
                else
                {
                    cols = (studentsNum / rows) + 1;
                }

                Table tb = new Table();
                tb.CssClass = "datagrid2";
                tb.Width = Unit.Percentage(100);

                TableRow tr1 = new TableRow();

                TableHeaderCell th = new TableHeaderCell();
                th.Style["text-align"] = "center";
                th.ColumnSpan = cols;
                th.Text = ddlCourses.SelectedItem.Text + "(当前共" + rows.ToString() + "行" + cols.ToString() + "列" + studentsNum + "名学生)";
                tr1.Cells.Add(th);
                tb.Rows.Add(tr1);

                for (int x = 0; x < rows; x++)
                {
                    TableRow tr = new TableRow();

                    for (int k = 0; k < cols; k++)
                    {
                        TableCell td = new TableCell();
                        td.HorizontalAlign = HorizontalAlign.Center;

                        if (studentsNum > 0)
                        {
                            List<string> listStudentCopy = listStudent;
                            List<string> newList = new List<string>();

                            while (listStudentCopy.Count > 0)
                            {
                                Random random = new Random();
                                int _index = random.Next(listStudentCopy.Count);

                                newList.Insert(0, listStudentCopy[_index]);
                                
                                listStudentCopy.RemoveAt(_index);
                            }

                            listStudent = newList;

                            int _rdm = GenerateRandom(studentsNum);

                            string _studentNo = listStudent[_rdm].Split("_".ToCharArray())[0];
                            string _studentName = listStudent[_rdm].Split("_".ToCharArray())[1];

                            td.Text = _studentName + "<br />(" + _studentNo + ")";
                            listStudent.RemoveAt(_rdm);

                            studentsNum--;
                        }
                        tr.Cells.Add(td);
                    }
                    tb.Rows.Add(tr);
                }

                phExamSeats.Controls.Add(tb);

                outputExcel.Visible = true;
            }
            else
            {
                Javascript.GoHistory(-1, "请选择课程：）", Page);
            }
        }

        protected int GenerateRandom(int end)
        {
            Random rdm = new Random();
            return rdm.Next(end);
        }
    }
}