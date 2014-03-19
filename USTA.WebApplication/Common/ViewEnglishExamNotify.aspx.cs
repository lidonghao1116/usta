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

namespace USTA.WebApplication.Common
{
    public partial class ViewEnglishExamNotify : CheckUserWithCommonPageBase
    {
       public int englishExamNotifyInfoId = (HttpContext.Current.Request["englishExamNotifyInfoId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["englishExamNotifyInfoId"]);

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
            dalOperationAboutEnglishExam.UpdateEnglishExamNotifyHitsById(englishExamNotifyInfoId);
            EnglishExamNotify englishExamNotify = dalOperationAboutEnglishExam.GetEnglishExamNotifyById(englishExamNotifyInfoId);

            ltlTitle.Text = englishExamNotify.englishExamNotifyTitle;

            this.ltlContent.Text = englishExamNotify.englishExamNotifyContent;
            ltlDeadLineTime.Text = englishExamNotify.deadLineTime.ToString();


            if (englishExamNotify.attachmentIds.Length > 0)
            {
                DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(englishExamNotify.attachmentIds, ref iframeCount, true,string.Empty);
            }
        }
    }
}