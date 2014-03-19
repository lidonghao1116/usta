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
    public class DalOperationAboutGradeCheckApplyReason
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DalOperationAboutGradeCheckApplyReason()
        {

            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }



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



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(StudentsGradeCheckApplyReason model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_StudentsGradeCheckApplyReason(");
            strSql.Append("gradeCheckApplyReasonTitle,gradeCheckApplyReasonRemark)");
            strSql.Append(" values (");
            strSql.Append("@gradeCheckApplyReasonTitle,@gradeCheckApplyReasonRemark);");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckApplyReasonTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@gradeCheckApplyReasonRemark", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.gradeCheckApplyReasonTitle;
            parameters[1].Value = model.gradeCheckApplyReasonRemark;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(StudentsGradeCheckApplyReason model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckApplyReason set ");
            strSql.Append("gradeCheckApplyReasonTitle=@gradeCheckApplyReasonTitle,");
            strSql.Append("gradeCheckApplyReasonRemark=@gradeCheckApplyReasonRemark");
            strSql.Append(" where gradeCheckApplyReasonId=@gradeCheckApplyReasonId");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckApplyReasonTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@gradeCheckApplyReasonRemark", SqlDbType.NVarChar,200),
					new SqlParameter("@gradeCheckApplyReasonId", SqlDbType.Int,4)};
            parameters[0].Value = model.gradeCheckApplyReasonTitle;
            parameters[1].Value = model.gradeCheckApplyReasonRemark;
            parameters[2].Value = model.gradeCheckApplyReasonId;

            int rows = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int gradeCheckApplyReasonId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckApplyReason ");
            strSql.Append(" where gradeCheckApplyReasonId=@gradeCheckApplyReasonId");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckApplyReasonId", SqlDbType.Int,4)
};
            parameters[0].Value = gradeCheckApplyReasonId;

            int rows = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        public DataSet Get(int gradeCheckApplyReasonId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckApplyReasonId,gradeCheckApplyReasonTitle,gradeCheckApplyReasonRemark from usta_StudentsGradeCheckApplyReason ");
            strSql.Append(" where gradeCheckApplyReasonId=@gradeCheckApplyReasonId");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckApplyReasonId", SqlDbType.Int,4)
};
            parameters[0].Value = gradeCheckApplyReasonId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        public DataSet GetAll()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  gradeCheckApplyReasonId,gradeCheckApplyReasonTitle,gradeCheckApplyReasonRemark from usta_StudentsGradeCheckApplyReason;");

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
    }
}
