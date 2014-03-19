using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using System.Configuration;
using USTA.PageBase;

public partial class Administrator_ViewCourseAttention : CheckUserWithCommonPageBase
{
    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["courseNo"] != null)
            {
                courseNo = Request["courseNo"];

                CalculateCourseAttention();
            }
        }
    }

    //统计信息
    public void CalculateCourseAttention()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DalOperationStudentSpecility dalspeciality = new DalOperationStudentSpecility();

        Courses courses=doac.FindCourseByNo(courseNo,classID,termtag);

        if (courses != null)
        {
            //调用统计方法
            Hashtable ht = doac.CalculateCourseAttentionNumber(courseNo,classID,termtag); 
            int total = 0;
            result.Text = "";

            result.Text += "<tr>";
            result.Text += "<td>" + "课程编号：" + courseNo + "</td>";
            result.Text += "<td>" + "课程名称：" + courses.courseName + "</td>";
            result.Text += "</tr>";

            result.Text += "<tr>";
            result.Text += "<td>" + "专业名称" + "</td>";
            result.Text += "<td>" + "关注人数 (单位：名)" + "</td>";
            result.Text += "</tr>";  

            //分专业统计关注人数信息
            foreach (DictionaryEntry objDE in ht)
            {
                result.Text += "<tr>";
                result.Text += "<td>" + dalspeciality.FindSpecilityNameByMajorTypeID(objDE.Key.ToString()) + "</td>";
                result.Text += "<td>" + objDE.Value.ToString() + "</td>";
                result.Text += "</tr>";
                total += int.Parse(objDE.Value.ToString());
            }

            result.Text += "<tr>";
            result.Text += "<td>" + "总人数：" + "</td>";
            result.Text += "<td>" + total.ToString() + "</td>";
            result.Text += "</tr>";
        }
    }
}