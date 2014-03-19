using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Common
{
    public partial class ViewGradeCheckNotify : CheckUserWithCommonPageBase
    {
        public string attachmentIds = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
                DataTable dt = dal.GetGradeCheckNotify().Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ltlNotifyTitle.Text = dt.Rows[i]["notifyTitle"].ToString().Trim();
                    ltlNotifyContent.Text = dt.Rows[i]["notifyContent"].ToString().Trim();
                    attachmentIds = dt.Rows[i]["attachmentIds"].ToString().Trim();
                }
            }
        }


        public string GetURL(string aids)
        {
            int iframeCount = 0;
            DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
            return dalOperationAttachments.GetAttachmentsList(aids, ref iframeCount, false, string.Empty);
        }
    }
}