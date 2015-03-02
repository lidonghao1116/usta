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
using System.Transactions;

namespace USTA.Dal
{
    /// <summary>
    /// 数据同步类，用于与信息平台的数据同步
    /// </summary>
    public class DalOperationAboutDataSynchronize
    {
        #region 数据库连接变量
        /// <summary>
        /// SSEDB数据库SqlConnection变量
        /// </summary>
        public SqlConnection conn
        {
            set;
            get;
        }

        /// <summary>
        /// USTA数据库SqlConnection变量
        /// </summary>
        public SqlConnection conn1
        {
            set;
            get;
        }
        #endregion

        #region 是否有新学期标识导入

        public bool isHasNewTermtag = false;

        #endregion

        #region 如果有新学期标识则赋值给一个字段

        public string newTermtag = string.Empty;

        #endregion

        #region 构造函数，用于初始化数据库连接等数据
        /// <summary>
        /// 构造函数，用于初始化数据库连接等数据
        /// </summary>
        public DalOperationAboutDataSynchronize()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStrSSEDB"].ConnectionString);
            conn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion


        #region 专业数据同步
        /// <summary>
        /// 专业数据同步
        /// </summary>
        public void MajorDataSynchronize()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //old:SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, "SELECT * FROM  MajorType");
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, "SELECT * FROM MajorType");


            while (dr.Read())
            {
                //old:dic.Add(dr["MajorTypeID"].ToString().Trim(), dr["MajorTypeName"].ToString().Trim());
                dic.Add(dr["ID"].ToString().Trim(), dr["MajorTypeName"].ToString().Trim());
            }
            dr.Close();
            conn.Close();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(conn1, CommandType.Text, "TRUNCATE TABLE usta_StudentSpecility;");

                    foreach (string key in dic.Keys)
                    {

                        SqlParameter[] parameters = {
                        new SqlParameter("@specilityName",dic[key]),
                    new SqlParameter("@MajorTypeID",key)
                                                    };
                        SqlHelper.ExecuteNonQuery(conn1, CommandType.Text, "INSERT INTO usta_StudentSpecility(specilityName,MajorTypeID)  VALUES(@specilityName, @MajorTypeID)", parameters);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
                finally
                {
                    conn1.Close();
                }
            }
        }
        #endregion

        #region 班级数据同步
        /// <summary>
        /// 班级数据同步
        /// </summary>
        public void SchoolClassDataSynchronize()
        {
            List<string> listSchoolClassID = new List<string>();
            List<string> listClassName = new List<string>();
            List<string> listMajorType = new List<string>();
            List<string> listRemark = new List<string>();
            List<string> listHeadteacher = new List<string>();
            List<string> listHeadteacherName = new List<string>();
            List<string> listLocale = new List<string>();

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "SELECT ID,ClassName,MajorType,Remark,Headteacher,HeadteacherName,Location FROM SchoolClass");

            while (dr.Read())
            {
                listSchoolClassID.Add(dr["ID"].ToString().Trim());
                listClassName.Add(dr["ClassName"].ToString().Trim());
                listMajorType.Add(dr["MajorType"].ToString().Trim());
                listRemark.Add(dr["Remark"].ToString().Trim());
                listHeadteacher.Add(dr["Headteacher"].ToString().Trim());
                listHeadteacherName.Add(dr["HeadteacherName"].ToString().Trim());
                listLocale.Add(dr["Location"].ToString().Trim());
            }
            dr.Close();
            conn.Close();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(conn1, CommandType.Text, "TRUNCATE TABLE usta_StudentClass;");

                    for (int i = 0; i < listClassName.Count; i++)
                    {

                        SqlParameter[] parameters = {
                        new SqlParameter("@className",listClassName[i]),
                        new SqlParameter("@MajorType",listMajorType[i]),
                        new SqlParameter("@remark",listRemark[i]),
                        new SqlParameter("@SchoolClassID",listSchoolClassID[i]),
                        new SqlParameter("@Headteacher",listHeadteacher[i]),
                    new SqlParameter("@HeadteacherName",listHeadteacherName[i]),
                    new SqlParameter("@locale",listLocale[i])
                                                    };


                        SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                            "INSERT INTO usta_StudentClass(className,MajorType,remark,SchoolClassID,Headteacher,HeadteacherName,locale) VALUES(@className,@MajorType,@remark,@SchoolClassID,@Headteacher,@HeadteacherName,@locale);", parameters);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
            }
            conn1.Close();
        }
        #endregion

        #region 学生数据同步
        /// <summary>
        /// 学生数据同步
        /// </summary>
        public void StudentDataSynchronize()
        {
            List<string> listStudentID = new List<string>();
            List<string> listStudentUSID = new List<string>();
            List<string> listStuNo = new List<string>();
            List<string> listName = new List<string>();
            List<string> listPhone = new List<string>();
            List<string> listEmail = new List<string>();
            List<string> listMajorType = new List<string>();
            List<string> listSchoolClass = new List<string>();
            List<string> listRemark = new List<string>();
            List<string> listSchoolClassName = new List<string>();
            List<string> listSex = new List<string>();
            List<string> listCardType = new List<string>();
            List<string> listCardNum = new List<string>();
            List<string> listMatriculationDate = new List<string>();

            Dictionary<string, string> dicMajorType = new Dictionary<string, string>();
            Dictionary<string, string> dicSchoolClass = new Dictionary<string, string>();


            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "select [ID],[StudentUSID],[StuNo],[Name],[Phone],[Email],[MajorType],[SchoolClass],[Remark],[SchoolClassName],[Sex],[CardType],[CardNum],[MatriculationDate] from Student");
            while (dr.Read())
            {
                listStudentID.Add(dr["ID"].ToString().Trim());
                listStudentUSID.Add(dr["StudentUSID"].ToString().Trim());
                listStuNo.Add(dr["StuNo"].ToString().Trim());
                listName.Add(dr["Name"].ToString().Trim());
                listPhone.Add(dr["Phone"].ToString().Trim());
                listEmail.Add(dr["Email"].ToString().Trim().Replace("'", string.Empty));
                listMajorType.Add(dr["MajorType"].ToString().Trim());
                listSchoolClass.Add(dr["SchoolClass"].ToString().Trim());
                listRemark.Add(dr["Remark"].ToString().Trim());
                listSchoolClassName.Add(dr["SchoolClassName"].ToString().Trim());
                listSex.Add(dr["Sex"].ToString().Trim());
                listCardType.Add(dr["CardType"].ToString().Trim());
                listCardNum.Add(dr["CardNum"].ToString().Trim());
                listMatriculationDate.Add(dr["MatriculationDate"].ToString().Trim());
            }
            dr.Close();
            conn.Close();

            //读取专业信息数据
            dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                "SELECT [specilityName] ,[MajorTypeID] FROM [USTA].[dbo].[usta_StudentSpecility]");
            while (dr.Read())
            {
                dicMajorType.Add(dr["MajorTypeID"].ToString().Trim(), dr["specilityName"].ToString().Trim());
            }
            dr.Close();

            //读取学生班级数据
            dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
               "SELECT [className],[SchoolClassID] FROM [USTA].[dbo].[usta_StudentClass]");
            while (dr.Read())
            {
                dicSchoolClass.Add(dr["SchoolClassID"].ToString().Trim(), dr["className"].ToString().Trim());
            }
            dr.Close();

            SqlParameter[] parameters;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < listStudentID.Count; i++)
                    {
                        parameters = new SqlParameter[]{
                        new SqlParameter("@studentNo",listStuNo[i])
                                                     };
                        dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                             "select [studentNo],[studentName],[studentUserPwd],[studentSpeciality],[mobileNo],[emailAddress],[remark],[classNo],[StudentID],[MajorType],[SchoolClass],[SchoolClassName] from [usta_StudentsList] WHERE studentNo=@studentNo;", parameters);

                        bool isHasValue = false;

                        while (dr.Read())
                        {
                            isHasValue = true;
                        }
                        dr.Close();

                        parameters = new SqlParameter[]{
                        new SqlParameter("@studentNo",listStuNo[i]),
                        new SqlParameter("@studentName",listName[i]),
                        new SqlParameter("@studentUserPwd","默认密码"),
                        new SqlParameter("@studentSpeciality",(dicMajorType.ContainsKey(listMajorType[i]) ? dicMajorType[listMajorType[i]] : string.Empty)),
                        new SqlParameter("@mobileNo",listPhone[i]),
                        new SqlParameter("@emailAddress",listEmail[i]),
                        new SqlParameter("@remark",listRemark[i]),
                        new SqlParameter("@classNo",string.Empty),
                        new SqlParameter("@StudentID",listStudentID[i]),
                        new SqlParameter("@MajorType",listMajorType[i]),
                        new SqlParameter("@SchoolClass",listSchoolClass[i]),
                        new SqlParameter("@SchoolClassName",(dicSchoolClass.ContainsKey(listSchoolClass[i]) ? dicSchoolClass[listSchoolClass[i]] : listSchoolClassName[i])),
                        new SqlParameter("@StudentUSID",listStudentUSID[i]),
                        new SqlParameter("@Sex",listSex[i]),
                        new SqlParameter("@CardType",listCardType[i]),
                        new SqlParameter("@CardNum",listCardNum[i]),
                        new SqlParameter("@MatriculationDate",listMatriculationDate[i])
                                                     };

                        if (!isHasValue)
                        {
                            SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                "INSERT INTO [usta_StudentsList] ([studentNo] ,[studentName] ,[studentUserPwd] ,[studentSpeciality] ,[mobileNo] ,[emailAddress] ,[remark] ,[classNo],[StudentID],[MajorType],[SchoolClass],[SchoolClassName],[StudentUSID],[Sex],[CardType],[CardNum],[MatriculationDate])" +
                                " VALUES (@studentNo ,@studentName ,@studentUserPwd ,@studentSpeciality,@mobileNo,@emailAddress,@remark ,@classNo,@StudentID,@MajorType,@SchoolClass,@SchoolClassName,@StudentUSID,@Sex,@CardType,@CardNum,@MatriculationDate);", parameters);
                        }
                        else
                        {
                            SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                "UPDATE [usta_StudentsList] SET [studentNo]=@studentNo,[studentName]=@studentName,[studentUserPwd]='默认密码' ,[studentSpeciality]=@studentSpeciality ,[mobileNo]=@mobileNo,[emailAddress]=@emailAddress,[remark]=@remark,[classNo]=@classNo,[StudentID]=@StudentID,[StudentUSID]=@StudentUSID,[MajorType]=@MajorType,[SchoolClass]=@SchoolClass,[SchoolClassName]=@SchoolClassName,[Sex]=@Sex,[CardType]=@CardType,[CardNum]=@CardNum,[MatriculationDate]=@MatriculationDate WHERE studentNo=@studentNo", parameters);
                        }

                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
            }
            conn1.Close();
        }
        #endregion

        #region 教师数据同步
        /// <summary>
        /// 教师数据同步
        /// </summary>
        public void TeacherDataSynchronize()
        {
            List<string> listTeacherID = new List<string>();
            List<string> listTeacherUSID = new List<string>();
            List<string> listName = new List<string>();
            List<string> listEmployeeNum = new List<string>();
            List<string> listPhone = new List<string>();
            List<string> listEmail = new List<string>();
            List<int> listIsAssistant = new List<int>();
            List<int> listIsSuperisor = new List<int>();
            List<string> listRemark = new List<string>();
            List<string> listEnterpriseID = new List<string>();
            List<string> listIsHeaderTeacher = new List<string>();
            List<string> listTeacherType = new List<string>();
            List<string> listSex = new List<string>();
            //是否为企业导师
            List<string> listIsBusinessGuru = new List<string>();


            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "select [ID],[TeacherUSID],[Name],[EmployeeNum],[Email],[IsAssistant],[IsSuperisor],[Remark],[Sex],[EnterpriseID],[IsHeadteacher],[TeacherType],[IsBusinessGuru] from  Teacher;");
            while (dr.Read())
            {
                listTeacherID.Add(dr["ID"].ToString().Trim());
                listTeacherUSID.Add(dr["TeacherUSID"].ToString().Trim());
                listName.Add(dr["Name"].ToString().Trim());
                listEmployeeNum.Add(dr["EmployeeNum"].ToString().Trim());
                listEmail.Add(dr["Email"].ToString().Trim());
                listRemark.Add(dr["Remark"].ToString().Trim());
                listEnterpriseID.Add(dr["EnterpriseID"].ToString().Trim());
                listIsHeaderTeacher.Add(string.IsNullOrEmpty(dr["IsHeadteacher"].ToString().Trim()) ? string.Empty : dr["IsHeadteacher"].ToString().Trim());
                listTeacherType.Add(string.IsNullOrEmpty(dr["TeacherType"].ToString().Trim()) ? string.Empty : dr["TeacherType"].ToString().Trim());
                listIsBusinessGuru.Add(string.IsNullOrEmpty(dr["IsBusinessGuru"].ToString().Trim()) ? string.Empty : dr["IsBusinessGuru"].ToString().Trim());
                listIsAssistant.Add(int.Parse(dr["IsAssistant"].ToString().Trim()));
                listIsSuperisor.Add(int.Parse(dr["IsSuperisor"].ToString().Trim()));
                listSex.Add(dr["Sex"].ToString().Trim());
            }
            dr.Close();
            conn.Close();

            SqlParameter[] parameters;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < listTeacherID.Count; i++)
                    {
                        parameters = new SqlParameter[] {
                            new SqlParameter("@teacherNo",listTeacherID[i])
                        };
                        dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                             "select [teacherNo] from [usta_TeachersList] WHERE teacherNo=@teacherNo;", parameters);

                        bool isHasValue = false;

                        while (dr.Read())
                        {
                            isHasValue = true;
                        }
                        dr.Close();

                        parameters = new SqlParameter[] {
                            new SqlParameter("@teacherNo",listTeacherID[i]),
                            new SqlParameter("@TeacherID",listTeacherID[i]),
                            new SqlParameter("@TeacherUSID",listTeacherUSID[i]),
                            new SqlParameter("@EmployeeNum",listEmployeeNum[i]),
                            new SqlParameter("@teacherUserPwd","默认密码"),
                            new SqlParameter("@teacherName",listName[i]),
                            new SqlParameter("@emailAddress",listEmail[i]),
                            new SqlParameter("@remark",listRemark[i]),
                            new SqlParameter("@isHeadteacher",(string.IsNullOrEmpty(listIsHeaderTeacher[i]) ? 0 : Convert.ToInt32(listIsHeaderTeacher[i]))),
                            new SqlParameter("@TeacherType",listTeacherType[i]),
                            new SqlParameter("@IsBusinessGuru",(string.IsNullOrEmpty(listIsBusinessGuru[i]) ? 0 : Convert.ToInt32(listIsBusinessGuru[i]))),
                            new SqlParameter("@IsAssistant",listIsAssistant[i]),
                            new SqlParameter("@IsSuperisor",listIsSuperisor[i]),
                            new SqlParameter("@Sex",listSex[i])
                        };

                        if (!isHasValue)
                        {
                            SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                "INSERT INTO [usta_TeachersList] ([teacherNo],[TeacherID],[TeacherUSID],[EmployeeNum],[teacherUserPwd],[teacherName],[emailAddress],[remark],[isHeadteacher],[TeacherType],[IsBusinessGuru],[IsAssistant],[IsSuperisor],[Sex])" +
                                " VALUES (@teacherNo,@TeacherID,@TeacherUSID,@EmployeeNum,@teacherUserPwd,@teacherName,@emailAddress,@remark,@isHeadteacher,@TeacherType,@IsBusinessGuru,@IsAssistant,@IsSuperisor,@Sex);", parameters);
                        }
                        else
                        {
                            SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                "UPDATE [usta_TeachersList] SET [TeacherID]=@TeacherID,[TeacherUSID]=@TeacherUSID,[EmployeeNum]=@EmployeeNum,[teacherName]=@teacherName,[teacherUserPwd]=@teacherUserPwd,[emailAddress]=@emailAddress,[isHeadteacher]=@isHeadteacher,[remark]=@remark,[TeacherType]=@TeacherType,[IsBusinessGuru]=@IsBusinessGuru,IsAssistant=@IsAssistant,IsSuperisor=@IsSuperisor,Sex=@Sex WHERE teacherNo=@teacherNo", parameters);

                        }
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
            }
            conn1.Close();
        }


        #endregion

        #region 学期标识同步
        /// <summary>
        /// 学期标识同步
        /// </summary>
        public void TermTagsDataSynchronize()
        {
            List<string> list = new List<string>();


            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, "SELECT DISTINCT NewSchoolYear FROM CoursePlan");
            while (dr.Read())
            {
                list.Add(dr["NewSchoolYear"].ToString().Trim());
            }
            dr.Close();
            conn.Close();

            SqlParameter[] parameters;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (string newSchoolYear in list)
                    {
                        parameters = new SqlParameter[]{
                            new SqlParameter("@termTag",newSchoolYear)
                        };
                        dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                             "SELECT termTag FROM usta_TermTags WHERE termTag=@termTag;", parameters);
                        bool isHasValue = false;

                        while (dr.Read())
                        {
                            isHasValue = true;
                        }
                        dr.Close();

                        if (!isHasValue)
                        {
                            SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                "INSERT INTO usta_TermTags VALUES(@termTag)", parameters);
                            //此处判断是否有新学期标识导入
                            this.isHasNewTermtag = true;
                            this.newTermtag = newSchoolYear;
                        }
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
            }
            conn1.Close();
        }
        #endregion

        #region 课程数据同步
        /// <summary>
        /// 课程数据同步
        /// </summary>
        public void CourseDataSynchronize()
        {
            List<string> listCourseID = new List<string>();
            List<string> listCourseName = new List<string>();
            List<string> listNewSchoolYear = new List<string>();
            List<string> listClassID = new List<string>();
            List<int> listIsDelete = new List<int>();
            List<string> listPeriod = new List<string>();
            List<string> listTestHours = new List<string>();
            List<string> listCredit = new List<string>();
            List<string> listCourseIntroduction = new List<string>();
            List<string> listDetailLocation = new List<string>();
            List<string> listLessonTimeAndAddress = new List<string>();
            List<string> listCourseSpeciality = new List<string>();

            List<string> listTimeAndRoom = new List<string>();

            SqlParameter[] parameters;

            //先查基本信息
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "SELECT [TheoryHours],[TestHours],[Credit],[writer],[ObjectivesAndrequirements],[CoursesInfo],[KeyAndDifficult],"
                + "[TeachingMaterials],[MainContentsAndHoursAllocation],[ClassID],B.[CourseID],B.[CourseName],[DetailLocation],[TimeAndRoom],"
                + "[IsDelete],[NewSchoolYear],[Experimental] "
                + " FROM [Course] A,[CoursePlan] B "
                + " WHERE A.CourseID =B.CourseID AND isDelete=0;"
                );
            while (dr.Read())
            {
                listCourseID.Add(dr["CourseID"].ToString().Trim());
                listCourseName.Add(dr["CourseName"].ToString().Trim());
                listNewSchoolYear.Add(dr["NewSchoolYear"].ToString().Trim());
                listClassID.Add(dr["ClassID"].ToString().Trim());
                listIsDelete.Add(int.Parse(dr["isDelete"].ToString().Trim()));
                listPeriod.Add(dr["TheoryHours"].ToString().Trim());
                listTestHours.Add(dr["TestHours"].ToString().Trim());
                listCredit.Add(dr["Credit"].ToString().Trim());
                listCourseIntroduction.Add(dr["CoursesInfo"].ToString().Trim());
                listDetailLocation.Add(dr["DetailLocation"].ToString().Trim());
            }
            dr.Close();

            //再查上课时间

            for (int i = 0; i < listCourseID.Count; i++)
            {
                string _timeAndRoom = string.Empty;

                parameters = new SqlParameter[]{
                            new SqlParameter("@courseName",listCourseName[i]),
                            new SqlParameter("@termTag",listNewSchoolYear[i]),
                            new SqlParameter("@classID",listClassID[i])
                        };
                dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "select [id],[ClassID],[CourseName],[StartStopWeek],[Week],[ClassTime],[Room],[Location],[Year],[ISExperimental]"
                + "from [CourseTimePlan] WHERE ClassID=@classID AND Year=@termTag AND CourseName=@courseName", parameters);
                while (dr.Read())
                {
                    _timeAndRoom += (dr["StartStopWeek"].ToString().Trim() + "_" + dr["Week"].ToString().Trim() + "_" + dr["ClassTime"].ToString().Trim() + "_" + dr["Room"].ToString().Trim() + "_" + dr["Location"].ToString().Trim() + "<br />");
                }
                dr.Close();

                listTimeAndRoom.Add(_timeAndRoom);
            }

            //再查课程属性

            for (int i = 0; i < listCourseID.Count; i++)
            {
                string _courseType = string.Empty;

                parameters = new SqlParameter[]{
                            new SqlParameter("@courseName",listCourseName[i]),
                            new SqlParameter("@courseID",listCourseID[i])
                        };
                dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "select [CourseType] from [TrainingPlanCourse] WHERE CourseID=@courseID AND CourseName=@courseName", parameters);
                while (dr.Read())
                {
                    _courseType = (dr["CourseType"].ToString().Trim());
                }
                dr.Close();
                listCourseSpeciality.Add(_courseType);
            }

            conn.Close();

            //此处需要自动导入上一学期相同课程的基本信息，以避免重复导入数据

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < listCourseID.Count; i++)
                    {
                        parameters = new SqlParameter[]{
                            new SqlParameter("@courseNo",listCourseID[i]),
                            new SqlParameter("@termTag",listNewSchoolYear[i]),
                            new SqlParameter("@classID",listClassID[i])
                        };
                        dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                             "select [ObjectID],[courseNo],[courseName],[termTag] from [usta_Courses] WHERE courseNo=@courseNo AND termTag=@termTag AND classID=@classID", parameters);

                        bool isHasValue = false;

                        while (dr.Read())
                        {
                            isHasValue = true;
                        }
                        dr.Close();

                        if (!isHasValue)
                        {
                            bool isHasLastTermTag = false;

                            if (this.isHasNewTermtag && !string.IsNullOrEmpty(this.newTermtag))
                            {
                                Courses courses = new Courses();

                                parameters = new SqlParameter[]{
                                    new SqlParameter("@termTag",this.newTermtag)
                                };

                                dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                                     "select top 1 [courseNo],[courseName],[period],[credit]," +
                                     "[courseSpeciality],[preCourse],[refferenceBooks],[termTag]," +
                                "[attachmentIds],[homePage],[courseAnswer],[teacherResume],[courseIntroduction]," +
                                "[examineMethod],[lessonTimeAndAddress],[teachingPlan],[bbsEmailAddress],[ObjectID]," +
                                "[ClassID],[isDelete] from [usta_Courses] where termTag<@termTag order by termTag desc", parameters);

                                while (dr.Read())
                                {
                                    isHasLastTermTag = true;

                                    courses.courseNo = dr["courseNo"].ToString().Trim();
                                    courses.courseName = dr["courseName"].ToString().Trim();
                                    courses.period = dr["period"].ToString().Trim();
                                    courses.credit = float.Parse(dr["credit"].ToString().Trim());
                                    courses.courseSpeciality = dr["courseSpeciality"].ToString().Trim();
                                    courses.preCourse = dr["preCourse"].ToString().Trim();
                                    courses.refferenceBooks = dr["refferenceBooks"].ToString().Trim();
                                    courses.termTag = dr["termTag"].ToString().Trim();
                                    courses.attachmentIds = dr["attachmentIds"].ToString().Trim();
                                    courses.homePage = dr["homePage"].ToString().Trim();
                                    courses.teacherResume = dr["teacherResume"].ToString().Trim();
                                    courses.courseIntroduction = dr["courseIntroduction"].ToString().Trim();
                                    courses.examineMethod = dr["examineMethod"].ToString().Trim();
                                    courses.lessonTimeAndAddress = dr["lessonTimeAndAddress"].ToString().Trim();
                                    courses.teachingPlan = dr["teachingPlan"].ToString().Trim();
                                    courses.bbsEmailAddress = dr["bbsEmailAddress"].ToString().Trim();
                                    courses.objectID = dr["ObjectID"].ToString().Trim();
                                    courses.classID = dr["ClassID"].ToString().Trim();
                                    courses.isDelete = int.Parse(dr["isDelete"].ToString().Trim());
                                }
                                dr.Close();

                                if (isHasLastTermTag)
                                {
                                    parameters = new SqlParameter[]{
                                    new SqlParameter("@courseNo",listCourseID[i]),
                                    new SqlParameter("@courseName",listCourseName[i]),
                                    new SqlParameter("@period",listPeriod[i]),
                                    new SqlParameter("@credit",listCredit[i]),
                                    new SqlParameter("@courseSpeciality",listCourseSpeciality[i]),
                                    new SqlParameter("@preCourse",courses.preCourse),
                                    new SqlParameter("@refferenceBooks",courses.refferenceBooks),
                                    new SqlParameter("@termTag",listNewSchoolYear[i]),
                                    new SqlParameter("@attachmentIds",courses.attachmentIds),
                                    new SqlParameter("@homePage",courses.homePage),
                                    new SqlParameter("@courseAnswer",courses.courseAnswer),
                                    new SqlParameter("@teacherResume",courses.teacherResume),
                                    new SqlParameter("@courseIntroduction",listCourseIntroduction[i]),
                                    new SqlParameter("@examineMethod",courses.examineMethod),
                                    new SqlParameter("@lessonTimeAndAddress",listTimeAndRoom[i]),
                                    new SqlParameter("@teachingPlan",courses.teachingPlan),
                                    new SqlParameter("@ClassID",listClassID[i]),
                                    new SqlParameter("@isDelete",courses.isDelete)
                                };

                                    SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                        "IINSERT INTO [usta_Courses] ([courseNo] ,[courseName] ,[period] ,[credit]," +
"[courseSpeciality] ,[preCourse] ,[refferenceBooks] ,[termTag] ,[attachmentIds] ,[homePage] ,[courseAnswer] ,[teacherResume] ," +
"[courseIntroduction] ,[examineMethod] ,[lessonTimeAndAddress] ,[teachingPlan] ," +
    "[ClassID] ,[isDelete] ) VALUES (@courseNo ,@courseName,@period,@credit,@courseSpeciality,@preCourse,@refferenceBooks,@termTag,@attachmentIds,@homePage ,@courseAnswer,@teacherResume,@courseIntroduction,@examineMethod,@lessonTimeAndAddress,@teachingPlan,@ClassID,@isDelete);", parameters);
                                }
                                //若无上个学期则按正常同步即可
                                else
                                {
                                    StringBuilder strSql = new StringBuilder();
                                    strSql.Append("insert into usta_Courses(");
                                    strSql.Append("courseNo,courseName,period,credit,courseSpeciality,termTag,courseIntroduction,lessonTimeAndAddress,ClassID,isDelete,TestHours)");
                                    strSql.Append(" values (");
                                    strSql.Append("@courseNo,@courseName,@period,@credit,@courseSpeciality,@termTag,@courseIntroduction,@lessonTimeAndAddress,@ClassID,@isDelete,@TestHours)");
                                    parameters = new SqlParameter[]{
					new SqlParameter("@courseName", SqlDbType.NChar,50),
					new SqlParameter("@period", SqlDbType.NChar,50),
					new SqlParameter("@credit", SqlDbType.Float,8),
					new SqlParameter("@courseSpeciality", SqlDbType.NVarChar,2000),
					new SqlParameter("@courseIntroduction", SqlDbType.NText),
					new SqlParameter("@lessonTimeAndAddress", SqlDbType.NVarChar,2000),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@TestHours", SqlDbType.NVarChar,10),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50),
					new SqlParameter("@ClassID", SqlDbType.NVarChar,50)};
                                    parameters[0].Value = listCourseName[i];
                                    parameters[1].Value = listPeriod[i];
                                    parameters[2].Value = listCredit[i];
                                    parameters[3].Value = listCourseSpeciality[i];
                                    parameters[4].Value = listCourseIntroduction[i];
                                    parameters[5].Value = listTimeAndRoom[i];
                                    parameters[6].Value = listIsDelete[i];
                                    parameters[7].Value = listTestHours[i];
                                    parameters[8].Value = listCourseID[i];
                                    parameters[9].Value = listNewSchoolYear[i];
                                    parameters[10].Value = listClassID[i];

                                    SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                        strSql.ToString(), parameters);
                                }
                            }
                            //若无新学期标识则按正常同步即可
                            else
                            {
                                StringBuilder strSql = new StringBuilder();
                                strSql.Append("insert into usta_Courses(");
                                strSql.Append("courseNo,courseName,period,credit,courseSpeciality,termTag,courseIntroduction,lessonTimeAndAddress,ClassID,isDelete,TestHours)");
                                strSql.Append(" values (");
                                strSql.Append("@courseNo,@courseName,@period,@credit,@courseSpeciality,@termTag,@courseIntroduction,@lessonTimeAndAddress,@ClassID,@isDelete,@TestHours)");
                                parameters = new SqlParameter[]{
					new SqlParameter("@courseName", SqlDbType.NChar,50),
					new SqlParameter("@period", SqlDbType.NChar,50),
					new SqlParameter("@credit", SqlDbType.Float,8),
					new SqlParameter("@courseSpeciality", SqlDbType.NVarChar,2000),
					new SqlParameter("@courseIntroduction", SqlDbType.NText),
					new SqlParameter("@lessonTimeAndAddress", SqlDbType.NVarChar,2000),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@TestHours", SqlDbType.NVarChar,10),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50),
					new SqlParameter("@ClassID", SqlDbType.NVarChar,50)};
                                parameters[0].Value = listCourseName[i];
                                parameters[1].Value = listPeriod[i];
                                parameters[2].Value = listCredit[i];
                                parameters[3].Value = listCourseSpeciality[i];
                                parameters[4].Value = listCourseIntroduction[i];
                                parameters[5].Value = listTimeAndRoom[i];
                                parameters[6].Value = listIsDelete[i];
                                parameters[7].Value = listTestHours[i];
                                parameters[8].Value = listCourseID[i];
                                parameters[9].Value = listNewSchoolYear[i];
                                parameters[10].Value = listClassID[i];

                                SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                    strSql.ToString(), parameters);
                            }
                        }
                        //否则只同步当前学期非教学平台独有的数据项，独有的数据仍然保存
                        else
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("update usta_Courses set ");
                            strSql.Append("courseName=@courseName,");
                            strSql.Append("period=@period,");
                            strSql.Append("credit=@credit,");
                            strSql.Append("courseSpeciality=@courseSpeciality,");
                            //strSql.Append("preCourse=@preCourse,");
                            //strSql.Append("refferenceBooks=@refferenceBooks,");
                            //strSql.Append("attachmentIds=@attachmentIds,");
                            //strSql.Append("homePage=@homePage,");
                            //strSql.Append("courseAnswer=@courseAnswer,");
                            //strSql.Append("teacherResume=@teacherResume,");
                            strSql.Append("courseIntroduction=@courseIntroduction,");
                            //strSql.Append("examineMethod=@examineMethod,");
                            strSql.Append("lessonTimeAndAddress=@lessonTimeAndAddress,");
                            //strSql.Append("teachingPlan=@teachingPlan,");
                            //strSql.Append("bbsEmailAddress=@bbsEmailAddress,");
                            strSql.Append("isDelete=@isDelete,");
                            strSql.Append("TestHours=@TestHours");
                            strSql.Append(" where courseNo=@courseNo AND termTag=@termTag AND classID=@classID");
                            parameters = new SqlParameter[]{
					new SqlParameter("@courseName", SqlDbType.NChar,50),
					new SqlParameter("@period", SqlDbType.NChar,50),
					new SqlParameter("@credit", SqlDbType.Float,8),
					new SqlParameter("@courseSpeciality", SqlDbType.NVarChar,2000),
					//new SqlParameter("@preCourse", SqlDbType.NVarChar,500),
					//new SqlParameter("@refferenceBooks", SqlDbType.NVarChar,1000),
					//new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					//new SqlParameter("@homePage", SqlDbType.NVarChar,200),
					//new SqlParameter("@courseAnswer", SqlDbType.NVarChar,4000),
					//new SqlParameter("@teacherResume", SqlDbType.NText),
					new SqlParameter("@courseIntroduction", SqlDbType.NText),
					//new SqlParameter("@examineMethod", SqlDbType.NVarChar,4000),
					new SqlParameter("@lessonTimeAndAddress", SqlDbType.NVarChar,2000),
					//new SqlParameter("@teachingPlan", SqlDbType.NText),
					//new SqlParameter("@bbsEmailAddress", SqlDbType.NVarChar,500),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@TestHours", SqlDbType.NVarChar,10),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50),
					new SqlParameter("@ClassID", SqlDbType.NVarChar,50)};
                            parameters[0].Value = listCourseName[i];
                            parameters[1].Value = listPeriod[i];
                            parameters[2].Value = listCredit[i];
                            parameters[3].Value = listCourseSpeciality[i];
                            parameters[4].Value = listCourseIntroduction[i];
                            parameters[5].Value = listTimeAndRoom[i];
                            parameters[6].Value = listIsDelete[i];
                            parameters[7].Value = listTestHours[i];
                            parameters[8].Value = listCourseID[i];
                            parameters[9].Value = listNewSchoolYear[i];
                            parameters[10].Value = listClassID[i];

                            SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                                strSql.ToString(), parameters);
                        }

                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
            }
            conn1.Close();
        }
        #endregion

        #region 教师任课数据同步
        /// <summary>
        /// 教师任课数据同步
        /// </summary>
        public void CoursesTeacherRelationDataSynchronize()
        {
            //List<string> listObjectID = new List<string>();
            List<string> listCourseID = new List<string>();
            List<string> listTeacherName = new List<string>();
            List<int> listAtCourseType = new List<int>();
            List<string> listNewSchoolYear = new List<string>();
            List<string> listClassID = new List<string>();

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "select DISTINCT [CourseID],[TeacherName],[TeacherName2],[ExpelimentTeacher],[ExpelimentTeacher2],[ExpelimentTeacher3],[NewSchoolYear],[ClassID] from CoursePlan");
            while (dr.Read())
            {
                if (!string.IsNullOrEmpty(dr["TeacherName"].ToString().Trim()))
                {
                    //listObjectID.Add(dr["ObjectID"].ToString().Trim());
                    listCourseID.Add(dr["CourseID"].ToString().Trim());
                    listTeacherName.Add(dr["TeacherName"].ToString().Trim());
                    listAtCourseType.Add(1);
                    listNewSchoolYear.Add(dr["NewSchoolYear"].ToString().Trim());
                    listClassID.Add(dr["ClassID"].ToString().Trim());
                }

                if (!string.IsNullOrEmpty(dr["TeacherName2"].ToString().Trim()))
                {
                    //listObjectID.Add(dr["ObjectID"].ToString().Trim());
                    listCourseID.Add(dr["CourseID"].ToString().Trim());
                    listTeacherName.Add(dr["TeacherName2"].ToString().Trim());
                    listAtCourseType.Add(1);
                    listNewSchoolYear.Add(dr["NewSchoolYear"].ToString().Trim());
                    listClassID.Add(dr["ClassID"].ToString().Trim());
                }

                if (!string.IsNullOrEmpty(dr["ExpelimentTeacher"].ToString().Trim()))
                {
                    //listObjectID.Add(dr["ObjectID"].ToString().Trim());
                    listCourseID.Add(dr["CourseID"].ToString().Trim());
                    listTeacherName.Add(dr["ExpelimentTeacher"].ToString().Trim());
                    listAtCourseType.Add(2);
                    listNewSchoolYear.Add(dr["NewSchoolYear"].ToString().Trim());
                    listClassID.Add(dr["ClassID"].ToString().Trim());
                }

                if (!string.IsNullOrEmpty(dr["ExpelimentTeacher2"].ToString().Trim()))
                {
                    //listObjectID.Add(dr["ObjectID"].ToString().Trim());
                    listCourseID.Add(dr["CourseID"].ToString().Trim());
                    listTeacherName.Add(dr["ExpelimentTeacher2"].ToString().Trim());
                    listAtCourseType.Add(2);
                    listNewSchoolYear.Add(dr["NewSchoolYear"].ToString().Trim());
                    listClassID.Add(dr["ClassID"].ToString().Trim());
                }

                if (!string.IsNullOrEmpty(dr["ExpelimentTeacher3"].ToString().Trim()))
                {
                    //listObjectID.Add(dr["ObjectID"].ToString().Trim());
                    listCourseID.Add(dr["CourseID"].ToString().Trim());
                    listTeacherName.Add(dr["ExpelimentTeacher3"].ToString().Trim());
                    listAtCourseType.Add(2);
                    listNewSchoolYear.Add(dr["NewSchoolYear"].ToString().Trim());
                    listClassID.Add(dr["ClassID"].ToString().Trim());
                }

            }
            dr.Close();
            conn.Close();

            SqlParameter[] parameters;


            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(conn1, CommandType.Text, "TRUNCATE TABLE usta_CoursesTeachersCorrelation;");

                    for (int i = 0; i < listCourseID.Count; i++)
                    {
                        //parameters = new SqlParameter[]
                        //{
                        //    new SqlParameter("@ObjectID",listObjectID[i]),
                        //    new SqlParameter("@termTag",listNewSchoolYear[i]),
                        //    new SqlParameter("@classID",listClassID[i]),
                        //    new SqlParameter("@CourseNo",listCourseID[i])
                        //};

                        //dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                        //     "select [teacherNo],[courseNo],[atCourseType],[termTag] from usta_CoursesTeachersCorrelation WHERE ObjectID=@ObjectID AND termTag=@termTag AND classID=@classID AND CourseNo=@CourseNo;", parameters);

                        //bool isHasValue = false;

                        //while (dr.Read())
                        //{
                        //    isHasValue = true;
                        //}
                        //dr.Close();

                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@teacherName",listTeacherName[i].Substring(0, (listTeacherName[i].IndexOf('(') != -1 ? listTeacherName[i].IndexOf('(') : 0)))
                        };

                        dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                            "select [teacherNo] from [usta_TeachersList] WHERE [teacherName]=@teacherName", parameters);

                        string teacherNo = string.Empty;

                        while (dr.Read())
                        {
                            teacherNo = dr["teacherNo"].ToString().Trim();
                        }
                        dr.Close();


                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@teacherNo",teacherNo),
                            new SqlParameter("@courseNo",listCourseID[i]),
                            new SqlParameter("@atCourseType",listAtCourseType[i]),
                            new SqlParameter("@termTag",listNewSchoolYear[i]),
                            new SqlParameter("@ClassID",listClassID[i])
                        };


                        //if (!isHasValue)
                        //{
                        SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                            "INSERT INTO usta_CoursesTeachersCorrelation([teacherNo],[courseNo],[atCourseType],[termTag],[ClassID]) VALUES(@teacherNo,@courseNo,@atCourseType,@termTag,@ClassID)", parameters);
                        //}
                        //else
                        //{
                        //    SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                        //        "UPDATE usta_CoursesTeachersCorrelation SET [teacherNo]=@teacherNo,[courseNo]=@courseNo,[termTag]=@termTag,[ClassID]=@ClassID WHERE ObjectID=@ObjectID AND teacherNo=@teacherNo AND termTag=@termTag AND classID=@classID AND CourseNo=@CourseNo;", parameters);
                        //}
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
            }
            conn1.Close();
        }
        #endregion

        #region 选课数据同步
        /// <summary>
        /// 选课数据同步
        /// </summary>
        public void CoursesElectiveDataSynchronize()
        {
            List<string> listObjectID = new List<string>();
            List<string> listCourseID = new List<string>();
            List<string> listStudentID = new List<string>();
            List<string> listYear = new List<string>();
            List<string> listClassID = new List<string>();


            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
                "select DISTINCT [ID],[CourseID],[StudentNo],[Year],[ClassID] from ElectiveStudent");
            while (dr.Read())
            {
                listObjectID.Add(dr["ID"].ToString().Trim());
                listCourseID.Add(dr["CourseID"].ToString().Trim());
                listStudentID.Add(dr["StudentNo"].ToString().Trim());
                listYear.Add(dr["Year"].ToString().Trim());
                listClassID.Add(dr["ClassID"].ToString().Trim());
            }
            dr.Close();
            conn.Close();

            SqlParameter[] parameters;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(conn1, CommandType.Text, "TRUNCATE TABLE usta_CoursesStudentsCorrelation;");

                    for (int i = 0; i < listObjectID.Count; i++)
                    {
                        parameters = new SqlParameter[]{
                        new SqlParameter("@ObjectID",listObjectID[i]),
                        new SqlParameter("@CourseNo",listCourseID[i]),
                        new SqlParameter("@StudentNo",listStudentID[i]),
                        new SqlParameter("@Year",listYear[i]),
                        new SqlParameter("@ClassID",listClassID[i])
                    };

                        SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                            "INSERT INTO usta_CoursesStudentsCorrelation([ObjectID],[CourseNo],[StudentNo],[Year],[ClassID]) VALUES(@ObjectID,@CourseNo,@StudentNo,@Year,@ClassID);", parameters);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
                finally
                {
                    conn1.Close();
                }
            }
        }
        #endregion

        #region 作业与实验数据同步
        /// <summary>
        /// 作业与实验数据同步
        /// </summary>
        public void SchoolWorkAndExperimentsDataSynchronize()
        {
            List<string> listStudentID = new List<string>();
            StringBuilder sb = new StringBuilder();

            SqlParameter[] parameters;

            SqlDataReader dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                "select [studentNo] from [usta_StudentsList]");
            while (dr.Read())
            {
                listStudentID.Add(dr["studentNo"].ToString().Trim());
            }
            dr.Close();

            foreach (string item in listStudentID)
            {
                List<string> listCoursesStudentsCorrelationCourseNo = new List<string>();
                List<string> listCoursesStudentsCorrelationYear = new List<string>();
                List<string> listCoursesStudentsCorrelationClassID = new List<string>();

                parameters = new SqlParameter[]{
                    new SqlParameter("@studentNo",item)
                };

                dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                "select [courseNo],[Year],[ClassID] from [usta_CoursesStudentsCorrelation] where studentNo=@studentNo;", parameters);
                while (dr.Read())
                {
                    listCoursesStudentsCorrelationCourseNo.Add(dr["courseNo"].ToString().Trim());
                    listCoursesStudentsCorrelationYear.Add(dr["Year"].ToString().Trim());
                    listCoursesStudentsCorrelationClassID.Add(dr["ClassID"].ToString().Trim());
                }
                dr.Close();

                #region 同步作业数据
                for (int i = 0; i < listCoursesStudentsCorrelationCourseNo.Count; i++)
                {

                    List<string> listCoursesStudentsCorrelationSchoolWorkNotifyId = new List<string>();

                    parameters = new SqlParameter[]{
                    new SqlParameter("@courseNo",listCoursesStudentsCorrelationCourseNo[i]),
                    new SqlParameter("@classID",listCoursesStudentsCorrelationClassID[i]),
                    new SqlParameter("@termTag",listCoursesStudentsCorrelationYear[i])
                };

                    dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                "select [schoolWorkNotifyId],[courseNo],[classID],[termTag] from [usta_SchoolWorkNotify] where courseNo=@courseNo AND classID=@classID AND termTag=@termTag;", parameters);
                    while (dr.Read())
                    {
                        listCoursesStudentsCorrelationSchoolWorkNotifyId.Add(dr["schoolWorkNotifyId"].ToString().Trim());
                    }
                    dr.Close();

                    for (int j = 0; j < listCoursesStudentsCorrelationSchoolWorkNotifyId.Count; j++)
                    {
                        //设置是否已经存在此作业数据的标识位
                        bool isExistSchoolWork = false;

                        parameters = new SqlParameter[]{
                    new SqlParameter("@schoolWorkNofityId",listCoursesStudentsCorrelationSchoolWorkNotifyId[j]),
                    new SqlParameter("@studentNo",item)
                };

                        dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
               "select [schoolWorkNofityId],[studentNo] from [usta_SchoolWorks] where schoolWorkNofityId=@schoolWorkNofityId AND studentNo=@studentNo;", parameters);
                        while (dr.Read())
                        {
                            isExistSchoolWork = true;
                        }
                        dr.Close();

                        //如果不存在此作业数据
                        if (!isExistSchoolWork)
                        {
                            sb.Append("INSERT INTO [usta_SchoolWorks]([schoolWorkNofityId],[studentNo]) VALUES(" +
                            listCoursesStudentsCorrelationSchoolWorkNotifyId[j]
                            + ",'" + item
                            + "');");
                            //SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                            //"INSERT INTO [usta_SchoolWorks]([schoolWorkNofityId],[studentNo]) VALUES('" +
                            //listCoursesStudentsCorrelationSchoolWorkNotifyId[j]
                            //+ "','" + item
                            //+ "')");
                        }
                    }
                }
                #endregion

                #region 同步实验数据
                for (int i = 0; i < listCoursesStudentsCorrelationCourseNo.Count; i++)
                {
                    parameters = new SqlParameter[]{
                    new SqlParameter("@courseNo",listCoursesStudentsCorrelationCourseNo[i]),
                    new SqlParameter("@classID",listCoursesStudentsCorrelationClassID[i]),
                    new SqlParameter("@termTag",listCoursesStudentsCorrelationYear[i])
                };

                    List<string> listCoursesStudentsCorrelationExperimentResourceId = new List<string>();

                    dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
                "select [experimentResourceId],[courseNo],[classID],[termTag] from [usta_ExperimentResources] where courseNo=@courseNo AND classID=@classID AND termTag=@termTag;", parameters);
                    while (dr.Read())
                    {
                        listCoursesStudentsCorrelationExperimentResourceId.Add(dr["experimentResourceId"].ToString().Trim());
                    }
                    dr.Close();

                    for (int j = 0; j < listCoursesStudentsCorrelationExperimentResourceId.Count; j++)
                    {
                        //设置是否已经存在此实验数据的标识位
                        bool isExistExperiment = false;

                        parameters = new SqlParameter[]{
                    new SqlParameter("@experimentResourceId",listCoursesStudentsCorrelationExperimentResourceId[j]),
                    new SqlParameter("@studentNo",item)
                };

                        dr = SqlHelper.ExecuteReader(conn1, CommandType.Text,
               "select [experimentResourceId],[studentNo] from [usta_Experiments] where experimentResourceId=@experimentResourceId AND studentNo=@studentNo;", parameters);
                        while (dr.Read())
                        {
                            isExistExperiment = true;
                        }
                        dr.Close();

                        //如果不存在此实验数据
                        if (!isExistExperiment)
                        {
                            sb.Append("INSERT INTO [usta_Experiments]([experimentResourceId],[studentNo]) VALUES(" +
                            listCoursesStudentsCorrelationExperimentResourceId[j]
                            + ",'" + item
                            + "');");
                            //SqlHelper.ExecuteNonQuery(conn1, CommandType.Text,
                            //"INSERT INTO [usta_Experiments]([experimentResourceId],[studentNo]) VALUES('" +
                            //listCoursesStudentsCorrelationExperimentResourceId[j]
                            //+ "','" + item
                            //+ "');");
                        }
                    }
                }
                #endregion
            }



            #region 批量更新操作

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //首先删除多余的作业和实验数据
                    SqlHelper.ExecuteNonQuery(conn1, CommandType.StoredProcedure, "spSchoolWorkAndExperimentsDataSynchronize");
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        SqlHelper.ExecuteNonQuery(conn1, CommandType.Text, sb.ToString());
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    throw ex;
                }
            }
            conn1.Close();

            #endregion

        }
        #endregion

    }
}