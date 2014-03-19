using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace USTA.Dal
{
	using USTA.Model;
	using USTA.Common;
	using System.Configuration;
	using System.Data.SqlClient;
	using System.Data;

	/// <summary>
	/// 学生专业操作类
	/// </summary>
	public class DalOperationStudentSpecility
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
		public DalOperationStudentSpecility()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		
		/// <summary>
		/// 添加专业类型
		/// </summary>
		/// <param name="specility">专业信息</param>
		public void AddStudentSpecility(StudentSpecility specility)
		{
			try
			{
				string strSql = "insert into usta_StudentSpecility(";
				strSql += "specilityName) ";
				strSql += " values (";
				strSql += "@specilityName)";
				SqlParameter[] parameters = {
					new SqlParameter("@specilityName", SqlDbType.NVarChar,50)};
				parameters[0].Value = specility.specilityName;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql, parameters);
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
		/// 查找所有的专业类型
		/// </summary>
		/// <returns>专业类型数据集</returns>
		public DataSet FindAllStudentSpecilitye()
		{
			DataSet ds = null;
			try
			{
                string strSql = "select specilityId,specilityName,MajorTypeID from usta_StudentSpecility";
				ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
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
			return ds;
		}
		
		
		/// <summary>
		/// 根据专业主键查找
		/// </summary>
		/// <param name="specilityeId">专业主键</param>
		/// <returns>专业信息</returns>
		public StudentSpecility FindStudentSpecilityById(int specilityeId)
		{
			StudentSpecility model = null;
			string commandstring = "select specilityId,specilityName from usta_StudentSpecility where specilityId=@specilityId";
			SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@specilityId",specilityeId)
			};

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);
			if (dr.Read())
			{
				model = new StudentSpecility
				{
					specilityId = int.Parse(dr["specilityId"].ToString().Trim()),
					specilityName = dr["specilityName"].ToString().Trim()                 
				};
			}
			dr.Close();
			conn.Close();
			return model;
		}
        /// <summary>
        /// 根据专业主键查找
        /// </summary>
        /// <param name="MajorTypeID">专业主键</param>
        /// <returns>专业信息</returns>
        public string FindSpecilityNameByMajorTypeID(string MajorTypeID)
        {
            string specilityName = null;
            string commandstring = "select specilityId,specilityName,MajorTypeID from usta_StudentSpecility where MajorTypeID=@MajorTypeID";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@MajorTypeID",MajorTypeID)
			};

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandstring, parameters);
            if (dr.Read())
            {
                specilityName = dr["specilityName"].ToString().Trim();
            }
            dr.Close();
            conn.Close();
            return specilityName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MajorTypeID"></param>
        /// <returns></returns>
        public DataSet FindClassByMajorTypeID(string MajorTypeID)
        {
            DataSet dt = null;
            string commandstring = "select classId,className,special,MajorType,SchoolClassID,remark from usta_StudentClass where MajorType=@MajorType";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@MajorType",MajorTypeID)
			};

            dt = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandstring, parameters);
            conn.Close();
            return dt;
        }
	  
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void DeleteStudentSpecilityById(int specilityId)
		{
			try
			{
				string strSql = "delete from usta_StudentSpecility where specilityId=@specilityId ";
				SqlParameter[] parameters = {
					new SqlParameter("@specilityId", SqlDbType.Int,4)};
				parameters[0].Value = specilityId;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql, parameters);
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
		/// 修改学生专业
		/// </summary>
		/// <param name="specility">学生专业</param>
		public void UpdateStudentSpecility(StudentSpecility specility)
		{
			try
			{
				string strSql = "update usta_StudentSpecility set specilityName=@specilityName where specilityId=@specilityId";
				SqlParameter[] parameters = new SqlParameter[2]{
					new SqlParameter("@specilityId",specility.specilityId),
					new SqlParameter("@specilityName",specility.specilityName),
				};

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql, parameters);
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
		/// 查看所有班级
		/// </summary>
		/// <returns>班级数据集</returns>
		public DataSet FindAllClass()
		{
			DataSet ds = null;
			try
			{
                string strSql = "select [classId],[className],[special],[remark],[MajorType],[SchoolClassID],[Headteacher],[HeadteacherName] from [usta_StudentClass]";
				ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
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
			return ds;
		}

		/// <summary>
		/// 查看所有班级
		/// </summary>
		/// <returns>所有班级数据集</returns>
		public DataSet FindOneClass(int classId)
		{
			DataSet ds = null;
			try
			{
				string strSql = "SELECT [classId],[className] ,special  ,[remark]  FROM [USTA].[dbo].[usta_StudentClass]  where  classId=@classId";
				SqlParameter[] parameters = new SqlParameter[1]{
					new SqlParameter("@classId",classId)};
				ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql,parameters);
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
			return ds;
		}
		/// <summary>
		/// 添加学生班级信息
		/// </summary>
		/// <param name="cla">学生班级信息</param>
		public void AddClass(StudentClass cla)
		{
			DataSet ds = null;
			try
			{
				string strSql = "INSERT INTO [USTA].[dbo].[usta_StudentClass]([className]  ,[special],[remark]) VALUES (@className ,@special  ,@remark)";
				SqlParameter[] parameters = new SqlParameter[3]{
					new SqlParameter("@className",cla.className),
					new SqlParameter("@special",cla.special),
					 new SqlParameter("@remark",cla.remark)
				};
				ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql, parameters);
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
		/// 更新学生班级信息
		/// </summary>
		/// <param name="cla">学生班级信息</param>
		public void UpdateClass(StudentClass cla)
		{
			DataSet ds = null;
			try
			{
				string strSql = "UPDATE [USTA].[dbo].[usta_StudentClass]   SET [className] = @className ,[special] = @special ,[remark] = @remark  WHERE classId=@classId";
				SqlParameter[] parameters = new SqlParameter[4]{
					new  SqlParameter("@classId",cla.classId),
					new SqlParameter("@className",cla.className),
					new SqlParameter("@special",cla.special),
					 new SqlParameter("@remark",cla.remark)
				};
				ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql, parameters);
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
		/// 删除学生班级信息
		/// </summary>
		/// <param name="classId">班级编号</param>
		public void DeleteClass(int classId)
		{
			DataSet ds = null;
			try
			{
				string strSql = "DELETE FROM [USTA].[dbo].[usta_StudentClass]  WHERE classId=@classId";
				SqlParameter[] parameters = new SqlParameter[1]{
					new SqlParameter("@classId",classId)};
				ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql,parameters);
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
