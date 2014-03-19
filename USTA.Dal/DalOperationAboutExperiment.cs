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
	/// 实验操作类
	/// </summary>
	public class DalOperationAboutExperiment
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
		public DalOperationAboutExperiment()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		#region
		/// <summary>
		/// 更新实验提交
		/// </summary>
		/// <param name="experiments"></param>
		public void SubmitExperiment(Experiments experiments)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_Experiments] SET updateTime=@updateTime,attachmentId=@attachmentId,isSubmit='1'  WHERE experimentResourceId=@experimentResourceId AND studentNo=@studentNo";

				//   SqlParameter[] parameters = new SqlParameter[4]{
				//  new SqlParameter("@experimentResourceId",experiments.experimentResourceId),
				//  new SqlParameter("@studentNo",experiments.studentNo),

				//  new SqlParameter("@updateTime",experiments.updateTime.ToString()),

				//  new SqlParameter("@attachmentId",experiments.attachmentId)
				//};
				SqlParameter[] parameters = {
					
					new SqlParameter("@experimentResourceId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					
					new SqlParameter("@attachmentId", SqlDbType.NVarChar,50),
					};

				parameters[0].Value = experiments.experimentResourceId;
				parameters[1].Value = experiments.studentNo;

				parameters[2].Value = experiments.updateTime;

				parameters[3].Value = experiments.attachmentId;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 插入一条实验提交
		/// </summary>
		/// <param name="experiments"></param>
		public void InsertExperiment(Experiments experiments)
		{
			try
			{
				string cmdstring = "INSERT INTO [USTA].[dbo].[usta_Experiments] ([experimentResourceId],[studentNo] ,[updateTime] ,[attachmentId]) VALUES ( @experimentResourceId,@studentNo,@updateTime,@attachmentId)";

				//   SqlParameter[] parameters = new SqlParameter[4]{
				//  new SqlParameter("@experimentResourceId",experiments.experimentResourceId),
				//  new SqlParameter("@studentNo",experiments.studentNo),

				//  new SqlParameter("@updateTime",experiments.updateTime.ToString()),

				//  new SqlParameter("@attachmentId",experiments.attachmentId)
				//};
				SqlParameter[] parameters = {
					new SqlParameter("@experimentResourceId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),				
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@attachmentId", SqlDbType.Int,4),
					};
				parameters[0].Value = experiments.experimentResourceId;
				parameters[1].Value = experiments.studentNo;
				parameters[2].Value = experiments.updateTime;
				parameters[3].Value = experiments.attachmentId;
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 通过提交的实验主键获得提交的实验
		/// </summary>
		/// <param name="experimentId"></param>
		/// <returns></returns>
		public Experiments GetExperimentById(int experimentId)
		{
			Experiments experiments = null;
			string cmdstring = "SELECT [experimentId],[experimentResourceId],[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[remark],[excellentTime],[isExcellent],[score] FROM [USTA].[dbo].[usta_Experiments] WHERE experimentId=@experimentId";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@experimentId",experimentId)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
			if (dr.Read())
			{
				string attachId = string.Empty;
				if (dr["attachmentId"].ToString() != "") attachId = dr["attachmentId"].ToString().Trim();
				experiments = new Experiments
			 {
				 experimentId = int.Parse(dr["experimentId"].ToString()),
				 experimentResourceId = int.Parse(dr["experimentResourceId"].ToString()),
				 studentNo = dr["studentNo"].ToString().Trim(),
				 isCheck = bool.Parse(dr["isCheck"].ToString()),
				 updateTime = Convert.ToDateTime(dr["updateTime"].ToString()),
				 attachmentId = attachId,
				 remark = dr["remark"].ToString().Trim(),
				 isExcellent = bool.Parse(dr["isExcellent"].ToString()),
				 //excellentTime = Convert.ToDateTime(dr["excellentTime"].ToString())             
			 };
				if (dr["checkTime"].ToString() != "") experiments.checkTime = Convert.ToDateTime(dr["checkTime"].ToString());
				if (dr["excellentTime"].ToString() != "") experiments.excellentTime = Convert.ToDateTime(dr["excellentTime"].ToString());

			}
			dr.Close();
			conn.Close();
			return experiments;
		}

		/// <summary>
		/// 通过布置的实验获得提交的实验列表
		/// </summary>
		/// <param name="resourceId"></param>
		/// <returns></returns>
		public DataSet GetExperimentsByResourcesId(int resourceId,string name,float low,float high)
		{
            string cmdstring = "SELECT [experimentId],[experimentResourceId],[usta_Experiments].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName],[excellentTime],[isExcellent],[isSubmit],score,returnAttachmentId,usta_Experiments.remark FROM [USTA].[dbo].[usta_Experiments],usta_StudentsList WHERE experimentResourceId=@experimentResourceId AND usta_StudentsList.studentNo=usta_Experiments.studentNo AND usta_Experiments.isSubmit=1 AND studentName like @name AND score between @low and @high ORDER BY [updateTime] ASC, isCheck ASC";
			SqlParameter[] parameters = new SqlParameter[4]{
				new SqlParameter("@experimentResourceId",resourceId),
                new SqlParameter("@name","%"+name+"%"),
                new SqlParameter("@low",low),
                new SqlParameter("@high",high)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
        /// <summary>
        /// 通过布置的实验获得提交的实验列表
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public DataSet GetExperimentsByResourcesId(int resourceId, string name)
        {
            string cmdstring = "SELECT [experimentId],[experimentResourceId],[usta_Experiments].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName],[excellentTime],[isExcellent],[isSubmit],score,returnAttachmentId,usta_Experiments.remark FROM [USTA].[dbo].[usta_Experiments],usta_StudentsList WHERE experimentResourceId=@experimentResourceId AND usta_StudentsList.studentNo=usta_Experiments.studentNo AND usta_Experiments.isSubmit=1 AND studentName like @name ORDER BY [updateTime] ASC, isCheck ASC";
            SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@experimentResourceId",resourceId),
                new SqlParameter("@name","%"+name+"%")
			};
            DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
            conn.Close();
            return ds;
        }
		/// <summary>
		/// 通过布置的实验获得未提交的实验列表
		/// </summary>
		/// <param name="resourceId"></param>
		/// <returns></returns>
		public DataSet GetNoExperimentsByResourcesId(int resourceId,string name)
		{
			string cmdstring = "SELECT [experimentId],[experimentResourceId],[usta_Experiments].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName],[excellentTime],[isExcellent],[isSubmit] FROM [USTA].[dbo].[usta_Experiments],usta_StudentsList WHERE experimentResourceId=@experimentResourceId AND usta_StudentsList.studentNo=usta_Experiments.studentNo AND usta_Experiments.isSubmit=0 AND studentName like @name order by isCheck asc";

			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@experimentResourceId",resourceId),
                new SqlParameter("@name","%"+name+"%")
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}



		/// <summary>
		/// 通过学生和实验主键获得提交的实验
		/// </summary>
		/// <param name="resourceId"></param>
		/// <param name="studentNo"></param>
		/// <returns></returns>
		public DataSet GetExperimentsByResourcesIdAndStudent(int resourceId, string studentNo)
		{
			string cmdstring = "SELECT [experimentId],[experimentResourceId],[usta_Experiments].[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[studentName]  FROM [USTA].[dbo].[usta_Experiments],usta_StudentsList WHERE experimentResourceId=@experimentResourceId AND usta_StudentsList.studentNo=usta_Experiments.studentNo AND usta_StudentsList.studentNo=@studentNo";
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@experimentResourceId",resourceId),
				new SqlParameter("@studentNo",studentNo)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}

		/// <summary>
		/// 更新提交的实验
		/// </summary>
		/// <param name="experiments"></param>
		public void UpdateExperiment(Experiments experiments)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_Experiments] SET [experimentResourceId] = @experimentResourceId,[studentNo] = @studentNo,[isCheck] = @isCheck,[updateTime] = @updateTime,[checkTime] = @checkTime,[attachmentId] = @attachmentId WHERE experimentId=@experimentId";
				//   SqlParameter[] parameters = new SqlParameter[7]{
				//  new SqlParameter("@experimentResourceId",experiments.experimentResourceId),
				//  new SqlParameter("@studentNo",experiments.studentNo),
				//  new SqlParameter("@isCheck",experiments.isCheck),
				//  new SqlParameter("@updateTime",experiments.updateTime.ToString()),
				//  new SqlParameter("@checkTime",experiments.checkTime),
				//  new SqlParameter("@attachmentId",experiments.attachmentId),
				//  new SqlParameter("@experimentId",experiments.experimentId)
				//};
				SqlParameter[] parameters = {
					new SqlParameter("@experimentId", SqlDbType.Int,4),
					new SqlParameter("@experimentResourceId", SqlDbType.Int,4),
					new SqlParameter("@studentNo", SqlDbType.NChar,10),
					new SqlParameter("@isCheck", SqlDbType.Bit,1),
					new SqlParameter("@updateTime", SqlDbType.DateTime),
					new SqlParameter("@checkTime", SqlDbType.DateTime),
					new SqlParameter("@attachmentId", SqlDbType.Int,4)					
					};
				parameters[0].Value = experiments.experimentId;
				parameters[1].Value = experiments.experimentResourceId;
				parameters[2].Value = experiments.studentNo;
				parameters[3].Value = experiments.isCheck;
				parameters[4].Value = experiments.updateTime;
				parameters[5].Value = experiments.checkTime;
				parameters[6].Value = experiments.attachmentId;
				;
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		///只改变原来附件的更新
		/// </summary>
		/// <param name="experimentId"></param>
		/// <param name="attachmentId"></param>
		public void UpdateExperiment(int experimentId, int attachmentId)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_Experiments] SET [updateTime] = @updateTime,[attachmentId] = @attachmentId WHERE experimentId=@experimentId";
				//   SqlParameter[] parameters = new SqlParameter[3]{          
				//  new SqlParameter("@updateTime",DateTime.Now.ToString()),    
				//  new SqlParameter("@attachmentId",attachmentId),
				//  new SqlParameter("@experimentId",experimentId)
				//};
				SqlParameter[] parameters = new SqlParameter[3]{          
			   new SqlParameter("@updateTime", SqlDbType.DateTime),   
			   new SqlParameter("@attachmentId", SqlDbType.Int,4),	
			  new SqlParameter("@experimentId", SqlDbType.Int,4)
			 };
				parameters[0].Value = DateTime.Now.ToString();
				parameters[1].Value = attachmentId;
				parameters[2].Value = experimentId;
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 获取学生提交的实验信息-用于批阅
		/// </summary>
		/// <param name="experimentId">实验ID</param>
		/// <returns>返回DataSet</returns>
		public DataSet FindExperimentsByexperimentIdForCheck(int experimentId)
		{
			string cmdstring = "select usta_Experiments.studentNo,studentName,usta_Experiments.remark,usta_Experiments.score,usta_Experiments.returnAttachmentId ";
			cmdstring += "from usta_Experiments,usta_StudentsList ";
			cmdstring += "where usta_Experiments.studentNo=usta_StudentsList.studentNo ";
			cmdstring += "AND experimentId=@experimentId";

			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@experimentId",experimentId)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, cmdstring, parameters);
			conn.Close();
			return ds;
		}
		
		/// <summary>
		/// 批阅学生实验
		/// </summary>
		/// <param name="experimentId">实验ID</param>
		/// <param name="remark">备注</param>
		/// <param name="score">分数</param>
		/// <param name="returnId">返回附件ID</param>
		public void CheckExperimentByexperimentId(int experimentId, string remark, string score, int returnId)
		{
			try
			{
				string cmdstring = "";
				if (returnId == 0)
				{
					cmdstring = "UPDATE [USTA].[dbo].[usta_Experiments] SET isCheck= @isCheck, remark=@remark,score=@score WHERE  experimentId=@experimentId";
				}
				else
				{
					cmdstring = "UPDATE [USTA].[dbo].[usta_Experiments] SET isCheck= @isCheck, remark=@remark,score=@score,returnAttachmentId=@returnId WHERE  experimentId=@experimentId";

				}
				SqlParameter[] parameters = {
			   new SqlParameter("@experimentId",SqlDbType.Int,4),
			   new SqlParameter("@isCheck",SqlDbType.Bit,1),
			   new SqlParameter("@remark",SqlDbType.NVarChar,500),
			   new SqlParameter("@score",SqlDbType.NChar,10),
			   new SqlParameter("@returnId",SqlDbType.Int,4)
			 };
				parameters[0].Value = experimentId;
				parameters[1].Value = "true";
				parameters[2].Value = remark;
				parameters[3].Value = score;
				parameters[4].Value = returnId;

				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		/// 修改状态:是否为优秀实验
		/// </summary>
		/// <param name="experimentResourceId">实验ID</param>
		/// <param name="studentNo">学号</param>
		/// <param name="excellent">是否为优秀</param>
		public void UpdateExperimentByexperimentResourceIdAndStudentNo(int experimentResourceId, string studentNo, int excellent)
		{
			try
			{
				string cmdstring = "UPDATE [USTA].[dbo].[usta_Experiments] SET isExcellent= @isExcellent,excellentTime=@excellentTime WHERE [studentNo] = @studentNo AND experimentResourceId=@experimentResourceId";
				SqlParameter[] parameters = new SqlParameter[4]{
			   new SqlParameter("@experimentResourceId",experimentResourceId),
			   new SqlParameter("@studentNo",studentNo),
			   new SqlParameter("@isExcellent",excellent),
			   new SqlParameter("@excellentTime",DateTime.Now)
			 };
				SqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdstring, parameters);
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
		#endregion

		/// <summary>
		/// 查询学生某实验的附件编号attachmentId
		/// </summary>
		/// <param name="experimentResourceId">实验编号</param>
		/// <param name="studentNo">学号</param>
		/// <returns>附件编号</returns>
		public int FindAttachmentIdExperimentByexperimentResourceIdAndStudentNo(int experimentResourceId, string studentNo)
		{
			string cmdstring = "SELECT attachmentId FROM [USTA].[dbo].[usta_Experiments]  WHERE [studentNo] = @studentNo AND experimentResourceId=@experimentResourceId";
			SqlParameter[] parameters = new SqlParameter[2]{
			   new SqlParameter("@experimentResourceId",experimentResourceId),
			   new SqlParameter("@studentNo",studentNo),
			 };
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
			int attachmentId = 0;
			while (dr.Read())
			{
				attachmentId = int.Parse(dr["attachmentId"].ToString().Trim());
			}
			dr.Close();
			conn.Close();
			return attachmentId;
		}

		/// <summary>
		///  查询附件
		/// </summary>
		/// <param name="attachmentId">附件编号</param>
		/// <returns>附件实体</returns>

		public Attachments FindAttachmentByAttachmentId(int attachmentId)
		{
			string cmdstring = "SELECT attachmentId,attachmentTitle,attachmentUrl,updateTime FROM [USTA].[dbo].[usta_Attachments]  WHERE  attachmentId=@attachmentId";
			SqlParameter[] parameters = new SqlParameter[1]{
			   new SqlParameter("@attachmentId",attachmentId)
			 };
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, cmdstring, parameters);
			Attachments attachment = null;
			while (dr.Read())
			{
				attachment = new Attachments();
				attachment.attachmentId = int.Parse(dr["attachmentId"].ToString().Trim());
				attachment.attachmentUrl = dr["attachmentUrl"].ToString().Trim();
				attachment.attachmentTitle = dr["attachmentTitle"].ToString().Trim();
				attachment.updateTime = DateTime.Parse(dr["updateTime"].ToString().Trim());
			}
			dr.Close();
			conn.Close();
			return attachment;
		}
		/// <summary>
		/// 判断某个学生是否提交实验
		/// </summary>
		/// <param name="studentNo">学号</param>
		/// <param name="experimentSourceId">实验编号</param>
		/// <param name="eid">评估编号</param>
		/// <param name="check">是否批阅</param>
		/// <returns>是否提交</returns>
		public bool ExperimentIsCommit(string studentNo, int experimentSourceId, ref int eid, ref bool check)
		{
			bool isSubmit = false;
			string sql = "SELECT [experimentId],[experimentResourceId],[studentNo] ,[isCheck] ,[updateTime] ,[checkTime],[attachmentId],[isSubmit] FROM [USTA].[dbo].[usta_Experiments] WHERE experimentResourceId=@experimentResourceId AND studentNo=@studentNo";
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@experimentResourceId",experimentSourceId),
				new SqlParameter("@studentNo",studentNo)
			};
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
			if (dr.Read())
			{
				eid = int.Parse(dr["experimentId"].ToString());
				check = bool.Parse(dr["isCheck"].ToString());
				isSubmit = bool.Parse(dr["isSubmit"].ToString());
				dr.Close();
				conn.Close();
			}
			dr.Close();
			conn.Close();
			return isSubmit;
		}
		/// <summary>
		/// 获得某个实验的优秀提交实验
		/// </summary>
		/// <param name="exprimentResourcesId">实验编号</param>
		/// <returns>优秀实验数据集</returns>
		public DataSet GetExcellentExpriments(int exprimentResourcesId)
		{
			string sql = "SELECT [experimentId],[attachmentId], [studentName] FROM [USTA].[dbo].[usta_Experiments] ,[USTA].[dbo].usta_StudentsList WHERE experimentResourceId=@exprimentResourcesId AND isExcellent='true' AND usta_Experiments.studentNo=usta_StudentsList.studentNo";
			SqlParameter[] parameters = new SqlParameter[1]{
				new SqlParameter("@exprimentResourcesId",exprimentResourcesId)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}


		/// <summary>
		/// 根据学生获得他的提交实验
		/// </summary>
		/// <param name="studentNo">学号</param>
        /// <param name="courseNoTermTagClassID">课程学期班级集成编号</param>
		/// <returns>实验数据集</returns>
        public DataSet GetExperimentByStudentNo(string studentNo, string courseNoTermTagClassID)
		{
            string sql = "SELECT courseNo,[experimentId],[attachmentId],[experimentResourceTitle],deadLine,isCheck,isExcellent,usta_ExperimentResources.experimentResourceId,isSubmit,score,remark,returnAttachmentId,classID,termTag FROM [USTA].[dbo].[usta_Experiments],usta_ExperimentResources WHERE  usta_Experiments.studentNo=@studentNo AND usta_ExperimentResources.experimentResourceId=usta_Experiments.experimentResourceId AND (RTRIM(courseNo)+RTRIM(termTag)+RTRIM(classID)=@courseNoTermTagClassID) ORDER BY experimentResourceId DESC;";
			SqlParameter[] parameters = new SqlParameter[2]{
				new SqlParameter("@studentNo",studentNo),
				new SqlParameter("@courseNoTermTagClassID",courseNoTermTagClassID)
			};
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text, sql, parameters);
			conn.Close();
			return ds;
		}

	}
}
