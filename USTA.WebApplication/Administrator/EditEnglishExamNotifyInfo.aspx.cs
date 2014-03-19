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
    public partial class EditEnglishExamNotifyInfo : CheckUserWithCommonPageBase
    {
        int englishExamNotifyInfoId = (HttpContext.Current.Request["englishExamNotifyInfoId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["englishExamNotifyInfoId"]);
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 
        public int fileFolderType = (int)FileFolderType.englishExam;

        //已经有的附件数，页面初始化时与前端JS进行交互
        public int iframeCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tryParseInt = 0;
                if (CommonUtility.SafeCheckByParams<string>(Request["englishExamNotifyInfoId"], ref tryParseInt))
                {
                    //获取Url中的参数
                    englishExamNotifyInfoId = tryParseInt;
                }

                InitialNotifyEdit(englishExamNotifyInfoId);
            }
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int englishExamNotifyInfoId)
        {
            DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
            EnglishExamNotify englishExamNotify = dalOperationAboutEnglishExam.GetEnglishExamNotifyById(englishExamNotifyInfoId);


            txtTitle.Text = englishExamNotify.englishExamNotifyTitle;

            this.Textarea1.Value = englishExamNotify.englishExamNotifyContent;
            datepicker.Value = englishExamNotify.deadLineTime.ToString("yyyy-MM-dd HH:mm:ss");

            hidAttachmentId.Value = englishExamNotify.attachmentIds;

            if (englishExamNotify.attachmentIds.Length > 0)
            {
                DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(englishExamNotify.attachmentIds, ref iframeCount, true, string.Empty);
            }

            if (englishExamNotify.locale.Trim() == "苏州")
            {
                ddlLocale.Items.Add(new ListItem("苏州", "苏州"));
                ddlLocale.Items.Add(new ListItem("合肥", "合肥"));
            }
            else if (englishExamNotify.locale.Trim() == "合肥")
            {
                ddlLocale.Items.Add(new ListItem("合肥", "合肥"));
                ddlLocale.Items.Add(new ListItem("苏州", "苏州"));
            }
        }
        //修改
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim().Length == 0 || this.Textarea1.Value.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "标题和内容不能为空，请输入！", Page);
            }
            else
            {
                DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
                EnglishExamNotify notify = new EnglishExamNotify();
                notify.englishExamNotifyId = englishExamNotifyInfoId;

                notify.englishExamNotifyTitle = txtTitle.Text.Trim();
                notify.englishExamNotifyContent = this.Textarea1.Value.Trim();
                notify.attachmentIds = hidAttachmentId.Value;
                notify.deadLineTime = Convert.ToDateTime(datepicker.Value);
                notify.updateTime = DateTime.Now;
                notify.locale = ddlLocale.SelectedValue;

                try
                {

                    dalOperationAboutEnglishExam.UpdateEnglishExamNotifyById(notify);//修改
                    Javascript.RefreshParentWindow("修改成功！", "/Administrator/EnglishExamManage.aspx?page="+pageIndex, Page);
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