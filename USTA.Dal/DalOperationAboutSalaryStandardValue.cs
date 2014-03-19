using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using USTA.Model;
using USTA.Common;

namespace USTA.Dal
{
    public class DalOperationAboutSalaryStandardValue
    {
        #region
        /// <summary>
        /// 
        /// </summary>
        public SqlConnection conn
        {
            set;
            get;
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        public DalOperationAboutSalaryStandardValue() {

            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="standardValue"></param>
        public void AddStandardValue(SalaryStandardValue standardValue) 
        {
            try 
            {
                string commandString = "INSERT INTO usta_SalaryStandardValue(salaryItemId, salaryItemValue, createdTime) VALUES(@salaryItemId, @salaryItemValue, @createdTime)";

                SqlParameter[] parameters = new SqlParameter[]{
              new SqlParameter("@salaryItemId", standardValue.SalaryItemId),
              new SqlParameter("@salaryItemValue", standardValue.SalaryItemValue),
              new SqlParameter("@createdTime", DateTime.Now)
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
        /// 
        /// </summary>
        public void DelStandardValue(int standardValueId) {

            try 
            {
                string commandString = "DELETE FROM usta_SalaryStandardValue WHERE salaryStandardValueId = @standardValueId";
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@standardValueId", standardValueId)
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
        /// 
        /// </summary>
        public List<SalaryStandardValue> GetStandardValueBySalaryItemId(int salaryItemId) 
        {
            List<SalaryStandardValue> standardValueList = new List<SalaryStandardValue>();
            try 
            {
                string commandString = "SELECT * FROM usta_SalaryStandardValue WHERE salaryItemId = @salaryItemId ORDER BY salaryItemValue DESC";

                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@salaryItemId", salaryItemId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);

                BuildSalaryStandardValue(reader, standardValueList);
                reader.Close();
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
            return standardValueList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="standardValueList"></param>
        private void BuildSalaryStandardValue(IDataReader reader, List<SalaryStandardValue> standardValueList) 
        {
            SalaryStandardValue standardValue;
            while (reader.Read()) {

                standardValue = new SalaryStandardValue();
                standardValue.SalaryStandardValueId = int.Parse(reader["SalaryStandardValueId"].ToString().Trim());
                standardValue.SalaryItemId = int.Parse(reader["SalaryItemId"].ToString().Trim());
                standardValue.SalaryItemValue = float.Parse(reader["salaryItemValue"].ToString().Trim());
                standardValue.CreatedTime = DateTime.Parse(reader["createdTime"].ToString().Trim());

                standardValueList.Add(standardValue);
            }
        }

        #endregion
    }
}
