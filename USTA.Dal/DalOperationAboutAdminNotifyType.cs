using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace USTA.Dal
{
	using USTA.Model;
	using USTA.Common;
	using System.Configuration;
	using System.Data.SqlClient;
	using System.Data;

	/// <summary>
	/// 通知类型操作类
	/// </summary>
	public class DalOperationAboutAdminNotifyType
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
		public DalOperationAboutAdminNotifyType()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		
		/// <summary>
		/// 添加管理员通知类型
		/// </summary>
		/// <param name="notifyType">通知类型对象</param>
		public void AddAdminNotifyType(AdminNotifyType notifyType)
		{
			try
			{
				string strSql = "insert into usta_AdminNotifyType(";
				strSql += "notifyTypeName,sequence,parentId) ";
				strSql += " values (";
				strSql += "@notifyTypeName,@sequence,@parentId)";
				SqlParameter[] parameters = {
					new SqlParameter("@notifyTypeName", SqlDbType.NVarChar,50),
					new SqlParameter("@sequence", SqlDbType.Int,4),
					new SqlParameter("@parentId", SqlDbType.Int,4)};
				parameters[0].Value = notifyType.notifyTypeName;
                parameters[1].Value = notifyType.sequence;
                parameters[2].Value = notifyType.parentId;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql, parameters);
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
        /// 查询所有的一级分类通知类型
        /// </summary>
        /// <returns>管理员数据信息集</returns>
        public DataSet FindAllParentAdminNotifyType()
        {
            DataSet ds = null;
            try
            {
                string strSql = "select notifyTypeId,notifyTypeName,sequence from usta_AdminNotifyType where parentId=0 order by sequence asc";
                ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
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
            return ds;
        }

        /// <summary>
        /// 根据父级Id查询所有的二级分类通知类型
        /// </summary>
        /// <returns>管理员数据信息集</returns>
        public DataSet FindAllAdminNotifyTypeByParentId(int parentId)
        {
            DataSet ds = null;
            try
            {
                string strSql = "select notifyTypeId,notifyTypeName,sequence from usta_AdminNotifyType WHERE parentId=" + parentId + " order by sequence asc";
                ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
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
            return ds;
        }

        /// <summary>
        /// 根据子级Id查询父级Id
        /// </summary>
        /// <returns>管理员数据信息集</returns>
        public DataSet FindParentIdById(int id)
        {
            DataSet ds = null;
            try
            {
                string strSql = "select notifyTypeId,notifyTypeName,sequence,parentId from usta_AdminNotifyType WHERE notifyTypeId=" + id + " order by sequence asc";
                ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
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
            return ds;
        }
		
		/// <summary>
		/// 查询所有的管理员通知类型
		/// </summary>
		/// <returns>管理员数据信息集</returns>
		public DataSet FindAllAdminNotifyType()
		{
			DataSet ds = null;
			try
			{
				string strSql = "select notifyTypeId,notifyTypeName,sequence from usta_AdminNotifyType order by sequence asc";
				ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
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
			return ds;
		}
		
		/// <summary>
		/// 按照主键查找通知类型
		/// </summary>
		/// <param name="notifyTypeId">通知类型主键</param>
		/// <returns>通知类型对象</returns>
		public AdminNotifyType FindAdminNotifyTypeById(int notifyTypeId)
		{
			AdminNotifyType model = null;
			string commandstring = "select notifyTypeId,notifyTypeName,sequence,parentId from usta_AdminNotifyType where notifyTypeId=@notifyTypeId";
			SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@notifyTypeId",notifyTypeId)
			};

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);
			if (dr.Read())
			{
				model = new AdminNotifyType
				{
					notifyTypeId = int.Parse(dr["notifyTypeId"].ToString().Trim()),
					notifyTypeName = dr["notifyTypeName"].ToString().Trim(),
                    sequence = int.Parse(dr["sequence"].ToString().Trim()),
                    parentId = int.Parse(dr["parentId"].ToString().Trim())    
				};
			}
			dr.Close();
			conn.Close();
			return model;
		}
		
		/// <summary>
		/// 查询第一个管理员通知类型
		/// </summary>
		/// <returns>通知类型对象</returns>
		public AdminNotifyType FindTheFirstAdminNotifyType()
		{
			AdminNotifyType model = null;
			string commandstring = "select top 1 notifyTypeId,notifyTypeName,sequence from usta_AdminNotifyType ";
		   
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring);
			if (dr.Read())
			{
				model = new AdminNotifyType
				{
					notifyTypeId = int.Parse(dr["notifyTypeId"].ToString().Trim()),
					notifyTypeName = dr["notifyTypeName"].ToString().Trim(),
					sequence = int.Parse(dr["sequence"].ToString().Trim())  
				};
			}
			dr.Close();
			conn.Close();
			return model;
		}
	   
	  
		 
		 
		 
		
		/// <summary>
		/// 删除一个管理员通知类型
		/// </summary>
		/// <param name="notifyTypeId">通知类型主键</param>
		public void DeleteAdminNotifyTypeById(int notifyTypeId)
		{
			try
			{
				string strSql = "delete from usta_AdminNotifyType where notifyTypeId=@notifyTypeId ";
				SqlParameter[] parameters = {
					new SqlParameter("@notifyTypeId", SqlDbType.Int,4)};
				parameters[0].Value = notifyTypeId;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql, parameters);
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
		/// 修改管理员通知类型
		/// </summary>
		/// <param name="notifyType">通知类型实体</param>
		public void UpdateAdminNotifyType(AdminNotifyType notifyType)
		{
			try
			{
				string strSql = "update usta_AdminNotifyType set notifyTypeName=@notifyTypeName,sequence=@sequence where notifyTypeId=@notifyTypeId";
				SqlParameter[] parameters = new SqlParameter[4]{
					new SqlParameter("@notifyTypeId",notifyType.notifyTypeId),
					new SqlParameter("@notifyTypeName",notifyType.notifyTypeName),
					new SqlParameter("@sequence",notifyType.sequence),
					new SqlParameter("@parentId",notifyType.parentId)
				};

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql, parameters);
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

	}
}
