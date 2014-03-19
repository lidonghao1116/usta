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
    public class DalOperationAboutGameCategory
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
        public DalOperationAboutGameCategory()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion


        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public int Exists(int gameCategoryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameCategoryId from usta_GameCategory");
            strSql.Append(" where gameCategoryId=@gameCategoryId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters).Tables[0].Rows.Count;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(GameCategory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_GameCategory(");
            strSql.Append("gameTitle,gameContent,attachmentIds,updateTime,startTime,endTime)");
            strSql.Append(" values (");
            strSql.Append("@gameTitle,@gameContent,@attachmentIds,@updateTime,@startTime,@endTime)");
            SqlParameter[] parameters = {
					new SqlParameter("@gameTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@gameContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@updateTime", SqlDbType.NChar,10),
					new SqlParameter("@startTime", SqlDbType.DateTime),
					new SqlParameter("@endTime", SqlDbType.DateTime)};
            parameters[0].Value = model.gameTitle;
            parameters[1].Value = model.gameContent;
            parameters[2].Value = model.attachmentIds;
            parameters[3].Value = model.updateTime;
            parameters[4].Value = model.startTime;
            parameters[5].Value = model.endTime;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(GameCategory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_GameCategory set ");
            strSql.Append("gameTitle=@gameTitle,");
            strSql.Append("gameContent=@gameContent,");
            strSql.Append("attachmentIds=@attachmentIds,");
            strSql.Append("updateTime=@updateTime,");
            strSql.Append("startTime=@startTime,");
            strSql.Append("endTime=@endTime,");
            strSql.Append("isOpenDraw=@isOpenDraw ");
            strSql.Append(" where gameCategoryId=@gameCategoryId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@gameContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@startTime", SqlDbType.DateTime),
					new SqlParameter("@endTime", SqlDbType.DateTime),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@isOpenDraw", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.gameTitle;
            parameters[1].Value = model.gameContent;
            parameters[2].Value = model.attachmentIds;
            parameters[3].Value = model.updateTime;
            parameters[4].Value = model.startTime;
            parameters[5].Value = model.endTime;
            parameters[6].Value = model.gameCategoryId;
            parameters[7].Value = model.isOpenDraw;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int gameCategoryId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_GameCategory ");
            strSql.Append(" where gameCategoryId=@gameCategoryId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }



        /// <summary>
        /// 根据gameCategoryId判断是否已经开放抽签
        /// </summary>
        public DataSet CheckIsOpenDrawByGameCategoryId(int gameCategoryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 isOpenDraw from usta_GameCategory ");
            strSql.Append(" where gameCategoryId=@gameCategoryId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataSet Get(int gameCategoryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gameCategoryId,gameTitle,gameContent,attachmentIds,updateTime,startTime,endTime,isOpenDraw from usta_GameCategory ");
            strSql.Append(" where gameCategoryId=@gameCategoryId");
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
            strSql.Append("select gameCategoryId,gameTitle,gameContent,attachmentIds,updateTime,startTime,endTime,isOpenDraw ");
            strSql.Append(" FROM usta_GameCategory ORDER BY gameCategoryId DESC;");
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 获取当前正在进行的活动报名信息
        /// </summary>
        public DataSet GetGameCategoryIng(DateTime now)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameCategoryId,gameTitle,gameContent,attachmentIds,updateTime,startTime,endTime,isOpenDraw ");
            strSql.Append(" FROM usta_GameCategory  WHERE startTime<@now AND endTime>@now ORDER BY gameCategoryId DESC;");
            SqlParameter[] parameters = {
					new SqlParameter("@now", SqlDbType.DateTime)
};
            parameters[0].Value = now;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据gameCategoryId判断当前是否已经过截止日期
        /// </summary>
        public DataSet CheckGameCategoryIsOverTimeByGameCategoryId(int gameCategoryId, DateTime now)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 gameCategoryId,gameTitle,gameContent,attachmentIds,updateTime,startTime,endTime,isOpenDraw ");
            strSql.Append(" FROM usta_GameCategory  WHERE startTime<@now AND endTime>@now ");
            strSql.Append(" AND gameCategoryId=@gameCategoryId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@now", SqlDbType.DateTime)
};
            parameters[0].Value = gameCategoryId;
            parameters[1].Value = now;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新抽签状态
        /// </summary>
        public int UpdateDrawState(GameCategory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_GameCategory set ");
            strSql.Append("isOpenDraw=@isOpenDraw");
            strSql.Append(" where gameCategoryId=@gameCategoryId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@isOpenDraw", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.gameCategoryId;
            parameters[1].Value = model.isOpenDraw;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion
    }
}