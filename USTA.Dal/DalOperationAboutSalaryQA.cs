using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using USTA.Model;
using System.Data;
using USTA.Common;

namespace USTA.Dal
{
    public class DalOperationAboutSalaryQA
    {

        private SqlConnection conn
        {
            set;
            get;
        }

        public DalOperationAboutSalaryQA()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }

        public void AddSalaryQA(SalaryQA salaryQA) 
        {
            try
            {
                string commandString = "INSERT INTO usta_salaryQA(userId, salaryId, qaContent, salaryType, createdTime) Values(@userId, @salaryId, @qaContent, @salaryType, @createdTime)";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@userId", salaryQA.teacher.teacherNo),
                new SqlParameter("@salaryId", salaryQA.salaryId),
                new SqlParameter("@qaContent", salaryQA.qaContent),
                new SqlParameter("@salaryType", salaryQA.salaryType),
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

        public List<SalaryQA> GetSalaryQA(int salaryId, int salaryType) 
        {
            List<SalaryQA> salaryQAs = new List<SalaryQA>();
            try
            {
                string commandString = "SELECT usta_salaryQA.*, usta_TeachersList.teacherName FROM usta_salaryQA, usta_TeachersList WHERE salaryType = @salaryType AND salaryId = @salaryId AND usta_salaryQA.userId = usta_TeachersList.teacherNo ORDER BY usta_salaryQA.createdTime DESC ";

                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@salaryId", salaryId),
                new SqlParameter("@salaryType", salaryType)
            };

                SqlDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                BuildSalaryQA(reader, salaryQAs);
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

            return salaryQAs;
        }

        private List<SalaryQA> BuildSalaryQA(SqlDataReader reader, List<SalaryQA> salaryQAs) 
        {
            
            SalaryQA salaryQA;
            while (reader.Read()) 
            {
                salaryQA = new SalaryQA();
                salaryQA.salaryQaId = int.Parse(reader["salaryqaId"].ToString().Trim());
                TeachersList teacher = new TeachersList();
                teacher.teacherNo = reader["userId"].ToString().Trim();
                teacher.teacherName = reader["teacherName"].ToString().Trim();
                salaryQA.teacher = teacher;

                salaryQA.salaryId = int.Parse(reader["salaryId"].ToString().Trim());
                salaryQA.qaContent = reader["qaContent"].ToString().Trim();
                salaryQA.salaryType = int.Parse(reader["salaryType"].ToString().Trim());
                salaryQA.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());

                salaryQAs.Add(salaryQA);
            }
            return salaryQAs;
        }
    }
}
