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
    /// 
    /// </summary>
    public class DalOperationAboutArchivesItems
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
        public DalOperationAboutArchivesItems()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion


        #region 增加一条结课资料规则
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddArchivesItems(ArchivesItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_ArchivesItems(");
            strSql.Append("archiveItemName,remark,teacherType,termTag)");
            strSql.Append(" values (");
            strSql.Append("@archiveItemName,@remark,@teacherType,@termTag);");
            SqlParameter[] parameters = {
					new SqlParameter("@archiveItemName", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,200),
					new SqlParameter("@teacherType", SqlDbType.NChar,10),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.archiveItemName;
            parameters[1].Value = model.remark;
            parameters[2].Value = model.teacherType;
            parameters[3].Value = model.termTag;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }
        #endregion




        #region 根据ID删除一条结课资料规则
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveItemId"></param>
        /// <returns></returns>
        public int DeleteArchivesItemById(int archiveItemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_ArchivesItems ");
            strSql.Append(" where archiveItemId=@archiveItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@archiveItemId", SqlDbType.Int,4)
};
            parameters[0].Value = archiveItemId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }
        #endregion




        #region 根据ID更新结课资料规则
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateArchivesItemById(ArchivesItems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_ArchivesItems set ");
            strSql.Append("archiveItemName=@archiveItemName,");
            strSql.Append("remark=@remark,");
            strSql.Append("termTag=@termTag,");
            strSql.Append("teacherType=@teacherType");
            strSql.Append(" where archiveItemId=@archiveItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@archiveItemName", SqlDbType.NChar,50),
					new SqlParameter("@remark", SqlDbType.NVarChar,200),
					new SqlParameter("@teacherType", SqlDbType.NChar,10),
					new SqlParameter("@archiveItemId", SqlDbType.Int,4),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.archiveItemName;
            parameters[1].Value = model.remark;
            parameters[2].Value = model.teacherType;
            parameters[3].Value = model.archiveItemId;
            parameters[4].Value = model.termTag;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }
        #endregion



        #region 根据ID获取结课资料规则数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveItemId"></param>
        /// <returns></returns>
        public DataSet GetArchivesItemById(int archiveItemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 termTag,archiveItemId,archiveItemName,remark,teacherType from usta_ArchivesItems ");
            strSql.Append(" where archiveItemId=@archiveItemId");
            SqlParameter[] parameters = {
					new SqlParameter("@archiveItemId", SqlDbType.Int,4)
};
            parameters[0].Value = archiveItemId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }
        #endregion



        #region 获取全部结课资料规则数据
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllArchivesItem()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select termTag,archiveItemId,archiveItemName,remark,teacherType from usta_ArchivesItems;");

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());

        }




        #region 根据学期标识和角色类型获取结课资料规则数据
        /// <summary>
        /// 根据学期标识和角色类型获取结课资料规则数据
        /// </summary>
        /// <param name="teacherType"></param>
        /// <param name="termTag"></param>
        /// <returns></returns>
        public DataSet GetArchivesItemByTeacherType(string teacherType,string termTag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select termTag,archiveItemId,archiveItemName,remark,teacherType from usta_ArchivesItems WHERE teacherType=@teacherType AND termTag=@termTag;");
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@teacherType",teacherType),
                new SqlParameter("@termTag",termTag)
            };

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }
        #endregion


        #endregion
    }
}