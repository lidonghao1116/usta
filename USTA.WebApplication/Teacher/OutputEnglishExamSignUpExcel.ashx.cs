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
    /// OutputEnglishExamSignUpExcel 的摘要说明
    /// </summary>
    public class OutputEnglishExamSignUpExcel : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            CommonFunction.CheckUser();

            context.Response.CacheControl = "no-cache";

            string englishExamNotifyId = context.Request["englishExamNotifyId"];

            string useSchoolClassId = context.Request["useSchoolClassId"];

            string schoolClassId = context.Request["schoolClassId"].Trim();

            string schoolClassName = string.Empty;
            string englishExamNotifyTitle = string.Empty;

            string englishExamSignUpExcel = System.Configuration.ConfigurationManager.AppSettings["EnglishExamSignUpExcel"];

            //StringBuilder sb = new StringBuilder();

            using (FileStream file1 = new FileStream(HttpContext.Current.Server.MapPath(englishExamSignUpExcel) + "template.xls", FileMode.Open))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(file1);

                DalOperationAboutEnglishExam dal = new DalOperationAboutEnglishExam();
                UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

                DataSet ds = dal.GetEnglishExamSignUpInfoByTeacherNo(user.userNo, false, schoolClassId, false, string.Empty, useSchoolClassId != null);
                DataTable dt = ds.Tables[0];
                DataRow[] dr1 = dt.Select("examType='四级'");
                DataRow[] dr2 = dt.Select("examType='六级'");

                //创建WorkSheet
                HSSFSheet sheet1 = workbook.GetSheet("四级");
                HSSFSheet sheet2 = workbook.GetSheet("六级");
                string[] cardTypeCollection = System.Configuration.ConfigurationManager.AppSettings["cardType"].Split(",".ToCharArray());

                int dr1RowCount = dr1.Length;

                for (int i = 0; i < dr1RowCount; i++)
                {
                    int cardType = -1;

                    for (int x = 0; x < cardTypeCollection.Length; x++)
                    {
                        if (cardTypeCollection[x].IndexOf(dr1[i]["cardType"].ToString().Trim()) != -1)
                        {
                            cardType = int.Parse(cardTypeCollection[x].Split("|".ToCharArray())[1]);
                        }
                    }

                    sheet1.CreateRow(i + 3).CreateCell(0).SetCellValue(i + 1);
                    sheet1.CreateRow(i + 3).CreateCell(1).SetCellValue(dr1[i]["studentName"].ToString().Trim());
                    sheet1.CreateRow(i + 3).CreateCell(2).SetCellValue(dr1[i]["sex"].ToString().Trim() == "0" ? "男" : "女");
                    sheet1.CreateRow(i + 3).CreateCell(3).SetCellValue(dr1[i]["studentNo"].ToString().Trim());
                    sheet1.CreateRow(i + 3).CreateCell(4).SetCellValue(cardType.ToString().Trim());
                    sheet1.CreateRow(i + 3).CreateCell(5).SetCellValue(dr1[i]["cardNum"].ToString().Trim());
                    sheet1.CreateRow(i + 3).CreateCell(6).SetCellValue(string.Empty);
                    sheet1.CreateRow(i + 3).CreateCell(7).SetCellValue(string.Empty);
                    sheet1.CreateRow(i + 3).CreateCell(8).SetCellValue(dr1[i]["studentNo"].ToString().Trim().Substring(2, 2));



                    string studentGrade = string.Empty;

                    if (DateTime.Now.Month >= 9)
                    {
                        studentGrade = "0" + (int.Parse(DateTime.Now.Year.ToString().Trim().Substring(2, 2)) - int.Parse(dr1[i]["studentNo"].ToString().Trim().Substring(2, 2)) + 1);
                    }
                    else
                    {
                        studentGrade = "0" + (int.Parse(DateTime.Now.Year.ToString().Trim().Substring(2, 2)) - int.Parse(dr1[i]["studentNo"].ToString().Trim().Substring(2, 2)));
                    }
                    sheet1.CreateRow(i + 3).CreateCell(9).SetCellValue(studentGrade);
                    sheet1.CreateRow(i + 3).CreateCell(10).SetCellValue("软件学院");
                    sheet1.CreateRow(i + 3).CreateCell(11).SetCellValue(dr1[i]["SchoolClassName"].ToString().Trim());
                    sheet1.CreateRow(i + 3).CreateCell(12).SetCellValue(dr1[i]["studentSpeciality"].ToString().Trim());
                    sheet1.CreateRow(i + 3).CreateCell(13).SetCellValue("四级");
                    sheet1.CreateRow(i + 3).CreateCell(14).SetCellValue(dr1[i]["examPlace"].ToString().Trim());

                    schoolClassName = dr1[i]["SchoolClassName"].ToString().Trim();
                    englishExamNotifyTitle = dr1[i]["englishExamNotifyTitle"].ToString().Trim();
                }


                int dr2RowCount = dr2.Length;

                for (int i = 0; i < dr2RowCount; i++)
                {
                    int cardType = -1;


                    for (int x = 0; x < cardTypeCollection.Length; x++)
                    {
                        if (cardTypeCollection[x].IndexOf(dr2[i]["cardType"].ToString().Trim()) != -1)
                        {
                            cardType = int.Parse(cardTypeCollection[x].Split("|".ToCharArray())[1]);
                        }
                    }

                    sheet2.CreateRow(i + 3).CreateCell(0).SetCellValue(i + 1);
                    sheet2.CreateRow(i + 3).CreateCell(1).SetCellValue(dr2[i]["studentName"].ToString().Trim());
                    sheet2.CreateRow(i + 3).CreateCell(2).SetCellValue(dr2[i]["sex"].ToString().Trim() == "0" ? "男" : "女");
                    sheet2.CreateRow(i + 3).CreateCell(3).SetCellValue(dr2[i]["studentNo"].ToString().Trim());
                    sheet2.CreateRow(i + 3).CreateCell(4).SetCellValue(cardType.ToString().Trim());
                    sheet2.CreateRow(i + 3).CreateCell(5).SetCellValue(dr2[i]["cardNum"].ToString().Trim());
                    sheet2.CreateRow(i + 3).CreateCell(6).SetCellValue(string.Empty);
                    sheet2.CreateRow(i + 3).CreateCell(7).SetCellValue(string.Empty);
                    sheet2.CreateRow(i + 3).CreateCell(8).SetCellValue(dr2[i]["studentNo"].ToString().Trim().Substring(2, 2));

                    string studentGrade = string.Empty;
                    if (DateTime.Now.Month >= 9)
                    {
                        studentGrade = "0" + (int.Parse(DateTime.Now.Year.ToString().Trim().Substring(2, 2)) - int.Parse(dr2[i]["studentNo"].ToString().Trim().Substring(2, 2)) + 1);
                    }
                    else
                    {
                        studentGrade = "0" + (int.Parse(DateTime.Now.Year.ToString().Trim().Substring(2, 2)) - int.Parse(dr2[i]["studentNo"].ToString().Trim().Substring(2, 2)));
                    }

                    sheet2.CreateRow(i + 3).CreateCell(9).SetCellValue(studentGrade);
                    sheet2.CreateRow(i + 3).CreateCell(10).SetCellValue("软件学院");
                    sheet2.CreateRow(i + 3).CreateCell(11).SetCellValue(dr2[i]["SchoolClassName"].ToString().Trim());
                    sheet2.CreateRow(i + 3).CreateCell(12).SetCellValue(dr2[i]["studentSpeciality"].ToString().Trim());
                    sheet2.CreateRow(i + 3).CreateCell(13).SetCellValue("六级");
                    sheet2.CreateRow(i + 3).CreateCell(14).SetCellValue(dr2[i]["examPlace"].ToString().Trim());

                    schoolClassName = dr2[i]["SchoolClassName"].ToString().Trim();
                    englishExamNotifyTitle = dr2[i]["englishExamNotifyTitle"].ToString().Trim();
                }


                string fileName = englishExamNotifyTitle + schoolClassName + "四六级报名数据汇总" + UploadFiles.DateTimeString();

                if (!Directory.Exists(context.Server.MapPath(englishExamSignUpExcel)))
                {
                    Directory.CreateDirectory(englishExamSignUpExcel);
                }

                System.IO.FileStream file = new System.IO.FileStream(HttpContext.Current.Server.MapPath(englishExamSignUpExcel + fileName + ".xls"), System.IO.FileMode.Create);
                workbook.Write(file);
                file.Dispose();

                ////插入值
                FileInfo DownloadFile = new FileInfo(context.Server.MapPath(englishExamSignUpExcel + fileName + ".xls"));

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

                if (File.Exists(context.Server.MapPath(englishExamSignUpExcel + fileName + ".xls")))
                {
                    File.Delete(context.Server.MapPath(englishExamSignUpExcel + fileName + ".xls"));
                }
                context.Response.Flush();
            }
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