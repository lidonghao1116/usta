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
    /// OutputGradeCheckExcel 的摘要说明
    /// </summary>
    public class OutputGradeCheckExcel : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            context.Response.CacheControl = "no-cache";

            string termYear = context.Request["year"];

            string locale = context.Request["locale"];

            string allGradeCheckExcel = System.Configuration.ConfigurationManager.AppSettings["AllGradeCheckExcel"];

            //StringBuilder sb = new StringBuilder();



            HSSFWorkbook workbook = new HSSFWorkbook();

            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            DataSet ds = (locale == "all" ? dal.GetAllStudentsDataAboutGradeCheckData(termYear) : dal.GetAllStudentsDataAboutGradeCheckDataByLocale(termYear, locale));

            DataTable dt = ds.Tables[0];

            //创建WorkSheet
            HSSFSheet sheet1 = workbook.CreateSheet(termYear + "级成绩审核汇总表");


            sheet1.CreateRow(0).CreateCell(0).SetCellValue("序号");
            sheet1.CreateRow(0).CreateCell(1).SetCellValue("学号");
            sheet1.CreateRow(0).CreateCell(2).SetCellValue("姓名");
            sheet1.CreateRow(0).CreateCell(3).SetCellValue("性别");
            sheet1.CreateRow(0).CreateCell(4).SetCellValue("班级");
            sheet1.CreateRow(0).CreateCell(5).SetCellValue("年级");
            sheet1.CreateRow(0).CreateCell(6).SetCellValue("班主任");


            int drRowCount = dt.Rows.Count;

            for (int i = 0; i < drRowCount; i++)
            {
                sheet1.CreateRow(i + 1).CreateCell(0).SetCellValue(i + 1);
                sheet1.CreateRow(i + 1).CreateCell(1).SetCellValue(dt.Rows[i]["studentNo"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(2).SetCellValue(dt.Rows[i]["studentName"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(3).SetCellValue(dt.Rows[i]["Sex"].ToString().Trim() == "1" ? "男" : "女");
                sheet1.CreateRow(i + 1).CreateCell(4).SetCellValue(dt.Rows[i]["SchoolClassName"].ToString().Trim());
                sheet1.CreateRow(i + 1).CreateCell(5).SetCellValue(termYear + "级");
                sheet1.CreateRow(i + 1).CreateCell(6).SetCellValue(dt.Rows[i]["HeadteacherName"].ToString().Trim());
            }

            DataSet ds1 = dal.GetGradeCheckItemsByTermYear(termYear);

            int columnCount = ds1.Tables[0].Rows.Count;

            //保存列名相关信息
            List<int> listGradeCheckId = new List<int>();
            Hashtable ht = new Hashtable();

            for (int i = 0; i < columnCount; i++)
            {
                sheet1.CreateRow(0).CreateCell(7 + i).SetCellValue(ds1.Tables[0].Rows[i]["gradeCheckItemName"].ToString().Trim());
                listGradeCheckId.Add(int.Parse(ds1.Tables[0].Rows[i]["gradeCheckId"].ToString().Trim()));

                ht.Add(ds1.Tables[0].Rows[i]["gradeCheckId"].ToString().Trim(), ds1.Tables[0].Rows[i]["gradeCheckItemDefaultValue"].ToString().Trim());
            }


            for (int i = 0; i < columnCount; i++)
            {

                for (int j = 0; j < drRowCount; j++)
                {
                    //context.Response.Write(listGradeCheckId[i] + "<br/>");
                    DataSet dsTemp = dal.GetAllGradeCheckDataAboutStudentsData(dt.Rows[j]["studentNo"].ToString().Trim(), listGradeCheckId[i]);

                    if (dsTemp == null)
                    {
                        sheet1.CreateRow(j + 1).CreateCell(7 + i).SetCellValue(ht[listGradeCheckId[i].ToString().Trim()].ToString());
                    }
                    else if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        sheet1.CreateRow(j + 1).CreateCell(7 + i).SetCellValue(dsTemp.Tables[0].Rows[0]["gradeCheckDetailValue"].ToString().Trim());
                    }
                }
            }

            //context.Response.End();

            sheet1.CreateRow(0).CreateCell(7 + columnCount).SetCellValue("是否符合学位申请条件");

            sheet1.CreateRow(0).CreateCell(8 + columnCount).SetCellValue("不及格科目（备注）");

            sheet1.CreateRow(0).CreateCell(9 + columnCount).SetCellValue("培养地");

            for (int i = 0; i < drRowCount; i++)
            {
                sheet1.CreateRow(i + 1).CreateCell(9 + columnCount).SetCellValue(dt.Rows[i]["locale"].ToString().Trim());
            }



            for (int j = 0; j < drRowCount; j++)
            {
                //context.Response.Write(listGradeCheckId[i] + "<br/>");
                DataSet dsTemp = dal.GetAllGradeCheckDataAboutConfirmAndRemarkData(dt.Rows[j]["studentNo"].ToString().Trim());

                //context.Response.Write(dt.Rows[j]["studentNo"].ToString().Trim());
                //context.Response.End();
                if (dsTemp == null)
                {
                    sheet1.CreateRow(j + 1).CreateCell(7 + columnCount).SetCellValue(string.Empty);

                    sheet1.CreateRow(j + 1).CreateCell(8 + columnCount).SetCellValue(string.Empty);
                }
                else
                {
                    sheet1.CreateRow(j + 1).CreateCell(7 + columnCount).SetCellValue(dsTemp.Tables[0].Rows[0]["isAccord"].ToString().Trim() == "1" ? "符合" : "不符合");

                    sheet1.CreateRow(j + 1).CreateCell(8 + columnCount).SetCellValue(dsTemp.Tables[0].Rows[0]["remark"].ToString().Trim());
                }
            }



            string fileName = termYear + "级成绩审核汇总表_" + UploadFiles.DateTimeString();

            if (!Directory.Exists(context.Server.MapPath(allGradeCheckExcel)))
            {
                Directory.CreateDirectory(context.Server.MapPath(allGradeCheckExcel));
            }

            System.IO.FileStream file = new System.IO.FileStream(HttpContext.Current.Server.MapPath(allGradeCheckExcel + fileName + ".xls"), System.IO.FileMode.Create);
            workbook.Write(file);
            file.Dispose();

            ////插入值
            FileInfo DownloadFile = new FileInfo(context.Server.MapPath(allGradeCheckExcel + fileName + ".xls"));

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

            if (File.Exists(context.Server.MapPath(allGradeCheckExcel + fileName + ".xls")))
            {
                File.Delete(context.Server.MapPath(allGradeCheckExcel + fileName + ".xls"));
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