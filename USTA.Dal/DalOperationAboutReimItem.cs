using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using USTA.Model;
using System.Web;
using USTA.Common;


namespace USTA.Dal
{
    public class DalOperationAboutReimItem
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
        /// 构造方法
        /// </summary>
        public DalOperationAboutReimItem() 
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region
        /// <summary>
        /// 添加一个报销条目
        /// </summary>
        /// <param name="reimItem"></param>
        public void AddReimItem(ReimItem reimItem) 
        {
            try
            {
                string commandString = "INSERT INTO usta_ReimItem (projectId, reimId, value, memo, createdTime) VALUES(@projectId, @reimId, @value, @memo, @createdTime)";

                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@projectId", reimItem.project.id),
                new SqlParameter("@reimId", reimItem.reim.id),
                new SqlParameter("@value", reimItem.value),
                new SqlParameter("@memo", reimItem.memo),
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
        /// 更新一个报销条目
        /// </summary>
        /// <param name="reimItem"></param>
        public void UpdateReimItem(ReimItem reimItem) 
        {
            try
            {
                string commandString = "UPDATE usta_ReimItem SET projectId = @projectId, reimId = @reimId, value = @value, memo = @memo WHERE id = @id";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@projectId", reimItem.project.id),
                new SqlParameter("@reimId", reimItem.reim.id),
                new SqlParameter("@value", reimItem.value),
                new SqlParameter("@memo", reimItem.memo),
                new SqlParameter("@id", reimItem.id)
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
        /// 删除一个报销条目
        /// </summary>
        /// <param name="reimItemId"></param>
        public void DelReimItem(int reimItemId) 
        {
            try
            {
                string commandString = "DELETE FROM usta_ReimItem WHERE id = @reimItemId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@reimItemId", reimItemId)
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
        /// 根据id查询报销条目
        /// </summary>
        /// <param name="reimItemId"></param>
        /// <returns></returns>
        public ReimItem GetReimItem(int reimItemId)
        {
            ReimItem reimItem = null;
            try
            {
                string commandString = "SELECT usta_ReimItem.*, usta_Project.name as projectName, usta_Reim.name as ReimName FROM usta_ReimItem, usta_Project, usta_Reim WHERE usta_ReimItem.id = @reimItemId AND usta_ReimItem.projectId = usta_Project.id AND usta_ReimItem.reimId = usta_Reim.id";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@reimItemId", reimItemId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                List<ReimItem> reimItems = new List<ReimItem>();
                BuildReimItems(reader, reimItems);
                reader.Close();
                if (reimItems != null && reimItems.Count != 0)
                {
                    reimItem = reimItems[0];
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
            return reimItem;
        
        }

        /// <summary>
        /// 根据项目名称或id查询该项目的报销总金额
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ReimItem> GetProjectReimItems(string projectName, int projectId) 
        {
            List<ReimItem> reimItems = new List<ReimItem>();
            try
            {
                string commandString;
                if (projectId != 0)
                {

                    commandString = "SELECT usta_Project.id AS projectId, usta_Project.name AS projectName, temp_ReimItem.value AS value "
                                + " FROM (SELECT usta_ReimItem.projectId AS projectId, SUM(value) AS value "
                                    + " FROM usta_ReimItem "
                                    + " WHERE usta_ReimItem.projectId=" + projectId
                                    + " GROUP BY usta_ReimItem.projectId) "
                                    + " AS temp_ReimItem, usta_Project "
                                + " WHERE temp_ReimItem.projectId = usta_Project.id";
                }
                else if (!string.IsNullOrWhiteSpace(projectName))
                {
                    commandString = "SELECT usta_Project.id AS projectId, usta_Project.name AS projectName, temp_ReimItem.value AS value "
                                + " FROM (SELECT usta_ReimItem.projectId AS projectId, SUM(value) AS value "
                                    + " FROM usta_ReimItem "
                                    + " GROUP BY usta_ReimItem.projectId) "
                                    + " AS temp_ReimItem, usta_Project "
                                + " WHERE temp_ReimItem.projectId = usta_Project.id AND usta_Project.name LIKE '%" + projectName + "%'";
                }
                else
                {
                    commandString = "SELECT usta_Project.id AS projectId, usta_Project.name AS projectName, temp_ReimItem.value AS value "
                                    + " FROM (SELECT usta_ReimItem.projectId AS projectId, SUM(value) AS value "
                                        + " FROM usta_ReimItem "
                                        + " GROUP BY usta_ReimItem.projectId) "
                                        + " AS temp_ReimItem, usta_Project "
                                    + " WHERE temp_ReimItem.projectId = usta_Project.id";

                }

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);

                ReimItem reimItem;
                while (reader.Read())
                {
                    reimItem = new ReimItem();
                    reimItem.project = new Project()
                    {
                        id = int.Parse(reader["projectId"].ToString().Trim()),
                        name = reader["projectName"].ToString().Trim()
                    };
                    reimItem.value = CommonUtility.ConvertFormatedFloat("{0:0.00}", reader["value"].ToString().Trim());

                    reimItems.Add(reimItem);
                }
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
            
            return reimItems;
        }


        /// <summary>
        /// 查询某个项目指定报销项的报销记录
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public List<ReimItem> GetReimItemsForProjectAndReim(int projectId, int reimId) 
        {
            List<ReimItem> reimItems = new List<ReimItem>();
            try
            {   
                string commandString = "SELECT * FROM usta_ReimItem WHERE usta_ReimItem.projectId = @projectId AND usta_ReimItem.reimId = @reimId";

                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@projectId", projectId),
                new SqlParameter("@reimId", reimId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);

                ReimItem reimItem;
                while (reader.Read())
                {
                    reimItem = new ReimItem();
                    reimItem.id = int.Parse(reader["id"].ToString().Trim());
                    reimItem.value = CommonUtility.ConvertFormatedFloat("{0:0.00}", reader["value"].ToString().Trim());
                    reimItem.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());
                    reimItem.memo = reader["memo"].ToString().Trim();
                    reimItems.Add(reimItem);
                }
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
            return reimItems;
        }

        /// <summary>
        /// 根据projectName和reimid查询相应的报销金额总和
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public float GetReimItemValue(string projectName, int reimId) 
        {
            float reimItemValue = 0.0f;
            try {
                string projectCase = null;
                string reimCase = null;

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(projectName))
                {
                    projectCase = " AND usta_Project.name LIKE '%" + projectName + "%' ";
                }

                if (reimId != 0)
                {
                    reimCase = " AND reimId = @reimId";
                    parameters.Add(new SqlParameter("@reimId", reimId));
                }

                string commandString = "select SUM(value) AS value FROM usta_ReimItem, usta_Project WHERE usta_ReimItem.projectId = usta_Project.id ";

                if (projectCase != null)
                {
                    commandString +=  projectCase;
                }

                if (reimCase != null)
                {
                    commandString += reimCase;
                }

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters.ToArray());

                while (reader.Read())
                {
                    if (!string.IsNullOrWhiteSpace(reader["value"].ToString()))
                    {
                        reimItemValue = CommonUtility.ConvertFormatedFloat("{0:0.00}", reader["value"].ToString().Trim());
                    }
                }
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
            return reimItemValue;
        
        }

        /// <summary>
        /// 根据projectId和reimId查询相应的报销金额总和
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public float GetReimItemValue(int projectId, int reimId) 
        {
            float reimItemValue = 0.00f;
            try
            {
                string projectCase = null;
                string reimCase = null;

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (projectId != 0)
                {
                    projectCase = " projectId = @projectId ";
                    parameters.Add(new SqlParameter("@projectId", projectId));
                }

                if (reimId != 0)
                {
                    reimCase = " reimId = @reimId";
                    parameters.Add(new SqlParameter("@reimId", reimId));
                }

                string commandString = "SELECT SUM(value) AS value FROM usta_ReimItem ";
                bool hasPrefix = false;

                if (projectCase != null)
                {
                    commandString += ((hasPrefix ? " AND " : " WHERE ") + projectCase);
                    hasPrefix = true;
                }

                if (reimCase != null)
                {
                    commandString += ((hasPrefix ? "AND" : " WHERE ") + reimCase);
                    hasPrefix = true;
                }

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters.ToArray());

                while (reader.Read())
                {
                    if (!string.IsNullOrWhiteSpace(reader["value"].ToString()))
                    {
                        reimItemValue = CommonUtility.ConvertFormatedFloat("{0:0.00}", reader["value"].ToString().Trim());
                    }
                }
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
            return reimItemValue;
        }

        /// <summary>
        /// 查询所有的报销条目：同一项目的相同报销项不同报销记录分开显示
        /// </summary>
        /// <returns></returns>
        public List<ReimItem> GetAllDistinctReimItems() 
        {
            List<ReimItem> reimItems = new List<ReimItem>();
            try
            {   
                string commandString = "SELECT usta_ReimItem.id AS id, usta_ReimItem.projectId as projectId, usta_Project.name as projectName, usta_ReimItem.reimId as reimId, usta_Reim.name as reimName, usta_ReimItem.value as value, usta_ReimItem.createdTime as createdTime, usta_ReimItem.memo as memo "
                    + " FROM usta_ReimItem, usta_Reim, usta_Project "
                    + " WHERE usta_ReimItem.projectId = usta_Project.id AND usta_ReimItem.reimId = usta_Reim.id ";

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);
                BuildReimItems(reader, reimItems);
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
            
            return reimItems;
        }



        /// <summary>
        /// 查询所有的报销条目：同一项目的相同报销项目金额进行合并
        /// 如果需要查询同一项目的相同报销项目的不同报销时间的记录，
        /// 请使用GetAllDistinctReimItems()
        /// </summary>
        /// <returns></returns>
        public List<ReimItem> GetAllReimItems() 
        {
            List<ReimItem> reimItems = new List<ReimItem>();
            try
            {   
                string commandString = "SELECT usta_Project.id AS projectId,"
                                            + "usta_Project.name AS projectName,"
                                            + "usta_Reim.id AS reimId,"
                                            + "usta_Reim.name AS reimName,"
                                            + "temp_ReimItem.value AS value "
                                        + "FROM ("
                                            + "SELECT usta_ReimItem.projectId AS projectId, "
                                                + "usta_ReimItem.reimId AS reimId, "
                                                + "SUM(usta_ReimItem.value) AS value "
                                            + "FROM usta_ReimItem "
                                            + "GROUP BY usta_ReimItem.projectId, usta_ReimItem.reimId) "
                                        + "AS temp_ReimItem, usta_Project, usta_Reim "
                                        + "WHERE temp_ReimItem.projectId = usta_Project.id "
                                        + "AND temp_ReimItem.reimId = usta_Reim.id";
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);

                while (reader.Read())
                {
                    ReimItem reimItem = new ReimItem();
                    Project project = new Project()
                    {
                        id = int.Parse(reader["projectId"].ToString().Trim()),
                        name = reader["projectName"].ToString().Trim()
                    };
                    reimItem.project = project;

                    Reim reim = new Reim()
                    {
                        id = int.Parse(reader["reimId"].ToString().Trim()),
                        name = reader["reimName"].ToString().Trim()
                    };

                    reimItem.reim = reim;

                    reimItem.value = CommonUtility.ConvertFormatedFloat("{0:F2}", reader["value"].ToString().Trim());
                    reimItems.Add(reimItem);

                }
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
            return reimItems;
        
        }
        /// <summary>
        /// 根据项目id和报销项id查询报销条目
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectId"></param>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public List<ReimItem> GetReimItems(string projectName, int projectId, int reimId) 
        {
            List<ReimItem> reimItems = new List<ReimItem>();
            try
            {
                string projectCase = null;
                string reimCase = null;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                if (projectId != 0)
                {
                    projectCase = " projectId = @projectId ";
                    parameterList.Add(new SqlParameter("@projectId", projectId));
                }
                else if (!string.IsNullOrWhiteSpace(projectName))
                {
                    projectCase = " usta_Project.name LIKE '%" + projectName + "%' ";
                }
                if (reimId != 0)
                {
                    reimCase = " reimId = @reimId";
                    parameterList.Add(new SqlParameter("@reimId", reimId));
                }

                string commandString = "SELECT usta_ReimItem.*, usta_Project.name as projectName, usta_Reim.name as ReimName FROM usta_ReimItem, usta_Project, usta_Reim WHERE usta_ReimItem.projectId = usta_Project.id AND usta_ReimItem.reimId = usta_Reim.id ";

                if (projectCase != null)
                {
                    commandString += (" AND " + projectCase);

                }
                if (reimCase != null)
                {
                    commandString += (" AND " + reimCase);
                }
                commandString += " ORDER BY createdTime DESC";
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());
                BuildReimItems(reader, reimItems);
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
            return reimItems;
        
        }

        /// <summary>
        /// 根据项目名称或者负责人id查询报销记录
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="userNo"></param>
        /// <returns></returns>
        public List<ReimItem> GetReimItemSummaryValues(string projectName, string userNo) 
        {
            List<ReimItem> reimItems = new List<ReimItem>();
            try
            {   

                List<SqlParameter> parameterList = new List<SqlParameter>();

                string commandString;


                if (!string.IsNullOrWhiteSpace(projectName))
                {
                    commandString = "SELECT usta_Project.id AS projectId,"
                                        + "usta_Project.name AS projectName,"
                                        + "usta_Reim.id AS reimId,"
                                        + "usta_Reim.name AS reimName,"
                                        + "temp_ReimItem.value AS value "
                                    + "FROM (SELECT usta_ReimItem.projectId AS projectId,"
                                                + " usta_ReimItem.reimId AS reimId, SUM(usta_ReimItem.value) AS value "
                                                + " FROM usta_ReimItem "
                                                + " GROUP BY usta_ReimItem.projectId, usta_ReimItem.reimId) "
                                    + " AS temp_ReimItem, usta_Project, usta_Reim"
                                    + " WHERE temp_ReimItem.projectId = usta_Project.id "
                                    + " AND temp_ReimItem.reimId = usta_Reim.id AND usta_Project.name LIKE '%" + projectName.Trim() + "%'" + (string.IsNullOrWhiteSpace(userNo) ? "" : " AND usta_Project.userNo = " + userNo);
                }
                else
                {

                    commandString = "SELECT usta_Project.id AS projectId,"
                                        + "usta_Project.name AS projectName,"
                                        + "usta_Reim.id AS reimId,"
                                        + "usta_Reim.name AS reimName,"
                                        + "temp_ReimItem.value AS value "
                                    + "FROM ( SELECT usta_ReimItem.projectId AS projectId,"
                                                + " usta_ReimItem.reimId AS reimId, SUM(usta_ReimItem.value) AS value "
                                                + " FROM usta_ReimItem "
                                                + " GROUP BY usta_ReimItem.projectId, usta_ReimItem.reimId) "
                                    + " AS temp_ReimItem, usta_Project, usta_Reim"
                                    + " WHERE temp_ReimItem.projectId = usta_Project.id "
                                    + " AND temp_ReimItem.reimId = usta_Reim.id " + (string.IsNullOrWhiteSpace(userNo) ? "" : " AND usta_Project.userNo = " + userNo);

                }

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());

                while (reader.Read())
                {
                    ReimItem reimItem = new ReimItem();
                    Project project = new Project()
                    {
                        id = int.Parse(reader["projectId"].ToString().Trim()),
                        name = reader["projectName"].ToString().Trim()
                    };
                    reimItem.project = project;

                    Reim reim = new Reim()
                    {
                        id = int.Parse(reader["reimId"].ToString().Trim()),
                        name = reader["reimName"].ToString().Trim()
                    };

                    reimItem.reim = reim;

                    reimItem.value = CommonUtility.ConvertFormatedFloat("{0:F2}", reader["value"].ToString().Trim());

                    reimItems.Add(reimItem);

                }
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
            return reimItems;
        
        }


        /// <summary>
        /// 根据项目名称或id和报销项查询报销记录
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectId"></param>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public List<ReimItem> GetReimItemSummaryValues(string projectName, int projectId, int reimId, string userNo) 
        {
            List<ReimItem> reimItems = new List<ReimItem>();

            try
            {   
                string projectCase = null;
                string reimCase = null;
                string userCase = null;

                List<SqlParameter> parameterList = new List<SqlParameter>();

                if (reimId != 0)
                {
                    reimCase = " reimId = @reimId ";
                    parameterList.Add(new SqlParameter("@reimId", reimId));
                }

                if (!string.IsNullOrWhiteSpace(userNo))
                {
                    userCase = " usta_Project.userNo = @userNo";
                    parameterList.Add(new SqlParameter("@userNo", userNo));
                }

                string commandString;
                if (projectId != 0)
                {

                    projectCase = " usta_ReimItem.projectId = @projectId ";
                    commandString = "SELECT usta_Project.id AS projectId,"
                                    + "usta_Project.name AS projectName,"
                                    + "usta_Reim.id AS reimId,"
                                    + "usta_Reim.name AS reimName,"
                                    + "temp_ReimItem.value AS value "
                                + " FROM ( SELECT usta_ReimItem.projectId AS projectId,"
                                            + " usta_ReimItem.reimId AS reimId, SUM(usta_ReimItem.value) AS value "
                                            + " FROM usta_ReimItem "
                                            + " WHERE " + projectCase
                                            + (reimCase == null ? "" : (" AND " + reimCase))
                                            + " GROUP BY usta_ReimItem.projectId, usta_ReimItem.reimId) "
                                + " AS temp_ReimItem, usta_Project, usta_Reim"
                                + " WHERE temp_ReimItem.projectId = usta_Project.id "
                                + " AND temp_ReimItem.reimId = usta_Reim.id";

                    parameterList.Add(new SqlParameter("@projectId", projectId));

                }
                else if (!string.IsNullOrWhiteSpace(projectName))
                {
                    commandString = "SELECT usta_Project.id AS projectId,"
                                        + "usta_Project.name AS projectName,"
                                        + "usta_Reim.id AS reimId,"
                                        + "usta_Reim.name AS reimName,"
                                        + "temp_ReimItem.value AS value "
                                    + "FROM (SELECT usta_ReimItem.projectId AS projectId,"
                                                + " usta_ReimItem.reimId AS reimId, SUM(usta_ReimItem.value) AS value "
                                                + " FROM usta_ReimItem "
                                                + (reimCase == null ? "" : (" WHERE " + reimCase))
                                                + " GROUP BY usta_ReimItem.projectId, usta_ReimItem.reimId) "
                                    + " AS temp_ReimItem, usta_Project, usta_Reim"
                                    + " WHERE temp_ReimItem.projectId = usta_Project.id "
                                    + " AND temp_ReimItem.reimId = usta_Reim.id AND usta_Project.name LIKE '%" + projectName.Trim() + "%'";
                }
                else
                {

                    commandString = "SELECT usta_Project.id AS projectId,"
                                        + "usta_Project.name AS projectName,"
                                        + "usta_Reim.id AS reimId,"
                                        + "usta_Reim.name AS reimName,"
                                        + "temp_ReimItem.value AS value "
                                    + "FROM ( SELECT usta_ReimItem.projectId AS projectId,"
                                                + " usta_ReimItem.reimId AS reimId, SUM(usta_ReimItem.value) AS value "
                                                + " FROM usta_ReimItem "
                                                + (reimCase == null ? "" : (" WHERE " + reimCase))
                                                + " GROUP BY usta_ReimItem.projectId, usta_ReimItem.reimId) "
                                    + " AS temp_ReimItem, usta_Project, usta_Reim"
                                    + " WHERE temp_ReimItem.projectId = usta_Project.id "
                                    + " AND temp_ReimItem.reimId = usta_Reim.id";

                }

                if (userCase != null)
                {
                    commandString += (" AND " + userCase);
                }

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());

                while (reader.Read())
                {
                    ReimItem reimItem = new ReimItem();
                    Project project = new Project()
                    {
                        id = int.Parse(reader["projectId"].ToString().Trim()),
                        name = reader["projectName"].ToString().Trim()
                    };
                    reimItem.project = project;

                    Reim reim = new Reim()
                    {
                        id = int.Parse(reader["reimId"].ToString().Trim()),
                        name = reader["reimName"].ToString().Trim()
                    };

                    reimItem.reim = reim;

                    reimItem.value = CommonUtility.ConvertFormatedFloat("{0:F2}", reader["value"].ToString().Trim());

                    reimItems.Add(reimItem);

                }
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
            return reimItems;
        
        }


        private void BuildReimItems(IDataReader reader, List<ReimItem> reimItems)
        {
            ReimItem reimItem;
            while (reader.Read())
            {
                reimItem = new ReimItem();
                reimItem.id = int.Parse(reader["id"].ToString().Trim());
                Project project = new Project()
                {
                    id = int.Parse(reader["projectId"].ToString().Trim()),
                    name = reader["projectName"].ToString().Trim()
                };

                reimItem.project = project;
                Reim reim = new Reim()
                {
                    id = int.Parse(reader["reimId"].ToString().Trim()),
                    name = reader["reimName"].ToString().Trim()
                };

                reimItem.reim = reim;
                reimItem.value = CommonUtility.ConvertFormatedFloat("{0:0.00}", reader["value"].ToString().Trim());
                reimItem.memo = reader["memo"].ToString().Trim();
                reimItem.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());

                reimItems.Add(reimItem);
            }
        }

        #endregion
    }
}
