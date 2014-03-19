using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace USTA.Dal
{
	using USTA.Model;
	using USTA.Common;

	/// <summary>
	/// 反馈操作类
	/// </summary>
	public class DalOperationFeedBack
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
		public DalOperationFeedBack()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		#region
		
		/// <summary>
		/// 添加反馈意见
		/// </summary>
		/// <param name="feedback">意见反馈实体</param>
		public void AddFeedBack(FeedBack feedback)
		{
			try
			{
                string sql = "INSERT INTO usta_FeedBack(feedBackTitle,feedBackContent,feedBackContactTo,[backUserNo],[backUserType],type,resolver) VALUES(@feedBackTitle,@feedBackContent ,@feedBackContactTo,@backUser,@backUserType,@type,@resolver)";

				SqlParameter[] parameters = {
					new SqlParameter("@feedBackTitle", SqlDbType.NChar,50),
					new SqlParameter("@feedBackContent", SqlDbType.NVarChar,500),
					new SqlParameter("@feedBackContactTo", SqlDbType.NChar,50),
					new SqlParameter("@backUser", SqlDbType.NVarChar,50),
					new SqlParameter("@backUserType", SqlDbType.Int),
                    new SqlParameter("@type", SqlDbType.Int), 
                    new SqlParameter("@resolver", SqlDbType.NVarChar,50)};
				parameters[0].Value = feedback.feedBackTitle;
				parameters[1].Value = feedback.feedBackContent;
				parameters[2].Value = feedback.feedBackContactTo;
				parameters[3].Value = feedback.backUserNo;
				parameters[4].Value = feedback.backUserType;
                parameters[5].Value = feedback.type;
                parameters[6].Value = feedback.resolver;
			 
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
		/// 查询所有反馈意见
		/// </summary>
		/// <returns>反馈意见数据表</returns>
		public DataTable FindFeedBack()
		{
			DataTable dt= null;
            string sql = "SELECT [feedBackId] ,[feedBackTitle],[feedBackContent] ,[feedBackContactTo],[isRead],[updateTime],type,resolver  FROM [USTA].[dbo].[usta_FeedBack] order by isRead asc,updateTime desc";//按照时间降序排列
			dt = (SqlHelper.ExecuteDataset(conn, CommandType.Text, sql)).Tables[0];
			conn.Close();
			return dt;
		}
        /// <summary>
        /// 查询四六级反馈，根据班主任查询
        /// </summary>
        /// <returns></returns>
        public DataTable FindFeedBack(string resolver)
        {
            DataTable dt = null;
            SqlParameter[] parameters = {
                  new SqlParameter("@resolver", resolver),
                                        };
            string sql = "SELECT [feedBackId] ,[feedBackTitle],[feedBackContent] ,[feedBackContactTo],[isRead],[updateTime],type,resolver  FROM [USTA].[dbo].[usta_FeedBack] where resolver=@resolver and type=2 order by isRead asc,updateTime desc";//按照时间降序排列
            dt = (SqlHelper.ExecuteDataset(conn, CommandType.Text, sql,parameters)).Tables[0];
            conn.Close();
            return dt;
        }
		/// <summary>
		/// 通过登录用户查找此用户的反馈
		/// </summary>
		/// <param name="user">用户登陆系统实体</param>
		/// <returns>数据集</returns>
		public DataSet FindByUser(UserCookiesInfo user)
		{
			string sql = "SELECT [feedBackId],[feedBackTitle],[feedBackContent],[feedBackContactTo],[isRead],[updateTime],[backInfo],[backTime],[backUserNo],[backUserType],type FROM [USTA].[dbo].[usta_FeedBack] WHERE backUserNo=@backUser AND backUserType=@backUserType";
			SqlParameter[] parameters = {
					new SqlParameter("@backUser", SqlDbType.NVarChar,50),
					new SqlParameter("@backUserType", SqlDbType.Int)};
			parameters[0].Value = user.userNo;
			parameters[1].Value = user.userType;
			return SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
		}
		
		/// <summary>
		/// 查询反馈意见 isRead=0表示未阅读,isRead=1表示已阅读
		/// </summary>
		/// <param name="isRead">是否阅读</param>
		/// <returns>数据表</returns>
		public DataTable FindFeedBackByIsRead(int isRead)
		{
			DataTable dt= null;
			string sql = "SELECT [feedBackId] ,[feedBackTitle],[feedBackContent] ,[feedBackContactTo],[isRead],[updateTime],type  FROM [USTA].[dbo].[usta_FeedBack] WHERE isRead=@isRead order by updateTime desc";//按照时间降序排列
			 SqlParameter[] parameters = {  
				 new SqlParameter("@isRead", SqlDbType.Bit,1)       
			};
			 parameters[0].Value = isRead;
			dt = (SqlHelper.ExecuteDataset(conn, CommandType.Text, sql,parameters)).Tables[0];
			conn.Close();
			return dt;
		}
		
		/// <summary>
		/// 查询反馈意见
		/// </summary>
		/// <param name="feedBackId">反馈意见编号</param>
		/// <returns>意见反馈实体</returns>
	   public FeedBack FindFeedBackById(int feedBackId)
		{
			FeedBack feedback = null;
			string sql = "SELECT [feedBackId] ,[feedBackTitle],[feedBackContent] ,[feedBackContactTo],[isRead],[updateTime],[backInfo],[backTime],[backUserNo],[backUserType],type  FROM [USTA].[dbo].[usta_FeedBack] WHERE feedBackId=@feedBackId";//按照时间降序排列
			SqlParameter[] parameters = {
				 new SqlParameter("@feedBackId", SqlDbType.Int,4)                           
			};
			parameters[0].Value = feedBackId;
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql,parameters);
		   while(dr.Read())
		   {
			   feedback = new FeedBack();
			   feedback.feedBackId = int.Parse(dr["feedBackId"].ToString());
			   feedback.feedBackTitle = dr["feedBackTitle"].ToString().Trim();
			   feedback.feedBackContent = dr["feedBackContent"].ToString().Trim();
			   feedback.feedBackContactTo = dr["feedBackContactTo"].ToString().Trim();
			   feedback.isRead = bool.Parse(dr["isRead"].ToString());
			   feedback.updateTime = DateTime.Parse(dr["updateTime"].ToString());
			   feedback.backInfo = dr["backInfo"].ToString().Trim();
			   if (dr["backTime"].ToString().Trim() != "") feedback.backTime = DateTime.Parse(dr["backTime"].ToString());
			   feedback.backUserNo = dr["backUserNo"].ToString();
			   feedback.backUserType = int.Parse(dr["backUserType"].ToString());
               feedback.type = int.Parse(dr["type"].ToString());
		   }
		   dr.Close();
		   conn.Close();
		   return feedback;
		}
	   
		/// <summary>
		/// 删除反馈意见
		/// </summary>
	   /// <param name="feedBackId">反馈意见编号</param>
	   public void DeleteFeedBackById(int feedBackId)
	   {
		   try
		   {
			   string sql = "DELETE FROM usta_FeedBack WHERE feedBackId=@feedBackId";
			   SqlParameter[] parameters = {
				 new SqlParameter("@feedBackId", SqlDbType.Int,4)                           
			};
			   parameters[0].Value = feedBackId;
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
		/// 修改反馈意见的阅读状态
		/// </summary>
	   /// <param name="feedBackId">反馈意见编号</param>
	   public void UpdateFeedBackIsReadById(int feedBackId)
	   {
		   try
		   {

			   string sql = "UPDATE  usta_FeedBack SET isRead=1 WHERE feedBackId=@feedBackId";
			   SqlParameter[] parameters = {
				 new SqlParameter("@feedBackId", SqlDbType.Int,4)                           
			};
			   parameters[0].Value = feedBackId;
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
		/// 获得feedback并且以dataset返回
		/// </summary>
	   /// <param name="feedBackId">反馈意见编号</param>
		/// <returns>查询的数据库行数</returns>
	   public DataSet GetFeedById(int feedBackId)
	   {
		   string sql = "SELECT [feedBackId] ,[feedBackTitle],[feedBackContent] ,[feedBackContactTo],[isRead],[updateTime],[backInfo],[backTime],[backUserNo],[backUserType]  FROM [USTA].[dbo].[usta_FeedBack] WHERE feedBackId=@feedBackId";//按照时间降序排列
		   SqlParameter[] parameters = {
				 new SqlParameter("@feedBackId", SqlDbType.Int,4)                           
			};
		   parameters[0].Value = feedBackId;
		   return SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
	   }

		/// <summary>
		/// 添加反馈回复
		/// </summary>
		/// <param name="feedbackId">反馈意见编号</param>
		/// <param name="backinfo">反馈内容</param>
	   public void Insertback(int feedbackId,string backinfo)
	   {
		   string sql = "UPDATE [USTA].[dbo].[usta_FeedBack]  SET [backInfo] = @backinfo,[backTime] = @backTime,[isRead]=1 WHERE feedBackId=@feedBackId";
		   SqlParameter[] parameters = {
				 new SqlParameter("@feedBackId", SqlDbType.Int,4),
				 new SqlParameter("@backinfo",SqlDbType.NVarChar ,500),
				 new SqlParameter("@backTime",SqlDbType.DateTime )   
			};
		   parameters[0].Value = feedbackId;
		   parameters[1].Value = backinfo;
		   parameters[2].Value = DateTime.Now;
		   SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
	   }
		#endregion
	}
}
