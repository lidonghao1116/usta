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

namespace USTA.WebApplication.Teacher
{
    /// <summary>
    /// OutPutSalaryEntryExcel 的摘要说明
    /// </summary>
    public class OutPutSalaryEntryExcel : IHttpHandler, IRequiresSessionState
    {
        private DalOperationAboutSalaryItem dalsi = new DalOperationAboutSalaryItem();
        private DalOperationAboutSalaryEntry dalse = new DalOperationAboutSalaryEntry();

        private string SearchTeacherIds(string keyWord)
        {
            CommonFunction.CheckUser();

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

        private List<SalaryEntry> QuerySalaryEntries(string teacherName, string termTag, string salaryMonth, int teacherType, int status)
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

            salaryMonth = salaryMonth != null && salaryMonth.Trim().Length > 0 ? salaryMonth.Trim() : null;

            

            List<SalaryEntry> salaryEntries = new List<SalaryEntry>();

            if (!(teacherName != null && (teacherNo == null || teacherNo.Trim().Length == 0)))
            {
                salaryEntries = dalse.GetSalaryEntrys(teacherNo, termTag, salaryMonth, teacherType, status);
            }

            return salaryEntries;

        }

        private List<SalaryItem> GetSalaryItem(int useFor)
        {
            
            List<SalaryItem> salaryItems = dalsi.GetAllSalaryItem(useFor, 0);

            return salaryItems;
        }


        private void SetTeacherSalary(HSSFSheet sheet, string teacherName, string termTag, string salaryMonth, int teacherType, int status)
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
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("不含税薪酬");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("含税薪酬");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("税后总薪酬");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("学期");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("月份");
            sheet.CreateRow(1).CreateCell(cIndex++).SetCellValue("状态");

            List<SalaryItem> salaryItems = GetSalaryItem(teacherType);
            int count = salaryItems.Count;
            for (var i = 0; i < count; i++)
            {
                sheet.CreateRow(1).CreateCell(cIndex + i).SetCellValue(salaryItems[i].salaryItemName + (salaryItems[i].hasTax ? "(含税)" : ""));
                inSalaryItemIndex.Add(salaryItems[i].salaryItemId.ToString(), cIndex + i);
            }

            for (var i = 0; i < count; i++)
            {
                sheet.CreateRow(1).CreateCell(cIndex + count + i).SetCellValue(salaryItems[i].salaryItemName);
                outSalaryItemIndex.Add(salaryItems[i].salaryItemId.ToString(), cIndex + i + count);
            }

            sheet.CreateRow(0).CreateCell(0).SetCellValue("基本信息");
            sheet.CreateRow(0).CreateCell(cIndex).SetCellValue("薪酬收入");
            if (count == 0)
            {
                sheet.CreateRow(0).CreateCell(cIndex + 1).SetCellValue("薪酬扣除");

            }
            else
            {
                sheet.CreateRow(0).CreateCell(cIndex + count).SetCellValue("薪酬扣除");

            }

            List<SalaryEntry> teacherSalaries = QuerySalaryEntries(teacherName, termTag, salaryMonth, teacherType, status);
            SalaryEntry salaryEntry;
            float totalSalaryWithTax = 0;
            float totalSalaryWithoutTax = 0;
            float totalSalary = 0;
            for (var i = 0; i < teacherSalaries.Count; i++)
            {
                cIndex = 0;
                salaryEntry = teacherSalaries[i];
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(salaryEntry.teacher.teacherName);
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(CommonUtility.ConvertTeacherType2String(salaryEntry.teacherType));
                if (teacherType != 1)
                {
                    sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(salaryEntry.course == null ? "" : salaryEntry.course.courseName);
                }

                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(salaryEntry.teacherCostWithoutTax);
                totalSalaryWithoutTax += salaryEntry.teacherCostWithoutTax;
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(salaryEntry.teacherCostWithTax);
                totalSalaryWithTax += salaryEntry.teacherCostWithTax;
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(salaryEntry.teacherTotalCost);
                totalSalary += salaryEntry.teacherTotalCost;
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(salaryEntry.termTag);
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(salaryEntry.salaryMonth);
                sheet.CreateRow(i + 2).CreateCell(cIndex++).SetCellValue(CommonUtility.ConvertSalaryEntryStatus(salaryEntry.salaryEntryStatus));

                List<SalaryItemElement> inItemElements = salaryEntry.GetSalaryInItemElements();
                List<SalaryItemElement> outItemElements = salaryEntry.GetSalaryOutItemElements();
                if (inItemElements != null)
                {
                    foreach (SalaryItemElement itemElement in inItemElements)
                    {
                        if(inSalaryItemIndex.Keys.Contains(itemElement.salaryItemId.ToString()))
                        {
                        sheet.CreateRow(i + 2).CreateCell(inSalaryItemIndex[itemElement.salaryItemId.ToString()]).SetCellValue(itemElement.itemCost);
                        }
                        
                    }
                }

                if (outItemElements != null)
                {
                    foreach (SalaryItemElement itemElement in outItemElements)
                    {
                        sheet.CreateRow(i + 2).CreateCell(outSalaryItemIndex[itemElement.salaryItemId.ToString()]).SetCellValue(itemElement.itemCost);
                    }
                }
            }
            cIndex = 2;
            sheet.CreateRow((cIndex++) + teacherSalaries.Count).CreateCell(0).SetCellValue("-----");
            sheet.CreateRow((cIndex++) + teacherSalaries.Count).CreateCell(0).SetCellValue("合计含税薪酬：" + totalSalaryWithTax);
            sheet.CreateRow((cIndex++) + teacherSalaries.Count).CreateCell(0).SetCellValue("合计不含税薪酬：" + totalSalaryWithoutTax);
            sheet.CreateRow((cIndex++) + teacherSalaries.Count).CreateCell(0).SetCellValue("总发放薪酬：" + totalSalary);
        }


        public void ProcessRequest(HttpContext context)
        {
            string teacherName = context.Request["tname"].Trim();
            string termTag = context.Request["termTag"].Trim();
            string salaryMonth = context.Request["month"].Trim();
            string teacherType = context.Request["teacherType"].Trim();
            string status = context.Request["status"].Trim();
            int salaryEntryStatus = 0;
            if (!string.IsNullOrWhiteSpace(status)) 
            {
                salaryEntryStatus = int.Parse(status);
            }

            HSSFWorkbook workbook = new HSSFWorkbook();

            if (teacherType == "0")
            {
                HSSFSheet sheet_Teacher = workbook.CreateSheet("院内教师(助教)月薪酬汇总");

                HSSFSheet sheet_OutTeacher = workbook.CreateSheet("院外教师月薪酬汇总");

                HSSFSheet sheet_OutAssistant = workbook.CreateSheet("院外助教月薪酬汇总");

                SetTeacherSalary(sheet_Teacher, teacherName, termTag, salaryMonth, 1, salaryEntryStatus);
                SetTeacherSalary(sheet_OutTeacher, teacherName, termTag, salaryMonth, 2, salaryEntryStatus);
                SetTeacherSalary(sheet_OutAssistant, teacherName, termTag, salaryMonth, 3, salaryEntryStatus);

            }
            else if (teacherType == "1")
            {
                HSSFSheet sheet_Teacher = workbook.CreateSheet("院内教师/助教月薪酬汇总");
                SetTeacherSalary(sheet_Teacher, teacherName, termTag, salaryMonth, 1, salaryEntryStatus);
            }
            else if (teacherType == "2")
            {
                HSSFSheet sheet_OutTeacher = workbook.CreateSheet("院外教师月薪酬汇总");
                SetTeacherSalary(sheet_OutTeacher, teacherName, termTag, salaryMonth, 2, salaryEntryStatus);
            }
            else if (teacherType == "3")
            {
                HSSFSheet sheet_OutAssistant = workbook.CreateSheet("院外助教月薪酬汇总");
                SetTeacherSalary(sheet_OutAssistant, teacherName, termTag, salaryMonth, 3, salaryEntryStatus);
            }


            string fileName = "教师月薪酬汇总";

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