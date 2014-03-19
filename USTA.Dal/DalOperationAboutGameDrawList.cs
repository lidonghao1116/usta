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
    public class DalOperationAboutGameDrawList
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
        public DalOperationAboutGameDrawList()
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
            strSql.Append("select gameDrawListId from usta_GameDrawList");
            strSql.Append(" where teacherNo=@teacherNo AND gameCategoryId=@gameCategoryId AND gameTypeId=@gameTypeId;");
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = teacherNo;
            parameters[1].Value = gameCategoryId;
            parameters[2].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn,CommandType.Text, strSql.ToString(), parameters).Tables[0].Rows.Count;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(GameDrawList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_GameDrawList(");
            strSql.Append("teacherNo,gameCategoryId,gameTypeId,groupNum,groupIndex,updateTime)");
            strSql.Append(" values (");
            strSql.Append("@teacherNo,@gameCategoryId,@gameTypeId,@groupNum,@groupIndex,@updateTime)");
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4),
					new SqlParameter("@groupNum", SqlDbType.NChar,10),
					new SqlParameter("@groupIndex", SqlDbType.TinyInt,1),
					new SqlParameter("@updateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.teacherNo;
            parameters[1].Value = model.gameCategoryId;
            parameters[2].Value = model.gameTypeId;
            parameters[3].Value = model.groupNum;
            parameters[4].Value = model.groupIndex;
            parameters[5].Value = model.updateTime;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(GameDrawList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_GameDrawList set ");
            strSql.Append("teacherNo=@teacherNo,");
            strSql.Append("gameCategoryId=@gameCategoryId,");
            strSql.Append("gameTypeId=@gameTypeId,");
            strSql.Append("groupNum=@groupNum,");
            strSql.Append("groupIndex=@groupIndex");
            strSql.Append(" where gameDrawListId=@gameDrawListId");
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo", SqlDbType.NVarChar,50),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4),
					new SqlParameter("@groupNum", SqlDbType.NChar,10),
					new SqlParameter("@groupIndex", SqlDbType.TinyInt,1),
					new SqlParameter("@gameDrawListId", SqlDbType.Int,4)};
            parameters[0].Value = model.teacherNo;
            parameters[1].Value = model.gameCategoryId;
            parameters[2].Value = model.gameTypeId;
            parameters[3].Value = model.groupNum;
            parameters[4].Value = model.groupIndex;
            parameters[5].Value = model.gameDrawListId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int gameDrawListId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_GameDrawList ");
            strSql.Append(" where gameDrawListId=@gameDrawListId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameDrawListId", SqlDbType.Int,4)
};
            parameters[0].Value = gameDrawListId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DataSet Get(int gameDrawListId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gameDrawListId,teacherNo,gameCategoryId,gameTypeId,groupNum,groupIndex from usta_GameDrawList ");
            strSql.Append(" where gameDrawListId=@gameDrawListId");
            SqlParameter[] parameters = {
					new SqlParameter("@gameDrawListId", SqlDbType.Int,4)
};
            parameters[0].Value = gameDrawListId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据活动届次和类型ID获取数据列表
        /// </summary>
        public DataSet GetList(int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT isOpenDraw,teacherName,gameTypeTitle,gameTitle,E.gameCategoryId,E.gameTypeId,E.teacherNo,F.gameDrawListId,F.groupIndex,F.groupNum, enrollUpdateTime,F.updateTime AS drawUpdateTime FROM (select C.gameEnrollListId,A.isOpenDraw,A.[gameCategoryId],A.[gameTitle],C.[updateTime] AS enrollUpdateTime,D.teacherNo,D.teacherName,B.gameTypeTitle,B.gameTypeId from [usta_GameCategory] A,[usta_GameType] B,[usta_GameEnrollList] C,usta_TeachersList D WHERE A.gameCategoryId=@gameCategoryId AND B.gameTypeId=@gameTypeId AND A.gameCategoryId=C.gameCategoryId AND B.gameTypeId=C.gameTypeId AND D.teacherNo=C.teacherNo) AS E LEFT JOIN usta_GameDrawList F ON E.gameCategoryId=F.gameCategoryId AND E.gameTypeId=F.gameTypeId AND E.teacherNo=F.teacherNo;");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;
            parameters[1].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }


        /// <summary>
        /// 根据活动届次和类型ID获取抽签列表数据
        /// </summary>
        public DataSet GetGroupNumList(int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DISTINCT [groupNum],[gameCategoryId],[gameTypeId] from [usta_GameDrawList] WHERE gameCategoryId=@gameCategoryId AND gameTypeId=@gameTypeId ORDER BY groupNum DESC;");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;
            parameters[1].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }


        /// <summary>
        /// 根据GroupNum获取抽签结果
        /// </summary>
        public DataSet GetGroupIndexList(string groupNum,int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [gameDrawListId],A.[teacherNo],B.teacherName, [gameCategoryId],[gameTypeId],[groupNum],[groupIndex],[updateTime] from [usta_GameDrawList] A,usta_TeachersList B WHERE A.teacherNo=B.teacherNo AND groupNum=@groupNum AND A.gameCategoryId=@gameCategoryId AND A.gameTypeId=@gameTypeId ORDER BY groupIndex ASC;");
            SqlParameter[] parameters = {
					new SqlParameter("@groupNum", SqlDbType.NChar,10),
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = groupNum;
            parameters[1].Value = gameCategoryId;
            parameters[2].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
    }

        /// <summary>
        /// 根据活动届次ID和活动类型ID判断是否已经有人开始抽签
        /// </summary>
        public DataSet CheckIsDrawByGameCategoryId(int gameCategoryId,int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [gameDrawListId],[teacherNo],[gameCategoryId],[gameTypeId],[groupNum],[groupIndex],[updateTime] from [usta_GameDrawList] WHERE gameCategoryId=@gameCategoryId AND gameTypeId=@gameTypeId;");
            SqlParameter[] parameters = {
					new SqlParameter("@gameCategoryId", SqlDbType.Int,4),
					new SqlParameter("@gameTypeId", SqlDbType.Int,4)
};
            parameters[0].Value = gameCategoryId;
            parameters[1].Value = gameTypeId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据活动届次ID和活动类型ID获取已经使用的编号
        /// </summary>
        public DataSet GetGroupNumAndIndexByGameCategoryIdAndGameTypeId(int gameCategoryId, int gameTypeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (RTRIM([groupNum]) + RTRIM([groupIndex])) AS groupNumAndIndex from [usta_GameDrawList] WHERE gameCategoryId=@gameCategoryId AND gameTypeId=@gameTypeId;");
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
