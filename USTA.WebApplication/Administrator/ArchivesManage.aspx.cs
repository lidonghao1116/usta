using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;
using System.Transactions;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace USTA.WebApplication.Administrator
{
    public partial class ArchivesManage : System.Web.UI.Page
    {
        public string teacherNo;
        public String term;

        public string termTag = HttpContext.Current.Request["termTag"] != null ? HttpContext.Current.Request["termTag"] : string.Empty;
        public string locale = HttpContext.Current.Request["locale"] != null ? HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["locale"]) : string.Empty;
        public string keyword = HttpContext.Current.Request["keyword"] != null ? HttpContext.Current.Request["keyword"] : string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //控制Tab的显示
                string fragmentFlag = "3";

                if (Request["fragment"] != null)
                {
                    fragmentFlag = Request["fragment"];
                }

                CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, divFragment1, divFragment2, divFragment3);


                switch (fragmentFlag)
                {
                    case "1":
                        //绑定学期标识下拉列表
                        DataBindTermTagList();
                        txtKeyword.Text = keyword;
                        //绑定课程列表--学期标识(termTag)
                        DataBindSearchCourse();
                        break;
                    case "2":
                        DataBindArchivesItems();
                        break;
                    case "3":
                        DataBindArchivesConfigInfo();
                        startTime.Attributes.Add("class", "required");
                        endTime.Attributes.Add("class", "required");
                        break;
                    default:

                        break;

                }
            }
        }

        /// <summary>
        /// 绑定配置信息
        /// </summary>
        protected void DataBindArchivesConfigInfo()
        {
            DalOperationAboutArchivesConfig dal = new DalOperationAboutArchivesConfig();
            DataTable dt= dal.GetArchivesConfig().Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                startTime.Value = Convert.ToDateTime(dt.Rows[i]["startTime"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                endTime.Value = Convert.ToDateTime(dt.Rows[i]["endTime"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 更新结课资料上传配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button3_Click(object sender, EventArgs e)
        {

            DateTime _startTime = Convert.ToDateTime(startTime.Value.Trim());
            DateTime _endTime = Convert.ToDateTime(endTime.Value.Trim());
            if (_startTime > _endTime)
            {
                Javascript.GoHistory(-1, "开始时间不能晚于截止时间，请修改:)", Page);
                return;
            }

            DalOperationAboutArchivesConfig dal = new DalOperationAboutArchivesConfig();
            dal.UpdateArchivesConfig(new ArchivesConfig { startTime = Convert.ToDateTime(startTime.Value.Trim()), endTime = Convert.ToDateTime(endTime.Value.Trim()) });
            //TODO此处需要插入当前学期所有课程带课老师的通知邮件

            Javascript.RefreshParentWindow("修改结课资料上传通知时间成功！", "/Administrator/ArchivesManage.aspx?fragment=3", Page);
        }

        /// <summary>
        /// 绑定搜索的课程信息
        /// </summary>
        public void DataBindSearchCourse()
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();

            DataView dv;

            if (!string.IsNullOrEmpty(termTag) && !string.IsNullOrEmpty(locale))
            {

                for (int i = 0; i < ddlTermTags.Items.Count; i++)
                {
                    if (ddlTermTags.Items[i].Value == termTag)
                    {
                        ddlTermTags.SelectedIndex = i;
                    }
                }

                if (termTag.Length >= 6 && termTag.Substring(5, 1) == "0")
                {
                    while (ddlPlace.Items.Count > 0)
                    {
                        ddlPlace.Items.RemoveAt(0);
                    }

                    ddlPlace.Items.Add(new ListItem("苏州", "苏州"));
                }
                else
                {
                    for (int i = 0; i < ddlPlace.Items.Count; i++)
                    {
                        if (ddlPlace.Items[i].Value == locale)
                        {
                            ddlPlace.SelectedIndex = i;
                        }
                    }
                }

                dv = doac.SearchCourses(termTag, keyword, ddlPlace.SelectedValue).Tables[0].DefaultView;
            }
            else
            {
                dv = doac.SearchCourses(ddlTermTags.SelectedValue, txtKeyword.Text.Trim(), ddlPlace.SelectedValue).Tables[0].DefaultView;

            }

            this.dlstCourses.DataSource = dv;
            this.dlstCourses.DataBind();

            if (dv.Count == 0)
            {
                this.dlstCourses.ShowFooter = true;
            }
            else
            {
                this.dlstCourses.ShowFooter = false;
            }
        }

        /// <summary>
        /// 绑定结课资料规则数据
        /// </summary>
        public void DataBindArchivesItems()
        {
            DalOperationAboutArchivesItems doac = new DalOperationAboutArchivesItems();
            UserCookiesInfo user = new UserCookiesInfo();

            DataView dv = doac.GetAllArchivesItem().Tables[0].DefaultView;


            this.dlstArchivesItems.DataSource = dv;
            this.dlstArchivesItems.DataBind();

            if (dv.Count == 0)
            {
                this.dlstArchivesItems.ShowFooter = true;
            }
            else
            {
                this.dlstArchivesItems.ShowFooter = false;
            }
        }

        #region 结课资料归档管理

        /// <summary>
        /// 绑定学期标识下拉列表
        /// </summary>
        public void DataBindTermTagList()
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.FindAllTermTags().Tables[0];
            string termTag = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                termTag = dt.Rows[i]["termTag"].ToString().Trim();
                ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
                ddlTermTags.Items.Add(li);
            }
            if (dt.Rows.Count > 0)
            {
                ddlTermTags.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// 搜索到的学生列表操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dlstArchivesItems_ItemCommand(object source, DataListCommandEventArgs e)
        {
            DalOperationAboutArchivesItems dos = new DalOperationAboutArchivesItems();

            if (e.CommandName == "delete")
            {
                int archiveItemId = int.Parse(dlstArchivesItems.DataKeys[e.Item.ItemIndex].ToString());//取选中行学生编号  

                if (dos.DeleteArchivesItemById(archiveItemId) > 0)
                {
                    Javascript.AlertAndRedirect("删除成功！", "/Administrator/ArchivesManage.aspx?fragment=2", Page);
                }
                else
                {
                    Javascript.GoHistory(-1, "删除失败！", Page);
                }
            }
        }
        #endregion



        protected string GetArchivesList(string teacherType, string courseNo, string classID, string termtag)
        {
            //已经有的附件数，页面初始化时与前端JS进行交互
            int iframeCount = 0;

            DalOperationAboutArchivesItems doac = new DalOperationAboutArchivesItems();
            DataTable dv = doac.GetArchivesItemByTeacherType(teacherType,termtag).Tables[0];
            DalOperationAboutArchives doaa = new DalOperationAboutArchives();
            DalOperationAttachments attachment = new DalOperationAttachments();

            Literal ltl = new Literal();

            if (dv.Rows.Count == 0)
            {
                return "当前暂无" + teacherType + "上传结课资料的要求<br />";
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(string.Format("<table class=\"datagrid2\"><tr><th colspan=\"2\">以下为{0}上传的结课资料：</th></tr>", teacherType));

                for (int i = 0; i < dv.Rows.Count; i++)
                {
                    sb.Append("<tr><td width=\"15%\">" + dv.Rows[i]["archiveItemName"].ToString().Trim() + "：</td>");


                    DataTable dt = doaa.FindArchivesByCourseNo(courseNo, classID, termtag, teacherType, int.Parse(dv.Rows[i]["archiveItemId"].ToString().Trim())).Tables[0];

                    List<string> attachmentIds = new List<string>();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        attachmentIds.Add(dt.Rows[j]["attachmentIds"].ToString().Trim());
                    }

                    string attachments = attachment.GetAttachmentsList(string.Join(",", attachmentIds.ToArray()), ref iframeCount, false, string.Empty);
                    sb.Append(attachments.Trim().Length == 0 ? "<td>上传文件列表：未上传</td></tr>" : "<td>上传文件列表：" + attachments + "</td></tr>");
                }
                sb.Append("</table>");
                return sb.ToString();
            }
        }
    }
}