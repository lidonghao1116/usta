using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace USTA.Dal
{
	using USTA.Model;
	using USTA.Common;
	using System.Configuration;
	using System.Data.SqlClient;
	using System.Data;

	/// <summary>
	///  系统基本信息设置类
	/// </summary>
	public class DalOperationBaseConfig
	{
		#region 全局变量及构造函数 
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
		public DalOperationBaseConfig()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		#region
		
		/// <summary>
		/// 修改系统配置信息
		/// </summary>
		/// <param name="baseconfig">系统配置实体</param>
		public void UpdateBaseConfig(BaseConfig baseconfig)
		{
			try
			{
				string sql = "UPDATE usta_BaseConfig SET systemName=@systemName,systemVersion=@systemVersion, systemCopyRight=@systemCopyRight,fileServerAddress=@fileServerAddress";
				SqlParameter[] parameters = {
					new SqlParameter("@systemName", SqlDbType.NVarChar,100),
					new SqlParameter("@systemVersion", SqlDbType.NChar,20),
					new SqlParameter("@systemCopyRight", SqlDbType.NVarChar,500),
					new SqlParameter("@fileServerAddress", SqlDbType.NChar,50)};
				parameters[0].Value = baseconfig.systemName;
				parameters[1].Value = baseconfig.systemVersion;
				parameters[2].Value = baseconfig.systemCopyRight;
				parameters[3].Value = baseconfig.fileServerAddress;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
			}
			catch (Exception ex)
			{
			   MongoDBLog.LogRecord(ex);
				CommonUtility.RedirectUrl();
			}
			finally
			{
				conn.Close();
			}
		   

		}
		
		/// <summary>
		///  获取系统配置信息
		/// </summary>
		/// <returns>系统配置实体</returns>
		public BaseConfig FindBaseConfig()
		{
			BaseConfig baseconfig = null;
			string sql = "SELECT [systemName],[systemVersion],[systemCopyRight],[fileServerAddress] FROM [USTA].[dbo].[usta_BaseConfig]";
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql);

			while (dr.Read())
			{
				baseconfig = new BaseConfig();
				baseconfig.systemName = dr["systemName"].ToString().Trim();
				baseconfig.systemVersion = dr["systemVersion"].ToString().Trim();
				baseconfig.systemCopyRight = dr["systemCopyRight"].ToString().Trim();
				baseconfig.fileServerAddress = dr["fileServerAddress"].ToString().Trim();                
			}
			dr.Close();
			conn.Close();
			return baseconfig;
		}
		#endregion
	}
}
