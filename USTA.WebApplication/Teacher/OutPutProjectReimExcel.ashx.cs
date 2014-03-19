using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using USTA.Model;
using USTA.Dal;
using System.IO;
using System.Text;

namespace USTA.WebApplication.Teacher
{
    /// <summary>
    /// OutPutProjectReimExcel 的摘要说明
    /// </summary>
    public class OutPutProjectReimExcel : IHttpHandler
    {

        private void SetProjectReimSheet(HSSFSheet projectSheet, List<ReimItem> reimItemList)
        {
            int cIndex = 0;
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("项目名称");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("报销项名称");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("报销金额");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("报销时间");
            if (reimItemList != null) {
                for (int rIndex = 0; rIndex < reimItemList.Count; rIndex++)
                {
                    cIndex = 0;
                    projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(reimItemList[rIndex].project.name);
                    projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(reimItemList[rIndex].reim.name);
                    projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(reimItemList[rIndex].value);
                    projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(reimItemList[rIndex].createdTime.ToString());

                }
            }
        }

        private void SetProjectReimSummarySheet(HSSFSheet projectSheet, List<ReimItem> reimItemList) 
        {

            int cIndex = 0;
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("项目名称");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("报销项");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("报销金额");
            
            if (reimItemList != null)
            {
                for (int rIndex = 0; rIndex < reimItemList.Count; rIndex++)
                {
                    cIndex = 0;
                    projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(reimItemList[rIndex].project.name);
                    projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(reimItemList[rIndex].reim.name);
                    projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(reimItemList[rIndex].value);
                }
            }
        }

        private void SetProjectSummarySheet(HSSFSheet projectSummarySheet, List<ReimItem> projectReimSummaryList)
        {
            int cIndex = 0;
            projectSummarySheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("项目名称");
            projectSummarySheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("报销金额");

            if (projectReimSummaryList != null)
            {
                for (int rIndex = 0; rIndex < projectReimSummaryList.Count; rIndex++)
                {
                    cIndex = 0;
                    projectSummarySheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(projectReimSummaryList[rIndex].project.name);
                   projectSummarySheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(projectReimSummaryList[rIndex].value);
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string projectName = context.Request["pname"].Trim();
            


            HSSFWorkbook workbook = new HSSFWorkbook();


            HSSFSheet projectSheet = workbook.CreateSheet("项目各报销项汇总");
            HSSFSheet projectReimSummarySheet = workbook.CreateSheet("项目报销项金额汇总");
            HSSFSheet projectSummarySheet = workbook.CreateSheet("项目报销总金额汇总");

            List<ProjectCategory> queryCategoryList = new List<ProjectCategory>();

            DalOperationAboutReimItem dalReimItem = new DalOperationAboutReimItem();

            List<ReimItem> reimItemList;
            if (!string.IsNullOrWhiteSpace(projectName))
            {
                reimItemList = dalReimItem.GetReimItems(projectName.Trim(), 0, 0);
            }
            else
            {
                reimItemList = dalReimItem.GetAllDistinctReimItems();
            }

            List<ReimItem> reimItemSummaryList = dalReimItem.GetReimItemSummaryValues(projectName, 0, 0, null);

            List<ReimItem> projectReimSummaryList = dalReimItem.GetProjectReimItems(projectName, 0);

            SetProjectReimSheet(projectSheet, reimItemList);

            SetProjectReimSummarySheet(projectReimSummarySheet, reimItemSummaryList);
            SetProjectSummarySheet(projectSummarySheet, projectReimSummaryList);

            string fileName = "项目报销项汇总";

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