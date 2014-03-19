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
    public class DalOperationAboutGameType
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
        /// 构造函数
        /// </summary>
        public DalOperationAboutGameType()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion


        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public int Exists(int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameTypeId from usta_GameType");
            strSql.Append(" where gameTypeId=@gameTypeId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters).Tables[0].Rows.Count;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(GameType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_GameType(");
            strSql.Append("gameTypeTitle,allowSexType,updateTime,groupCapability,gameCategoryId)");
            strSql.Append(" values (");
            strSql.Append("@gameTypeTitle,@allowSexType,@updateTime,@groupCapability,@gameCategoryId)");
            SqlParameter[] parameters = {
					new SqlParameter("@gameTypeTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@allowSexType", SqlDbType.NVarChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@groupCapability", SqlDbType.TinyInt,1),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4)};
            parameters[0].Value = model.gameTypeTitle;
            parameters[1].Value = model.allowSexType;
            parameters[2].Value = model.updateTime;
            parameters[3].Value = model.groupCapability;
            parameters[4].Value = model.gameCategoryId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(GameType model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_GameType set ");
            strSql.Append("gameTypeTitle=@gameTypeTitle,");
            strSql.Append("allowSexType=@allowSexType,");
            strSql.Append("groupCapability=@groupCapability,");
            strSql.Append("gameCategoryId=@gameCategoryId,");
            strSql.Append("updateTime=@updateTime");
            strSql.Append(" where gameTypeId=@gameTypeId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameTypeTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@allowSexType", SqlDbType.NVarChar,10),
					new SqlParameter("@groupCapability", SqlDbType.TinyInt,1),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4)};
            parameters[0].Value = model.gameTypeTitle;
            parameters[1].Value = model.allowSexType;
            parameters[2].Value = model.groupCapability;
            parameters[3].Value = model.updateTime;
            parameters[4].Value = model.gameTypeId;
            parameters[5].Value = model.gameCategoryId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_GameType ");
            strSql.Append(" where gameTypeId=@gameTypeId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = gameTypeId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据gameTypeId获取GameType数据
        /// </summary>
        public DataSet Get(int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gameTypeId,gameTypeTitle,allowSexType,updateTime,groupCapability,gameCategoryId from usta_GameType ");
            strSql.Append(" where gameTypeId=@gameTypeId;");
            SqlParameter[] parameters = {
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据gameCategoryId获取GameType数据
        /// </summary>
        public DataSet GetGameTypeByGameCategoryId(int gameCategoryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameTitle,gameTypeId,gameTypeTitle,allowSexType,A.updateTime,groupCapability,A.gameCategoryId from usta_GameType A,usta_GameCategory B WHERE A.gameCategoryId=B.gameCategoryId ");
            strSql.Append(" AND A.gameCategoryId=@gameCategoryId;");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameTitle,gameTypeId,gameTypeTitle,allowSexType,A.updateTime,groupCapability,A.gameCategoryId ");
            strSql.Append(" FROM usta_GameType A, usta_GameCategory B WHERE A.gameCategoryId=B.gameCategoryId ORDER BY gameTypeId DESC;");
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }



        /// <summary>
        /// 根据gameCategoryId和教师性别获取数据列表
        /// </summary>
        public DataSet GetListByGameCategoryIdAndSex(int gameCategoryId, string allowSexType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameTypeId,gameTypeTitle,allowSexType,updateTime,groupCapability,gameCategoryId ");
            strSql.Append(" FROM usta_GameType WHERE CHARINDEX(@allowSexType,allowSexType)>0 AND gameCategoryId=@gameCategoryId ORDER BY gameTypeId DESC");
            SqlParameter[] parameters = {
					new SqlParameter("@allowSexType", SqlDbType.NVarChar,10),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4)
};
            parameters[0].Value = allowSexType;
            parameters[1].Value = gameCategoryId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }



        /// <summary>
        /// 根据gameCategoryId和gameTypeId获取组容量
        /// </summary>
        public DataSet GetGroupCapabilityByGameTypeId(int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 groupCapability ");
            strSql.Append(" FROM usta_GameType  WHERE");
            strSql.Append(" gameTypeId=@gameTypeId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;
            parameters[1].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion
    }
}