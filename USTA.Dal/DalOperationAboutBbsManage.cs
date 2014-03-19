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
	/// Bbs管理类
	/// </summary>
	public class DalOperationAboutBbsManage
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
		public DalOperationAboutBbsManage()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion


		/// <summary>
		/// 添加自定义版块
		/// </summary>
		/// <param name="bbsForum">版块对象</param>
		/// <returns>添加记录的行数</returns>
		public int AddForumInfo(BbsForum bbsForum)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@forumTitle", SqlDbType.NChar,50),
					new SqlParameter("@userNo", SqlDbType.NChar,10),
					new SqlParameter("@userType", SqlDbType.Int,4),
					new SqlParameter("@forumType", SqlDbType.Int,4)
					};
			parameters[0].Value = bbsForum.forumTitle;
			parameters[1].Value = bbsForum.userNo;
			parameters[2].Value = bbsForum.userType;
			parameters[3].Value = bbsForum.forumType;
			
			return SqlHelper.ExecuteNonQuery(conn, CommandType.Text,
				"INSERT INTO [usta_BbsForum]([forumTitle],[userNo],[userType],[forumType]) VALUES (@forumTitle,@userNo,@userType,@forumType)", parameters);
		}

		/// <summary>
		/// 编辑自定义版块
		/// </summary>
		/// <param name="bbsForum">版块对象</param>
		/// <returns>数据库中受影响的记录行数</returns>
		public int EditForumInfo(BbsForum bbsForum)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@forumId", SqlDbType.Int,4),
					new SqlParameter("@forumTitle", SqlDbType.NChar,50)
					};
			parameters[0].Value = bbsForum.forumId;
			parameters[1].Value = bbsForum.forumTitle;
		  
			return SqlHelper.ExecuteNonQuery(conn, CommandType.Text,
				"UPDATE [usta_BbsForum] SET [forumTitle] = @forumTitle WHERE [forumId]=@forumId", parameters);
		}


		/// <summary>
		/// 删除自定义版块
		/// </summary>
		/// <param name="forumId">版块编号</param>
		/// <returns>删除的记录行数</returns>
		public int DeleteForumByForumId(string forumId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@forumId",forumId)};
			return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "DELETE [usta_BbsForum] WHERE forumId=@forumId", parameters);
		}


	   /// <summary>
		/// 删除主题
	   /// </summary>
	   /// <param name="topicId">主题编号</param>
		/// <returns>删除的记录行数</returns>
		public int DeleteTopicByTopicId(int topicId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@topicId",topicId)};
		   return SqlHelper.ExecuteNonQuery(conn, "spBbsTopicsDelete", parameters);
		}

		/// <summary>
		/// 删除回复
		/// </summary>
		/// <param name="postId">回复编号</param>
		/// <returns>删除的记录行数</returns>
		public int DeletePostByPostId(int postId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@postId",postId)};
		   return SqlHelper.ExecuteNonQuery(conn, "spBbsPostsDelete", parameters);
		}

		/// <summary>
		/// 当主题数据表及回复表有更新时，更新对应版块的统计信息，包括最后更新时间、最后发表的主题等
		/// </summary>
		/// <param name="courseNo">课程编号</param>
		/// <returns>数据库受影响的行数</returns>
		public int UpdateForumStatistics(string courseNo)
		{

			SqlParameter[] parameters = {
					new SqlParameter("@courseNo",courseNo)};
			return SqlHelper.ExecuteNonQuery(conn, "spBbsPostsDelete", parameters);
		}

		/// <summary>
		/// 获得所有的版面
		/// </summary>
		/// <returns>数据集</returns>
		public DataSet GetAllForums()
		{
			string sql = "SELECT [forumId],[forumTitle],[lastUpdateTime],[lastTopicTitle],[lastTopicId] FROM [USTA].[dbo].[usta_BbsForum]";
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 以主键获得版面
		/// </summary>
		/// <param name="forumId">版块编号</param>
		/// <returns>版块对象</returns>
		public BbsForum GetForumById(string forumId)
		{
			string sql = "SELECT [forumId],[forumTitle],[lastUpdateTime],[lastTopicTitle],[lastTopicId] FROM [USTA].[dbo].[usta_BbsForum] WHERE forumId=@forumId";
			SqlParameter[] parameters = {
					new SqlParameter("@forumId",forumId)};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql,parameters);
			BbsForum bbsForum = null;

			if (dr.Read())
			{
				bbsForum = new BbsForum
				{
					forumId=int.Parse(dr["forumId"].ToString().Trim()),
					forumTitle=dr["forumTitle"].ToString().Trim()
				};

			}

			dr.Close();
			conn.Close();
			return bbsForum;
		}
		
		/// <summary>
		/// 修改版块的信息
		/// </summary>
		/// <param name="bbsForum">版块对象</param>
		public void UpdateBbsForum(BbsForum bbsForum)
		{
			try
			{
				string sql = "UPDATE usta_BbsForum SET forumTitle=@forumTitle,lastUpdateTime=@lastUpdateTime,lastTopicTitle=@lastTopicTitle,lastTopicId=@lastTopicId,bbsEmailAddress=@bbsEmailAddress WHERE forumId=@forumId";

				SqlParameter[] parameters = {
					new SqlParameter("@forumId", SqlDbType.Int,4),
					new SqlParameter("@forumTitle", SqlDbType.NChar,50),
					new SqlParameter("@lastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@lastTopicTitle", SqlDbType.NChar,50),
					new SqlParameter("@lastTopicId", SqlDbType.Int,4),
					new SqlParameter("@bbsEmailAddress", SqlDbType.NVarChar,500)};
				parameters[0].Value = bbsForum.forumId;
				parameters[1].Value = bbsForum.forumTitle;
				parameters[2].Value = bbsForum.lastUpdateTime;
				parameters[3].Value = bbsForum.lastTopicTitle;
				parameters[4].Value = bbsForum.lastTopicId;
				parameters[5].Value = bbsForum.bbsEmaiAddress;
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
		/// 获取版块的信息
		/// </summary>
		/// <param name="forumId">版块编号</param>
		/// <returns>版块对象</returns>
		public BbsForum FindBbsForum(int forumId)
		{
			BbsForum bbsForum = null;
			string sql = "SELECT [forumId],[forumTitle],[lastUpdateTime],[lastTopicTitle],[lastTopicId],[bbsEmailAddress] FROM usta_BbsForum WHERE forumId=@forumId";
			SqlParameter[] parameters = new SqlParameter[1]{               
				 new SqlParameter("@forumId",forumId)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);

			while (dr.Read())
			{
				bbsForum = new BbsForum();
				bbsForum.forumId = int.Parse(dr["forumId"].ToString().Trim());
				bbsForum.forumTitle = dr["forumTitle"].ToString().Trim();
				bbsForum.lastUpdateTime = DateTime.Parse(dr["lastUpdateTime"].ToString().Trim());
				bbsForum.lastTopicTitle = dr["lastTopicTitle"].ToString().Trim();
				bbsForum.lastTopicId = dr["lastTopicId"].ToString().Trim().Length > 0 ? int.Parse(dr["lastTopicId"].ToString().Trim()) : 0;
				bbsForum.bbsEmaiAddress = dr["bbsEmailAddress"].ToString().Trim();
			}
			dr.Close();
			conn.Close();
			return bbsForum;
		}
	}
}