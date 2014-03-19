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
    public class DalOperationAboutReim :DalBase
    {
        #region

        public DalOperationAboutReim()
        {
        }
        #endregion

        #region
        public void AddReim(Reim reim) 
        {
            try 
            {
                string commandString = "INSERT INTO usta_Reim (name, comment, createdTime) VALUES(@name, @comment, @createdTime)";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", reim.name),
                new SqlParameter("@comment", reim.comment),
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

        public void UpdateReim(Reim reim) 
        {
            try
            {
                string commandString = "UPDATE usta_Reim SET name = @name, @comment = @comment WHERE id = @reimId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", reim.name),
                new SqlParameter("@comment", reim.comment),
                new SqlParameter("@reimId", reim.id)
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

        public List<Reim> GetAllReims()
        {
            List<Reim> reims = new List<Reim>();
            try
            {
                string commandString = "SELECT * FROM usta_Reim ORDER BY createdTime DESC";

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);
                BuildReims(reader, reims);
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
            return reims;
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public Reim GetReim(int reimId) 
        {
            Reim reim = null;
            try
            {   
                string commandString = "SELECT * FROM usta_Reim WHERE id = @reimId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@reimId", reimId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                List<Reim> reimList = new List<Reim>();
                BuildReims(reader, reimList);
                reader.Close();
                if (reimList != null && reimList.Count != 0)
                {
                    reim = reimList[0];
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
            return reim;
        
        }

        private void BuildReims(IDataReader reader, List<Reim> reims)
        {
            Reim reim;
            while (reader.Read()) 
            {
                reim = new Reim();
                reim.id = int.Parse(reader["id"].ToString().Trim());
                reim.name = reader["name"].ToString().Trim();
                reim.comment = reader["comment"].ToString().Trim();
                reim.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());

                reims.Add(reim);
            }
        }

        #endregion


    }
}
