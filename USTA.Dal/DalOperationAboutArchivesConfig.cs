using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

using USTA.Model;
using System.Web;

namespace USTA.Dal
{
    /// <summary>
    /// 结课资料上传相关配置
    /// </summary>
    public class DalOperationAboutArchivesConfig
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
        public DalOperationAboutArchivesConfig()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion
        
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public DataSet GetArchivesConfig()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 startTime,endTime from usta_ArchivesConfig;");
            return SqlHelper.ExecuteDataset(conn,CommandType.Text,strSql.ToString());
		}



        /// <summary>
        /// 判断当前时间是否在通知区间时间内
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public bool CheckArchivesNotifyTime()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 startTime,endTime from usta_ArchivesConfig where @nowTime>startTime and @nowTime<endTime;");
            SqlParameter[] parameters = {
					new SqlParameter("@nowTime", SqlDbType.DateTime)
                                        };
            parameters[0].Value = DateTime.Now;

            if (SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
		/// 更新配置信息
		/// </summary>
		public int UpdateArchivesConfig(ArchivesConfig model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update usta_ArchivesConfig set ");
            strSql.Append("startTime=@startTime,");
            strSql.Append("endTime=@endTime;");
			SqlParameter[] parameters = {
					new SqlParameter("@startTime", SqlDbType.DateTime),
					new SqlParameter("@endTime", SqlDbType.DateTime)};
            parameters[0].Value = model.startTime;
            parameters[1].Value = model.endTime;

			return SqlHelper.ExecuteNonQuery(conn,CommandType.Text,strSql.ToString(),parameters);
		}
    }
}