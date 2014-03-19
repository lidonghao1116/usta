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
	using System.Data.OleDb;//批量导入连接Excel

	/// <summary>
	/// 用户操作类
	/// </summary>
	public class  DalOperationUsers
	{
		#region 全局变量及构造函数 
		/// <summary>
		/// 数据库连接属性
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
		public DalOperationUsers()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

            termTag = DalCommon.GetTermTag(conn);
		}
		#endregion

		#region
        public string findStudentNobyId(string ID)
        {
            string sql = "SELECT studentNo FROM usta_StudentsList WHERE StudentID=@ID";
            SqlParameter[] parameters ={
				new SqlParameter("@ID", ID)
			};
            
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
            String result = string.Empty;
            while (dr.Read())
            {
                result = dr["studentNo"].ToString();
            }
            return result;
        } 
		/// <summary>
		/// 查询管理员信息
		/// </summary>
		/// <param name="adminNo">管理员编号</param>
		/// <returns>管理员信息</returns>
		public AdminList FindAdminByNo(string adminNo)
		{
			AdminList admin = null;
			string sql = "SELECT adminUserName,adminName FROM usta_AdminList WHERE adminUserName=@adminUserName";
			SqlParameter[] parameters ={
				new SqlParameter("@adminUserName", SqlDbType.NChar,10)
			};
			parameters[0].Value = adminNo;
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
			while (dr.Read())
			{
				admin = new AdminList();               
				admin.adminUserName = dr[0].ToString();
				admin.adminName = dr[1].ToString();               
			}
			dr.Close();
			conn.Close();
			return admin;
		}

		
		/// <summary>
		/// 更新管理员信息
		/// </summary>
		/// <param name="admin">管理员信息</param>
		public void UpdateAdminByAdminList(AdminList admin)
		{
			try
			{
				string sql = "UPDATE usta_AdminList SET adminUserPwd=@adminUserPwd,adminName=@adminName WHERE adminUserName=@adminUserName";
				SqlParameter[] parameters = {
					new SqlParameter("@adminUserName", SqlDbType.NChar,10),
					new SqlParameter("@adminUserPwd", SqlDbType.NChar,32),
					new SqlParameter("@adminName", SqlDbType.NChar,10)};
				parameters[0].Value = admin.adminUserName;
				parameters[1].Value = admin.adminUserPwd;
				parameters[2].Value = admin.adminName;

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
		/// 增加学生信息
		/// </summary>
		/// <param name="student">学生信息</param>
		public void AddStudent(StudentsList student)
		{
			try
			{
				string sql = "INSERT INTO usta_StudentsList VALUES(@studentNo,@studentName ,@studentUserPwd ,@studentSpeciality,@mobileNo, @emailAddress,@remark,@classNo)";
				 SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@studentName", SqlDbType.NChar,10),
					new SqlParameter("@studentUserPwd", SqlDbType.NChar,32),
					new SqlParameter("@studentSpeciality", SqlDbType.NChar,15),
					new SqlParameter("@mobileNo", SqlDbType.NChar,50),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					new SqlParameter("@classNo", SqlDbType.NChar,5)};
			parameters[0].Value = student.studentNo;
			parameters[1].Value = student.studentName;
			parameters[2].Value =CommonUtility.EncodeUsingMD5(student.studentUserPwd);
			parameters[3].Value = student.studentSpeciality;
			parameters[4].Value = student.mobileNo;
			parameters[5].Value = student.emailAddress;
			parameters[6].Value = student.remark;
			parameters[7].Value = student.classNo;
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
		/// 查询所有学生信息
		/// </summary>
		/// <returns>学生信息数据集</returns>
		public DataSet FindAllStudent()
		{
			DataSet ds = null;
			string sql = "SELECT studentNo, studentName,studentSpeciality,mobileNo,emailAddress,remark FROM usta_StudentsList";
			ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			conn.Close();
			return ds;
		}

	   
		/// <summary>
		/// 删除学生信息与选课信息studentNo
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <returns>删除是否成功</returns>
		public int DeleteStudentByNo(string studentNo)
		{
			int isDeleteSuccess = 0;

			conn.Open();
			SqlTransaction transaction = conn.BeginTransaction();
			try
			{
				string sql = "DELETE FROM usta_CoursesStudentsCorrelation WHERE studentNo=@studentNo;DELETE FROM usta_StudentsList WHERE studentNo=@studentNo";
				SqlParameter[] parameters = {
				 new SqlParameter("@studentNo", SqlDbType.NChar,10)
			};
				parameters[0].Value =studentNo;

				isDeleteSuccess = SqlHelper.ExecuteNonQuery(transaction, CommandType.Text, sql, parameters);
				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
			   MongoDBLog.LogRecord(ex);
				CommonUtility.RedirectUrl();
			}
			finally
			{
				conn.Close();
			}

			return isDeleteSuccess;
		}

		 
		/// <summary>
		/// 通过学号studentNo查询学生信息
		/// </summary>
		/// <param name="studentNo">学生编号</param>
		/// <returns>学生信息</returns>
		public StudentsList FindStudentByNo(string studentNo)
		{
			StudentsList student = null;
            string sql = "SELECT studentUserPwd,studentName,studentNo,studentSpeciality,mobileNo,SchoolClassName,emailAddress,remark FROM usta_StudentsList WHERE studentNo=@studentNo";
			SqlParameter[] parameters = {
				 new SqlParameter("@studentNo", SqlDbType.NChar,10)
			};
			parameters[0].Value = studentNo;
		   SqlDataReader dr= SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
			while(dr.Read())
			{
				student = new StudentsList();
				student.studentNo = dr["studentNo"].ToString();
				student.studentName = dr["studentName"].ToString();
				student.studentUserPwd = dr["studentUserPwd"].ToString().Trim();
				student.studentSpeciality = dr["studentSpeciality"].ToString();
				student.mobileNo = dr["mobileNo"].ToString();
				student.emailAddress = dr["emailAddress"].ToString();
				student.classNo = dr["SchoolClassName"].ToString();
				student.remark = dr["remark"].ToString();
			}
			dr.Close();
			conn.Close();
			return student;
		}
		
		/// <summary>
		/// 修改学生信息
		/// </summary>
		/// <param name="student">学生信息</param>
		public void UpdateStudentByStudent(StudentsList student)
		{
			try
			{
				string sql = "UPDATE usta_StudentsList SET studentUserPwd=@studentUserPwd,mobileNo=@mobileNo, emailAddress=@emailAddress, remark=@remark, studentSpeciality=@studentSpeciality, studentName=@studentName,classNo=@classNo WHERE studentNo=@studentNo";
				SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@studentName", SqlDbType.NChar,10),
					new SqlParameter("@studentUserPwd", SqlDbType.NChar,32),
					new SqlParameter("@studentSpeciality", SqlDbType.NChar,15),
					new SqlParameter("@mobileNo", SqlDbType.NChar,50),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					new SqlParameter("@classNo", SqlDbType.NChar,5)};
				parameters[0].Value = student.studentNo;
				parameters[1].Value = student.studentName;
				parameters[2].Value = student.studentUserPwd;
				parameters[3].Value = student.studentSpeciality;
				parameters[4].Value = student.mobileNo;
				parameters[5].Value = student.emailAddress;
				parameters[6].Value = student.remark;
				parameters[7].Value = student.classNo;
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
	 
		//-----------------------------------------------------------------
		
		/// <summary>
		/// 增加助教信息
		/// </summary>
		/// <param name="assistant">助教信息</param>
		public void  AddAssistant(AssistantsList assistant)
		{
			try
			{
				string sql = "INSERT INTO usta_TeachersList VALUES(@assistantNo,@assistantUserPwd, @assistantName,@emailAddress ,@officeAddress,@remark,@type)";
				SqlParameter[] parameters = {
					new SqlParameter("@assistantNo", SqlDbType.NVarChar,50),
					new SqlParameter("@assistantUserPwd", SqlDbType.NChar,32),
					new SqlParameter("@assistantName", SqlDbType.NChar,10),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@officeAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					  new SqlParameter("@type", SqlDbType.Int)                      };
				parameters[0].Value = assistant.assistantNo;
				parameters[1].Value = CommonUtility.EncodeUsingMD5(assistant.assistantUserPwd);
				parameters[2].Value = assistant.assistantName;
				parameters[3].Value = assistant.emailAddress;
				parameters[4].Value = assistant.officeAddress;
				parameters[5].Value = assistant.remark;
				parameters[6].Value = 2;

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
		/// 查询所有助教信息
		/// </summary>
		/// <returns>助教信息数据集</returns>
		public DataSet FindAllAssistant()
		{
			DataSet ds = null;
			string sql = "SELECT assistantNo, assistantName,emailAddress,officeAddress,remark FROM usta_AssistantsList AND type='2'";
			ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			conn.Close();
			return ds;
		}
		
		/// <summary>
		/// 删除助教信息通过助教编号assistantNo
		/// </summary>
		/// <param name="assistantNo">助教编号</param>
		/// <returns></returns>
		public int DeleteAssistantByNo(string assistantNo)
		{
			int isDeleteSuccess = 0;

			conn.Open();
			SqlTransaction transaction = conn.BeginTransaction();
			try
			{
				string sql = "DELETE FROM usta_CoursesTeachersCorrelation WHERE teacherNo=@assistantNo;DELETE FROM usta_TeachersList WHERE teacherNo=@assistantNo AND type='2'";
				SqlParameter[] parameters ={
				new SqlParameter("@assistantNo", SqlDbType.NVarChar,50)
			};
				parameters[0].Value = assistantNo;

				isDeleteSuccess = SqlHelper.ExecuteNonQuery(transaction, CommandType.Text, sql, parameters);
				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
			   MongoDBLog.LogRecord(ex);
				CommonUtility.RedirectUrl();
			}
			finally
			{
				conn.Close();
			}

			return isDeleteSuccess;
			
		}
		/// <summary>
		/// 查询助教信息
		/// </summary>
		/// <param name="assistantNo">助教编号</param>
		/// <returns>助教信息</returns>
		public AssistantsList FindAssistantByNo(string assistantNo)
		{
			AssistantsList assistant = null;
			string cmdstring = "SELECT teacherNo as [assistantNo]  ,teacherUserPwd as [assistantUserPwd] , teacherName as [assistantName] ,[emailAddress] ,[officeAddress]  ,[remark] FROM [USTA].[dbo].[usta_TeachersList] WHERE teacherNo=@teacherNo AND type=2";

			SqlParameter[] parameters ={
				new SqlParameter("@teacherNo", SqlDbType.NVarChar,50)
			};
			parameters[0].Value = assistantNo;
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
			if (dr.Read())
			{
				assistant = new AssistantsList
				{
					assistantUserPwd = dr["assistantUserPwd"].ToString().Trim(),
					assistantNo = dr["assistantNo"].ToString().Trim(),
					assistantName = dr["assistantName"].ToString().Trim(),
					emailAddress = dr["emailAddress"].ToString().Trim(),
					officeAddress = dr["officeAddress"].ToString().Trim(),
					remark = dr["remark"].ToString().Trim()
				};
			}
			dr.Close();
			conn.Close();
			return assistant;
		}
		
		/// <summary>
		/// 修改助教信息
		/// </summary>
		/// <param name="assistant">助教信息</param>
		public void UpdateAssistantByAssistant(AssistantsList assistant)
		{
			try
			{
				string sql = "UPDATE usta_TeachersList SET teacherUserPwd=@assistantUserPwd,officeAddress=@officeAddress, emailAddress=@emailAddress, remark=@remark, teacherName=@assistantName WHERE teacherNo=@assistantNo  AND type=2";
				SqlParameter[] parameters = {
					new SqlParameter("@assistantNo", SqlDbType.NVarChar,50),
					new SqlParameter("@assistantUserPwd", SqlDbType.NChar,32),
					new SqlParameter("@assistantName", SqlDbType.NChar,10),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@officeAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500)};
				parameters[0].Value = assistant.assistantNo;
				parameters[1].Value = assistant.assistantUserPwd;
				parameters[2].Value = assistant.assistantName;
				parameters[3].Value = assistant.emailAddress;
				parameters[4].Value = assistant.officeAddress;
				parameters[5].Value = assistant.remark;
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

		//-----------------------------------------------------------------
		
		/// <summary>
		/// 增加教师信息
		/// </summary>
		/// <param name="teacher">教师信息</param>
		public void AddTeacher(TeachersList teacher)
		{
			try
			{
				string sql = "INSERT INTO usta_TeachersList VALUES(@teacherNo,@teacherUserPwd,@teacherName,@emailAddress, @officeAddress,@remark,@type)";
				SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@teacherUserPwd", SqlDbType.NChar,32),
					new SqlParameter("@teacherName", SqlDbType.NChar,10),
				   
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@officeAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
											 new SqlParameter("@type", SqlDbType.Int,4)};
				parameters[0].Value = teacher.teacherNo;
				parameters[1].Value = CommonUtility.EncodeUsingMD5(teacher.teacherUserPwd);
				parameters[2].Value = teacher.teacherName;
				parameters[3].Value = teacher.emailAddress;
				parameters[4].Value = teacher.officeAddress;
				parameters[5].Value = teacher.remark;
				parameters[6].Value = teacher.type;

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
		/// 查询所有教师信息
		/// </summary>
		/// <returns>教师信息数据集</returns>
		public DataSet FindAllTeacher()
		{
			DataSet ds = null;
			string sql = "SELECT teacherNo, teacherName,emailAddress,officeAddress,remark FROM usta_TeachersList AND type='1'";
			ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			conn.Close();
			return ds;
		}

		
		/// <summary>
		/// 查询教师信息,通过教师编号teacherNo
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
		/// <returns>教师信息</returns>
		public TeachersList FindTeacherByEmail(string email)
		{
			TeachersList teacher = null;
            string cmdstring = "SELECT [teacherNo] ,[teacherName] ,[teacherUserPwd],[emailAddress] ,[officeAddress] ,[remark]  FROM [USTA].[dbo].[usta_TeachersList] WHERE emailAddress=@emailAddress";
			SqlParameter[] parameters = {
					 new SqlParameter("@emailAddress", SqlDbType.NChar,50)
			};
            parameters[0].Value = email;

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
			if (dr.Read())
			{
				teacher = new TeachersList
				{
					teacherUserPwd = dr["teacherUserPwd"].ToString().Trim(),
					teacherNo = dr["teacherNo"].ToString().Trim(),
					teacherName = dr["teacherName"].ToString().Trim(),
					emailAddress = dr["emailAddress"].ToString().Trim(),
					officeAddress = dr["officeAddress"].ToString().Trim(),
					remark = dr["remark"].ToString().Trim()

				};
			}
			dr.Close();
			conn.Close();
			return teacher;
		}
		
		/// <summary>
		/// 删除教师信息,通过教师编号teacherNo
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
		/// <returns>删除是否成功</returns>
		public int DeleteTeacherByNo(string teacherNo)
		{
			int isDeleteSuccess = 0;

			conn.Open();
			SqlTransaction transaction = conn.BeginTransaction();
			try
			{
				string sql = "DELETE FROM usta_CoursesTeachersCorrelation WHERE teacherNo=@teacherNo;DELETE FROM usta_TeachersList WHERE teacherNo=@teacherNo AND type='1'";
				SqlParameter[] parameters = {
					 new SqlParameter("@teacherNo", SqlDbType.NVarChar,50)
			};
				parameters[0].Value = teacherNo;

				isDeleteSuccess = SqlHelper.ExecuteNonQuery(transaction, CommandType.Text, sql, parameters);
				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
			   MongoDBLog.LogRecord(ex);
				CommonUtility.RedirectUrl();
			}
			finally
			{
				conn.Close();
			}

			return isDeleteSuccess;
		   
		}
		
		/// <summary>
		/// 修改教师信息
		/// </summary>
		/// <param name="teacher">教师信息</param>
		public void UpdateTeacherByTeacher(TeachersList teacher)
		{
			try
			{
				string sql = "UPDATE usta_TeachersList SET teacherUserPwd=@teacherUserPwd,officeAddress=@officeAddress,emailAddress=@emailAddress,remark=@remark,teacherName= @teacherName WHERE teacherNo=@teacherNo AND type='1'";
				SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@teacherUserPwd", SqlDbType.NChar,32),
					new SqlParameter("@teacherName", SqlDbType.NVarChar,50),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@officeAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500)};
				parameters[0].Value = teacher.teacherNo;
				parameters[1].Value =teacher.teacherUserPwd;
				parameters[2].Value = teacher.teacherName;
				parameters[3].Value = teacher.emailAddress;
				parameters[4].Value = teacher.officeAddress;
				parameters[5].Value = teacher.remark;
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
		/// 读取Excel文档 
		/// </summary>
		/// <param name="Path">文件名称</param>
		/// <param name="sheetName">工作薄名称</param>
		/// <returns>返回一个数据集</returns>
		public DataSet ExcelToDS(string Path, string sheetName)
		{
			//Excel文件中要有工作表[sheetName]
			string strExcel = "select * from [" + sheetName + "$]";

			string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
			OleDbConnection conn = new OleDbConnection(strConn);
			OleDbDataAdapter myCommand = null;
			DataSet ds = new DataSet();
			try
			{
				conn.Open();
				myCommand = new OleDbDataAdapter(strExcel, strConn);
				myCommand.Fill(ds, "sheetTable");//存储在数据集ds中的表sheetTable里
				conn.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return ds;
		}

		/// <summary> 
		///将Excel表的信息数据保存到数据库
		/// </summary>
		/// <param name="dt">文件名称</param> 
		/// <param name="tableType">表类型</param> 
		/// <returns>删除是否成功</returns> 
		public int DataTabletoDBTables(DataTable dt,string tableType)
		{
			
			if (tableType.Equals("学生信息"))
			{               
					if (dt.Rows.Count > 0)
					{
						for (int i = 0; i < dt.Rows.Count; i++)                         
							{
								StudentsList student = new StudentsList();
								student.studentNo = dt.Rows[i][1].ToString().Trim();
								student.studentName = dt.Rows[i][2].ToString().Trim();
								student.studentSpeciality = dt.Rows[i][3].ToString().Trim();
								student.classNo = dt.Rows[i][4].ToString().Trim();
								student.mobileNo = dt.Rows[i][5].ToString().Trim();
								student.emailAddress = dt.Rows[i][6].ToString().Trim();
								student.remark = dt.Rows[i][7].ToString().Trim();
								student.studentUserPwd = student.studentNo;//密码默认与学号相同
								this.AddStudent(student);                               
							}                      
					   return dt.Rows.Count;
					}
			}
			else if (tableType.Equals("教师信息"))
			{
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						TeachersList teacher = new TeachersList();
						teacher.teacherNo = dt.Rows[i][1].ToString().Trim();
						teacher.teacherName = dt.Rows[i][2].ToString().Trim();
						teacher.emailAddress = dt.Rows[i][3].ToString().Trim();
						teacher.officeAddress = dt.Rows[i][4].ToString().Trim();
						teacher.remark = dt.Rows[i][5].ToString().Trim();
						teacher.teacherUserPwd = teacher.teacherNo;//密码默认与教师编号相同
						this.AddTeacher(teacher);
					}
					return dt.Rows.Count;
				}
			}
			else if (tableType.Equals("助教信息"))
			{
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						AssistantsList assistant = new AssistantsList();
						assistant.assistantNo = dt.Rows[i][1].ToString().Trim();
						assistant.assistantName = dt.Rows[i][2].ToString().Trim();
						assistant.emailAddress = dt.Rows[i][3].ToString().Trim();
						assistant.officeAddress = dt.Rows[i][4].ToString().Trim();
						assistant.remark = dt.Rows[i][5].ToString().Trim();
						assistant.assistantUserPwd = assistant.assistantNo;//密码默认与助教编号相同
						this.AddAssistant(assistant);
					}
					return dt.Rows.Count;
				}
			}
			else if (tableType.Equals("课程信息"))
			{
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						Courses course = new Courses();
						course.courseNo = dt.Rows[i][1].ToString().Trim();
						course.courseName= dt.Rows[i][2].ToString().Trim();
						course.period =dt.Rows[i][3].ToString().Trim();
						course.credit=float.Parse( dt.Rows[i][4].ToString().Trim());
						course.courseSpeciality= dt.Rows[i][5].ToString();

						course.termTag =dt.Rows[i][6].ToString().Trim();//学期标识
						new DalOperationAboutCourses().AddCourse(course);
					}
					return dt.Rows.Count;
				}
			}
			else if (tableType.Equals("考试安排"))
			{
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						ExamArrangeList examTime = new ExamArrangeList();
						examTime.courseName= dt.Rows[i][1].ToString().Trim();
						examTime.examArrangeTime =DateTime.Parse(dt.Rows[i][2].ToString().Trim());
						examTime.examArrageAddress = dt.Rows[i][3].ToString().Trim();
						examTime.remark =dt.Rows[i][4].ToString().Trim();
						examTime.teacherName = dt.Rows[i][5].ToString();
						examTime.courseNo= dt.Rows[i][6].ToString().Trim();
						this.AddExamArrange(examTime);
					}
					return dt.Rows.Count;
				}
			} 
			return 0;
		}
		//----------------------------------------
		
		/// <summary>
		/// 增加考试安排信息
		/// </summary>
		/// <param name="examTime">考试安排信息</param>
		public void AddExamArrange(ExamArrangeList examTime)
		{
			try
			{
				string sql = "INSERT INTO usta_ExamArrangeList VALUES(@courseName,@examArrangeTime,@examArrageAddress,@remark, @teacherName,@courseNo)";
				SqlParameter[] parameters = {
					new SqlParameter("@courseName", SqlDbType.NChar,50),
					new SqlParameter("@examArrangeTime", SqlDbType.DateTime),
					new SqlParameter("@examArrageAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					new SqlParameter("@teacherName", SqlDbType.NChar,10),
					new SqlParameter("@courseNo", SqlDbType.NChar,20)};
				parameters[0].Value = examTime.courseName;
				parameters[1].Value = examTime.examArrangeTime;
				parameters[2].Value = examTime.examArrageAddress;
				parameters[3].Value = examTime.remark;
				parameters[4].Value = examTime.teacherName;
				parameters[5].Value = examTime.courseNo;
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

		///<summary>
		///查询所有考试安排信息
		/// </summary> 
		/// <returns>考试安排信息数据集</returns> 
		public DataSet FindExamArrange()
		{
			DataSet ds = null;
			string sql = "SELECT [examArrangeListId] ,[courseName],[examArrangeTime] ,[examArrageAddress],[remark],[teacherName],[courseNo] FROM [USTA].[dbo].[usta_ExamArrangeList]";
			ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 删除考试安排信息
		/// </summary>
		/// <param name="examArrangeListId">考试安排信息编号</param>
		public void DeleteExamArrangeById(int examArrangeListId)
		{
			try
			{
				string sql = "DELETE FROM usta_ExamArrangeList WHERE examArrangeListId=@examArrangeListId";
				SqlParameter[] parameters = {
				 new SqlParameter("@examArrangeListId", SqlDbType.Int,4)
			 };
				parameters[0].Value = examArrangeListId;
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

		//---------------------------------------------
		/// <summary> 
		///查询当前学期的密码所有映射信息
		/// </summary> 
		/// <returns>映射信息数据集</returns> 
		public DataSet FindPasswordMapping()
		{
			DataSet ds = null;
			string sql = "SELECT [passwordMappingId] ,[userNo] ,[userName] ,[initializePassword]  FROM [USTA].[dbo].[usta_PasswordMapping] WHERE termTag=(SELECT TOP 1 termTag FROM usta_TermTags ORDER BY termTag DESC)";
			ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
			conn.Close();
			return ds;
		}

		/// <summary> 
		///查询当前学期的密码映射信息--教师与助教
		/// </summary> 
		/// <returns>映射信息数据集</returns> 
		public DataSet FindPasswordMapping(int userType,string keyword)
		{
			DataSet ds = new DataSet();
			string sql = "SELECT [passwordMappingId], [userNo] ,[userName] ,[initializePassword] FROM [USTA].[dbo].[usta_PasswordMapping] WHERE termTag=(SELECT TOP 1 termTag FROM usta_TermTags ORDER BY termTag DESC) AND userType=@userType  AND ((RTRIM(userNo) LIKE '%' + '" + keyword + "' +'%') OR (RTRIM(userName) LIKE '%' + '" + keyword + "' +'%'))";
			SqlParameter[] parameters= {
				//new SqlParameter("@keyword", SqlDbType.NChar,10),
				new SqlParameter("@userType", SqlDbType.SmallInt,2)            
					};
			//parameters[0].Value = string.Empty;
			parameters[0].Value = userType;
			ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql,parameters);
			conn.Close();
			return ds;
		}

		/// <summary> 
		///查询当前学期的密码映射信息--学生-专业划分
		/// </summary> 
		/// <returns>映射信息数据集</returns> 
		public DataSet FindPasswordMapping(int userType, string keyword, string studentSpeciality)
		{
			DataSet ds = null;
			string sql = "select passwordMappingId,userNo,userName,initializePassword,studentSpeciality ";
			sql += "from usta_StudentsList,usta_PasswordMapping ";
			sql += "where userType=@userType ";
			sql += "and termTag=(SELECT TOP 1 termTag FROM usta_TermTags ORDER BY termTag DESC) ";
			sql += "and (userNo  LIKE '%' + '" + keyword + "' +'%' OR userName  LIKE '%' + '" + keyword + "' +'%') ";
			//sql += "and (userNo LIKE @keyword  OR userName LIKE @keyword) ";
			sql += "and usta_StudentsList.studentNo=usta_PasswordMapping.userNo ";
			sql += "and studentSpeciality=@studentSpeciality ";
			sql += "order by userNo asc ";
			//string sql = "spSearchInitializePassword";
			SqlParameter[] parameters = {
					new SqlParameter("@keyword", SqlDbType.NChar,10),
					new SqlParameter("@userType", SqlDbType.SmallInt,2),
					new SqlParameter("@studentSpeciality", SqlDbType.NChar,15)
					};
			parameters[0].Value = keyword ;
			parameters[1].Value = userType;
			parameters[2].Value = studentSpeciality;

			ds = SqlHelper.ExecuteDataset(conn,CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}

	   
		/// <summary>
		/// 查询用户信息：用户类型和关键字  --密码重置    
		/// </summary>
		/// <param name="type">用户类型</param>
		/// <param name="keyword">关键字</param>
		/// <returns></returns>
		public DataTable FindUserByTypeAndKeywod(int type, string keyword)
		{
			string sql=null;
			DataSet ds = null;
			DataTable dt = null;
			SqlParameter[] parameters;
			switch(type)
			{              
				case 1://教师
                    sql = "SELECT teacherNo, teacherName,emailAddress FROM usta_TeachersList WHERE type=@type AND (teacherNo like @keyword or teacherName like @keyword)";
					parameters =new SqlParameter[2]{
							  new SqlParameter("@keyword","%" + keyword + "%"),
                              new SqlParameter("@type",type)    
					};
					ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
					dt = ds.Tables[0];
					dt.Columns["teacherNo"].ColumnName = "userNo";
					dt.Columns["teacherName"].ColumnName = "userName";
					conn.Close();
					break;
				case 2: //助教
                    sql = "SELECT teacherNo as assistantNo, teacherName as assistantName,emailAddress FROM usta_TeachersList WHERE type=@type AND (teacherNo like @keyword or teacherName like @keyword)";
                    parameters = new SqlParameter[2]{
							  new SqlParameter("@keyword","%" + keyword + "%"),
                              new SqlParameter("@type",type)    
					};
					ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
					dt = ds.Tables[0];
					dt.Columns["assistantNo"].ColumnName = "userNo";
					dt.Columns["assistantName"].ColumnName = "userName";
					conn.Close();
					break;
				case 3://学生
					sql = "SELECT studentNo, studentName,studentSpeciality,emailAddress FROM usta_StudentsList WHERE studentNo like @keyword or studentName like @keyword";
					parameters = new SqlParameter[1]{
							  new SqlParameter("@keyword","%" + keyword + "%")                                        
					};
					ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
					dt = ds.Tables[0];
					dt.Columns["studentNo"].ColumnName = "userNo";
					dt.Columns["studentName"].ColumnName = "userName";
					conn.Close();
					break;
				default:
					break;
			}         
			return dt;
		}

        /// <summary>
        /// 查询老师信息：学期标识和关键字
        /// </summary>
        /// <returns></returns>
        public DataTable SearchTeacherByTermTagAndKeyword(string termTag, string keyword) 
        {
            
            if (termTag == null || termTag.Trim().Length == 0)
            {
                return this.SearchUserByTypeAndKeywod(1, keyword);
            }
            else {
                string sql = null;
                DataSet ds = null;
                DataTable dt = null;
                SqlParameter[] parameters;

                sql = "SELECT distinct(usta_TeachersList.teacherNo) as teacherNo, teacherName, type, emailAddress FROM usta_TeachersList, usta_CoursesTeachersCorrelation WHERE termTag = @termTag AND usta_TeachersList.teacherNo = usta_CoursesTeachersCorrelation.teacherNo AND (usta_TeachersList.teacherNo like @keyword or teacherName like @keyword)";
                parameters = new SqlParameter[]{
                    new SqlParameter("@termTag", termTag),
                    new SqlParameter("@keyword", "%" + keyword + "%")
                };
                ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
                dt = ds.Tables[0];
                conn.Close();
                return dt;
            }
        }
        /// <summary>
        /// type= 1:院内，2：院外老师，3：院外助教
        /// </summary>
        /// <param name="termTag"></param>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable SearchTeacherByTermTagAndKeyword(string termTag, string keyword,int type)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@termTag", termTag),
                    new SqlParameter("@keyword", "%" + keyword + "%")
                };
            if (string.IsNullOrWhiteSpace(termTag))
            {
                string sql = "SELECT distinct(usta_TeachersList.teacherNo) AS teacherNo, usta_TeachersList.teacherName AS teacherName,usta_TeachersList.type AS type,usta_TeachersList.emailAddress AS emailAddress,usta_TeachersList.teacherType AS teacherType,usta_TeachersList.officeAddress AS officeAddress,usta_TeachersList.EmployeeNum AS employeeNum FROM usta_TeachersList, usta_CoursesTeachersCorrelation WHERE (usta_TeachersList.teacherNo like @keyword or usta_TeachersList.teacherName like @keyword) ";

               if (type == 1)
               {
                   sql = sql + " AND usta_TeachersList.teacherType='本院'";
               }
               else if(type == 2)
               {
                   sql = sql + "AND usta_TeachersList.teacherType<>'本院' AND usta_CoursesTeachersCorrelation.atCourseType=1";
               }
               else if (type == 3)
               {
                   sql = sql + "AND teacherType<>'本院' AND usta_CoursesTeachersCorrelation.atCourseType=2";
               }
               return SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters).Tables[0];
            }
            else
            {
                string sql = null;
                DataSet ds = null;
                DataTable dt = null;


                sql = "SELECT distinct(usta_TeachersList.teacherNo) as teacherNo, teacherName, type,teacherType,emailAddress,officeAddress,usta_TeachersList.EmployeeNum AS employeeNum FROM usta_TeachersList, usta_CoursesTeachersCorrelation WHERE termTag = @termTag AND usta_TeachersList.teacherNo = usta_CoursesTeachersCorrelation.teacherNo AND (usta_TeachersList.teacherNo like @keyword or teacherName like @keyword)";
                if (type == 1)
                {
                    sql = sql + " AND TeacherType='本院'";
                }
                else if (type == 2)
                {
                    sql = sql + "AND TeacherType<>'本院' AND atCourseType=1";
                }
                else if (type == 3)
                {
                    sql = sql + "AND TeacherType<>'本院' AND atCourseType=2";
                }
                ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
                dt = ds.Tables[0];
                conn.Close();
                return dt;
            }
        }

		 
		/// <summary>
		/// 查询用户信息：用户类型和关键字  --搜索用户    
		/// </summary>
		/// <param name="type">用户类型</param>
		/// <param name="keyword">关键字</param>
		/// <returns>用户信息</returns>
		public DataTable SearchUserByTypeAndKeywod(int type, string keyword)
		{
			string sql = null;
			DataSet ds = null;
			DataTable dt = null;
			SqlParameter[] parameters;
			switch (type)
			{
				case 1://教师
                    sql = "select [teacherNo],[teacherUserPwd],[teacherName],[emailAddress],[officeAddress],[remark],[type],[isAdmin],[TeacherID],[TeacherUSID],[EmployeeNum],[isHeadteacher],[TeacherType],[IsBusinessGuru],[IsAssistant],[IsSuperisor] from [usta_TeachersList] WHERE (teacherNo like @keyword or teacherName like @keyword) ";
					parameters = new SqlParameter[1]{
							 new SqlParameter("@keyword", "%" + keyword + "%")                                      
					};
					ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
					dt = ds.Tables[0];                   
					conn.Close();
					break;
				case 2: //助教
                    sql = "SELECT teacherNo as assistantNo, teacherName as assistantName,emailAddress FROM usta_TeachersList WHERE (teacherNo like @keyword or teacherName like @keyword) AND type='2'";
					parameters = new SqlParameter[1]{
							 new SqlParameter("@keyword", "%" + keyword + "%")                                        
					};
					ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
					dt = ds.Tables[0];                    
					conn.Close();
					break;
				case 3://学生

					sql = "SELECT studentNo, studentName,studentSpeciality FROM usta_StudentsList WHERE studentNo like @keyword or studentName like @keyword";
					parameters = new SqlParameter[1]{
							  new SqlParameter("@keyword", "%" + keyword + "%")                              
					};
					ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
					dt = ds.Tables[0];                  
					conn.Close();
					break;
				default:
					break;
			}
			return dt;
		}
        public DataSet SearchTeacher(string key, string teacherType)
        {

            string sql = "SELECT [teacherNo],[teacherUserPwd],[teacherName],[emailAddress],[officeAddress],[remark],[type],[isAdmin],[TeacherID],[TeacherUSID],[EmployeeNum],[isHeadteacher],[isAssistant],[TeacherType],[IsBusinessGuru] FROM [USTA].[dbo].[usta_TeachersList] WHERE (teacherNo like @key or teacherName like @key)";
            if (teacherType != null && teacherType != "")
            {
                sql = sql + "AND TeacherType = @teacherType";
            }
            SqlParameter[] parameters = new SqlParameter[2]{
							  new SqlParameter("@key", "%" + key + "%"),
                              new SqlParameter("@teacherType",teacherType)
					};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }
		 
		/// <summary>
		/// 查询选课学生信息
		/// </summary>
		/// <param name="courseNo">课程编号</param>
		/// <returns>学生信息数据集</returns>
		public DataSet SearchStudentByCourseNo(string yearCourseNoClassID)
		{
			string sql ="select usta_StudentsList.studentNo,studentName,emailAddress,courseNo ";
			sql+="from usta_StudentsList,usta_CoursesStudentsCorrelation ";
			sql+="where usta_StudentsList.studentNo=usta_CoursesStudentsCorrelation.studentNo ";
            sql += "and (RTRIM(year)+RTRIM(courseNo)+RTRIM(classID))=@yearCourseNoClassID;";
			SqlParameter[] parameters = new SqlParameter[]{ 
							  new SqlParameter("@yearCourseNoClassID", yearCourseNoClassID)                   
					};
		   DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
		   conn.Close();
		   return ds;
		}
		/// <summary>
		/// 返回学生这学期的考试安排，作业，实验
		/// </summary>
		/// <param name="studentNo">学生编号</param>
		/// <returns>学生这学期的考试安排，作业，实验数据集</returns>
		public DataSet StudentHasExam(string studentNo)
		{
			string cmdstring = "SELECT examArrangeListId,courseName,examArrangeTime,examArrageAddress,remark FROM usta_CoursesStudentsCorrelation,usta_ExamArrangeList WHERE usta_CoursesStudentsCorrelation.studentNo=@studentNo AND    usta_CoursesStudentsCorrelation.courseNo=usta_ExamArrangeList.courseNo;";
			cmdstring += "SELECT experimentResourceId,experimentResourceTitle,usta_CoursesStudentsCorrelation.courseNo FROM usta_CoursesStudentsCorrelation,usta_ExperimentResources WHERE usta_CoursesStudentsCorrelation.studentNo=@studentNo AND usta_CoursesStudentsCorrelation.courseNo=usta_ExperimentResources.courseNo AND deadLine>@now AND usta_ExperimentResources.experimentResourceId NOT IN( SELECT experimentResourceId FROM usta_Experiments WHERE usta_Experiments.studentNo=@studentNo);";
			cmdstring += "SELECT schoolWorkNotifyId,schoolWorkNotifyTitle,usta_SchoolWorkNotify.courseNo FROM usta_CoursesStudentsCorrelation,usta_SchoolWorkNotify WHERE usta_CoursesStudentsCorrelation.studentNo=@studentNo AND usta_CoursesStudentsCorrelation.courseNo=usta_SchoolWorkNotify.courseNo AND deadLine>@now AND isOnline='true' AND usta_SchoolWorkNotify.schoolWorkNotifyId NOT IN ( SELECT  schoolWorkNofityId FROM usta_SchoolWorks WHERE usta_SchoolWorks.studentNo=@studentNo)";
			SqlParameter[] parameters ={
				new SqlParameter("@studentNo", SqlDbType.NChar,10),
				new SqlParameter("@now",SqlDbType.DateTime)
			};
			parameters[0].Value = studentNo;
			parameters[1].Value = DateTime.Now.ToString();
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 主页显示提示，包括考试安排，未上交作业，未上交实验，还有反馈回复等
		/// </summary>
		/// <param name="studentNo">学生编号</param>
		/// <returns>包括考试安排，未上交作业，未上交实验，还有反馈回复数据集</returns>
		public DataSet StudentTips(string studentNo)
		{
            string cmdstring = "SELECT examArrangeListId,courseName,examArrangeTime,examArrageAddress,remark FROM usta_CoursesStudentsCorrelation,usta_ExamArrangeList WHERE usta_CoursesStudentsCorrelation.studentNo=@studentNo AND    usta_CoursesStudentsCorrelation.courseNo=usta_ExamArrangeList.courseNo ORDER BY examArrangeListId DESC;";
            cmdstring += "SELECT usta_Experiments.[experimentResourceId],experimentResourceTitle,courseNo ,classID,termTag,deadline FROM usta_Experiments,usta_ExperimentResources WHERE studentNo=@studentNo AND usta_Experiments.experimentResourceId=usta_ExperimentResources.experimentResourceId AND isSubmit='0' AND usta_ExperimentResources.termTag=@termTag ORDER BY deadLine DESC,experimentResourceId DESC;";
            cmdstring += "SELECT schoolWorkNofityId,schoolWorkNotifyTitle,courseNo,isOnline,classID,termTag,deadline FROM usta_SchoolWorks,usta_SchoolWorkNotify WHERE studentNo=@studentNo AND usta_SchoolWorks.schoolWorkNofityId=usta_SchoolWorkNotify.schoolWorkNotifyId AND isSubmit='0' AND isOnline='1' and usta_SchoolWorkNotify.termTag=@termTag ORDER BY deadLine DESC,schoolWorkNofityId DESC;";
            cmdstring += "SELECT schoolWorkNofityId,schoolWorkNotifyTitle,courseNo,isOnline,classID,termTag,deadline FROM usta_SchoolWorks,usta_SchoolWorkNotify WHERE studentNo=@studentNo AND usta_SchoolWorks.schoolWorkNofityId=usta_SchoolWorkNotify.schoolWorkNotifyId AND isSubmit='0' AND isOnline='0' and usta_SchoolWorkNotify.termTag=@termTag ORDER BY schoolWorkNofityId DESC;";
            cmdstring += "select [feedBackId],[feedBackTitle],[feedBackContent],[feedBackContactTo],[isRead],[updateTime],[backInfo],[backTime],[backUserNo],[backUserType] from [usta_FeedBack] WHERE backUserNo=@studentNo AND len(backInfo)<>0 AND isRead=0 ORDER BY feedBackId DESC;";
			SqlParameter[] parameters ={
				new SqlParameter("@studentNo", SqlDbType.NChar,10),
				new SqlParameter("@now",SqlDbType.DateTime),
               new SqlParameter("@termTag", SqlDbType.NVarChar,50)
			};
			parameters[0].Value = studentNo;
			parameters[1].Value = DateTime.Now.ToString();
            parameters[2].Value = termTag;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 判断是否有实验未提交
		/// </summary>
		/// <param name="studentNo">学生编号</param>
		/// <returns>实验未提交数据集</returns>
		public DataSet StudenthasExperimentNotCommit(string studentNo)
		{
			string cmdstring = "SELECT experimentResourceId,experimentResourceTitle FROM usta_CoursesStudentsCorrelation,usta_ExperimentResources WHERE usta_CoursesStudentsCorrelation.studentNo=@studentNo AND usta_CoursesStudentsCorrelation.courseNo=usta_ExperimentResources.courseNo AND usta_ExperimentResources.experimentResourceId NOT IN( SELECT experimentResourceId FROM usta_Experiments WHERE usta_Experiments.studentNo=@studentNo)";
			SqlParameter[] parameters ={
			   new SqlParameter("@studentNo", SqlDbType.NChar,10)
			};
			parameters[0].Value = studentNo;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 搜索教师，助教(不包括企业导师)
		/// </summary>
		/// <param name="searchstring">搜索参数</param>
		/// <returns>搜索教师，助教信息</returns>
		public DataSet SearchTeacherAndAssitant(string searchstring)
		{
            string cmdstring = "select 1 as usertye, emailAddress as email,teacherName as username from usta_TeachersList where IsBusinessGuru=1  AND teacherName like '%'+@strings+'%'";

				SqlParameter[] parameters ={
			   new SqlParameter("@strings", searchstring)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}

        /// <summary>
        /// 查询教师信息,通过教师编号teacherNo
        /// </summary>
        /// <param name="teacherNo">教师编号</param>
        /// <returns>教师信息</returns>
        public TeachersList FindTeacherByNo(string teacherNo)
        {
            TeachersList teacher = null;
            string cmdstring = "SELECT [teacherNo] ,[teacherName] ,[teacherUserPwd],[emailAddress] ,[officeAddress] ,[remark]  FROM [USTA].[dbo].[usta_TeachersList] WHERE teacherNo=@teacherNo";
            SqlParameter[] parameters = {
					 new SqlParameter("@teacherNo", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = teacherNo;

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
            if (dr.Read())
            {
                teacher = new TeachersList
                {
                    teacherUserPwd = dr["teacherUserPwd"].ToString().Trim(),
                    teacherNo = dr["teacherNo"].ToString().Trim(),
                    teacherName = dr["teacherName"].ToString().Trim(),
                    emailAddress = dr["emailAddress"].ToString().Trim(),
                    officeAddress = dr["officeAddress"].ToString().Trim(),
                    remark = dr["remark"].ToString().Trim()

                };
            }
            dr.Close();
            conn.Close();
            return teacher;
        }
		
		#endregion
	}
	
}
