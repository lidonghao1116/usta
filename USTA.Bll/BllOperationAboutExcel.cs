using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace USTA.Bll
{
	using NPOI.HSSF.UserModel;
	using USTA.Common;
	using USTA.Model;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	/// <summary>
	/// Excel操作业务类
	/// </summary>
	public sealed class BllOperationAboutExcel
	{
		#region 全局变量及构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public BllOperationAboutExcel()
		{


		}
		#endregion

		#region 判断导入的Excel当前读取的是哪个工作薄并返回相应的Sql查询参数
		/// <summary>
		/// 判断导入的Excel当前读取的是哪个工作薄并返回相应的Sql查询参数
		/// </summary>
		/// <param name="sheetName">工作薄名称</param>
		/// <param name="modelClassCorrelationSheet">将具体的实体类存入object中</param>
		/// <returns></returns>
		public static ExcelSheetData ReturnSqlJudgeBySheetName(string sheetName, object modelClassCorrelationSheet)
		{
			ExcelSheetData ExcelSheetData = new ExcelSheetData();
			SqlParameter[] parameters;
			switch (sheetName)
			{
				case "Sheet1":
					parameters = new SqlParameter[6] {
					new SqlParameter("@teacherNo", ((TeachersList)modelClassCorrelationSheet).teacherNo),
					new SqlParameter("@teacherUserPwd", ((TeachersList)modelClassCorrelationSheet).teacherUserPwd),
					new SqlParameter("@teacherName", ((TeachersList)modelClassCorrelationSheet).teacherName),
					new SqlParameter("@emailAddress", ((TeachersList)modelClassCorrelationSheet).emailAddress),
					new SqlParameter("@officeAddress",((TeachersList)modelClassCorrelationSheet).officeAddress),
					new SqlParameter("@remark", ((TeachersList)modelClassCorrelationSheet).remark)};
					ExcelSheetData.spName = "spTeachersListAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;

				case "Sheet2":
					parameters = new SqlParameter[6] {
						new SqlParameter("@assistantNo", ((AssistantsList)modelClassCorrelationSheet).assistantNo),
					new SqlParameter("@assistantUserPwd", ((AssistantsList)modelClassCorrelationSheet).assistantUserPwd),
					new SqlParameter("@assistantName", ((AssistantsList)modelClassCorrelationSheet).assistantName),
					new SqlParameter("@emailAddress", ((AssistantsList)modelClassCorrelationSheet).emailAddress),
					new SqlParameter("@officeAddress", ((AssistantsList)modelClassCorrelationSheet).officeAddress),
					new SqlParameter("@remark",((AssistantsList)modelClassCorrelationSheet).remark)};
					ExcelSheetData.spName = "spAssistantsListAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;
				case "Sheet3":
					parameters = new SqlParameter[8] {
					new SqlParameter("@studentNo", ((StudentsList)modelClassCorrelationSheet).studentNo),
					new SqlParameter("@studentName", ((StudentsList)modelClassCorrelationSheet).studentName),
					new SqlParameter("@studentUserPwd", ((StudentsList)modelClassCorrelationSheet).studentUserPwd),
					new SqlParameter("@studentSpeciality", ((StudentsList)modelClassCorrelationSheet).studentSpeciality),
					new SqlParameter("@classNo", ((StudentsList)modelClassCorrelationSheet).classNo),
					new SqlParameter("@mobileNo", ((StudentsList)modelClassCorrelationSheet).mobileNo),
					new SqlParameter("@emailAddress", ((StudentsList)modelClassCorrelationSheet).emailAddress),
					new SqlParameter("@remark", ((StudentsList)modelClassCorrelationSheet).remark)
					};
					ExcelSheetData.spName = "spStudentsListAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;

				case "Sheet4":
					parameters = new SqlParameter[1]{
					new SqlParameter("@termTag", ((TermTags)modelClassCorrelationSheet).termTag)};
					ExcelSheetData.spName = "spTermTagsAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;

				case "Sheet5":
					parameters = new SqlParameter[9] {
					new SqlParameter("@courseNo", ((Courses)modelClassCorrelationSheet).courseNo),
					new SqlParameter("@courseName", ((Courses)modelClassCorrelationSheet).courseName),
					new SqlParameter("@period", ((Courses)modelClassCorrelationSheet).period),
					new SqlParameter("@credit", ((Courses)modelClassCorrelationSheet).credit),
					new SqlParameter("@courseSpeciality",((Courses)modelClassCorrelationSheet).courseSpeciality),
					new SqlParameter("@preCourse", ((Courses)modelClassCorrelationSheet).preCourse),
					new SqlParameter("@refferenceBooks", ((Courses)modelClassCorrelationSheet).referenceBooks),
					new SqlParameter("@termTag", ((Courses)modelClassCorrelationSheet).termTag),
					new SqlParameter("@attachmentIds", ((Courses)modelClassCorrelationSheet).attachmentIds)
					};
					ExcelSheetData.spName = "spCoursesAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;

				case "Sheet6":
					parameters = new SqlParameter[2] {
					new SqlParameter("@teacherNo", ((CoursesTeachersCorrelation)modelClassCorrelationSheet).teacherNo),
					new SqlParameter("@courseNo", ((CoursesTeachersCorrelation)modelClassCorrelationSheet).courseNo)};
					ExcelSheetData.spName = "spCoursesTeachersCorrelationAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;

				case "Sheet7":
					parameters = new SqlParameter[2] {
					new SqlParameter("@assistantNo", ((CoursesAssistantsCorrelation)modelClassCorrelationSheet).assistantNo),
					new SqlParameter("@courseNo", ((CoursesAssistantsCorrelation)modelClassCorrelationSheet).courseNo)};
					ExcelSheetData.spName = "spCoursesAssistantsCorrelationAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;

				case "Sheet8":
					parameters = new SqlParameter[2] {
					new SqlParameter("@studentNo", ((CoursesStudentsCorrelation)modelClassCorrelationSheet).studentNo),
					new SqlParameter("@courseNo", ((CoursesStudentsCorrelation)modelClassCorrelationSheet).courseNo)};
					ExcelSheetData.spName = "spCoursesStudentsCorrelationAdd";
					ExcelSheetData.sqlParammeters = parameters;
					break;

				default:
					break;
			}
			return ExcelSheetData;

		}
		#endregion

		#region 根据工作薄的名称判断进行对应实体类数据的封装
		/// <summary>
		/// 根据工作薄的名称判断进行对应实体类数据的封装
		/// </summary>
		/// <param name="sheetName">工作薄名称</param>
		/// <param name="list">具体实体类项目的集合</param>
		/// <returns>返回封装后的实体类数据</returns>
		public static object ReturnModelDataJudgeBySheetName(string sheetName, List<string> list)
		{
			object modelClassCorrelationSheet = null;
			switch (sheetName)
			{
				case "Sheet1":
					modelClassCorrelationSheet =
						new TeachersList
						{
							teacherNo = list[0],
							teacherName = list[1],
							emailAddress = list[2],
							officeAddress = list[3],
							remark = list[4],
							teacherUserPwd = list[5]
						};
					break;
				case "Sheet2":
					modelClassCorrelationSheet = new AssistantsList
					{
						assistantNo = list[0],
						assistantName = list[1],
						emailAddress = list[2],
						officeAddress = list[3],
						remark = list[4],
						assistantUserPwd = list[5]
					};
					break;
				case "Sheet3":
					modelClassCorrelationSheet = new StudentsList
					{
						studentNo = list[0],
						studentName = list[1],
						studentSpeciality = list[2],
						classNo = list[3],
						mobileNo = list[4],
						emailAddress = list[5],
						remark = list[6],
						studentUserPwd = list[7]
					};
					break;

				case "Sheet4":
					modelClassCorrelationSheet = new TermTags
					{
						termTag = list[0]
					};
					break;

				case "Sheet5":
					modelClassCorrelationSheet = new Courses
					{
						courseNo = list[0],
						courseName = list[1],
						period = list[2],
						credit = float.Parse(list[3]),
						courseSpeciality = list[4],
						termTag = list[5],
						preCourse = string.Empty,
						referenceBooks = string.Empty,
						attachmentIds = string.Empty
					};
					break;

				case "Sheet6":
					modelClassCorrelationSheet = new CoursesTeachersCorrelation
					{
						teacherNo = list[0],
						courseNo = list[1]
					};
					break;

				case "Sheet7":
					modelClassCorrelationSheet = new CoursesAssistantsCorrelation
					{
						assistantNo = list[0],
						courseNo = list[1]
					};
					break;

				case "Sheet8":
					modelClassCorrelationSheet = new CoursesStudentsCorrelation
					{
						studentNo = list[0],
						courseNo = list[1]
					};
					break;


				default:
					break;
			}
			return modelClassCorrelationSheet;
		}
		#endregion

		#region 创建初始密码Excel表，由于目前使用的是将初始密码存入数据库的功能，所以暂时不使用
		/// <summary>
		/// 创建初始密码Excel表，由于目前使用的是将初始密码存入数据库的功能，所以暂时不使用
		/// </summary>
		/// <param name="excelPasswordMapping">密码映射实体类集合</param>
		public static void CreateExcelAboutPasswordMapping(List<PasswordMapping> excelPasswordMapping)
		{
			HSSFWorkbook workbook = new HSSFWorkbook();
			//创建WorkSheet
			HSSFSheet sheet = workbook.CreateSheet("初始密码表");
			//插入值

			int rowCount = excelPasswordMapping.Count;
			int colCount = 4;

			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					switch (j)
					{
						case 0:
							sheet.CreateRow(i).CreateCell(j).SetCellValue(excelPasswordMapping[i].userNo);
							continue;
						case 1:
							sheet.CreateRow(i).CreateCell(j).SetCellValue(excelPasswordMapping[i].userName);
							continue;
						case 2:
							sheet.CreateRow(i).CreateCell(j).SetCellValue(excelPasswordMapping[i].initializePassword);
							continue;
						default:
							continue;
					}
				}
			}

			FileStream file = new FileStream(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["InitializePasswordExcelPath"] + Guid.NewGuid().ToString() + ".xls"), FileMode.Create);
			workbook.Write(file);
			file.Dispose();

		}

		#endregion

		#region 读取Excel数据并返回List<ExcelSheetData>数组类型给Dal进行处理
		/// <summary>
		/// 读取Excel数据并返回ExcelSheetData类型的集合给Dal进行处理
		/// </summary>
		/// <param name="filePath">Excel文件路径</param>
		/// <param name="colsCount">记录列数</param>
		/// <param name="FileUpload1"></param>
		/// <returns>返回ExcelData实体类</returns>
		public static ExcelData BllImportExcelData(string filePath, int[] colsCount, FileUpload FileUpload1)
		{
			//设定一个自定义ExcelData类型，用于返回全部的Excel数据
			ExcelData excelData = null;

			//出错的工作薄名称
			string exceptionSheetName = string.Empty;

			//出错的行号
			int exceptionRowNo = 0;

			////出错的列号
			//int exceptionColNo = 0;

			try
			{
				List<ExcelSheetData> excelSheetDataArray = new List<ExcelSheetData>();

				List<PasswordMapping> excelPasswordMapping = new List<PasswordMapping>();

				int sheetCount = 0;

				using (FileStream file = new FileStream(filePath, FileMode.Open))
				{
					//建立WorkBook
					HSSFWorkbook HSSFWorkbook = new HSSFWorkbook(file);
					//获取工作薄的数目
					sheetCount = HSSFWorkbook.NumberOfSheets;

					//循环存储值
					for (int i = 0; i < sheetCount; i++)
					{

						HSSFSheet Sheet = HSSFWorkbook.GetSheetAt(i);

						string sheetName = HSSFWorkbook.GetSheetName(i);

						//设置当前的工作薄名称
						exceptionSheetName = sheetName;

						IEnumerator rows = Sheet.GetRowEnumerator();

						//当前行编号
						int currentRowNo = 0;

						while (rows.MoveNext())
						{
							if (currentRowNo > 0)
							{
								exceptionRowNo = currentRowNo;

								HSSFRow row = (HSSFRow)rows.Current;

								List<string> ilistTemp = new List<string>();

								//初始密码（4位数字）
								string initializePwd = CommonUtility.GenerateRandomPassword();

								for (int j = 0; j < colsCount[i]; j++)
								{
									//exceptionColNo = j + 1;

									ilistTemp.Add((row.GetCell(j) != null ? row.GetCell(j).ToString().Trim() : string.Empty));

									if (j == (colsCount[i] - 1))
									{
										//try
										//{
										//实例化一个初始密码映射实体类
										PasswordMapping passwordMapping = null;

										switch (sheetName)
										{
											case "Sheet1":
												passwordMapping = new PasswordMapping
												{
													userNo = row.GetCell(0).ToString(),
													userName = row.GetCell(1).ToString(),
													initializePassword = initializePwd
												};
												passwordMapping.userType = 1;
												excelPasswordMapping.Add(passwordMapping);
												ilistTemp.Add(CommonUtility.EncodeUsingMD5(initializePwd));
												break;
											case "Sheet2":
												passwordMapping = new PasswordMapping
												{
													userNo = row.GetCell(0).ToString(),
													userName = row.GetCell(1).ToString(),
													initializePassword = initializePwd
												};
												passwordMapping.userType = 2;
												excelPasswordMapping.Add(passwordMapping);
												ilistTemp.Add(CommonUtility.EncodeUsingMD5(initializePwd));
												break;
											case "Sheet3":
												passwordMapping = new PasswordMapping
												{
													userNo = row.GetCell(0).ToString(),
													userName = row.GetCell(1).ToString(),
													initializePassword = initializePwd
												};
												passwordMapping.userType = 3;
												excelPasswordMapping.Add(passwordMapping);
												ilistTemp.Add(CommonUtility.EncodeUsingMD5(initializePwd));
												break;
											default:
												break;
										}
									}
								}
								//将要执行的SQL信息添加到ExcelSheetData数据中

								ExcelSheetData excelSheetDataTemp = ReturnSqlJudgeBySheetName(sheetName, ReturnModelDataJudgeBySheetName(sheetName, ilistTemp));

								excelSheetDataTemp.sheetName = exceptionSheetName;
								excelSheetDataTemp.sheetRowNo = exceptionRowNo;

								excelSheetDataArray.Add(excelSheetDataTemp);
							}

							//行编号值加1
							currentRowNo += 1;
						}
					}
					excelData = new ExcelData { excelSheetData = excelSheetDataArray, excelPasswordMapping = excelPasswordMapping };
				}
			}
			catch (Exception ex)
			{
				MongoDBLog.LogRecord(ex);
				HttpContext.Current.Response.Write("<script type='text/javascript'>alert('很抱歉，读取Excel文件数据失败！此次操作未更改任何数据库数据，相关信息如下：\\n\\n出错的工作薄名称为："
					+ exceptionSheetName
					+ "\\n出错的单元格行号为：" + exceptionRowNo
					//+ "\n出错的单元格列号为：" + exceptionColNo
					+ "\\n可能的原因为：\\n 此单元格数据格式可能不正确，例如：单元格数据是否存在多余的空格。"
					+ "请检查Excel文件数据，修改后重新上传！');history.go(-1);</script>");
			}
			finally
			{
				if (File.Exists(HttpContext.Current.Server.MapPath("/" + FileUpload1.FileName)))
				{
					File.Delete(HttpContext.Current.Server.MapPath("/" + FileUpload1.FileName));
				}
			}

			return excelData;

		}
		#endregion


        #region 读取Excel数据并返回List<BllImportGradeCheckExcelData>数组类型给Dal进行处理
        /// <summary>
        /// 读取Excel数据并返回BllImportGradeCheckExcelData类型的集合给Dal进行处理
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="_termYear">学年</param>
        /// <returns>返回GradeCheckExcelData实体类</returns>
        public static GradeCheckExcelData BllImportGradeCheckExcelData(string filePath,string _termYear)
        {
            //设定一个自定义GradeCheckExcelData类型，用于返回全部的Excel数据
            GradeCheckExcelData gradeCheckExcelData = null;

            //出错的行号
            int exceptionRowNo = 0;

            //出错的列号
            int exceptionColNo = 0;

            //自定义出错信息
            string errorInfo = "导入的Excel数据中存在学生学年与指定学年不匹配的记录！";

            List<StudentsGradeCheckConfirm> _listStudentsGradeCheckConfirm = new List<StudentsGradeCheckConfirm>();
            List<StudentsGradeCheckDetail> _listStudentsGradeCheckDetail = new List<StudentsGradeCheckDetail>();

            int _errorNo = 0;
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    //建立WorkBook
                    HSSFWorkbook HSSFWorkbook = new HSSFWorkbook(file);

                    HSSFSheet Sheet = HSSFWorkbook.GetSheetAt(0);

                    IEnumerator rows = Sheet.GetRowEnumerator();

                    //导入时间 
                    DateTime _now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    //获取“是否符合学位申请条件”与“不及格科目”两个特定列的列号
                    int isAccordColumnNo = -1;
                    int remarkColumnNo = -1;

                    //Hashtable ht = new Hashtable();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    int _columnsCount=0;

                    while (rows.MoveNext())
                    {
                        HSSFRow row = (HSSFRow)rows.Current;

                        StudentsGradeCheckConfirm _studentsGradeCheckConfirm = new StudentsGradeCheckConfirm();
                        _studentsGradeCheckConfirm.updateTime = _now;

                        //HttpContext.Current.Response.Write(_columnsCount + "<Br/>");

                        if (exceptionRowNo == 0)
                        {
                            _columnsCount = row.PhysicalNumberOfCells;

                            for (int j = 0; j < _columnsCount; j++)
                            {
                                string _value = row.GetCell(j).ToString().Trim();

                                //HttpContext.Current.Response.Write(_value+"<Br/>");

                                switch (_value)
                                {
                                    case "是否符合学位申请条件":
                                        isAccordColumnNo = j;
                                        break;
                                    case "不及格科目":
                                        remarkColumnNo = j;
                                        break;
                                    default:
                                        break;
                                }

                                if (j > 5 && _value != "是否符合学位申请条件" && _value != "不及格科目")
                                {
                                    //ht.Add("_cols"+j.ToString().Trim(), _value);
                                    dic.Add("_cols" + j.ToString().Trim(), _value);
                                }
                            }
                        }
                        //HttpContext.Current.Response.Write(_columnsCount);
                        //HttpContext.Current.Response.End();
                        //HttpContext.Current.Response.Write(ht.Count);

                        //foreach (DictionaryEntry _item in ht)
                        //{
                        //    HttpContext.Current.Response.Write(_item.Key + ":" + _item.Value + "<br/>");
                        //    ;
                        //}
                        //HttpContext.Current.Response.Write(isAccordColumnNo+"<br/>");
                        //HttpContext.Current.Response.Write(remarkColumnNo);
                        //HttpContext.Current.Response.End();

                        //从第二行开始读取数据（并且第一列序号数据不为空）
                        if (exceptionRowNo > 0 && !string.IsNullOrEmpty(row.GetCell(0).ToString().Trim()))
                        {
                            string _studentNo = string.Empty;

                            for (int j = 0; j < _columnsCount; j++)
                            {
                                //HttpContext.Current.Response.Write(j.ToString().Trim());

                                exceptionColNo = j;

                                string _value = row.GetCell(j).ToString().Trim();

                                switch (j)
                                {
                                    //序号
                                    case 0:
                                        //HttpContext.Current.Response.Write(_value + "<Br/>");
                                        break;
                                    //学号
                                    case 1:
                                        //HttpContext.Current.Response.Write(_value + "<Br/>");
                                        _studentsGradeCheckConfirm.studentNo = _value;
                                        _studentNo = _value;
                                        break;
                                    //姓名
                                    case 2:
                                        //HttpContext.Current.Response.Write(_value + "<Br/>");
                                        break;
                                    //班级
                                    case 3:
                                        //HttpContext.Current.Response.Write(_value + "<Br/>");
                                        break;
                                    //年级
                                    case 4:
                                        //HttpContext.Current.Response.Write(_value + "<Br/>");
                                        //HttpContext.Current.Response.End();
                                        //判断学年是否正确
                                        if (_value.Length >= 2 && _value.Substring(0, 2) != _termYear)
                                        {
                                            throw new Exception(errorInfo + "当前选择的学年为：" + _termYear + "，读取到的学年值为：" + _value + "，请检查！");
                                        }
                                        break;
                                    //班主任
                                    case 5:
                                        break;
                                    default:
                                        break;
                                }

                                if (j > 5)
                                {
                                    if (isAccordColumnNo == j)
                                    {
                                        //判断是否符合学位申请列的数据
                                        if ((_value != "符合" && _value != "不符合"))
                                        {
                                            throw new Exception("是否符合学位申请条件列的值只能为“符合”或者“不符合”，当前读取到的值为“" + _value +"”");
                                        }
                                        _studentsGradeCheckConfirm.isAccord = (_value == "符合" ? 1 : 0);
                                    }
                                    else if (remarkColumnNo == j)
                                    {
                                        _studentsGradeCheckConfirm.remark = _value;
                                    }
                                    //动态变化列
                                    else
                                    {
                                        //HttpContext.Current.Response.Write(j+"_"+ht[(object)("_cols" + j).ToString().Trim()].ToString().Trim()+"<br/>");
                                        //_errorNo = j;
                                        //if (!dic.ContainsKey("_cols" + j))
                                        //{
                                        //    HttpContext.Current.Response.Write(j+"_"+exceptionRowNo+"_"+"<br/>");
                                        //}
                                        _listStudentsGradeCheckDetail.Add(new StudentsGradeCheckDetail
                                        {
                                            updateTime = _now,
                                            studentNo = _studentNo,
                                            //gradeCheckItemName = ht[(object)("_cols"+j).ToString().Trim()].ToString().Trim(),
                                            gradeCheckItemName = dic["_cols"+j].ToString().Trim(),
                                            termYear = _termYear,
                                            gradeCheckDetailValue = _value,
                                            colNo = j + 1
                                        });
                                        if (j == 7)
                                        {
                                            //HttpContext.Current.Response.Write(_value);
                                            //HttpContext.Current.Response.End();
                                        }
                                    }
                                }

                                
                            }
                            _listStudentsGradeCheckConfirm.Add(_studentsGradeCheckConfirm);
                        }
                        exceptionRowNo += 1;
                    }
                    gradeCheckExcelData = new GradeCheckExcelData { listStudentsGradeCheckConfirm = _listStudentsGradeCheckConfirm, listStudentsGradeCheckDetail = _listStudentsGradeCheckDetail };
                }
               // HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Write(_errorNo);
                MongoDBLog.LogRecord(ex);
                HttpContext.Current.Response.Write("<script type='text/javascript'>alert('很抱歉，读取Excel文件数据失败！此次操作未更改任何数据库数据，相关信息如下：\\n\\n出错的单元格行号为：" + (exceptionRowNo + 1)
                    + "\\n出错的单元格列号为：" + (exceptionColNo+1)
                    + "\\n可能的原因为：" + ex.Message
                    + "请检查Excel文件数据，修改后重新上传！');history.go(-1);</script>");
            }

            return gradeCheckExcelData;

        }
        #endregion
	}
}