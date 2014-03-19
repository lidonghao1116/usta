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
    public partial class AddGameCategory : CheckUserWithCommonPageBase
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
        public int fileFolderType = (int)FileFolderType.draw;

        protected void Page_Load(object sender, EventArgs e)
        {

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
                dal.Add(new GameCategory { attachmentIds = hidAttachmentId.Value, startTime = Convert.ToDateTime(_startTime), endTime = Convert.ToDateTime(_endTime), gameTitle = txtGameTitle.Text.Trim(), gameContent = txttGameContent.Text.Trim(), updateTime = DateTime.Now });

                Javascript.RefreshParentWindow("添加活动届次成功：）", "/Administrator/DrawManage.aspx?fragment=1&page=" + pageIndex, Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "添加活动届次失败：（", Page);
                return;
            }
        }
    }
}