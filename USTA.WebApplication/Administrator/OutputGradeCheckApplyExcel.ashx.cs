using System;
using System.Collections;
using System.Collections.Generic;
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

namespace USTA.WebApplication.Administrator
{
    /// <summary>
    /// OutputGradeCheckApply 的摘要说明
    /// </summary>
    public class OutputGradeCheckApply : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            context.Response.CacheControl = "no-cache";

            string termTagCourseNoClassID = context.Server.UrlDecode(context.Request["termTagCourseNoClassID"]);
            string termTag = (context.Request["termTag"] == null ? string.Empty : context.Request["termTag"]);
            string courseName = (context.Request["courseName"] == null ? string.Empty : context.Server.UrlDecode(context.Request["courseName"]));

            string gradeCheckApplyExcel = System.Configuration.ConfigurationManager.AppSettings["GradeCheckApplyExcel"];

            //StringBuilder sb = new StringBuilder();



            HSSFWorkbook workbook = new HSSFWorkbook();


            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            //// Response.Write(ddlCourses.SelectedValue);

            DataTable dt = dal.GetAllStudentGradeCheckApply(termTagCourseNoClassID, "all","all").Tables[0];

            //创建WorkSheet
            HSSFSheet sheet1 = workbook.CreateSheet(CommonUtility.ChangeTermToString(termTag) + "_" + courseName);


            sheet1.CreateRow(0).CreateCell(0).SetCellValue("序号");
            sheet1.CreateRow(0).CreateCell(1).SetCellValue("学号");
            sheet1.CreateRow(0).CreateCell(2).SetCellValue("姓名");
            sheet1.CreateRow(0).CreateCell(3).SetCellValue("专业");
            sheet1.CreateRow(0).CreateCell(4).SetCellValue("班级");
            sheet1.CreateRow(0).CreateCell(5).SetCellValue("手机号");
            sheet1.CreateRow(0).CreateCell(6).SetCellValue("邮件地址");
            sheet1.CreateRow(0).CreateCell(7).SetCellValue("所属学期");
            sheet1.CreateRow(0).CreateCell(8).SetCellValue("课程名称");
            sheet1.CreateRow(0).CreateCell(9).SetCellValue("重修重考类型");
            sheet1.CreateRow(0).CreateCell(10).SetCellValue("审核结果");
            sheet1.CreateRow(0).CreateCell(11).SetCellValue("备注");


            int drRowCount = dt.Rows.Count;

            for (int i = 0; i < drRowCount; i++)
            {
                sheet1.CreateRow(i + 1).CreateCell(0).SetCellValue(i + 1);
                sheet1.CreateRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["studentNo"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["studentName"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["studentSpeciality"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["SchoolClassName"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(5).SetCellValue(dt.Rows[i]["mobileNo"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["emailAddress"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(7).SetCellValue(CommonUtility.ChangeTermToString(termTag));
                sheet1.CreateRow(i + 1).CreateCell(8).SetCellValue(courseName);
                sheet1.CreateRow(i + 1).CreateCell(9).SetCellValue(dt.Rows[i]["gradeCheckApplyType"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(10).SetCellValue(dt.Rows[i]["applyResult"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(11).SetCellValue(dt.Rows[i]["applyChecKSuggestion"].ToString().Trim());
            }



            string fileName = CommonUtility.ChangeTermToString(termTag) + "_" + courseName + UploadFiles.DateTimeString();

            if (!Directory.Exists(context.Server.MapPath(gradeCheckApplyExcel)))
            {
                Directory.CreateDirectory(context.Server.MapPath(gradeCheckApplyExcel));
            }

            System.IO.FileStream file = new System.IO.FileStream(HttpContext.Current.Server.MapPath(gradeCheckApplyExcel + fileName + ".xls"), System.IO.FileMode.Create);
            workbook.Write(file);
            file.Dispose();

            ////插入值
            FileInfo DownloadFile = new FileInfo(context.Server.MapPath(gradeCheckApplyExcel + fileName + ".xls"));

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

            if (File.Exists(context.Server.MapPath(gradeCheckApplyExcel + fileName + ".xls")))
            {
                File.Delete(context.Server.MapPath(gradeCheckApplyExcel + fileName + ".xls"));
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