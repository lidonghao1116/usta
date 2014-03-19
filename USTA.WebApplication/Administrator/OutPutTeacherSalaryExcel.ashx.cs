using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

using NPOI.HSSF.UserModel;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    /// <summary>
    /// OutPutTeacherSalaryExcel 的摘要说明
    /// </summary>
    public class OutPutTeacherSalaryExcel : IHttpHandler, IRequiresSessionState
    {
        private string SearchTeacherIds(string keyWord)
        {

            DalOperationUsers dos = new DalOperationUsers();
            DataView dv = dos.SearchUserByTypeAndKeywod(1, keyWord).DefaultView;
            DataRowCollection drCollection = dv.Table.Rows;
            List<string> tidList = new List<string>();

            for (var i = 0; i < drCollection.Count; i++)
            {
                string s = drCollection[i]["teacherNo"].ToString();
                tidList.Add("'" + s + "'");
            }

            string tids = string.Join(",", tidList.ToArray());
            return tids;
        }

        private List<TeacherSalary> QueryTeacherSalaries(string teacherName, string termTag, int teacherType, int status)
        {
            string teacherNo = null;
            if (teacherName != null && teacherName.Trim().Length > 0)
            {
                teacherName = teacherName.Trim();
                teacherNo = SearchTeacherIds(teacherName);
                if (teacherNo == null || teacherNo.Trim().Length == 0)
                {
                    teacherNo = null;
                }
            }
            else
            {
                teacherName = null;
            }

            if (termTag == null || termTag.Trim().Length == 0 || termTag.Trim() == "all")
            {
                termTag = null;
            }

            DalOperationAboutTeacherSalary dalts = new DalOperationAboutTeacherSalary();

            List<TeacherSalary> teacherSalaries = new List<TeacherSalary>();

            if (!(teacherName != null && (teacherNo == null || teacherNo.Trim().Length == 0)))
            {
                teacherSalaries = dalts.GetTeacherSalarys(teacherNo, teacherType, termTag, status);
            }

            return teacherSalaries;

        }

        private List<SalaryItem> GetSalaryItem(int useFor)
        {
            DalOperationAboutSalaryItem dalsi = new DalOperationAboutSalaryItem();
            List<SalaryItem> salaryItems = dalsi.GetAllSalaryItem(useFor, 0);

            return salaryItems;
        }


        private void SetTeacherSalary(HSSFSheet sheet, string teacherName, string termTag, int teacherType, int status)
        {
            Dictionary<string, int> inSalaryItemIndex = new Dictionary<string, int>();
            Dictionary<string, int> outSalaryItemIndex = new Dictionary<string, int>();
            int cIndex = 0;
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("姓名");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("类型");
            if (teacherType != 1)
            {
                sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("课程");
            }
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("薪酬预算");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("学期");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("状态");

            List<SalaryItem> salaryItems = GetSalaryItem(teacherType);
            int count = salaryItems.Count;
            for (var i = 0; i < count; i++)
            {
                sheet.CreateRow(1).CreateCell(cIndex + i).SetCellValue(salaryItems[i].salaryItemName);
                inSalaryItemIndex.Add(salaryItems[i].salaryItemId.ToString(), cIndex + i);
            }

            sheet.CreateRow(0).CreateCell(0).SetCellValue("基本信息");
            sheet.CreateRow(0).CreateCell(cIndex).SetCellValue("薪酬预算");


            List<TeacherSalary> teacherSalaries = QueryTeacherSalaries(teacherName, termTag, teacherType, status);
            TeacherSalary teacherSalary;
            double totalTeachCost = 0;
            for (var i = 0; i < teacherSalaries.Count; i++)
            {
                cIndex = 0;
                teacherSalary = teacherSalaries[i];
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(teacherSalary.teacher.teacherName);
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(CommonUtility.ConvertTeacherType2String(teacherSalary.teacherType));
                if (teacherType != 1)
                {
                    sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(teacherSalary.course == null ? "" : teacherSalary.course.courseName);
                }

                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(teacherSalary.totalTeachCost);
                totalTeachCost += teacherSalary.totalTeachCost;
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(teacherSalary.termTag);
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(teacherSalary.isConfirm ? "已确认" : "未确认");

                List<SalaryItemElement> inItemElements = teacherSalary.GetSalaryInItemElements();

                if (inItemElements != null)
                {
                    foreach (SalaryItemElement itemElement in inItemElements)
                    {
                        sheet.CreateRow(i + 2).CreateCell(inSalaryItemIndex[itemElement.salaryItemId.ToString()]).SetCellValue(itemElement.itemCost);
                    }
                }
            }
            cIndex = 2;
            sheet.CreateRow((cIndex++) + teacherSalaries.Count).CreateCell(0).SetCellValue("-----");
            sheet.CreateRow((cIndex++) + teacherSalaries.Count).CreateCell(0).SetCellValue("合计薪酬金额：" + totalTeachCost);
        }


        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            string teacherName = context.Request["tname"].Trim();
            string termTag = context.Request["termTag"].Trim();
            string teacherType = context.Request["teacherType"].Trim();
            string status = context.Request["status"].Trim();

            HSSFWorkbook workbook = new HSSFWorkbook();

            if (string.IsNullOrWhiteSpace(status)) 
            {
                status = "0";
            }

            if (teacherType == "0")
            {
                HSSFSheet sheet_Teacher = workbook.CreateSheet("院内教师(助教)薪酬预算汇总");

                HSSFSheet sheet_OutTeacher = workbook.CreateSheet("院外教师薪酬预算汇总");

                HSSFSheet sheet_OutAssistant = workbook.CreateSheet("院外助教薪酬预算汇总");

                SetTeacherSalary(sheet_Teacher, teacherName, termTag, 1, int.Parse(status));
                SetTeacherSalary(sheet_OutTeacher, teacherName, termTag, 2, int.Parse(status));
                SetTeacherSalary(sheet_OutAssistant, teacherName, termTag, 3, int.Parse(status));

            }
            else if (teacherType == "1")
            {
                HSSFSheet sheet_Teacher = workbook.CreateSheet("院内教师(助教)薪酬预算汇总");
                SetTeacherSalary(sheet_Teacher, teacherName, termTag, 1, int.Parse(status));
            }
            else if (teacherType == "2")
            {
                HSSFSheet sheet_OutTeacher = workbook.CreateSheet("院外教师薪酬预算汇总");
                SetTeacherSalary(sheet_OutTeacher, teacherName, termTag, 2, int.Parse(status));
            }
            else if (teacherType == "3")
            {
                HSSFSheet sheet_OutAssistant = workbook.CreateSheet("院外助教薪酬预算汇总");
                SetTeacherSalary(sheet_OutAssistant, teacherName, termTag, 3, int.Parse(status));
            }


            string fileName = "教师薪酬预算汇总";

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