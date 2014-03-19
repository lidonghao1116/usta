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
using System.Text;
using System.Collections;

namespace USTA.WebApplication.Administrator
{
    public partial class OutputGradeCheckApplyWord1 : System.Web.UI.Page
    {
        string courseName = HttpContext.Current.Request["courseName"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltlWord.Text = WordDataBind();
            }
        }

        protected string WordDataBind()
        {
            Response.CacheControl = "no-cache";

            string termTagCourseNoClassID = Request["termTagCourseNoClassID"];
            string termTag = Request["termTag"];

            //获取对应课程的重修重考学生信息
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
            DataSet ds = dal.GetStudentGradeCheckApplyAccordByCourse(termTagCourseNoClassID);
            DataTable dt = ds.Tables[0];

            StringBuilder sb = new StringBuilder();


            if (dt.Rows.Count == 0)
            {
                Response.Write("当前课程未找到重修重考记录：（");
                return string.Empty;
            }
            //每页包含的重修重考记录数
            int pagesize = 12;

            int limitCount = (dt.Rows.Count % pagesize == 0 ? dt.Rows.Count / pagesize : dt.Rows.Count / pagesize + 1);


            //目前表格只能容纳13行记录
            for (int x = 0; x < limitCount; x++)
            {

                //获取课程数据
                DalOperationAboutCourses dalCourses = new DalOperationAboutCourses();
                DataTable dtCourses = dalCourses.GetCoursesByTermTagCourseNoClassID(termTagCourseNoClassID).Tables[0];
                //拼接课程抬头信息
                sb.Append("<table width=\"100%\" cellspacing=\"3\"><tr><td colspan=\"4\" align=\"center\" style=\"font-size:19pt;font-family:'宋体';\" cell>中国科学技术大学研究生重修重考申请表</td></tr>");
                sb.Append("<tr><td colspan=\"4\" align=\"center\" style=\"font-size:11.5pt;font-family:'宋体';font-family:'黑体';\">" + CommonUtility.FormatTermTag(termTag) + "</td></tr>");

                if (dtCourses.Rows.Count > 0)
                {
                    string _location = (dtCourses.Rows[0]["ClassID"].ToString().Trim().Contains("合肥") ? "软件学院合肥" : "软件学院苏州");
                    sb.Append(string.Format("<tr><td style=\"font-size:11.5pt;font-family:'宋体';\">研究生所在系(室)：</td><td style=\"font-size:11.5pt;font-family:'宋体';border-bottom:black 1px solid;\">{0}</td><td style=\"font-size:11.5pt;font-family:'宋体';\">开课系(室)：</td><td style=\"font-size:11.5pt;font-family:'宋体';border-bottom:black 1px solid;\">{1}</td></tr>", _location, _location));
                    sb.Append(string.Format("<tr><td style=\"font-size:11.5pt;font-family:'宋体';\">课&nbsp;&nbsp;程&nbsp;&nbsp;编&nbsp;&nbsp;号：</td><td style=\"font-size:11.5pt;font-family:'宋体';border-bottom:black 1px solid;\">{0}</td><td style=\"font-size:11.5pt;font-family:'宋体';\">总&nbsp;&nbsp;&nbsp;学&nbsp;&nbsp;&nbsp;时：</td><td style=\"font-size:11.5pt;font-family:'宋体';border-bottom:black 1px solid;\">{1}</td></tr>", dtCourses.Rows[0]["courseNo"].ToString().Trim(), dtCourses.Rows[0]["period"].ToString().Trim() + "/" + dtCourses.Rows[0]["TestHours"].ToString().Trim()));
                    sb.Append(string.Format("<tr><td style=\"font-size:11.5pt;font-family:'宋体';\">课&nbsp;&nbsp;程&nbsp;&nbsp;名&nbsp;&nbsp;称：</td><td style=\"font-size:11.5pt;font-family:'宋体';border-bottom:black 1px solid;\">{0}</td><td style=\"font-size:11.5pt;font-family:'宋体';\">学&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;分：</td><td style=\"font-size:11.5pt;font-family:'宋体';border-bottom:black 1px solid;\">{1}</td></tr>", dtCourses.Rows[0]["courseName"].ToString().Trim(), dtCourses.Rows[0]["credit"].ToString().Trim()));

                    string _teacherName = string.Empty;
                    DataTable _dtTeachers = dalCourses.GetTeachersByTermTagCourseNoClassID(termTagCourseNoClassID).Tables[0];
                    if (_dtTeachers.Rows.Count > 0)
                    {
                        sb.Append(string.Format("<tr><td style=\"font-size:11.5pt;font-family:'宋体';\">主      讲      老    师：</td><td style=\"font-size:11.5pt;font-family:'宋体';border-bottom:black 1px solid;\">{0}</td><td></td><td></td></tr>", _dtTeachers.Rows[0]["teacherName"].ToString().Trim()));
                    }
                }

                sb.Append("</table>");


                //拼接重修重考记录表格
                sb.Append("<table width=\"100%\" cellSpacing=\"0\" style=\"font-size:'小四';border-left:black 1px solid;border-top:black 1px solid;margin-top:20px;\"><tr><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:8%;\">序号</td><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:15%;\">学号</td><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:10%;\">姓名</td><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:20%;\">班级</td><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:20%;\">重修重考原因</td><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:10%;\">重修 Or 重考</td><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:10%;\">备注</td><td valign=\"top\" style=\"border-right:black 1px solid;border-bottom:black 1px solid;width:7%;\">成绩</td></tr>");

                for (int i = 0; i < pagesize; i++)
                {
                    int _itemIndex = x * pagesize + i;

                    if (_itemIndex < dt.Rows.Count)
                    {
                        sb.Append(string.Format("<tr><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{0}</td><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{1}</td><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{2}</td><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{3}</td><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{4}</td><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{5}</td><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{6}</td><td style=\"padding-left:3px;border-right:black 1px solid;border-bottom:black 1px solid;\">{7}</td></tr>", i + 1, dt.Rows[_itemIndex]["studentNo"].ToString().Trim(), dt.Rows[_itemIndex]["studentName"].ToString().Trim(), dt.Rows[_itemIndex]["SchoolClassName"].ToString().Trim(), dt.Rows[_itemIndex]["applyReason"].ToString().Trim(), dt.Rows[_itemIndex]["gradeCheckApplyType"].ToString().Trim(), dt.Rows[_itemIndex]["applyChecKSuggestion"].ToString().Trim(), string.Empty));
                    }
                }
                sb.Append("</table>");

                //拼接重修重考页脚信息
                sb.Append("<table width=\"100%\"><tr><td colspan=\"4\" style=\"font-size:8.5pt;font-family:'宋体';margin-top:20px;\">注：为确保成绩登记无误，请教学秘书务必参照“硕士研究生课程一览表”准确填写课程编号和名称。</td></tr>");
                sb.Append("<tr><td style=\"font-size:11.5pt;font-family:'宋体';\">任课教师签字：</td><td style=\"font-size:11.5pt;font-family:'宋体';\">年</td><td style=\"font-size:11.5pt;font-family:'宋体';\">月</td><td style=\"font-size:11.5pt;font-family:'宋体';\">日</td></tr>");
                sb.Append("</table>");
            }
            return sb.ToString();
        }

        protected void btnOutputWord_Click(object sender, EventArgs e)
        {

            string fileName = courseName + "_重修重考数据汇总" + UploadFiles.DateTimeString();


            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Encoding code = Encoding.GetEncoding("gb2312");
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = code;//这句很重要
            Response.ContentType = "application/ms-word";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".doc");

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            this.ltlWord.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}