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
    using NPOI;
    using NPOI.HSSF.UserModel;

   public class DalOperationAboutEnglishExam
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
        public DalOperationAboutEnglishExam()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion


        #region 四六级报名教师管理部分

        #region 根据编号查找教师是否为班主任

        /// <summary>
        /// 根据编号查找教师是否为班主任
       /// </summary>
       /// <param name="teacherNo"></param>
       /// <returns></returns>
        public bool CheckIsHeadTeacherByTeacherNo(string teacherNo)
        {
            if(string.IsNullOrEmpty(teacherNo))
            {
                return false;
            }

            bool isHeaderTeacher = false;

            string commandString = "select teacherNo FROM usta_TeachersList WHERE teacherNo=@teacherNo AND isHeadteacher=1";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@teacherNo",teacherNo)
			};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
            while (dr.Read())
            {
                isHeaderTeacher = true;
            }
            dr.Close();
            conn.Close();

            return isHeaderTeacher;

        }
        #endregion

        #region 查找所有班级

        /// <summary>
        /// 查找所有班级
        /// </summary>
        /// <param name="teacherNo"></param>
        /// <returns></returns>
        public DataSet GetAllSchoolClass()
        {
            string commandString = "select [className],[SchoolClassID] FROM usta_StudentClass";
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString);
            return ds;
        }

        #endregion
       
        #region 根据班主任编号查找所管理的班级

        /// <summary>
        /// 根据班主任编号查找所管理的班级
        /// </summary>
        /// <param name="teacherNo"></param>
        /// <returns></returns>
        public DataSet GetSchoolClassByTeacherNo(string teacherNo)
        {
            string commandString = "select [className],[SchoolClassID] FROM usta_TeachersList A,usta_StudentClass B WHERE teacherNo=@teacherNo AND RTRIM(A.teacherNo)=RTRIM(B.Headteacher)";
            SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@teacherNo",teacherNo)
			};
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
        }

        #endregion
       
        #region 根据班级编号查找所有报名信息

        /// <summary>
        /// 根据班级编号查找所有报名信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetEnglishExamSignUpInfoByTeacherNo(string teacherNo, bool isAll, string schoolClassId,bool isSeach,string studentNoOrName,bool useSchoolClassId)
        {
            DataSet ds = new DataSet();

            List<string> listSchoolClass = new List<string>();


            if (teacherNo == "admin")
            {
                ds = this.GetAllSchoolClass();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    listSchoolClass.Add("'" + ds.Tables[0].Rows[i]["SchoolClassID"].ToString().Trim() + "'");
                }
            }
            else
            {
                if (!useSchoolClassId)
                {
                    ds = this.GetSchoolClassByTeacherNo(teacherNo);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listSchoolClass.Add("'" + ds.Tables[0].Rows[i]["SchoolClassID"].ToString().Trim() + "'");
                    }
                }
                else
                {
                    listSchoolClass.Add(schoolClassId);
                }
            }

            if (isSeach)
            {
                string commandString = "select [studentName],[englishExamId],[deadLineTime],[examCertificateState],[examCertificateRemark],[isPaid],[isPaidRemark],A.[studentNo],[cardType],C.[studentName],[cardNum],[sex],[SchoolClassName],[studentSpeciality],[examPlace],[examType],[grade],[gradeCertificateState],[gradeCertificateRemark],englishExamNotifyTitle,D.[englishExamNotifyId],[englishExamSignUpConfirm],A.[updateTime] FROM usta_EnglishExam A,usta_StudentClass B,usta_StudentsList C,usta_EnglishExamNotify D WHERE  D.englishExamNotifyId=A.englishExamNotifyId AND A.studentNo = C.studentNo AND B.SchoolClassID=C.SchoolClass AND (C.studentNo like @keyword or C.studentName like @keyword) AND C.SchoolClass in(" + (isAll ? String.Join(",", listSchoolClass.ToArray()) : "'" + schoolClassId + "'") + ") ORDER BY SchoolClassName;";
                SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@keyword","%"+studentNoOrName+"%")
                    };
                ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString,parameters);
            }
            else
            {
                if (listSchoolClass.Count >0)
                {
                    string commandString = "select [studentName],[englishExamId],[deadLineTime],[examCertificateState],[examCertificateRemark],[isPaid],[isPaidRemark],A.[studentNo],[examPlace],[examType],[cardType],C.[studentName],[cardNum],[sex],[SchoolClassName],[studentSpeciality],[grade],[gradeCertificateState],[gradeCertificateRemark],englishExamNotifyTitle,D.[englishExamNotifyId],[englishExamSignUpConfirm],A.[updateTime] FROM usta_EnglishExam A,usta_StudentClass B,usta_StudentsList C,usta_EnglishExamNotify D WHERE  D.englishExamNotifyId=A.englishExamNotifyId AND A.studentNo = C.studentNo AND B.SchoolClassID=C.SchoolClass AND C.SchoolClass in(" + (isAll ? String.Join(",", listSchoolClass.ToArray()) : "'" + schoolClassId + "'") + ") ORDER BY SchoolClassName;";
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString);
                }
            }
            return ds;
        }

        #endregion


        #region 根据班级编号和培养地查找所有报名信息

        /// <summary>
        /// 根据班级编号查找所有报名信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetEnglishExamSignUpInfoByTeacherNoAndLocale(string teacherNo, bool isAll, string schoolClassId, bool isSeach, string studentNoOrName, bool useSchoolClassId, string locale)
        {
            DataSet ds = new DataSet();

            List<string> listSchoolClass = new List<string>();


            if (teacherNo == "admin")
            {
                ds = this.GetAllSchoolClass();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    listSchoolClass.Add("'" + ds.Tables[0].Rows[i]["SchoolClassID"].ToString().Trim() + "'");
                }
            }
            else
            {
                if (!useSchoolClassId)
                {
                    ds = this.GetSchoolClassByTeacherNo(teacherNo);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        listSchoolClass.Add("'" + ds.Tables[0].Rows[i]["SchoolClassID"].ToString().Trim() + "'");
                    }
                }
                else
                {
                    listSchoolClass.Add(schoolClassId);
                }
            }

            if (isSeach)
            {
                string commandString = "select [studentName],[englishExamId],[deadLineTime],[examCertificateState],[examCertificateRemark],[isPaid],[isPaidRemark],A.[studentNo],[cardType],C.[studentName],[cardNum],[sex],[SchoolClassName],[studentSpeciality],[examPlace],[examType],[grade],[gradeCertificateState],[gradeCertificateRemark],englishExamNotifyTitle,D.[englishExamNotifyId],[englishExamSignUpConfirm],A.[updateTime] FROM usta_EnglishExam A,usta_StudentClass B,usta_StudentsList C,usta_EnglishExamNotify D WHERE  D.englishExamNotifyId=A.englishExamNotifyId AND A.studentNo = C.studentNo AND B.SchoolClassID=C.SchoolClass AND  B.locale=D.locale AND D.locale=@locale AND (C.studentNo like @keyword or C.studentName like @keyword) AND C.SchoolClass in(" + (isAll ? String.Join(",", listSchoolClass.ToArray()) : "'" + schoolClassId + "'") + ") ORDER BY SchoolClassName;";
                SqlParameter[] parameters = new SqlParameter[]{new SqlParameter("@keyword","%"+studentNoOrName+"%"),new SqlParameter("@locale",locale)
                    };
                ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
            }
            else
            {
                if (listSchoolClass.Count > 0)
                {
                    string commandString = "select [studentName],[englishExamId],[deadLineTime],[examCertificateState],[examCertificateRemark],[isPaid],[isPaidRemark],A.[studentNo],[examPlace],[examType],[cardType],C.[studentName],[cardNum],[sex],[SchoolClassName],[studentSpeciality],[grade],[gradeCertificateState],[gradeCertificateRemark],englishExamNotifyTitle,D.[englishExamNotifyId],[englishExamSignUpConfirm],A.[updateTime] FROM usta_EnglishExam A,usta_StudentClass B,usta_StudentsList C,usta_EnglishExamNotify D WHERE  D.englishExamNotifyId=A.englishExamNotifyId AND A.studentNo = C.studentNo AND B.SchoolClassID=C.SchoolClass  AND  B.locale=D.locale AND D.locale=@locale AND C.SchoolClass in(" + (isAll ? String.Join(",", listSchoolClass.ToArray()) : "'" + schoolClassId + "'") + ") ORDER BY SchoolClassName;";
                    SqlParameter[] parameters = new SqlParameter[]{new SqlParameter("@locale",locale)
                    };
                    ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
                }
            }
            return ds;
        }

        #endregion
       
        #region 批量更新缴费、准考证、成绩单状态

        /// <summary>
        /// 批量更新缴费、准考证、成绩单状态
        /// </summary>
        /// <returns></returns>
        public int BatchUpdateSignUpInfoState(string key,string value,int englishExamNotifyId,string schoolClassId)
        {
            switch (key)
            {
                case "isPaid":
                    string commandString = "UPDATE usta_EnglishExam set isPaid=@isPaid WHERE englishExamId in(select [englishExamId] from [usta_EnglishExam] A,[usta_StudentsList] B WHERE A.studentNo = B.studentNo AND B.SchoolClass=@schoolClass AND A.englishExamNotifyId=@englishExamNotifyId);";
                    SqlParameter[] parameters = new SqlParameter[]{new SqlParameter("@schoolClass",schoolClassId),
                        new SqlParameter("@englishExamNotifyId",englishExamNotifyId),new SqlParameter("@isPaid",int.Parse(value))
            };
                   return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
                case "examCertificateState":
                   string commandString1 = "UPDATE usta_EnglishExam set examCertificateState=@examCertificateState WHERE englishExamId in(select [englishExamId] from [usta_EnglishExam] A,[usta_StudentsList] B WHERE A.studentNo = B.studentNo AND B.SchoolClass=@schoolClass AND A.englishExamNotifyId=@englishExamNotifyId);";
                    SqlParameter[] parameters1 = new SqlParameter[]{new SqlParameter("@schoolClass",schoolClassId),
                        new SqlParameter("@englishExamNotifyId",englishExamNotifyId),new SqlParameter("@examCertificateState",value)
            };
                    return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString1, parameters1);
                case "gradeCertificateState":
                    string commandString2 = "UPDATE usta_EnglishExam set gradeCertificateState=@gradeCertificateState  WHERE englishExamId in(select [englishExamId] from [usta_EnglishExam] A,[usta_StudentsList] B WHERE A.studentNo = B.studentNo AND B.SchoolClass=@schoolClass AND A.englishExamNotifyId=@englishExamNotifyId);";
                    SqlParameter[] parameters2 = new SqlParameter[]{new SqlParameter("@schoolClass",schoolClassId),
                        new SqlParameter("@englishExamNotifyId",englishExamNotifyId),new SqlParameter("@gradeCertificateState",value)
            };
                    return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString2, parameters2);
                default:
                    break;
            }
            return 0;
        }

        #endregion

        #region 确认或取消报名

       /// <summary>
        /// 确认或取消报名
       /// </summary>
       /// <param name="englishExamId"></param>
       /// <param name="studentNo"></param>
       /// <param name="isCancelConfirm"></param>
       /// <returns></returns>
        public int ConfirmOrCancelSignUpInfo(string englishExamId,string studentNo,string examType,string examPlace, bool isCancelConfirm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_EnglishExam set ");
            strSql.Append("englishExamSignUpConfirm=@englishExamSignUpConfirm,");
            strSql.Append("examType=@examType,");
            strSql.Append("examPlace=@examPlace");
            //strSql.Append("englishExamSignUpConfirmTime=@englishExamSignUpConfirmTime");
            strSql.Append(" where englishExamId=@englishExamId");
            strSql.Append(" AND studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@englishExamSignUpConfirm", SqlDbType.TinyInt,1),
					new SqlParameter("@examType", SqlDbType.NChar,10),
					new SqlParameter("@examPlace", SqlDbType.NChar,10),
					//new SqlParameter("@englishExamSignUpConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@englishExamId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10)};
            parameters[0].Value = isCancelConfirm ? 0 : 1;
            parameters[1].Value = examType;
            parameters[2].Value = examPlace;
            //parameters[3].Value = DateTime.Now.ToString();
            parameters[3].Value = englishExamId;
            parameters[4].Value = studentNo;


            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion

        #endregion

        #region 获取全部四六级通知数据
        /// <summary>
        /// 获取全部四六级通知数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllEnglishExamNotify()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select englishExamNotifyId,englishExamNotifyTitle,englishExamNotifyContent,deadLineTime,attachmentIds,updateTime,hits,locale ");
            strSql.Append(" FROM usta_EnglishExamNotify ORDER BY deadLineTime DESC;");
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
        #endregion


        #region 根据培养地获取四六级通知数据
        /// <summary>
        /// 根据培养地获取四六级通知数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetEnglishExamNotifyByLocale(string locale)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select englishExamNotifyId,englishExamNotifyTitle,englishExamNotifyContent,deadLineTime,attachmentIds,updateTime,hits,locale ");
            strSql.Append(" FROM usta_EnglishExamNotify WHERE locale=@locale ORDER BY deadLineTime DESC;");
            SqlParameter[] parameters = {
					new SqlParameter("@locale", SqlDbType.NVarChar,20)
};
            parameters[0].Value = locale;
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 根据学号获取培养地
        /// <summary>
        /// 根据学号获取培养地
        /// </summary>
        /// <returns></returns>
        public DataSet GetLocaleByStudentNo(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 locale");
            strSql.Append(" FROM [usta_StudentClass] A,[usta_StudentsList] B");
            strSql.Append(" WHERE A.SchoolClassID=B.SchoolClass AND studentNo=@studentNo;");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
};
            parameters[0].Value = studentNo;
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据培养地获取目前正在进行的四六级通知数据Ing
        /// <summary>
        /// 根据培养地获取目前正在进行的四六级通知数据Ing
        /// </summary>
        /// <returns></returns>
        public DataSet GetEnglishExamNotifyIng(string locale)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 englishExamNotifyId,englishExamNotifyTitle,englishExamNotifyContent,deadLineTime,attachmentIds,updateTime,hits,locale");
            strSql.Append(" FROM usta_EnglishExamNotify");
            strSql.Append(" WHERE deadLineTime>getdate() AND local=@locale;");
            SqlParameter[] parameters = {
					new SqlParameter("@locale", SqlDbType.NVarChar,20)
};
            parameters[0].Value = locale;
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(),parameters);
        }
        #endregion

        #region 根据Id获取目前正在进行的四六级通知数据Ing
        /// <summary>
        /// 根据Id获取目前正在进行的四六级通知数据Ing
        /// </summary>
        /// <returns></returns>
        public DataSet GetEnglishExamNotifyIngById(int englishExamNotifyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select englishExamNotifyId,englishExamNotifyTitle,englishExamNotifyContent,deadLineTime,attachmentIds,updateTime,hits,locale");
            strSql.Append(" FROM usta_EnglishExamNotify");
            strSql.Append(" WHERE englishExamNotifyId=@englishExamNotifyId;");
            SqlParameter[] parameters = {
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)
};
            parameters[0].Value = englishExamNotifyId;
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 根据学号和Id获取目前正在进行的四六级通知数据Ing
        /// <summary>
        /// 根据学号和Id获取目前正在进行的四六级通知数据Ing
        /// </summary>
        /// <returns></returns>
        public DataSet GetEnglishExamNotifyIngByStudentNoAndId(int englishExamNotifyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select englishExamNotifyId,englishExamNotifyTitle,englishExamNotifyContent,deadLineTime,attachmentIds,updateTime,hits,locale");
            strSql.Append(" FROM usta_EnglishExamNotify A,usta_EnglishExam B");
            strSql.Append(" WHERE A.englishExamNotifyId=@englishExamNotifyId AND A.englishExamNotifyId=B.englishExamNotifyId AND B.studentNo=@studentNo;");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)
};
            parameters[0].Value = englishExamNotifyId;
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion


        #region 获取指定Id的四六级通知数据
        /// <summary>
        /// 获取指定Id的四六级通知数据
        /// </summary>
        /// <returns></returns>
        public EnglishExamNotify GetEnglishExamNotifyById(int englishExamNotifyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 englishExamNotifyId,englishExamNotifyTitle,englishExamNotifyContent,deadLineTime,attachmentIds,updateTime,locale from usta_EnglishExamNotify ");
            strSql.Append(" where englishExamNotifyId=@englishExamNotifyId");
            SqlParameter[] parameters = {
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)
};
            parameters[0].Value = englishExamNotifyId;

            EnglishExamNotify model = new EnglishExamNotify();
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["englishExamNotifyId"] != null && ds.Tables[0].Rows[0]["englishExamNotifyId"].ToString().Trim() != "")
                {
                    model.englishExamNotifyId = int.Parse(ds.Tables[0].Rows[0]["englishExamNotifyId"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["englishExamNotifyTitle"] != null && ds.Tables[0].Rows[0]["englishExamNotifyTitle"].ToString().Trim() != "")
                {
                    model.englishExamNotifyTitle = ds.Tables[0].Rows[0]["englishExamNotifyTitle"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["englishExamNotifyContent"] != null && ds.Tables[0].Rows[0]["englishExamNotifyContent"].ToString().Trim() != "")
                {
                    model.englishExamNotifyContent = ds.Tables[0].Rows[0]["englishExamNotifyContent"].ToString().Trim();
                }
                model.attachmentIds = ds.Tables[0].Rows[0]["attachmentIds"].ToString();
                if (ds.Tables[0].Rows[0]["updateTime"] != null && ds.Tables[0].Rows[0]["updateTime"].ToString().Trim() != "")
                {
                    model.updateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["updateTime"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["deadLineTime"] != null && ds.Tables[0].Rows[0]["deadLineTime"].ToString().Trim() != "")
                {
                    model.deadLineTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["deadLineTime"].ToString().Trim());
                }

                model.locale = string.Empty;

                if (ds.Tables[0].Rows[0]["locale"] != null && ds.Tables[0].Rows[0]["locale"].ToString().Trim() != "")
                {
                    model.locale = ds.Tables[0].Rows[0]["locale"].ToString().Trim();
                }
                return model;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 发布四六级通知
        /// <summary>
        /// 发布四六级通知
        /// </summary>
        /// <returns></returns>
        public int AddEnglishExamNotify(EnglishExamNotify englishExamNotify)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_EnglishExamNotify(");
            strSql.Append("englishExamNotifyTitle,englishExamNotifyContent,attachmentIds,deadLineTime,locale)");
            strSql.Append(" values (");
            strSql.Append("@englishExamNotifyTitle,@englishExamNotifyContent,@attachmentIds,@deadLineTime,@locale);");
            SqlParameter[] parameters = {
					new SqlParameter("@englishExamNotifyTitle", SqlDbType.NChar,50),
					new SqlParameter("@englishExamNotifyContent", SqlDbType.NText),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@deadLineTime", SqlDbType.DateTime),
					new SqlParameter("@locale", SqlDbType.NVarChar,20)};
            parameters[0].Value = englishExamNotify.englishExamNotifyTitle;
            parameters[1].Value = englishExamNotify.englishExamNotifyContent;
            parameters[2].Value = englishExamNotify.attachmentIds;
            parameters[3].Value = englishExamNotify.deadLineTime;
            parameters[4].Value = englishExamNotify.locale;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据Id修改四六级通知
        /// <summary>
        /// 根据Id修改四六级通知
        /// </summary>
        /// <returns></returns>
        public int UpdateEnglishExamNotifyById(EnglishExamNotify englishExamNotify)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_EnglishExamNotify set ");
            strSql.Append("englishExamNotifyTitle=@englishExamNotifyTitle,");
            strSql.Append("englishExamNotifyContent=@englishExamNotifyContent,");
            strSql.Append("attachmentIds=@attachmentIds,");
            strSql.Append("deadLineTime=@deadLineTime,");
            strSql.Append("locale=@locale");
            strSql.Append(" where englishExamNotifyId=@englishExamNotifyId");
            SqlParameter[] parameters = {
					new SqlParameter("@englishExamNotifyTitle", SqlDbType.NChar,50),
					new SqlParameter("@englishExamNotifyContent", SqlDbType.NText),
					new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@deadLineTime", SqlDbType.DateTime),
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4),
					new SqlParameter("@locale", SqlDbType.NVarChar,20)};
            parameters[0].Value = englishExamNotify.englishExamNotifyTitle;
            parameters[1].Value = englishExamNotify.englishExamNotifyContent;
            parameters[2].Value = englishExamNotify.attachmentIds;
            parameters[3].Value = englishExamNotify.updateTime;
            parameters[4].Value = englishExamNotify.deadLineTime;
            parameters[5].Value = englishExamNotify.englishExamNotifyId;
            parameters[6].Value = englishExamNotify.locale;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 根据Id修改四六级通知浏览次数
        /// <summary>
        /// 根据Id修改四六级通知浏览次数
        /// </summary>
        /// <returns></returns>
        public int UpdateEnglishExamNotifyHitsById(int englishExamNotifyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_EnglishExamNotify set ");
            strSql.Append("hits=hits+1");
            strSql.Append(" where englishExamNotifyId=@englishExamNotifyId");
            SqlParameter[] parameters = {
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)};
            parameters[0].Value = englishExamNotifyId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 删除指定Id的四六级通知信息
        /// <summary>
        /// 删除指定Id的四六级通知信息
        /// </summary>
        /// <returns></returns>
        public int DeleteEnglishExamNotifyById(int englishExamNotifyId)
        {
            StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from usta_EnglishExamNotify ");
			strSql.Append(" where englishExamNotifyId=@englishExamNotifyId");
			SqlParameter[] parameters = {
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)
};
			parameters[0].Value = englishExamNotifyId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }

        #endregion

        #region 学生四六级报名相关操作

        #region 增加一条四六级报名信息

        /// <summary>
        /// 增加一条四六级报名信息
        /// </summary>
        /// <returns></returns>
        public int AddEnglishExamSignUp(string studentNo,string examPlace,string examType,int englishExamNotifyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into usta_EnglishExam(");
            strSql.Append("studentNo,examPlace,examType,englishExamNotifyId)");
            strSql.Append(" values (");
            strSql.Append("@studentNo,@examPlace,@examType,@englishExamNotifyId)");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@examPlace", SqlDbType.NChar,10),
					new SqlParameter("@examType", SqlDbType.NChar,10),
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)};
            parameters[0].Value = studentNo;
            parameters[1].Value = examPlace;
            parameters[2].Value = examType;
            parameters[3].Value = englishExamNotifyId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters); ;
        }

        #endregion

       
        /// <summary>
        /// 根据Id修改四六级报名信息
        /// </summary>
        /// <returns></returns>
        public int UpdateEnglishExamSignUp(string studentNo,string examPlace,string examType,int englishExamId)
        {
            StringBuilder strSql=new StringBuilder();
			strSql.Append("update usta_EnglishExam set ");
			strSql.Append("examPlace=@examPlace,");
			strSql.Append("examType=@examType");
			strSql.Append(" where studentNo=@studentNo AND englishExamId=@englishExamId");
			SqlParameter[] parameters = {
					new SqlParameter("@examPlace", SqlDbType.NChar,10),
					new SqlParameter("@examType", SqlDbType.NChar,10),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@englishExamId", SqlDbType.Int,4)};
			parameters[0].Value = examPlace;
			parameters[1].Value = examType;
			parameters[2].Value = studentNo;
			parameters[3].Value = englishExamId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters);
        }



        
       /// <summary>
        /// 根据Id修改四六级报名相关状态信息
       /// </summary>
       /// <param name="studentNo"></param>
       /// <param name="examPlace"></param>
       /// <param name="examType"></param>
       /// <param name="englishExamId"></param>
       /// <returns></returns>
        public int UpdateEnglishExamSignUpState(EnglishExam model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update usta_EnglishExam set ");
            strSql.Append("examCertificateState=@examCertificateState,");
            strSql.Append("examCertificateRemark=@examCertificateRemark,");
            strSql.Append("isPaid=@isPaid,");
            strSql.Append("isPaidRemark=@isPaidRemark,");
            strSql.Append("grade=@grade,");
            strSql.Append("gradeCertificateState=@gradeCertificateState,");
            strSql.Append("gradeCertificateRemark=@gradeCertificateRemark ");
            strSql.Append(" where englishExamId=@englishExamId");
            strSql.Append(" AND studentNo=@studentNo");
            strSql.Append(" AND englishExamNotifyId=@englishExamNotifyId;");
            SqlParameter[] parameters = {
					new SqlParameter("@examCertificateState", SqlDbType.NChar,10),
					new SqlParameter("@examCertificateRemark", SqlDbType.NChar,100),
					new SqlParameter("@isPaid", SqlDbType.TinyInt,1),
					new SqlParameter("@isPaidRemark", SqlDbType.NVarChar,100),
					new SqlParameter("@grade", SqlDbType.NChar,10),
					new SqlParameter("@gradeCertificateState", SqlDbType.NChar,10),
					new SqlParameter("@gradeCertificateRemark", SqlDbType.NVarChar,100),
					new SqlParameter("@englishExamId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)};
            parameters[0].Value = model.examCertificateState;
            parameters[1].Value = model.examCertificateRemark;
            parameters[2].Value = model.isPaid;
            parameters[3].Value = model.isPaidRemark;
            parameters[4].Value = model.grade;
            parameters[5].Value = model.gradeCertificateState;
            parameters[6].Value = model.gradeCertificateRemark;
            parameters[7].Value = model.englishExamId;
            parameters[8].Value = model.studentNo;
            parameters[9].Value = model.englishExamNotifyId;

            return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSql.ToString(), parameters); ;
        }



        #region 判断当前是否已经报名

        /// <summary>
        /// 判断当前是否已经报名
        /// </summary>
        /// <returns></returns>
        public int CheckHasSignUpInfo(string studentNo,int englishExamNotifyId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select englishExamNotifyId from usta_EnglishExam WHERE studentNo=@studentNo");
            strSql.Append(" AND englishExamNotifyId=@englishExamNotifyId");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@englishExamNotifyId", SqlDbType.Int,4)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = englishExamNotifyId;

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

            return ds.Tables[0].Rows.Count;
        }

        #endregion



        #region 根据学号查找所有相关的四六级报名信息

        /// <summary>
        /// 根据学号查找所有相关的四六级报名信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllEnglishExamSignUpInfoByStudentNo(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select A.englishExamId,A.updateTime,examCertificateState,examCertificateRemark,isPaid,studentNo,examPlace,examType,grade,gradeCertificateState,gradeCertificateRemark,A.englishExamNotifyId,englishExamSignUpConfirm,englishExamNotifyTitle from usta_EnglishExam A,usta_EnglishExamNotify B WHERE A.englishExamNotifyId=B.englishExamNotifyId ");
            strSql.Append(" AND studentNo=@studentNo");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10)
};
            parameters[0].Value = studentNo;

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);
            return ds;
        }

        #endregion


        #region 获取报名学生所需的相关信息

        /// <summary>
        /// 获取报名学生所需的相关信息
        /// </summary>
        /// <returns></returns>
        public StudentsList GetEnglishExamSignUpStudentInfoByStudentNo(string studentNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 studentNo,studentName,studentUserPwd,studentSpeciality,mobileNo,emailAddress,remark,classNo,StudentID,StudentUSID,MajorType,SchoolClass,SchoolClassName,isAdmin,Sex,CardNum,CardType,MatriculationDate from usta_StudentsList ");
            strSql.Append(" where studentNo=@studentNo ");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10)};
            parameters[0].Value = studentNo;

            StudentsList model = new StudentsList();
            DataSet ds = SqlHelper.ExecuteDataset(conn
            ,CommandType.Text,strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["studentNo"] != null && ds.Tables[0].Rows[0]["studentNo"].ToString() != "")
                {
                    model.studentNo = ds.Tables[0].Rows[0]["studentNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["studentName"] != null && ds.Tables[0].Rows[0]["studentName"].ToString() != "")
                {
                    model.studentName = ds.Tables[0].Rows[0]["studentName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["studentUserPwd"] != null && ds.Tables[0].Rows[0]["studentUserPwd"].ToString() != "")
                {
                    model.studentUserPwd = ds.Tables[0].Rows[0]["studentUserPwd"].ToString();
                }
                if (ds.Tables[0].Rows[0]["studentSpeciality"] != null && ds.Tables[0].Rows[0]["studentSpeciality"].ToString() != "")
                {
                    model.studentSpeciality = ds.Tables[0].Rows[0]["studentSpeciality"].ToString();
                }
                if (ds.Tables[0].Rows[0]["mobileNo"] != null && ds.Tables[0].Rows[0]["mobileNo"].ToString() != "")
                {
                    model.mobileNo = ds.Tables[0].Rows[0]["mobileNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["emailAddress"] != null && ds.Tables[0].Rows[0]["emailAddress"].ToString() != "")
                {
                    model.emailAddress = ds.Tables[0].Rows[0]["emailAddress"].ToString();
                }
                if (ds.Tables[0].Rows[0]["remark"] != null && ds.Tables[0].Rows[0]["remark"].ToString() != "")
                {
                    model.remark = ds.Tables[0].Rows[0]["remark"].ToString();
                }
                if (ds.Tables[0].Rows[0]["classNo"] != null && ds.Tables[0].Rows[0]["classNo"].ToString() != "")
                {
                    model.classNo = ds.Tables[0].Rows[0]["classNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["StudentID"] != null && ds.Tables[0].Rows[0]["StudentID"].ToString() != "")
                {
                    model.StudentID = ds.Tables[0].Rows[0]["StudentID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["MajorType"] != null && ds.Tables[0].Rows[0]["MajorType"].ToString() != "")
                {
                    model.MajorType = ds.Tables[0].Rows[0]["MajorType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SchoolClass"] != null && ds.Tables[0].Rows[0]["SchoolClass"].ToString() != "")
                {
                    model.SchoolClass = ds.Tables[0].Rows[0]["SchoolClass"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SchoolClassName"] != null && ds.Tables[0].Rows[0]["SchoolClassName"].ToString() != "")
                {
                    model.SchoolClassName = ds.Tables[0].Rows[0]["SchoolClassName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["isAdmin"] != null && ds.Tables[0].Rows[0]["isAdmin"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["isAdmin"].ToString() == "1") || (ds.Tables[0].Rows[0]["isAdmin"].ToString().ToLower() == "true"))
                    {
                        model.isAdmin = true;
                    }
                    else
                    {
                        model.isAdmin = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Sex"] != null && ds.Tables[0].Rows[0]["Sex"].ToString() != "")
                {
                    model.Sex = int.Parse(ds.Tables[0].Rows[0]["Sex"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CardNum"] != null && ds.Tables[0].Rows[0]["CardNum"].ToString() != "")
                {
                    model.CardNum = ds.Tables[0].Rows[0]["CardNum"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CardType"] != null && ds.Tables[0].Rows[0]["CardType"].ToString() != "")
                {
                    model.CardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["MatriculationDate"] != null && ds.Tables[0].Rows[0]["MatriculationDate"].ToString() != "")
                {
                    model.MatriculationDate = DateTime.Parse(ds.Tables[0].Rows[0]["MatriculationDate"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 根据学号和报名Id判断是否已经过了报名截止日期或者已经确认了报名
        /// <summary>
       /// 根据学号和报名Id判断是否已经过了报名截止日期或者已经确认了报名
       /// </summary>
       /// <param name="studentNo"></param>
       /// <param name="englishExamId"></param>
       /// <returns></returns>
        public bool CheckIsOverDateOrSignUpConfirm(string studentNo, int englishExamId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 B.englishExamNotifyId from usta_EnglishExam A,usta_EnglishExamNotify B where A.englishExamNotifyId=B.englishExamNotifyId AND A.englishExamId=@englishExamId");
            strSql.Append(" AND studentNo=@studentNo AND deadLineTime>getdate();");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@englishExamId", SqlDbType.Int,4)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = englishExamId;

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);


            if (ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }

            strSql = new StringBuilder();
            strSql.Append("select top 1 englishExamNotifyId from usta_EnglishExam WHERE englishExamId=@englishExamId");
            strSql.Append(" AND studentNo=@studentNo AND englishExamSignUpConfirm>0;");
            parameters = new SqlParameter[]{
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@englishExamId", SqlDbType.Int,4)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = englishExamId;

            ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region 根据学号和englishExamId查找相关的四六级报名信息
        /// <summary>
        /// 根据学号和englishExamId查找相关的四六级报名信息
        /// </summary>
        /// <returns></returns>
        public EnglishExam GetEnglishExamSignUpInfoByStudentNo(string studentNo,int englishExamId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 englishExamId,examCertificateState,examCertificateRemark,isPaid,studentNo,examPlace,examType,grade,gradeCertificateState,gradeCertificateRemark,englishExamNotifyId,englishExamSignUpConfirm from usta_EnglishExam ");
            strSql.Append(" where studentNo=@studentNo AND englishExamId=@englishExamId");
            SqlParameter[] parameters = {
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@englishExamId", SqlDbType.Int,4)
};
            parameters[0].Value = studentNo;
            parameters[1].Value = englishExamId;

            EnglishExam model = new EnglishExam();
            DataSet ds = SqlHelper.ExecuteDataset(conn,CommandType.Text,strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["englishExamId"] != null && ds.Tables[0].Rows[0]["englishExamId"].ToString().Trim() != "")
                {
                    model.englishExamId = int.Parse(ds.Tables[0].Rows[0]["englishExamId"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["examCertificateState"] != null && ds.Tables[0].Rows[0]["examCertificateState"].ToString().Trim() != "")
                {
                    model.examCertificateState = ds.Tables[0].Rows[0]["examCertificateState"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["examCertificateRemark"] != null && ds.Tables[0].Rows[0]["examCertificateRemark"].ToString().Trim() != "")
                {
                    model.examCertificateRemark = ds.Tables[0].Rows[0]["examCertificateRemark"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["isPaid"] != null && ds.Tables[0].Rows[0]["isPaid"].ToString().Trim() != "")
                {
                    model.isPaid = int.Parse(ds.Tables[0].Rows[0]["isPaid"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["studentNo"] != null && ds.Tables[0].Rows[0]["studentNo"].ToString().Trim() != "")
                {
                    model.studentNo = ds.Tables[0].Rows[0]["studentNo"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["examPlace"] != null && ds.Tables[0].Rows[0]["examPlace"].ToString().Trim() != "")
                {
                    model.examPlace = ds.Tables[0].Rows[0]["examPlace"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["examType"] != null && ds.Tables[0].Rows[0]["examType"].ToString().Trim() != "")
                {
                    model.examType = ds.Tables[0].Rows[0]["examType"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["grade"] != null && ds.Tables[0].Rows[0]["grade"].ToString().Trim() != "")
                {
                    model.grade = ds.Tables[0].Rows[0]["grade"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["gradeCertificateState"] != null && ds.Tables[0].Rows[0]["gradeCertificateState"].ToString().Trim() != "")
                {
                    model.gradeCertificateState = ds.Tables[0].Rows[0]["gradeCertificateState"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["gradeCertificateRemark"] != null && ds.Tables[0].Rows[0]["gradeCertificateRemark"].ToString().Trim() != "")
                {
                    model.gradeCertificateRemark = ds.Tables[0].Rows[0]["gradeCertificateRemark"].ToString().Trim();
                }
                if (ds.Tables[0].Rows[0]["englishExamNotifyId"] != null && ds.Tables[0].Rows[0]["englishExamNotifyId"].ToString().Trim() != "")
                {
                    model.englishExamNotifyId = int.Parse(ds.Tables[0].Rows[0]["englishExamNotifyId"].ToString().Trim());
                }
                if (ds.Tables[0].Rows[0]["englishExamSignUpConfirm"] != null && ds.Tables[0].Rows[0]["englishExamSignUpConfirm"].ToString().Trim() != "")
                {
                    model.englishExamSignUpConfirm = int.Parse(ds.Tables[0].Rows[0]["englishExamSignUpConfirm"].ToString().Trim());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #endregion

        #region 获取三届以内的全部学生信息
        /// <summary>
        /// 获取三届以内的全部学生信息
        /// </summary>
        public DataSet GetThreeGradeStudentsInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [studentNo],[studentName],[studentUserPwd],[studentSpeciality],[mobileNo],[emailAddress],[remark],[classNo],[StudentID],[StudentUSID],[MajorType],[SchoolClass],[SchoolClassName],[isAdmin],[Sex],[CardNum],[CardType],[MatriculationDate] from [usta_StudentsList] WHERE SUBSTRING(RTRIM(studentNo),3,2)");
            strSql.Append("IN(SELECT TOP 3 SUBSTRING(RTRIM(termTag),3,2) FROM dbo.usta_TermTags order by termTag desc);");
            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString());
        }
        #endregion

    }
}