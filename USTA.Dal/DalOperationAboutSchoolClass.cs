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
    /// 
    /// </summary>
    public class DalOperationAboutSchoolClass
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
        public DalOperationAboutSchoolClass()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion


        /// <summary>
        /// 查看指定年级和专业下的全部班级
        /// </summary>
        /// <returns>班级数据集</returns>
        public DataSet GetSchoolClassByMajorAndTermYear(string major, string termYear)
        {
            string strSql = "select [classId],[className],[special],[remark],[MajorType],[SchoolClassID],[Headteacher],[HeadteacherName] from [usta_StudentClass] WHERE MajorType=@MajorType AND SUBSTRING(LTRIM(className),1,2)=@termYear";
            SqlParameter[] parameters = new SqlParameter[]{
							 new SqlParameter("@MajorType",  major),    
							 new SqlParameter("@termYear",  termYear)                                        
					};

            if (major == "all")
            {
                strSql = "select [classId],[className],[special],[remark],[MajorType],[SchoolClassID],[Headteacher],[HeadteacherName] from [usta_StudentClass] WHERE SUBSTRING(LTRIM(className),1,2)=@termYear";
                parameters = new SqlParameter[]{  
							 new SqlParameter("@termYear",  termYear)                                        
					};
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql, parameters);
            }


            if (termYear == "all")
            {
                strSql = "select [classId],[className],[special],[remark],[MajorType],[SchoolClassID],[Headteacher],[HeadteacherName] from [usta_StudentClass] WHERE MajorType=@MajorType";
                parameters = new SqlParameter[]{
							 new SqlParameter("@MajorType",  major)                     
					};
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql, parameters);
            }

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql, parameters);
        }


        /// <summary>
        /// 根据关键字模糊查找班级
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public DataSet GetSchoolClassByKeyWord(string keyword)
        {
            string strSql = "select [classId],[className],[special],[remark],[MajorType],[SchoolClassID],[Headteacher],[HeadteacherName] from [usta_StudentClass] WHERE className like @keyword;";
            SqlParameter[] parameters = new SqlParameter[]{
							 new SqlParameter("@keyword",  "%" + keyword + "%")                                      
					};

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql, parameters);
        }
    }
}