using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace USTA.Dal
{
    using USTA.Model;
    using USTA.Common;
    using System.Text.RegularExpressions;
    /// <summary>
    /// 操作工作量的类
    /// </summary>
    public class DalOperationNorm
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
        public DalOperationNorm()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region
        /// <summary>
        /// 添加一项工作量统计项
        /// </summary>
        /// <param name="norm"></param>
        public void AddNorm(Norm norm)
        {
            try
            {
                string sql = "INSERT INTO [USTA].[dbo].[usta_Norm]([name],[parentId],[comment],[type],[year]) VALUES(@name,@parentId,@comment,@type,@year)";
                SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.NVarChar,50),
                    new SqlParameter("@parentId", SqlDbType.Int),
                    new SqlParameter("@comment", SqlDbType.NVarChar,1024),
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@year", SqlDbType.NVarChar,10)
                                            };
                parameters[0].Value = norm.name;
                parameters[1].Value = norm.parentId;
                parameters[2].Value = norm.comment;
                parameters[3].Value = norm.type;
                parameters[4].Value = norm.year;
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
        }


        public Boolean ExistNormName(string name,int parentId,string year)
        {
            string sql = "SELECT [normId],[name],[parentId],[comment],[year] FROM [USTA].[dbo].[usta_Norm] where  year = @year and name=@name and parentId= @parentId";
            SqlParameter[] parameters = {
					new SqlParameter("@parentId",parentId),
                    new SqlParameter("@name",name),
                    new SqlParameter("@year",year)};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);

            return ds.Tables[0].Rows.Count > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="norm"></param>
        public void EditNorm(Norm norm)
        {
            try
            {
                string sql = "UPDATE [USTA].[dbo].[usta_Norm] SET [name] = @name,[parentId] = @parentId ,[comment] =@comment,[type]=@type WHERE normId = @normId";
                SqlParameter[] parameters = {
                     new SqlParameter("@normId", SqlDbType.Int),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
                    new SqlParameter("@parentId", SqlDbType.Int),
                    new SqlParameter("@comment", SqlDbType.NVarChar,1024),
                     new SqlParameter("@type", SqlDbType.Int),
                     new SqlParameter("@year", SqlDbType.NVarChar,10)
                                            };
                parameters[0].Value = norm.normId;
                parameters[1].Value = norm.name;
                parameters[2].Value = norm.parentId;
                parameters[3].Value = norm.comment;
                parameters[4].Value = norm.type;
                parameters[5].Value = norm.year;
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetFirstNorms(string year)
        {
            string sql = "SELECT [normId],[name],[parentId],[comment],[year] FROM [USTA].[dbo].[usta_Norm] where parentId = 0 and year = @year";
            SqlParameter[] parameters = {
					new SqlParameter("@year",year)};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql,parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normId"></param>
        /// <returns></returns>
        public DataSet GetChildNorms(int normId, string year)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@year",year)};
            string sql = "SELECT [normId],[name],[parentId],[comment],[year] FROM [USTA].[dbo].[usta_Norm] where parentId = @normId and year=@year";
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normId"></param>
        /// <returns></returns>
        public DataSet GetChildNorms(int normId, int type, string year)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@type",type), new SqlParameter("@year",year)                    };
            string sql = "SELECT [normId],[name],[parentId],[comment],[type],[year] FROM [USTA].[dbo].[usta_Norm] where parentId = @normId and type=@type and year=@year";
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Norm getNormById(int id)
        {
            Norm norm = null;
            SqlParameter[] parameters = {
					new SqlParameter("@normId",id)};
            string sql = "SELECT [normId],[name],[parentId],[comment],type,year FROM [USTA].[dbo].[usta_Norm] where normId = @normId";
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);

            if (dr.Read())
            {
                norm = new Norm();
                norm.normId = Convert.ToInt32( dr["normId"].ToString());
                norm.name = dr["name"].ToString();
                norm.comment = dr["comment"].ToString();
                norm.parentId = Convert.ToInt32(dr["parentId"].ToString());
                norm.type = Convert.ToInt32(dr["type"].ToString());
                norm.year = dr["year"].ToString();
            }
            dr.Close();
            conn.Close();
            return norm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void deleteNormById(int id)
        {
            try
            {
                string sql = "DELETE FROM [USTA].[dbo].[usta_Norm] WHERE normId = @normId";
                SqlParameter[] parameters = {
					new SqlParameter("@normId",id)};
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normId"></param>
        /// <returns></returns>
        private int GetNormType(int normId)
        {
           Norm norm = this.getNormById(normId);
           if (norm != null) return norm.type;
           else
               return 0;
        }
    
            /// <summary>
            /// 
            /// </summary>
            /// <param name="termTag"></param>
            /// <param name="searchKey"></param>
            /// <param name="teacherType"></param>
            /// <returns></returns>
        public DataSet getTeacherLoad(string termTag,string searchKey,string teacherType)
        {
            DataSet ds = new DataSet();
            DataTable datatable = new DataTable();
            DataSet normDs = GetFirstNorms(termTag);
            DataTable dt = normDs.Tables[0];
            int[] ids = new int[dt.Rows.Count];
            datatable.Columns.Add("教师");
            datatable.Columns.Add("硕士教学");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                datatable.Columns.Add(dt.Rows[i]["name"].ToString());
                ids[i] =  Convert.ToInt32( dt.Rows[i]["normId"]);
            }
            datatable.Columns.Add("合计");
            ds.Tables.Add(datatable);
            datatable.TableName = "工作汇总";
            DalOperationUsers daluser = new DalOperationUsers();

            DataTable teachers = daluser.SearchTeacher(searchKey, teacherType).Tables[0];
            DataTable normValues = GetNormValuesByTerm(termTag).Tables[0];
            for (int i = 0; i < teachers.Rows.Count; i++)
            {
                DataRow dr = datatable.NewRow();
                dr.SetField(datatable.Columns[0], teachers.Rows[i]["teacherName"].ToString().Trim());
               DataRow[] shuoshirows =  normValues.Select("normId=-1 and teacherNo='" + teachers.Rows[i]["teacherNo"].ToString().Trim() + "'");
               if (shuoshirows.Length == 0)
               {
                   dr.SetField(datatable.Columns[1], 0);
               }
               else
               {
                   dr.SetField(datatable.Columns[1], shuoshirows[0]["value"]);
               }
                for (int j = 2; j < datatable.Columns.Count - 1; j++)
                {
                    Object value = null;
                    DataRow[] rows = normValues.Select("normId=" + ids[j - 2] + " and teacherNo='" + teachers.Rows[i]["teacherNo"].ToString().Trim() + "'");
                    if (GetNormType(ids[j - 2]) == 0)
                    {
                        if (rows.Length > 0)
                            value = rows[0]["value"];
                        float factValue = 0;
                        if (value != null)
                        {
                            factValue = float.Parse(value.ToString());
                        }
                        dr.SetField(datatable.Columns[j], factValue);
                    }
                    else
                    {
                        if (rows.Length > 0)
                        dr.SetField(datatable.Columns[j], rows[0]["textValue"]);
                    }
                    
                }
                DataRow[] rootrow = normValues.Select("normId=" + 0 + " and teacherNo='" + teachers.Rows[i]["teacherNo"].ToString().Trim() + "'");

                object roots = null;
                if (rootrow.Length > 0)
                    roots = rootrow[0]["value"];
                float rootValue = 0;
                if (roots != null)
                {
                    rootValue = float.Parse(roots.ToString());
                }
                dr.SetField("合计", rootValue);
                datatable.Rows.Add(dr);
            }
            DataTable shuoshitable = new DataTable();
            shuoshitable.TableName = "硕士教学";
            shuoshitable.Columns.Add("教师");
            shuoshitable.Columns.Add("课程名称");
            shuoshitable.Columns.Add("学期");
            shuoshitable.Columns.Add("类型");
            shuoshitable.Columns.Add("理论课时");
            shuoshitable.Columns.Add("实验课时");
            DataTable shuoshinormChildValues = this.GetChildNorms(-1,termTag).Tables[0];

            for (int i = 0; i < shuoshinormChildValues.Rows.Count; i++)
            {
                shuoshitable.Columns.Add(shuoshinormChildValues.Rows[i]["name"].ToString());

            }
            for (int i = 0; i < teachers.Rows.Count; i++)
            {


                DataTable shuoshit = this.GetCourseStatistic(teachers.Rows[i]["teacherNo"].ToString().Trim(), termTag);
                for (int rownum = 0; rownum < shuoshit.Rows.Count; rownum++)
                {
                    DataRow dr = shuoshitable.NewRow();
                    dr.SetField(shuoshitable.Columns[0], teachers.Rows[i]["teacherName"].ToString().Trim());
                    for (int col = 3; col < shuoshit.Columns.Count; col++)
                    {
                        dr.SetField(shuoshit.Columns[col].ColumnName, shuoshit.Rows[rownum][shuoshit.Columns[col]].ToString().Trim());
                    }
                    shuoshitable.Rows.Add(dr);

                }




            }
            ds.Tables.Add(shuoshitable);




            for (int ci = 0; ci < dt.Rows.Count; ci++)
            {
                DataTable childTable = new DataTable();
                childTable.TableName = dt.Rows[ci]["name"].ToString();
                DataTable normChildValues = this.GetChildNorms(int.Parse(dt.Rows[ci]["normId"].ToString()),termTag).Tables[0];
                int[] dids = new int[normChildValues.Rows.Count];
                childTable.Columns.Add("教师");
                for (int i = 0; i < normChildValues.Rows.Count; i++)
                {
                    childTable.Columns.Add(normChildValues.Rows[i]["name"].ToString());
                    dids[i] = Convert.ToInt32(normChildValues.Rows[i]["normId"]);
                }
                for (int i = 0; i < teachers.Rows.Count; i++)
                {
                    DataRow dr = childTable.NewRow();
                    dr.SetField(childTable.Columns[0], teachers.Rows[i]["teacherName"].ToString().Trim());
                    for (int j = 1; j < childTable.Columns.Count; j++)
                    {
                        Object value = null;
                        DataRow[] rows = normValues.Select("normId=" + dids[j - 1] + " and teacherNo='" + teachers.Rows[i]["teacherNo"].ToString().Trim() + "'");
                        if (GetNormType(dids[j - 1]) == 0)
                        {
                            if (rows.Length > 0)
                                value = rows[0]["value"];
                            float factValue = 0;
                            if (value != null)
                            {
                                factValue = float.Parse(value.ToString());
                            }
                            dr.SetField(childTable.Columns[j], factValue);
                        }
                        else
                        {
                            if (rows.Length > 0)
                                dr.SetField(childTable.Columns[j], rows[0]["textValue"]);
                        }
                    }
                    childTable.Rows.Add(dr);
                }

                ds.Tables.Add(childTable);
            }
            conn.Close();
            return ds;
            
        }
        /// <summary>
        /// 获得所有教师
        /// </summary>
        /// <returns>教师数据集</returns>
        private DataSet GetTeachers()
        {
            string cmdstring = "SELECT [teacherNo],[teacherName]  FROM [USTA].[dbo].[usta_TeachersList]";

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring);
            return ds;
        }
        private DataSet GetNormValuesByTerm(string term)
        {
            string cmdstring = "SELECT [normValueId],[term],[teacherNo],[value],[normId],[textValue] FROM [USTA].[dbo].[usta_NormValue] where term = @term";
            SqlParameter[] parameters = {
					new SqlParameter("@term",term)};

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring,parameters);
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public DataSet GetNormValuesByTermOut(string term)
        {
            DataSet ds = GetNormValuesByTerm(term);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normId"></param>
        /// <param name="teacherNo"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public NormValue getNormValue(int normId, string teacherNo, string term)
        {
            NormValue result = null;
            SqlParameter[] parameters = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@teacherNo",teacherNo),
                    new SqlParameter("@term",term)};
            string cmdstring = "SELECT [normValueId],[term],[teacherNo],[value],[normId],textValue FROM [USTA].[dbo].[usta_NormValue] where term = @term and teacherNo=@teacherNo and normId =@normId";

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
            if (dr.Read())
            {
                result = new NormValue
                {
                    normId =Convert.ToInt32( dr["normId"].ToString()),
                    normValueId = Convert.ToInt32(dr["normValueId"].ToString()),
                    teacherNo = dr["teacherNo"].ToString().Trim(),
                    value = float.Parse( dr["value"].ToString()),
                    term = dr["term"].ToString().Trim(),
                    textValue = dr["textValue"].ToString().Trim()
                };
            }
            dr.Close();
            conn.Close();
            return result;
        }

        public NormValue getNormValue(int normId, string teacherNo, string term, string courseNo, string classId, string termTag,int atType)
        {
            NormValue result = null;
            SqlParameter[] parameters = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@teacherNo",teacherNo),
                    new SqlParameter("@term",term),
                    new SqlParameter("@courseNo",courseNo),
                    new SqlParameter("@classID",classId),
                    new SqlParameter("@termTag",termTag),
                     new SqlParameter("@atType",atType)                    };
            string cmdstring = "SELECT [normValueId],[term],[teacherNo],[value],[normId],textValue FROM [USTA].[dbo].[usta_NormValue] where term = @term and teacherNo=@teacherNo and normId =@normId and classId = @classID and courseNo=@courseNo and termTag =@termTag and atType=@atType";

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
            if (dr.Read())
            {
                result = new NormValue
                {
                    normId =Convert.ToInt32( dr["normId"].ToString()),
                    normValueId = Convert.ToInt32(dr["normValueId"].ToString()),
                    teacherNo = dr["teacherNo"].ToString().Trim(),
                    value = float.Parse( dr["value"].ToString()),
                    term = dr["term"].ToString().Trim(),
                    textValue = dr["textValue"].ToString().Trim()
                };
            }
            dr.Close();
            conn.Close();
            return result;
        }
        public void setNormValue(int normId, string teacherNo, string term, string value)
        {
            string normsql = "select type from usta_Norm where normId=@normId";
            SqlParameter[] parametersN = {
                    new SqlParameter("@normId",normId)
                                        };
            DataSet dsn = SqlHelper.ExecuteDataset(conn, CommandType.Text, normsql, parametersN);
            int type = 0;
            if (dsn.Tables[0].Rows.Count > 0)
            {
                type = int.Parse(dsn.Tables[0].Rows[0]["type"].ToString());
            }
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@teacherNo",teacherNo),
                    new SqlParameter("@term",term)};
                string cmdstring = "SELECT [normValueId],[term],[teacherNo],[value],[normId] FROM [USTA].[dbo].[usta_NormValue] where term = @term and teacherNo=@teacherNo and normId =@normId";

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string normValueId = ds.Tables[0].Rows[0]["normValueId"].ToString();
                    SqlParameter[] parameters1 = {
					new SqlParameter("@normValueId",normValueId),
                    new SqlParameter("@value",value)};
                    string upstring = "";
                    if (type == 0)
                    {
                        upstring = "UPDATE [USTA].[dbo].[usta_NormValue] SET [value] = @value WHERE normValueId=@normValueId";
                    }
                    else
                    {
                        upstring = "UPDATE [USTA].[dbo].[usta_NormValue] SET [textValue] = @value WHERE normValueId=@normValueId";

                    }
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, upstring, parameters1);
                }
                else
                {
                    SqlParameter[] parameters2 = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@teacherNo",teacherNo),
                    new SqlParameter("@term",term),
                    new SqlParameter("@value",value)};
                    string insertString = "";
                    if (type == 0)
                    {
                        insertString = "INSERT INTO [USTA].[dbo].[usta_NormValue]([term],[teacherNo],[value],[normId]) VALUES(@term,@teacherNo,@value,@normId)";
                    }
                    else
                    {
                        insertString = "INSERT INTO [USTA].[dbo].[usta_NormValue]([term],[teacherNo],[value],[normId],[textValue]) VALUES(@term,@teacherNo,0,@normId,@value)";

                    }
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, insertString, parameters2);
                }
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normId"></param>
        /// <param name="teacherNo"></param>
        /// <param name="term"></param>
        /// <param name="value"></param>
        /// <param name="courseNo"></param>
        /// <param name="classId"></param>
        /// <param name="termTag"></param>
        public void setNormValue(int normId, string teacherNo, string term, string value,string courseNo,string classId,string termTag,int atType)
        {
            string normsql = "select type from usta_Norm where normId=@normId";
            SqlParameter[] parametersN = {
                    new SqlParameter("@normId",normId)
                                        };
            DataSet dsn = SqlHelper.ExecuteDataset(conn, CommandType.Text, normsql, parametersN);
            int type = 0;
            if (dsn.Tables[0].Rows.Count > 0)
            {
                type = int.Parse(dsn.Tables[0].Rows[0]["type"].ToString());
            }
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@teacherNo",teacherNo),
                    new SqlParameter("@term",term),
                    new SqlParameter("@courseNo",courseNo),
                    new SqlParameter("@classID",classId),
                    new SqlParameter("@termTag",termTag),
                    new SqlParameter("@atType",atType)};
                string cmdstring = "SELECT [normValueId],[term],[teacherNo],[value],[normId] FROM [USTA].[dbo].[usta_NormValue] where term = @term and teacherNo=@teacherNo and normId =@normId and classId = @classID and courseNo=@courseNo and termTag =@termTag and atType=@atType";

                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string normValueId = ds.Tables[0].Rows[0]["normValueId"].ToString();
                    SqlParameter[] parameters1 = {
					new SqlParameter("@normValueId",normValueId),
                    new SqlParameter("@value",value)};
                    string upstring = "";
                    if (type == 0)
                    {
                        upstring = "UPDATE [USTA].[dbo].[usta_NormValue] SET [value] = @value WHERE normValueId=@normValueId";
                    }
                    else
                    {
                        upstring = "UPDATE [USTA].[dbo].[usta_NormValue] SET [textValue] = @value WHERE normValueId=@normValueId";

                    }
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, upstring, parameters1);
                }
                else
                {
                    SqlParameter[] parameters2 = {
					new SqlParameter("@normId",normId),
                    new SqlParameter("@teacherNo",teacherNo),
                    new SqlParameter("@term",term),
                    new SqlParameter("@value",value),
                    new SqlParameter("@courseNo",courseNo),
                    new SqlParameter("@classID",classId),
                    new SqlParameter("@termTag",termTag),
                    new SqlParameter("@atType",atType)};
                    string insertString = "";
                    if (type == 0)
                    {
                        insertString = "INSERT INTO [USTA].[dbo].[usta_NormValue]([term],[teacherNo],[value],[normId],[courseNo],[classID],[termTag],[atType]) VALUES(@term,@teacherNo,@value,@normId,@courseNo,@classID,@termTag,@atType)";
                    }
                    else
                    {
                        insertString = "INSERT INTO [USTA].[dbo].[usta_NormValue]([term],[teacherNo],[value],[normId],[textValue],[courseNo],[classID],[termTag],[atType]) VALUES(@term,@teacherNo,0,@normId,@value,@courseNo,@classID,@termTag,@atType)";

                    }
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, insertString, parameters2);
                }
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        private void AddFomula(NormFormula formula)
        {
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@targetNormId",formula.targetNormId),
                    new SqlParameter("@formula",formula.formula),
                    new SqlParameter("@formulaShow",formula.formulaShow),
                    new SqlParameter("@termYear",formula.termYear)};
                string insertString = "INSERT INTO [USTA].[dbo].[usta_NormFormula]([targetNormId],[formula],[formulaShow],[termYear])VALUES(@targetNormId,@formula,@formulaShow,@termYear)";
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, insertString, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        public void setFormula(NormFormula formula)
        {
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@targetNormId",formula.targetNormId),
                    new SqlParameter("@termYear",formula.termYear),                        };
                string sql = "SELECT * FROM [USTA].[dbo].[usta_NormFormula] WHERE targetNormId = @targetNormId AND termYear = @termYear";
                DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    UpdateFomula(formula);
                }
                else
                {
                    AddFomula(formula);
                }
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        private void UpdateFomula(NormFormula formula)
        {
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@targetNormId",formula.targetNormId),
                    new SqlParameter("@formula",formula.formula),
                    new SqlParameter("@formulaShow",formula.formulaShow),
                     new SqlParameter("@termYear",formula.termYear)};
                string upString = "UPDATE [USTA].[dbo].[usta_NormFormula] SET formula = @formula,formulaShow = @formulaShow WHERE targetNormId = @targetNormId AND termYear=@termYear";
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, upString, parameters);
            }
            catch (Exception e)
            {
                MongoDBLog.LogRecord(e);
                CommonUtility.RedirectUrl();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="termYear"></param>
        /// <returns></returns>
        public NormFormula GetFormula(int targetId,string termYear)
        {
            NormFormula formula = null;
            SqlParameter[] parameters = {
					new SqlParameter("@targetNormId",targetId),
                    new SqlParameter("@termYear",termYear)};
            string cmd = "SELECT [targetNormId],[formula],[formulaShow],[termYear] FROM [USTA].[dbo].[usta_NormFormula] WHERE targetNormId = @targetNormId AND termYear = @termYear ";
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmd, parameters);
            if (dr.Read())
            {
                formula = new NormFormula()
                {
                    targetNormId = Convert.ToInt32(dr["targetNormId"].ToString()),
                    formula = dr["formula"].ToString().Trim(),
                    formulaShow = dr["formulaShow"].ToString().Trim(),
                    termYear = dr["termYear"].ToString().Trim()
                };
            }
            dr.Close();
            return formula;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="teacherNo"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public float Execute(NormFormula formula,string teacherNo,string term)
        {
            if (formula.targetNormId != -1)
            {
                string targetExpression = this.GetFromExpression(formula.formula, teacherNo, term, null, null, null,0);
                if (targetExpression == null) return 0;
                if (targetExpression.Contains("/0"))//此处存在bug，懒得解决，dzx帮忙解决下呗！
                {
                    return 0;//TODO该部分没做完。
                }
                float result = qswhEval3(targetExpression);
                this.setNormValue(formula.targetNormId, teacherNo, term, result.ToString());
                return result;
            }
            else
            {
                DataTable dt = GetTeacherCourses(teacherNo, term);
                float result1 = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string targetExpression = this.GetFromExpression(formula.formula, teacherNo, term, dt.Rows[i]["courseNo"].ToString().Trim(), dt.Rows[i]["ClassID"].ToString().Trim(), dt.Rows[i]["termTag"].ToString().Trim(), int.Parse(dt.Rows[i]["atCourseType"].ToString().Trim()));
                    if (targetExpression == null) return 0;
                    if (targetExpression.Contains("/0"))//此处存在bug，懒得解决，dzx帮忙解决下呗！
                    {
                        return 0;//TODO该部分没做完。
                    }
                    result1 += qswhEval3(targetExpression);
                   
                }
                this.setNormValue(formula.targetNormId, teacherNo, term, result1.ToString());
                return result1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherNo"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        private DataTable GetTeacherCourses(string teacherNo, string term)
        {
            string sql = "select tc.courseNo,tc.termTag,tc.ClassID,tc.atCourseType from usta_CoursesTeachersCorrelation tc,usta_Courses c where teacherNo=@teacherNo and tc.termTag like @term and tc.termTag = c.termTag and tc.courseNo = c.courseNo and tc.ClassID = c.ClassID";
            SqlParameter[] parameters = {
					new SqlParameter("@teacherNo",teacherNo),
                    new SqlParameter("@term","%20"+term+"%")
                                        };
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            return ds.Tables[0];
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="express"></param>
        /// <param name="teacherNo"></param>
        /// <param name="term"></param>
        /// <param name="courseNo"></param>
        /// <param name="classId"></param>
        /// <param name="termTag"></param>
        /// <returns></returns>
        public string GetFromExpression(string express, string teacherNo, string term,string courseNo,string classId,string termTag,int atType)
        {
            string result = "";
            string number = "";
            List<string> queue = new List<string>();
            for (int i = 0; i < express.Length; i++)
            {
                string o = express.Substring(i, 1);
                if (IsOperator(o))
                {
                    foreach (string v in queue)
                    {
                        number = number + v;
                    }
                    if (number.Length > 0)
                    {
                        int value = 0;
                        if (number == "A")
                        {
                            value = -1;
                        }
                        else if (number.Equals("B"))
                        {
                            value = -2;
                        }
                        else if (number.Equals("C"))
                        {
                            value = -3;
                        }
                        else
                        {
                            value = Convert.ToInt32(number);
                        }
                        NormValue val = null;
                        if (courseNo == null)
                        {
                            val = this.getNormValue(value, teacherNo, term);
                        }
                        else
                        {
                            val = this.getNormValue(value, teacherNo, term, courseNo, classId, termTag, atType);
                        }
                        if (val != null)
                        {
                            result = result + val.value.ToString();
                        }
                        else
                        {
                            result = result + "0";
                        }
                        number = "";
                    }
                    queue.Clear();
                    result = result + o;
                }
                else
                {
                    queue.Add(o);
                }
            }
            foreach (string v in queue)
            {
                number = number + v;
            }

            if (number.Length > 0)
            {
                int value = 0;
                if (number == "A")
                {
                    value = -1;
                }
                else if (number.Equals("B"))
                {
                    value = -2;
                }
                else if (number.Equals("C"))
                {
                    value = -3;
                }
                else
                {
                    value = Convert.ToInt32(number);
                }
                NormValue val = null;
                if (courseNo == null)
                {
                    val = this.getNormValue(value, teacherNo, term);
                }
                else
                {
                    val = this.getNormValue(value, teacherNo, term, courseNo, classId, termTag, atType);
                }
                if (val != null)
                {
                    result = result + val.value.ToString();
                }
                else
                {
                    result = result + "0";
                }
                number = "";
            }

            queue.Clear();
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private Boolean IsOperator(string o)
        {
            return "+".Equals(o) || "-".Equals(o) || "*".Equals(o) || "/".Equals(o) || "(".Equals(o) || ")".Equals(o);
        }

        Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
        public float  qswhEval3(string Expression)
        {
            return float.Parse(Microsoft.JScript.Eval.JScriptEvaluate(Expression, ve).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NormConfirm GetNormConfirm(int id)
        {
            NormConfirm normConfirm = null;
            string sql = "SELECT [id],[term],[teacherNo],[question],[answer],[value],[type],[isDelete],[createTime] FROM [USTA].[dbo].[usta_NormConfirm] WHERE id=@id and isDelete =0";
            SqlParameter[] parameters = {
					new SqlParameter("@id",id)};
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
            if (dr.Read())
            {
                normConfirm = new NormConfirm
                {
                    id=int.Parse(dr["id"].ToString().Trim()),
                    term=dr["term"].ToString().Trim(),
                    teacherNo = dr["teacherNo"].ToString().Trim(),
                    question = dr["question"].ToString().Trim(),
                    answer = dr["answer"].ToString().Trim(),
                    value = dr["value"].ToString().Trim(),
                    isDelete = 0,
                    type = int.Parse(dr["type"].ToString()),
                    createTime = DateTime.Parse(dr["createTime"].ToString())
                };
            }
            dr.Close();
            conn.Close();
            return normConfirm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normConfirm"></param>
        public void AddNormConfirm(NormConfirm normConfirm)
        {
            try
            {
                string sql = "INSERT INTO [USTA].[dbo].[usta_NormConfirm]([term],[teacherNo],[question],[answer],value,type) VALUES(@term ,@teacherNo,@question,@answer,@value,@type)";
                SqlParameter[] parameters = {
					new SqlParameter("@term",normConfirm.term),
                    new SqlParameter("@teacherNo",normConfirm.teacherNo),
                    new SqlParameter("@question",normConfirm.question),
                    new SqlParameter("@answer",normConfirm.answer),
                    new SqlParameter("@value",normConfirm.value),
                    new SqlParameter("@type",normConfirm.type)};
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
            }
            catch (Exception e)
            {
                MongoDBLog.LogRecord(e);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normConfirm"></param>
        public void UpdataNormConfirm(NormConfirm normConfirm)
        {
            try
            {
                if (GetNormConfirm(normConfirm.term, normConfirm.teacherNo) != null) return;
                string sql = "UPDATE [USTA].[dbo].[usta_NormConfirm] SET [term] = @term,[teacherNo] = @teacherNo,[question] = @question,[answer] = @answer,[value] = @value ,[type] = @type WHERE id=@id";
                SqlParameter[] parameters = {
                    new SqlParameter("@id",normConfirm.id),                            
					new SqlParameter("@term",normConfirm.term),
                    new SqlParameter("@teacherNo",normConfirm.teacherNo),
                    new SqlParameter("@question",normConfirm.question),
                    new SqlParameter("@answer",normConfirm.answer),
                    new SqlParameter("@value",normConfirm.value),
                    new SqlParameter("@type",normConfirm.type)};
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
            }
            catch (Exception e)
            {
                MongoDBLog.LogRecord(e);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="answer"></param>
        public void SetAnswer(int id, string answer)
        {
             string sql = "UPDATE [USTA].[dbo].[usta_NormConfirm] SET [answer] = @answer WHERE id=@id";
             SqlParameter[] parameters = {
                    new SqlParameter("@id",id),                            
					new SqlParameter("@answer",answer)};
             SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        /// <param name="teacherNo"></param>
        /// <returns></returns>
        public DataSet GetNormConfirm(string term, string teacherNo)
        {
            string sql = "SELECT [id] ,[term],[teacherNo],[question],[answer],type,value,isDelete,createTime FROM [USTA].[dbo].[usta_NormConfirm] WHERE teacherNo=@teacherNo AND term = @term order by createTime desc";
            SqlParameter[] parameters = {
					new SqlParameter("@term",term),
                    new SqlParameter("@teacherNo",teacherNo)};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void deleteNormConfirm(int id)
        {
            try
            {
                string sql = "DELETE FROM [USTA].[dbo].[usta_NormConfirm] WHERE id=@id";
                SqlParameter[] parameters = {
					new SqlParameter("@id",id)
                   };
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
            }
            catch (Exception e)
            {
                MongoDBLog.LogRecord(e);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
            }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetTermYears()
        {
            string sql = "select distinct term as term from usta_NormValue";
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
            return ds;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public UserAuth GetUserAuth(string page)
        {
            UserAuth userAuth = null;
            string sql = "SELECT [id],[pageName],[userIds] FROM [USTA].[dbo].[usta_UserAuth] WHERE pageName = @page";
            SqlParameter[] parameters = {
					new SqlParameter("@page",page)
                   };
            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
            if (dr.Read())
            {
                userAuth = new UserAuth
                {
                    id=Convert.ToInt32( dr["id"].ToString().Trim()),
                    pageName=dr["pageName"].ToString().Trim(),
                    userIds =dr["userIds"].ToString().Trim()
                };
            }
            dr.Close();
            return userAuth;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAuth"></param>
        public void setUserAuth(UserAuth userAuth)
        {
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@pageName",userAuth.pageName),
                    new SqlParameter("@userIds",userAuth.userIds)
                   };
                string sql = "";
                if (GetUserAuth(userAuth.pageName) == null)
                {
                    sql = "INSERT INTO [USTA].[dbo].[usta_UserAuth]([pageName],[userIds]) VALUES (@pageName,@userIds)";

                }
                else
                {
                    sql = "UPDATE [USTA].[dbo].[usta_UserAuth] SET [userIds] = @userIds WHERE  [pageName] = @pageName";

                }
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
            }
            catch (Exception e)
            {
                MongoDBLog.LogRecord(e);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="term"></param>
        public void ApplyTemplate(string term)
        {

            string sql = "SELECT [normValueId],[term],[teacherNo],[value],[normId],[textValue] from usta_NormValue where teacherNo='template' and term =@term ";
            SqlParameter[] parameters = {
					new SqlParameter("@term",term)};
            DataSet templateDS = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
            DataSet teachersDS = GetAllTeachers();

            for (int i = 0; i < teachersDS.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < templateDS.Tables[0].Rows.Count; j++)
                {
                    setNormValue(int.Parse(templateDS.Tables[0].Rows[j]["normId"].ToString()),
                         teachersDS.Tables[0].Rows[i]["teacherNo"].ToString(), term, templateDS.Tables[0].Rows[j]["value"].ToString());
                }
            }
            conn.Close();
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataSet GetAllTeachers()
        {
            string cmdstring = "SELECT [teacherNo],[teacherName]  FROM [USTA].[dbo].[usta_TeachersList]";

            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring);
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherNo"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public DataTable GetCourseStatistic(string teacherNo,string term)
        {
            string sql = "select teacherName,c.courseName,tc.atCourseType,c.termTag,c.period,c.TestHours,c.courseNo,c.classID from usta_TeachersList t,usta_CoursesTeachersCorrelation tc,usta_Courses c where t.teacherNo = tc.teacherNo and tc.termTag = c.termTag and tc.courseNo = c.courseNo and tc.ClassID = c.ClassID and c.termTag like @term and t.teacherNo = @teacherNo";
            SqlParameter[] parameters = {
					new SqlParameter("@term","%20"+term+"%"),
                                        new SqlParameter("@teacherNo",teacherNo)};
           DataSet ds  = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql,parameters);
           DataTable result = new DataTable();
           result.Columns.Add("courseNo");
           result.Columns.Add("classID");
           result.Columns.Add("termTag");
           result.Columns.Add("课程名称");
           result.Columns.Add("学期");
           result.Columns.Add("类型");
           result.Columns.Add("理论课时");
           result.Columns.Add("实验课时");
          DataTable dtColum =  GetCourseStatisticNorm(term).Tables[0];
          int[] normIds = new int[dtColum.Rows.Count];
          for (int i = 0; i < dtColum.Rows.Count; i++)
          {
              result.Columns.Add(dtColum.Rows[i]["name"].ToString());
          }
          for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          {
              DataRow dr = result.NewRow();
              dr.SetField("courseNo", ds.Tables[0].Rows[i]["courseNo"]);
              dr.SetField("classID", ds.Tables[0].Rows[i]["classID"]);
              dr.SetField("termTag", ds.Tables[0].Rows[i]["termTag"]);
              dr.SetField("课程名称", ds.Tables[0].Rows[i]["courseName"].ToString().Trim() + "(" + ds.Tables[0].Rows[i]["classID"].ToString().Trim()+ ")");
              dr.SetField("学期", CommonUtility.ChangeTermToString(ds.Tables[0].Rows[i]["termTag"].ToString().Trim()));
              dr.SetField("类型", ds.Tables[0].Rows[i]["atCourseType"].ToString().Trim() == "1" ? "教师" : "助教");

              NormValue coursePre = this.getNormValue(-2, teacherNo, term, ds.Tables[0].Rows[i]["courseNo"].ToString(), ds.Tables[0].Rows[i]["classID"].ToString(), ds.Tables[0].Rows[i]["termTag"].ToString(), int.Parse(ds.Tables[0].Rows[i]["atCourseType"].ToString().Trim()));
              if (coursePre == null || coursePre.value.ToString() != ds.Tables[0].Rows[i]["period"].ToString().Trim())
              {
                  this.setNormValue(-2, teacherNo, term, ds.Tables[0].Rows[i]["period"].ToString().Trim(), ds.Tables[0].Rows[i]["courseNo"].ToString().Trim(), ds.Tables[0].Rows[i]["classID"].ToString().Trim(), ds.Tables[0].Rows[i]["termTag"].ToString().Trim(), int.Parse(ds.Tables[0].Rows[i]["atCourseType"].ToString().Trim()));
              }
              dr.SetField("理论课时", ds.Tables[0].Rows[i]["period"].ToString().Trim());

              NormValue coursePer = this.getNormValue(-3, teacherNo, term, ds.Tables[0].Rows[i]["courseNo"].ToString(), ds.Tables[0].Rows[i]["classID"].ToString(), ds.Tables[0].Rows[i]["termTag"].ToString(), int.Parse(ds.Tables[0].Rows[i]["atCourseType"].ToString().Trim()));
              if (coursePer == null || coursePer.value.ToString() != ds.Tables[0].Rows[i]["TestHours"].ToString().Trim())
              {
                  this.setNormValue(-3, teacherNo, term, ds.Tables[0].Rows[i]["TestHours"].ToString().Trim(), ds.Tables[0].Rows[i]["courseNo"].ToString().Trim(), ds.Tables[0].Rows[i]["classID"].ToString().Trim(), ds.Tables[0].Rows[i]["termTag"].ToString().Trim(), int.Parse(ds.Tables[0].Rows[i]["atCourseType"].ToString().Trim()));
              }
              dr.SetField("实验课时", ds.Tables[0].Rows[i]["TestHours"].ToString().Trim());
              for (int j = 0; j < dtColum.Rows.Count; j++)
              {
                  int normIDs = int.Parse(dtColum.Rows[j]["normId"].ToString().Trim());
                  NormValue nv = this.getNormValue(normIDs, teacherNo, term, ds.Tables[0].Rows[i]["courseNo"].ToString(), ds.Tables[0].Rows[i]["classID"].ToString(), ds.Tables[0].Rows[i]["termTag"].ToString(), int.Parse(ds.Tables[0].Rows[i]["atCourseType"].ToString().Trim()));
                  Norm nm = this.getNormById(normIDs);
                  if (nm != null && nm.type == 1)
                  {
                      dr.SetField(dtColum.Rows[j]["name"].ToString(), nv == null ? "" : nv.textValue);
                  }
                  else
                  {
                      dr.SetField(dtColum.Rows[j]["name"].ToString(), nv == null ? 0 : nv.value);
                  }
              }
              result.Rows.Add(dr);
          }
          return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetCourseStatisticNorm(string year)
        {
            string sql = "select normId,name,parentId,comment,type from usta_Norm where parentId=-1 and year=@year";
            SqlParameter[] parameters = {
					new SqlParameter("@year",year)};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql,parameters);
            return ds;
        }

        #endregion

    }

}
