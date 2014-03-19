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
using System.Transactions;

using USTA.Model;
using USTA.Common;

namespace USTA.Dal
{
	/// <summary>
	/// Bbs话题操作类
	/// </summary>
	public class DalOperationPatch
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
		public DalOperationPatch()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion


		#region
		/// <summary>
		/// 更新话题
		/// </summary>
		/// <param name="topicId">话题编号</param>
		/// <param name="title">标题</param>
		/// <param name="content">话题内容</param>
		/// <param name="attachmentId">附件编号</param>
		/// <returns>话题更新行数</returns>
		public int UpdateBBSTopic(int topicId, string title, string content, string attachmentId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@topicId", SqlDbType.Int),
					new SqlParameter("@topicTitle", SqlDbType.NChar,50),
					new SqlParameter("@topicContent", SqlDbType.NText),			
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)
					};
			parameters[0].Value = topicId;
			parameters[1].Value = title;
			parameters[2].Value = content;
			parameters[3].Value = attachmentId;
			string sql = "UPDATE [USTA].[dbo].[usta_BbsTopics]   SET [topicTitle] = @topicTitle ,[topicContent] = @topicContent,[attachmentIds] = @attachmentIds WHERE topicId=@topicId";
			
			return  SqlHelper.ExecuteNonQuery(conn,CommandType.Text, sql, parameters);
		}
	   /// <summary>
	   /// 查询最新话题
	   /// </summary>
	   /// <returns>话题数据集</returns>
		public DataSet GetLatestTopic()
		{
			string sql = "SELECT courseNo,Max(updateTime) AS updateTime FROM"

+" ("
+"SELECT Max(updateTime) AS updateTime,courseNo from usta_BbsPosts Group BY courseNo "
+"union "
+"SELECT Max(updateTime) AS updateTime,courseNo from usta_BbsTopics Group BY courseNo "
+ ") "
+ "AS A "
+"Group BY courseNo";

			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			return ds;
		}
		/// <summary>
		/// 查询最新的话题和回复
		/// </summary>
		/// <returns>话题和回复数据集</returns>
		public DataSet GetLatestTopicAndPostsUpdateTime()
		{
			string sql = "SELECT topicId,Max(updateTime) AS updateTime FROM"
+ " ("
+ " SELECT Max(updateTime) AS updateTime,topicId from usta_BbsPosts Group BY topicId"
+ " union "
+ "SELECT Max(updateTime) AS updateTime,topicId from usta_BbsTopics Group BY topicId"
+ " )"
+ " AS A"
+ " Group BY topicId";

			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			return ds;
		}
		/// <summary>
		/// 获取最新课程通知
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <returns>数据集</returns>
		public DataSet GetLatestCourseNotify(string studentNo)
		{
			string sql = "   SELECT [courseNotifyInfoId],[courseNotifyInfoTitle] ,[courseNotifyInfoContent], [updateTime],[publishUserNo],[notifyType] ,[courseName] ,[isTop] FROM [USTA].[dbo].[usta_CoursesNotifyInfo],usta_CoursesStudentsCorrelation,usta_Courses where usta_Courses.courseNo=usta_CoursesNotifyInfo.courseNo AND usta_Courses.termTag=usta_CoursesNotifyInfo.termTag AND usta_Courses.ClassID=usta_CoursesNotifyInfo.classID AND usta_CoursesStudentsCorrelation.courseNo=usta_CoursesNotifyInfo.courseNo AND usta_CoursesStudentsCorrelation.[Year]=usta_CoursesNotifyInfo.termTag AND usta_CoursesStudentsCorrelation.ClassID=usta_CoursesNotifyInfo.classID AND DATEDIFF(DAY,updateTime,GETDATE())<@newDays  AND studentNo=@studentNo";
			SqlParameter[] parameters = {
					new SqlParameter("@studentNo", studentNo),
					new SqlParameter("@newDays", CommonUtility.GetNewDays())
		 };
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
		   
		   
			return ds;
		}
		/// <summary>
		/// 邮件发送次数加一
		/// </summary>
		/// <param name="sendEmailListId">发送队列编号</param>
		public void AddSendEmailTime(int sendEmailListId)
		{
			string sql = "UPDATE [USTA].[dbo].[usta_SendingEmailList] SET [sendTimes] = sendTimes+1 WHERE sendingEmailListId=@sendEmailListId";
			SqlParameter[] parameters = {
					new SqlParameter("@sendEmailListId", sendEmailListId)
		 };
			SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
			conn.Close();
		}

		/// <summary>
		/// 获得某个用户的头像连接
		/// </summary>
		/// <param name="userNo">用户编号</param>
		/// <param name="userType">用户类型</param>
		/// <returns>头像所在文件路径</returns>
		public string GetAvatar(string userNo, int userType)
		{
			string result = string.Empty;
			string sql = "SELECT avatarUrl  FROM [USTA].[dbo].[usta_UserAvatar] WHERE userNo=@userNo and userType=@userType";
			SqlParameter[] parameters = {
					new SqlParameter("@userNo", userNo),
					new SqlParameter("@userType", userType)
		 };
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			if (ds.Tables[0].Rows.Count > 0)
			{
				result = ds.Tables[0].Rows[0]["avatarUrl"].ToString().Trim();
			}
			else
			{
				result = "avatar/1-1.gif";
			}
			return result;
		}
		/// <summary>
		/// 获得所有的头像
		/// </summary>
		/// <returns>头像数据</returns>
		public DataSet GetAllAvatars()
		{
			string sql = "SELECT [avatarId],[avatarDir] FROM [USTA].[dbo].[usta_Avatar]";
			return SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
		}
		/// <summary>
		/// 设置用户的头像
		/// </summary>
		/// <param name="userNo">用户编号</param>
		/// <param name="userType">用户类型</param>
		/// <param name="avatarUrl">头像的文件路径</param>
		public void SetAvatar(string userNo, int userType,string avatarUrl)
		{
			try
			{
				SqlParameter[] parameters = {
					new SqlParameter("@userNo", userNo),
					new SqlParameter("@userType", userType),
					new SqlParameter("@avatarUrl", avatarUrl)
		 };
				string sql = "SELECT [userNo],[userType],[avatarUrl] FROM [USTA].[dbo].[usta_UserAvatar] WHERE userNo=@userNo AND userType=@userType";
				DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
				string sql1 = "";
				if (ds.Tables[0].Rows.Count > 0)
				{
					sql1 += "UPDATE [USTA].[dbo].[usta_UserAvatar] SET [avatarUrl] =@avatarUrl WHERE userNo=@userNo AND userType=@userType";
				}
				else
				{
					sql1 += "INSERT INTO [USTA].[dbo].[usta_UserAvatar] ([userNo],[userType],[avatarUrl]) VALUES(@userNo,@userType,@avatarUrl)";
				}
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql1, parameters);

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
        public void clean()
        {
            List<NewOld> lst = new List<NewOld>();
            lst.Add(new NewOld("09-10-2-1", "G430113301", "1，2", "2009-2"));
            lst.Add(new NewOld("09-10-2-10", "G430113334", "1，2", "2009-2"));
            lst.Add(new NewOld("09-10-2-11", "G430113398", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-12", "G430113398", "2", "2009-2"));
            lst.Add(new NewOld("09-10-2-34", "G430113398", "1，2", "2009-2"));
            lst.Add(new NewOld("09-10-2-13", "G430113451", "1", "2009-2"));

            lst.Add(new NewOld("09-10-2-14", "G430113446", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-15", "G430113464", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-16", "G430113454", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-17", "G430113448", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-18", "G430113450", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-19", "G430113460", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-2", "G430113445", "1,2", "2009-2"));
            lst.Add(new NewOld("09-10-2-3", "G430113445", "苏州3班", "2009-2"));
            lst.Add(new NewOld("09-10-2-20", "G430113423", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-21", "G430113456", "1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-22", "G430113411", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-23", "G430113435", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-24", "G430113457", "1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-25", "G430113459", "1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-26", "G430113463", "1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-27", "G430113425", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-28", "G430113452", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-29", "G430113453", "1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-30", "G430113462", "1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-31", "G430113455", "1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-32", "G430113335", "1", "2009-2"));
            lst.Add(new NewOld("09-10-2-38", "G430113413", "1,2", "2009-2"));
            lst.Add(new NewOld("09-10-2-5", "G430113412", "苏州1班", "2009-2"));
            lst.Add(new NewOld("09-10-2-6", "G430113412", "苏州2班", "2009-2"));
            lst.Add(new NewOld("09-10-2-7", "G430113412", "苏州3班", "2009-2"));
            lst.Add(new NewOld("09-10-2-7", "G430113458", "1班", "2009-2"));
            using( TransactionScope scope = new TransactionScope()){
            
                 try
                {
                     foreach ( NewOld nocs in lst)
                    {
               
                    SqlParameter[] parameters = {
					new SqlParameter("@oldCourseNo", nocs.oldcourseNo),
					new SqlParameter("@newCourseNo", nocs.newcourseNo),
					new SqlParameter("@termtag", nocs.termtag),
                    new SqlParameter("@classId", nocs.classId)
		            };
                    string sql = "update usta_Courses set period=(select top 1  period from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
                        sql+="update usta_Courses set credit=(select top 1  credit from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set courseSpeciality=(select top 1  courseSpeciality from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set preCourse=(select top 1  preCourse from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set refferenceBooks=(select top 1  refferenceBooks from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set homePage=(select top 1  homePage from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set courseAnswer=(select top 1  courseAnswer from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set teacherResume=(select top 1  teacherResume from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set courseIntroduction=(select top 1  courseIntroduction from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set examineMethod=(select top 1  examineMethod from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set lessonTimeAndAddress=(select top 1  lessonTimeAndAddress from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set teachingPlan=(select top 1  teachingPlan from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";
sql+="update usta_Courses set bbsEmailAddress=(select top 1  bbsEmailAddress from usta_Courses where courseNo=@oldCourseNo) where courseNo=@newCourseNo and termTag=@termtag and ClassID=@classId;";

sql+="update usta_CourseResources set courseNo=@newCourseNo,classID=@classId,termTag=@termtag where courseNo=@oldCourseNo;";
sql+="update usta_CoursesNotifyInfo set courseNo=@newCourseNo,classID=@classId,termTag=@termtag where courseNo=@oldCourseNo;";
sql+="update usta_ExperimentResources set courseNo=@newCourseNo,classID=@classId,termTag=@termtag where courseNo=@oldCourseNo;";
sql+="update usta_SchoolWorkNotify set courseNo=@newCourseNo,classID=@classId,termTag=@termtag where courseNo=@oldCourseNo;";
                    sql+="update usta_BbsTopics set courseNo=(@newCourseNo+@classId+@termtag) where courseNo=@oldCourseNo;";
                    sql += "update usta_BbsPosts set courseNo=(@newCourseNo+@classId+@termtag) where courseNo=@oldCourseNo;";

                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
                         }
                     scope.Complete();
                 }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    CommonUtility.RedirectUrl();
                }
                    
               
              
            }
            
        }
		#endregion
	}
}
