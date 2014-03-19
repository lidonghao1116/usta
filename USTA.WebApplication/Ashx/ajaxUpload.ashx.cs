using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;


using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;
using System.Web.SessionState;

public class ajaxUpload : IHttpHandler,IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        CommonFunction.CheckUser();

        context.Response.CacheControl = "no-cache";
        string hidFrId = string.Empty;
        string hiddenId = string.Empty;
        string hidFileFolderType = string.Empty;

        try
        {
            //禁止网站外提交表单
            if (context.Request.ServerVariables["HTTP_REFERER"] != null)
            {
                if (context.Request.ServerVariables["SERVER_NAME"].ToLower().Trim() != context.Request.ServerVariables["HTTP_REFERER"].ToLower().Trim().Substring(7, context.Request.ServerVariables["SERVER_NAME"].ToLower().Trim().Length))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            if (context.Request.Form["hidFileFolderType"] != null)
            {
                hidFrId = context.Request.Form["hidFrId"];
                hiddenId = context.Request.Form["hidHiddenId"];
                hidFileFolderType = context.Request.Form["hidFileFolderType"];
            }


            if (context.Request.Files.Count > 0)
            {
                HttpPostedFile file = context.Request.Files[0];

                if (UploadFiles.IsAllowedExtension(file.FileName))
                {
                    DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                    Attachments attachments = null;
                    attachments = UploadFiles.doUpload(file,
                        int.Parse(hidFileFolderType),
                        BllOperationAboutUser.GetUserCookiesInfo());
                    attachments = dalOperationAttachments.AddAdminNotifyAttachment(attachments);
                    context.Response.Write("<script type='text/javascript'>if(window.top.document.getElementById('TB_iframeContent')==null){window.top.uploadSuccess('"
                        + hidFrId + "','" + attachments.attachmentId + "','"
                        + attachments.attachmentTitle + "'," + hidFileFolderType + ",'" + hiddenId + 
                        "');} else{window.top.document.getElementById('TB_iframeContent').contentWindow.uploadSuccess('"
                        + hidFrId + "','" + attachments.attachmentId + "','" + attachments.attachmentTitle + "'," + hidFileFolderType + ",'" + hiddenId + "');}</script>");
                    context.Response.Flush();
                }
                else
                {
                    context.Response.Write("<script>window.top.uploadError('" + hidFrId + "','" + hidFileFolderType + "', '上传文件类型有误！请重新选择文件上传');</script>");
                    context.Response.Flush();
                }
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            MongoDBLog.LogRecord(ex);
            context.Response.Write("<script>window.top.uploadError('" + hidFrId + "','" + hidFileFolderType + "', '上传失败，请重新上传或者向管理员通知此错误');</script>");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}