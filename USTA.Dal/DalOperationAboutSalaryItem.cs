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
    /// 薪酬项操作类
    /// </summary>
    public class DalOperationAboutSalaryItem
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
        public DalOperationAboutSalaryItem()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }

        /// <summary>
        /// 添加一条薪酬项
        /// </summary>
        /// <param name="salaryItemName"></param>
        /// <param name="salaryItemDesc"></param>
        public void AddSalaryItem(SalaryItem item)
        {
            try
            {
                string commandString = "INSERT INTO [USTA].[dbo].[usta_salaryItem]([salaryItemName], [salaryItemUnit], [salaryItemDesc], [useFor], [salaryItemStatus], [hasTax], [isDefaultChecked], [createdTime]) VALUES (@salaryItemName, @salaryItemUnit, @salaryItemDesc, @useFor, @salaryItemStatus,@hasTax, @isDefaultChecked, @createdTime)";
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@salaryItemName", item.salaryItemName), 
                new SqlParameter("@salaryItemUnit", item.salaryItemUnit),
                new SqlParameter("@salaryItemDesc", item.salaryItemDesc),
                new SqlParameter("@useFor", item.useFor),
                new SqlParameter("@salaryItemStatus", item.salaryItemStatus),
                new SqlParameter("@hasTax", item.hasTax),
                new SqlParameter("@isDefaultChecked", item.isDefaultChecked),
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
            conn.Close();
        }

        /// <summary>
        /// 更新一条薪酬项
        /// </summary>
        /// <param name="item"></param>
        public void updateSalaryItem(SalaryItem item)
        {
            try
            {
                string commandString = "UPDATE usta_salaryItem SET salaryItemName = @salaryItemName, salaryItemUnit = @salaryItemUnit, salaryItemDesc = @salaryItemDesc, useFor = @useFor,  hasTax = @hasTax, isDefaultChecked = @isDefaultChecked WHERE salaryItemId = @salaryItemId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@salaryItemName", item.salaryItemName),
                new SqlParameter("@salaryItemUnit", item.salaryItemUnit),
                new SqlParameter("@salaryItemDesc", item.salaryItemDesc),
                new SqlParameter("@hasTax", item.hasTax),
                new SqlParameter("@useFor", item.useFor),
                new SqlParameter("@isDefaultChecked", item.isDefaultChecked),
                new SqlParameter("@salaryItemId", item.salaryItemId)
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

        public void updateSalaryItemStatus(int salaryItemId, int newStatus)
        {
            try
            {
                string commandString = "UPDATE usta_salaryItem SET salaryItemStatus = @newStatus WHERE salaryItemId = @salaryItemId";
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@salaryItemId", salaryItemId),
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
        /// 根据薪酬项id查询薪酬项信息
        /// </summary>
        /// <param name="salaryItemId"></param>
        /// <returns></returns>
        public SalaryItem GetSalaryItemById(int salaryItemId)
        {
            SalaryItem salaryItem = null;
            try
            {

                string commandString = "SELECT * FROM usta_salaryItem WHERE salaryItemId = @salaryItemId";
                SqlParameter[] parameters = new SqlParameter[1]{
              new SqlParameter("@salaryItemId", salaryItemId)  
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);

                List<SalaryItem> items = new List<SalaryItem>();
                buildSalaryItems(reader, items);

                reader.Close();

                if (items != null && items.Count == 1)
                {
                    salaryItem = items[0];
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

            return salaryItem;
        }
        /*
        /// <summary>
        /// 删除一条薪酬项
        /// </summary>
        /// <param name="salaryItemId"></param>
        public void delSalaryItem(int salaryItemId) 
        {
            string commandString = "DELETE FROM usta_salaryItem WHERE salaryItemId = @salaryItemId";

            SqlParameter[] parameters = new SqlParameter[1]
            {
                new SqlParameter("@salaryItemId", salaryItemId)
            };

            SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);

            conn.Close();
        }
        */
        /// <summary>
        /// 获得所有已添加的条目
        /// </summary>
        /// <returns></returns>
        public List<SalaryItem> GetAllSalaryItem()
        {
            List<SalaryItem> items = new List<SalaryItem>();
            try
            {
                string commandString = "SELECT * FROM usta_salaryItem ORDER BY salaryItemStatus ASC,  createdTime DESC";
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);

                buildSalaryItems(reader, items);
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
            return items;
        }

        public List<SalaryItem> GetAllSalaryItem(int useFor, int salaryItemStatus)
        {
            List<SalaryItem> items = new List<SalaryItem>();
            try
            {
                string commandString = "SELECT * FROM usta_salaryItem WHERE useFor = @useFor " + (salaryItemStatus == 0 ? "" :  " AND salaryItemStatus = @salaryItemStatus ")  + "  ORDER BY createdTime DESC";

                SqlParameter[] parameters = new SqlParameter[]{
            new SqlParameter("@useFor", useFor),
            new SqlParameter("@salaryItemStatus", salaryItemStatus)
          };
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);

                buildSalaryItems(reader, items);
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
            return items;
        }

        /// <summary>
        /// 根据分页，查询一页可显示的薪酬项
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SalaryItem> GetPageSalaryItem(int pageNo, int pageSize)
        {
            List<SalaryItem> items = new List<SalaryItem>();
            try
            {
                string commandStrng = "SELECT TOP " + pageSize + " * FROM usta_salaryItem WHERE salaryItemId NOT IN (SELECT TOP " + (pageNo - 1) * pageSize + " salaryItemId FROM usta_salaryItem ORDER BY salaryItemId ASC)";

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandStrng);

                buildSalaryItems(reader, items);
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

            return items;
        }

        /// <summary>
        /// 根据reader构建一个SalaryItem实例
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private void buildSalaryItems(IDataReader reader, List<SalaryItem> items)
        {
            while (reader.Read())
            {
                SalaryItem item = new SalaryItem();
                item.salaryItemId = int.Parse(reader["salaryItemId"].ToString().Trim());
                item.salaryItemName = reader["salaryItemName"].ToString().Trim();
                if (reader["salaryItemUnit"] != null)
                {
                    item.salaryItemUnit = reader["salaryItemUnit"].ToString().Trim();
                }
                if (reader["salaryItemDesc"] != null)
                {
                    item.salaryItemDesc = reader["salaryItemDesc"].ToString().Trim();
                }
                item.useFor = int.Parse(reader["useFor"].ToString().Trim());
                item.salaryItemStatus = int.Parse(reader["salaryItemStatus"].ToString().Trim());
                item.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());
                item.hasTax = bool.Parse(reader["hasTax"].ToString().Trim());
                item.isDefaultChecked = bool.Parse(reader["isDefaultChecked"].ToString().Trim());
                items.Add(item);
            }
        }
    }
}