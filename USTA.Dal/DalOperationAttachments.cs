using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Web;

namespace USTA.Dal
{
	using USTA.Model;
	using USTA.Common;

	/// <summary>
	/// 附件操作类
	/// </summary>
	public class DalOperationAttachments
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
		public DalOperationAttachments()
		{
			conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
		}
		#endregion

		#region

		/// <summary>
		/// 添加通知和办事流程附件
		/// </summary>
		/// <param name="attachment">附件实体</param>
		/// <returns>附件实体</returns>
		public Attachments AddAdminNotifyAttachment(Attachments attachment)
		{
			using (SqlCommand cmd = new SqlCommand("spAttachmentsAdd", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = conn;

				conn.Open();
				cmd.Parameters.Add(new SqlParameter("@attachmentTitle", attachment.attachmentTitle));
				cmd.Parameters.Add(new SqlParameter("@attachmentUrl", attachment.attachmentUrl));
				cmd.Parameters.Add(new SqlParameter("@attachmentId", 0));
				cmd.Parameters["@attachmentId"].Direction = ParameterDirection.Output;
				cmd.ExecuteNonQuery();
				attachment.attachmentId = int.Parse(cmd.Parameters["@attachmentId"].Value.ToString());
				conn.Close();
			}
			return attachment;

		}



		/// <summary>
		/// 修改通知和办事流程附件, 暂不使用，因为项目采用的方案是删除后再上传，不再进行直接更新
		/// </summary>
		/// <param name="attachment">附件实体</param>
		public void UpdateAdminNotifyAttachment(Attachments attachment)
		{
			try
			{
				SqlParameter[] parameters = {
					new SqlParameter("@attachmentTitle", SqlDbType.NChar,50),
					new SqlParameter("@attachmentId", SqlDbType.Int,4),
					new SqlParameter("@updateTime", SqlDbType.DateTime)};
				parameters[0].Value = attachment.attachmentTitle;
				parameters[1].Value = attachment.attachmentId;
				parameters[2].Value = DateTime.Now;
				SqlHelper.ExecuteNonQuery(conn, "spAttachmentsUpdate", parameters);
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
		///查询单个通知和办事流程附件 
		/// </summary>
		/// <param name="attachmentId">附件编号</param>
		/// <returns></returns>
		public Attachments FindAdminNotifyAttachmentById(int attachmentId)
		{
			Attachments attachment = null;
			SqlParameter[] parameters = {
				 new SqlParameter("@attachmentId", SqlDbType.Int,4)                                       
			};
			parameters[0].Value = attachmentId;
			SqlDataReader dr = SqlHelper.ExecuteReader(conn, "spAttachmentsGetModel", parameters);
			while (dr.Read())
			{
				attachment = new Attachments
				{
					attachmentId = int.Parse(dr["attachmentId"].ToString()),
					attachmentTitle = dr["attachmentTitle"].ToString().Trim(),
					attachmentUrl = dr["attachmentUrl"].ToString().Trim(),
					updateTime = DateTime.Parse(dr["updateTime"].ToString().Trim())
				};
			}
			dr.Close();
			conn.Close();
			return attachment;
		}


	  /// <summary>
	  /// 查询所有的附件
	  /// </summary>
	  /// <param name="attachmentIds">附件编号字符</param>
	  /// <param name="iframeCount">附件控件的数据</param>
	  /// <param name="isUseDeleteAttachment">是否用户删除附件</param>
	  /// <returns>附件内容串</returns>
        public string GetAttachmentsList(string attachmentIds, ref int iframeCount, bool isUseDeleteAttachment, string hiddentId)
        {
            StringBuilder sb = new StringBuilder();

            string sql = "SELECT [attachmentId],[attachmentTitle],[attachmentUrl],[updateTime] FROM [usta_Attachments] WHERE CHARINDEX(','+LTRIM(attachmentId)+',',','+@attachmentIds+',')>0";

            SqlParameter[] parameters = {
				new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)                                        
			};
            parameters[0].Value = attachmentIds;

            SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
            while (dr.Read())
            {
                iframeCount += 1;
                //查询对应的附件信息
                sb.Append("<table id='tableIframes" + iframeCount + "' class='uploadStyle uploadStyleView' style='margin-bottom:5px;'><tr><td><a href='");
                sb.Append(dr["attachmentUrl"].ToString().Trim());
                sb.Append("' target='_blank' >");
                sb.Append(dr["attachmentTitle"].ToString().Trim());

                sb.Append("</a>");
                if (isUseDeleteAttachment)
                {
                    sb.Append("    <a onclick=\"deleteAttachment(" + dr["attachmentId"].ToString().Trim() + "," + iframeCount + ",'" + hiddentId + "');\" style='text-decoration:underline;cursor:pointer;'>[删除]</a>");
                }
                sb.Append("</td></tr></table>");
            }
            dr.Close();
            conn.Close();
            return sb.ToString();
        }

		/// <summary>
		/// 根据传入的ID数据获取附件的URL
		/// </summary>
		/// <param name="attachmentIds">附件字符</param>
		/// <returns>附件列表</returns>
		public IList<string> GetAttachmentByIds(string attachmentIds)
		{
			IList<string> ilist = new List<string>();
			string sql = "SELECT [attachmentId],[attachmentTitle],[attachmentUrl],[updateTime] FROM [usta_Attachments] WHERE CHARINDEX(','+LTRIM(attachmentId)+',',','+@attachmentIds+',')>0";

			SqlParameter[] parameters = {
				new SqlParameter("@attachmentIds", SqlDbType.NVarChar,200)                                        
			};
			parameters[0].Value = attachmentIds;

			try
			{
				SqlDataReader dr = SqlHelper.ExecuteReader(conn, CommandType.Text, sql, parameters);
				while (dr.Read())
				{
					ilist.Add(dr["attachmentUrl"].ToString().Trim());
				}
				dr.Close();
			}

			finally
			{
				conn.Close();
			}

			return ilist;
		}
		#endregion
	}
}
