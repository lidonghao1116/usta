using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using USTA.Model;
using System.Data;
using System.Transactions;
using USTA.Common;
using System.Web;
using System.Threading.Tasks;

namespace USTA.Dal
{
	/// <summary>
	/// Email操作类
	/// </summary>
	public class DalOperationAboutEmail
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
		public DalOperationAboutEmail()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

		}
		#endregion


		/// <summary>
		/// 获取EmailConfig信息
		/// </summary>
		/// <param name="sender">邮件发送者</param>
		/// <returns>邮件配置实体</returns>
		public EmailConfig GetEmailConfig()
		{
			EmailConfig emailConfig = null;
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text,
				  "SELECT emailAddress,emailPassword,emailServerAddress,emailServerPort FROM usta_EmailConfig");
			while (dr.Read())
			{
				emailConfig = new EmailConfig
				{
					emailAddress = dr["emailAddress"].ToString().Trim(),
					emailPassword = dr["emailPassword"].ToString().Trim(),
					emailServerAddress = dr["emailServerAddress"].ToString().Trim(),
					emailServerPort = int.Parse(dr["emailServerPort"].ToString().Trim())
				};
			}
			dr.Close();
			conn.Close();
			return emailConfig;
		}

		/// <summary>
		/// 更新EmailConfig
		/// </summary>
		/// <param name="emailConfig">邮件配置实体</param>
		/// <returns>操作成功标号</returns>
		public int UpdateEmailConfig(EmailConfig emailConfig)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					new SqlParameter("@emailPassword", SqlDbType.NChar,50),
					new SqlParameter("@emailServerAddress", SqlDbType.NChar,50),
					new SqlParameter("@emailServerPort", SqlDbType.Int,4),
					 new SqlParameter("@sender", SqlDbType.NChar,10)};
			parameters[0].Value = emailConfig.emailAddress;
			parameters[1].Value = emailConfig.emailPassword;
			parameters[2].Value = emailConfig.emailServerAddress;
			parameters[3].Value = emailConfig.emailServerPort;
			parameters[4].Value = emailConfig.sender;

			SqlHelper.ExecuteNonQuery(conn, CommandType.Text,
				"UPDATE usta_EmailConfig SET emailAddress=@emailAddress,emailPassword=@emailPassword,emailServerAddress=@emailServerAddress, emailServerPort=@emailServerPort WHERE sender=@sender",
				parameters);
			return 1;
		}

		/// <summary>
		/// 添加邮件到邮件发送队列，支持添加多封邮件
		/// </summary>
		/// <param name="sendingEmailList">邮件发送队列数组</param>
		/// <returns>操作成功标号</returns>
		public int AddEmailToSendingQueue(SendingEmailList[] sendingEmailList)
		{
            int sendingEmailListId = -1;

			//使用全局事务进行控制，保证插入数据的完整性
			using (TransactionScope scope = new TransactionScope())
			{
				foreach (SendingEmailList sendingEmail in sendingEmailList)
				{
					SqlParameter[] parameters = {
					new SqlParameter("@emailTitle", SqlDbType.NChar,50),
					new SqlParameter("@emailContent", SqlDbType.NText),
					new SqlParameter("@emailAttachmentIds", SqlDbType.NVarChar,200),
					
				
					new SqlParameter("@userName", SqlDbType.NChar,10),
					new SqlParameter("@emailAddress", SqlDbType.NChar,50),
					 new SqlParameter("@sender", SqlDbType.NChar,10),
					   new SqlParameter("@sendType", SqlDbType.Int,4)  
												};
					parameters[0].Value = sendingEmail.emailTitle;
					parameters[1].Value = sendingEmail.emailContent;
					parameters[2].Value = sendingEmail.emailAttachmentIds;

					parameters[3].Value = sendingEmail.userName;
					parameters[4].Value = sendingEmail.emailAddress;
					parameters[5].Value = sendingEmail.sender;
					parameters[6].Value = sendingEmail.sendType;
                    sendingEmailListId = int.Parse(SqlHelper.ExecuteScalar(conn, CommandType.Text,
                        "INSERT INTO usta_SendingEmailList(emailTitle,emailContent,emailAttachmentIds,userName,emailAddress,sender,sendType) VALUES (@emailTitle,@emailContent,@emailAttachmentIds,@userName,@emailAddress,@sender,@sendType);SELECT @@identity;"
						, parameters).ToString());
				}
				scope.Complete();
			}

			return sendingEmailListId;
		}

        /// <summary>
        /// 查询邮件发送队列所有数据,-1表示查看所有的邮件
        /// </summary>
        /// <param name="sender">邮件发送者</param>
        /// <returns>邮件发送队列数据集</returns>
        public DataSet GetEmailSendingQueue(int emailType)
        {
            string strSql = "SELECT sendingEmailListId,emailTitle,emailContent,emailAttachmentIds,updateTime,isSendSuccess,userName,emailAddress,sendTimes,sendType FROM usta_SendingEmailList ORDER BY isSendSuccess ASC";

            if (emailType != -1)
            {
                strSql = "SELECT sendingEmailListId,emailTitle,emailContent,emailAttachmentIds,updateTime,isSendSuccess,userName,emailAddress,sendTimes,sendType FROM usta_SendingEmailList where sendType=@sendType ORDER BY isSendSuccess ASC";

                SqlParameter[] parameters = {
                    new SqlParameter("@sendType", SqlDbType.Int,4)};
                parameters[0].Value = emailType;
                return  SqlHelper.ExecuteDataset(conn, CommandType.Text,
                    strSql, parameters);
            }

            return SqlHelper.ExecuteDataset(conn, CommandType.Text,
                    strSql);
        }



	   /// <summary>
	   /// 发送成功后更新对应记录的发送状态为成功
	   /// </summary>
	   /// <param name="sendingEmailListId">发送队列编号</param>
	   /// <returns>操作成功标号</returns>
		public int SetSendSuccess(int sendingEmailListId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@sendingEmailListId", sendingEmailListId)};
			SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "UPDATE usta_SendingEmailList SET isSendSuccess=1 WHERE sendingEmailListId=@sendingEmailListId", parameters);
			return 1;
		}

		/// <summary>
		/// 定时检测待发送邮件数据表中是否有待发送邮件，若有则取出发送，并设置发送状态为成功
		/// 与Global.asax中的定时Timer类结合使用
		/// </summary>
		/// <returns>操作成功标号</returns>
		public int CheckEmailSendingQueueToSend()
		{

			List<EmailInfo> emailInfoList = new List<EmailInfo>();
			string sender = string.Empty;

			//查询前四条未发送记录，取消发送次数超过4次后不再发送的限制，
            //主要为了保证Email尽最大可能地发送出去(AND sendTimes<4)
			DataSet ds = SqlHelper.ExecuteDataset(conn, CommandType.Text,
                "SELECT TOP " + ConfigurationManager.AppSettings["maxSendEmailNum"] + " sendingEmailListId,emailTitle,emailContent,emailAttachmentIds,updateTime,userName,emailAddress,sender,sendTimes FROM usta_SendingEmailList WHERE isSendSuccess=0");


            //此处有异常，已经改进
            if (ds.Tables[0].Rows.Count > 0)
            {
                AddSendEmailTime(int.Parse(ds.Tables[0].Rows[0]["sendingEmailListId"].ToString().Trim()));
            }

		   
			List<string> fileNameListCollecton = new List<string>();
			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				EmailConfig emailConfig = GetEmailConfig();
				emailInfoList.Add(new EmailInfo
				{
					sendingEmailListId = int.Parse(ds.Tables[0].Rows[i]["sendingEmailListId"].ToString().Trim()),
					emailTitle = ds.Tables[0].Rows[i]["emailTitle"].ToString().Trim(),
					emailContent = ds.Tables[0].Rows[i]["emailContent"].ToString().Trim(),
					senderDisplayName = ds.Tables[0].Rows[i]["userName"].ToString().Trim(),
					receiverEmailAddress = ds.Tables[0].Rows[i]["emailAddress"].ToString().Trim(),


					//此处还需要添加EmailConfig
					senderEmailAddress = emailConfig.emailAddress,
					senderEmailPassword = emailConfig.emailPassword,
					senderEmailServer = emailConfig.emailServerAddress,
					senderEmailServerPort = emailConfig.emailServerPort
				});
				fileNameListCollecton.Add(ds.Tables[0].Rows[i]["emailAttachmentIds"].ToString().Trim());

			}



			
		   // dr.Close();
			//处理附件名称



			for (int i = 0; i < emailInfoList.Count; i++)
			{
				emailInfoList[i].fileNameList = GetFileListByAttachmentIds(fileNameListCollecton[i]);
			}

			//使用并行计算改进性能
			int emailInfoListCount = emailInfoList.Count;

			Parallel.For(0, emailInfoListCount, (i) =>
			{

				//使用全局事务进行控制，保证插入数据的完整性
				using (TransactionScope scope = new TransactionScope())
				{
					//附件功能待完善，已经改进
					CommonUtility.SendEmail(emailInfoList);

					foreach (EmailInfo emailInfo in emailInfoList)
					{
						SetSendSuccess(emailInfo.sendingEmailListId);
					}

					scope.Complete();
				}
			});



			return 1;
		}
		/// <summary>
		/// 邮件发送次数加一
		/// </summary>
		/// <param name="sendEmailListId">发送邮件队列编号</param>
		public void AddSendEmailTime(int sendEmailListId)
		{
			string sql = "UPDATE [USTA].[dbo].[usta_SendingEmailList] SET [sendTimes] = sendTimes+1 WHERE sendingEmailListId=@sendEmailListId";
			SqlParameter[] parameters = {
					new SqlParameter("@sendEmailListId", sendEmailListId)
		 };
			SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
			
		}
		/// <summary>
		/// 删除邮件列表中的邮件
		/// </summary>
		/// <param name="sendEmailListId">发送邮件队列编号</param>
		/// <returns>数据库删除行数</returns>
		public int DeleteSendEmail(int sendEmailListId)
		{
			string sql = "DELETE FROM [USTA].[dbo].[usta_SendingEmailList] WHERE sendingEmailListId=@sendEmailListId";
			SqlParameter[] parameters = {
					new SqlParameter("@sendEmailListId", sendEmailListId)
		 };
		   return SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);

		}



        /// <summary>
        /// 根据主键ID获取邮件的详细信息
        /// </summary>
        /// <param name="sendEmailListId">发送邮件队列编号</param>
        /// <returns></returns>
        public DataSet GetEmailInfoById(int sendEmailListId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 sendingEmailListId,emailTitle,emailContent,emailAttachmentIds,updateTime,isSendSuccess,userName,emailAddress,sender,sendTimes,sendType from usta_SendingEmailList ");
            strSql.Append(" where sendingEmailListId=@sendingEmailListId");
            SqlParameter[] parameters = {
					new SqlParameter("@sendingEmailListId", SqlDbType.Int,4)
};
            parameters[0].Value = sendEmailListId;

            return SqlHelper.ExecuteDataset(conn, CommandType.Text, strSql.ToString(), parameters);

        }


		#region 根据传入的参数查询对应的附件并返回一个文件名列表List
		/// <summary>
		/// 获取文件列表
		/// </summary>
		/// <param name="attachmentIds">附件字符串</param>
		/// <returns>附件列表</returns>
		public List<string>[] GetFileListByAttachmentIds(string attachmentIds)
        {
            List<string>[] fileList = new List<string>[2];
            List<string> fileNameList = new List<string>();
			List<string> fileUrlList = new List<string>();

            string sql = "SELECT [attachmentTitle],[attachmentUrl] FROM [usta_Attachments] WHERE CHARINDEX(','+LTRIM(attachmentId)+',',','+@attachmentIds+',')>0";

			SqlParameter[] parameters = new SqlParameter[1]{
				 new SqlParameter("@attachmentIds",attachmentIds),                                        
			};

			SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
			while (dr.Read())
			{
                fileNameList.Add(dr["attachmentTitle"].ToString().Trim());
                fileUrlList.Add(ConfigurationManager.AppSettings["sseaDomain"].Substring(0, ConfigurationManager.AppSettings["sseaDomain"].Length - 2) + dr["attachmentUrl"].ToString().Trim());
			}
			dr.Close();
            fileList[0] = fileNameList;
            fileList[1] = fileUrlList;
			return fileList;
		}
		#endregion
	}
}