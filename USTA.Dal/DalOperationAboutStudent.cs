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
using USTA.Common;

namespace USTA.Dal
{
	using USTA.Model;

	/// <summary>
	/// 学生操作类
	/// </summary>
	public class DalOperationAboutStudent
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
		public DalOperationAboutStudent()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

			termTag = DalCommon.GetTermTag(conn);
		}
		#endregion

		#region
		/// <summary>
		/// 通过学号获得这个学生的这学期的所有选课
		/// </summary>
		/// <param name="StudentNo">学号</param>
		/// <returns>学生选课信息</returns>
		public DataSet GetCoursesByStudentNo(string StudentNo)
		{
            string commandstring = "SELECT coursesStudentsCorrelationId,usta_Courses.courseNo,usta_Courses.ClassID,usta_Courses.termTag,courseName FROM usta_CoursesStudentsCorrelation,usta_Courses WHERE usta_CoursesStudentsCorrelation.courseNo=usta_Courses.courseNo AND usta_CoursesStudentsCorrelation.ClassID=usta_Courses.ClassID  AND  usta_CoursesStudentsCorrelation.studentNo=@studentNo AND termTag=Year AND termTag=@termTag";
			SqlParameter[] parameters ={
			   new SqlParameter("@studentNo", SqlDbType.NChar,10),
			   new SqlParameter("@termTag", SqlDbType.NVarChar,50) 
			};
			parameters[0].Value = StudentNo;
			parameters[1].Value = termTag;
			DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return dr;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StudentNo"></param>
        /// <returns></returns>
        public DataSet GetallCourseByStudentNo(string StudentNo)
        {
            string commandstring = "SELECT coursesStudentsCorrelationId,usta_Courses.courseNo,usta_Courses.ClassID,usta_Courses.termTag,courseName FROM usta_CoursesStudentsCorrelation,usta_Courses WHERE usta_CoursesStudentsCorrelation.courseNo=usta_Courses.courseNo AND usta_CoursesStudentsCorrelation.ClassID=usta_Courses.ClassID  AND  usta_CoursesStudentsCorrelation.studentNo=@studentNo AND termTag=Year ORDER BY termTag DESC;";
            SqlParameter[] parameters ={
			   new SqlParameter("@studentNo", SqlDbType.NChar,10)
			};
            parameters[0].Value = StudentNo;

            DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return dr;
        }

		/// <summary>
		/// 通过学号获得这个学生的某学期期的所有选课
		/// </summary>
		/// <param name="StudentNo">学号</param>
		/// <param name="termTags">学期标识</param>
		/// <returns>学生选课信息</returns>
		public DataSet GetCoursesByStudentNo(string StudentNo, string termTags)
		{
            string commandstring = "SELECT usta_Courses.courseNo,courseName,[usta_Courses].ClassID,termTag FROM usta_CoursesStudentsCorrelation,usta_Courses WHERE usta_CoursesStudentsCorrelation.courseNo=usta_Courses.courseNo AND usta_CoursesStudentsCorrelation.ClassID=usta_Courses.ClassID  AND  usta_CoursesStudentsCorrelation.studentNo=@studentNo AND termTag=@termTag AND termTag=Year";
			SqlParameter[] parameters ={
			   new SqlParameter("@studentNo", SqlDbType.NChar,10),
			   new SqlParameter("@termTag", SqlDbType.NVarChar,50) 
			};
			parameters[0].Value = StudentNo;
			parameters[1].Value = termTags;

			DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return dr;
		}

		/// <summary>
		/// 通过学号获得这个学生的这学期的其他课程
		/// </summary>
		/// <param name="StudentNo">学号</param>
		/// <returns>学生未选课程数据集</returns>
		public DataSet GetOtherCoursesByStudentNo(string StudentNo)
		{
			DalCommon dalCommon = new DalCommon();


            string commandstring = "SELECT courseNo,courseName,classID,termTag FROM usta_Courses where (courseNo+termTag+classID) NOT IN ( SELECT (courseNo+termTag+classID) FROM usta_CoursesStudentsCorrelation WHERE studentNo=@studentNo) AND termTag=@termTag";
			SqlParameter[] parameters ={
			   new SqlParameter("@studentNo", SqlDbType.NChar,10),
			   new SqlParameter("@termTag", SqlDbType.NVarChar,50) 
			};
			parameters[0].Value = StudentNo;
			parameters[1].Value = termTag;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 通过学号获得这个学生的这学期的其他课程
		/// </summary>
		/// <param name="StudentNo">学号</param>
		/// <param name="termTags">学期标识</param>
		/// <returns>学生未选课程数据集</returns>
		public DataSet GetOtherCoursesByStudentNo(string StudentNo, string termTags)
		{
			DalCommon dalCommon = new DalCommon();


            string commandstring = "SELECT courseNo,courseName,classID,termTag FROM usta_Courses where (courseNo+termTag+classID) NOT IN ( SELECT (courseNo+termTag+classID) FROM usta_CoursesStudentsCorrelation WHERE studentNo=@studentNo) AND termTag=@termTag";
			SqlParameter[] parameters ={
			   new SqlParameter("@studentNo", SqlDbType.NChar,10),
			   new SqlParameter("@termTag", SqlDbType.NVarChar,50) 
			};
			parameters[0].Value = StudentNo;
			parameters[1].Value = termTags;

			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 通过专业获得所有学生，专业字段有待进一步确定
		/// </summary>
		/// <param name="subject">专业</param>
		/// <returns>某专业课程数据集 </returns>
		public DataSet GetStudentBySubject(string subject)
		{
			string cmdstring = "SELECT [studentNo],[studentName] FROM [USTA].[dbo].[usta_StudentsList] WHERE studentSpeciality=@studentSpeciality";

			SqlParameter[] parameters ={
			  new SqlParameter("@studentSpeciality", SqlDbType.NChar,15)
			};
			parameters[0].Value = subject;
			DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return dr;
		}

		/// <summary>
		/// 通过学号获得学生
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <returns>学生实体</returns>
		public StudentsList GetStudentById(string studentNo)
		{
			StudentsList student = null;
            string cmdstring = "SELECT [studentNo] ,[studentName] ,[studentSpeciality],[mobileNo],[emailAddress] ,[remark],SchoolClassName  FROM [USTA].[dbo].[usta_StudentsList] WHERE studentNo=@studentNo";
			SqlParameter[] parameters ={
			   new SqlParameter("@studentNo", SqlDbType.NChar,10),
			};
			parameters[0].Value = studentNo;

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
			if (dr.Read())
			{
				student = new StudentsList
				{
					studentNo = dr["studentNo"].ToString().Trim(),
					studentName = dr["studentName"].ToString().Trim(),
					studentSpeciality = dr["studentSpeciality"].ToString().Trim(),
					mobileNo = dr["mobileNo"].ToString().Trim(),
					emailAddress = dr["emailAddress"].ToString().Trim(),
					remark = dr["remark"].ToString().Trim(),
                    SchoolClassName = dr["SchoolClassName"].ToString().Trim()
				};
			}
			return student;
		}
		/// <summary>
		/// 更新学生信息
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <param name="email">email</param>
		/// <param name="mobileNo">联系电话</param>
		/// <param name="remark">备注</param>
		public void UpdateStudent(string studentNo, string email, string mobileNo,string remark)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_StudentsList] SET [mobileNo] = @mobileNo,[emailAddress] = @emailAddress,[remark]=@remark WHERE studentNo=@studentNo";

				SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),	
					new SqlParameter("@mobileNo", SqlDbType.NChar,50),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NChar,500)
											};
				parameters[0].Value = studentNo;
				parameters[1].Value = mobileNo;
				parameters[2].Value = email;
				parameters[3].Value = remark;

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
		/// 删除选课
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <param name="courseNo">课程编号</param>
		public void DelChooseCourse(string studentNo, string courseNo)
		{
			try
			{
				string sql = "DELETE FROM [USTA].[dbo].[usta_CoursesStudentsCorrelation] WHERE courseNo=@courseNo AND studentNo=@studentNo";
				SqlParameter[] parameters = {
				new SqlParameter("@courseNo", SqlDbType.NChar,20),
				new SqlParameter("@studentNo", SqlDbType.NChar,10)
				
			};
				parameters[0].Value = courseNo;
				parameters[1].Value = studentNo;

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
        /// 删除选课
        /// </summary>
        /// <param name="studentNo">学号</param>
        /// <param name="courseNo">课程编号</param>
        public void DelChooseCourseByCoursesStudentsCorrelationId(string coursesStudentsCorrelationId)
        {
            try
            {
                string sql = "DELETE FROM [USTA].[dbo].[usta_CoursesStudentsCorrelation] WHERE coursesStudentsCorrelationId=@coursesStudentsCorrelationId ";
                SqlParameter[] parameters = {
				new SqlParameter("@coursesStudentsCorrelationId", SqlDbType.Int)
				
			};
                parameters[0].Value = coursesStudentsCorrelationId;

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
		/// 添加选课
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <param name="courseNo">课程编号</param>
        public void AddChooseCourse(string studentNo, string courseNo, string termTag, string ClassID)
		{
			try
			{
                string sql = "INSERT INTO [USTA].[dbo].[usta_CoursesStudentsCorrelation] ([studentNo],[courseNo],[Year],[ClassID]) VALUES(@studentNo ,@courseNo,@Year,@ClassID)";
				SqlParameter[] parameters = {
				new SqlParameter("@courseNo", SqlDbType.NChar,20),
				new SqlParameter("@studentNo", SqlDbType.NChar,10),
                new SqlParameter("@Year", SqlDbType.NVarChar,50),
                new SqlParameter("@ClassID", SqlDbType.NVarChar,50)
				
			};
				parameters[0].Value = courseNo;
				parameters[1].Value = studentNo;
                parameters[2].Value = termTag;
                parameters[3].Value = ClassID;
				using (TransactionScope scope = new TransactionScope())
				{
					SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
					UpdateSchoolWordAndExperiment(studentNo, courseNo);
					scope.Complete();
				}

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
	   /// 更新学生某个课程的实验和作业，一般在中途给某学生添加课程的时候调用
	   /// </summary>
	   /// <param name="studentNo">学号</param>
	   /// <param name="courseNo">课程编号</param>
		protected void UpdateSchoolWordAndExperiment(string studentNo, string courseNo)
		{
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@courseNo",courseNo),
				new SqlParameter("@studentNo",studentNo)
				};
			DataSet dsExperiment = SqlHelper.ExecuteDataset(conn, CommandType.Text, "select * from usta_ExperimentResources where courseNo=@courseNo", parameters);
			DataSet dsSchoolwork = SqlHelper.ExecuteDataset(conn, CommandType.Text, "select * from usta_SchoolWorkNotify where courseNo=@courseNo", parameters);
			for (int i = 0; i < dsExperiment.Tables[0].Rows.Count; i++)
			{
				string experimentResourceId = dsExperiment.Tables[0].Rows[i]["experimentResourceId"].ToString();
				SqlParameter[] parameters1 = new SqlParameter[3]{
				new SqlParameter("@experimentResourceId",experimentResourceId),
				new SqlParameter("@studentNo",studentNo),
				new SqlParameter("@attachmentId","0")
				};

				DataSet dsforCheck = SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT [experimentId] FROM [USTA].[dbo].[usta_Experiments] WHERE studentNo=@studentNo AND experimentResourceId=@experimentResourceId", parameters1);
				if (dsforCheck.Tables[0].Rows.Count == 0)
				{
					SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "INSERT INTO [USTA].[dbo].[usta_Experiments] ([experimentResourceId],[studentNo],attachmentId)  VALUES (@experimentResourceId ,@studentNo,@attachmentId)", parameters1);
				}
			}
			for (int i = 0; i < dsSchoolwork.Tables[0].Rows.Count; i++)
			{
				string schoolworkNotifyId = dsSchoolwork.Tables[0].Rows[i]["schoolWorkNotifyId"].ToString();
				SqlParameter[] parameters1 = new SqlParameter[3]{
				new SqlParameter("@schoolWorkNofityId",schoolworkNotifyId),
				new SqlParameter("@studentNo",studentNo),
				new SqlParameter("@attachmentId","0")
				};

				DataSet dsforCheck = SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT schoolWorkNofityId FROM [USTA].[dbo].[usta_SchoolWorks] WHERE studentNo=@studentNo AND schoolWorkNofityId=@schoolWorkNofityId", parameters1);
				if (dsforCheck.Tables[0].Rows.Count == 0)
				{
					SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "INSERT INTO [USTA].[dbo].[usta_SchoolWorks] ([schoolWorkNofityId],[studentNo],attachmentId)  VALUES (@schoolWorkNofityId ,@studentNo,@attachmentId)", parameters1);
				}
			}
		}

        public string GetTeacherNoByStudent(string studentNo)
        {

            string sql = "select t.teacherNo,t.teacherName from usta_TeachersList t,usta_StudentClass c,usta_StudentsList s where t.TeacherID=c.Headteacher and c.SchoolClassID = s.SchoolClass and s.studentNo=@studentNo";
            SqlParameter[] parameters = {
                new SqlParameter("@studentNo",studentNo)};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
            string result = "";
            if (dr.Read())
            {
                result = dr["teacherNo"].ToString().Trim();
            }
            dr.Close();
            conn.Close();
            return result;
        }
		#endregion
	}
}
