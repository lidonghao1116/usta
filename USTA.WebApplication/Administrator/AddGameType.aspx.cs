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
    public partial class AddGameType : CheckUserWithCommonPageBase
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DalOperationAboutGameCategory dal = new DalOperationAboutGameCategory();
                DataTable dt = dal.GetList().Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlGameCategory.Items.Add(new ListItem(dt.Rows[i]["gameTitle"].ToString().Trim(), dt.Rows[i]["gameCategoryId"].ToString().Trim()));
                }
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            if (txtGameTypeTitle.Text.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "请填写活动类型名称：）", Page);
                return;
            }

            int groupCapability = 0;
            if (!(CommonUtility.SafeCheckByParams<String>(txtGroupCapability.Text.Trim(), ref groupCapability) && groupCapability > 0))
            {
                Javascript.GoHistory(-1, "每组人数必须为大于0的整数，请修改:)", Page);
                return;
            }

            try
            {
                DalOperationAboutGameType dal = new DalOperationAboutGameType();
                dal.Add(new GameType { gameTypeTitle = txtGameTypeTitle.Text.Trim(), updateTime = DateTime.Now, allowSexType = rblTeacher.SelectedValue, groupCapability = groupCapability, gameCategoryId = int.Parse(ddlGameCategory.SelectedValue) });

                Javascript.RefreshParentWindow("添加活动类型成功：）", "/Administrator/DrawManage.aspx?fragment=2&page=" + pageIndex, Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "添加活动类型失败：（", Page);
                return;
            }
        }
    }
}