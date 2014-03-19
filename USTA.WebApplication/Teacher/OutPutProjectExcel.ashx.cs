using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Text;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;
using System.Data;

namespace USTA.WebApplication.Teacher
{
    /// <summary>
    /// OutPutProjectExcel 的摘要说明
    /// </summary>
    public class OutPutProjectExcel : IHttpHandler, IRequiresSessionState
    {

        private void SetProjectSheet(HSSFSheet projectSheet, List<Project> projectList)
        {
            int cIndex = 0;
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("项目名称");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("项目负责人");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("项目类别");
            projectSheet.CreateRow(0).CreateCell(cIndex++).SetCellValue("登记时间");

            for (int rIndex = 0; rIndex < projectList.Count; rIndex++) 
            {
                cIndex = 0;
                projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(projectList[rIndex].name);
                projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(projectList[rIndex].userName);
                projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(projectList[rIndex].category.name);
                projectSheet.CreateRow(rIndex + 1).CreateCell(cIndex++).SetCellValue(projectList[rIndex].createdTime.ToString());
            
            }

        }

        public void ProcessRequest(HttpContext context)
        {
            string projectName = context.Request["pname"].Trim();
            string userName = context.Request["uname"].Trim();
            string rootCategoryId = context.Request["rc"].Trim();
            string subCategoryId = context.Request["sc"].Trim();
            string thirdCategoryId = context.Request["tc"].Trim();


            

            HSSFWorkbook workbook = new HSSFWorkbook();

            
            HSSFSheet projectSheet = workbook.CreateSheet("项目汇总");

            List<ProjectCategory> queryCategoryList = new List<ProjectCategory>();

            DalOperationAboutProject dalProject = new DalOperationAboutProject();
            DalOperationAboutProjectCategory dalProCate = new DalOperationAboutProjectCategory();

            if (!(string.IsNullOrWhiteSpace(thirdCategoryId) || "0" == thirdCategoryId.Trim()))
            {
                queryCategoryList.Add(dalProCate.GetProjectCategoryById(int.Parse(thirdCategoryId.Trim())));
            }
            else if (!(string.IsNullOrWhiteSpace(subCategoryId) || "0" == subCategoryId.Trim()))
            {
                queryCategoryList.AddRange(dalProCate.GetAllLastProjectCategoryByParentId(int.Parse(subCategoryId.Trim())));
            }
            else if (!(string.IsNullOrWhiteSpace(rootCategoryId) || "0" == rootCategoryId.Trim()))
            {

                queryCategoryList.AddRange(dalProCate.GetAllLastProjectCategoryByParentId(int.Parse(rootCategoryId.Trim())));
            }

            string categoryIds = null;

            if (queryCategoryList.Count != 0)
            {
                List<int> categoryIdList = new List<int>();
                foreach (ProjectCategory category in queryCategoryList)
                {
                    categoryIdList.Add(category.id);
                }

                categoryIds = string.Join(",", categoryIdList.ToArray());
            }


            List<Project> projectList;
            if (string.IsNullOrWhiteSpace(projectName) && string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(categoryIds))
            {
                projectList = dalProject.GetAllProjects();
            }
            else
            {
                projectList = dalProject.GetPrjects(categoryIds, userName.Trim(), projectName.Trim());
            }

            SetProjectSheet(projectSheet, projectList);

            string fileName = "项目汇总";

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