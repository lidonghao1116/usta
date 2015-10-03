using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Transactions;
using USTA.Model;
using USTA.Bll;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using System.Text;
using System.Collections;


public partial class Administrator_StudentManager : System.Web.UI.Page
{
    public int fileFolderType = (int)FileFolderType.gradeCheck;
    public int fileFolderType1 = (int)FileFolderType.gradeCheckExcelTemplate;
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    public string studentNo = (HttpContext.Current.Request.QueryString["studentNo"] == null ? string.Empty : HttpContext.Current.Request.QueryString["studentNo"]);
    public string fragmentFlag = "1";
    //已经有的附件数，页面初始化时与前端JS进行交互
    public int iframeCount = 0;
    public int iframeCount1 = 0;

    public string tableHeader = string.Empty;

    public List<int> listGradeCheckId = new List<int>();

    Hashtable ht = new Hashtable();

    //========
    public string _ddlSearchYear = HttpContext.Current.Request["ddlSearchYear"] == null ? string.Empty : HttpContext.Current.Request["ddlSearchYear"];
    public string _ddlSearchMajor = HttpContext.Current.Request["ddlSearchMajor"] == null ? string.Empty : HttpContext.Current.Request["ddlSearchMajor"];
    public string _ddlSearchSchoolClass = HttpContext.Current.Request["ddlSearchSchoolClass"] == null ? string.Empty : HttpContext.Current.Request["ddlSearchSchoolClass"];
    public string _ddlGradeCheckDegree = HttpContext.Current.Request["ddlGradeCheckDegree"] == null ? string.Empty : HttpContext.Current.Request["ddlGradeCheckDegree"];
    public string _ddlGradeCheckLocale = HttpContext.Current.Request["ddlGradeCheckLocale"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ddlGradeCheckLocale"]);
    public string _keyword = HttpContext.Current.Request["keyword"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["keyword"]);
    //========

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //控制Tab的显示

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, liFragment4
                , liFragment5, liFragment6, liFragment7, liFragment8, liFragment9, divFragment1, divFragment2, divFragment3, divFragment4, divFragment5, divFragment6, divFragment7, divFragment8, divFragment9);


            if (fragmentFlag.Equals("1"))
            {
                startTime.Attributes.Remove("class");
                endTime.Attributes.Remove("class");
                DalOperationAboutGradeCheck doan = new DalOperationAboutGradeCheck();
                DataTable dt = doan.GetTermYear().Tables[0];

                ddlSearchYear.Items.Add(new ListItem("所有", "all"));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSearchYear.Items.Add(new ListItem("20" + dt.Rows[i]["termYear"].ToString().Trim() + "学年", dt.Rows[i]["termYear"].ToString().Trim()));
                }

                if (!string.IsNullOrEmpty(_ddlSearchYear))
                {
                    for (int i = 0; i < ddlSearchYear.Items.Count; i++)
                    {
                        if (ddlSearchYear.Items[i].Value == _ddlSearchYear.Trim())
                        {
                            ddlSearchYear.SelectedIndex = i;
                            break;
                        }
                    }
                }

                DalOperationStudentSpecility doss = new DalOperationStudentSpecility();

                dt = doss.FindAllStudentSpecilitye().Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSearchMajor.Items.Add(new ListItem(dt.Rows[i]["specilityName"].ToString().Trim(), dt.Rows[i]["MajorTypeID"].ToString().Trim()));
                }


                if (!string.IsNullOrEmpty(_ddlSearchMajor))
                {
                    for (int i = 0; i < ddlSearchMajor.Items.Count; i++)
                    {
                        if (ddlSearchMajor.Items[i].Value == _ddlSearchMajor.Trim())
                        {
                            ddlSearchMajor.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(_ddlSearchYear) || (!string.IsNullOrEmpty(_ddlSearchMajor)))
                {
                    GetSchoolClassList();
                }

                if (!string.IsNullOrEmpty(_ddlSearchSchoolClass))
                {
                    for (int i = 0; i < ddlSearchSchoolClass.Items.Count; i++)
                    {
                        if (ddlSearchSchoolClass.Items[i].Value == _ddlSearchSchoolClass.Trim())
                        {
                            ddlSearchSchoolClass.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(_ddlGradeCheckDegree))
                {
                    for (int i = 0; i < ddlGradeCheckDegree.Items.Count; i++)
                    {
                        if (ddlGradeCheckDegree.Items[i].Value == _ddlGradeCheckDegree.Trim())
                        {
                            ddlGradeCheckDegree.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(_ddlGradeCheckLocale))
                {
                    for (int i = 0; i < ddlGradeCheckLocale.Items.Count; i++)
                    {
                        if (ddlGradeCheckLocale.Items[i].Value == _ddlGradeCheckLocale.Trim())
                        {
                            ddlGradeCheckLocale.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(_keyword))
                {
                    txtKeyword.Text = _keyword;
                }

                DataListBind();
            }


            if (fragmentFlag.Equals("2"))
            {
                startTime.Attributes.Remove("class");
                endTime.Attributes.Remove("class");
                DataBindTermTagList(); DataBindSearchCourse();
            }


            if (fragmentFlag.Equals("3"))
            {
                //txtKeyword.Attributes.Add("class", "required");
                DataListBindGradeCheckApplyReason();
            }

            if (fragmentFlag.Equals("5"))
            {
                DalOperationAboutGradeCheck doan = new DalOperationAboutGradeCheck();
                DataTable dt = doan.GetTermYear().Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTermYear.Items.Add(new ListItem("20" + dt.Rows[i]["termYear"].ToString().Trim() + "学年", dt.Rows[i]["termYear"].ToString().Trim()));
                }

                DataListBindGradeCheck();
            }


            if (fragmentFlag.Equals("6"))
            {
                DalOperationAboutGradeCheck doan = new DalOperationAboutGradeCheck();
                DataTable dt = doan.GetTermYear().Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTermYearImportExcelData.Items.Add(new ListItem("20" + dt.Rows[i]["termYear"].ToString().Trim() + "学年", dt.Rows[i]["termYear"].ToString().Trim()));
                }
            }


            if (Request["studentNo"] != null && fragmentFlag.Equals("4"))
            {
                liFragment4.Visible = true;
                studentNo = Request["studentNo"];

                //删除
                if (Request["del"] == "true" && Request["courseNo"] != null && Request["coursesStudentsCorrelationId"] != null)
                {
                    DalOperationAboutStudent dalw = new DalOperationAboutStudent();
                    string coursesStudentsCorrelationId = Request["coursesStudentsCorrelationId"].ToString().Trim();
                    dalw.DelChooseCourseByCoursesStudentsCorrelationId(coursesStudentsCorrelationId);
                }

                DalOperationAboutStudent dal = new DalOperationAboutStudent();
                lblstudentName.Text = dal.GetStudentById(Request["studentNo"]).studentName;
                DalOperationAboutStudent dal1 = new DalOperationAboutStudent();
                DataSet ds = dal1.GetCoursesByStudentNo(Request["studentNo"].ToString().Trim());
                dlstcourses.DataSource = ds.Tables[0];
                dlstcourses.DataBind();
            }


            if (fragmentFlag.Equals("6"))
            {
                BindGradeCheckExcelTemplate(spanAttachment2, false);
            }

            if (Request["studentNo"] != null && fragmentFlag.Equals("7"))
            {
                liFragment7.Visible = true;
                studentNo = Request["studentNo"];
                DlstStudentSchoolClassNameDataBind();
                DlstStudentGradeCheckDataBind();

                if (Request["action"] != null && Request["action"].ToString().Trim() == "delete")
                {
                    dlstStudentGradeCheckDetail_Delete();
                }
            }

            if (fragmentFlag.Equals("8"))
            {
                BindGradeCheckExcelTemplate(spanExcelTemplate, true);

                if (!IsPostBack)
                {
                    Javascript.ExcuteJavascriptCode("initBeforeUnloadEvent('温馨提示：当前页面相关操作必须点击提交才能生效~（此为提示，并不代表您真正未保存数据），确定离开吗？');", Page);
                }
            }

            if (fragmentFlag.Equals("9"))
            {

                BindGradeCheckAllowTime();

                BindGradeCheckNotify();

                if (!IsPostBack)
                {
                    txtNotifyTitle.CssClass = "required";
                    Javascript.ExcuteJavascriptCode("initBeforeUnloadEvent('温馨提示：当前页面数据可能未保存哟~（此为提示，并不代表您真正未保存数据），确定离开吗？');", Page);
                }
            }
        }


        if (fragmentFlag.Equals("1") || fragmentFlag.Equals("2") || fragmentFlag.Equals("6") || fragmentFlag.Equals("8") || fragmentFlag.Equals("9"))
        {
            startTime.Attributes.Remove("class");
            endTime.Attributes.Remove("class");
        }
    }

    protected void btnImportGradeCheckExcelData_Click(object sender, EventArgs e)
    {
        string _folder = Server.MapPath(ConfigurationManager.AppSettings["GradeCheckImportExcel"]);

        if (FileUpload1.HasFile)
        {
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            if (!(UploadFiles.GetExtension((FileUpload1.FileName)).ToLower().Trim() == ".xls" || UploadFiles.GetExtension((FileUpload1.FileName)).ToLower().Trim() == ".xlsx"))
            {
                Javascript.GoHistory(-1, "上传文件类型有误，请上传Excel文件：）", Page);
                return;
            }

            string _path = _folder + "ImportGradeCheckExcelData" + UploadFiles.DateTimeString() + ".xls";

            FileUpload1.SaveAs(_path);

            string _termYear = ddlTermYearImportExcelData.SelectedValue;


            //读取要导入的成绩审核Excel文件的全部数据
            GradeCheckExcelData gradeCheckExcelData = BllOperationAboutExcel.BllImportGradeCheckExcelData(_path, _termYear);
            //Response.End();
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            for (int i = 0; i < gradeCheckExcelData.listStudentsGradeCheckDetail.Count;i++ )
            {
                DataTable dt = dal.GetGradeCheckItemsByGradeCheckItemName(gradeCheckExcelData.listStudentsGradeCheckDetail[i].gradeCheckItemName, ddlTermYearImportExcelData.SelectedValue).Tables[0];
                
                //Response.Write(_studentsGradeCheckDetail.gradeCheckItemName+"<br/>");
                if (dt.Rows.Count > 0)
                {
                    //_studentsGradeCheckDetail.gradeCheckItemName = dt.Rows[0]["gradeCheckItemName"].ToString().Trim();
                    //Response.Write(dt.Rows[0]["gradeCheckItemName"].ToString().Trim()+":"+dt.Rows[0]["gradeCheckId"].ToString().Trim()+"<Br/>");
                    gradeCheckExcelData.listStudentsGradeCheckDetail[i].gradeCheckId = int.Parse(dt.Rows[0]["gradeCheckId"].ToString().Trim());
                    //_studentsGradeCheckDetail.gradeCheckItemDefaultValue = dt.Rows[0]["gradeCheckItemDefaultValue"].ToString().Trim();
                }
                else
                {
                    Javascript.GoHistory(-1, "成绩审核Excel文件有数据问题，\\n未找到" + ddlTermYearImportExcelData.SelectedItem.Text + "第" + gradeCheckExcelData.listStudentsGradeCheckDetail[i].colNo + "列审核规则\\n请检查此列名是否存在或者与教学辅助系统中的列名完全一致（包括空格标点等）！", Page);
                    return;
                }
            }
            //Response.Write("ssss");
            //Response.End();

            //检测是否有不符合选择的学年及培养地要求的记录，只要有一条不符合，则不导入
            for (int i = 0; i < gradeCheckExcelData.listStudentsGradeCheckDetail.Count; i++)
            {
                DataSet _checkdata = dal.CheckDataConsistenceByStudentNoTermYearLocale(gradeCheckExcelData.listStudentsGradeCheckDetail[i].studentNo, ddlTermYearImportExcelData.SelectedValue, ddlLocaleImportExcelData.SelectedValue);

                if (_checkdata.Tables[0].Rows.Count == 0)
                {
                    Javascript.GoHistory(-1, "成绩审核Excel文件有数据问题，\\n学号为" + gradeCheckExcelData.listStudentsGradeCheckDetail[i].studentNo + "的学生数据记录不符合所选择的学年及培养地要求\\n请检查！", Page);
                    return;
                }
            }


            //Response.Write(gradeCheckExcelData.listStudentsGradeCheckConfirm.Count + "<br/>");

            string errorStudentNo = string.Empty;
            //将获取的导入数据存入数据库，事务控制
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //首先删除指定学年和培养地的成绩审核数据
                    dal.DeleteStudentsGradeCheckConfirmItemsByTermYear(_termYear, ddlLocaleImportExcelData.SelectedValue.Trim());
                    dal.DeleteStudentsGradeCheckDetailItemsByTermYear(_termYear, ddlLocaleImportExcelData.SelectedValue.Trim());

                    //接下来遍历获取到的导入数据，并写入数据库，还需要加一个学年和培养地验证判断(已添加)
                    //首先导入usta_StudentsGradeCheckConfirm表数据
                    foreach (StudentsGradeCheckConfirm _studentsGradeCheckConfirm in gradeCheckExcelData.listStudentsGradeCheckConfirm)
                    {
                        //Response.Write(_studentsGradeCheckConfirm.studentNo + "<br/>");
                        errorStudentNo = _studentsGradeCheckConfirm.studentNo;
                        dal.AddStudentGradeCheckConfirm(_studentsGradeCheckConfirm);
                    }

                    //其次导入usta_StudentsGradeCheckDetail表数据
                    foreach (StudentsGradeCheckDetail _studentsGradeCheckDetail in gradeCheckExcelData.listStudentsGradeCheckDetail)
                    {
                        //if (_studentsGradeCheckDetail.colNo == 8)
                        //{
                        //    Response.Write(_studentsGradeCheckDetail.gradeCheckId + "_" + _studentsGradeCheckDetail.gradeCheckDetailValue+"<Br/>");
                        //}
                        errorStudentNo = _studentsGradeCheckDetail.studentNo;
                        dal.AddGradeCheckDetailByStudentNo(_studentsGradeCheckDetail);
                    }

                    scope.Complete();
                    Javascript.AlertAndRedirect("导入成绩审核数据成功，点击确定查看：）", "/Administrator/StudentManager.aspx?fragment=1", Page);
                }
                catch (System.Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "导入成绩审核数据失败：（\\n学号为：" + errorStudentNo + "的学生数据不符合导入格式要\\n请检查！", Page);
                }
            }
        }
        else
        {
            Javascript.GoHistory(-1, "请先选择Excel文件！", Page);
            return;
        }
    }

    protected void ddlTermYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataListBindGradeCheck();
    }


    /// <summary>
    /// 获取动态表头
    /// </summary>
    /// <returns></returns>
    protected string GetTableHeader()
    {
        StringBuilder sb = new StringBuilder();
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

        DataTable dt = dal.GetGradeCheckItemsByTermYear(ddlSearchYear.SelectedValue).Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sb.Append(string.Format("<th width=\"6%\">{0}</th>",dt.Rows[i]["gradeCheckItemName"].ToString().Trim()));
            listGradeCheckId.Add(int.Parse(dt.Rows[i]["gradeCheckId"].ToString().Trim()));
            ht.Add(dt.Rows[i]["gradeCheckId"].ToString().Trim(), dt.Rows[i]["gradeCheckItemDefaultValue"].ToString().Trim());
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取动态表单元格
    /// </summary>
    /// <returns></returns>
    protected string GetTableTd(string studentNo, DateTime updateTime)
    {
        StringBuilder sb = new StringBuilder();
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

        foreach (int _key in listGradeCheckId)
        {
            DataTable dt = dal.GetGradeCheckDetailByGradeCheckIdAndStudentNoAndUpdateTime(_key, studentNo, updateTime).Tables[0];

            if (dt.Rows.Count == 0)
            {
                sb.Append("<td width=\"6%\">" + ht[_key.ToString()] + "</td>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (DictionaryEntry _item in ht)
                    {
                        if (_item.Key.ToString().Trim() == dt.Rows[i]["gradeCheckId"].ToString().Trim())
                        {
                            sb.Append(string.Format("<td width=\"6%\">{0}</td>", dt.Rows[i]["gradeCheckDetailValue"].ToString().Trim()));
                        }
                    }
                }
            }
        }

        return sb.ToString();
    }

    //下拉列表事件
    protected void ddlApplyResult_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlBindCourses(true);
        DataBindSearchCourse();
    }

    //下拉列表事件
    protected void ddlApplyLocale_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlBindCourses(true);
        DataBindSearchCourse();
    }
    //下拉列表事件
    protected void ddlTermTags_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBindCourses(txtCourseName.Text.Trim().Length == 0);
        //绑定课程列表--学期标识(termTag)
        DataBindSearchCourse();
    }
    //模糊查询学生信息
    protected void btnQueryCourses_Click(object sender, EventArgs e)
    {
        ddlBindCourses(false);
        DataBindSearchCourse();
    }

    //下拉列表事件
    protected void ddlCourses_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定课程列表--学期标识(termTag)
        DataBindSearchCourse();
    }

    protected void ddlBindCourses(bool isAll)
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataTable dv = doac.SearchCourses(ddlTermTags.SelectedValue, txtCourseName.Text.Trim()).Tables[0];

        bool isSearchNoResult = false;

        while (ddlCourses.Items.Count > 0)
        {
            ddlCourses.Items.RemoveAt(0);
        }

        if (isAll)
        {
            ddlCourses.Items.Add(new ListItem("全部课程", "all"));
        }

        if (dv.Rows.Count == 0)
        {
            dv = doac.SearchCourses(ddlTermTags.SelectedValue, "").Tables[0];
            isSearchNoResult = true;
        }

        for (int i = 0; i < dv.Rows.Count; i++)
        {
            ddlCourses.Items.Add(new ListItem(dv.Rows[i]["courseName"].ToString().Trim() + "(" + dv.Rows[i]["classID"].ToString().Trim() + ")", dv.Rows[i]["termTag"].ToString().Trim() + dv.Rows[i]["courseNo"].ToString().Trim() + dv.Rows[i]["classID"].ToString().Trim()));
        }

        if (isSearchNoResult)
        {
            ddlCourses.Items.Add(new ListItem("全部课程", "all"));
        }
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        DataBindSearchCourse();
    }
    //绑定搜索的课程信息
    public void DataBindSearchCourse()
    {
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

        // Response.Write(ddlCourses.SelectedValue);

        DataTable dt = dal.GetAllStudentGradeCheckApply(ddlCourses.SelectedValue == "all" ? "all_" + ddlTermTags.SelectedValue : ddlCourses.SelectedValue, ddlApplyResult.SelectedValue, ddlApplyLocale.SelectedValue).Tables[0];
        DataView dv = dt.DefaultView;
        this.AspNetPager1.RecordCount = dv.Count;
        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;
        pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
        pds.PageSize = CommonUtility.pageSize;

        this.dlstGradeCheckApply.DataSource = pds;
        this.dlstGradeCheckApply.DataBind();

        if (dt.Rows.Count == 0)
        {
            this.dlstGradeCheckApply.ShowFooter = true;
        }
        else
        {
            this.dlstGradeCheckApply.ShowFooter = false;
        }
    }

    //绑定学期标识下拉列表
    /// <summary>
    /// 
    /// </summary>
    public void DataBindTermTagList()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataTable dt = doac.FindAllTermTags().Tables[0];
        string termTag = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            termTag = dt.Rows[i]["termTag"].ToString().Trim();
            ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
            ddlTermTags.Items.Add(li);
        }
        ddlBindCourses(txtCourseName.Text.Trim().Length == 0);
    }

    protected void BindGradeCheckAllowTime()
    {
        DateTime now = DateTime.Now;
        StudentsGradeCheckConfig model = new StudentsGradeCheckConfig { startTime = now, endTime = now };

        DalOperationAboutGradeCheck dos = new DalOperationAboutGradeCheck();
        DataTable dt = dos.GetGradeCheckAllowTime().Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            model.startTime = Convert.ToDateTime(dt.Rows[i]["startTime"].ToString().Trim());
            model.endTime = Convert.ToDateTime(dt.Rows[i]["endTime"].ToString().Trim());
        }

        startTime.Value = model.startTime.ToString("yyyy-MM-dd HH:mm:ss");
        endTime.Value = model.endTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    protected void BindGradeCheckNotify()
    {
        StudentsGradeCheckConfig model = new StudentsGradeCheckConfig { notifyTitle = string.Empty, notifyContent = string.Empty };

        DalOperationAboutGradeCheck dos = new DalOperationAboutGradeCheck();
        DataTable dt = dos.GetGradeCheckDocument().Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            model.notifyTitle = dt.Rows[i]["notifyTitle"].ToString().Trim();
            model.notifyContent = dt.Rows[i]["notifyContent"].ToString().Trim();
            model.attachmentIds = dt.Rows[i]["attachmentIds"].ToString().Trim();
        }

        txtNotifyTitle.Text = model.notifyTitle;
        txtNotifyContent.Text = model.notifyContent;
        hidAttachmentId.Value = model.attachmentIds;

        if (model.attachmentIds.Length > 0)
        {
            DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
            spanAttachment.InnerHtml = dalOperationAttachments.GetAttachmentsList(model.attachmentIds, ref iframeCount, true, string.Empty);
        }
    }

    protected void BindGradeCheckExcelTemplate(HtmlGenericControl span,bool isDelete)
    {
        DalOperationAboutGradeCheck dos = new DalOperationAboutGradeCheck();
        DataTable dt = dos.GetGradeCheckExcelTemplate().Tables[0];

        string _ids = string.Empty;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            _ids = dt.Rows[i]["excelTemplateAttachmentIds"].ToString().Trim();
        }

        hidAttachmentId1.Value = _ids;

        if (_ids.Length > 0)
        {
            DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
            span.InnerHtml = dalOperationAttachments.GetAttachmentsList(_ids, ref iframeCount1, isDelete, "1");
        }
    }

    protected void GetSchoolClassList()
    {
        while (ddlSearchSchoolClass.Items.Count > 0)
        {
            ddlSearchSchoolClass.Items.RemoveAt(0);
        }

        ddlSearchSchoolClass.Items.Add(new ListItem("在所有班级中查找", "all"));

        if (!(ddlSearchYear.SelectedValue == "all" && ddlSearchMajor.SelectedValue == "all"))
        {
            DalOperationAboutSchoolClass doss = new DalOperationAboutSchoolClass();

            DataSet ds = doss.GetSchoolClassByMajorAndTermYear(ddlSearchMajor.SelectedValue == "all" ? "all" : ddlSearchMajor.SelectedValue, ddlSearchYear.SelectedValue == "all" ? "all" : ddlSearchYear.SelectedValue);

            if (ds != null)
            {

                DataTable dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSearchSchoolClass.Items.Add(new ListItem(dt.Rows[i]["className"].ToString().Trim(), dt.Rows[i]["SchoolClassID"].ToString().Trim()));
                }
            }
        }
    }

    //模糊查询学生信息
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataListBind();
    }
    //绑定搜索到的学生数据到DataList
    protected void DataListBind()
    {
        tableHeader = this.GetTableHeader();
        DalOperationAboutGradeCheck dos = new DalOperationAboutGradeCheck();
        DataView dv = dos.SearchStudentsList(ddlSearchYear.SelectedValue, ddlSearchMajor.SelectedValue, ddlSearchSchoolClass.SelectedValue, txtKeyword.Text.Trim(),ddlGradeCheckLocale.SelectedValue,ddlGradeCheckDegree.SelectedValue).Tables[0].DefaultView;

        this.AspNetPager2.RecordCount = dv.Count;
        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;
        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize;

        this.dlSearchStudent.DataSource = pds;
        this.dlSearchStudent.DataBind();

        if (pds.Count == 0)
        {
            this.dlSearchStudent.ShowFooter = true;
        }
        else
        {
            this.dlSearchStudent.ShowFooter = false;
        }
    }
    //搜索到的学生列表操作
    protected void dlSearchStudent_ItemCommand(object source, DataListCommandEventArgs e)
    {
        DalOperationUsers dos = new DalOperationUsers();

        if (e.CommandName == "delete")
        {
            string studentNo = dlSearchStudent.DataKeys[e.Item.ItemIndex].ToString();//取选中行学生编号  

            if (dos.DeleteStudentByNo(studentNo) > 0)
            {
                Javascript.AlertAndRedirect("删除成功！", "/Administrator/StudentManager.aspx?fragment=1&page=" + pageIndex, Page);
            }
            else
            {
                Javascript.GoHistory(-1, "删除失败！", Page);
            }
        }
    }
    
    /// <summary>
    /// 搜索到的学生列表分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataListBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSearchYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSchoolClassList();
        DataListBind();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSearchMajor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSchoolClassList();
        DataListBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSearchSchoolClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataListBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlGradeCheckDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataListBind();
    }

    /// <summary>
    /// 培养地筛选
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlGradeCheckLocale_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataListBind();
    }


    protected void DataListBindGradeCheck()
    {
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
        if (ddlTermYear.SelectedValue.Trim() == "all")
        {
            dlstGradeCheck.DataSource = dal.GetAllGradeCheckItems().Tables[0];
            dlstGradeCheck.DataBind();
        }
        else
        {
            dlstGradeCheck.DataSource = dal.GetGradeCheckItemsByTermYear(ddlTermYear.SelectedValue).Tables[0];
            dlstGradeCheck.DataBind();
        }
    }


    protected void DataListBindGradeCheckApplyReason()
    {
        DalOperationAboutGradeCheckApplyReason dal = new DalOperationAboutGradeCheckApplyReason();
        DataTable dt= dal.GetAll().Tables[0];
        dlstGradeCheckApplyReason.DataSource = dt;

        dlstGradeCheckApplyReason.DataBind();

        if (dt.Rows.Count == 0)
        {
            dlstGradeCheckApplyReason.ShowFooter = true;
        }
        else
        {
            dlstGradeCheckApplyReason.ShowFooter = false;
        }
    }


    protected void dlstGradeCheck_ItemCommand(object source, DataListCommandEventArgs e)
    {
        DalOperationAboutGradeCheck dos = new DalOperationAboutGradeCheck();

        if (e.CommandName == "delete")
        {
            int gradeCheckId = int.Parse(dlstGradeCheck.DataKeys[e.Item.ItemIndex].ToString());//取选中行学生编号  

            if (dos.DeleteGradeCheckItemById(gradeCheckId) > 0)
            {
                Javascript.AlertAndRedirect("删除成功！", "/Administrator/StudentManager.aspx?fragment=5", Page);
            }
            else
            {
                Javascript.GoHistory(-1, "删除失败！", Page);
            }
        }
    }


    protected void dlstGradeCheckApplyReason_ItemCommand(object source, DataListCommandEventArgs e)
    {
        DalOperationAboutGradeCheckApplyReason dos = new DalOperationAboutGradeCheckApplyReason();

        if (e.CommandName == "delete")
        {
            int gradeCheckApplyReasonId = int.Parse(dlstGradeCheckApplyReason.DataKeys[e.Item.ItemIndex].ToString());//取选中行学生编号  

            if (dos.Delete(gradeCheckApplyReasonId))
            {
                Javascript.AlertAndRedirect("删除成功！", "/Administrator/StudentManager.aspx?fragment=3", Page);
            }
            else
            {
                Javascript.GoHistory(-1, "删除失败！", Page);
            }
        }
    }


    protected void DlstStudentSchoolClassNameDataBind()
    {
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
        DataView dv = dal.GetTermYearByStudentNo(studentNo).Tables[0].DefaultView;

        this.dlstStudentSchoolClassName.DataSource = dv;
        this.dlstStudentSchoolClassName.DataBind();
    }


    protected void DlstStudentGradeCheckDataBind()
    {
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
        DataView dv = dal.GetUpdateTimeByStudentNo(studentNo).Tables[0].DefaultView;

        this.dlstStudentGradeCheck.DataSource = dv;
        this.dlstStudentGradeCheck.DataBind();
    }

    protected void dlstStudentGradeCheckDetail_Delete()
    {
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

        DateTime updateTime = DateTime.Now;

        if (!CommonUtility.SafeCheckByParams<string>(Request["updateTime"].ToString(), ref updateTime))
        {
            Javascript.GoHistory(-1, "参数有误", Page);
            return;
        }

        try
        {
            dal.DeleteGradeCheckDetailByStudentNoAndUpdateTime(studentNo, updateTime);
            Javascript.RefreshParentWindow("删除成绩审核记录成功！", "/Administrator/StudentManager.aspx?fragment=7&studentNo=" + studentNo, Page);
        }
        catch (Exception ex)
        {
            MongoDBLog.LogRecord(ex);
            Javascript.GoHistory(-1, "删除成绩审核记录失败！", Page);
        }
        finally
        {
            dal.conn.Close();
        }
    }


    protected void dlstStudentGradeCheck_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList dataList = (DataList)e.Item.FindControl("dlstStudentGradeCheckItem");
            DataRowView rowv = (DataRowView)e.Item.DataItem;
            string updateTime = rowv["updateTime"].ToString().Trim();
            DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
            //DataSet ds = dalOperationAboutGradeCheck.GetGradeCheckDetailByStudentNo(studentNo, updateTime);
            DataSet ds = dalOperationAboutGradeCheck.GetGradeCheckDetailByStudentNo(studentNo);
            dataList.DataSource = ds.Tables[0].DefaultView;
            dataList.DataBind();

            if (dataList.Items.Count == 0)
            {
                dataList.ShowFooter = true;
            }
            else
            {
                dataList.ShowFooter = false;
            }
        }
    }



    protected string GetGradeCheckDetailByGradeCheckId(int gradeCheckId)
    {
        DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
        DataSet ds = dalOperationAboutGradeCheck.GetGradeCheckItemById(gradeCheckId);

        string gradeCheckItemName = string.Empty;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            gradeCheckItemName = ds.Tables[0].Rows[i]["gradeCheckItemName"].ToString().Trim();
        }
        return gradeCheckItemName + "：";
    }


    protected string GetStudentGradeCheckConfirm(DateTime updateTime)
    {
        DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
        DataSet ds = dalOperationAboutGradeCheck.GetStudentGradeCheckConfirm(studentNo, updateTime);

        string isAccord = string.Empty;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            isAccord = ds.Tables[0].Rows[i]["isAccord"].ToString().Trim();
        }
        return isAccord;
    }



    protected string GetStudentGradeCheckConfirmAboutRemark(DateTime updateTime)
    {
        DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
        DataSet ds = dalOperationAboutGradeCheck.GetStudentGradeCheckConfirm(studentNo, updateTime);

        string remark = string.Empty;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            remark = ds.Tables[0].Rows[i]["remark"].ToString().Trim();
        }
        return remark;
    }

    //判断是否是规定的办理时间范围
    protected bool isAllowTime()
    {
        DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
        return dalOperationAboutGradeCheck.CheckGradeCheckAllowTime();
    }

    protected string GetGradeCheckApplyInfo(DateTime updateTime)
    {
        StringBuilder sb = new StringBuilder();
        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
        //DataTable dt = dal.GetStudentGradeCheckApplyByStudentNoAndUpdateTime(studentNo, updateTime).Tables[0];
        DataTable dt = dal.GetStudentGradeCheckApplyByStudentNo(studentNo).Tables[0];

        if (dt.Rows.Count > 0)
        {
            sb.Append("<br /><b>当前已申请的重修重考记录：</b><br />");
        }

        int rowsCount = dt.Rows.Count;

        for (int i = 0; i < rowsCount; i++)
        {
            sb.Append("<table class=\"datagrid2\"><th>详细信息</th>");
            sb.Append("<tr><td>所属学期：" + CommonUtility.ChangeTermToString(dt.Rows[i]["termTag"].ToString().Trim()) + "</td></tr><tr><td>课程名称：" + dt.Rows[i]["courseName"].ToString().Trim() + "</td></tr><tr><td>申请时间：" + dt.Rows[i]["updateTime"].ToString().Trim() + "</td></tr><tr><td>审核结果：" + (string.IsNullOrEmpty(dt.Rows[i]["applyResult"].ToString().Trim()) ? "未处理" : dt.Rows[i]["applyResult"].ToString().Trim()) + "</td></tr><tr><td>审核意见：" + dt.Rows[i]["applyChecKSuggestion"].ToString().Trim() + "</td></tr>");
            sb.Append("</table>");
        }

        return sb.ToString();
    }

    //展现确认无疑问、确认有疑问等按钮
    protected string DisplayConfirmPanel(int itemIndex, DateTime updateTime)
    {
        StringBuilder sb = new StringBuilder();

        DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
        DataTable dt = dal.GetStudentGradeCheckConfirmRecordNum(studentNo, updateTime).Tables[0];

        int recordNum = dt.Rows.Count;

        if (recordNum > 0)
        {
            sb.Append("<br />当前状态：确认无疑问&nbsp;&nbsp;");
        }
        else
        {
            sb.Append("<br />当前状态：暂未确认&nbsp;&nbsp;");
        }

        return sb.ToString();

    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        DateTime _startTime = Convert.ToDateTime(startTime.Value.Trim());
        DateTime _endTime = Convert.ToDateTime(endTime.Value.Trim());
        if (_startTime > _endTime)
        {
            Javascript.GoHistory(-1, "开始时间不能晚于截止时间，请修改:)", Page);
            return;
        }

        try
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
            dal.UpdateGradeCheckAllowTime(Convert.ToDateTime(_startTime), Convert.ToDateTime(_endTime));

            dal.UpdateGradeCheckNotify(txtNotifyTitle.Text.Trim(), txtNotifyContent.Text.Trim(), hidAttachmentId.Value);

            Javascript.ExcuteJavascriptCode("delBeforeUnloadEvent();", Page);
            Javascript.AlertAndRedirect("更新重修重考通知及开放时间成功：）", "/Administrator/StudentManager.aspx?fragment=9", Page);
        }
        catch (Exception ex)
        {
            MongoDBLog.LogRecord(ex);
            Javascript.ExcuteJavascriptCode("delBeforeUnloadEvent();", Page);
            Javascript.GoHistory(-1, "更新重修重考通知及开放时间失败：（", Page);
            return;
        }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            dal.UpdateGradeCheckExcelTemplate(hidAttachmentId1.Value);

            Javascript.AlertAndRedirect("更新重修重考Excel模板成功：）", "/Administrator/StudentManager.aspx?fragment=8", Page);
        }
        catch (Exception ex)
        {
            MongoDBLog.LogRecord(ex);
            Javascript.GoHistory(-1, "更新重修重考Excel模板失败：（", Page);
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_termYears"></param>
    /// <returns></returns>
    public string formatTermYears(string _termYears){
        string _format = string.Empty;
        if(_termYears.Trim().Length >0){
           string[] _items =  _termYears.Split(",".ToCharArray());
           for (int i = 0; i < _items.Length; i++)
           {
               _format += "20" + _items[i] + "学年&nbsp;&nbsp;";
           }
        }
        return _format;
    }
}