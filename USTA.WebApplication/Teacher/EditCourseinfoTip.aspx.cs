using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

public partial class Teacher_EditCourseinfoTip : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.courseResources;

    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["courseNo"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        if (!IsPostBack)
        {
            DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
            Courses Courses = DalOperationAboutCourses.GetCoursesByNo(Request["courseNo"],Server.UrlDecode(Request["classID"]),Request["termtag"].Trim());
            tex_preCourses.Text = Courses.preCourse;
            text_ReferenceBooks.Text = Courses.referenceBooks;
            txtcourseurl.Text = Courses.homePage;
            txtExamtype.Text = Courses.examineMethod;
            hidencourseNo.Value = Courses.courseNo;
            txtAnswer.Text = Courses.courseAnswer;
        }

    }
    protected void EditCourses_Click(object sender, EventArgs e)
    {
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();

        Courses Courses = new Courses();

        Courses.preCourse = tex_preCourses.Text;
        Courses.referenceBooks = text_ReferenceBooks.Text;
        Courses.courseNo = hidencourseNo.Value.Trim();
        Courses.termTag = Request["termtag"].Trim();
        Courses.homePage = CommonUtility.JavascriptStringFilter(txtcourseurl.Text);
        Courses.examineMethod = CommonUtility.JavascriptStringFilter(txtExamtype.Text.Trim());
        Courses.courseAnswer = txtAnswer.Text.Trim();
        Courses.classID = Server.UrlDecode(Request["classId"].Trim());
        Courses.termTag = Request["termtag"].Trim();

        DalOperationAboutCourses.UpdateCourses(Courses);
        Javascript.RefreshParentWindow("修改成功!", "CInfoCourseInfos.aspx?courseNo=" + Courses.courseNo + "&classID="+Server.UrlEncode(Server.UrlDecode(Request["classID"].ToString().Trim()))+"&termtag="+Request["termtag"].Trim(), Page);
    }
}
