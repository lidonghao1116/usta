using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Model;
using USTA.Common;
using System.Configuration;
using USTA.PageBase;

public partial class Administrator_EditForum : CheckUserWithCommonPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["forumId"] != null)
            {
                DalOperationAboutBbsManage DalOperationAboutBbsManage = new DalOperationAboutBbsManage();
                BbsForum bbsforum = DalOperationAboutBbsManage.FindBbsForum(int.Parse(Request["forumId"].ToString().Trim()));
                txtForumTitle.Text = bbsforum.forumTitle;
                txtEmail.Text = bbsforum.bbsEmaiAddress;
            }
        }
    }

    protected void btnCommit_Click(object sender, EventArgs e)
    {

        if (txtForumTitle.Text.Trim() == "")
        {
            Javascript.GoHistory(-1, Page);
        }
        else 
        {
            DalOperationAboutBbsManage DalOperationAboutBbsManage = new DalOperationAboutBbsManage();
            BbsForum bbsforum = DalOperationAboutBbsManage.FindBbsForum(int.Parse(Request["forumId"].ToString().Trim()));
            bbsforum.forumTitle = txtForumTitle.Text;
            bbsforum.bbsEmaiAddress = txtEmail.Text;
            DalOperationAboutBbsManage.UpdateBbsForum(bbsforum);
            Javascript.RefreshParentWindow("BbsManage.aspx", Page);
        }        
    }
}
