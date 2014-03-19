using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

public partial class bbs_AvatarsSelect : CheckUserWithCommonPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DalOperationPatch dalpa = new DalOperationPatch();
        if (Request["avatarUrl"]!=null)
        {
            UserCookiesInfo user=BllOperationAboutUser.GetUserCookiesInfo();
            dalpa.SetAvatar(user.userNo, user.userType, Request["avatarUrl"].Trim());
            Javascript.RefreshParentWindow("BBSUserinfo.aspx", Page);
            return;
        }

        DirectoryInfo di = new DirectoryInfo(Server.MapPath("avatar"));
        


        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Extension.ToLower() == ".gif" || file.Extension.ToLower() == ".jpg")
            {
                ltlAvatar.Text += "<a href=\"?avatarUrl=avatar/" + file.Name + "\" style=\"width:100px;height:100px;\">"
                    + "<img src=\"avatar/" + file.Name + "\" border=\"0px\" title=\"点击即可修改成功~\" alt=\"点击即可修改成功~\" onmouseover=\"this.style.border='1px solid #CCCCCC';\" onmouseout=\"this.style.border='0px';\" /></a>";
            }
        }
    }
}
