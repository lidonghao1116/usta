using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using System.Configuration;
using USTA.PageBase;

public partial class bbs_UploadAvatar : CheckUserWithCommonPageBase
{
    public int fileFolderType = (int)FileFolderType.bbsAvatar;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (hidAttachmentId.Value.Length != 0)
        {
            int attachmentId = 0;

            Attachments attachments = null;


            if (CommonUtility.SafeCheckByParams<string>(hidAttachmentId.Value, ref attachmentId))
            {
                DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                attachments = dalOperationAttachments.FindAdminNotifyAttachmentById(attachmentId);
            }

            Thumbnails.CreateImage(Server.MapPath(attachments != null ? attachments.attachmentUrl : "avatar/1-1.gif"));

            DalOperationPatch dalpa = new DalOperationPatch();
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            dalpa.SetAvatar(user.userNo, user.userType, attachments != null ? attachments.attachmentUrl : "avatar/1-1.gif");
            Javascript.RefreshParentWindow("BBSUserinfo.aspx", Page);
            return;
        }
        else
        {
            Javascript.GoHistory(-1, "请选择上传文件！", Page);
        }
    }
}