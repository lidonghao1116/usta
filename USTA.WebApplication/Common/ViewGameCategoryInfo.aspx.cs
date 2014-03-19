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
    public partial class ViewGameCategoryInfo : CheckUserWithCommonPageBase
    {
        public int gameCategoryId = (HttpContext.Current.Request["gameCategoryId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["gameCategoryId"]);

        //已经有的附件数，页面初始化时与前端JS进行交互
        public int iframeCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tryParseInt = 0;
                if (CommonUtility.SafeCheckByParams<string>(Request["gameCategoryId"], ref tryParseInt))
                {
                    //获取Url中的参数
                    gameCategoryId = tryParseInt;
                }

                InitialNotifyEdit(gameCategoryId);
            }
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int gameCategoryId)
        {
            DalOperationAboutGameCategory dal = new DalOperationAboutGameCategory();
            DataTable dt = dal.Get(gameCategoryId).Tables[0];

            if (dt.Rows.Count > 0)
            {
                ltlTitle.Text = dt.Rows[0]["gameTitle"].ToString().Trim();

                this.ltlContent.Text = dt.Rows[0]["gameContent"].ToString().Trim();
                ltlDeadLineTime.Text = dt.Rows[0]["endTime"].ToString().Trim();

                if (dt.Rows[0]["attachmentIds"].ToString().Trim().Length > 0)
                {
                    DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                    ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(dt.Rows[0]["attachmentIds"].ToString().Trim(), ref iframeCount, true, string.Empty);
                }
            }
        }
    }
}