using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;

using USTA.Common;
using USTA.Dal;
using USTA.PageBase;

namespace USTA.WebApplication.Teacher.Ashx
{
    /// <summary>
    /// BatchDownloadFiles 的摘要说明
    /// </summary>
    public class BatchDownloadFiles : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            context.Response.ContentType = "text/html";
            if (context.Request["attachmentIds"] != null)
            {
                IList<string> ilist = new List<string>();
                DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
                ilist = dalOperationAttachments.GetAttachmentByIds(context.Request["attachmentIds"]);
                if (ilist.Count > 0)
                {
                    string zipedFile = "批量文件_" + UploadFiles.DateTimeString() + ".zip";
                    ZipAndUnZipFile.MultiFilesZip(ilist, context.Server.MapPath("/temp/") + zipedFile, 4096);
                    context.Response.Clear();
                    context.Response.Redirect("/temp/" + zipedFile);
                    context.Response.Flush();
                }
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
}