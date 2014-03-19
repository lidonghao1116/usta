using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace USTA.Dal
{
    using USTA.Model;
    using USTA.Common;
    public class DalOperationAboutUserAuth
    {
        public SqlConnection conn
        {
            set;
            get;
        }

        public DalOperationAboutUserAuth() 
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }

        public UserAuth GetUserAuth(string page)
        {
            UserAuth userAuth = null;
            try 
            {
                string sql = "SELECT [id],[pageName],[userIds] FROM [USTA].[dbo].[usta_UserAuth] WHERE pageName = @page";
                SqlParameter[] parameters = {
					new SqlParameter("@page",page)
                   };
                SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
                if (dr.Read())
                {
                    userAuth = new UserAuth
                    {
                        id = Convert.ToInt32(dr["id"].ToString().Trim()),
                        pageName = dr["pageName"].ToString().Trim(),
                        userIds = dr["userIds"].ToString().Trim()
                    };
                }
                dr.Close();
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
            return userAuth;
        }
        public void setUserAuth(UserAuth userAuth)
        {
            try 
            {
                SqlParameter[] parameters = {
					new SqlParameter("@pageName",userAuth.pageName),
                    new SqlParameter("@userIds",userAuth.userIds)
                   };
                string sql = "";
                if (GetUserAuth(userAuth.pageName) == null)
                {
                    sql = "INSERT INTO [USTA].[dbo].[usta_UserAuth]([pageName],[userIds]) VALUES (@pageName,@userIds)";

                }
                else
                {
                    sql = "UPDATE [USTA].[dbo].[usta_UserAuth] SET [userIds] = @userIds WHERE  [pageName] = @pageName";

                }
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
    }
}
