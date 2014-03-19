using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace USTA.Common
{
    using USTA.Model;

    /// <summary>
    /// 上传文件处理类
    /// </summary>
    public sealed class UploadFiles
    {
        private static readonly string uploadFileFolderName = ConfigurationManager.AppSettings["UploadFilesPath"];
        private static readonly string uploadFileServerPath = HttpContext.Current.Server.MapPath(uploadFileFolderName);

        #region 文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file">要上传的文件</param>
        /// <param name="fileFolderType">要上传的文件目录</param>
        /// <param name="userCookiesInfo">用户Cookies实体类</param>
        /// <returns>返回附件实体类</returns>
        public static Attachments doUpload(HttpPostedFile file, int fileFolderType, UserCookiesInfo userCookiesInfo)
        {
            //上传文件子目录，根据传入的枚举值进行获取
            string fileFolderName = GetFileFolderName(fileFolderType);

            string fileFolderNameOnDate = GetSaveFilePath();

            Attachments attachments = null;
            if (!Directory.Exists(uploadFileServerPath + fileFolderName + fileFolderNameOnDate))
            {
                Directory.CreateDirectory(uploadFileServerPath + fileFolderName + fileFolderNameOnDate);
            }

            string strNewPath = string.Empty;
            string strFileName = file.FileName;

            strNewPath = fileFolderName + fileFolderNameOnDate + GetCorrectFileName(file.FileName).Replace(GetExtension(file.FileName), "") +"_"+ DateTimeString() + GetExtension(file.FileName);

            
            ////为作业或实验上传文件重命名文件名称
            //if (fileFolderType == (int)FileFolderType.experiments || fileFolderType == (int)FileFolderType.schoolWorks)
            //{
            //    strNewPath = fileFolderName + fileFolderNameOnDate + UploadFileRename.RenameExperimentsOrSchoolWorks(userCookiesInfo) + GetExtension(file.FileName);
            //}

            ////为批改后的作业或实验上传文件重命名文件名称
            //if (fileFolderType == (int)FileFolderType.remarkExperimentsAndSchoolWorks)
            //{
            //    strNewPath = fileFolderName + fileFolderNameOnDate + UploadFileRename.RenameRemarkExperimentsAndSchoolWorks(userCookiesInfo) + GetExtension(file.FileName);
            //}

            ////为作业或实验上传文件重命名附件名称
            //if (fileFolderType == (int)FileFolderType.experiments || fileFolderType == (int)FileFolderType.schoolWorks)
            //{
            //    attachments.attachmentTitle = UploadFileRename.RenameExperimentsOrSchoolWorks(userCookiesInfo) + GetExtension(file.FileName);
            //}

            file.SaveAs(uploadFileServerPath + strNewPath);
            attachments = new Attachments { attachmentUrl = uploadFileFolderName + strNewPath, attachmentTitle = GetCorrectFileName(file.FileName) };

            return attachments;
        }

        #endregion

        #region 获取真实文件名，兼容FF和IE

        /// <summary>
        /// 获取真实文件名，兼容FF和IE
        /// </summary>
        /// <param name="waitFilterFileName">要检查的文件名</param>
        /// <returns>返回正确的文件名称</returns>
        public static string GetCorrectFileName(string waitFilterFileName)
        {
            try
            {
                int startPos = waitFilterFileName.LastIndexOf("\\");
                string ext = waitFilterFileName.Substring(startPos + 1, waitFilterFileName.Length - (startPos + 1));
                return ext;
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                return string.Empty;
            }
        }

        #endregion

        #region 文件格式后缀
        /// <summary>
        /// 文件格式后缀
        /// </summary>
        /// <param name="fileName">要检查的文件名</param>
        /// <returns>返回后缀</returns>
        public static string GetExtension(string fileName)
        {
            //.tar.bz2|.tar.gz
            try
            {
                int startPos = fileName.LastIndexOf(".");
                string ext = fileName.Substring(startPos, fileName.Length - startPos);

                if (fileName.IndexOf(".tar.bz2") != -1)
                {
                    ext = ".tar.bz2";
                }

                if (fileName.IndexOf(".tar.gz") != -1)
                {
                    ext = ".tar.gz";
                }

                return ext;
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                return string.Empty;
            }
        }
        #endregion

        #region 文件存储路径
        /// <summary>
        /// 文件存储路径
        /// </summary>
        /// <returns>文件存储路径</returns>
        private static string GetSaveFilePath()
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                string yearStr = dateTime.Year.ToString(); ;
                string monthStr = dateTime.Month.ToString();
                string dayStr = dateTime.Day.ToString();
                string hourStr = dateTime.Hour.ToString();
                string minuteStr = dateTime.Minute.ToString();

                if (int.Parse(monthStr) < 10)
                {
                    monthStr = "0" + monthStr;
                }

                if (int.Parse(dayStr) < 10)
                {
                    dayStr = "0" + dayStr;
                }

                string dir = yearStr + monthStr + dayStr;

                return dir + "/";
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                return string.Empty;
            }
        }

        #endregion

        #region 返回日期字符串，精确到毫秒数
        /// <summary>
        /// 返回日期字符串
        /// </summary>
        /// <returns> 返回日期毫秒字符串</returns>
        public static string DateTimeString()
        {
            DateTime dateTime = DateTime.Now;
            return dateTime.Year.ToString() + dateTime.Month.ToString() + dateTime.Day.ToString()
             + dateTime.Hour.ToString() + dateTime.Minute.ToString() + dateTime.Second.ToString() + dateTime.Millisecond.ToString();
        }

        #endregion

        #region 根据传入的枚举参数返回相应的文件名
        /// <summary>
        /// 根据传入的枚举参数返回相应的文件名
        /// </summary>
        /// <returns>文件名</returns>
        private static string GetFileFolderName(int fileFolderType)
        {
            string fileFolderName = string.Empty;

            foreach (int enumValue in Enum.GetValues(typeof(FileFolderType)))
            {
                if (enumValue == fileFolderType)
                {
                    fileFolderName = Enum.GetName(typeof(FileFolderType), enumValue) + "/";
                }
            }


            return fileFolderName;
        }
        #endregion

        #region 上传文件后缀名检测
        /// <summary>
        /// 上传文件后缀名检测
        /// </summary>
        /// <param name="fileName">要检查的文件</param>
        /// <returns>文件后缀名是否满足要求</returns>
        public static bool IsAllowedExtension(string fileName)
        {
            string strExtension = "";

            string extensionList = ConfigurationManager.AppSettings["fileExtension"];

            if (!string.IsNullOrEmpty(fileName))
            {
                strExtension = fileName.Substring(fileName.LastIndexOf("."));

                if (extensionList.IndexOf(strExtension.ToLower()) != -1)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
