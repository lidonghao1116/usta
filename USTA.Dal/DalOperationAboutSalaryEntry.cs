using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using USTA.Model;
using USTA.Common;
using System.Web;

namespace USTA.Dal
{
    /// <summary>
    /// 薪酬条目操作类
    /// </summary>
    public class DalOperationAboutSalaryEntry
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
        #endregion

        #region
        /// <summary>
        /// 构造函数
        /// </summary>
        public DalOperationAboutSalaryEntry()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region 方法集合
        /// <summary>
        /// 添加一条SalaryEntry记录
        /// </summary>
        /// <param name="salaryEntry"></param>
        public void AddSalaryEntry(SalaryEntry salaryEntry)
        {
            try
            {
                string commandString = "INSERT INTO [USTA].[dbo].[usta_salaryEntry]([teacherNo],[teacherType],[courseNo],[atCourseType], [salaryInItemValueList],[salaryOutItemValueList],[salaryInAdjustFactor],[salaryOutAdjustFactor],[teachPeriod],[teachAssiPeriod],[termTag],[teacherCostWithTax],[teacherCostWithoutTax], [teacherTotalCost],[salaryMonth],[salaryEntryStatus],[memo] , [createdTime]) VALUES (@teacherNo, @teacherType,@courseNo, @atCourseType, @salaryInItemValueList, @salaryOutItemValueList, @salaryInAdjustFactor, @salaryOutAdjustFactor, @teachPeriod, @teachAssiPeriod, @termTag, @teacherCostWithTax, @teacherCostWithoutTax, @teacherTotalCost, @salaryMonth,@salaryEntryStatus,@memo, @createdTime)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@teacherNo", salaryEntry.teacher.teacherNo),
                    new SqlParameter("@teacherType", salaryEntry.teacherType),
                    new SqlParameter("@courseNo",   salaryEntry.course == null ? null :  salaryEntry.course.courseNo),
                    new SqlParameter("@atCourseType", salaryEntry.atCourseType),
                    new SqlParameter("@salaryInItemValueList", salaryEntry.GetSalaryInItemValueList()),
                    new SqlParameter("@salaryOutItemValueList", salaryEntry.GetSalaryOutItemValueList()),
                    new SqlParameter("@salaryInAdjustFactor", salaryEntry.salaryInAdjustFactor),
                    new SqlParameter("@salaryOutAdjustFactor", salaryEntry.salaryOutAdjustFactor),
                    new SqlParameter("@teachPeriod", salaryEntry.teachPeriod),
                    new SqlParameter("@teachAssiPeriod", salaryEntry.teachAssiPeriod),
                    new SqlParameter("@termTag", salaryEntry.termTag),
                    new SqlParameter("@teacherCostWithTax", salaryEntry.teacherCostWithTax),
                    new SqlParameter("@teacherCostWithoutTax", salaryEntry.teacherCostWithoutTax),
                    new SqlParameter("@teacherTotalCost", salaryEntry.teacherTotalCost),
                    new SqlParameter("@salaryMonth", salaryEntry.salaryMonth),
                    new SqlParameter("@salaryEntryStatus", "1"),
                    new SqlParameter("@memo", salaryEntry.memo),
                    new SqlParameter("@createdTime", DateTime.Now.ToString())
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
        /// 更新薪酬的月份、备注及状态信息
        /// </summary>
        /// <param name="salaryEntry"></param>
        public void UpdateSalaryEntryMonthMemoAndStatus(SalaryEntry salaryEntry) 
        {

            try {
                string commandString = "UPDATE usta_salaryEntry SET salaryMonth = @salaryMonth, salaryEntryStatus = @salaryEntryStatus, memo = @memo WHERE salaryEntryId = @salaryEntryId";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@salaryMonth", salaryEntry.salaryMonth),
                    new SqlParameter("@salaryEntryStatus", salaryEntry.salaryEntryStatus),
                    new SqlParameter("@memo", salaryEntry.memo),
                    new SqlParameter("@salaryEntryId", salaryEntry.salaryEntryId)
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
        /// 删除一条SalaryEntry记录
        /// </summary>
        /// <param name="salaryEntryId"></param>
        public void DelSalaryEntry(int salaryEntryId)
        {
            try
            {
                string commandString = "DELETE FROM usta_salaryEntry WHERE salaryEntryId = @salaryEntryId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@salaryEntryId", salaryEntryId)
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
        /// 教师确认SalaryEntry信息
        /// </summary>
        /// <param name="salaryEntryId"></param>
        /// <param name="newStatus"></param>
        public void updateSalaryEntryStatus(int salaryEntryId, int newStatus)
        {
            try
            {
                string commandString = "UPDATE usta_salaryEntry SET salaryEntryStatus = @newStatus WHERE salaryEntryId = @salaryEntryId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@salaryEntryId", salaryEntryId),
                new SqlParameter("@newStatus", newStatus)
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
        /// 根据salaryEntryId查询一条SalaryEntry记录
        /// </summary>
        /// <param name="salaryEntryId"></param>
        /// <returns></returns>
        public SalaryEntry GetSalaryEntry(int salaryEntryId)
        {
            SalaryEntry salaryEntry = null;
            try
            {
                string commandString = "SELECT * FROM usta_salaryEntry WHERE salaryEntryId = @salaryEntryId";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@salaryEntryId", salaryEntryId)
                };

                IDataReader dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                List<SalaryEntry> salaryEntries = new List<SalaryEntry>();
                BuildSalaryEntry(dataReader, salaryEntries);
                dataReader.Close();
                if (salaryEntries != null && salaryEntries.Count == 1)
                {
                    salaryEntry = salaryEntries[0];
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

            return salaryEntry;

        }

        /// <summary>
        /// 根据查询条件查询相应的SalaryEntry列表信息
        /// </summary>
        /// <param name="teacherNo"></param>
        /// <param name="termTag"></param>  
        /// <param name="salaryMonth"></param>
        /// <param name="teacherType"></param>
        /// <returns></returns>
        public List<SalaryEntry> GetSalaryEntrys(string teacherNo, string termTag, string salaryMonth, int teacherType, int salaryEntryStatus)
        {
            List<SalaryEntry> salaryEntries = new List<SalaryEntry>();
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                string teacherCase = null;
                string termTagCase = null;
                string salaryMonthCase = null;
                string teacherTypeCase = null;
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

                if (salaryMonth != null && salaryMonth.Trim().Length != 0)
                {
                    salaryMonthCase = "salaryMonth = @salaryMonth";
                    parameterList.Add(new SqlParameter("@salaryMonth", salaryMonth.Trim()));
                }

                if (teacherType != 0)
                {
                    teacherTypeCase = "teacherType = @teacherType";
                    parameterList.Add(new SqlParameter("@teacherType", teacherType));
                }

                if (salaryEntryStatus > 0 && salaryEntryStatus < 4)
                {   // 单个状态
                    statusCase = "salaryEntryStatus = @salaryEntryStatus ";
                    parameterList.Add(new SqlParameter("@salaryEntryStatus", salaryEntryStatus));
                }
                else if (salaryEntryStatus == 4)
                {   //多个状态：已发放(未确认, 已确认)
                    statusCase = "salaryEntryStatus >= 2 ";
                }

                bool hasPrefix = false;
                string commandString = "SELECT * FROM usta_salaryEntry WHERE ";
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

                if (salaryMonthCase != null)
                {
                    commandString += ((hasPrefix ? " AND " : "") + salaryMonthCase);
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

                commandString += " ORDER BY createdTime DESC, salaryMonth DESC";

                IDataReader dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());

                BuildSalaryEntry(dataReader, salaryEntries);
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
            return salaryEntries;
        }

        public TotalSalaryModel GetTotalSalaryEntryValues(string teacherNo, string termTag, string salaryMonth, int teacherType, int salaryEntryStatus)
        {
            TotalSalaryModel salaryModel = new TotalSalaryModel()
            {
                salaryWithTax = 0,
                salaryWithoutTax = 0,
                salaryTotal = 0
            };
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                string teacherCase = null;
                string termTagCase = null;
                string salaryMonthCase = null;
                string teacherTypeCase = null;
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

                if (salaryMonth != null && salaryMonth.Trim().Length != 0)
                {
                    salaryMonthCase = "salaryMonth = @salaryMonth";
                    parameterList.Add(new SqlParameter("@salaryMonth", salaryMonth.Trim()));
                }

                if (teacherType != 0)
                {
                    teacherTypeCase = "teacherType = @teacherType";
                    parameterList.Add(new SqlParameter("@teacherType", teacherType));
                }

                if (salaryEntryStatus > 0 && salaryEntryStatus < 4)
                {   // 单个状态
                    statusCase = "salaryEntryStatus = @salaryEntryStatus ";
                    parameterList.Add(new SqlParameter("@salaryEntryStatus", salaryEntryStatus));
                }
                else if (salaryEntryStatus == 4)
                {   //多个状态：已发放(未确认, 已确认)
                    statusCase = "salaryEntryStatus >= 2 ";
                }

                bool hasPrefix = false;

                string commandString;
                if (teacherCase == null && termTagCase == null && salaryMonthCase == null && teacherTypeCase == null && statusCase == null)
                {

                    commandString = "SELECT SUM(teacherCostWithTax) as salaryWithTax, SUM(teacherCostWithoutTax) as salaryWithoutTax, SUM(teacherTotalCost) as totalCost FROM usta_salaryEntry ";

                }
                else
                {
                    commandString = "SELECT SUM(teacherCostWithTax) as salaryWithTax, SUM(teacherCostWithoutTax) as salaryWithoutTax, SUM(teacherTotalCost) as totalCost FROM usta_salaryEntry WHERE ";
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

                    if (salaryMonthCase != null)
                    {
                        commandString += ((hasPrefix ? " AND " : "") + salaryMonthCase);
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
                if (dataReader.Read())
                {
                    if (string.IsNullOrWhiteSpace(dataReader["salaryWithTax"].ToString()))
                    {
                        salaryModel.salaryWithTax = 0.0;
                    }
                    else
                    {
                        salaryModel.salaryWithTax = CommonUtility.ConvertFormatedDouble("{0:0.00}", dataReader["salaryWithTax"].ToString().Trim());
                    }

                    if (string.IsNullOrWhiteSpace(dataReader["salaryWithoutTax"].ToString()))
                    {
                        salaryModel.salaryWithoutTax = 0.0;
                    }
                    else
                    {
                        salaryModel.salaryWithoutTax = CommonUtility.ConvertFormatedDouble("{0:0.00}", dataReader["salaryWithoutTax"].ToString().Trim());
                    }

                    if (string.IsNullOrWhiteSpace(dataReader["totalCost"].ToString()))
                    {
                        salaryModel.salaryTotal = 0.0;
                    }
                    else
                    {
                        salaryModel.salaryTotal = CommonUtility.ConvertFormatedDouble("{0:0.00}", dataReader["totalCost"].ToString().Trim());
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
            return salaryModel;

        }


        /// <summary>
        /// 获得所有的SalaryEntry记录
        /// </summary>
        /// <returns></returns>
        public List<SalaryEntry> GetAllSalaryEntry()
        {
            List<SalaryEntry> salaryEntries = new List<SalaryEntry>();
            try
            {
                string commandString = "SELECT * FROM usta_salaryEntry ORDER BY createdTime DESC, salaryMonth DESC";
                IDataReader dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);

                BuildSalaryEntry(dataReader, salaryEntries);
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
            return salaryEntries;
        }

        public List<SalaryEntry> GetAllSalaryEntry(string atCourseType)
        {
            List<SalaryEntry> salaryEntries = new List<SalaryEntry>();
            try
            {
                string commandString = "SELECT * FROM usta_salaryEntry WHERE atCourseType = @atCourseType ORDER BY createdTime DESC, salaryMonth DESC";

                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@atCourseType", atCourseType)
            };

                IDataReader dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);

                BuildSalaryEntry(dataReader, salaryEntries);
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
            return salaryEntries;
        }

        /// <summary>
        /// 从DataReader中转化并构造SalaryEntry记录集
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private void BuildSalaryEntry(IDataReader dataReader, List<SalaryEntry> salaryEntries)
        {

            DalOperationAboutTeacher dalt = new DalOperationAboutTeacher();
            DalOperationAboutCourses dalc = new DalOperationAboutCourses();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    SalaryEntry salaryEntry = new SalaryEntry();
                    salaryEntry.salaryEntryId = int.Parse(dataReader["salaryEntryId"].ToString().Trim());
                    TeachersList teacher = dalt.GetTeacherById(dataReader["teacherNo"].ToString().Trim());
                    salaryEntry.teacher = teacher;
                    salaryEntry.teacherType = int.Parse(dataReader["teacherType"].ToString().Trim());
                    salaryEntry.termTag = dataReader["termTag"].ToString().Trim();
                    Courses course = dalc.GetCoursesByNo(dataReader["courseNo"].ToString().Trim(), salaryEntry.termTag);
                    salaryEntry.course = course;
                    salaryEntry.atCourseType = dataReader["atCourseType"].ToString().Trim();

                    salaryEntry.SetSalaryInItemValueList(dataReader["salaryInItemValueList"].ToString().Trim(), true);
                    salaryEntry.SetSalaryOutItemValueList(dataReader["salaryOutItemValueList"].ToString().Trim(), true);
                    if (dataReader["salaryInAdjustFactor"] != null)
                    {
                        salaryEntry.salaryInAdjustFactor = float.Parse(dataReader["salaryInAdjustFactor"].ToString().Trim());
                    }

                    if (dataReader["salaryOutAdjustFactor"] != null)
                    {
                        salaryEntry.salaryOutAdjustFactor = float.Parse(dataReader["salaryOutAdjustFactor"].ToString().Trim());
                    }

                    if (dataReader["teachPeriod"] != null)
                    {
                        salaryEntry.teachPeriod = int.Parse(dataReader["teachPeriod"].ToString().Trim());
                    }

                    if (dataReader["teachAssiPeriod"] != null)
                    {
                        salaryEntry.teachAssiPeriod = int.Parse(dataReader["teachAssiPeriod"].ToString().Trim());
                    }

                    
                    salaryEntry.teacherCostWithTax = float.Parse(dataReader["teacherCostWithTax"].ToString().Trim());
                    salaryEntry.teacherCostWithoutTax = float.Parse(dataReader["teacherCostWithoutTax"].ToString().Trim());
                    salaryEntry.teacherTotalCost = float.Parse(dataReader["teacherTotalCost"].ToString().Trim());
                    salaryEntry.salaryMonth = dataReader["salaryMonth"].ToString().Trim();
                    salaryEntry.salaryEntryStatus = int.Parse(dataReader["salaryEntryStatus"].ToString());
                    salaryEntry.createdTime = DateTime.Parse(dataReader["createdTime"].ToString().Trim());
                    salaryEntry.memo = dataReader["memo"].ToString().Trim();
                    salaryEntries.Add(salaryEntry);
                }
            }

        }
        #endregion
    }
}