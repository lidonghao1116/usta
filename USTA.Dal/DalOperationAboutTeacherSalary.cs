using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace USTA.Dal
{

    using USTA.Model;
    using USTA.Common;

    /// <summary>
    /// 教师/助教教学工作总薪酬操作类
    /// </summary>
    public class DalOperationAboutTeacherSalary
    {
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
        public DalOperationAboutTeacherSalary()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }

        /// <summary>
        /// 添加一条教师/助教的教学工作及薪酬标准信息
        /// </summary>
        /// <param name="salary"></param>
        public void AddTeacherSalary(TeacherSalary salary)
        {
             try
            {
                string commandString = "INSERT INTO [USTA].[dbo].[usta_TeacherSalarySummary]([teacherNo], [teacherType],[courseNo],[atCourseType],[teachPeriod],[experPeriod], [salaryItemValueList], [totalTeachCost], [termTag],[isConfirm], [memo], [createdTime]) VALUES(@teacherNo, @teacherType,@courseNo,@atCourseType,@teachPeriod,@experPeriod,@salaryItemValueList, @totalTeachCost, @termTag,@isConfirm, @memo, @createdTime)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@teacherNo", salary.teacher.teacherNo),
                    new SqlParameter("teacherType", salary.teacherType),
                    new SqlParameter("@courseNo", salary.course == null ? null : salary.course.courseNo),
                    new SqlParameter("@atCourseType", salary.atCourseType),
                    new SqlParameter("@teachPeriod", salary.teachPeriod),
                    new SqlParameter("@experPeriod", salary.experPeriod),
                    new SqlParameter("@salaryItemValueList", salary.GetSalaryInItemValueList()),
                    new SqlParameter("@totalTeachCost", salary.totalTeachCost),
                    new SqlParameter("@termTag", salary.termTag),
                    new SqlParameter("@isConfirm", false),
                    new SqlParameter("@memo", salary.memo),
                    new SqlParameter("@createdTime", DateTime.Now)
                };

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally {
                conn.Close();
           }
        }
        /// <summary>
        /// 教师确认薪酬标准信息
        /// </summary>
        /// <param name="teacherSalaryId"></param>
        public void confirmTeacherSalary(int teacherSalaryId)
        {
            try
            {
                string commandString = "UPDATE usta_TeacherSalarySummary SET isConfirm = @isConfirm WHERE teacherSalaryId = @teacherSalaryId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@teacherSalaryId", teacherSalaryId),
                new SqlParameter("@isConfirm", true)
            };

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);

            }
            catch (Exception ex)
            {

                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally {

                conn.Close();
            }
        }

        /// <summary>
        /// 删除一条教师/助教的教学工作及薪酬标准信息
        /// </summary>
        /// <param name="teacherSalaryId"></param>
        public void DelTeacherSalary(int teacherSalaryId)
        {
            try
            {
                string commandString = "DELETE FROM usta_TeacherSalarySummary WHERE teacherSalaryId = @teacherSalaryId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@teacherSalaryId", teacherSalaryId)
            };

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);

            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally {
                conn.Close();
            }
            
        }

        public TeacherSalary GetTeacherSalaryBySalaryId(int teacherSalaryId) 
        {
            TeacherSalary teacherSalary = null;
            try
            {
                string commandString = "SELECT * FROM usta_TeacherSalarySummary WHERE teacherSalaryId = @teacherSalaryId";


                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@teacherSalaryId", teacherSalaryId)
            };
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                List<TeacherSalary> salaries = new List<TeacherSalary>();
                buildTeacherSalarys(reader, salaries);
                reader.Close();
                if (salaries != null && salaries.Count == 1)
                {
                    teacherSalary = salaries[0];
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
            return teacherSalary;
        
        }

        /// <summary>
        /// 根据学期标识获得指定学期指定老师及对应课程的薪酬预算信息
        /// </summary>
        /// <param name="teacherNo"></param>
        /// <param name="courseNo"></param>
        /// <param name="termTag"></param>
        /// <returns></returns>
        public TeacherSalary GetTeacherSalaryByTidAndCidAndTermTag(string teacherNo, string courseNo, int teacherType, string termTag)
        {
            TeacherSalary teacherSalary = null;
            try 
            {
                if (termTag == null)
                {
                    termTag = DalCommon.GetTermTag(conn);
                }
                string commandString = null;
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (courseNo == null || courseNo.Trim().Length == 0)
                {
                    commandString = "SELECT * FROM usta_TeacherSalarySummary WHERE teacherNo = @teacherNo And termTag = @termTag AND teacherType = @teacherType";
                }
                else
                {
                    commandString = "SELECT * FROM usta_TeacherSalarySummary WHERE teacherNo = @teacherNo And courseNo = @courseNo And termTag = @termTag AND teacherType = @teacherType";
                    parameters.Add(new SqlParameter("@courseNo", courseNo));
                }
                parameters.Add(new SqlParameter("@teacherNo", teacherNo));
                parameters.Add(new SqlParameter("@termTag", termTag));
                parameters.Add(new SqlParameter("@teacherType", teacherType));

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters.ToArray());
                List<TeacherSalary> salaries = new List<TeacherSalary>();
                buildTeacherSalarys(reader, salaries);
                reader.Close();
                if (salaries != null && salaries.Count == 1)
                {
                    teacherSalary = salaries[0];
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
            return teacherSalary;
        }

        public double GetTeacherSalarysValue(string teacherNo, int teacherType, string termTag, int status) 
        {
            double totalSalaryValue = 0;
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                string teacherCase = null;
                string teacherTypeCase = null;
                string termTagCase = null;
                string statusCase = null;

                if (teacherNo != null && teacherNo.Trim().Length != 0)
                {
                    if (!teacherNo.Contains('\''))
                    {
                        teacherNo = "'" + teacherNo + "'";
                    }
                    teacherCase = "teacherNo in (" + teacherNo + ")";
                }

                if (termTag != null && termTag.Trim().Length != 0)
                {

                    termTagCase = "termTag = @termTag";
                    parameterList.Add(new SqlParameter("@termTag", termTag.Trim()));
                }

                if (teacherType != 0)
                {
                    teacherTypeCase = "teacherType = @teacherType";
                    parameterList.Add(new SqlParameter("@teacherType", teacherType));
                }

                if (status != 0)
                {
                    if (status == 1)
                    {
                        statusCase = "isConfirm = 0";
                    }
                    else if (status == 2)
                    {
                        statusCase = "isConfirm = 1";
                    }
                }

                bool hasPrefix = false;
                string commandString;
                if (teacherCase == null && termTagCase == null && teacherTypeCase == null && statusCase == null)
                {

                    commandString = "SELECT SUM(totalTeachCost) as totalTeachCost FROM usta_TeacherSalarySummary";
                }
                else {

                    commandString = "SELECT SUM(totalTeachCost) as totalTeachCost FROM usta_TeacherSalarySummary WHERE ";

                    if (teacherCase != null)
                    {
                        commandString += ((hasPrefix ? " AND " : "") + teacherCase);
                        hasPrefix = true;
                    }

                    if (termTagCase != null)
                    {
                        commandString += ((hasPrefix ? " AND " : "") + termTagCase);
                        hasPrefix = true;
                    }

                    if (teacherTypeCase != null)
                    {
                        commandString += ((hasPrefix ? " AND " : "") + teacherTypeCase);
                        hasPrefix = true;
                    }

                    if (statusCase != null)
                    {
                        commandString += ((hasPrefix ? " AND " : "") + statusCase);
                        hasPrefix = true;
                    }
                }

                IDataReader dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());
                if (dataReader.Read()) {
                    if (!string.IsNullOrWhiteSpace(dataReader["totalTeachCost"].ToString()))
                    {
                        totalSalaryValue = CommonUtility.ConvertFormatedDouble("{0:0.00}", dataReader["totalTeachCost"].ToString().Trim());
                    }
                    
                }
                dataReader.Close();
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

            return totalSalaryValue;
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherNo"></param>
        /// <param name="teacherType"></param>
        /// <param name="termTag"></param>
        /// <returns></returns>
        public List<TeacherSalary> GetTeacherSalarys(string teacherNo, int teacherType, string termTag, int status) 
        {
            List<TeacherSalary> teacherSalaries = new List<TeacherSalary>();

            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                string teacherCase = null;
                string teacherTypeCase = null;
                string termTagCase = null;
                string statusCase = null;

                if (teacherNo != null && teacherNo.Trim().Length != 0)
                {
                    if (!teacherNo.Contains('\''))
                    {
                        teacherNo = "'" + teacherNo + "'";
                    }
                    teacherCase = "teacherNo in (" + teacherNo + ")";
                }

                if (termTag != null && termTag.Trim().Length != 0)
                {

                    termTagCase = "termTag = @termTag";
                    parameterList.Add(new SqlParameter("@termTag", termTag.Trim()));
                }

                if (teacherType != 0)
                {
                    teacherTypeCase = "teacherType = @teacherType";
                    parameterList.Add(new SqlParameter("@teacherType", teacherType));
                }

                if (status != 0) 
                {
                    if (status == 1) {
                        statusCase = "isConfirm = 0";
                    }
                    else if (status == 2) 
                    {
                        statusCase = "isConfirm = 1";
                    }
                }

                bool hasPrefix = false;
                string commandString = "SELECT * FROM usta_TeacherSalarySummary WHERE ";

                if (teacherCase != null)
                {
                    commandString += ((hasPrefix ? " AND " : "") + teacherCase);
                    hasPrefix = true;
                }

                if (termTagCase != null)
                {
                    commandString += ((hasPrefix ? " AND " : "") + termTagCase);
                    hasPrefix = true;
                }

                if (teacherTypeCase != null)
                {
                    commandString += ((hasPrefix ? " AND " : "") + teacherTypeCase);
                    hasPrefix = true;
                }

                if (statusCase != null) 
                {
                    commandString += ((hasPrefix ? " AND " : "") + statusCase);
                    hasPrefix = true;
                }

                commandString += " ORDER BY createdTime DESC";

                IDataReader dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());
                buildTeacherSalarys(dataReader, teacherSalaries);
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally {
                conn.Close();
            }
            return teacherSalaries;
        
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TeacherSalary> GetAllTeacherSalary() 
        {
            List<TeacherSalary> salaries = new List<TeacherSalary>();
            try
            {
                string commandString = "SELECT * FROM usta_TeacherSalarySummary ORDER BY createdTime DESC";
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);

                buildTeacherSalarys(reader, salaries);
                reader.Close();
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally {
                conn.Close();
            }
            
            return salaries;
        
        }

        /// <summary>
        /// 把从数据库中读出的数据转化为对象实例
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="salaries"></param>
        /// <returns></returns>
        private void buildTeacherSalarys(IDataReader reader, List<TeacherSalary> salaries)
        {
            DalOperationAboutCourses courseDal = new DalOperationAboutCourses();
            DalOperationAboutTeacher teacherDal = new DalOperationAboutTeacher();
            while (reader.Read()) 
            {
                TeacherSalary salary = new TeacherSalary();
                salary.teacherSalaryId = int.Parse(reader["teacherSalaryId"].ToString().Trim()); 
                salary.teacher = teacherDal.GetTeacherById(reader["teacherNo"].ToString().Trim());
                salary.teacherType = int.Parse(reader["teacherType"].ToString().Trim());
                salary.termTag = reader["termTag"].ToString().Trim();
                if (reader["courseNo"] != null && reader["courseNo"].ToString().Trim().Length > 0) {
                    salary.course = courseDal.GetCoursesByNo(reader["courseNo"].ToString().Trim(), salary.termTag);
                    salary.atCourseType = int.Parse(reader["atCourseType"].ToString().Trim());
                    salary.teachPeriod = int.Parse(reader["teachPeriod"].ToString().Trim());
                    salary.experPeriod = int.Parse(reader["experPeriod"].ToString().Trim());
                }
                salary.SetSalaryInItemValueList(reader["salaryItemValueList"].ToString().Trim(), true);
                salary.totalTeachCost = float.Parse(reader["totalTeachCost"].ToString().Trim());
                
                salary.isConfirm = bool.Parse(reader["isConfirm"].ToString().Trim());
                salary.memo = reader["memo"].ToString().Trim();
                salary.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());

                salaries.Add(salary);
            }
        }
    }
}
