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
    public class DalOperationAboutGradeCheck
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
        public DalOperationAboutGradeCheck()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion


        #region 获取学年
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetTermYear()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DISTINCT SUBSTRING(LTRIM(SchoolClassName),1,2) as termYear from usta_StudentsList WHERE LTRIM(RTRIM(SchoolClassName)) not like '%暂%' ORDER BY termYear DESC;");

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
        #endregion

        #region 根据学年、专业、班级搜索学生数据
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet SearchStudentsList(string termYear, string major, string schoolClass, string studentNoOrName,string locale,string degree)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from (select  A.studentNo,A.studentName,A.studentUserPwd,A.studentSpeciality,A.mobileNo,A.emailAddress,A.remark,A.classNo,A.StudentID,A.StudentUSID,A.MajorType,A.SchoolClass,A.SchoolClassName,A.isAdmin,A.Sex,A.CardNum,A.CardType,A.MatriculationDate,B.locale from usta_StudentsList A,usta_StudentClass B WHERE A.SchoolClass=B.SchoolClassID ) AS D LEFT JOIN usta_StudentsGradeCheckConfirm C ON C.studentNo=D.studentNo AND C.gradeCheckConfirmId in (SELECT top 1 gradeCheckConfirmId from usta_StudentsGradeCheckConfirm WHERE studentNo=D.studentNo  order by gradeCheckConfirmId desc)");

            List<string> condition = new List<string>();
            List<SqlParameter> conditionParameter = new List<SqlParameter>();

            strSql.Append(" WHERE ");

            if (locale != "all")
            {
                condition.Add(" D.locale=@locale ");
                conditionParameter.Add(new SqlParameter("@locale", locale));
            }

            if (degree != "all")
            {
                if (degree == "-1")
                {
                    condition.Add(" C.isAccord IS NULL ");
                    conditionParameter.Add(new SqlParameter("@isAccord", degree));
                }
                else
                {
                    condition.Add(" C.isAccord=@isAccord ");
                    conditionParameter.Add(new SqlParameter("@isAccord", int.Parse(degree)));
                }
            }

            if (termYear != "all")
            {
                condition.Add(" SUBSTRING(RTRIM(LTRIM(D.SchoolClassName)),1,2)=@termYear ");
                conditionParameter.Add(new SqlParameter("@termYear", termYear));
            }

            if (major != "all")
            {
                condition.Add(" D.MajorType=@MajorType ");
                conditionParameter.Add(new SqlParameter("@MajorType", major));
            }

            if (schoolClass != "all")
            {
                condition.Add(" D.SchoolClass=@SchoolClass ");
                conditionParameter.Add(new SqlParameter("@SchoolClass", schoolClass));
            }

            condition.Add(" (D.studentNo like @keyword or D.studentName like @keyword) ");
            conditionParameter.Add(new SqlParameter("@keyword", "%" + studentNoOrName + "%"));

            if (condition.Count > 0)
            {
                strSql.Append(string.Join("AND", condition.ToArray()));
            }

            SqlParameter[] parameters = conditionParameter.ToArray<SqlParameter>();

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 获取所有的成绩审核单项数据

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllGradeCheckItems()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gradeCheckId,gradeCheckItemName,gradeCheckItemDefaultValue,displayOrder,termYear,termYears from usta_StudentsGradeCheck ORDER BY displayOrder ASC,termYear DESC;");

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());

        }

        #endregion


        #region 根据学年获取成绩审核单项数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="termYear"></param>
        /// <returns></returns>
        public DataSet GetGradeCheckItemsByTermYear(string termYear)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gradeCheckId,gradeCheckItemName,gradeCheckItemDefaultValue,displayOrder,termYear,termYears from usta_StudentsGradeCheck ");
            strSql.Append(" where charindex(@termYear,termYears)>0 ORDER BY displayOrder ASC;");
            SqlParameter[] parameters = {
					new SqlParameter("@termYear", SqlDbType.NVarChar, 50)
};
            parameters[0].Value = termYear;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion


        #region 根据名称获取成绩审核单项数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gradeCheckItemName"></param>
        /// <param name="termYear"></param>
        /// <returns></returns>
        public DataSet GetGradeCheckItemsByGradeCheckItemName(string gradeCheckItemName, string termYear)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 gradeCheckId,gradeCheckItemName,gradeCheckItemDefaultValue,displayOrder,termYear,termYears from usta_StudentsGradeCheck ");
            strSql.Append(" where gradeCheckItemName=@gradeCheckItemName AND CHARINDEX(@termYear,termYears) ORDER BY displayOrder ASC;");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckItemName", SqlDbType.NChar, 50),
					new SqlParameter("@termYear", SqlDbType.NVarChar, 50)
};
            parameters[0].Value = gradeCheckItemName;
            parameters[1].Value = termYear;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 根据学号、学年、培养地来判断是否符合数据一致性
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gradeCheckItemName"></param>
        /// <returns></returns>
        public DataSet CheckDataConsistenceByStudentNoTermYearLocale(string studentNo,string termYear,string locale)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT studentNo FROM usta_StudentsList A,usta_StudentClass B where A.SchoolClass=B.SchoolClassID AND SUBSTRING(B.className,1,2)=@termYear AND B.locale=@locale And A.studentNo=@studentNo");
            SqlParameter[] parameters = {
                    new SqlParameter("@studentNo", SqlDbType.NChar,10),
                    new SqlParameter("@termYear", SqlDbType.NChar,2),
                    new SqlParameter("@locale", SqlDbType.NVarChar,20)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = termYear;
            parameters[2].Value = locale;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 根据ID获取成绩审核单项数据
        /// <summary>
        /// 根据ID获取成绩审核单项数据
        /// </summary>
        /// <param name="gradeCheckId"></param>
        /// <returns></returns>
        public DataSet GetGradeCheckItemById(int gradeCheckId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckId,gradeCheckItemName,gradeCheckItemDefaultValue,displayOrder,termYear,termYears from usta_StudentsGradeCheck ");
            strSql.Append(" where gradeCheckId=@gradeCheckId");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckId", SqlDbType.Int,4)
};
            parameters[0].Value = gradeCheckId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 根据ID删除成绩审核单项数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gradeCheckId"></param>
        /// <returns></returns>
        public int DeleteGradeCheckItemById(int gradeCheckId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheck ");
            strSql.Append(" where gradeCheckId=@gradeCheckId;");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckId", SqlDbType.Int,4)
};
            parameters[0].Value = gradeCheckId;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 根据ID更新成绩审核单项数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateGradeCheckItemById(StudentsGradeCheck model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheck set ");
            strSql.Append("gradeCheckItemName=@gradeCheckItemName,");
            strSql.Append("gradeCheckItemDefaultValue=@gradeCheckItemDefaultValue,");
            strSql.Append("displayOrder=@displayOrder,");
            strSql.Append("termYears=@termYears");
            strSql.Append(" where gradeCheckId=@gradeCheckId");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckItemName", SqlDbType.NChar,50),
					new SqlParameter("@gradeCheckItemDefaultValue", SqlDbType.NChar,50),
					new SqlParameter("@gradeCheckId", SqlDbType.Int,4),
					new SqlParameter("@displayOrder", SqlDbType.Int,4),
					new SqlParameter("@termYears", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.gradeCheckItemName;
            parameters[1].Value = model.gradeCheckItemDefaultValue;
            parameters[2].Value = model.gradeCheckId;
            parameters[3].Value = model.displayOrder;
            parameters[4].Value = model.termYears;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 增加一条成绩审核单项数据

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddGradeCheckItems(StudentsGradeCheck model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_StudentsGradeCheck(");
            strSql.Append("gradeCheckItemName,gradeCheckItemDefaultValue,displayOrder,termYears)");
            strSql.Append(" values (");
            strSql.Append("@gradeCheckItemName,@gradeCheckItemDefaultValue,@displayOrder,@termYears);");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckItemName", SqlDbType.NChar,50),
					new SqlParameter("@gradeCheckItemDefaultValue", SqlDbType.NChar,50),
					new SqlParameter("@displayOrder", SqlDbType.Int,4),
					new SqlParameter("@termYears", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.gradeCheckItemName;
            parameters[1].Value = model.gradeCheckItemDefaultValue;
            parameters[2].Value = model.displayOrder;
            parameters[3].Value = model.termYears;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 为单个学生添加成绩审核单项具体数据

        #region 根据学号获取学年
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetTermYearByStudentNo(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DISTINCT SchoolClassName,studentNo,studentName from usta_StudentsList WHERE studentNo=@studentNo;");

            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
};
            parameters[0].Value = studentNo.Trim();

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据学号获取更新时间信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetUpdateTimeByStudentNo(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DISTINCT updateTime from usta_StudentsGradeCheckDetail WHERE studentNo=@studentNo;");

            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
};
            parameters[0].Value = studentNo.Trim();

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据学号获取成绩审核单项具体数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentNo"></param>
        /// <returns></returns>
        public DataSet GetGradeCheckDetailByStudentNo(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select gradeCheckDetailId,A.gradeCheckId,gradeCheckDetailValue, A.updateTime from usta_StudentsGradeCheckDetail A");
            strSql.Append(" WHERE a.studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
};
            parameters[0].Value = studentNo;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion


        #region 根据学号增加成绩审核单项具体数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddGradeCheckDetailByStudentNo(StudentsGradeCheckDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_StudentsGradeCheckDetail(");
            strSql.Append("gradeCheckId,gradeCheckDetailValue,studentNo,updateTime)");
            strSql.Append(" values (");
            strSql.Append("@gradeCheckId,@gradeCheckDetailValue,@studentNo,@updateTime)");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckId", SqlDbType.Int,4),
					new SqlParameter("@gradeCheckDetailValue", SqlDbType.NVarChar,200),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.gradeCheckId;
            parameters[1].Value = model.gradeCheckDetailValue;
            parameters[2].Value = model.studentNo;
            parameters[3].Value = model.updateTime;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion


        #region 根据学号更新成绩审核单项具体数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateGradeCheckDetailByStudentNo(StudentsGradeCheckDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckDetail set ");
            strSql.Append("gradeCheckDetailValue=@gradeCheckDetailValue");
            strSql.Append(" where studentNo=@studentNo AND updateTime=@updateTime AND gradeCheckId=@gradeCheckId");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckDetailValue", SqlDbType.NVarChar,200),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@gradeCheckId", SqlDbType.Int,4)};
            parameters[0].Value = model.gradeCheckDetailValue;
            parameters[1].Value = model.studentNo;
            parameters[2].Value = model.updateTime;
            parameters[3].Value = model.gradeCheckId;



            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion


        #region 根据学号删除成绩审核单项具体数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="gradeCheckDetailId"></param>
        /// <returns></returns>
        public int DeleteGradeCheckDetailByStudentNoAndGradeCheckDetailId(string studentNo, int gradeCheckDetailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckDetail ");
            strSql.Append(" where gradeCheckDetailId=@gradeCheckDetailId");
            strSql.Append(" AND studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,4),
					new SqlParameter("@gradeCheckDetailId", SqlDbType.Int,4)
};
            parameters[0].Value = gradeCheckDetailId;
            parameters[1].Value = studentNo;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 根据学号和更新时间获取对应的成绩审核单项具体数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public DataSet GetAllGradeCheckDetailByStudentNoAndUpdateTime(string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select B.gradeCheckDetailId,A.gradeCheckId,B.gradeCheckDetailValue, B.updateTime,A.gradeCheckItemName,A.gradeCheckItemDefaultValue,A.displayOrder from usta_StudentsGradeCheck A,usta_StudentsGradeCheckDetail B");
            strSql.Append(" WHERE  A.gradeCheckId=B.gradeCheckId AND B.studentNo=@studentNo AND B.updateTime=@updateTime");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;



            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion



        #region 根据规则ID，学号和更新时间获取最新一条的成绩审核单项具体数据
        /// <summary>
        /// 根据规则ID，学号和更新时间获取最新一条的成绩审核单项具体数据
        /// </summary>
        /// <param name="gradeCheckId"></param>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public DataSet GetGradeCheckDetailByGradeCheckIdAndStudentNoAndUpdateTime(int gradeCheckId, string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [gradeCheckDetailId],[gradeCheckId],[gradeCheckDetailValue],[studentNo],[updateTime] FROM [USTA].[dbo].[usta_StudentsGradeCheckDetail] ");
            strSql.Append(" WHERE  gradeCheckId=@gradeCheckId AND studentNo=@studentNo AND updateTime=@updateTime");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@gradeCheckId", SqlDbType.Int,4)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;
            parameters[2].Value = gradeCheckId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion

        #region 根据学号和更新时间删除成绩审核单项具体数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public int DeleteGradeCheckDetailByStudentNoAndUpdateTime(string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckDetail ");
            strSql.Append(" where updateTime=@updateTime");
            strSql.Append(" AND studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion


        #endregion

        #region 是否符合学位申请相关操作
        #region 根据学号和成绩审核时间更新是否符合学位申请
        /// <summary>
        /// 根据学号和成绩审核时间更新是否符合学位申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateStudentGradeCheckConfirmByStudentNoAndUpdateTime(StudentsGradeCheckConfirm model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckConfirm set ");
            strSql.Append("isAccord=@isAccord,remark=@remark");
            strSql.Append(" where updateTime=@updateTime AND studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@isAccord", SqlDbType.TinyInt,1),
					new SqlParameter("@remark", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.studentNo;
            parameters[1].Value = model.updateTime;
            parameters[2].Value = model.isAccord;
            parameters[3].Value = model.remark;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion



        #region 根据学号和成绩审核时间删除学位确认记录
        /// <summary>
        /// 根据学号和成绩审核时间删除学位确认记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DeleteStudentGradeCheckConfirmByStudentNoAndUpdateTime(StudentsGradeCheckConfirm model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckConfirm ");
            strSql.Append(" where updateTime=@updateTime AND studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.studentNo;
            parameters[1].Value = model.updateTime;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion


        #region 添加一条确认记录
        /// <summary>
        /// 添加一条确认记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddStudentGradeCheckConfirm(StudentsGradeCheckConfirm model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_StudentsGradeCheckConfirm(");
            strSql.Append("studentNo,updateTime,isAccord,remark)");
            strSql.Append(" values (");
            strSql.Append("@studentNo,@updateTime,@isAccord,@remark)");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@isAccord", SqlDbType.TinyInt,1),
					new SqlParameter("@remark", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.studentNo;
            parameters[1].Value = model.updateTime;
            parameters[2].Value = model.isAccord;
            parameters[3].Value = model.remark;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion


        #region 根据学号和成绩审核时间查询确认记录
        /// <summary>
        /// 根据学号和成绩审核时间查询确认记录
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public DataSet GetStudentGradeCheckConfirm(string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckConfirmId,studentNo,updateTime,isAccord,remark from usta_StudentsGradeCheckConfirm ");
            strSql.Append(" where studentNo=@studentNo AND updateTime=@updateTime");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion

        #region 获取成绩审核确认的记录条数
        /// <summary>
        /// 获取成绩审核确认的记录条数
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public DataSet GetStudentGradeCheckConfirmRecordNum(string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  gradeCheckConfirmId,studentNo,updateTime,isAccord,remark from usta_StudentsGradeCheckConfirm ");
            strSql.Append(" where studentNo=@studentNo AND updateTime=@updateTime");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion



        #region 检查是否已经确认过成绩审核信息
        /// <summary>
        /// 检查是否已经确认过成绩审核信息
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        public DataSet CheckIsConfirmGradeInfo(string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckConfirmId,studentNo,updateTime,isAccord,remark from usta_StudentsGradeCheckConfirm ");
            strSql.Append(" where studentNo=@studentNo AND updateTime=@updateTime");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion


        #region 重修重考文件管理

        #region 获取重修重考文件
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetGradeCheckDocument()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckConfigId,notifyTitle,notifyContent,attachmentIds from usta_StudentsGradeCheckConfig;");


            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
        #endregion


        #region 获取重修重考Excel模板文件
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetGradeCheckExcelTemplate()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 excelTemplateAttachmentIds from usta_StudentsGradeCheckConfig;");


            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
        #endregion

        #region 更新重修重考Excel模板文件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelTemplateAttachmentIds"></param>
        /// <returns></returns>
        public int UpdateGradeCheckExcelTemplate(string excelTemplateAttachmentIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckConfig set ");
            strSql.Append("excelTemplateAttachmentIds=@excelTemplateAttachmentIds;");
            SqlParameter[] parameters = {
					new SqlParameter("@excelTemplateAttachmentIds", SqlDbType.NVarChar,200)
                                        };
            parameters[0].Value = excelTemplateAttachmentIds;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 更新重修重考文件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="docName"></param>
        /// <returns></returns>
        public int UpdateGradeCheckDocument(string path, string docName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckConfig set ");
            strSql.Append("gradeCheckDocumentPath=@gradeCheckDocumentPath,gradeCheckDocumentName=@gradeCheckDocumentName;");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckDocumentPath", SqlDbType.NVarChar,200),
					new SqlParameter("@gradeCheckDocumentName", SqlDbType.NVarChar,100)
                                        };
            parameters[0].Value = path;
            parameters[1].Value = docName;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 添加重修重考文件
        public DataSet AddGradeCheckDocument(string path, string docName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_StudentsGradeCheckConfig(");
            strSql.Append("gradeCheckDocumentPath,gradeCheckDocumentName)");
            strSql.Append(" values (");
            strSql.Append("@gradeCheckDocumentPath,@gradeCheckDocumentName)");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckDocumentPath", SqlDbType.NVarChar,200),
					new SqlParameter("@gradeCheckDocumentName", SqlDbType.NVarChar,100)};
            parameters[0].Value = path;
            parameters[1].Value = docName;


            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #endregion

        #region 重修重考通知管理

        #region 获取重修重考通知
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetGradeCheckNotify()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckConfigId,notifyTitle,notifyContent,startTime,endTime,attachmentIds from usta_StudentsGradeCheckConfig;");


            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
        #endregion


        #region 更新重修重考通知
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifyTitle"></param>
        /// <param name="notifyContent"></param>
        /// <returns></returns>
        public int UpdateGradeCheckNotify(string notifyTitle, string notifyContent, string attachmentIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckConfig set ");
            strSql.Append("notifyTitle=@notifyTitle,notifyContent=@notifyContent,attachmentIds=@attachmentIds;");
            SqlParameter[] parameters = {
					new SqlParameter("@notifyTitle", SqlDbType.NVarChar,100),
					new SqlParameter("@notifyContent", SqlDbType.Text),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)
                                        };
            parameters[0].Value = notifyTitle;
            parameters[1].Value = notifyContent;
            parameters[2].Value = attachmentIds;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #endregion



        #region 重修重考开放时间管理

        #region 获取重修重考开放时间管理
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetGradeCheckAllowTime()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckConfigId,notifyTitle,notifyContent,startTime,endTime from usta_StudentsGradeCheckConfig;");


            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
        #endregion


        #region 更新重修重考开放时间管理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int UpdateGradeCheckAllowTime(DateTime startTime, DateTime endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckConfig set ");
            strSql.Append("startTime=@startTime,endTime=@endTime;");
            SqlParameter[] parameters = {
					new SqlParameter("@startTime", SqlDbType.DateTime),
					new SqlParameter("@endTime", SqlDbType.DateTime)
                                        };
            parameters[0].Value = startTime;
            parameters[1].Value = endTime;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 判断重修重考开始时间是否晚于截止时间
        /// <summary>
        /// 判断重修重考开始时间是否晚于截止时间
        /// </summary>
        /// <returns></returns>
        public bool CheckGradeCheckAllowTime()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 startTime,endTime from usta_StudentsGradeCheckConfig;");

            bool isHasStartTime = false;
            bool isHasEndTime = false;

            DateTime _startTime = DateTime.Now;
            DateTime _endTime = _startTime;

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, strSql.ToString());

            while (dr.Read())
            {
                if (dr["startTime"] != null)
                {
                    isHasStartTime = true;
                    _startTime = Convert.ToDateTime(dr["startTime"].ToString().Trim());
                }

                if (dr["endTime"] != null)
                {
                    isHasEndTime = true;
                    _endTime = Convert.ToDateTime(dr["endTime"].ToString().Trim());
                }

            }
            dr.Close();
            conn.Close();

            if (isHasStartTime && isHasEndTime && (DateTime.Now > _startTime) && (DateTime.Now < _endTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #endregion

        #region 获取全部成绩审核数据相关操作

        #region 获取指定学年的全部学生及班级相关数据
        /// <summary>
        /// 获取指定学年的全部学生及班级相关数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllStudentsDataAboutGradeCheckData(string termYear)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [studentNo],[studentName],[SchoolClass],[sex],[SchoolClassName],locale,SUBSTRING(LTRIM(RTRIM(SchoolClassName)),1,2)+'级' as termYear,[HeadteacherName] FROM usta_StudentsList A,usta_StudentClass B WHERE A.SchoolClass=B.SchoolClassID AND SUBSTRING(LTRIM(RTRIM(SchoolClassName)),1,2)=@termYear ORDER BY locale ASC;");

            SqlParameter[] parameters = {
					new SqlParameter("@termYear", SqlDbType.NChar,10)};
            parameters[0].Value = termYear;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据培养地获取指定学年的全部学生及班级相关数据
        /// <summary>
        /// 根据培养地获取指定学年的全部学生及班级相关数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllStudentsDataAboutGradeCheckDataByLocale(string termYear, string locale)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [studentNo],[studentName],[SchoolClass],[sex],[SchoolClassName],locale,SUBSTRING(LTRIM(RTRIM(SchoolClassName)),1,2)+'级' as termYear,[HeadteacherName] FROM usta_StudentsList A,usta_StudentClass B WHERE A.SchoolClass=B.SchoolClassID AND SUBSTRING(LTRIM(RTRIM(SchoolClassName)),1,2)=@termYear AND locale=@locale;");

            SqlParameter[] parameters = {
					new SqlParameter("@termYear", SqlDbType.NChar,10),
					new SqlParameter("@locale", SqlDbType.NVarChar,20)
                                        };
            parameters[0].Value = termYear;
            parameters[1].Value = locale;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 获取指定学年的全部学生的成绩审核相关数据
        /// <summary>
        /// 获取指定学年的全部学生的成绩审核相关数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllGradeCheckDataAboutStudentsData(string studentNo, int gradeCheckId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 [updateTime] from [usta_StudentsGradeCheckDetail] WHERE studentNo=@studentNo ORDER BY updateTime DESC;");
            SqlParameter[] parameters = new SqlParameter[]{
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
            };

            parameters[0].Value = studentNo;

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);


            if (ds.Tables[0].Rows.Count > 0)
            {
                strSql = new StringBuilder();
                strSql.Append("select top 1 B.gradeCheckDetailId,B.gradeCheckDetailValue,A.gradeCheckId,B.gradeCheckDetailValue, B.updateTime,A.gradeCheckItemName,A.gradeCheckItemDefaultValue,A.displayOrder from usta_StudentsGradeCheck A left join usta_StudentsGradeCheckDetail B on A.gradeCheckId=B.gradeCheckId ");
                strSql.Append(" WHERE A.gradeCheckId=@gradeCheckId AND B.studentNo=@studentNo AND B.updateTime=@updateTime");
                parameters = new SqlParameter[]{
                    new SqlParameter("@studentNo", SqlDbType.NChar,10),
                    new SqlParameter("@updateTime", SqlDbType.DateTime),
                    new SqlParameter("@gradeCheckId", SqlDbType.Int,4)
};
                parameters[0].Value = studentNo;
                parameters[1].Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["updateTime"]);
                parameters[2].Value = gradeCheckId;

                return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
            }
            else
            {
                return null;
            }
        }
        #endregion




        #region 获取指定学年的全部学生的是否符合学位申请及备注的信息
        /// <summary>
        /// 获取指定学年的全部学生的是否符合学位申请及备注的信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllGradeCheckDataAboutConfirmAndRemarkData(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 [updateTime] from [usta_StudentsGradeCheckDetail] WHERE studentNo=@studentNo ORDER BY updateTime DESC;");
            SqlParameter[] parameters = new SqlParameter[]{
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
            };

            parameters[0].Value = studentNo;

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);


            if (ds.Tables[0].Rows.Count > 0)
            {
                strSql = new StringBuilder();
                strSql.Append("select top 1 [isAccord],[remark] from [usta_StudentsGradeCheckConfirm]");
                strSql.Append(" WHERE studentNo=@studentNo AND updateTime=@updateTime");
                parameters = new SqlParameter[]{
                    new SqlParameter("@studentNo", SqlDbType.NChar,10),
                    new SqlParameter("@updateTime", SqlDbType.DateTime)
};
                parameters[0].Value = studentNo;
                parameters[1].Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["updateTime"]);

                return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
            }
            else
            {
                return null;
            }
        }
        #endregion
        #endregion


        #region 办理重修重考相关

        #region 更新重修重考申请状态
        /// <summary>
        /// 更新重修重考申请状态
        /// </summary>
        /// <param name="applyResult"></param>
        /// <param name="applyChecKSuggestion"></param>
        /// <param name="gradeCheckApplyId"></param>
        /// <returns></returns>
        public int UpdateStudentGradeCheckApplyState(string applyResult, string applyChecKSuggestion, int gradeCheckApplyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckApply set ");
            strSql.Append("applyResult=@applyResult,");
            strSql.Append("applyChecKSuggestion=@applyChecKSuggestion");
            strSql.Append(" where gradeCheckApplyId=@gradeCheckApplyId");
            SqlParameter[] parameters = {
					new SqlParameter("@applyResult", SqlDbType.NChar,10),
					new SqlParameter("@applyChecKSuggestion", SqlDbType.NVarChar,500),
					new SqlParameter("@gradeCheckApplyId", SqlDbType.Int,4)};
            parameters[0].Value = applyResult;
            parameters[1].Value = applyChecKSuggestion;
            parameters[2].Value = gradeCheckApplyId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 增加一条重修重考记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddStudentGradeCheckApply(StudentsGradeCheckApply model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_StudentsGradeCheckApply(");
            strSql.Append("studentNo,updateTime,courseNo,ClassID,termTag,gradeCheckApplyType,applyReason,applyUpdateTime)");
            strSql.Append(" values (");
            strSql.Append("@studentNo,@updateTime,@courseNo,@ClassID,@termTag,@gradeCheckApplyType,@applyReason,@applyUpdateTime);");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@ClassID", SqlDbType.NVarChar,50),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50),
					new SqlParameter("@gradeCheckApplyType", SqlDbType.NChar,10),
					new SqlParameter("@applyReason", SqlDbType.NVarChar,50),
					new SqlParameter("@applyUpdateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.studentNo;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = model.courseNo;
            parameters[3].Value = model.ClassID;
            parameters[4].Value = model.termTag;
            parameters[5].Value = model.gradeCheckApplyType;
            parameters[6].Value = model.applyReason;
            parameters[7].Value = model.applyUpdateTime;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 修改一条重修重考记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateStudentGradeCheckApply(StudentsGradeCheckApply model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_StudentsGradeCheckApply set ");
            strSql.Append("studentNo=@studentNo,");
            strSql.Append("updateTime=@updateTime,");
            strSql.Append("courseNo=@courseNo,");
            strSql.Append("ClassID=@ClassID,");
            strSql.Append("termTag=@termTag,");
            strSql.Append("gradeCheckApplyType=@gradeCheckApplyType,");
            strSql.Append("applyReason=@applyReason");
            strSql.Append(" where gradeCheckApplyId=@gradeCheckApplyId AND studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@ClassID", SqlDbType.NVarChar,50),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50),
					new SqlParameter("@gradeCheckApplyType", SqlDbType.NChar,10),
					new SqlParameter("@gradeCheckApplyId", SqlDbType.Int,4),
					new SqlParameter("@applyReason", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.studentNo;
            parameters[1].Value = model.updateTime;
            parameters[2].Value = model.courseNo;
            parameters[3].Value = model.ClassID;
            parameters[4].Value = model.termTag;
            parameters[5].Value = model.gradeCheckApplyType;
            parameters[6].Value = model.gradeCheckApplyId;
            parameters[7].Value = model.applyReason;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据学号和ID删除一条重修重考记录(不可删除已经审核通过的记录)
        /// <summary>
        /// 根据学号和ID删除一条重修重考记录(不可删除已经审核通过的记录)
        /// </summary>
        /// <param name="gradeCheckApplyId"></param>
        /// <param name="studentNo"></param>
        /// <returns></returns>
        public int DeleteStudentGradeCheckApplyById(int gradeCheckApplyId, string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckApply ");
            strSql.Append(" where gradeCheckApplyId=@gradeCheckApplyId AND studentNo=@studentNo AND applyResult IS NULL");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckApplyId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
};
            parameters[0].Value = gradeCheckApplyId;
            parameters[1].Value = studentNo;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 根据学号和时间删除多条重修重考记录(不可删除已经审核通过的记录)
        /// <summary>
        /// 根据学号和时间删除多条重修重考记录(不可删除已经审核通过的记录)
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public int DeleteStudentGradeCheckApplyByStudentNoAndUpdateTime(string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckApply ");
            strSql.Append(" where studentNo=@studentNo AND updateTime=@updateTime AND applyResult IS NULL");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 根据主键获取重修重考记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gradeCheckApplyId"></param>
        /// <returns></returns>
        public DataSet GetStudentGradeCheckApplyById(int gradeCheckApplyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gradeCheckApplyId,studentNo,updateTime,A.courseNo,A.ClassID,A.termTag,courseName,gradeCheckApplyType,applyResult,applyChecKSuggestion from usta_StudentsGradeCheckApply A, usta_Courses B where A.courseNo=B.courseNo AND A.termTag=B.termTag AND A.ClassID=B.ClassID");

            strSql.Append(" AND gradeCheckApplyId=@gradeCheckApplyId;");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckApplyId", SqlDbType.Int,4)
};
            parameters[0].Value = gradeCheckApplyId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 根据学号获取重修重考记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentNo"></param>
        /// <returns></returns>
        public DataSet GetStudentGradeCheckApplyByStudentNo(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [applyReason],[gradeCheckApplyId],courseName,[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],[gradeCheckApplyType],[applyResult],[applyChecKSuggestion],[applyUpdateTime] from [usta_StudentsGradeCheckApply] A,[usta_Courses] B");
            strSql.Append(" WHERE studentNo=@studentNo AND A.courseNo=B.courseNo AND A.ClassID=B.ClassID AND A.termTag=B.termTag;");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
};
            parameters[0].Value = studentNo;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据学号和课程号判断是否有重复的重修重考记录
        /// <summary>
        /// 根据学号和课程号判断是否有重复的重修重考记录
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="termTagCourseNoClassID"></param>
        /// <returns></returns>
        public DataSet GetStudentGradeCheckApplyByStudentNoAndTermTagCourseNoClassID(string studentNo, string termTagCourseNoClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 [applyReason],[gradeCheckApplyId],courseName,[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],[gradeCheckApplyType],[applyResult],[applyChecKSuggestion],[applyUpdateTime] from [usta_StudentsGradeCheckApply] A,[usta_Courses] B");
            strSql.Append(" WHERE studentNo=@studentNo AND A.courseNo=B.courseNo AND A.ClassID=B.ClassID AND A.termTag=B.termTag AND (RTRIM(A.termTag)+RTRIM(A.courseNo)+RTRIM(A.ClassID))=@termTagCourseNoClassID;");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = termTagCourseNoClassID;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据学号和更新时间获取未处理的重修重考记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentNo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        public DataSet GetStudentGradeCheckApplyByStudentNoAndUpdateTimeNotDeal(string studentNo, DateTime updateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [gradeCheckApplyId],courseName,[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],[gradeCheckApplyType],[applyResult],[applyChecKSuggestion],[applyReason] from [usta_StudentsGradeCheckApply] A,[usta_Courses] B");
            strSql.Append(" WHERE studentNo=@studentNo AND updateTime=@updateTime AND A.courseNo=B.courseNo AND A.ClassID=B.ClassID AND A.termTag=B.termTag AND applyResult IS NULL;");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@updateTime", SqlDbType.DateTime)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = updateTime;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion



        #region 根据课程获取重修重考申请批准的学生信息
        /// <summary>
        /// 根据课程获取重修重考申请批准的学生信息
        /// </summary>
        /// <param name="termTagCourseNoClassID"></param>
        /// <returns></returns>
        public DataSet GetStudentGradeCheckApplyAccordByCourse(string termTagCourseNoClassID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM (select [SchoolClassName],[gradeCheckApplyId],A.[studentNo],B.studentName,[updateTime],[courseNo],[ClassID],[termTag],[gradeCheckApplyType],[applyResult],[applyChecKSuggestion],[applyReason] from [usta_StudentsGradeCheckApply] A,usta_StudentsList B");
            strSql.Append(" WHERE A.studentNo=B.studentNo AND (RTRIM(A.termTag)+RTRIM(A.courseNo)+RTRIM(A.ClassID))=@termTagCourseNoClassID AND applyResult='符合') AS C;");
            SqlParameter[] parameters = {
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120)
};
            parameters[0].Value = termTagCourseNoClassID;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 获取全部重修重考记录
        /// <summary>
        /// 获取全部重修重考记录
        /// </summary>
        /// <param name="termTagCourseNoClassID"></param>
        /// <param name="applyResult"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public DataSet GetAllStudentGradeCheckApply(string termTagCourseNoClassID, string applyResult,string locale)
        {
            StringBuilder strSql = new StringBuilder();
            SqlParameter[] parameters;

            if (termTagCourseNoClassID.StartsWith("all"))
            {
                string _termTag = termTagCourseNoClassID.Split("_".ToCharArray())[1];

                strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND A.termTag=@termTag ORDER BY applyResult ASC;");
                parameters = new SqlParameter[]{
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)
};
                parameters[0].Value = termTagCourseNoClassID;
                parameters[1].Value = _termTag;

                if (applyResult == "all" && locale == "all")
                {
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND A.termTag=@termTag ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[]{
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)
};
                    parameters[0].Value = termTagCourseNoClassID;
                    parameters[1].Value = _termTag;
                }
                else if (applyResult == "all")
                {
                    strSql = new StringBuilder();
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND C.locale=@locale AND A.termTag=@termTag ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[] {
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@locale", SqlDbType.NVarChar,20),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)
};
                    parameters[0].Value = termTagCourseNoClassID;
                    parameters[1].Value = locale;
                    parameters[2].Value = _termTag;
                }
                else if (locale == "all")
                {
                    strSql = new StringBuilder();
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND A.applyResult=@applyResult AND A.termTag=@termTag ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[] {
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@applyResult", SqlDbType.NChar,10),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)
};
                    parameters[0].Value = termTagCourseNoClassID;
                    parameters[1].Value = applyResult;
                    parameters[2].Value = _termTag;
                }
                else
                {
                    strSql = new StringBuilder();
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND C.locale=@locale AND A.applyResult=@applyResult AND A.termTag=@termTag ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[] {
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@applyResult", SqlDbType.NChar,10),
					new SqlParameter("@locale", SqlDbType.NVarChar,20),
					new SqlParameter("@termTag", SqlDbType.NVarChar,50)
};
                    parameters[0].Value = termTagCourseNoClassID;
                    parameters[1].Value = applyResult;
                    parameters[2].Value = locale;
                    parameters[3].Value = _termTag;
                }
            }
            else
            {
                if (applyResult == "all" && locale == "all")
                {
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND (RTRIM(A.termTag)+RTRIM(A.courseNo)+RTRIM(A.ClassID))=@termTagCourseNoClassID ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[]{
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120)
};
                    parameters[0].Value = termTagCourseNoClassID;
                }
                else if (applyResult == "all")
                {
                    strSql = new StringBuilder();
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND (RTRIM(A.termTag)+RTRIM(A.courseNo)+RTRIM(A.ClassID))=@termTagCourseNoClassID AND C.locale=@locale ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[] {
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@locale", SqlDbType.NVarChar,20)
};
                    parameters[0].Value = termTagCourseNoClassID;
                    parameters[1].Value = locale;
                }
                else if (locale == "all")
                {
                    strSql = new StringBuilder();
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND (RTRIM(A.termTag)+RTRIM(A.courseNo)+RTRIM(A.ClassID))=@termTagCourseNoClassID AND A.applyResult=@applyResult ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[] {
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@applyResult", SqlDbType.NChar,10)
};
                    parameters[0].Value = termTagCourseNoClassID;
                    parameters[1].Value = applyResult;
                }
                else
                {
                    strSql = new StringBuilder();
                    strSql.Append("select C.locale,[gradeCheckApplyId],B.[studentNo],[updateTime],A.[courseNo],A.[ClassID],A.[termTag],D.courseName,A.[applyResult],A.applyReason,A.[gradeCheckApplyType],A.[applyChecKSuggestion],A.[applyUpdateTime],[studentName],[studentSpeciality],[mobileNo],[emailAddress],[SchoolClassName] from [usta_StudentsGradeCheckApply] A,[usta_StudentsList] B, usta_StudentClass C,usta_Courses D");
                    strSql.Append(" where B.SchoolClass=C.SchoolClassID AND D.courseNo=A.courseNo AND D.termTag=A.termTag AND D.classID=A.classID AND A.studentNo=B.studentNo AND (RTRIM(A.termTag)+RTRIM(A.courseNo)+RTRIM(A.ClassID))=@termTagCourseNoClassID AND C.locale=@locale AND A.applyResult=@applyResult ORDER BY applyResult ASC;");
                    parameters = new SqlParameter[] {
					new SqlParameter("@termTagCourseNoClassID", SqlDbType.NVarChar,120),
					new SqlParameter("@applyResult", SqlDbType.NChar,10),
					new SqlParameter("@locale", SqlDbType.NVarChar,20)
};
                    parameters[0].Value = termTagCourseNoClassID;
                    parameters[1].Value = applyResult;
                    parameters[2].Value = locale;
                }
            }
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #endregion


        #endregion


        #region 成绩审核Excel导入功能相关

        #region 删除指定学年的全部审核数据

        /// <summary>
        /// 删除usta_StudentsGradeCheckConfirm表指定学年的数据
        /// </summary>
        /// <param name="termYear"></param>
        /// <returns></returns>
        public int DeleteStudentsGradeCheckConfirmItemsByTermYear(string termYear,string locale)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckConfirm ");
            strSql.Append(" WHERE studentNo in(SELECT studentNo FROM usta_StudentsList A,usta_StudentClass B where A.SchoolClass=B.SchoolClassID AND SUBSTRING(B.className,1,2)=@termYear AND B.locale=@locale);");
            SqlParameter[] parameters = {
                    new SqlParameter("@termYear", SqlDbType.NChar,2),
                    new SqlParameter("@locale", SqlDbType.NVarChar,20)
};
            parameters[0].Value = termYear;
            parameters[1].Value = locale;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        /// <summary>
        /// 删除usta_StudentsGradeCheckDetail表指定学年的数据
        /// </summary>
        /// <param name="termYear"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public int DeleteStudentsGradeCheckDetailItemsByTermYear(string termYear, string locale)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from usta_StudentsGradeCheckDetail ");
            strSql.Append(" WHERE studentNo in(SELECT studentNo FROM usta_StudentsList A,usta_StudentClass B where A.SchoolClass=B.SchoolClassID AND SUBSTRING(B.className,1,2)=@termYear AND B.locale=@locale);");
            SqlParameter[] parameters = {
                    new SqlParameter("@termYear", SqlDbType.NChar,2),
                    new SqlParameter("@locale", SqlDbType.NVarChar,20)
};
            parameters[0].Value = termYear;
            parameters[1].Value = locale;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);

        }

        #endregion

        #region 查询是否有重名的规则
        /// <summary>
        /// 查询是否有重名的规则
        /// </summary>
        /// <param name="gradeCheckItemName"></param>
        /// <returns></returns>
        public DataSet CheckIsExistGradeCheckItemName(string gradeCheckItemName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT gradeCheckItemName FROM dbo.usta_StudentsGradeCheck where gradeCheckItemName=@gradeCheckItemName");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckItemName", SqlDbType.NChar,50)
};
            parameters[0].Value = gradeCheckItemName;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 查询是否有重名的规则
        /// <summary>
        /// 查询是否有重名的规则
        /// </summary>
        /// <param name="gradeCheckItemName"></param>
        /// <param name="gradeCheckId"></param>
        /// <returns></returns>
        public DataSet CheckIsExistGradeCheckItemName(string gradeCheckItemName, int gradeCheckId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT gradeCheckItemName FROM dbo.usta_StudentsGradeCheck where gradeCheckItemName=@gradeCheckItemName and gradeCheckId=@gradeCheckId");
            SqlParameter[] parameters = {
					new SqlParameter("@gradeCheckItemName", SqlDbType.NChar,50),
					new SqlParameter("@gradeCheckId", SqlDbType.Int,4)
};
            parameters[0].Value = gradeCheckItemName;
            parameters[1].Value = gradeCheckId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #endregion
    }
}
