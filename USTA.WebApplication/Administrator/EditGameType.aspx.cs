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
    public partial class EditGameType : CheckUserWithCommonPageBase
    {
        public int gameTypeId = (HttpContext.Current.Request["gameTypeId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["gameTypeId"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DalOperationAboutGameCategory dalGC = new DalOperationAboutGameCategory();
                DataTable dtGC = dalGC.GetList().Tables[0];

                for (int i = 0; i < dtGC.Rows.Count; i++)
                {
                    ddlGameCategory.Items.Add(new ListItem(dtGC.Rows[i]["gameTitle"].ToString().Trim(), dtGC.Rows[i]["gameCategoryId"].ToString().Trim()));
                }

                DalOperationAboutGameType dal = new DalOperationAboutGameType();
                DataTable dt = dal.Get(gameTypeId).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    txtGameTypeTitle.Text = dt.Rows[0]["gameTypeTitle"].ToString().Trim();

                    string _allowSexType = dt.Rows[0]["allowSexType"].ToString().Trim();

                    txtGroupCapability.Text = dt.Rows[0]["groupCapability"].ToString().Trim();
                    hidGroupCapability.Value = dt.Rows[0]["groupCapability"].ToString().Trim();

                    for (int i = 0; i < ddlGameCategory.Items.Count; i++)
                    {
                        if (ddlGameCategory.Items[i].Value == dt.Rows[0]["gameCategoryId"].ToString().Trim())
                        {
                            ddlGameCategory.SelectedIndex = i;
                            break;
                        }
                    }

                    foreach (ListItem rbt in rblTeacher.Items)
                    {
                        if (rbt.Value == _allowSexType)
                        {
                            rbt.Selected = true;
                        }
                    }
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

                //如果修改了组容量值，则首先判断是否已经有人开始抽签，如果有的话，则不允许修改组容量，但是可以修改其他内容
                if (txtGroupCapability.Text.Trim() != hidGroupCapability.Value.Trim())
                {
                    DalOperationAboutGameDrawList _dal = new DalOperationAboutGameDrawList();
                    if (_dal.CheckIsDrawByGameCategoryId(int.Parse(ddlGameCategory.SelectedValue), gameTypeId).Tables[0].Rows.Count > 0)
                    {
                        Javascript.GoHistory(-1, "当有已有教师参加抽签，禁止修改分组人数，因为这样会造成抽签数据错误，\\n，请重新发布活动届次！", Page);
                        return;
                    }
                }


                dal.Update(new GameType { gameTypeTitle = txtGameTypeTitle.Text.Trim(), updateTime = DateTime.Now, allowSexType = rblTeacher.SelectedValue, gameTypeId = gameTypeId, groupCapability = groupCapability, gameCategoryId = int.Parse(ddlGameCategory.SelectedValue) });

                Javascript.RefreshParentWindow("修改活动类型成功：）", "/Administrator/DrawManage.aspx?fragment=2", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "修改活动类型失败：（", Page);
                return;
            }
        }
    }
}