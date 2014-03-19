using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using System.Configuration;
using System.Data;
using USTA.PageBase;
using System.Text;

public partial class Administrator_ViewArchives : CheckUserWithCommonPageBase
{

    public string courseNo = HttpContext.Current.Request["courseNo"] != null ? HttpContext.Current.Request["courseNo"] : string.Empty;
    public string classID = HttpContext.Current.Request["ClassID"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ClassID"]) : string.Empty;
    public string termtag = HttpContext.Current.Request["termtag"] != null ? HttpContext.Current.Request["termtag"] : string.Empty;
    public Courses course = null;


    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //查看结课资料
        if (!IsPostBack)
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            course = doac.GetCoursesByNo(courseNo, classID, termtag);
            if (course == null)
            {
                course = new Courses { courseNo = courseNo, classID = classID, termTag = termtag };
            }

            ltlAttachments.Text = "该课程还没有上传期末资料归档！";

            DalOperationAboutArchives doaa = new DalOperationAboutArchives();

            if (doaa.IsExistArchivesBycourseNoCompatible(courseNo, classID, termtag) != 0)
            {
                string attachmentIds = doaa.FindArchivesByCourseNoCompatible(courseNo, classID, termtag).attachmentIds;
                DalOperationAttachments attachment = new DalOperationAttachments();
                string _attachmentIds = attachment.GetAttachmentsList(attachmentIds, ref iframeCount, false,string.Empty);
                ltlAttachments.Text = _attachmentIds.Trim().Length == 0 ? "未上传资料" : _attachmentIds;
            }

            DataBindArchivesItems("教师");
            DataBindArchivesItems("助教");
        }

    }


    //绑定结课资料规则数据
    public void DataBindArchivesItems(string teacherType)
    {
        ltlAttachments.Text = string.Empty;

        DalOperationAboutArchivesItems doac = new DalOperationAboutArchivesItems();
        DataTable dv = doac.GetArchivesItemByTeacherType(teacherType, termtag).Tables[0];
        DalOperationAboutArchives doaa = new DalOperationAboutArchives();
        DalOperationAttachments attachment = new DalOperationAttachments();

        Literal ltl = new Literal();

        if (dv.Rows.Count == 0)
        {
            ltl = new Literal();
            ltl.Text = "当前暂无上传结课资料要求<br />";

            phUpload.Controls.Add(ltl);
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("<table class=\"datagrid2\"><tr><th>以下为{0}上传的结课资料：</th></tr>", teacherType));

            for (int i = 0; i < dv.Rows.Count; i++)
            {
                sb.Append("<tr><td>" + dv.Rows[i]["archiveItemName"].ToString().Trim() + "</td></tr>");


                DataTable dt = doaa.FindArchivesByCourseNo(courseNo, classID, termtag, teacherType, int.Parse(dv.Rows[i]["archiveItemId"].ToString().Trim())).Tables[0];

                List<string> attachmentIds = new List<string>();

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    attachmentIds.Add(dt.Rows[j]["attachmentIds"].ToString().Trim());
                }

                string attachments = attachment.GetAttachmentsList(string.Join(",", attachmentIds.ToArray()), ref iframeCount, false, string.Empty);
                sb.Append(attachments.Trim().Length == 0 ? "<tr><td>上传文件列表：未上传</td></tr>" : "<tr><td>上传文件列表：" + attachments + "</td></tr>");
            }
            sb.Append("</table>");

            ltl = new Literal();
            ltl.Text = sb.ToString();
            phUpload.Controls.Add(ltl);
        }
    }


}
