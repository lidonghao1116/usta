using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

using USTA.Model;

namespace USTA.Dal
{
    /// <summary>
    /// 考试安排操作类
    /// </summary>
    public class DalOperationAboutExamArrage
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
        public DalOperationAboutExamArrage()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region
        /// <summary>
        /// 通过考试安排主键查找考试安排
        /// </summary>
        /// <param name="examArrangeListId">考试安排主键</param>
        /// <returns>考试安排数据集</returns>
        public ExamArrangeList GetExamArrangeById(int examArrangeListId)
        {
            ExamArrangeList examArrangeList = null;
            string cmdstring = "SELECT [examArrangeListId],[courseName],[examArrangeTime] ,[examArrageAddress],[remark] ,[teacherName] ,[courseNo]  FROM [USTA].[dbo].[usta_ExamArrangeList] WHERE examArrangeListId=@examArrangeListId";
            SqlParameter[] parameters = new SqlParameter[1]{
               new SqlParameter("@examArrangeListId",examArrangeListId)
        };
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
            if (dr.Read())
                examArrangeList = new ExamArrangeList
                {
                    examArrangeListId = int.Parse(dr["examArrangeListId"].ToString()),
                    courseName = dr["courseName"].ToString().Trim(),
                    examArrangeTime = Convert.ToDateTime(dr["examArrangeTime"].ToString()),
                    examArrageAddress = dr["examArrageAddress"].ToString().Trim(),
                    remark = dr["remark"].ToString().Trim(),
                    teacherName = dr["teacherName"].ToString().Trim(),
                    courseNo = dr["courseNo"].ToString().Trim()

                };
            dr.Close();
            conn.Close();
            return examArrangeList;
        }

       /// <summary>
       /// 通过课程查看考试安排
       /// </summary>
       /// <param name="courseNo">课程编号</param>
       /// <returns>课程安排数据集</returns>
        public ExamArrangeList GetExamArrangeByCourseNo(string courseNo)
        {
            ExamArrangeList examArrangeList = null;
            string cmdstring = "SELECT [examArrangeListId],[courseName],[examArrangeTime] ,[examArrageAddress],[remark] ,[teacherName] ,[courseNo]  FROM [USTA].[dbo].[usta_ExamArrangeList] WHERE courseNo=@courseNo";
            SqlParameter[] parameters = new SqlParameter[1]{
               new SqlParameter("@courseNo",courseNo)
        };
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
            if (dr.Read())
                examArrangeList = new ExamArrangeList
                {
                    examArrangeListId = int.Parse(dr["examArrangeListId"].ToString()),
                    courseName = dr["courseName"].ToString().Trim(),
                    examArrangeTime = Convert.ToDateTime(dr["examArrangeTime"].ToString()),
                    examArrageAddress = dr["examArrageAddress"].ToString(),
                    remark = dr["remark"].ToString().Trim(),
                    teacherName = dr["teacherName"].ToString().Trim(),
                    courseNo = dr["courseNo"].ToString().Trim()

                };
            dr.Close();
            conn.Close();
            return examArrangeList;
        }
        #endregion
    }
}
