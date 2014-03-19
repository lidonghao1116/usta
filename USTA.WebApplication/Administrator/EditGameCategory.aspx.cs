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
using USTA.PageBase;
using System.Configuration;

namespace USTA.WebApplication.Administrator
{
    public partial class EditGameCategory : CheckUserWithCommonPageBase
    {
        public int gameCategoryId = (HttpContext.Current.Request["gameCategoryId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["gameCategoryId"]);
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
        public int fileFolderType = (int)FileFolderType.draw;
        //已经有的附件数，页面初始化时与前端JS进行交互
        public int iframeCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitialNotifyEdit(gameCategoryId);
            }
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int englishExamNotifyInfoId)
        {
            DalOperationAboutGameCategory dal = new DalOperationAboutGameCategory();
            DataTable dt = dal.Get(gameCategoryId).Tables[0];

            if (dt.Rows.Count > 0)
            {
                txtGameTitle.Text = dt.Rows[0]["gameTitle"].ToString().Trim();

                txttGameContent.Text = dt.Rows[0]["gameContent"].ToString().Trim();

                startTime.Value = Convert.ToDateTime(dt.Rows[0]["startTime"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                endTime.Value = Convert.ToDateTime(dt.Rows[0]["endTime"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");


                string _attachmentIds = dt.Rows[0]["attachmentIds"].ToString().Trim();

                hidAttachmentId.Value = _attachmentIds;
                if (_attachmentIds.Length > 0)
                {
                    DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                    ltlAttachment.Text = dalOperationAttachments.GetAttachmentsList(_attachmentIds, ref iframeCount, true, string.Empty);
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            DateTime _startTime = Convert.ToDateTime(startTime.Value.Trim());
            DateTime _endTime = Convert.ToDateTime(endTime.Value.Trim());
            if (_startTime > _endTime)
            {
                Javascript.GoHistory(-1, "开始时间不能晚于截止时间，请修改:)", Page);
                return;
            }

            try
            {
                DalOperationAboutGameCategory dal = new DalOperationAboutGameCategory();
                dal.Update(new GameCategory { attachmentIds = hidAttachmentId.Value, startTime = Convert.ToDateTime(_startTime), endTime = Convert.ToDateTime(_endTime), gameTitle = txtGameTitle.Text.Trim(), gameContent = txttGameContent.Text.Trim(), updateTime = DateTime.Now, gameCategoryId = gameCategoryId });

                Javascript.RefreshParentWindow("更新活动届次成功：）", "/Administrator/DrawManage.aspx?fragment=1&page=" + pageIndex, Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "更新活动届次失败：（", Page);
                return;
            }
        }
    }
}