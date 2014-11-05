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
    /// OutputExamSeatArrange 的摘要说明
    /// </summary>
    public class OutputExamSeatArrange : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            if (context.Session["examSeatArrange_dt1"] == null || context.Session["examSeatArrange_dt2"] == null)
            {
                context.Response.Write("请先根据课程生成随机座位表：）");
                context.Response.End();
            }

            context.Response.CacheControl = "no-cache";

            string strRows = context.Request["rows"];
            string courseName = context.Server.UrlDecode(context.Request["courseName"]);

            string examSeatArrangeExcel = System.Configuration.ConfigurationManager.AppSettings["ExamSeatArrangeExcel"];

            //StringBuilder sb = new StringBuilder();


            HSSFWorkbook workbook = new HSSFWorkbook();

            //创建WorkSheet
            HSSFSheet sheet1 = workbook.CreateSheet(courseName + "_随机座位考试安排表");

            List<string> listStudent = new List<string>();

            DataTable dt1 = (DataTable)context.Session["examSeatArrange_dt1"];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                listStudent.Add(dt1.Rows[i]["studentNo"].ToString().Trim() + "_" + dt1.Rows[i]["studentName"].ToString().Trim());
            }

            DataTable dt2 = (DataTable)context.Session["examSeatArrange_dt2"];

            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                listStudent.Add(dt2.Rows[j]["studentNo"].ToString().Trim() + "_" + dt2.Rows[j]["studentName"].ToString().Trim());
            }

            //总学生人数
            int studentsNum = dt1.Rows.Count + dt2.Rows.Count;

            //座位行数
            int rows = int.Parse(strRows.Trim());
            //座位列数
            int cols = -1;

            if (studentsNum % rows == 0)
            {
                cols = studentsNum / rows;
            }
            else
            {
                cols = (studentsNum / rows) + 1;
            }

            for (int x = 0; x < rows; x++)
            {
                for (int k = 0; k < cols; k++)
                {

                    if (studentsNum > 0)
                    {
                        List<string> listStudentCopy = listStudent;
                        List<string> newList = new List<string>();

                        while (listStudentCopy.Count > 0)
                        {
                            Random random = new Random();
                            int _index = random.Next(listStudentCopy.Count);

                            newList.Insert(0, listStudentCopy[_index]);

                            listStudentCopy.RemoveAt(_index);
                        }

                        listStudent = newList;

                        int _rdm = GenerateRandom(studentsNum);

                        string _studentNo = listStudent[_rdm].Split("_".ToCharArray())[0];
                        string _studentName = listStudent[_rdm].Split("_".ToCharArray())[1];

                        HSSFRow _row = sheet1.CreateRow(x);
                        HSSFCell _cell = _row.CreateCell(k);
                        _cell.SetCellValue(_studentName + "(" + _studentNo + ")");

                        HSSFCellStyle cs = workbook.CreateCellStyle();

                        cs.WrapText = true;

                        _cell.CellStyle = cs;
                        _row.HeightInPoints = 2 * sheet1.DefaultRowHeight / 20;

                        int columnWidth = sheet1.GetColumnWidth(k) / 256;  
                        int length = Encoding.Default.GetBytes(_cell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                        sheet1.SetColumnWidth(k, columnWidth * 256);  

                        listStudent.RemoveAt(_rdm);

                        studentsNum--;
                    }
                }
            }

            string fileName = courseName + "_随机座位考试安排表_" + UploadFiles.DateTimeString();

            if (!Directory.Exists(context.Server.MapPath(examSeatArrangeExcel)))
            {
                Directory.CreateDirectory(context.Server.MapPath(examSeatArrangeExcel));
            }

            System.IO.FileStream file = new System.IO.FileStream(HttpContext.Current.Server.MapPath(examSeatArrangeExcel + fileName + ".xls"), System.IO.FileMode.Create);
            workbook.Write(file);
            file.Dispose();

            ////插入值
            FileInfo DownloadFile = new FileInfo(context.Server.MapPath(examSeatArrangeExcel + fileName + ".xls"));

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

            if (File.Exists(context.Server.MapPath(examSeatArrangeExcel + fileName + ".xls")))
            {
                File.Delete(context.Server.MapPath(examSeatArrangeExcel + fileName + ".xls"));
            }
            context.Response.Flush();
        }

        protected int GenerateRandom(int end)
        {
            Random rdm = new Random();
            return rdm.Next(end);
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