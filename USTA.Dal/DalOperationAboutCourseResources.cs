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
	/// 课程资源操作类
	/// </summary>
	public class DalOperationAboutCourseResources
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
		public DalOperationAboutCourseResources()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		#region
		/// <summary>
		/// 通过课程资源主键查找课程资源
		/// </summary>
		/// <param name="CourseResourcesId">课程资源编号</param>
		/// <returns>课程资源实体</returns>
		public CourseResources GetCourseResourcesbyId(int CourseResourcesId)
		{
			CourseResources CourseResources = null;
			string commandString = "SELECT [courseResourceId] ,[courseResourceTitle],[attachmentIds],[courseNo],[updateTime] FROM [usta_CourseResources] WHERE courseResourceId=@courseResourceId";
			SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@courseResourceId",CourseResourcesId)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
			while (dr.Read())
			{
				CourseResources = new CourseResources
				{
					courseResourceId = int.Parse(dr["courseResourceId"].ToString().Trim()),
					courseResourceTitle = dr["courseResourceTitle"].ToString().Trim(),
					attachmentIds = dr["attachmentIds"].ToString().Trim(),
					courseNo = dr["courseNo"].ToString().Trim(),
					updateTime = Convert.ToDateTime(dr["updateTime"].ToString().Trim())

				};
			}
			dr.Close();
			conn.Close();

			return CourseResources;

		}

		/// <summary>
		/// 通过课程资源主键删除
		/// </summary>
		/// <param name="courseResourcesId">课程资源编号</param>
		public void DelCourseResourcesbyId(int courseResourcesId)
		{
			try
			{
				string commandString = "DELETE FROM [USTA].[dbo].[usta_CourseResources] WHERE courseResourceId=@courseResourceId";
				SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@courseResourceId",courseResourcesId)
			};
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
		/// 更新某个课程资源
		/// </summary>
		/// <param name="courseResources">课程资源实体</param>
		public void UpdateCourseResources(CourseResources courseResources)
		{
			try
			{
				string commandString = "UPDATE [USTA].[dbo].[usta_CourseResources] SET [courseResourceTitle] = @courseResourceTitle,[attachmentIds] = @attachmentIds ,[courseNo] = @courseNo,[updateTime] = @updateTime WHERE courseResourceId=@courseResourceId";

				SqlParameter[] parameters = {
					new SqlParameter("@courseResourceId", SqlDbType.Int,4),
					new SqlParameter("@courseResourceTitle", SqlDbType.NChar,50),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@updateTime", SqlDbType.DateTime)};
				parameters[0].Value = courseResources.courseResourceId;
				parameters[1].Value = courseResources.courseResourceTitle;
				parameters[2].Value = courseResources.attachmentIds;
				parameters[3].Value = courseResources.courseNo;
				parameters[4].Value = courseResources.updateTime;
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
		/// 插入一项课程资源
		/// </summary>
		/// <param name="courseResources">课程资源实体</param>
		public void InsertCourseResources(CourseResources courseResources)
		{
			try
			{
				string commandString = "INSERT INTO [USTA].[dbo].[usta_CourseResources]([courseResourceTitle],[attachmentIds],[courseNo],[updateTime],classID,termTag) VALUES  (@courseResourceTitle ,@attachmentIds,@courseNo,@updateTime,@classId,@termtag)";

				SqlParameter[] parameters = {
					new SqlParameter("@courseResourceTitle", SqlDbType.NChar,50),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
                     new SqlParameter("@classId", SqlDbType.NVarChar,50),
                     new SqlParameter("@termtag", SqlDbType.NVarChar,50)};
				parameters[0].Value = courseResources.courseResourceTitle;
				parameters[1].Value = courseResources.attachmentIds;
				parameters[2].Value = courseResources.courseNo;
				parameters[3].Value = courseResources.updateTime;
                parameters[4].Value = courseResources.classID;
                parameters[5].Value = courseResources.termTag;
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
		#endregion
	}
}
