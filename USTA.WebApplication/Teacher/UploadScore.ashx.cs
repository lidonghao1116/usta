using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;

using NPOI.HSSF.UserModel;
using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Teacher
{
    /// <summary>
    /// UploadScore 的摘要说明
    /// </summary>
    public class UploadScore : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            context.Response.CacheControl = "no-cache";

            string courseNo = context.Request["courseNo"];
            string classId = context.Request["classID"].Trim();
            string termtag = context.Request["termtag"].Trim();
            //取得选课信息
            DalOperationAboutCourses dalOperationAboutCourses = new DalOperationAboutCourses();
            DataTable dt = dalOperationAboutCourses.FindStudentInfoFromStudentListAndCorrelation(courseNo, classId, termtag).Tables[0];

            //取得实验提交表信息
            DalOperationAboutExperimentResources doaer = new DalOperationAboutExperimentResources();
            DataTable dtExp = doaer.FindExperimentResourcesByCourseNo(courseNo, classId, termtag).Tables[0];

            //取得对应的CourseNo已布置的全部实验统计信息
            DataTable dtExperiment = doaer.GetExperimentsResourcesByCourseNo(courseNo, classId, termtag).Tables[0];

            //取得作业提交表信息
            DalOperationAboutSchoolWorks doask = new DalOperationAboutSchoolWorks();
            DataTable dtSchwork = doask.FindSchoolWorksByCourseNo(courseNo, classId, termtag).Tables[0];

            //取得对应的CourseNo已布置的全部作业统计信息
            DataTable dtSchNotify = doask.GetSchoolWorksByCourseNo(courseNo, classId, termtag).Tables[0];

            StringBuilder sb = new StringBuilder();

            HSSFWorkbook workbook = new HSSFWorkbook();

            //创建Excel单元格样式
            HSSFCellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HSSFCellStyle.ALIGN_RIGHT;


            //创建WorkSheet
            HSSFSheet sheet = workbook.CreateSheet("成绩统计表");

            int rowCount = dt.Rows.Count;

            sheet.CreateRow(0).CreateCell(0).SetCellValue("学生编号");
            sheet.CreateRow(0).CreateCell(1).SetCellValue("学生姓名");
            sheet.CreateRow(0).CreateCell(2).SetCellValue("课程编号");

            for (int i = 0; i < dtExperiment.Rows.Count; i++)
            {
                sheet.CreateRow(0).CreateCell(i + 3).SetCellValue(dtExperiment.Rows[i]["experimentResourceTitle"].ToString().Trim());
            }
            for (int j = 0; j < dtSchNotify.Rows.Count; j++)
            {
                sheet.CreateRow(0).CreateCell(j + 3 + dtExperiment.Rows.Count).SetCellValue(dtSchNotify.Rows[j]["schoolWorkNotifyTitle"].ToString().Trim());
            }

            //填充每列的数据
            string studentNo = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sheet.CreateRow(i + 1).CreateCell(0).SetCellValue(dt.Rows[i]["studentNo"].ToString().Trim());
                sheet.CreateRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["studentName"].ToString().Trim());
                sheet.CreateRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["courseNo"].ToString().Trim());

                studentNo = dt.Rows[i]["studentNo"].ToString().Trim();

                //局部变量，指明学生所有的实验数和作业数
                int studentExperimentsCount = 0;
                int studentSchoolWorksCount = 0;

                //添加实验列数据
                for (int j = 0; j < dtExp.Rows.Count; j++)
                {
                    if (studentNo.Equals(dtExp.Rows[j]["studentNo"].ToString().Trim()))
                    {
                        studentExperimentsCount += 1;
                        sheet.CreateRow(i + 1).CreateCell(2 + studentExperimentsCount).SetCellValue(dtExp.Rows[j]["score"].ToString().Trim());
                    }
                }
                //添加作业列数据

                for (int k = 0; k < dtSchwork.Rows.Count; k++)
                {
                    if (studentNo.Equals(dtSchwork.Rows[k]["studentNo"].ToString().Trim()))
                    {
                        studentSchoolWorksCount += 1;
                        sheet.CreateRow(i + 1).CreateCell(2 + studentSchoolWorksCount + studentExperimentsCount).SetCellValue(dtSchwork.Rows[k]["score"].ToString().Trim());
                    }
                }
            }

            IEnumerator rows = sheet.GetRowEnumerator();

            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;

                IEnumerator cols = row.GetCellEnumerator();

                while (cols.MoveNext())
                {
                    HSSFCell cell = (HSSFCell)cols.Current;

                    cell.CellStyle = cellStyle;
                }
            }

            string fileName = "成绩统计" + UploadFiles.DateTimeString();

            if (!Directory.Exists(context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ScoreExcelPath"])))
            {
                Directory.CreateDirectory(context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ScoreExcelPath"]));
            }

            System.IO.FileStream file = new System.IO.FileStream(HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ScoreExcelPath"] + fileName + ".xls"), System.IO.FileMode.Create);
            workbook.Write(file);
            file.Dispose();

            ////插入值
            FileInfo DownloadFile = new FileInfo(context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ScoreExcelPath"] + fileName + ".xls"));

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

            if (File.Exists(context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ScoreExcelPath"] + fileName + ".xls")))
            {
                File.Delete(context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ScoreExcelPath"] + fileName + ".xls"));
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