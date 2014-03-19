using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using USTA.Model;
using USTA.Common;

namespace USTA.Dal
{
	/// <summary>
	/// 作业操作类
	/// </summary>
	public class DalOperationAboutSchoolWorks
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
		public DalOperationAboutSchoolWorks()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		/// <summary>
		/// 更新一条作业提交
		/// </summary>
		/// <param name="schoolwork">提交的作业实体</param>
		public void SubmitSchoolWork(SchoolWorks schoolwork)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_SchoolWorks] SET attachmentId=@attachmentId,remark=@remark,updateTime=@updateTime,isSubmit=1 WHERE schoolWorkNofityId=@schoolWorkNofityId AND studentNo=@studentNo";

				SqlParameter[] parameters = {
					
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@schoolWorkNofityId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					
				
					new SqlParameter("@attachmentId", SqlDbType.NVarChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500)
					};

				parameters[0].Value = schoolwork.updateTime;
				parameters[1].Value = schoolwork.schoolWorkNofityId;
				parameters[2].Value = schoolwork.studentNo;

				parameters[3].Value = schoolwork.attachmentId;
				parameters[4].Value = schoolwork.remark;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 插入一条作业提交
		/// </summary>
		/// <param name="schoolwork">提交作业的实体</param>
		public void AddSchoolWork(SchoolWorks schoolwork)
		{
			try
			{
				string cmdstring = "INSERT INTO [USTA].[dbo].[usta_SchoolWorks] ([schoolWorkNofityId],[studentNo] ,[updateTime] ,[attachmentId],[remark]) VALUES ( @schoolWorkNofityId,@studentNo,@updateTime,@attachmentId,@remark)";

				SqlParameter[] parameters = {
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@schoolWorkNofityId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@attachmentId", SqlDbType.Int,4),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					};
				parameters[0].Value = schoolwork.updateTime;
				parameters[1].Value = schoolwork.schoolWorkNofityId;
				parameters[2].Value = schoolwork.studentNo;
				parameters[3].Value = schoolwork.attachmentId;
				parameters[4].Value = schoolwork.remark;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 获得某次作业提交的列表
		/// </summary>
		/// <param name="schoolWorkNofityId">提交的作业编号</param>
		/// <returns>提交的作业数据集</returns>
		public DataSet FindAllSchoolWorksByschoolWorkNofityId(int schoolWorkNofityId)
		{
            string cmdstring = "SELECT [schoolWorkId],[schoolWorkNofityId],[usta_SchoolWorks].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName],[excellentTime],[isExcellent],score,[USTA].[dbo].[usta_SchoolWorks].remark FROM [USTA].[dbo].[usta_SchoolWorks],usta_StudentsList WHERE schoolWorkNofityId=@schoolWorkNofityId AND usta_StudentsList.studentNo=usta_SchoolWorks.studentNo";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@schoolWorkNofityId",schoolWorkNofityId)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 获得某次作业提交的列表
		/// </summary>
		/// <param name="schoolWorkNofityId">提交的作业编号</param>
		/// <returns></returns>
		public DataSet FindSchoolWorksByschoolWorkNofityId(int schoolWorkNofityId,string studentName,float low,float high)
		{

            string cmdstring = "SELECT [schoolWorkId],[schoolWorkNofityId],[usta_SchoolWorks].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName],[excellentTime],[isExcellent],score,usta_SchoolWorks.remark,returnAttachmentId FROM [USTA].[dbo].[usta_SchoolWorks],usta_StudentsList WHERE schoolWorkNofityId=@schoolWorkNofityId AND usta_StudentsList.studentNo=usta_SchoolWorks.studentNo AND usta_SchoolWorks.isSubmit=1 AND studentName like @studentName AND cast( score   as   float) between @low and @high ORDER BY [updateTime] ASC, isCheck ASC";
			SqlParameter[] parameters = new SqlParameter[4]{
				new SqlParameter("@schoolWorkNofityId",schoolWorkNofityId),
                new SqlParameter("@studentName","%"+studentName+"%"),
                new SqlParameter("@low",low),
                new SqlParameter("@high",high)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
        /// <summary>
        /// 获得某次作业提交的列表
        /// </summary>
        /// <param name="schoolWorkNofityId">提交的作业编号</param>
        /// <returns></returns>
        public DataSet FindSchoolWorksByschoolWorkNofityId(int schoolWorkNofityId, string studentName)
        {

            string cmdstring = "SELECT [schoolWorkId],[schoolWorkNofityId],[usta_SchoolWorks].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName],[excellentTime],[isExcellent],score,usta_SchoolWorks.remark,returnAttachmentId FROM [USTA].[dbo].[usta_SchoolWorks],usta_StudentsList WHERE schoolWorkNofityId=@schoolWorkNofityId AND usta_StudentsList.studentNo=usta_SchoolWorks.studentNo AND usta_SchoolWorks.isSubmit=1 AND studentName like @studentName  ORDER BY [updateTime] ASC, isCheck ASC";
            SqlParameter[] parameters = {
				new SqlParameter("@schoolWorkNofityId",schoolWorkNofityId),
                new SqlParameter("@studentName","%"+studentName+"%")
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
            conn.Close();
            return ds;
        }
		/// <summary>
		/// 查询提交课程作业及提交的信息
		/// </summary>
		/// <param name="courseNo">课程编号</param>
		/// <returns>提交的作业数据集</returns>
		public DataSet FindSchoolWorksByCourseNo(string courseNo,string classId,string termtag)
		{
			string commandString = "SELECT studentNo,schoolWorkNotifyTitle,remark,score ";
			commandString += "FROM  usta_SchoolWorkNotify,usta_SchoolWorks ";
			commandString += "WHERE usta_SchoolWorkNotify.schoolWorkNotifyId=usta_SchoolWorks.schoolWorkNofityId ";
			commandString += "AND courseNo=@courseNo AND classID=@classId AND termTag = @termtag ";
			SqlParameter[] parameters = new SqlParameter[3]{
                new SqlParameter("@courseNo",courseNo),
                new SqlParameter("@classId",classId),
                new SqlParameter("@termtag",termtag)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 课程作业的信息
		/// </summary>
		/// <param name="courseNo">课程编号</param>
		/// <returns>课程作业数据集</returns>
		public DataSet GetSchoolWorksByCourseNo(string courseNo,string classId,string termtag)
		{
			string commandString = "select schoolWorkNotifyId,schoolWorkNotifyTitle,schoolWorkNotifyContent,courseNo from dbo.usta_SchoolWorkNotify WHERE courseNo=@courseNo AND classID=@classId AND termTag=@termtag order by schoolWorkNotifyId asc";
			SqlParameter[] parameters = new SqlParameter[3]{new SqlParameter("@courseNo",courseNo),
                new SqlParameter("@classId",classId),
                new SqlParameter("@termtag",termtag)

			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 获得某次作业未提交的列表
		/// </summary>
		/// <param name="schoolWorkNofityId">未提交的作业编号</param>
		/// <returns>未提交作业数据集</returns>
		public DataSet FindNoSchoolWorksByschoolWorkNofityId(int schoolWorkNofityId ,string studentName)
		{
            string sql = "SELECT [schoolWorkId],[schoolWorkNofityId],[usta_SchoolWorks].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName],[excellentTime],[isExcellent] FROM [USTA].[dbo].[usta_SchoolWorks],usta_StudentsList WHERE schoolWorkNofityId=@schoolWorkNofityId AND usta_StudentsList.studentNo=usta_SchoolWorks.studentNo AND usta_SchoolWorks.isSubmit=0 AND usta_StudentsList.studentName like @studentName order by isCheck asc";
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@schoolWorkNofityId",schoolWorkNofityId),
                new SqlParameter("@studentName","%" + studentName + "%")
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 通过学生和实验主键获得提交的作业
		/// </summary>
		/// <param name="schoolWorkNofityId">提交的作业编号</param>
		/// <param name="studentNo">学号</param>
		/// <returns>提交的作业数据集</returns>
		public DataSet FindSchoolWorksByschoolWorkNofityIdAndStudent(int schoolWorkNofityId, string studentNo)
		{
			string cmdstring = "SELECT [schoolWorkId],[schoolWorkNofityId],[usta_SchoolWorks].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName]  FROM [USTA].[dbo].[usta_SchoolWorks],usta_StudentsList WHERE schoolWorkNofityId=@schoolWorkNofityId AND usta_StudentsList.studentNo=usta_SchoolWorks.studentNo AND usta_StudentsList.studentNo=@studentNo";
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@schoolWorkNofityId",schoolWorkNofityId),
				new SqlParameter("@studentNo",studentNo)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 新提交的作业
		/// </summary>
		/// <param name="schoolwork">提交的作业实体</param>
		public void UpdateSchoolWork(SchoolWorks schoolwork)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_SchoolWorks] SET [schoolWorkNofityId] = @schoolWorkNofityId,[studentNo] = @studentNo,[isCheck] = @isCheck,[updateTime] = @updateTime,[checkTime] = @checkTime,[attachmentId] = @attachmentId WHERE schoolWorkId=@schoolWorkId";

				SqlParameter[] parameters = {
					new SqlParameter("@schoolWorkId", SqlDbType.Int,4),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@schoolWorkNofityId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@isCheck", SqlDbType.Bit,1),
					new SqlParameter("@checkTime", SqlDbType.DateTime),
					new SqlParameter("@attachmentId", SqlDbType.NVarChar,50)
					};
				parameters[0].Value = schoolwork.schoolWorkId;
				parameters[1].Value = schoolwork.updateTime;
				parameters[2].Value = schoolwork.schoolWorkNofityId;
				parameters[3].Value = schoolwork.studentNo;
				parameters[4].Value = schoolwork.isCheck;
				parameters[5].Value = schoolwork.checkTime;
				parameters[6].Value = schoolwork.attachmentId;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 删除布置的课程作业
		/// </summary>
		/// <param name="schoolworkNotifyId">作业主键</param>
		public void DeleteSchoolworkNotify(int schoolworkNotifyId)
		{
           
			using (TransactionScope scope = new TransactionScope())
			{
				try
				{
					string cmdstring = "delete from usta_SchoolWorkNotify where schoolWorkNotifyId=@schoolworkNotifyId;";
					cmdstring += "delete from usta_SchoolWorks where schoolWorkNofityId=@schoolworkNotifyId;";

					SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@schoolworkNotifyId",schoolworkNotifyId)
				};
					SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
                    scope.Complete();
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

		/// <summary>
		/// 获取学生提交的作业信息
		/// </summary>
		/// <param name="schoolWorkId">提交的作业编号</param>
		/// <returns>提交的作业数据集</returns>
		public DataSet FindSchoolWorkSchoolWorkIdForCheck(int schoolWorkId)
		{
			string cmdstring = "select usta_SchoolWorks.studentNo,studentName,usta_SchoolWorks.remark,usta_SchoolWorks.score,usta_SchoolWorks.returnAttachmentId ";
			cmdstring += "from usta_SchoolWorks,usta_StudentsList ";
			cmdstring += "where usta_SchoolWorks.studentNo=usta_StudentsList.studentNo ";
			cmdstring += "AND schoolWorkId=@schoolWorkId";

			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@schoolWorkId",schoolWorkId)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 批阅学生作业
		/// </summary>
		/// <param name="schoolWorkId">提交的作业编号</param>
		/// <param name="remark">评语</param>
		/// <param name="score">分数</param>
		/// <param name="attachmentId">附件编号</param>
		public void CheckSchoolWorkByschoolWorkId(int schoolWorkId, string remark, string score, int attachmentId)
		{
			try
			{
				string cmdstring = string.Empty;

				if (attachmentId != 0)
				{
					cmdstring = "UPDATE [USTA].[dbo].[usta_SchoolWorks] SET isCheck= @isCheck,remark=@remark,score=@score,isSubmit='1',returnAttachmentId=@returnId WHERE schoolWorkId=@schoolWorkId";
				}
				else
				{
					cmdstring = "UPDATE [USTA].[dbo].[usta_SchoolWorks] SET isCheck= @isCheck,remark=@remark,score=@score,isSubmit='1' WHERE schoolWorkId=@schoolWorkId";
				}


				SqlParameter[] parameters =  {
			   new SqlParameter("@schoolWorkId",SqlDbType.Int,4),
			   new SqlParameter("@remark",SqlDbType.NVarChar,500),
			   new SqlParameter("@score",SqlDbType.NChar,10),
			   new SqlParameter("@isCheck",SqlDbType.Bit,1),
			   new SqlParameter("@returnId",SqlDbType.Int,4)
			 };

				parameters[0].Value = schoolWorkId;
				parameters[1].Value = remark;
				parameters[2].Value = score;
				parameters[3].Value = "true";
				parameters[4].Value = attachmentId;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 修改状态:是否为优秀作业
		/// </summary>
		/// <param name="schoolWorkNofityId">作业编号</param>
		/// <param name="studentNo">学号</param>
		/// <param name="excellent">是否优秀</param>
		public void UpdateSchoolWorkByschoolWorkNofityIdAndStudentNo(int schoolWorkNofityId, string studentNo, int excellent)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_SchoolWorks] SET isExcellent= @isExcellent,excellentTime=@excellentTime WHERE [studentNo] = @studentNo AND schoolWorkNofityId=@schoolWorkNofityId";

				SqlParameter[] parameters = new SqlParameter[4]{
			   new SqlParameter("@schoolWorkNofityId",SqlDbType.Int,4),
			   new SqlParameter("@studentNo",SqlDbType.NChar,10),
			   new SqlParameter("@isExcellent",SqlDbType.Bit,1),
			   new SqlParameter("@excellentTime",SqlDbType.DateTime)
			 };
				parameters[0].Value = schoolWorkNofityId;
				parameters[1].Value = studentNo;
				parameters[2].Value = excellent;
				parameters[3].Value = DateTime.Now;
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 判断是否提交作业
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <param name="schoolworkNotifyId">作业编号</param>
		/// <param name="id">作业编号</param>
		/// <param name="check">是否批阅</param>
		/// <returns>是否提交</returns>
		public bool SchoolworkIsCommit(string studentNo, int schoolworkNotifyId, ref int id, ref bool check)
		{
			bool isSubmit = false;
			string sql = "SELECT [schoolWorkId] ,[updateTime] ,[schoolWorkNofityId] ,[studentNo] ,[isCheck] ,[checkTime] ,[attachmentId] ,[remark],[excellentTime],[isExcellent],[isSubmit] FROM [USTA].[dbo].[usta_SchoolWorks] WHERE schoolWorkNofityId=@schoolWorkNofityId AND studentNo=@studentNo";
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@schoolWorkNofityId",schoolworkNotifyId),
				new SqlParameter("@studentNo",studentNo)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
			if (dr.Read())
			{
				id = int.Parse(dr["schoolWorkId"].ToString());
				check = bool.Parse(dr["isCheck"].ToString());
				isSubmit = bool.Parse(dr["isSubmit"].ToString());
				dr.Close();
				conn.Close();
			}
			dr.Close();
			conn.Close();
			return isSubmit;
		}
		/// <summary>
		/// 获得某个作业的优秀作业
		/// </summary>
		/// <param name="schoolworkNotifyId">作业编号</param>
		/// <returns>优秀作业数据集</returns>
		public DataSet ExcelentWorks(int schoolworkNotifyId)
		{
			string sql = "SELECT [schoolWorkId],[attachmentId], [studentName] FROM [USTA].[dbo].[usta_SchoolWorks] ,[USTA].[dbo].usta_StudentsList WHERE schoolWorkNofityId=@schoolWorkNofityId AND isExcellent='true' AND usta_SchoolWorks.studentNo=usta_StudentsList.studentNo";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@schoolWorkNofityId",schoolworkNotifyId)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 由主键获得提交的作业
		/// </summary>
		/// <param name="schoolWorkId">作业编号</param>
		/// <returns>作业实体</returns>
		public SchoolWorks GetSchoolWorkById(int schoolWorkId)
		{
			SchoolWorks schoolWorks = null;
			string sql = "SELECT [schoolWorkId] ,[updateTime] ,[schoolWorkNofityId] ,[studentNo] ,[isCheck] ,[checkTime] ,[attachmentId] ,[remark],[excellentTime],[isExcellent] FROM [USTA].[dbo].[usta_SchoolWorks]  WHERE schoolWorkId=@schoolWorkId";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@schoolWorkId",schoolWorkId)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
			if (dr.Read())
			{
				schoolWorks = new SchoolWorks
				{
					schoolWorkId = int.Parse(dr["schoolWorkId"].ToString()),
					updateTime = Convert.ToDateTime(dr["updateTime"].ToString()),
					schoolWorkNofityId = int.Parse(dr["schoolWorkNofityId"].ToString()),
					studentNo = dr["studentNo"].ToString(),
					isCheck = bool.Parse(dr["isCheck"].ToString()),
					attachmentId = dr["attachmentId"].ToString().Trim(),

					isExcellent = bool.Parse(dr["isExcellent"].ToString()),
					remark = dr["remark"].ToString()
				};
				if (dr["checkTime"].ToString() != "") schoolWorks.checkTime = Convert.ToDateTime(dr["checkTime"].ToString());
				if (dr["excellentTime"].ToString() != "") schoolWorks.excellentTime = Convert.ToDateTime(dr["excellentTime"].ToString());
			}
			conn.Close();
			return schoolWorks;
		}

		/// <summary>
		/// 获得学生的课程作业
		/// </summary>
		/// <param name="studentNo">学号</param>
        /// <param name="courseNoTermTagClassID">课程学期班级集成编号</param>
		/// <returns>学生作业数据集</returns>
		public DataSet GetSchoolWorksByStudentNo(string studentNo, string courseNoTermTagClassID)
		{
            string sql = "SELECT courseNo,[schoolWorkId],[attachmentId],usta_SchoolWorkNotify.schoolWorkNotifyId,schoolWorkNotifyTitle,deadline,isCheck,isExcellent,isSubmit,score,remark,isOnline,returnAttachmentId,classID,termTag FROM [USTA].[dbo].[usta_SchoolWorks],usta_SchoolWorkNotify  WHERE  usta_SchoolWorks.studentNo=@studentNo AND usta_SchoolWorkNotify.schoolWorkNotifyId=usta_SchoolWorks.schoolWorkNofityId AND (RTRIM(courseNo)+RTRIM(termTag)+RTRIM(classID)=@courseNoTermTagClassID) ORDER BY schoolWorkNotifyId DESC;";
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@studentNo",studentNo),
				new SqlParameter("@courseNoTermTagClassID",courseNoTermTagClassID)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}

	}
}
