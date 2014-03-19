using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using USTA.Model;
using USTA.Common;

namespace USTA.Dal
{
	/// <summary>
	/// 管理员发布通知操作DAL类
	/// </summary>
	public class DalOperationAboutAdminNotify
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
		#endregion

		#region
		/// <summary>
		/// 构造函数
		/// </summary>
		public DalOperationAboutAdminNotify()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		#region 方法集合
		/// <summary>
		/// 获得所有的通知
		/// </summary>
		/// <returns>通知数据集</returns>
		public DataSet GetAllNotifys()
		{
			string commandstring = "SELECT [adminNotifyInfoId],[notifyTitle],[notifyContent],[notifyTypeId],[updateTime],[attachmentIds],[isTop],[scanCount] FROM [usta_AdminNotifyInfo] ORDER BY isTop DESC,updateTime DESC ";
			DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring);
			conn.Close();
			return dr;
		}

		
		/// <summary>
		/// 添加文章
		/// </summary>
		/// <param name="notify">文章的一个对象</param>
		public void AddNotifyInfo(AdminNotifyInfo notify)
		{
			try
			{
				string sql = "INSERT INTO usta_AdminNotifyInfo(notifyTitle,notifyContent,notifyTypeId,attachmentIds) VALUES(@notifyTitle,@notifyContent,@notifyTypeId,@attachmentIds)";
				SqlParameter[] parameters = {
					new SqlParameter("@notifyTitle", SqlDbType.NChar,50),
					new SqlParameter("@notifyContent", SqlDbType.NText),
					new SqlParameter("@notifyTypeId", SqlDbType.Int,4),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)
					};
				parameters[0].Value = notify.notifyTitle;
				parameters[1].Value = notify.notifyContent;
				parameters[2].Value = notify.notifyTypeId;                
				parameters[3].Value = notify.attachmentIds;
				
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
		/// 查询文章
		/// </summary>
		/// <param name="notityId"> 文章主键</param>
		/// <returns>一个文章对象</returns>
		public AdminNotifyInfo FindNotifyByNo(int notityId)
		{
			AdminNotifyInfo notify = null;
			string sql = "SELECT [adminNotifyInfoId],[notifyTitle] ,[notifyContent] ,[notifyTypeId],[updateTime] ,[attachmentIds] ,[isTop],[scanCount]  FROM [USTA].[dbo].[usta_AdminNotifyInfo] WHERE adminNotifyInfoId=@adminNotifyInfoId";
			SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@adminNotifyInfoId",notityId)
			 };
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql,parameters);

			while (dr.Read())
			{
				notify = new AdminNotifyInfo();
				notify.adminNotifyInfoIds = dr["adminNotifyInfoId"].ToString();
				notify.notifyTitle = dr["notifyTitle"].ToString().Trim();
				notify.notifyContent = dr["notifyContent"].ToString().Trim();
				notify.notifyTypeId = int.Parse(dr["notifyTypeId"].ToString());
				notify.updateTime = DateTime.Parse(dr["updateTime"].ToString());
				notify.attachmentIds = dr["attachmentIds"].ToString();
				notify.isTop = int.Parse(dr["isTop"].ToString());
				notify.scanCount = int.Parse(dr["scanCount"].ToString());
			}

			dr.Close();
			conn.Close();
			return notify;
		}

		
		/// <summary>
		/// 查询文章
		/// </summary>
		/// <param name="notityId"> 文章主键</param>
		/// <returns>通知数据集</returns>
		public DataSet FindNotifybyNo(int notityId)
		{

			string sql = "SELECT [adminNotifyInfoId],[notifyTitle] ,[notifyContent] ,[notifyTypeId],[updateTime] ,[attachmentIds] ,[isTop],[scanCount]  FROM [USTA].[dbo].[usta_AdminNotifyInfo] WHERE adminNotifyInfoId=@adminNotifyInfoId";
			SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@adminNotifyInfoId",notityId)
			 };
			DataSet ds= SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}

		 
		/// <summary>
		/// 修改文章
		/// </summary>
		/// <param name="notify">文章实体</param>
		public void UpdateNotifyInfo(AdminNotifyInfo notify)
		{
			try
			{
				string sql = "UPDATE usta_AdminNotifyInfo SET notifyTypeId=@notifyTypeId, notifyTitle=@notifyTitle, notifyContent=@notifyContent, attachmentIds=@attachmentIds, updateTime=@updateTime WHERE adminNotifyInfoId=@adminNotifyInfoId";
				SqlParameter[] parameters = {
					new SqlParameter("@adminNotifyInfoId", SqlDbType.Int,4),
					new SqlParameter("@notifyTitle", SqlDbType.NChar,50),
					new SqlParameter("@notifyContent", SqlDbType.NText),
					new SqlParameter("@notifyTypeId", SqlDbType.Int,4),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)};
				parameters[0].Value = notify.adminNotifyInfoIds;
				parameters[1].Value = notify.notifyTitle;
				parameters[2].Value = notify.notifyContent;
				parameters[3].Value = notify.notifyTypeId;
				parameters[4].Value = notify.updateTime;
				parameters[5].Value = notify.attachmentIds;
				
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
		/// 查询通知,按照类型
		/// </summary>
		/// <param name="notifyTypeId">主键</param>
		/// <returns>通知数据集</returns>
		public DataSet FindTheTop5NotifyByTypeId(int notifyTypeId)
		{
			string sql = "SELECT TOP 5 [adminNotifyInfoId],[notifyTitle],[notifyContent],[notifyTypeId],[updateTime],[attachmentIds],[isTop],[scanCount] FROM [usta_AdminNotifyInfo] WHERE notifyTypeId=@notifyTypeId ORDER BY isTop DESC,updateTime DESC ";
			SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@notifyTypeId",notifyTypeId)
			 };
			DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql,parameters);
			conn.Close();
			return dr;
		}

		
		/// <summary>
		/// 查询通知
		/// </summary>
		/// <param name="notifyTypeId">通知类型</param>
		/// <returns>通知数据集</returns>
		public DataSet FindNotifyByTypeId(int notifyTypeId)
		{
			string sql = "SELECT [adminNotifyInfoId],[notifyTitle],[notifyContent],[notifyTypeId],[updateTime],[attachmentIds],[isTop],[scanCount] FROM [usta_AdminNotifyInfo] WHERE notifyTypeId=@notifyTypeId ORDER BY isTop DESC,updateTime DESC ";
			SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@notifyTypeId",notifyTypeId)
			 };
			DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return dr;
		}

		/// <summary>
		/// 删除通知
		/// </summary>
		/// <param name="notifyId">通知主键</param>
		public void DeleteNotifyById(int notifyId)
		{
			try
			{
				string sql = "DELETE FROM usta_AdminNotifyInfo WHERE adminNotifyInfoId=@adminNotifyInfoId";
				SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@adminNotifyInfoId",notifyId)
			 };
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
		/// 将所选的通知置顶值加一
		/// </summary>
		/// <param name="notifyId">通知主键</param>
		public void Addtop(int notifyId)
		{
			try
			{
				string sql = "UPDATE usta_AdminNotifyInfo SET isTop=isTop+1 WHERE adminNotifyInfoId=@adminNotifyInfoId";
				SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@adminNotifyInfoId",notifyId)};
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql,parameters);
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
		/// 将所选的通知置顶取消
		/// </summary>
		/// <param name="notifyId">通知主键</param>
		public void Canceltop(int notifyId)
		{
			try
			{
				string sql = "UPDATE usta_AdminNotifyInfo SET isTop=0 WHERE adminNotifyInfoId=@adminNotifyInfoId";
				SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@adminNotifyInfoId",notifyId)};
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
		/// 将所选的通知浏览次数加1
		/// </summary>
		/// <param name="notifyId">通知主键</param>
		public void AddScanCount(int notifyId)
		{
			try
			{
				string sql = "UPDATE usta_AdminNotifyInfo SET scanCount=scanCount+1 WHERE adminNotifyInfoId=@adminNotifyInfoId";
				SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@adminNotifyInfoId",notifyId)};
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
		#endregion
	   
	}
}
