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
using System.Data;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Text;


public partial class Teacher_CInfoAttachment : System.Web.UI.Page
{
    public int fileFolderType = (int)FileFolderType.archives;//结课资料文件

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Master.ShowLiControl(this.Page, "liFragment9");
        }
        DataBindArchivesItems();

    }

    //绑定结课资料规则数据
    public void DataBindArchivesItems()
    {
        DalOperationAboutArchivesItems doac = new DalOperationAboutArchivesItems();
        DataTable dv = doac.GetArchivesItemByTeacherType(Master.teacherType, Master.termtag).Tables[0];
        DalOperationAboutArchives doaa = new DalOperationAboutArchives();
        DalOperationAttachments attachment = new DalOperationAttachments();

        Literal ltl = new Literal();

        if (dv.Rows.Count == 0)
        {
            btnUpload.Visible = false;
            ltl = new Literal();
            ltl.Text = "当前暂无上传结课资料要求";

            phUpload.Controls.Add(ltl);
            return;
        }

        Table tb = new Table();
        tb.CssClass = "tableAddStyleNone";
        tb.Width = Unit.Percentage(100);


        for (int i = 0; i < dv.Rows.Count; i++)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.HorizontalAlign = HorizontalAlign.Left;
            td.CssClass = "border";
            td.Width = Unit.Percentage(10);

            HiddenField hid = new HiddenField();
            hid.ID = "hid_" + dv.Rows[i]["archiveItemId"].ToString().Trim();
            hid.ClientIDMode = ClientIDMode.Static;

            ltl = new Literal();
            ltl.Text = dv.Rows[i]["archiveItemName"].ToString().Trim() + "：";

            td.Controls.Add(ltl);
            td.Controls.Add(hid);
            tr.Cells.Add(td);

            td = new TableCell();
            td.HorizontalAlign = HorizontalAlign.Left;
            td.CssClass = "border";

            DataTable dt = doaa.FindArchivesByCourseNo(Master.courseNo, Master.classID, Master.termtag, Master.teacherType, int.Parse(dv.Rows[i]["archiveItemId"].ToString().Trim())).Tables[0];

            List<string> attachmentIds = new List<string>();

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                attachmentIds.Add(dt.Rows[j]["attachmentIds"].ToString().Trim());
            }

            string attachments = attachment.GetAttachmentsList(string.Join(",", attachmentIds.ToArray()), ref Master.iframeCount, true, (i + 1).ToString());
            ltl = new Literal();
            ltl.Text = (attachments.Trim().Length == 0 ? "未上传" : attachments)+ "&nbsp;&nbsp;<input type=\"button\" value=\"为" + dv.Rows[i]["archiveItemName"].ToString().Trim() + "添加一个附件\"" + " onclick=\"addIframe(" + fileFolderType +
"," + (i + 1).ToString() + "," + (i + 1).ToString() + ");\" />" + "&nbsp;&nbsp;<b>上传文件大小不超过</b>" + ConfigurationManager.AppSettings["uploadFileLimit"] + "<div id=\"iframes" + (i + 1).ToString() + "\"></div><br />";

            hid = new HiddenField();
            hid.ID = "hidAttachmentId" + (i + 1).ToString();
            hid.ClientIDMode = ClientIDMode.Static;
            hid.Value = string.Join(",", attachmentIds.ToArray());
            td.Controls.Add(ltl);
            td.Controls.Add(hid);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        phUpload.Controls.Add(tb);
    }

    //结课资料
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        List<int> listArchiveItemIds = new List<int>();
        List<string> listArchivesAttachmentIds = new List<string>();

        DalOperationAboutArchives doaa = new DalOperationAboutArchives();

        foreach (Control ctlTable in phUpload.Controls)
        {
            foreach (Control ctlTableRow in ctlTable.Controls)
            {
                foreach (Control ctlTableCell in ctlTableRow.Controls)
                {
                    foreach (Control ctl in ctlTableCell.Controls)
                    {
                        string _type = ctl.GetType().ToString();

                        if (_type == "System.Web.UI.WebControls.HiddenField")
                        {
                            if (ctl.ID.StartsWith("hid_"))
                            {
                                listArchiveItemIds.Add(int.Parse(ctl.ID.Split("_".ToCharArray())[1]));
                            }
                            else if (ctl.ID.StartsWith("hidAttachmentId"))
                            {
                                listArchivesAttachmentIds.Add(((HiddenField)ctl).Value);
                            }
                        }
                    }
                }
            }
        }


        Archives archives = new Archives
        {
            courseNo = Request["courseNo"],
            classID = Server.UrlDecode(Request["classID"]),
            termTag = Request["termTag"].Trim(),
            teacherType = Master.teacherType
        };


        for (int i = 0; i < listArchiveItemIds.Count; i++)
        {
            archives.archiveItemId = listArchiveItemIds[i];
            archives.attachmentIds = listArchivesAttachmentIds[i];

            //判断是否存在课程结课资料记录，并返回archiveId
            int archiveId = doaa.IsExistArchivesBycourseNo(Master.courseNo, Master.classID, Master.termtag, Master.teacherType, archives.archiveItemId);

            if (archiveId != 0)
            {
                archives.archiveId = archiveId;
                doaa.UpdateArchives(archives);
                Javascript.AlertAndRedirect("上传成功！", "CInfoAttachment.aspx?courseNo=" + archives.courseNo + "&classID=" + Master.classID + "&termtag=" + Master.termtag + "&teacherType=" + Master.teacherType, Page);
            }
            else
            {
                doaa.AddArchives(archives);
                Javascript.AlertAndRedirect("上传成功！", "CInfoAttachment.aspx?courseNo=" + archives.courseNo + "&classID=" + Master.classID + "&termtag=" + Master.termtag + "&teacherType=" + Master.teacherType, Page);
            }
        }

    }

}