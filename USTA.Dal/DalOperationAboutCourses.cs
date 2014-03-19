using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

using USTA.Model;
using USTA.Common;
using System.Threading.Tasks;
using System.Transactions;

namespace USTA.Dal
{
    /// <summary>
    /// 课程相关类
    /// </summary>
    public class DalOperationAboutCourses
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
        public DalOperationAboutCourses()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

            termTag = DalCommon.GetTermTag(conn);
        }
        #endregion

        /// <summary>
        /// 通过课程号得到课程的基本信息 返回一个课程实例
        /// </summary>
        /// <param name="CourseNo"></param>
        /// <returns></returns>
        public Courses GetCoursesByNo(String CourseNo, string termTag) 
        {
            Courses course = null;
            string commandString = "SELECT [courseNo] ,[courseName],[period] ,[credit] ,[courseSpeciality],[preCourse],[refferenceBooks] ,[termTag] ,[attachmentIds] ,[homePage],[courseAnswer] ,[teacherResume] ,[courseIntroduction],[examineMethod] ,[lessonTimeAndAddress],teachingPlan,bbsEmailAddress, [testHours] FROM [USTA].[dbo].[usta_Courses] WHERE CourseNo=@CourseNo AND termTag = @termTag";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@CourseNo",CourseNo));
            if(string.IsNullOrWhiteSpace(termTag)){
                termTag = DalCommon.GetTermTag(conn);
            }
            else{
                termTag = termTag.Trim();
            }
            parameters.Add(new SqlParameter("@termTag", termTag));

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters.ToArray());

            while (dr.Read())
            {
                course = new Courses
                {
                    courseNo = dr["courseNo"].ToString().Trim(),
                    courseName = dr["courseName"].ToString().Trim(),
                    period = dr["period"].ToString().Trim(),
                    credit = (dr["credit"].ToString().Trim().Length > 0) ? float.Parse(dr["credit"].ToString().Trim()) : 0,
                    courseSpeciality = dr["courseSpeciality"].ToString().Trim(),
                    preCourse = dr["preCourse"].ToString().Trim(),
                    referenceBooks = dr["refferenceBooks"].ToString().Trim(),
                    attachmentIds = dr["attachmentIds"].ToString().Trim(),
                    termTag = dr["termTag"].ToString().Trim()
                };

                course.homePage = (dr["homePage"].ToString().Trim().Length == 0) ? "未添加" : dr["homePage"].ToString().Trim();

                course.teacherResume = (dr["teacherResume"].ToString().Trim().Length == 0) ? "未添加" : dr["teacherResume"].ToString().Trim();

                course.courseAnswer = (dr["courseAnswer"].ToString().Trim().Length == 0) ? "未添加" : dr["courseAnswer"].ToString().Trim();

                course.courseIntroduction = (dr["courseIntroduction"].ToString().Trim().Length == 0) ? "未添加" : dr["courseIntroduction"].ToString().Trim();

                course.examineMethod = (dr["examineMethod"].ToString().Trim().Length == 0) ? "未添加" : dr["examineMethod"].ToString().Trim();

                course.lessonTimeAndAddress = (dr["lessonTimeAndAddress"].ToString().Trim().Length == 0) ? "未添加" : dr["lessonTimeAndAddress"].ToString().Trim();

                course.teachingPlan = (dr["teachingPlan"].ToString().Trim().Length == 0) ? "未添加" : dr["teachingPlan"].ToString().Trim();

                course.bbsEmaiAddress = (dr["bbsEmailAddress"].ToString().Trim().Length == 0) ? "未添加" : dr["bbsEmailAddress"].ToString().Trim();

                course.TestHours = "0";

                if (dr["testHours"] != null)
                {
                    course.TestHours = dr["testHours"].ToString().Trim();
                }
            }

            dr.Close();
            conn.Close();
            return course;
        
        }

        #region
        /// <summary>
        /// 通过课程号得到课程的基本信息 返回一个课程实例
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <returns>课程实体</returns>
        public Courses GetCoursesByNo(string CourseNo,string classId,string termtag)
        {
            Courses course = new Courses();
            string commandstring = "SELECT [courseNo] ,[courseName],[period] ,[credit] ,[courseSpeciality],[preCourse],[refferenceBooks] ,[termTag] ,[attachmentIds] ,[homePage],[courseAnswer] ,[teacherResume] ,[courseIntroduction],[examineMethod] ,[lessonTimeAndAddress],teachingPlan,bbsEmailAddress,TestHours FROM [USTA].[dbo].[usta_Courses] WHERE CourseNo=@CourseNo AND classID=@classId AND termTag=@termtag";
            SqlParameter[] parameters = new SqlParameter[3]{
                new SqlParameter("@CourseNo",CourseNo),
                new SqlParameter("@classId",classId),
                 new SqlParameter("@termtag",termtag)
			};

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);


            while (dr.Read())
            {
                course = new Courses
                {
                    courseNo = dr["courseNo"].ToString().Trim(),
                    courseName = dr["courseName"].ToString().Trim(),
                    period = dr["period"].ToString().Trim(),
                    credit = (dr["credit"].ToString().Trim().Length > 0) ? float.Parse(dr["credit"].ToString().Trim()) : 0,
                    courseSpeciality = dr["courseSpeciality"].ToString().Trim(),
                    preCourse = dr["preCourse"].ToString().Trim(),
                    referenceBooks = dr["refferenceBooks"].ToString().Trim(),
                    attachmentIds = dr["attachmentIds"].ToString().Trim(),
                    termTag = dr["termTag"].ToString().Trim()
                };

                course.homePage = (dr["homePage"].ToString().Trim().Length == 0) ? "未添加" : dr["homePage"].ToString().Trim();

                course.teacherResume = (dr["teacherResume"].ToString().Trim().Length == 0) ? "未添加" : dr["teacherResume"].ToString().Trim();

                course.courseAnswer = (dr["courseAnswer"].ToString().Trim().Length == 0) ? "未添加" : dr["courseAnswer"].ToString().Trim();

                course.courseIntroduction = (dr["courseIntroduction"].ToString().Trim().Length == 0) ? "未添加" : dr["courseIntroduction"].ToString().Trim();

                course.examineMethod = (dr["examineMethod"].ToString().Trim().Length == 0) ? "未添加" : dr["examineMethod"].ToString().Trim();

                course.lessonTimeAndAddress = (dr["lessonTimeAndAddress"].ToString().Trim().Length == 0) ? "未添加" : dr["lessonTimeAndAddress"].ToString().Trim();

                course.teachingPlan = (dr["teachingPlan"].ToString().Trim().Length == 0) ? "未添加" : dr["teachingPlan"].ToString().Trim();

                course.bbsEmaiAddress = (dr["bbsEmailAddress"].ToString().Trim().Length == 0) ? "未添加" : dr["bbsEmailAddress"].ToString().Trim();

                course.TestHours = "0";

                if (dr["testHours"] != null)
                {
                    course.TestHours = dr["testHours"].ToString().Trim();
                }
            }

            dr.Close();
            conn.Close();
            return course;
        }

        /// <summary>
        /// 通过课程编号得到课程 返回Dataset
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <returns>Dataset</returns>
        public DataSet GetCoursesByCourseNo(string CourseNo, string classID, string termtag)
        {

            string commandstring = "SELECT [courseNo] ,[courseName],[period],[ClassID] ,[credit] ,[courseSpeciality],[preCourse],[refferenceBooks] ,[termTag] ,[attachmentIds] ,[homePage],[courseAnswer] ,[teacherResume] ,[courseIntroduction],[examineMethod] ,[lessonTimeAndAddress],teachingPlan,bbsEmailAddress FROM [USTA].[dbo].[usta_Courses] WHERE CourseNo=@CourseNo AND ClassID=@classID AND termTag=@termTag;";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@CourseNo",CourseNo),
                new SqlParameter("@classID",classID),
                new SqlParameter("@termtag",termtag)
			};

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);


            conn.Close();
            return ds;
        }

        /// <summary>
        /// 通过课程集成编号获得课程编号，同时添加该课程的某学生关注
        /// </summary>
        /// <param name="termTagCourseNoClassID"></param>
        /// <returns></returns>
        public DataSet GetCoursesByTermTagCourseNoClassID(string termTagCourseNoClassID)
        {

            string commandstring = "SELECT [courseNo],[ClassID],[courseName],[period] ,[credit] ,[courseSpeciality],[preCourse],[refferenceBooks] ,[termTag],[TestHours] ,[attachmentIds] ,[homePage],[courseAnswer] ,[teacherResume] ,[courseIntroduction],[examineMethod] ,[lessonTimeAndAddress],teachingPlan,bbsEmailAddress FROM [USTA].[dbo].[usta_Courses] WHERE (RTRIM(termTag)+RTRIM(courseNo)+RTRIM(ClassID))=@termTagCourseNoClassID;";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@termTagCourseNoClassID",termTagCourseNoClassID)
			};


            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);

            conn.Close();
            return ds;
        }
         
        
        /// <summary>
        ///  通过课程编号获得课程编号，同时添加该课程的某学生关注
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <param name="StudentNo">学号</param>
        /// <returns>数据集</returns>
        public DataSet GetCoursesByCourseNo(string CourseNo, string classID, string termtag, string StudentNo)
        {

            string commandstring = "SELECT [courseNo] ,[courseName],[period] ,[credit] ,[courseSpeciality],[preCourse],[refferenceBooks] ,[termTag] ,[attachmentIds] ,[homePage],[courseAnswer] ,[teacherResume] ,[courseIntroduction],[examineMethod] ,[lessonTimeAndAddress],teachingPlan,bbsEmailAddress FROM [USTA].[dbo].[usta_Courses] WHERE CourseNo=@CourseNo AND ClassID=@classID AND termTag=@termTag;";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@CourseNo",CourseNo),
                new SqlParameter("@classID",classID),
                new SqlParameter("@termtag",termtag)
			};


            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);

            conn.Close();
            return ds;
        }
        /// <summary>
        /// 获得课程基本信息，教师，以及助教
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <returns>数据集</returns>
        public DataSet GetCourseInfoTeacherAndAssistant(string courseNo,string classID,string termtag)
        {
            string commandstring = "SELECT [courseNo] ,[courseName],[period] ,[credit] ,[courseSpeciality],[preCourse],[refferenceBooks] ,[termTag] ,[attachmentIds] ,[homePage],[courseAnswer] ,[teacherResume] ,[courseIntroduction],[examineMethod] ,[lessonTimeAndAddress],teachingPlan,bbsEmailAddress FROM [USTA].[dbo].[usta_Courses] WHERE CourseNo=@CourseNo AND ClassID=@ClassID AND termTag=@termTag;";
            commandstring += "SELECT usta_TeachersList.teacherNo,emailAddress,teacherName FROM usta_CoursesTeachersCorrelation,usta_TeachersList WHERE CourseNo=@CourseNo AND ClassID=@ClassID AND termTag=@termTag AND usta_CoursesTeachersCorrelation.teacherNo=usta_TeachersList.teacherNo AND atCourseType=1;";
            commandstring += "SELECT usta_TeachersList.teacherNo as assistantNo,emailAddress,teacherName as assistantName FROM usta_CoursesTeachersCorrelation,usta_TeachersList WHERE CourseNo=@CourseNo AND ClassID=@ClassID AND termTag=@termTag AND usta_CoursesTeachersCorrelation.teacherNo=usta_TeachersList.teacherNo AND atCourseType=2;";
            string[] tableNames = { "0", "1", "2" };
            SqlParameter[] parameters = new SqlParameter[3]{
                new SqlParameter("@CourseNo",courseNo),
                new SqlParameter("@ClassID",classID),
                new SqlParameter("@termTag",termtag)
			};
            DataSet ds = new DataSet();
            SqlHelper.FillDataset(conn, CommandType.Text, commandstring, ds, tableNames, parameters);
            conn.Close();



            return ds;
        }

        /// <summary>
        /// 通过课程号获得课程，同时更新课程关注
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <param name="StudentNo">学号</param>
        /// <returns>课程实体</returns>
        public Courses GetCoursesByNo(string CourseNo, string classID, string termtag, string StudentNo)
        {
            Courses course = null;
            string commandstring = "SELECT [courseNo] ,[courseName],[period] ,[credit] ,[courseSpeciality],[preCourse],[refferenceBooks] ,[termTag] ,[attachmentIds] ,[homePage],[courseAnswer] ,[teacherResume] ,[courseIntroduction],[examineMethod] ,[lessonTimeAndAddress],teachingPlan,bbsEmailAddress FROM [USTA].[dbo].[usta_Courses] WHERE courseNo=@courseNo";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@courseNo",CourseNo)
			};

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);
            if (dr.Read())
            {
                course = new Courses
                {
                    courseNo = dr["courseNo"].ToString().Trim(),
                    courseName = dr["courseName"].ToString().Trim(),
                    period = dr["period"].ToString().Trim(),
                    credit =(dr["credit"].ToString().Trim().Length>0 ?float.Parse(dr["credit"].ToString().Trim()):0),
                    courseSpeciality = dr["courseSpeciality"].ToString().Trim(),
                    preCourse = dr["preCourse"].ToString().Trim(),
                    referenceBooks = dr["refferenceBooks"].ToString().Trim(),
                    attachmentIds = dr["attachmentIds"].ToString().Trim(),
                    termTag = dr["termTag"].ToString().Trim()
                };


                course.homePage = (dr["homePage"].ToString().Trim().Length == 0) ? "未添加" : dr["homePage"].ToString().Trim();

                course.teacherResume = (dr["teacherResume"].ToString().Trim().Length == 0) ? "未添加" : dr["teacherResume"].ToString().Trim();

                course.courseAnswer = (dr["courseAnswer"].ToString().Trim().Length == 0) ? "未添加" : dr["courseAnswer"].ToString().Trim();

                course.courseIntroduction = (dr["courseIntroduction"].ToString().Trim().Length == 0) ? "未添加" : dr["courseIntroduction"].ToString().Trim();

                course.examineMethod = (dr["examineMethod"].ToString().Trim().Length == 0) ? "未添加" : dr["examineMethod"].ToString().Trim();

                course.lessonTimeAndAddress = (dr["lessonTimeAndAddress"].ToString().Trim().Length == 0) ? "未添加" : dr["lessonTimeAndAddress"].ToString().Trim();

                course.teachingPlan = (dr["teachingPlan"].ToString().Trim().Length == 0) ? "未添加" : dr["teachingPlan"].ToString().Trim();

                course.bbsEmaiAddress = (dr["bbsEmailAddress"].ToString().Trim().Length == 0) ? "未添加" : dr["bbsEmailAddress"].ToString().Trim();
            }
            dr.Close();

            conn.Close();
            return course;
        }

        /// <summary>
        /// 更新课程关注信息
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="studentNo">学号</param>
        public void UpdateCoursesAttention(string courseNo,string classID,string termTag, string studentNo)
        {
            try
            {
                string getAttention = "SELECT [courseAttentionId],[studentNo],[courseNo] FROM [USTA].[dbo].[usta_CoursesAttention] WHERE courseNo=@courseNo AND classID=@classID AND termTag=@termTag AND studentNo=@studentNo";

                SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@classID", SqlDbType.NVarChar,50),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)};
                parameters[0].Value = studentNo;
                parameters[1].Value = courseNo;
                parameters[2].Value = classID;
                parameters[3].Value = termTag;
                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, getAttention, parameters);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    string insertAttention = "INSERT INTO [USTA].[dbo].[usta_CoursesAttention]([studentNo],[courseNo],[classID],[termTag]) VALUES (@studentNo ,@courseNo,@classID,@termTag)";
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, insertAttention, parameters);
                }
                ds.Dispose();
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
        /// 通过课程号获得与该课程的相关信息
        /// </summary>
        /// <param name="studentNo">学号</param>
        /// <param name="courseNo">课程编号</param>
        /// <param name="fragmentFlag">标签显示编号</param>
        /// <returns>数据集</returns>
        public DataSet GetCoursesInfo(string studentNo, string courseNo,string classId,string termtag, string fragmentFlag)
        {
            //sqlString
            string commandstring = string.Empty;

            SqlParameter[] parameters = new SqlParameter[3] { 
                new SqlParameter("@CourseNo", courseNo),
            new SqlParameter("@classId", classId),
            new SqlParameter("@termtag", termtag)};

            DataSet ds = new DataSet();

            switch (fragmentFlag)
            {
                //提升性能，避免不必要的加载
                case "1":
                case "2":
                    string[] tableNames = { "0", "1" };
                    commandstring = "SELECT usta_TeachersList.teacherNo,teacherName FROM usta_CoursesTeachersCorrelation,usta_TeachersList WHERE CourseNo=@CourseNo AND classID=@classId AND termTag=@termtag AND usta_CoursesTeachersCorrelation.teacherNo=usta_TeachersList.teacherNo AND atCourseType='1';";
                    commandstring += "SELECT usta_TeachersList.teacherNo as assistantNo, teacherName as assistantName FROM usta_CoursesTeachersCorrelation,usta_TeachersList WHERE CourseNo=@CourseNo AND classID=@classId AND termTag=@termtag AND usta_CoursesTeachersCorrelation.teacherNo=usta_TeachersList.teacherNo AND  atCourseType='2';";
                    SqlHelper.FillDataset(conn, CommandType.Text, commandstring, ds, tableNames, parameters);
                    break;
                case "3":
                    commandstring = "SELECT courseNotifyInfoId,courseNotifyInfoTitle,updateTime,isTop FROM usta_CoursesNotifyInfo WHERE courseNo=@CourseNo AND classID=@classId AND termTag=@termtag ORDER BY isTop DESC,updateTime DESC;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "4":
                case "5":
                    commandstring = "SELECT courseResourceId,courseResourceTitle,attachmentIds,updateTime FROM usta_CourseResources WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag order by updateTime desc;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "6":
                    commandstring = "SELECT experimentResourceId,experimentResourceTitle,updateTime FROM usta_ExperimentResources WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag order by updateTime desc;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "7":
                    commandstring = "SELECT [schoolWorkNotifyId],[schoolWorkNotifyTitle],updateTime FROM [USTA].[dbo].[usta_SchoolWorkNotify] WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag order by updateTime desc;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "8":
                    //commandstring = "SELECT  [courseCommentId] ,[courseNo],[courseCommentContent] ,[courseCommentUserName],[updateTime]  FROM [USTA].[dbo].[usta_CourseComments] WHERE CourseNo=@CourseNo ORDER BY updateTime DESC;";
                    //ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                default:
                    break;
            }
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 通过课程号获得与该课程的相关信息
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="fragmentFlag">标签显示编号</param>
        /// <returns>数据集</returns>
        public DataSet GetCoursesInfo(string courseNo,string classId,string termtag, string fragmentFlag)
        {
            //sqlString
            string commandstring = string.Empty;

            SqlParameter[] parameters = new SqlParameter[3] { new SqlParameter("@CourseNo", courseNo),
             new SqlParameter("@classId", classId),
             new SqlParameter("@termtag", termtag),};

            DataSet ds = new DataSet();

            switch (fragmentFlag)
            {
                //提升性能，避免不必要的加载
                case "1":
                case "2":
                    string[] tableNames = { "0", "1" };
                    commandstring = "SELECT usta_TeachersList.teacherNo,teacherName FROM usta_CoursesTeachersCorrelation,usta_TeachersList WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag AND usta_CoursesTeachersCorrelation.teacherNo=usta_TeachersList.teacherNo AND atCourseType='1'  ;";
                    commandstring += "SELECT usta_TeachersList.teacherNo as assistantNo,teacherName as assistantName FROM usta_CoursesTeachersCorrelation,usta_TeachersList WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag AND usta_CoursesTeachersCorrelation.teacherNo=usta_TeachersList.teacherNo AND atCourseType='2';";
                    SqlHelper.FillDataset(conn, CommandType.Text, commandstring, ds, tableNames, parameters);
                    break;
                case "3":
                    commandstring = "SELECT courseNotifyInfoId,courseNotifyInfoTitle,updateTime,isTop,classID,termTag FROM usta_CoursesNotifyInfo WHERE courseNo=@CourseNo AND termTag=@termtag AND classID=@classId ORDER BY isTop DESC,updateTime DESC;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "4":
                case "5":
                    commandstring = "SELECT courseResourceId,courseResourceTitle,attachmentIds,updateTime,classID,termTag FROM usta_CourseResources WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag order by updateTime desc;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "6":
                    commandstring = "SELECT experimentResourceId,experimentResourceTitle,updateTime,classID,termTag FROM usta_ExperimentResources WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag order by updateTime desc;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "7":
                    commandstring = "SELECT [schoolWorkNotifyId],[schoolWorkNotifyTitle],updateTime,classID,termTag FROM [USTA].[dbo].[usta_SchoolWorkNotify] WHERE CourseNo=@CourseNo  AND classID=@classId AND termTag=@termtag order by updateTime desc;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                case "8":
                    //commandstring = "SELECT  [courseCommentId] ,[courseNo],[courseCommentContent] ,[courseCommentUserName],[updateTime]  FROM [USTA].[dbo].[usta_CourseComments] WHERE CourseNo=@CourseNo ORDER BY updateTime DESC;";
                    //ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                    break;
                default:
                    break;
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 通过课程编号获得该课程的教师和助教
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <returns>数据集</returns>
        public DataSet GetTeacherAndAssitant(string courseNo)
        {
            string commandstring = string.Empty;
            DataSet ds = new DataSet();
            string[] tableNames = { "0", "1" };
            commandstring = "SELECT usta_TeachersList.teacherNo,teacherName FROM usta_CoursesTeachersCorrelation,usta_TeachersList WHERE CourseNo=@CourseNo AND usta_CoursesTeachersCorrelation.teacherNo=usta_TeachersList.teacherNo;";
            commandstring += "SELECT usta_AssistantsList.assistantNo,assistantName FROM usta_CoursesAssistantsCorrelation,usta_AssistantsList WHERE CourseNo=@CourseNo AND usta_CoursesAssistantsCorrelation.assistantNo=usta_AssistantsList.assistantNo;";

            SqlParameter[] parameters = new SqlParameter[1] { new SqlParameter("@CourseNo", courseNo) };
            SqlHelper.FillDataset(conn, CommandType.Text, commandstring, ds, tableNames, parameters);
            conn.Close();
            return ds;

        }

        /// <summary>
        /// 通过课程集成主键获得任课教师列表
        /// </summary>
        /// <param name="termTagCourseNoClassID">课程编号</param>
        /// <returns>数据集</returns>
        public DataSet GetTeachersByTermTagCourseNoClassID(string termTagCourseNoClassID)
        {
            string commandstring = "SELECT teacherName FROM usta_CoursesTeachersCorrelation A,usta_TeachersList B WHERE (RTRIM(A.termTag)+RTRIM(A.courseNo)+RTRIM(A.ClassID))=@termTagCourseNoClassID AND A.teacherNo=B.teacherNo AND atCourseType=1;";
            SqlParameter[] parameters = new SqlParameter[]{new SqlParameter("@termTagCourseNoClassID",termTagCourseNoClassID)
			};
            DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return dr;
        }
        /// <summary>
        /// 通过课程号得到课程的助教
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <returns>数据集</returns>
        public DataSet GetAssistantByCourseNo(string CourseNo)
        {

            string commandstring = "SELECT usta_AssistantsList.assistantNo,assistantName FROM usta_CoursesAssistantsCorrelation,usta_AssistantsList WHERE CourseNo=@CourseNo AND usta_CoursesAssistantsCorrelation.assistantNo=usta_AssistantsList.assistantNo";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@CourseNo",CourseNo)
			};
            DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return dr;

        }
        /// <summary>
        /// 通过课程号得到课程的作业
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <returns>课程作业的数据集</returns>
        public DataSet GetNotifyByCourseNo(string CourseNo)
        {

            string commandstring = "SELECT courseNotifyInfoId,courseNotifyInfoTitle FROM usta_CoursesNotifyInfo WHERE courseNo=@CourseNo";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@CourseNo",CourseNo)
			};
            DataSet dr = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return dr;
        }
        /// <summary>
        /// 通过课程号得到与课程相关的资源
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <returns>课程资源数据集</returns>
        public DataSet GetResoursesByCourseNo(string CourseNo)
        {

            string commandstring = "SELECT courseResourceId,courseResourceTitle FROM usta_CourseResources WHERE CourseNo=@CourseNo";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@CourseNo",CourseNo)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 通过课程编号获得该课程布置的实验
        /// </summary>
        /// <param name="CourseNo">课程编号</param>
        /// <returns>课程实验资源数据集</returns>
        public DataSet GetExperimentResourcesByCourseNo(string CourseNo)
        {

            string commandstring = "SELECT experimentResourceId,experimentResourceTitle FROM usta_ExperimentResources WHERE CourseNo=@CourseNo";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@CourseNo",CourseNo)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 更新某个课程
        /// </summary>
        /// <param name="courses">课程对象</param>
        public void UpdateCourses(Courses courses)
        {
            try
            {
                string commandstring = "UPDATE usta_Courses SET preCourse =@PreCourse,refferenceBooks =@RefferenceBooks,attachmentIds = @attachmentIds,[homePage] = @homePage,[examineMethod] = @examineMethod ,[courseAnswer]=@courseAnswer WHERE courseNo=@CourseNo AND classID=@classId AND termTag=@termtag";

                SqlParameter[] parameters = {
					new SqlParameter("@CourseNo", SqlDbType.NChar,20),
					
					new SqlParameter("@PreCourse", SqlDbType.NVarChar,500),
					new SqlParameter("@RefferenceBooks", SqlDbType.NVarChar,1000),
					
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@homePage", SqlDbType.NVarChar,200),
					new SqlParameter("@courseAnswer", SqlDbType.NVarChar,4000),
	
					new SqlParameter("@examineMethod", SqlDbType.NVarChar,4000),
                    new SqlParameter("@classId", SqlDbType.NVarChar,100),
                    new SqlParameter("@termtag", SqlDbType.NVarChar,100),
                           };
                parameters[0].Value = courses.courseNo;
                parameters[1].Value = courses.preCourse;
                parameters[2].Value = courses.referenceBooks;
                parameters[3].Value = courses.attachmentIds;
                parameters[4].Value = courses.homePage;
                parameters[5].Value = courses.courseAnswer;
                parameters[6].Value = courses.examineMethod;
                parameters[7].Value = courses.classID;
                parameters[8].Value = courses.termTag;

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandstring, parameters);
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
        /// 更新课程bbs邮箱
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="email">邮件地址</param>
        public void UpdateCoursesBBsEmail(string courseNo,string classId,string termtag, string email)
        {
            try
            {
                string commandstring = "UPDATE usta_Courses SET [bbsEmailAddress]=@bbsEmail WHERE courseNo=@CourseNo";
                SqlParameter[] parameters = new SqlParameter[4]{new SqlParameter("@CourseNo", SqlDbType.NChar,20),
			   new SqlParameter("@bbsEmail", SqlDbType.NVarChar,500),
                 new SqlParameter("@classId", SqlDbType.NVarChar,50), 
			     new SqlParameter("@termtag", SqlDbType.NVarChar,50)
				};
                parameters[0].Value = courseNo;
                parameters[1].Value = email;
                parameters[2].Value = classId;
                parameters[3].Value = termtag;
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandstring, parameters);
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
        /// 查找课程bbs邮箱
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        public string GetCoursesBBsEmail(string courseNo)
        {

            string commandstring = "select bbsEmailAddress from usta_Courses  WHERE courseNo=@CourseNo";
                SqlParameter[] parameters = new SqlParameter[1]{
                    new SqlParameter("@CourseNo", SqlDbType.NChar,20)
			   
				};
                parameters[0].Value = courseNo;
                 
                DataSet ds= SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
                string result = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0]["bbsEmailAddress"].ToString();
                }
                return result;
           
        }
   
        /// <summary>
        /// 添加课程信息
        /// </summary>
        /// <param name="course">课程对象</param>
        public void AddCourse(Courses course)
        {
            try
            {
                string sql = "INSERT INTO usta_Courses(courseNo,courseName,period,credit,courseSpeciality,termTag) VALUES(@CourseNo,@courseName,@period,@credit,@courseSpeciality,@termTag)";

                SqlParameter[] parameters = {
					new SqlParameter("@CourseNo", SqlDbType.NChar,20),
					new SqlParameter("@courseName", SqlDbType.NChar,50),
					new SqlParameter("@period", SqlDbType.NChar,50),
					new SqlParameter("@credit", SqlDbType.Float,8),
					new SqlParameter("@courseSpeciality", SqlDbType.NChar,50),				
					new SqlParameter("@termTag", SqlDbType.NVarChar,50),
					};
                parameters[0].Value = course.courseNo;
                parameters[1].Value = course.courseName;
                parameters[2].Value = course.period;
                parameters[3].Value = course.credit;
                parameters[4].Value = course.courseSpeciality;
                parameters[5].Value = course.termTag;

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
        /// 查询所有课程信息
        /// </summary>
        /// <returns>课程信息数据集</returns>
        public DataSet FindAllCourse()
        {
            DataSet ds = null;
            string sql = "SELECT courseNo,courseName,period,credit,courseSpeciality,termTag FROM usta_Courses";
            ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
            conn.Close();
            return ds;
        }
        
        /// <summary>
        /// 删除课程信息
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        public void DeleteCourseByNo(string courseNo,string termTag,string ClassID)
        {
            try
            {
                string sql = "DELETE FROM usta_CoursesTeachersCorrelation WHERE courseNo=@CourseNo;DELETE FROM usta_CoursesStudentsCorrelation WHERE courseNo=@CourseNo AND termTag=@termTag AND ClassID=@ClassID;DELETE FROM usta_Courses WHERE courseNo=@CourseNo AND termTag=@termTag AND ClassID=@ClassID";
                SqlParameter[] parameters = new SqlParameter[]{
			   new SqlParameter("@CourseNo",courseNo),
			   new SqlParameter("@termTag",termTag),
			   new SqlParameter("@ClassID",ClassID)
                };
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
        /// 更新课程信息
        /// </summary>
        /// <param name="course">课程实体</param>
        public void UpdateCourseByCourse(Courses course)
        {
            try
            {
                string sql = "UPDATE usta_Courses SET courseName=@courseName, period=@period, credit=@credit, courseSpeciality=@courseSpeciality, termTag=@termTag  WHERE courseNo=@CourseNo";

                SqlParameter[] parameters = {
					new SqlParameter("@CourseNo", SqlDbType.NChar,20),
					new SqlParameter("@courseName", SqlDbType.NChar,50),
					new SqlParameter("@period", SqlDbType.NChar,50),
					new SqlParameter("@credit", SqlDbType.Float,8),
					new SqlParameter("@courseSpeciality", SqlDbType.NChar,50),				
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)
					};
                parameters[0].Value = course.courseNo;
                parameters[1].Value = course.courseName;
                parameters[2].Value = course.period;
                parameters[3].Value = course.credit;
                parameters[4].Value = course.courseSpeciality;
                parameters[5].Value = course.termTag;
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
        /// 按照课程编号查询课程
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <returns>课程对象</returns>
        public Courses FindCourseByNo(string courseNo,string classID,string termTag)
        {
            Courses course = null;
            string sql = "SELECT courseNo,courseName,period,credit,courseSpeciality,termTag FROM usta_Courses where courseNo=@courseNo AND classID=@classID AND termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[3]{
			   new SqlParameter("@courseNo",courseNo),
			   new SqlParameter("@classID",classID),
			   new SqlParameter("@termTag",termTag)
			};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);

            while (dr.Read())
            {
                course = new Courses();
                course.courseNo = dr["courseNo"].ToString().Trim();
                course.courseName = dr["courseName"].ToString().Trim();
                course.period = dr["period"].ToString().Trim();
                course.credit = (dr["credit"].ToString().Trim().Length > 0 ? float.Parse(dr["credit"].ToString()) : 0);
                course.courseSpeciality = dr["courseSpeciality"].ToString().Trim();
                course.termTag = dr["termTag"].ToString();
            }

            dr.Close();
            conn.Close();
            return course;
        }
        /// <summary>
        /// 根据学期标识查询课程
        /// </summary>
        /// <param name="termTag">学期标识</param>
        /// <returns>课程信息数据集</returns>
        public DataSet FindCourseByTermTage(string termTag)
        {
            DataSet ds = null;
            string sql = "SELECT courseNo,ClassID,courseName,period,credit,courseSpeciality,termTag,classID FROM usta_Courses WHERE termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[1]{
			   new SqlParameter("@termTag",termTag)
			};
            ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 获得当前学期的所有课程
        /// </summary>
        /// <returns>当前学期课程数据集</returns>
        public DataSet FindCurrentCourses()
        {
            DataSet ds = null;
            string sql = "SELECT courseNo,courseName,termTag,classID FROM usta_Courses WHERE termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[1]{
			   new SqlParameter("@termTag",termTag)
			};
            ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }


        /// <summary>
        /// 获得当前学期的所有课程
        /// </summary>
        /// <param name="locale">培养地</param>
        /// <returns></returns>
        public DataSet FindCurrentCoursesByLocale(string locale)
        {
            DataSet ds = new DataSet();

            string sql = "SELECT courseNo,courseName,termTag,classID FROM usta_Courses WHERE termTag=@termTag AND CHARINDEX(@locale,classID)>0";


            SqlParameter[] parameters = new SqlParameter[]{
			   new SqlParameter("@termTag",termTag),
			   new SqlParameter("@locale",locale)
			};

            if (termTag.Substring(5, 1) == "0")
            {
                sql = "SELECT courseNo,courseName,termTag,classID FROM usta_Courses WHERE termTag=@termTag";

                parameters = new SqlParameter[]{
			        new SqlParameter("@termTag",termTag)
			    };
            }

            ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 统计各个专业的课程关注人数信息
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <returns>哈希表</returns>
        public Hashtable CalculateCourseAttentionNumber(string courseNo,string classID,string termTag)
        {
            DalOperationStudentSpecility dalspeciality = new DalOperationStudentSpecility();
            DataTable dtSpeciality = dalspeciality.FindAllStudentSpecilitye().Tables[0];

            int dtSpecialityCount = dtSpeciality.Rows.Count;

            Hashtable ht = new Hashtable(dtSpecialityCount);
            
            Parallel.For(0, dtSpecialityCount, delegate(int i)
            {
                ht.Add(dtSpeciality.Rows[i]["MajorTypeID"].ToString().Trim(), 0);
            });


            string sql = "SELECT [courseAttentionId],usta_StudentsList.[studentNo],[courseNo],MajorType ";
            sql += "FROM [usta_CoursesAttention], usta_StudentsList ";
            sql += "where [usta_CoursesAttention].studentNo=usta_StudentsList.studentNo ";
            sql += "and courseNo=@courseNo AND classID=@classID AND termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[3] {
                new SqlParameter("@courseNo", courseNo) ,
                new SqlParameter("@classID", classID) ,
                new SqlParameter("@termTag", termTag) 
            };
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            DataTable dt = ds.Tables[0];
            string speciality = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                speciality = dt.Rows[i]["MajorType"].ToString().Trim();
                ht[speciality] = int.Parse(ht[speciality].ToString().Trim()) + 1;
            }
            conn.Close();
            return ht;
        }

        /// <summary>
        /// 判断教师时候任这门课
        /// </summary>
        /// <param name="teacherNo">教师编号</param>
        /// <param name="courseNo">课程编号</param>
        /// <returns>布尔值</returns>
        public bool IsTeacherAtCourse(string teacherNo, string courseNo,string classID,string termTag)
        {
            string commandstring = "SELECT [teacherNo],[courseNo]  FROM [USTA].[dbo].[usta_CoursesTeachersCorrelation] WHERE teacherNo=@teacherNo AND courseNo=@courseNo AND classID=@classID AND termTag=@termTag";
            SqlParameter[] parameters = {
											new SqlParameter("@teacherNo",teacherNo),
											new SqlParameter("@courseNo",courseNo),
											new SqlParameter("@classID",classID),
											new SqlParameter("@termTag",termTag)
										};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);

            bool isTeacherAtCourse = false;

            while (dr.Read())
            {
                isTeacherAtCourse = true;
            }
            dr.Close();
            conn.Close();
            return isTeacherAtCourse;
        }

        /// <summary>
        /// 助教是否任这门课
        /// </summary>
        /// <param name="assistantNo">助教编号</param>
        /// <param name="courseNo">课程编号</param>
        /// <returns>布尔值</returns>
        public bool IsAssistantAtCourse(string teacherNo, string courseNo,string classID,string termTag)
        {
            string commandstring = "SELECT [teacherNo],[courseNo]  FROM [USTA].[dbo].[usta_CoursesTeachersCorrelation] WHERE teacherNo=@teacherNo AND courseNo=@courseNo AND classID=@classID AND termTag=@termTag AND atCourseType=2";

            SqlParameter[] parameters = new SqlParameter[4]{new SqlParameter("@teacherNo",teacherNo),new SqlParameter("@courseNo",courseNo),new SqlParameter("@classID",classID),new SqlParameter("@termTag",termTag)
			};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);

            bool isAssistantAtCourse = false;

            while (dr.Read())
            {
                isAssistantAtCourse = true;
            }
            dr.Close();
            conn.Close();
            return isAssistantAtCourse;
        }

        /// <summary>
        /// 判断学生时候有选这门课
        /// </summary>
        /// <param name="studentNo">学号</param>
        /// <param name="courseNo">课程编号</param>
        /// <returns>布尔值</returns>
        public bool IsStudentHasCourse(string studentNo, string courseNo, string classID, string termTag)
        {
            string commandstring = "SELECT [studentNo],[courseNo],[Year] as termTag FROM [USTA].[dbo].[usta_CoursesStudentsCorrelation] WHERE studentNo=@studentNo AND courseNo=@courseNo AND classID=@classID AND Year=@termTag";
            SqlParameter[] parameters = new SqlParameter[4]{new SqlParameter("@studentNo",studentNo),new SqlParameter("@courseNo",courseNo),new SqlParameter("@classID",classID),new SqlParameter("@termTag",termTag)
			};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);

            bool isStudentAtCourse = false;

            while (dr.Read())
            {
                isStudentAtCourse = true;
            }
            dr.Close();
            conn.Close();
            return isStudentAtCourse;
        }
        /// <summary>
        /// 通过课程号获得此课程的相关作业
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <returns>课程作业数据集</returns>
        public DataSet GetSchoolworkNotifyBycourseNo(string courseNo)
        {
            DataSet ds = null;
            string sql = "SELECT [schoolWorkNotifyId],[schoolWorkNotifyTitle],[schoolWorkNotifyContent],[updateTime],[deadline],[courseNo],[isOnline],[attachmentIds] FROM [USTA].[dbo].[usta_SchoolWorkNotify] WHERE courseNo=@courseNo ORDER BY updateTime DESC";
            SqlParameter[] parameters = new SqlParameter[1]{
			   new SqlParameter("@courseNo",courseNo)
			};
            ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 添加作业通知
        /// </summary>
        /// <param name="schoolworkNotify">课程作业实体</param>
        /// <returns>作业实体</returns>
        public SchoolWorkNotify AddSchoolworkNotify(SchoolWorkNotify schoolworkNotify)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("spSchoolWorkNotifyAdd", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter("@schoolWorkNotifyTitle", schoolworkNotify.schoolWorkNotifyTitle));
                    cmd.Parameters.Add(new SqlParameter("@schoolWorkNotifyContent", schoolworkNotify.schoolWorkNotifyContent));
                    cmd.Parameters.Add(new SqlParameter("@updateTime", schoolworkNotify.updateTime));
                    cmd.Parameters.Add(new SqlParameter("@deadline", schoolworkNotify.deadline));
                    cmd.Parameters.Add(new SqlParameter("@isOnline", schoolworkNotify.isOnline));
                    cmd.Parameters.Add(new SqlParameter("@attachmentIds", schoolworkNotify.attachmentIds));
                    cmd.Parameters.Add(new SqlParameter("@courseNo", schoolworkNotify.courseNo));
                    cmd.Parameters.Add(new SqlParameter("@classId", schoolworkNotify.classID));
                    cmd.Parameters.Add(new SqlParameter("@termtag", schoolworkNotify.termTag));
                    cmd.Parameters.Add(new SqlParameter("@schoolWorkNotifyId", 0));
                    cmd.Parameters["@schoolWorkNotifyId"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    schoolworkNotify.schoolWorkNotifyId = int.Parse(cmd.Parameters["@schoolWorkNotifyId"].Value.ToString());
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
            return schoolworkNotify;
        }
       /// <summary>
       /// 查询课程学生信息-选课表中
       /// </summary>
       /// <param name="courseNo">课程编号</param>
       /// <returns>学生信息数据集</returns>
        public DataSet FindStudentInfoByCourseNo(string courseNo,string classId,string termtag)
        {
            string sql = "SELECT studentNo,courseNo FROM  usta_CoursesStudentsCorrelation  WHERE courseNo=@courseNo AND classID=@classId AND Year=@termtag";
            SqlParameter[] parameters = new SqlParameter[3]{
				 new SqlParameter("@courseNo",courseNo),
                 new SqlParameter("@classId",classId),
                 new SqlParameter("@termtag",termtag)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }
       /// <summary>
       /// 查询选课学生信息
       /// </summary>
       /// <param name="courseNo">课程编号</param>
       /// <returns>选课学生数据集</returns>
        public DataSet FindStudentInfoFromStudentListAndCorrelation(string courseNo,string classId,string termtag)
        {
            string sql = "SELECT studentName,usta_CoursesStudentsCorrelation.studentNo,usta_CoursesStudentsCorrelation.[courseNo] ";
            sql += "FROM  usta_CoursesStudentsCorrelation,usta_StudentsList ";
            sql += "WHERE usta_StudentsList.studentNo=usta_CoursesStudentsCorrelation.studentNo AND courseNo=@courseNo AND classID=@classId AND Year=@termtag";
            SqlParameter[] parameters = new SqlParameter[3]{
				 new SqlParameter("@courseNo",courseNo),
                 new SqlParameter("@classId",classId),
                 new SqlParameter("@termtag",termtag)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 根据主键删除作业及学生提交的作业
        /// </summary>
        /// <param name="schoolworkNotifyId">作业主键</param>
        public void DeleteSchoolworkNotify(int schoolworkNotifyId)
        {
            try
            {
                string sql = "DELETE FROM [USTA].[dbo].[usta_SchoolWorkNotify] WHERE schoolWorkNotifyId=@schoolWorkNotifyId;DELETE FROM [USTA].[dbo].[usta_SchoolWorks] WHERE schoolWorkNofityId=@schoolWorkNotifyId";
                SqlParameter[] parameters = new SqlParameter[1]{
					new SqlParameter("@schoolWorkNotifyId",schoolworkNotifyId)
				};
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
        /// 修改/更新作业通知
        /// </summary>
        /// <param name="schoolworkNotify">作业实体</param>
        public void UpdateSchoolworkNotify(SchoolWorkNotify schoolworkNotify)
        {
            try
            {
                string sql = "UPDATE [USTA].[dbo].[usta_SchoolWorkNotify] SET [schoolWorkNotifyTitle] = @schoolWorkNotifyTitle,[schoolWorkNotifyContent] = @schoolWorkNotifyContent,[updateTime] =@updateTime,[deadline] =@deadline ,[courseNo] = @courseNo,[isOnline] = @isOnline,[attachmentIds] = @attachmentIds WHERE schoolWorkNotifyId=@schoolWorkNotifyId";

                SqlParameter[] parameters = {
					new SqlParameter("@schoolWorkNotifyId", SqlDbType.Int,4),
					new SqlParameter("@schoolWorkNotifyTitle", SqlDbType.NChar,50),
					new SqlParameter("@schoolWorkNotifyContent", SqlDbType.NText),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@deadline", SqlDbType.DateTime),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@isOnline", SqlDbType.Bit,1),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)};
                parameters[0].Value = schoolworkNotify.schoolWorkNotifyId;
                parameters[1].Value = schoolworkNotify.schoolWorkNotifyTitle;
                parameters[2].Value = schoolworkNotify.schoolWorkNotifyContent;
                parameters[3].Value = schoolworkNotify.updateTime;
                parameters[4].Value = schoolworkNotify.deadline;
                parameters[5].Value = schoolworkNotify.courseNo;
                parameters[6].Value = schoolworkNotify.isOnline;
                parameters[7].Value = schoolworkNotify.attachmentIds;
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
        /// 通过作业通知得到提交的作业
        /// </summary>
        /// <param name="workNotifyId">作业主键</param>
        /// <returns>作业数据集</returns>
        public DataSet GetshoolworkByNotifyId(int workNotifyId)
        {
            string sql = "SELECT [schoolWorkId],[updateTime],[schoolWorkNofityId],[studentNo],[isCheck],[checkTime],[attachmentId] FROM [USTA].[dbo].[usta_SchoolWorks] WHERE schoolWorkNofityId=@schoolWorkNofityId";
            SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@schoolWorkNofityId",workNotifyId)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }

        /// <summary>
        ///  获得作业通知的具体项目返回dataset
        /// </summary>
        /// <param name="schoolworkNotifyId">作业主键</param>
        /// <returns>作业数据集</returns>
        public DataSet GetSchoolworkNotifybyId(int schoolworkNotifyId)
        {
            string sql = "SELECT [schoolWorkNotifyId],[schoolWorkNotifyTitle],[schoolWorkNotifyContent],[updateTime],[deadline],[courseNo],[isOnline],[attachmentIds] FROM [USTA].[dbo].[usta_SchoolWorkNotify] WHERE schoolWorkNotifyId=@schoolWorkNotifyId";
            SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@schoolWorkNotifyId",schoolworkNotifyId)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 获得作业通知的具体项目
        /// </summary>
        /// <param name="schoolworkNotifyId">作业主键</param>
        /// <returns>作业实体</returns>
        public SchoolWorkNotify GetSchoolworkNotifyById(int schoolworkNotifyId)
        {
            SchoolWorkNotify schoolWorkNotify = null;
            string sql = "SELECT [schoolWorkNotifyId],[schoolWorkNotifyTitle],[schoolWorkNotifyContent],[updateTime],[deadline],[courseNo],[isOnline],[attachmentIds] FROM [USTA].[dbo].[usta_SchoolWorkNotify] WHERE schoolWorkNotifyId=@schoolWorkNotifyId";
            SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@schoolWorkNotifyId",schoolworkNotifyId)
			};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
            while (dr.Read())
            {
                schoolWorkNotify = new SchoolWorkNotify
                {
                    schoolWorkNotifyId = int.Parse(dr["schoolWorkNotifyId"].ToString()),
                    schoolWorkNotifyTitle = dr["schoolWorkNotifyTitle"].ToString().Trim(),
                    schoolWorkNotifyContent = dr["schoolWorkNotifyContent"].ToString().Trim(),
                    updateTime = Convert.ToDateTime(dr["updateTime"].ToString()),
                    deadline = Convert.ToDateTime(dr["deadline"].ToString()),
                    courseNo = dr["courseNo"].ToString(),
                    isOnline = Boolean.Parse(dr["isOnline"].ToString()),
                    attachmentIds = dr["attachmentIds"].ToString()
                };
            }
            return schoolWorkNotify;
        }

        /// <summary>
        /// 获得历史学期标识
        /// </summary>
        /// <returns>学期标识数据集</returns>
        public DataSet GetHistoryTags()
        {
            string cmdstring = "SELECT [termTag]  FROM [USTA].[dbo].[usta_TermTags]  WHERE  termTag NOT IN (SELECT MAX(termTag) AS termTag FROM usta_TermTags) ORDER BY termTag DESC";
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring);
            conn.Close();
            return ds;
        }


        /// <summary>
        /// 根据学号获得有选课记录的历史学期标识
        /// <param name="studentNo">学号</param>
        /// </summary>
        /// <returns>学期标识数据集</returns>
        public DataSet GetHistoryTagsByStudentNo(string studentNo)
        {
            string cmdstring = "SELECT DISTINCT [Year] FROM [USTA].[dbo].[usta_CoursesStudentsCorrelation] WHERE studentNo=@studentNo ORDER BY Year DESC"; 
            SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@studentNo",studentNo)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring,parameters);
            conn.Close();
            return ds;
        }


        /// <summary>
        /// 根据教师编号获得有任课记录的历史学期标识
        /// <param name="teacherNo">教师编号</param>
        /// </summary>
        /// <returns>学期标识数据集</returns>
        public DataSet GetHistoryTagsByTeacherNo(string teacherNo)
        {
            string cmdstring = "SELECT DISTINCT [termTag] FROM [USTA].[dbo].[usta_CoursesTeachersCorrelation] WHERE teacherNo=@teacherNo ORDER BY termTag DESC";
            SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@teacherNo",teacherNo)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 获得学期标识列表
        /// </summary>
        /// <returns>学期标识数据集</returns>
        public DataSet FindAllTermTags()
        {
            string cmdstring = "SELECT [termTag]  FROM [USTA].[dbo].[usta_TermTags]   ORDER BY termTag DESC";
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 获得学期的课程列表
        /// </summary>
        /// <param name="termTags">学期标识</param>
        /// <returns>课程数据集</returns>
        public DataSet FindTermCoursesList(string termTags)
        {
            string cmdstring = "SELECT [courseNo],[courseName],[period],[credit],[courseSpeciality],[preCourse],[refferenceBooks],[termTag],[attachmentIds],[classID] FROM [USTA].[dbo].[usta_Courses]  WHERE  termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@termTag",termTags)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 获得当前学期的课程列表
        /// </summary>
        /// <returns>学期的课程数据集</returns>
        public DataSet FindCurrentTermCoursesList()
        {
            string cmdstring = "SELECT [courseNo],[courseName],[period],[credit],[courseSpeciality],[preCourse],[refferenceBooks],[termTag],[attachmentIds],[classID] FROM [USTA].[dbo].[usta_Courses]  WHERE  termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@termTag",termTag)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
            conn.Close();
            return ds;
        }

       /// <summary>
       /// 更新课程的介绍
       /// </summary>
       /// <param name="courseNo">课程编号</param>
       /// <param name="introduction">课程介绍</param>
        public void UpdateCourseIntroduction(string courseNo,string classId,string termtag, string introduction)
        {
            try
            {
                string sql = "UPDATE [USTA].[dbo].[usta_Courses] SET  [courseIntroduction] =@courseIntroduction WHERE courseNo=@courseNo AND ClassID=@classId AND termTag=@termtag";

                SqlParameter[] parameters = new SqlParameter[4]{
				new SqlParameter("@courseIntroduction", SqlDbType.NText),
				 new SqlParameter("@courseNo", SqlDbType.NChar,20),
                 new SqlParameter("@classId",SqlDbType.NVarChar,50),
                 new SqlParameter("@termtag",SqlDbType.NVarChar,50)
			};
                parameters[0].Value = introduction;
                parameters[1].Value = courseNo;
                parameters[2].Value = classId;
                parameters[3].Value = termtag;
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
        /// 更新课程教师的简介
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="resume">教师简介</param>
        public void UpdateTeacherResume(string courseNo,string classId,string termtag, string resume)
        {
            try
            {
                string sql = "UPDATE [USTA].[dbo].[usta_Courses] SET  [teacherResume] =@teacherResume WHERE courseNo=@courseNo AND ClassID=@classId AND termTag=@termtag";

                SqlParameter[] parameters = new SqlParameter[4]{
					new SqlParameter("@teacherResume", SqlDbType.NText),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
                    new SqlParameter("@classId",SqlDbType.NVarChar,50),
                     new SqlParameter("@termtag",SqlDbType.NVarChar,50)
				};
                parameters[0].Value = resume;
                parameters[1].Value = courseNo;
                parameters[2].Value = classId;
                parameters[3].Value = termtag;
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
        /// 根据学期标识和关键字查找课程
        /// </summary>
        /// <param name="termTag">学期标识</param>
        /// <param name="searchString">查询关键字</param>
        /// <returns>课程数据集</returns>
        public DataSet SearchCourses(string termTag, string searchString)
        {
            string sql = "SELECT [courseNo],[courseName],[period],[credit],[courseSpeciality],[termTag],[classID],[TestHours]   FROM [USTA].[dbo].[usta_Courses]  WHERE termTag=@termTag AND (courseName LIKE '%'+@keyword+'%' OR courseNo LIKE '%'+@keyword+'%') ORDER BY termTag DESC;";
            if (termTag == "all")
            {
                sql = "SELECT [courseNo],[courseName],[period],[credit],[courseSpeciality],[termTag],[classID],[TestHours]   FROM [USTA].[dbo].[usta_Courses]  WHERE (courseName LIKE '%'+@keyword+'%' OR courseNo LIKE '%'+@keyword+'%')  ORDER BY termTag DESC;";
            }
            SqlParameter[] parameters = new SqlParameter[2]{
				 new SqlParameter("@keyword",searchString),
				 new SqlParameter("@termTag",termTag)
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }


        /// <summary>
        /// 根据学期标识、开课地点和关键字查找课程
        /// </summary>
        /// <param name="termTag">学期标识</param>
        /// <param name="searchString">查询关键字</param>
        /// <returns>课程数据集</returns>
        public DataSet SearchCourses(string termTag, string searchString,string coursePlace)
        {
            string sql = "SELECT [courseNo],[courseName],[period],[credit],[courseSpeciality],[termTag],[classID]   FROM [USTA].[dbo].[usta_Courses]  WHERE termTag=@termTag AND (classID LIKE '%'+ @coursePlace + '%') AND (courseName LIKE '%'+@keyword+'%' OR courseNo LIKE '%'+@keyword+'%') ORDER BY termTag DESC;";
            
            if (termTag == "all")
            {
                sql = "SELECT [courseNo],[courseName],[period],[credit],[courseSpeciality],[termTag],[classID]   FROM [USTA].[dbo].[usta_Courses]  WHERE (classID LIKE '%'+ @coursePlace + '%') AND (courseName LIKE '%'+@keyword+'%' OR courseNo LIKE '%'+@keyword+'%')  ORDER BY termTag DESC;";
            }

            SqlParameter[] parameters = new SqlParameter[]{
				 new SqlParameter("@keyword",searchString),
				 new SqlParameter("@termTag",termTag),
				 new SqlParameter("@coursePlace",((termTag.Length >= 6 && termTag.Substring(5, 1) == "0") ? "暑期" : coursePlace))
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }


        /// <summary>
        /// 更新教学安排
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="plan">课程教学计划</param>
        public void UpdateCourseTeachingPlan(string courseNo,string classId,string termtag, string plan)
        {
            try
            {
                string sql = "UPDATE [USTA].[dbo].[usta_Courses] SET [teachingPlan] = @teachingPlan  WHERE courseNo=@courseNo AND classID=@classId AND termTag=@termtag";

                SqlParameter[] parameters = new SqlParameter[4]{
					new SqlParameter("@teachingPlan", SqlDbType.NText),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
                    new SqlParameter("@classId",SqlDbType.NVarChar,50),
                    new SqlParameter("@termtag",SqlDbType.NVarChar,50)
				};
                parameters[0].Value = plan;
                parameters[1].Value = courseNo;
                parameters[2].Value = classId;
                parameters[3].Value = termtag;
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
        /// 将所选的课程通知置顶值加一
        /// </summary>
        /// <param name="courseNotifyInfoId">课程通知编号</param>
        public void Addtop(int courseNotifyInfoId)
        {
            try
            {
                string sql = "UPDATE usta_CoursesNotifyInfo SET isTop=isTop+1 WHERE courseNotifyInfoId=@courseNotifyInfoId";
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

        /// <summary>
        /// 将所选的课程通知置顶取消
        /// </summary>
        /// <param name="courseNotifyInfoId">课程通知编号</param>
        public void Canceltop(int courseNotifyInfoId)
        {
            try
            {
                string sql = "UPDATE usta_CoursesNotifyInfo SET isTop=0 WHERE courseNotifyInfoId=@courseNotifyInfoId";
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

       /// <summary>
       /// 查询课程通知
       /// </summary>
       /// <param name="courseNotifyInfoId">课程通知编号</param>
       /// <returns>课程通知实体</returns>
        public CoursesNotifyInfo FindCourseNotifyById(int courseNotifyInfoId)
        {
            CoursesNotifyInfo coursesNotify = null;
            try
            {
                string sql = "SELECT courseNotifyInfoId,courseNotifyInfoTitle,courseNotifyInfoContent,updateTime,publishUserNo,courseNo,isTop,attachmentIds,notifyType,scanCount FROM usta_CoursesNotifyInfo  WHERE courseNotifyInfoId=@courseNotifyInfoId";
                SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@courseNotifyInfoId",courseNotifyInfoId)};

                SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
                while (dr.Read())
                {
                    coursesNotify = new CoursesNotifyInfo
                    {
                        courseNotifyInfoId = int.Parse(dr["courseNotifyInfoId"].ToString()),
                        courseNotifyInfoTitle = dr["courseNotifyInfoTitle"].ToString().Trim(),
                        publishUserNo = dr["publishUserNo"].ToString().Trim(),
                        updateTime = Convert.ToDateTime(dr["updateTime"].ToString()),
                        courseNo = dr["courseNo"].ToString(),
                        isTop = int.Parse(dr["isTop"].ToString()),
                        notifyType = int.Parse(dr["notifyType"].ToString()),
                        attachmentIds = dr["attachmentIds"].ToString(),
                        courseNotifyInfoContent = dr["courseNotifyInfoContent"].ToString(),
                        scanCount = int.Parse(dr["scanCount"].ToString())
                    };
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
            return coursesNotify;
        }

        
        /// <summary>
        /// 删除课程通知
        /// </summary>
        /// <param name="courseNotifyInfoId">课程通知编号</param>
        public void DeleteCourseNotifyById(int courseNotifyInfoId)
        {
            try
            {
                string sql = "DELETE FROM usta_CoursesNotifyInfo WHERE courseNotifyInfoId=@courseNotifyInfoId";
                SqlParameter[] parameters = new SqlParameter[1]{
			   new SqlParameter("@courseNotifyInfoId",courseNotifyInfoId)
			};
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


        #region 获取当前学期所有课程的任课教师信息（包括助教）
        /// <summary>
        /// 获取当前学期所有课程的任课教师信息（包括助教）
        /// </summary>
        /// <returns></returns>
        public DataSet GetTeachersInfoOfCurrentCourses()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT teacherName,emailAddress,B.atCourseType FROM usta_Courses A,dbo.usta_CoursesTeachersCorrelation B,usta_TeachersList C WHERE A.termTag=B.termTag AND A.courseNo=B.courseNo AND A.ClassID=B.ClassID AND B.teacherNo=C.teacherNo ANDA.termTag in(select top 1 termTag from usta_TermTags order by termTag desc)");
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }

        #endregion
    }
}
