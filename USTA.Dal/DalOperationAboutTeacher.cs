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
	/// 教师相关操作类
	/// </summary>
	public class DalOperationAboutTeacher
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
		public DalOperationAboutTeacher()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

			termTag = DalCommon.GetTermTag(conn);
		}
		#endregion

		#region

        public DataSet GetTeachers(string ids)
        {
             string commandstring = "SELECT [teacherNo],[teacherName] FROM [USTA].[dbo].[usta_TeachersList] WHERE [teacherNo] in (@ids)";
             SqlParameter[] parameters ={
				new SqlParameter("@ids", ids)        
			};
             DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
             conn.Close();
             return ds;
        }
		/// <summary>
		/// 通过教师获得当前学期的任课
		/// </summary>
		/// <param name="TeacherNo">教师编号</param>
		/// <returns>教师课程数据集</returns>
		public DataSet GetCoursesByTeachertNo(string TeacherNo)
		{

            string commandstring = "SELECT coursesTeachersCorrelationId,usta_Courses.courseNo,usta_Courses.ClassID,courseName,usta_CoursesTeachersCorrelation.teacherNo,usta_Courses.termTag FROM usta_CoursesTeachersCorrelation,usta_Courses WHERE usta_CoursesTeachersCorrelation.courseNo=usta_Courses.courseNo AND usta_CoursesTeachersCorrelation.teacherNo=@TeacherNo AND usta_CoursesTeachersCorrelation.ClassID=usta_Courses.ClassID AND usta_Courses.termTag=@termTag AND usta_CoursesTeachersCorrelation.termTag=usta_Courses.termTag AND usta_CoursesTeachersCorrelation.atCourseType=1 ORDER BY atCourseType ASC;";
			SqlParameter[] parameters ={
				new SqlParameter("@TeacherNo", SqlDbType.NVarChar,50),
				new SqlParameter("@termTag", SqlDbType.NVarChar,50)             
			};
		   parameters[0].Value = TeacherNo;
		   parameters[1].Value = termTag;
		   DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 通过教师获得当前学期其他任课
		/// </summary>
		/// <param name="TeacherNo">教师编号</param>
		/// <returns>数据集</returns>
		public DataSet GetOtherCoursesByTeacherNo(string TeacherNo)
		{
            string commandstring = "SELECT courseNo,courseName,ClassID,termTag FROM usta_Courses where (courseNo+termTag+classID) NOT IN( SELECT (courseNo+termTag+classID) FROM usta_CoursesTeachersCorrelation WHERE teacherNo=@TeacherNo) AND termTag=@termTag";
			SqlParameter[] parameters ={
				new SqlParameter("@TeacherNo", SqlDbType.NVarChar,50),
				new SqlParameter("@termTag", SqlDbType.NVarChar,50)             
			};
			parameters[0].Value = TeacherNo;
			parameters[1].Value = termTag;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 通过教师获得某学期其他任课
		/// </summary>
		/// <param name="TeacherNo">教师编号</param>
		/// <param name="termTags">学期标识</param>
		/// <returns>教师未选课程数据集</returns>
		public DataSet GetOtherCoursesByTeacherNo(string TeacherNo,string termTags)
		{
            string commandstring = "SELECT courseNo,courseName,ClassID,termTag FROM usta_Courses where (courseNo+termTag+classID) NOT IN( SELECT (courseNo+termTag+classID) FROM usta_CoursesTeachersCorrelation WHERE teacherNo=@TeacherNo) AND termTag=@termTag";
			SqlParameter[] parameters ={
				new SqlParameter("@TeacherNo", SqlDbType.NVarChar,50),
				new SqlParameter("@termTag", SqlDbType.NVarChar,50)             
			};
			parameters[0].Value = TeacherNo;
			parameters[1].Value = termTags;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 通过教师和学期标示获得当前学期任课
		/// </summary>
		/// <param name="TeacherNo">教师编号</param>
		/// <param name="termTags">学期标识</param>
		/// <returns>教师任教课程数据集</returns>
		public DataSet GetCoursesByTeachertNo(string TeacherNo,string termTags)
		{

            string commandstring = "SELECT usta_Courses.courseNo,usta_Courses.ClassID,courseName,usta_Courses.termTag FROM usta_CoursesTeachersCorrelation,usta_Courses WHERE usta_CoursesTeachersCorrelation.courseNo=usta_Courses.courseNo AND usta_CoursesTeachersCorrelation.teacherNo=@TeacherNo AND atCourseType=1 AND usta_CoursesTeachersCorrelation.ClassID=usta_Courses.ClassID AND  usta_Courses.termTag=@termTag  AND usta_CoursesTeachersCorrelation.termTag=usta_Courses.termTag";
			SqlParameter[] parameters ={
				new SqlParameter("@TeacherNo", SqlDbType.NChar,50),
				new SqlParameter("@termTag", SqlDbType.NVarChar,50)             
			};
			parameters[0].Value = TeacherNo;
			parameters[1].Value = termTags;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 通过名字获得教师
		/// </summary>
		/// <param name="name">教师姓名</param>
		/// <returns>教师数据集</returns>
		public DataSet GetTeachersByName(string name)
		{
			string cmdstring = "SELECT [teacherNo],[teacherName]  FROM [USTA].[dbo].[usta_TeachersList] WHERE teacherName=@teacherName";
			SqlParameter[] parameters ={
				new SqlParameter("@teacherName", SqlDbType.NChar,10)
			};
			parameters[0].Value = name;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 更改教师的身份为助教或者取消助教身份
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
        /// <param name="isAssistant">是否为助教</param>
		public void ChangeTeacherType(string teacherNo,bool isAssistant)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_TeachersList] SET  [type] = " + (isAssistant?2:1) + " WHERE teacherNo=@teacherNo";
				SqlParameter[] parameters ={
				new SqlParameter("@teacherNo", SqlDbType.NVarChar,50)
				};
				parameters[0].Value = teacherNo;
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
		/// 获得所有教师
		/// </summary>
		/// <returns>教师数据集</returns>
		public DataSet GetTeachers()
		{
			string cmdstring = "SELECT [teacherNo],[teacherName],[employeeNum] FROM [USTA].[dbo].[usta_TeachersList]";
			
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 通过教师主键获得教师dataset
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
		/// <returns>教师数据集</returns>
		public DataSet GetTeacherbyId(string teacherNo)
		{
			string cmdstring = "SELECT [teacherNo] ,[teacherName] ,[emailAddress] ,[officeAddress] ,[remark],[Sex]  FROM [USTA].[dbo].[usta_TeachersList] WHERE teacherNo=@teacherNo";
			SqlParameter[] parameters ={
				 new SqlParameter("@teacherNo", SqlDbType.NVarChar,50)
			};
			parameters[0].Value = teacherNo;
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			return ds;
		}
		/// <summary>
		/// 通过教师主键获得教师
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
		/// <returns>教师实体</returns>
		public TeachersList GetTeacherById(string teacherNo)
		{
			TeachersList teacher=null;
			string cmdstring = "SELECT [teacherNo] ,[teacherName] ,[emailAddress] ,[officeAddress] ,[remark],[type],[teacherType]  FROM [USTA].[dbo].[usta_TeachersList] WHERE teacherNo=@teacherNo";
			SqlParameter[] parameters ={
				 new SqlParameter("@teacherNo", SqlDbType.NVarChar,50)
			};
			parameters[0].Value = teacherNo;
			SqlDataReader dr=SqlHelper.ExecuteReader(conn,CommandType.Text,cmdstring,parameters);
			while(dr.Read())
			{
				teacher = new TeachersList{
					teacherNo = dr["teacherNo"].ToString().Trim(),
					teacherName = dr["teacherName"].ToString().Trim(),
					emailAddress = dr["emailAddress"].ToString().Trim(),
					officeAddress = dr["officeAddress"].ToString().Trim(),
                    type = int.Parse(dr["type"].ToString().Trim()),
                    remark = dr["remark"].ToString().Trim(),
                };
                if (dr["teacherType"] != null)
                {
                    teacher.teacherType = dr["teacherType"].ToString().Trim();

                }
                else {
                    teacher.teacherType = "";
                }
            }
					
			dr.Close();
			conn.Close();
			return teacher;
		}
		/// <summary>
		/// 更新老师联系方式
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
		/// <param name="email">email</param>
		/// <param name="office">办公室</param>
		/// <param name="remark">备注</param>
		public void UpdateEmail(string teacherNo, string email,string office,string remark)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_TeachersList] SET [emailAddress] = @emailAddress,[officeAddress] = @officeAddress,remark=@remark WHERE teacherNo=@teacherNo";

				SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@officeAddress", SqlDbType.NChar,50),
					new SqlParameter("@remark",SqlDbType.NChar,500)
					};
				parameters[0].Value = teacherNo;
				parameters[1].Value = email;
				parameters[2].Value = office;
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
		/// 删除教师的任课
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
		/// <param name="courseNo">课程编号</param>
		public void DelCourseOfTeacher(string teacherNo, string courseNo)
		{
			try
			{
				string cmdstring = "DELETE FROM [USTA].[dbo].[usta_CoursesTeachersCorrelation] WHERE teacherNo=@teacherNo AND courseNo=@courseNo";
				SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@teacherNo",teacherNo),
				new SqlParameter("@courseNo",courseNo),
				
			};
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
        /// 删除教师的任课
        /// </summary>
        /// <param name="teacherNo">教师编号</param>
        /// <param name="courseNo">课程编号</param>
        public void DelCourseOfTeacherByCoursesTeachersCorrelationId(string coursesTeachersCorrelationId)
        {
            try
            {
                string cmdstring = "DELETE FROM [USTA].[dbo].[usta_CoursesTeachersCorrelation] WHERE coursesTeachersCorrelationId=@coursesTeachersCorrelationId";
                SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@coursesTeachersCorrelationId",coursesTeachersCorrelationId)
				
			};
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
		/// 添加教师的任课
		/// </summary>
		/// <param name="teacherNo">教师编号</param>
		/// <param name="courseNo">课程编号</param>
        public void AddCourseOfTeacher(string teacherNo, string courseNo, string ClassID, string termTag)
		{
			try
			{
                string cmdstring = "INSERT INTO [USTA].[dbo].[usta_CoursesTeachersCorrelation] ([teacherNo],[courseNo],[atCourseType],[termTag],[ClassID]) VALUES (@teacherNo,@courseNo,1,@termTag,@ClassID)";
				SqlParameter[] parameters = new SqlParameter[4]{
				new SqlParameter("@teacherNo",teacherNo),
				new SqlParameter("@courseNo",courseNo),
                new SqlParameter("@ClassID",ClassID),
                new SqlParameter("@termTag",termTag)
				
			};
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
        /// 
        /// </summary>
        /// <param name="No">教师/助教编号</param>
        /// <returns>数据集</returns>
        public String GetTeacherNoByAddressEmail(string email)
        {
            String teacherNo = null;
            string commandstring = "select teacherNo from dbo.usta_TeachersList where emailAddress=@emailAddress";
            SqlParameter[] parameters = new SqlParameter[1]{
                new SqlParameter("@emailAddress",email),				
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);

            teacherNo = ds.Tables[0].Rows[0]["teacherNo"].ToString().Trim();
            conn.Close();
            return teacherNo;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="No">教师/助教编号</param>
		/// <returns>数据集</returns>
		public DataSet GetCoursesByTeacherAssistant(string No)
		{
            string commandstring = "SELECT usta_Courses.ClassID,usta_Courses.termTag,usta_Courses.courseNo as courseNo,courseName, period,usta_CoursesTeachersCorrelation.teacherNo,atCourseType  FROM usta_CoursesTeachersCorrelation,usta_Courses WHERE usta_CoursesTeachersCorrelation.courseNo=usta_Courses.courseNo ";
            commandstring += " and usta_CoursesTeachersCorrelation.ClassID=usta_Courses.ClassID and usta_CoursesTeachersCorrelation.termTag=usta_Courses.termTag ";
            commandstring += " AND usta_CoursesTeachersCorrelation.teacherNo=@assistantNo AND usta_Courses.termTag=@termTag";
			SqlParameter[] parameters = new SqlParameter[2]{new SqlParameter("@assistantNo",No),
				 new SqlParameter("@termTag",termTag)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
			conn.Close();
			return ds;
		}

        /// <summary>
        /// 根据TermTag查询该学期有课程的教师/助教
        /// </summary>
        /// <param name="termTag"></param>
        /// <returns></returns>
        public DataSet GetTeacherByTermTag(string termTag) 
        {
            // TODO
            return null;
        }

        /// <summary>
        /// 根据teacherNo和courseNo查询当前学期该教师对此门课程的任课类型
        /// </summary>
        /// <returns></returns>
        public CoursesTeachersCorrelation GetCoursesTypeTeacher(string teacherNo, string courseNo, string classId) 
        {
            CoursesTeachersCorrelation ctCorrelation = null;
            if(string.IsNullOrWhiteSpace(teacherNo) || string.IsNullOrWhiteSpace(courseNo)){
                return ctCorrelation;
            }


            string commandString = "SELECT atCourseType FROM usta_CoursesTeachersCorrelation WHERE teacherNo = @teacherNo AND courseNo = @courseNo AND classID = @classId AND termTag = @termTag";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@teacherNo", teacherNo),
                new SqlParameter("@courseNo", courseNo),
                new SqlParameter("@termTag", termTag),
                new SqlParameter("@classId", classId)
            };

            IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
            if(reader.Read()){
                int atCourseType = int.Parse(reader["atCourseType"].ToString().Trim());
                ctCorrelation = new CoursesTeachersCorrelation()
                {
                    atCourseType = atCourseType
                };
            }

            return ctCorrelation;
        }


		#endregion
	}
}
