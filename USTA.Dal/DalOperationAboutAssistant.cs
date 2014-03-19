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
    /// 助教相关操作类
    /// </summary>
    public class DalOperationAboutAssistant
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
        public DalOperationAboutAssistant()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

            termTag = DalCommon.GetTermTag(conn);
        }
        #endregion


        #region

        /// <summary>
        /// 通过助教的编号获得助教的一个实体dataset
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <returns>助教信息数据集</returns>
        public DataSet GetAssistantById(string assistantNo)
        {
            string cmdstring = "SELECT [teacherNo],[teacherName] ,[emailAddress] ,[officeAddress]  ,[remark] FROM [USTA].[dbo].[usta_TeachersList] WHERE teacherNo=@assistantNo";
            SqlParameter[] parameters = new SqlParameter[1]{
               new SqlParameter("@assistantNo",assistantNo)
        };
            DataSet  ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 获取所有助教
        /// </summary>
        /// <returns>Dataset</returns>
        public DataSet GetAllAssitants()
        {
            string cmdstring = "SELECT teacherNo as assistantNo, teacherName as [assistantName]  ,[emailAddress] ,[officeAddress]  ,[remark] FROM [USTA].[dbo].[usta_AssistantsList] WHERE type='2'";
            
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring );
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 通过助教的编号获得助教的一个实体
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <returns>助教对象</returns>
        public AssistantsList GetAssistantbyId(string assistantNo)
        {
            AssistantsList assistant=null;
            string cmdstring = "SELECT teacherNo as assistantNo, teacherName as [assistantName] ,[emailAddress] ,[officeAddress]  ,[remark] FROM [USTA].[dbo].[usta_TeachersList] WHERE teacherNo=@assistantNo AND type='2'";
           SqlParameter[] parameters = new SqlParameter[1]{
               new SqlParameter("@assistantNo",assistantNo)
        };
            SqlDataReader dr=SqlHelper.ExecuteReader(conn,CommandType.Text,cmdstring,parameters);
            while(dr.Read()){
                assistant = new AssistantsList
                {
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
        /// 通过助教的编号获得助教辅助的课程
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <returns>课程数据集</returns>
        public DataSet GetCoursesByAssistantNo(string assistantNo)
        {

            string commandstring = "SELECT coursesTeachersCorrelationId,usta_Courses.courseNo,usta_Courses.ClassID,courseName,usta_CoursesTeachersCorrelation.teacherNo as assistantNo,usta_Courses.termTag FROM usta_CoursesTeachersCorrelation,usta_Courses WHERE usta_CoursesTeachersCorrelation.courseNo=usta_Courses.courseNo AND usta_CoursesTeachersCorrelation.teacherNo=@assistantNo AND atCourseType=2 AND usta_CoursesTeachersCorrelation.ClassID=usta_Courses.ClassID AND  usta_Courses.termTag=@termTag  AND usta_CoursesTeachersCorrelation.termTag=usta_Courses.termTag";
            SqlParameter[] parameters = new SqlParameter[2]{new SqlParameter("@assistantNo",assistantNo),
                 new SqlParameter("@termTag",termTag)
            };
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 通过助教的编号,以及学期编号获得助教辅助的课程
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <param name="termTags">学期标识</param>
        /// <returns>课程数据集</returns>
        public DataSet GetCoursesByAssistantNo(string assistantNo,string termTags)
        {

            string commandstring = "SELECT usta_Courses.courseNo,usta_Courses.ClassID,courseName,usta_CoursesTeachersCorrelation.teacherNo as assistantNo,usta_Courses.termTag FROM usta_CoursesTeachersCorrelation,usta_Courses WHERE usta_CoursesTeachersCorrelation.courseNo=usta_Courses.courseNo AND usta_CoursesTeachersCorrelation.teacherNo=@assistantNo AND atCourseType=2 AND usta_CoursesTeachersCorrelation.ClassID=usta_Courses.ClassID AND  usta_Courses.termTag=@termTag  AND usta_CoursesTeachersCorrelation.termTag=usta_Courses.termTag";
            SqlParameter[] parameters = new SqlParameter[2]{new SqlParameter("@assistantNo",assistantNo),
                 new SqlParameter("@termTag",termTags)
            };
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return ds;
        }

        
        

        /// <summary>
        /// 删除助教的任课
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <param name="courseNo">课程编号</param>
        public void DelCourseOfAssistant(string assistantNo, string courseNo)
        {
            try
            {
                string cmdstring = "DELETE FROM [USTA].[dbo].[usta_CoursesTeachersCorrelation] WHERE teacherNo=@assistantNo AND courseNo=@courseNo AND atCourseType='2'";
                SqlParameter[] parameters = new SqlParameter[2]{
                new SqlParameter("@assistantNo",assistantNo),
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
        /// 删除助教的任课
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <param name="courseNo">课程编号</param>
        public void DelCourseOfAssistantByCoursesTeachersCorrelationId(string coursesTeachersCorrelationId)
        {
            try
            {
                string cmdstring = "DELETE FROM [USTA].[dbo].[usta_CoursesTeachersCorrelation] WHERE  coursesTeachersCorrelationId=@coursesTeachersCorrelationId";
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
        /// 添加助教的任课
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <param name="courseNo">课程编号</param>
        public void AddCourseOfAssistant(string assistantNo, string courseNo, string ClassID, string termTag)
        {
            try
            {
                string cmdstring = "INSERT INTO [USTA].[dbo].[usta_CoursesTeachersCorrelation] ([teacherNo],[courseNo],[atCourseType],[termTag],[ClassID]) VALUES (@assistantNo,@courseNo,2,@termTag,@ClassID)";
                SqlParameter[] parameters = new SqlParameter[4]{
                new SqlParameter("@assistantNo",assistantNo),
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
        /// 通过助教的编号得到当前学期不属于他的所有其他课程
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <returns>课程数据集</returns>
        public DataSet GetOtherCoursesByAssistantNo(string assistantNo)
        {
            string commandstring = "SELECT courseNo,courseName,classID,termTag FROM usta_Courses WHERE (courseNo+termTag+classID) NOT IN(	SELECT (courseNo+termTag+classID) FROM usta_CoursesTeachersCorrelation	WHERE teacherNo=@assistantNo) AND termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[2]{new SqlParameter("@assistantNo",assistantNo),
                 new SqlParameter("@termTag",termTag)
            };
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return ds;
        }
       /// <summary>
        /// 通过助教的编号得到某学期不属于他的所有其他课程
       /// </summary>
        /// <param name="assistantNo">助教编号</param>
       /// <param name="termTags">学期标识</param>
       /// <returns>课程数据集</returns>
       
        public DataSet GetOtherCoursesByAssistantNo(string assistantNo, string termTags)
        {
            string commandstring = "SELECT courseNo,courseName FROM usta_Courses WHERE courseNo NOT IN(	SELECT courseNo FROM usta_CoursesTeachersCorrelation	WHERE teacherNo=@assistantNo AND atCourseType=2) AND termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[2]{new SqlParameter("@assistantNo",assistantNo),
                 new SqlParameter("@termTag",termTags)
            };
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return ds;
        }
        #endregion

        
    }
}
