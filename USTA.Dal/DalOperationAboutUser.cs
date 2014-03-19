using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.OleDb;
using System.Web;

namespace USTA.Dal
{
    using USTA.Bll;
    using USTA.Common;
    using USTA.Model;
    using System.Transactions;
    using System.Web.UI;

    /// <summary>
    /// 用户相关操作类
    /// </summary>
    public class DalOperationAboutUser
    {
        #region 全局变量及构造函数
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
        public DalOperationAboutUser()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region 检测用户登录合法性

        /// <summary>
        /// 检测用户登录合法性
        /// </summary>
        /// <param name="CheckUserLogin">用户登陆实体</param>
        /// <returns>用户登陆信息实体</returns>
        public UserCookiesInfo CheckUserLogin(CheckUserLogin CheckUserLogin)
        {
            UserCookiesInfo UserCookiesInfo = null;

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CheckUserLogin.spName, CheckUserLogin.sqlParammeters);

            while (dr.Read())
            {
                UserCookiesInfo = new UserCookiesInfo { userNo = dr[0].ToString(), userName = dr[2].ToString() };

                UserCookiesInfo.userType = ((dr["isAdmin"] != null && bool.Parse(dr["isAdmin"].ToString().Trim())) ? 0 : CheckUserLogin.userType);

                UserCookiesInfo.teacherType = (dr.FieldCount !=6 ? "student" : dr["TeacherType"].ToString().Trim());

                UserCookiesInfo.Sex = (dr.FieldCount != 6 ? "student" : dr["Sex"].ToString().Trim()); ;
            }

            dr.Close();
            conn.Close();

            return UserCookiesInfo;
        }
        #endregion

        #region 修改用户密码
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="CheckUserLogin">用户登陆实体</param>
        /// <returns>修改是否成功</returns>
        public bool UpdateUserPwd(CheckUserLogin CheckUserLogin)
        {
            SqlHelper.ExecuteNonQuery(conn, CheckUserLogin.spName, CheckUserLogin.sqlParammeters);
            conn.Close();
            return true;
        }
        #endregion

        #region 导入Excel数据
        /// <summary>
        /// 导入Excel数据
        /// </summary>
        /// <param name="excelData">Excel数据实体</param>
        /// <returns></returns>
        public int ImportExcelData(ExcelData excelData)
        {

            //出错的工作薄名称
            string exceptionSheetName = string.Empty;

            //出错的行号
            int exceptionRowNo = 0;

            ////出错的列号
            //int exceptionColNo = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //int count = 0;
                    //try
                    //{
                    foreach (ExcelSheetData excelSheetData in excelData.excelSheetData)
                    {
                        //count += 1;
                        //HttpContext.Current.Response.Write(count + "<br/>");
                        exceptionSheetName = excelSheetData.sheetName;
                        exceptionRowNo = excelSheetData.sheetRowNo;
                        //exceptionColNo = excelSheetData.sheetColNo;

                        SqlHelper.ExecuteNonQuery(conn, excelSheetData.spName, excelSheetData.sqlParammeters);

                        //执行插入操作
                        //SqlHelper.ExecuteNonQuery(conn, excelSheetData.spName, excelSheetData.sqlParammeters);
                    }
                    //}
                    //catch
                    //{
                    //    HttpContext.Current.Response.Write(count);
                    //    //HttpContext.Current.Response.End();
                    //}

                    //HttpContext.Current.Response.End();
                    //获取当前学期标识
                    string termTag = DalCommon.GetTermTag(conn);

                    //HttpContext.Current.Response.Write(termTag.ToString());
                    //HttpContext.Current.Response.End();

                    //以下需要添加插入密码映射表操作

                    //foreach (ExcelPasswordMapping excelPasswordMapping in excelData.excelPasswordMapping)
                    //{
                    //    AddPasswordMapping(new PasswordMapping
                    //    {
                    //        userName = excelPasswordMapping.listPasswordMappingUserName[0],
                    //        userNo = excelPasswordMapping.listPasswordMappingUserNo[0],
                    //        initializePassword = excelPasswordMapping.listPasswordMappingInitializePassword[0],
                    //        //此处暂时添加为0
                    //        termTag = termTag
                    //    });
                    //}
                    //count = 1;
                    foreach (PasswordMapping passwordMapping in excelData.excelPasswordMapping)
                    {
                        AddPasswordMapping(new PasswordMapping
                        {
                            userName = passwordMapping.userName,
                            userNo = passwordMapping.userNo,
                            initializePassword = passwordMapping.initializePassword,
                            //此处暂时添加为0
                            termTag = termTag,
                            userType = passwordMapping.userType
                        });
                    }
                    scope.Complete();

                    HttpContext.Current.Response.Write("<script type='text/javascript'>alert('导入Excel数据成功！确定后将转至系统首页');location.href='/';</script>");
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    HttpContext.Current.Response.Write("<script type='text/javascript'>alert('导入Excel数据失败！此次操作未更改任何数据库数据，相关信息如下：\\n\\n出错的工作薄名称为："
                    + exceptionSheetName
                    + "\\n出错的单元格行号为：" + exceptionRowNo
                        //+ "\n出错的单元格列号为：" + exceptionColNo
                    + "\\n可能的原因为：\\n1. 此单元格数据格式可能不正确，例如：单元格数据是否存在多余的空格。\\n2. 此单元格数据存在重复而发生冲突。即可能数据库中已经存在此条记录，或者在Excel文件中存在两条重复的记录。');history.go(-1);</script>");
                }
            }
            return 1;
        }
        #endregion

        #region 插入密码映射表
        /// <summary>
        /// 插入密码映射表
        /// </summary>
        /// <param name="passwordMapping">密码映射实体</param>
        /// <returns>密码修改成功标号</returns>
        public int AddPasswordMapping(PasswordMapping passwordMapping)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@userNo", SqlDbType.NChar,10),
					new SqlParameter("@userName", SqlDbType.NChar,10),
					new SqlParameter("@initializePassword", SqlDbType.NChar,4),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50),
					new SqlParameter("@userType", SqlDbType.SmallInt,2)};
            parameters[0].Value = passwordMapping.userNo;
            parameters[1].Value = passwordMapping.userName;
            parameters[2].Value = passwordMapping.initializePassword;
            parameters[3].Value = passwordMapping.termTag;
            parameters[4].Value = passwordMapping.userType;

            SqlHelper.ExecuteNonQuery(conn, "spPasswordMappingAdd", parameters);
            conn.Close();
            return 1;
        }
        #endregion
    }
}
