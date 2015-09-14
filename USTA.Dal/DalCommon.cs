using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace USTA.Dal
{
    /// <summary>
    /// 公用DAL操作类
    /// </summary>
   public sealed class DalCommon
   {
       #region 全局变量及构造函数

       /// <summary>
       /// 构造函数
       /// </summary>
       public DalCommon()
       {

       }
       #endregion

       #region 获取当前学期标识
        /// <summary>
       /// 获取当前学期标识
        /// </summary>
        /// <param name="conn">数据库连接字符串</param>
        /// <returns>当前学期标识</returns>
       public static string GetTermTag(SqlConnection conn)
       {
           string result = string.Empty;
           string cmdstring = "SELECT max([termTag]) AS termTag  FROM [USTA].[dbo].[usta_TermTags]";
           SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring);
           while (dr.Read())
           {
               result = dr[0].ToString();
           }
           dr.Close();
           conn.Close();
           return result;
       }
       #endregion
   }
}
