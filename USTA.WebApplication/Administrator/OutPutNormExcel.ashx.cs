using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

using NPOI.HSSF.UserModel;
using USTA.Dal;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    /// <summary>
    /// OutPutNormExcel 的摘要说明
    /// </summary>
    public class OutPutNormExcel : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";

            string term = context.Request["term"];
            string searchKey = context.Request["searchKey"].Trim();
            string teacherType = context.Request["teacherType"].Trim();
            DalOperationNorm dalnorm = new DalOperationNorm();
            DataSet ds = dalnorm.getTeacherLoad(term, searchKey, teacherType);

            HSSFWorkbook workbook = new HSSFWorkbook();
            for (int tableIndex = 0; tableIndex < ds.Tables.Count; tableIndex++)
            {
                HSSFSheet sheet = workbook.CreateSheet(ds.Tables[tableIndex].TableName);

                DataRowCollection drc = ds.Tables[tableIndex].Rows;
                for (int i = 0; i < ds.Tables[tableIndex].Columns.Count; i++)
                {
                    sheet.CreateRow(0).CreateCell(i).SetCellValue(ds.Tables[tableIndex].Columns[i].ColumnName);
                }
                for (int i = 0; i < drc.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[tableIndex].Columns.Count; j++)
                    {
                        sheet.CreateRow(i + 1).CreateCell(j).SetCellValue(drc[i][j].ToString());
                    }
                }
            }





            string fileName = "norm";

            System.IO.FileStream file = new System.IO.FileStream(HttpContext.Current.Server.MapPath(fileName + ".xls"), System.IO.FileMode.Create);
            workbook.Write(file);
            file.Dispose();

            ////插入值
            FileInfo DownloadFile = new FileInfo(context.Server.MapPath(fileName + ".xls"));

            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.Buffer = false;
            Encoding code = Encoding.GetEncoding("gb2312");
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.HeaderEncoding = code;//这句很重要
            context.Response.ContentType = "application/octet-stream";
            context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");
            context.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
            context.Response.WriteFile(DownloadFile.FullName);

            if (File.Exists(context.Server.MapPath(fileName + ".xls")))
            {
                File.Delete(context.Server.MapPath(fileName + ".xls"));
            }
            context.Response.Flush();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}