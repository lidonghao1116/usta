using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Dal;
using USTA.Bll;
using USTA.Model;

public partial class bbs_BBSUserinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment4");
        DalOperationPatch dalpa = new DalOperationPatch();

        UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
        string src = dalpa.GetAvatar(user.userNo, user.userType);
        ltavatar.Text = "<img src=\"" + src + "\">";
    }
}