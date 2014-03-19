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
	/// Bbs操作类
	/// </summary>
	public class DalOperationAboutBbs
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
		/// 学期标识
		/// </summary>
		public string termTag = string.Empty;

		/// <summary>
		/// 构造函数
		/// </summary>
		public DalOperationAboutBbs()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

			termTag = DalCommon.GetTermTag(conn);
		}
		#endregion

		/// <summary>
		/// 查询所有版块列表
		/// </summary>
		/// <returns>数据集</returns>
		public DataSet GetAllForumsList()
		{
			return GetAllForumsList(termTag);
			
		}

	   /// <summary>
	   /// 查询所有版块列表函数
	   /// </summary>
	   /// <param name="termTags">学期标识</param>
	   /// <returns>数据集</returns>
		public DataSet GetAllForumsList(string termTags)
		{
			string[] tableNames = { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
			SqlParameter[] parameters = {
					new SqlParameter("@termTag",termTags)};

			DataSet ds = new DataSet();
			SqlHelper.FillDataset(conn, CommandType.Text,
				"SELECT courseNo,courseName,termTag,classID FROM usta_Courses WHERE termTag=@termTag;" +
				"SELECT forumId,forumTitle,lastUpdateTime,lastTopicTitle,lastTopicId,[userNo],[userType],[forumType] FROM [usta_BbsForum] WHERE forumType='0';" +
				"SELECT forumId,forumTitle,lastUpdateTime,lastTopicTitle,lastTopicId,[userNo],[userType],[forumType] FROM [usta_BbsForum] WHERE forumType='1';"
				+ "SELECT COUNT(topicId) AS topicsCount,courseNo FROM usta_BbsTopics GROUP BY courseNo;"
				+ "SELECT COUNT(postId) AS postIdCount,courseNo FROM usta_BbsPosts GROUP BY courseNo;"
				+ "SELECT courseNo,postUserName,updateTime FROM usta_BbsPosts WHERE updateTime in (SELECT MAX(updateTime) FROM usta_BbsPosts GROUP BY courseNo);"
				+ "SELECT COUNT(topicId) AS todayTopicsCount,courseNo FROM usta_BbsTopics WHERE DATEDIFF(dd,updateTime,GETDATE())=0 GROUP BY courseNo;" +
				"SELECT COUNT(postId) AS todayPostsCount,courseNo FROM usta_BbsPosts WHERE DATEDIFF(dd,updateTime,GETDATE())=0 GROUP BY courseNo;" + "SELECT courseNo,Max(updateTime) AS updateTime FROM"

+ " ("
+ "SELECT Max(updateTime) AS updateTime,courseNo from usta_BbsPosts Group BY courseNo "
+ "union "
+ "SELECT Max(updateTime) AS updateTime,courseNo from usta_BbsTopics Group BY courseNo "
+ ") "
+ "AS A "
+ "Group BY courseNo;"
				, ds, tableNames, parameters);
			return ds;
			//string[] tableNames = { "0", "1" ,"2"};
			//SqlParameter[] parameters = {
			//        new SqlParameter("@termTag",termTags)};

			//DataSet ds = new DataSet();
			//SqlHelper.FillDataset(conn, CommandType.Text,
			//    "SELECT courseNo,courseName,termTag FROM usta_Courses WHERE termTag=@termTag;" +
			//    "SELECT forumId,forumTitle,lastUpdateTime,lastTopicTitle,lastTopicId,[userNo],[userType],[forumType] FROM [usta_BbsForum] WHERE forumType='0';" +
			//    "SELECT forumId,forumTitle,lastUpdateTime,lastTopicTitle,lastTopicId,[userNo],[userType],[forumType] FROM [usta_BbsForum] WHERE forumType='1';"
			//    , ds, tableNames, parameters);
			//return ds;
		}

	   /// <summary>
	   /// 查询所有主题列表,若为自定义版块，courseNo应赋值为forumId
	   /// </summary>
	   /// <param name="courseNo">课程编号</param>
		/// <returns>Dataset</returns>
		public DataSet GetAllTopicsByForumId(string courseNo)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@courseNo",courseNo)};

			string sql = "SELECT topicId,topicUserName,topicTitle,topicContent,hits,courseNo,updateTime,attachmentIds,isTop,isbigTop FROM [usta_BbsTopics] WHERE courseNo=@courseNo AND isbigTop='0' ORDER BY isTop DESC,updateTime DESC";
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 查找版块的所有主题
		/// </summary>
		/// <param name="courseNo">课程编号</param>
		/// <returns>数据集</returns>
		public DataSet GetTopicsTopByForumId(string courseNo)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@courseNo",courseNo)};

			string sql = "SELECT topicId,topicUserName,topicTitle,topicContent,hits,courseNo,updateTime,attachmentIds,isTop,isbigTop FROM [usta_BbsTopics] WHERE courseNo=@courseNo AND isbigTop='1' ORDER BY isTop DESC,updateTime DESC";
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 查询对应主题ID的信息及所有回复
		/// </summary>
		///  <param name="topicId">话题编号</param>
		/// <returns>Dataset</returns>
		public DataSet GetTopicAndPostsByTopicId(int topicId)
		{
			string[] tableNames = { "0", "1" };
			SqlParameter[] parameters = {
					new SqlParameter("@topicId",topicId)};

			DataSet ds =new DataSet();
			SqlHelper.FillDataset(conn, CommandType.Text,
				"SELECT postId,topicId,postContent,postUserName,updateTime,attachmentIds,postUserNo,postUserType FROM [usta_BbsPosts] WHERE topicId=@topicId;"+
				"SELECT topicId,topicUserName,topicTitle,topicContent,hits,courseNo,updateTime,attachmentIds,topicUserNo,topicUserType FROM [usta_BbsTopics] WHERE topicId=@topicId"
				, ds, tableNames, parameters);
			return ds;
		}

		/// <summary>
		/// 获取主题信息
		/// </summary>
		///  <param name="topicId">话题编号</param>
		/// <returns>主题对象</returns>
		public BbsTopics FindTopicById(int topicId)
		{
			BbsTopics topic = null;
			string commandstring = "select topicId,topicUserName, topicTitle,topicContent, hits,courseNo,updateTime,attachmentIds,isTop ";
				   commandstring+="from dbo.usta_BbsTopics ";
				   commandstring += "where topicId=@topicId";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@topicId",topicId)
			};

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);
			if (dr.Read())
			{
				topic = new BbsTopics
				{
					topicId = int.Parse(dr["topicId"].ToString().Trim()),
					topicUserName = dr["topicUserName"].ToString().Trim(),
					topicTitle = dr["topicTitle"].ToString().Trim(),
					topicContent = dr["topicContent"].ToString().Trim(),
					hits = int.Parse(dr["hits"].ToString().Trim()),
					courseNo = dr["courseNo"].ToString().Trim(),
					updateTime =DateTime.Parse(dr["updateTime"].ToString().Trim()),
					attachmentIds = dr["attachmentIds"].ToString().Trim(),
					isTop = int.Parse(dr["isTop"].ToString().Trim())
				};
			}
			dr.Close();
			conn.Close();
			return topic;
		}

	   /// <summary>
	   /// 添加主题
	   /// </summary>
	   /// <param name="bbsTopics">主题信息对象</param>
	   /// <returns>添加的记录数</returns>
		public int AddTopicByForumId(BbsTopics bbsTopics)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@topicUserName", SqlDbType.NChar,20),
					new SqlParameter("@topicTitle", SqlDbType.NChar,50),
					new SqlParameter("@topicContent", SqlDbType.NText),
					
					new SqlParameter("@courseNo", SqlDbType.NVarChar,50),
					
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@userNo", SqlDbType.NVarChar,50),
					new SqlParameter("@userType", SqlDbType.Int),
					new SqlParameter("@isBigTop",SqlDbType.Int,4)
					};
			parameters[0].Value = bbsTopics.topicUserName;
			parameters[1].Value = bbsTopics.topicTitle;
			parameters[2].Value = bbsTopics.topicContent;       
			parameters[3].Value = bbsTopics.courseNo;           
			parameters[4].Value = bbsTopics.attachmentIds;
			parameters[5].Value =bbsTopics.topicUserNo;
			parameters[6].Value =bbsTopics.topicUserType;
			parameters[7].Value = bbsTopics.isbigTop;

			return SqlHelper.ExecuteNonQuery(conn, CommandType.Text,"INSERT INTO [usta_BbsTopics]([topicUserName],[topicTitle],[topicContent],[courseNo],[attachmentIds],[topicUserNo],[topicUserType],[isbigTop])VALUES(@topicUserName,@topicTitle,@topicContent,@courseNo,@attachmentIds,@userNo,@userType,@isbigTop)", parameters);
		}

	   /// <summary>
	   /// 添加主题
	   /// </summary>
	   /// <param name="bbsPosts">主题对象</param>
	   /// <returns>添加的记录行数</returns>
		public int AddPostByTopicId(BbsPosts bbsPosts)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@topicId", SqlDbType.Int,4),
					new SqlParameter("@postContent", SqlDbType.NText),
					new SqlParameter("@postUserName", SqlDbType.NChar,20),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@userNo", SqlDbType.NVarChar,50),
					new SqlParameter("@userType", SqlDbType.Int),
					new SqlParameter("@courseNo", SqlDbType.NVarChar,50)};
			parameters[0].Value = bbsPosts.topicId;
			parameters[1].Value = bbsPosts.postContent;
			parameters[2].Value = bbsPosts.postUserName;         
			parameters[3].Value = bbsPosts.attachmentIds;
			parameters[4].Value = bbsPosts.postUserNo;
			parameters[5].Value = bbsPosts.postUserType;
			parameters[6].Value = bbsPosts.courseNo;
			return int.Parse(SqlHelper.ExecuteScalar(conn, "spBbsPostsAdd", parameters).ToString());
		}

		/// <summary>
		/// 取得创建话题与回复时 对应的版块bbsEmailAddress
		/// </summary>
		/// <param name="forumId">版块编号</param>
		/// <returns>版块的邮箱地址</returns>
		public string FindbbsEmailAddressFromCoursesORbbsForum(string forumId,string classId,string termtag)
		{
			string commandstring=string.Empty;
			string bbsEmailAddress = null;
			int forumtestId=0;
			if (CommonUtility.SafeCheckByParams<string>(forumId, ref forumtestId))
			{
				commandstring = "select bbsEmailAddress from dbo.usta_BbsForum where forumId=@forumId ";
			}
			else
			{
				commandstring = "select bbsEmailAddress from usta_Courses  where courseNo=@forumId AND classID=@classId AND termTag=@termtag";
			}
			SqlParameter[] parameters = new SqlParameter[3] {
				new SqlParameter("@forumId",forumId),
                new SqlParameter("@classId",classId),
                new SqlParameter("@termtag",termtag)
			};

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);
			if (dr.Read())
			  {
				  bbsEmailAddress = dr["bbsEmailAddress"].ToString().Trim().Length > 0 ? dr["bbsEmailAddress"].ToString().Trim() : string.Empty;
			 }
			dr.Close();
			conn.Close();
			return bbsEmailAddress;
		}
	   /// <summary>
	   /// 查询最后回复
	   /// </summary>
	   /// <returns>数据集</returns>
		public DataSet GetLastPost()
		{

			string sql = "SELECT COUNT(postId) AS postsCount,topicId FROM usta_BbsPosts GROUP BY topicId;";

			sql += "SELECT topicId,postId,postUserName,updateTime FROM usta_BbsPosts WHERE postId in (SELECT MAX(postId) FROM usta_BbsPosts GROUP BY topicId);";

			DataSet ds = new DataSet();

			string[] tableNames = { "0", "1" };

			SqlHelper.FillDataset(conn, CommandType.Text, sql, ds, tableNames);

			return ds;
		}

		/// <summary>
		/// 增加主题点击量
		/// </summary>
		/// <param name="topicId">话题编号</param>
		/// <returns>添加话题的行数</returns>
		public int AddTopicHits(string topicId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@topicId", SqlDbType.Int,4)};
			parameters[0].Value = topicId;
			return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "UPDATE [usta_BbsTopics] SET hits=hits+1 WHERE topicId=@topicId", parameters);
		}

		/// <summary>
		/// 搜索话题
		/// </summary>
		/// <param name="searchstring">查询关键字</param>
		/// <returns>数据集</returns>
		public DataSet SearchTopic(string searchstring)
		{
			string sql = "SELECT [topicId],[topicUserName] ,[topicTitle],[topicContent],[hits],[courseNo] ,[updateTime],[attachmentIds] FROM [USTA].[dbo].[usta_BbsTopics] WHERE (topicUserName LIKE '%'+ @searchstring +'%') OR (topicTitle like '%'+ @searchstring +'%') or (topicContent like '%'+ @searchstring +'%')";
			SqlParameter[] parameters = {
					new SqlParameter("@searchstring",searchstring)};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}
	   /// <summary>
		/// 根据版面搜索话题
	   /// </summary>
	   /// <param name="searchstring">查询关键字</param>
	   /// <param name="forum">版块编号</param>
	   /// <returns>数据集</returns>
		public DataSet SearchTopic(string searchstring,string forum)
		{
			string sql = "SELECT [topicId],[topicUserName] ,[topicTitle],[topicContent],[hits],[courseNo] ,[updateTime],[attachmentIds] FROM [USTA].[dbo].[usta_BbsTopics] WHERE ((topicUserName LIKE '%'+ @searchstring +'%') OR (topicTitle like '%'+ @searchstring +'%') or (topicContent like '%'+ @searchstring +'%')) AND courseNo=@courseNo";
			SqlParameter[] parameters = {
					new SqlParameter("@searchstring",searchstring),
					new SqlParameter("@courseNo",forum)};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 根据版面搜索回复
		/// </summary>
		/// <param name="searchstring">查询关键字</param>
		/// <returns>数据集</returns>
		public DataSet SearchPost(string searchstring)
		{
			string sql = "SELECT [postId],[usta_BbsPosts].[topicId],topicTitle,[postContent],[postUserName] ,[usta_BbsPosts].[updateTime],[usta_BbsPosts].[attachmentIds]  FROM [USTA].[dbo].[usta_BbsPosts],[USTA].[dbo].[usta_BbsTopics]  WHERE [usta_BbsPosts].topicId=[usta_BbsTopics].topicId and ((postContent like  '%'+ @searchstring +'%') or (postUserName like  '%'+ @searchstring +'%'))";
			SqlParameter[] parameters = {
					new SqlParameter("@searchstring",searchstring)};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 搜索回复
		/// </summary>
		/// <param name="searchstring">查询关键字</param>
		/// <param name="forum">版块编号</param>
		/// <returns>数据集</returns>
		public DataSet SearchPost(string searchstring, string forum)
		{
			string sql = "SELECT [postId],[usta_BbsPosts].[topicId],topicTitle,[postContent],[postUserName] ,[usta_BbsPosts].[updateTime],[usta_BbsPosts].[attachmentIds]  FROM [USTA].[dbo].[usta_BbsPosts],[USTA].[dbo].[usta_BbsTopics]  WHERE ([usta_BbsPosts].topicId=[usta_BbsTopics].topicId and ((postContent like  '%'+ @searchstring +'%') or (postUserName like  '%'+ @searchstring +'%'))) AND courseNo=@courseNo";
			SqlParameter[] parameters = {
					new SqlParameter("@searchstring",searchstring),
										new SqlParameter("@courseNo",forum)};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 设置主题置顶
		/// </summary>
		/// <param name="topicId">话题编号</param>
		/// <returns>数据库受影响的行数</returns>
		public int SetTopicOnTop(int topicId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@topicId",topicId)
										};
			return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "UPDATE [usta_BbsTopics] SET isTop=isTop+1 WHERE topicId=@topicId", parameters);
		}
/// <summary>
/// 取消主题置顶
/// </summary>
/// <param name="topicId">主题编号</param>
		/// <returns>数据库受影响的行数</returns>
		public int CancelTopicOnTop(int topicId)
		{

			SqlParameter[] parameters = {
					new SqlParameter("@topicId",topicId)
										};
			return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "UPDATE [usta_BbsTopics] SET isTop='0' WHERE topicId=@topicId", parameters);
		}
	   
	}
}