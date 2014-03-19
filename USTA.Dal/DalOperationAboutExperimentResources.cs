using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using USTA.Model;
using USTA.Common;

namespace USTA.Dal
{
	/// <summary>
	/// 实验资源操作类
	/// </summary>
	public class DalOperationAboutExperimentResources
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
		public DalOperationAboutExperimentResources()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion


		#region

		/// <summary>
		/// 通过实验主键获得实验
		/// </summary>
		/// <param name="experimentResourcesId">实验编号</param>
		/// <returns>实验数据集</returns>
		public DataSet GetExperimentResourcesById(int experimentResourcesId)
		{
			string commandString = "SELECT [experimentResourceId],[courseNo] ,[attachementIds],[experimentResourceTitle] ,[experimentResourceContent],[deadLine],[updateTime]  FROM [USTA].[dbo].[usta_ExperimentResources] WHERE experimentResourceId=@experimentResourceId";
			SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@experimentResourceId",experimentResourcesId)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
			conn.Close();
			return ds;
		}
	   /// <summary>
	   /// 课程实验的信息
	   /// </summary>
	   /// <param name="courseNo">课程编号</param>
	   /// <returns>课程实验数据集</returns>
		public DataSet GetExperimentsResourcesByCourseNo(string courseNo,string classId,string termtag)
		{
			string commandString = "select experimentResourceId,courseNo,experimentResourceTitle,attachementIds, experimentResourceContent,deadLine,updateTime from usta_ExperimentResources WHERE courseNo=@courseNo AND classID=@classId AND termTag = @termtag order by experimentResourceId asc";
			SqlParameter[] parameters = new SqlParameter[3]{
                new SqlParameter("@courseNo",courseNo),
                new SqlParameter("@classId",classId),
                new SqlParameter("@termtag",termtag)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 取得提交课程实验及提交的信息
		/// </summary>
		/// <param name="courseNo">课程编号</param>
		/// <returns>课程实验数据集</returns>
		public DataSet FindExperimentResourcesByCourseNo(string courseNo,string classId,string termtag)
		{
			string commandString = "SELECT studentNo,experimentResourceTitle,remark,score ";
			commandString += "FROM  usta_ExperimentResources,usta_Experiments ";
			commandString += "WHERE usta_ExperimentResources.experimentResourceId=usta_Experiments.experimentResourceId ";
			commandString += "AND courseNo=@courseNo AND classID=@classId AND termTag=@termtag ";
			SqlParameter[] parameters = new SqlParameter[3]{
                new SqlParameter("@courseNo",courseNo),
                new SqlParameter("@classId",classId),
                 new SqlParameter("@termtag",termtag)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, commandString, parameters);
			conn.Close();
			return ds;
		}
		/// <summary>
		/// 通过布置的实验主键获得布置实验
		/// </summary>
		/// <param name="experimentResourcesId">实验编号</param>
		/// <returns>课程实验实体</returns>
		public ExperimentResources GetExperimentResourcesbyId(int experimentResourcesId)
		{
			ExperimentResources ExperimentResources = null;
			string commandString = "SELECT [experimentResourceId],[courseNo] ,[attachementIds],[experimentResourceTitle] ,[experimentResourceContent],[deadLine],[updateTime]  FROM [USTA].[dbo].[usta_ExperimentResources] WHERE experimentResourceId=@experimentResourceId";
			SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@experimentResourceId",experimentResourcesId)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
			if (dr.Read())
			{
				ExperimentResources = new ExperimentResources
				{
					experimentResourceId = int.Parse(dr["experimentResourceId"].ToString()),
					courseNo = dr["courseNo"].ToString().Trim(),
					attachmentIds = dr["attachementIds"].ToString().Trim(),
					experimentResourceTitle = dr["experimentResourceTitle"].ToString().Trim(),
					experimentResourceContent = dr["experimentResourceContent"].ToString().Trim(),
					deadLine = Convert.ToDateTime(dr["deadLine"].ToString()),
					updateTime = Convert.ToDateTime(dr["updateTime"].ToString())
				};
			}
			dr.Close();
			conn.Close();
			return ExperimentResources;
		}

	   /// <summary>
	   /// 添加一条布置实验
	   /// </summary>
	   /// <param name="experimentResources">实验实体</param>
	   /// <returns>实体实体</returns>
		public ExperimentResources InsertExperimentResources(ExperimentResources experimentResources)
		{
			try
			{
				using (SqlCommand cmd = new SqlCommand("spExperimentResourcesAdd", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Connection = conn;

					conn.Open();
					cmd.Parameters.Add(new SqlParameter("@courseNo", experimentResources.courseNo));
					cmd.Parameters.Add(new SqlParameter("@attachementIds", experimentResources.attachmentIds));
					cmd.Parameters.Add(new SqlParameter("@experimentResourceTitle", experimentResources.experimentResourceTitle));
					cmd.Parameters.Add(new SqlParameter("@experimentResourceContent", experimentResources.experimentResourceContent));
					cmd.Parameters.Add(new SqlParameter("@deadLine", experimentResources.deadLine));
					cmd.Parameters.Add(new SqlParameter("@updateTime", experimentResources.updateTime));
                    cmd.Parameters.Add(new SqlParameter("@classId", experimentResources.classID));
                    cmd.Parameters.Add(new SqlParameter("@termtag", experimentResources.termTag));
					cmd.Parameters.Add(new SqlParameter("@experimentResourceId", 0));
					cmd.Parameters["@experimentResourceId"].Direction = ParameterDirection.Output;
					cmd.ExecuteNonQuery();
					experimentResources.experimentResourceId = int.Parse(cmd.Parameters["@experimentResourceId"].Value.ToString());
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
			return experimentResources;
		   
		}

		/// <summary>
		/// 更新布置的实验信息
		/// </summary>
		/// <param name="experimentResources">实验实体</param>
		public void UpdateExperimentResources(ExperimentResources experimentResources)
		{
			try
			{
				string commandString = "UPDATE [USTA].[dbo].[usta_ExperimentResources] SET [courseNo] = @courseNo ,[attachementIds] = @attachementIds,[experimentResourceTitle] = @experimentResourceTitle  ,[experimentResourceContent] = @experimentResourceContent ,[deadLine] = @deadLine ,[updateTime] = @updateTime WHERE experimentResourceId=@experimentResourceId";

				SqlParameter[] parameters = {
					new SqlParameter("@experimentResourceId", SqlDbType.Int,4),
					new SqlParameter("@courseNo", SqlDbType.NChar,20),
					new SqlParameter("@attachementIds", SqlDbType.NVarChar,200),
					new SqlParameter("@experimentResourceTitle", SqlDbType.NChar,50),
					new SqlParameter("@experimentResourceContent", SqlDbType.NText),
					new SqlParameter("@deadLine", SqlDbType.DateTime),
					new SqlParameter("@updateTime", SqlDbType.DateTime)};
				parameters[0].Value = experimentResources.experimentResourceId;
				parameters[1].Value = experimentResources.courseNo;
				parameters[2].Value = experimentResources.attachmentIds;
				parameters[3].Value = experimentResources.experimentResourceTitle;
				parameters[4].Value = experimentResources.experimentResourceContent;
				parameters[5].Value = experimentResources.deadLine;
				parameters[6].Value = experimentResources.updateTime;
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
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
		/// 通过主键删除布置的实验
		/// </summary>
		/// <param name="experimentResourcesId">实验编号</param>
		public void DelExperimentResources(int experimentResourcesId)
		{
			using (TransactionScope scope = new TransactionScope())
			{
				try
				{

					string commandString = "DELETE FROM [USTA].[dbo].[usta_ExperimentResources] WHERE experimentResourceId=@experimentResourceId;DELETE FROM [USTA].[dbo].[usta_Experiments] WHERE experimentResourceId=@experimentResourceId";
					SqlParameter[] parameters = new SqlParameter[1]{new SqlParameter("@experimentResourceId",experimentResourcesId)
			};
					SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
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
				scope.Complete();
			}
		}
		#endregion
	}
}
