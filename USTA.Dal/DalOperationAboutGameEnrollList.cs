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
    public class DalOperationAboutGameEnrollList
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
        public DalOperationAboutGameEnrollList()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion


        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public int Exists(string teacherNo, int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameEnrollListId from usta_GameEnrollList");
            strSql.Append(" where teacherNo=@teacherNo AND gameCategoryId=@gameCategoryId AND gameTypeId=@gameTypeId");
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = teacherNo;
            parameters[1].Value = gameCategoryId;
            parameters[2].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters).Tables[0].Rows.Count;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(GameEnrollList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_GameEnrollList(");
            strSql.Append("teacherNo,gameCategoryId,gameTypeId,updateTime)");
            strSql.Append(" values (");
            strSql.Append("@teacherNo,@gameCategoryId,@gameTypeId,@updateTime)");
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4),
					new SqlParameter("@updateTime", SqlDbType.Date)};
            parameters[0].Value = model.teacherNo;
            parameters[1].Value = model.gameCategoryId;
            parameters[2].Value = model.gameTypeId;
            parameters[3].Value = model.updateTime;

            return SqlHelper.ExecuteNonQuery(conn,CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(GameEnrollList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_GameEnrollList set ");
            strSql.Append("teacherNo=@teacherNo,");
            strSql.Append("gameCategoryId=@gameCategoryId,");
            strSql.Append("gameTypeId=@gameTypeId");
            strSql.Append(" where gameEnrollListId=@gameEnrollListId");
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4),
					new SqlParameter("@gameEnrollListId", SqlDbType.Int,4)};
            parameters[0].Value = model.teacherNo;
            parameters[1].Value = model.gameCategoryId;
            parameters[2].Value = model.gameTypeId;
            parameters[3].Value = model.gameEnrollListId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int gameEnrollListId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_GameEnrollList ");
            strSql.Append(" where gameEnrollListId=@gameEnrollListId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameEnrollListId", SqlDbType.Int,4)
};
            parameters[0].Value = gameEnrollListId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataSet Get(int gameEnrollListId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gameEnrollListId,teacherNo,gameCategoryId,gameTypeId from usta_GameEnrollList ");
            strSql.Append(" where gameEnrollListId=@gameEnrollListId ORDER BY gameEnrollListId DESC;");
            SqlParameter[] parameters = {
					new SqlParameter("@gameEnrollListId", SqlDbType.Int,4)
};
            parameters[0].Value = gameEnrollListId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gameEnrollListId,teacherNo,gameCategoryId,gameTypeId ");
            strSql.Append(" FROM usta_GameEnrollList ORDER BY gameEnrollListId DESC;");
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }


        /// <summary>
        /// 根据教师编号、届次ID和活动类型ID获取报名信息
        /// </summary>
        public DataSet GetListByTeacherNo_GameCategoryId_GameTypeId(string teacherNo, int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT isOpenDraw,teacherName,gameTypeTitle,gameTitle,E.gameCategoryId,E.gameTypeId,E.teacherNo,F.gameDrawListId,F.groupIndex,F.groupNum, enrollUpdateTime,F.updateTime AS drawUpdateTime FROM (select  top 1 C.gameEnrollListId,A.isOpenDraw,A.[gameCategoryId],A.[gameTitle],C.[updateTime] AS enrollUpdateTime,D.teacherNo,D.teacherName,B.gameTypeTitle,B.gameTypeId from [usta_GameCategory] A,[usta_GameType] B,[usta_GameEnrollList] C,usta_TeachersList D WHERE A.gameCategoryId=@gameCategoryId AND B.gameTypeId=@gameTypeId AND C.teacherNo=@teacherNo AND A.gameCategoryId=C.gameCategoryId AND B.gameTypeId=C.gameTypeId AND D.teacherNo=C.teacherNo ORDER BY C.gameEnrollListId DESC) AS E LEFT JOIN usta_GameDrawList F ON E.gameCategoryId=F.gameCategoryId AND E.gameTypeId=F.gameTypeId AND E.teacherNo=F.teacherNo;");
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = teacherNo;
            parameters[1].Value = gameCategoryId;
            parameters[2].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }


        /// <summary>
        /// 根据GameCategoryId和gameTypeId获取报名人数
        /// </summary>
        public DataSet GetEnrollListCountByGameCategoryIdAndGameTypeId(int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(gameEnrollListId) from usta_GameEnrollList ");
            strSql.Append(" where gameCategoryId=@gameCategoryId AND gameTypeId=@gameTypeId;");
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
