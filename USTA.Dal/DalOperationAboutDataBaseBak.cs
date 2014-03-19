using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Data;
using System.IO;

namespace USTA.Dal
{
    using USTA.Common;

    /// <summary>
    /// 数据库备份操作类，密封
    /// </summary>
    public sealed class DalOperationAboutDataBaseBak
    {
        #region
        /// <summary>
        /// SqlConnection变量
        /// </summary>
        public SqlConnection conn
        {
            set;
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DalOperationAboutDataBaseBak()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region 数据库文件备份函数
        /// <summary>
        /// 数据库备份
        /// </summary>
        /// <returns>操作成功成功标号</returns>
        public int DataBaseBak()
        {
            DateTime dt = DateTime.Now;

            string fileFolder = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DataBaseBakPath"]);

            string fileName = dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString()
                + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + ".bak";

            string zipFileName = dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString()
                + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString() + "_" + dt.Millisecond.ToString() + ".zip";

            SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "USE master;BACKUP DATABASE USTA TO DISK='"
                + fileFolder
                + fileName  + "'");

            return 1;
        }
        
        #endregion
    }
}
