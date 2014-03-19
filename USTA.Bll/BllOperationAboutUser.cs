using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace USTA.Bll
{
	using USTA.Model;
	using USTA.Common;

	/// <summary>
	/// 关于用户操作的业务类
	/// </summary>
	public sealed class BllOperationAboutUser
	{
		#region 全局变量及构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public BllOperationAboutUser()
		{

		}
		#endregion

		#region 根据用户类型返回数据库查询信息（用于登录验证）
        /// <summary>
        /// 根据用户类型返回数据库查询信息（用于登录验证）
        /// </summary>
        /// <param name="userType">用户类型</param>
        /// <param name="USID">用户唯一验证USID</param>
        /// <returns>返回登录实体类</returns>
		public static CheckUserLogin ReturnSqlJudgeByUserType(int userType, string USID)
		{
			CheckUserLogin CheckUserLogin = new CheckUserLogin();
			SqlParameter[] parameters;
			switch (userType)
			{
				case 1:
					parameters = new SqlParameter[1] {
					new SqlParameter("@teacherUSID", SqlDbType.NVarChar,50)};
                    parameters[0].Value = USID;
					CheckUserLogin.spName = "spTeachersList";
					CheckUserLogin.sqlParammeters = parameters;
					CheckUserLogin.userType = 1;
					break;
				case 3:
					parameters = new SqlParameter[1] {
					new SqlParameter("@studentUSID",SqlDbType.NVarChar,50)};
                    parameters[0].Value = USID;
					CheckUserLogin.spName = "spStudentsList";
					CheckUserLogin.sqlParammeters = parameters;
					CheckUserLogin.userType = 3;
					break;
				default:
					break;
			}
			return CheckUserLogin;
		}
		#endregion

		#region 根据用户类型返回数据库查询相关信息
		/// <summary>
		/// 根据用户类型返回数据库查询相关信息
		/// </summary>
		/// <param name="userType">用户类型</param>
		/// <param name="userName">用户名</param>
		/// <param name="userPwd">用户密码</param>
		/// <param name="newPwd">新密码</param>
		/// <returns>返回登录实体类</returns>
		public static CheckUserLogin ReturnSqlJudgeByUserTypeforEditPwd(int userType, string userName, string userPwd, string newPwd)
		{
			CheckUserLogin CheckUserLogin = new CheckUserLogin();
			SqlParameter[] parameters;
			switch (userType)
			{
				case 0:
					parameters = new SqlParameter[3] {
					new SqlParameter("@adminUserName", userName),
					new SqlParameter("@adminUserPwd", userPwd),
					new SqlParameter("@adminUserNewPwd", newPwd)};
					CheckUserLogin.spName = "spUpdateAdminListPwd";
					CheckUserLogin.sqlParammeters = parameters;
					CheckUserLogin.userType = 0;
					break;
				case 1:
					parameters = new SqlParameter[3] {
					new SqlParameter("@teacherNo", userName),
					new SqlParameter("@teacherUserPwd", userPwd),
					 new SqlParameter("@teacherUserNewPwd", newPwd)};
					CheckUserLogin.spName = "spUpdateTeachersListPwd";
					CheckUserLogin.sqlParammeters = parameters;
					CheckUserLogin.userType = 1;
					break;
				case 2:
					parameters = new SqlParameter[3] {
					new SqlParameter("@teacherNo", userName),
					new SqlParameter("@teacherUserPwd", userPwd),
					 new SqlParameter("@teacherUserNewPwd", newPwd)};
					CheckUserLogin.spName = "spUpdateTeachersListPwd";
					CheckUserLogin.sqlParammeters = parameters;
					CheckUserLogin.userType = 2;
					break;
				case 3:
					parameters = new SqlParameter[3] {
					new SqlParameter("@studentNo",userName),
					new SqlParameter("@studentUserPwd", userPwd),
					new SqlParameter("@studentUserNewPwd", newPwd)};
					CheckUserLogin.spName = "spUpdateStudentsListPwd";
					CheckUserLogin.sqlParammeters = parameters;
					CheckUserLogin.userType = 3;
					break;
				default:
					break;
			}
			return CheckUserLogin;
		}
		#endregion

		#region 获取当前用户的Cookies并返回相应的实体类
		/// <summary>
		/// 获取当前用户的Cookies并返回相应的实体类
		/// </summary>
		/// <returns>返回用户Cookies实体类</returns>
		public static UserCookiesInfo GetUserCookiesInfo()
		{
            object _session =  HttpContext.Current.Session[ConfigurationManager.AppSettings["sessionKey"].Trim()];
            string session = _session!=null?_session.ToString().Trim():null;

            if(string.IsNullOrEmpty(session))
            {
                CommonUtility.RedirectLoginUrl();
                return null;
            }

            UserCookiesInfo UserCookiesInfo = (UserCookiesInfo)SerializeCookies.DeSerializeCookiesMethod(session);

            return UserCookiesInfo;
		}
		#endregion
	}
}