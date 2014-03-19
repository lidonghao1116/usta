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
	/// <summary>
    /// 结课资料上传函数
	/// </summary>
	public class DalOperationAboutArchives
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

		/// <summary>
		/// 构造函数
		/// </summary>
		public DalOperationAboutArchives()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		/// <summary>
		/// 插入一条课程归档记录
		/// </summary>
		/// <param name="archives">课程归档实体</param>
        public void AddArchives(Archives archives)
        {
            try
            {
                string cmdstring = "INSERT INTO [USTA].[dbo].[usta_Archives] ([attachmentIds],[courseNo],classID,termTag,teacherType,archiveItemId)  VALUES ( @attachmentIds,@courseNo,@classId,@termtag,@teacherType,@archiveItemId)";

                SqlParameter[] parameters = {
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
                    new SqlParameter("@classId", SqlDbType.NVarChar,50),
                    new SqlParameter("@termtag", SqlDbType.NVarChar,50),
                    new SqlParameter("@teacherType", SqlDbType.NChar,10) ,
                    new SqlParameter("@archiveItemId", SqlDbType.Int,4)               
                };

                parameters[0].Value = archives.attachmentIds;
                parameters[1].Value = archives.courseNo;
                parameters[2].Value = archives.classID;
                parameters[3].Value = archives.termTag;
                parameters[4].Value = archives.teacherType;
                parameters[5].Value = archives.archiveItemId;

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 获得课程归档
		/// </summary>
		/// <param name="archiveId">课程归档主键</param>
		/// <returns>课程归档对象</returns>
		public Archives FindArchivesById(int archiveId)
		{
			Archives archives = null;
			string cmdstring = "SELECT [archiveId],[attachmentIds],[usta_Archives].[courseNo],[courseName] FROM [USTA].[dbo].[usta_Archives],usta_Courses WHERE archiveId=@archiveId AND usta_Courses.courseNo=usta_Archives.courseNo";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@archiveId",archiveId)
			};

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
			if (dr.Read())
			{

				archives = new Archives
				{
					archiveId = int.Parse(dr["archiveId"].ToString()),
					attachmentIds = dr["attachmentIds"].ToString().Trim(),
					courseNo = dr["courseNo"].ToString().Trim(),                  
				};
			}
			dr.Close();
			conn.Close();
			return archives;
		}
		/// <summary>
		/// 获取课程归档
		/// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="classId">classId</param>
        /// <param name="termtag">termtag</param>
        /// <param name="teacherType">teacherType</param>
        /// <param name="archiveItemId">archiveItemId</param>
		/// <returns>课程归档实体</returns>
        public DataSet FindArchivesByCourseNo(string courseNo, string classId, string termtag, string teacherType, int archiveItemId)
		{
            string cmdstring = "SELECT [archiveId],[archiveItemId],[attachmentIds],[courseNo],[teacherType] FROM [USTA].[dbo].[usta_Archives] WHERE courseNo=@courseNo AND classID=@classID AND termTag=@termTag AND archiveItemId=@archiveItemId AND teacherType=@teacherType";
			SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@courseNo",courseNo),
				new SqlParameter("@classID",classId),
				new SqlParameter("@termTag",termtag),
				new SqlParameter("@teacherType",teacherType),
				new SqlParameter("@archiveItemId",archiveItemId)
			};

            //SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
            //if (dr.Read())
            //{

            //    archives = new Archives
            //    {
            //        archiveId = int.Parse(dr["archiveId"].ToString()),
            //        attachmentIds = dr["attachmentIds"].ToString().Trim(),
            //        courseNo = dr["courseNo"].ToString().Trim(),
            //        teacherType = dr["teacherType"].ToString().Trim(),
            //    };
            //}
            //dr.Close();
            //conn.Close();
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
		}


        /// <summary>
        /// 获取课程归档（）
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="classId">classId</param>
        /// <param name="termtag">termtag</param>
        /// <param name="termtag">teacherType</param>
        /// <returns>课程归档实体</returns>
        public Archives FindArchivesByCourseNoCompatible(string courseNo, string classId, string termtag)
        {
            Archives archives = new Archives { attachmentIds = string.Empty };
            string cmdstring = "SELECT [archiveId],[archiveItemId],[attachmentIds],[courseNo],[teacherType],[classID],[termTag] FROM [USTA].[dbo].[usta_Archives] WHERE courseNo=@courseNo AND classID=@classID AND termTag=@termTag";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@courseNo",courseNo),
				new SqlParameter("@classID",classId),
				new SqlParameter("@termTag",termtag)
			};

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
            if (dr.Read())
            {

                archives = new Archives
                {
                    archiveId = int.Parse(dr["archiveId"].ToString()),
                    attachmentIds = dr["attachmentIds"].ToString().Trim(),
                    courseNo = dr["courseNo"].ToString().Trim(),
                    classID = dr["classID"].ToString().Trim(),
                    termTag = dr["termTag"].ToString().Trim()
                };
            }
            dr.Close();
            conn.Close();
            return archives;
        }

        /// <summary>
        /// 获得课程归档编号
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="classId"></param>
        /// <param name="termtag"></param>
        /// <param name="teacherType"></param>
        /// <param name="archiveItemId">课程归档编号</param>
        /// <returns></returns>
		public int IsExistArchivesBycourseNo(string courseNo,string classId,string termtag,string teacherType, int archiveItemId)
		{
            int archiveId = 0;
            string cmdstring = "SELECT [archiveId],[archiveItemId],[attachmentIds],[courseNo] FROM [USTA].[dbo].[usta_Archives] WHERE courseNo=@courseNo AND classID=@classId AND termTag=@termtag AND teacherType=@teacherType AND archiveItemId=@archiveItemId";
			SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@courseNo",courseNo),
                new SqlParameter("@classId",classId),
                new SqlParameter("@termtag",termtag),
                new SqlParameter("@teacherType",teacherType),
                new SqlParameter("@archiveItemId",archiveItemId)
			};

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
		   
			if (dr.Read())
			{
                archiveId = int.Parse(dr["archiveId"].ToString().Trim());
			}
			dr.Close();
            return archiveId;
		}


        /// <summary>
        /// 查找是否存在结课资料（向兼容）
        /// </summary>
        /// <param name="courseNo">课程编号</param>
        /// <param name="classId"></param>
        /// <param name="termtag"></param>
        /// <returns></returns>
        public int IsExistArchivesBycourseNoCompatible(string courseNo, string classId, string termtag)
        {
            int archiveId = 0;
            string cmdstring = "SELECT [archiveId],[archiveItemId],[attachmentIds],[courseNo] FROM [USTA].[dbo].[usta_Archives] WHERE courseNo=@courseNo AND classID=@classId AND termTag=@termtag AND teacherType IS NULL AND archiveItemId IS NULL";
            SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@courseNo",courseNo),
                new SqlParameter("@classId",classId),
                new SqlParameter("@termtag",termtag)
			};

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);

            if (dr.Read())
            {
                archiveId = int.Parse(dr["archiveId"].ToString().Trim());
            }
            dr.Close();
            return archiveId;
        }
		/// <summary>
		/// 获取课程归档
		/// </summary>
		/// <param name="termTag">学期标识</param>
		/// <returns>课程归档数据集</returns>
		public DataSet FindArchivesBytermTag(string termTag)
		{
			string cmdstring = "SELECT [archiveId],[attachmentIds],[usta_Archives].[courseNo],[courseName] FROM [USTA].[dbo].[usta_Archives],usta_Courses WHERE termTag=@termTag";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@termTag",termTag)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 更新课程归档
		/// </summary>
		/// <param name="archives">课程归档实体</param>
		public void UpdateArchives(Archives archives)
		{
			try
			{
			   string cmdstring = "UPDATE [USTA].[dbo].[usta_Archives] SET [attachmentIds] = @attachmentIds WHERE archiveId=@archiveId";


			   SqlParameter[] parameters = {
					new SqlParameter("@archiveId", SqlDbType.Int,4),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)
                                           };
			   parameters[0].Value = archives.archiveId;
			   parameters[1].Value = archives.attachmentIds;
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
