using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USTA.Model;

namespace USTA.Common
{
    /// <summary>
    /// 上传文件重命名方法类
    /// </summary>
    public sealed class UploadFileRename
    {

        /// <summary>
        /// UploadFileRename的构造函数
        /// </summary>
        public UploadFileRename()
        {

        }


        #region 根据上传文件类型的不同进行上传文件命名策略，此处暂不添加，因为目前只有一种作业及实验报告命名不同于其他
        /// <summary>
        /// 根据上传文件类型的不同进行上传文件命名策略
        /// </summary>
        public static void UploadFilesReNameStrategy()
        {
            //TODO
        }
        #endregion


        #region 作业及实验报告命名方法，不包括后缀
        /// <summary>
        ///作业及实验报告命名方法
        /// </summary>
        /// <param name="userCookiesInfo">用户Cookie信息</param>
        /// <returns>规范后的命名格式</returns>
        public static string RenameExperimentsOrSchoolWorks(UserCookiesInfo userCookiesInfo)
        {
            return String.Format("{0}_{1}_{2}", userCookiesInfo.userNo.Trim(), userCookiesInfo.userName.Trim(), UploadFiles.DateTimeString());
        }
        #endregion


        #region 公告通知命名方法，此处暂不添加，因为命名直接采用上传文件的文件名
       
        #endregion

        #region 回传文件命名方法
        /// <summary>
        /// 回传文件命名方法
        /// </summary>
        /// <param name="userCookiesInfo">用户Cookie信息</param>
        /// <returns>规范后的命名格式</returns>
        public static string RenameRemarkExperimentsAndSchoolWorks(UserCookiesInfo userCookiesInfo)
        {
            return userCookiesInfo.userNo.Trim() + "_" + userCookiesInfo.userName.Trim() + "(已批阅)" + "_" + UploadFiles.DateTimeString();
        }
        #endregion

        #region 期末归档命名方法，此处暂不添加，因为命名直接采用上传文件的文件名

        #endregion
    }
}