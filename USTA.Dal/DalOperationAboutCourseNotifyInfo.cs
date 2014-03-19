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
	/// 课程信息发布通知类
	/// </summary>
	public class DalOperationAboutCourseNotifyInfo
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
		public DalOperationAboutCourseNotifyInfo()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		#region
		/// <summary>
		/// 通过课程通知的主键来查找某个通知
		/// </summary>
		/// <param name="courseNotifyInfoId">课程通知编号</param>
		/// <returns>课程通知对象</returns>
		public CoursesNotifyInfo GetCourseNotifyInfoById(int courseNotifyInfoId)
		{
			CoursesNotifyInfo CoursesNotifyInfo=null;
			string commandString = "SELECT [courseNotifyInfoId],[courseNotifyInfoTitle] ,[courseNotifyInfoContent] ,[updateTime] ,[publishUserNo]  ,[notifyType]  ,[courseNo]  ,[attachmentIds],[scanCount] FROM [USTA].[dbo].[usta_CoursesNotifyInfo] WHERE courseNotifyInfoId=@courseNotifyInfoId";
			SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@courseNotifyInfoId",courseNotifyInfoId)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn,CommandType.Text,commandString,parameters);
			while(dr.Read())
			{
				CoursesNotifyInfo = new CoursesNotifyInfo
				{
					courseNotifyInfoId = int.Parse(dr["courseNotifyInfoId"].ToString()),
					courseNotifyInfoTitle = dr["courseNotifyInfoTitle"].ToString().Trim(),
					courseNotifyInfoContent = dr["courseNotifyInfoContent"].ToString().Trim(),
					updateTime = Convert.ToDateTime(dr["updateTime"].ToString()),
					publishUserNo = dr["publishUserNo"].ToString().Trim(),
					notifyType = int.Parse(dr["notifyType"].ToString()),
					courseNo = dr["courseNo"].ToString().Trim(),
					attachmentIds = dr["attachmentIds"].ToString(),
					scanCount = int.Parse(dr["scanCount"].ToString())
				};
			}
			dr.Close();
			conn.Close();
			return CoursesNotifyInfo;
		}

		/// <summary>
		///  通过课程通知的主键来查找某个通知返回dataset
		/// </summary>
		/// <param name="courseNotifyInfoId">课程通知编号</param>
		/// <returns>Dataset</returns>
		public  DataSet GetCourseNotifyInfobyId(int courseNotifyInfoId)
		{
			string commandString = "SELECT [courseNotifyInfoId],[courseNotifyInfoTitle] ,[courseNotifyInfoContent] ,[updateTime] ,[publishUserNo]  ,[notifyType]  ,[courseNo]  ,[attachmentIds],[scanCount] FROM [USTA].[dbo].[usta_CoursesNotifyInfo] WHERE courseNotifyInfoId=@courseNotifyInfoId";
			SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@courseNotifyInfoId",courseNotifyInfoId)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 通过主键删除课程通知
		/// </summary>
		/// <param name="courseNotifyInfoId">课程通知主键</param>
		public void DelCourseNotifyInfoById(int courseNotifyInfoId)
		{
			try
			{
				string commandString = "DELETE FROM [USTA].[dbo].[usta_CoursesNotifyInfo] WHERE courseNotifyInfoId=@courseNotifyInfoId";
				SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@courseNotifyInfoId",courseNotifyInfoId)
			};
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
			}catch (Exception ex)
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
		/// 添加一项课程通知
		/// </summary>
		/// <param name="coursesNotifyInfo">课程通知实体</param>
		public void InsertCourseNotifyInfo(CoursesNotifyInfo coursesNotifyInfo)
		{
			try
			{
				string commandString = "INSERT INTO [USTA].[dbo].[usta_CoursesNotifyInfo]([courseNotifyInfoTitle],[courseNotifyInfoContent],[publishUserNo],[notifyType],[courseNo] ,[attachmentIds],classID,termTag) VALUES (@courseNotifyInfoTitle ,@courseNotifyInfoContent ,@publishUserNo ,@notifyType,@courseNo ,@attachmentIds,@classId,@termtag)";

				SqlParameter[] parameters = {
					new SqlParameter("@courseNotifyInfoTitle", SqlDbType.NChar,50),
					new SqlParameter("@courseNotifyInfoContent", SqlDbType.NText),
					
					new SqlParameter("@publishUserNo", SqlDbType.NChar,20),
					new SqlParameter("@notifyType", SqlDbType.SmallInt,2),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
                    new SqlParameter("@classId", SqlDbType.NVarChar,50),
                    new SqlParameter("@termtag", SqlDbType.NVarChar,50)};
				parameters[0].Value = coursesNotifyInfo.courseNotifyInfoTitle;
				parameters[1].Value = coursesNotifyInfo.courseNotifyInfoContent;
			   
				parameters[2].Value = coursesNotifyInfo.publishUserNo;
				parameters[3].Value = coursesNotifyInfo.notifyType;
				parameters[4].Value = coursesNotifyInfo.courseNo;
				parameters[5].Value = coursesNotifyInfo.attachmentIds;
                parameters[6].Value = coursesNotifyInfo.classID;
                parameters[7].Value = coursesNotifyInfo.termTag;
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
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
		/// 更新课程通知
		/// </summary>
		/// <param name="coursesNotifyInfo">课程通知实体</param>
		public void UpdateCourseNotifyInfo(CoursesNotifyInfo coursesNotifyInfo)
		{
			try
			{
				string commandString = " UPDATE [USTA].[dbo].[usta_CoursesNotifyInfo] SET [courseNotifyInfoTitle] = @courseNotifyInfoTitle,[courseNotifyInfoContent] = @courseNotifyInfoContent  ,[updateTime] = @updateTime ,[publishUserNo] = @publishUserNo ,[notifyType] = @notifyType ,[attachmentIds] = @attachmentIds  WHERE courseNotifyInfoId=@courseNotifyInfoId";

				SqlParameter[] parameters = {
					new SqlParameter("@courseNotifyInfoId", SqlDbType.Int,4),
					new SqlParameter("@courseNotifyInfoTitle", SqlDbType.NChar,50),
					new SqlParameter("@courseNotifyInfoContent", SqlDbType.NText),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@publishUserNo", SqlDbType.NChar,20),
					new SqlParameter("@notifyType", SqlDbType.SmallInt,2),
					
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
                    new SqlParameter("@classId",SqlDbType.NVarChar,50),
                    new SqlParameter("@termtag",SqlDbType.NVarChar,50)
					};
				parameters[0].Value = coursesNotifyInfo.courseNotifyInfoId;
				parameters[1].Value = coursesNotifyInfo.courseNotifyInfoTitle;
				parameters[2].Value = coursesNotifyInfo.courseNotifyInfoContent;
				parameters[3].Value = coursesNotifyInfo.updateTime;
				parameters[4].Value = coursesNotifyInfo.publishUserNo;
				parameters[5].Value = coursesNotifyInfo.notifyType;
			   
				parameters[6].Value = coursesNotifyInfo.attachmentIds;
                parameters[7].Value = coursesNotifyInfo.classID;
                parameters[8].Value = coursesNotifyInfo.termTag;
				
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
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
		/// 将所选的课程通知的浏览次数加1
		/// </summary>
		/// <param name="courseNotifyInfoId">课程通知主键</param>
		public void AddScanCount(int courseNotifyInfoId)
		{
			try
			{
				string sql = "UPDATE usta_CoursesNotifyInfo SET scanCount=scanCount+1 WHERE courseNotifyInfoId=@courseNotifyInfoId";
				SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@courseNotifyInfoId",courseNotifyInfoId)};
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
