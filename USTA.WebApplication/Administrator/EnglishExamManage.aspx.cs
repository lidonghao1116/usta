using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.Bll;

namespace USTA.WebApplication.Administrator
{
    public partial class EnglishExamManage : System.Web.UI.Page
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 
        public int fileFolderType = (int)FileFolderType.englishExam;

        public string schoolClassName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3,liFragment4
                , divFragment1, divFragment2, divFragment3,divFragment4);

            if (fragmentFlag.Equals("1"))
            {
                if (!IsPostBack)
                {
                    DataListBindEnglishExamNotify();
                }
            }
            if (fragmentFlag.Equals("2"))
            {
                if (!IsPostBack)
                {
                    Javascript.ExcuteJavascriptCode("initBeforeUnloadEvent('温馨提示：当前页面数据可能未保存哟~（此为提示，并不代表您真正未保存数据），确定离开吗？');", Page);
                    txtTitle.Attributes.Add("class", "required");
                }

            }
            if (fragmentFlag.Equals("3"))
            {
                txtTitle.Attributes.Remove("class");
                datepicker.Attributes.Remove("class");
                if (!IsPostBack)
                {
                    DataBindSchoolClassList(ddlSerachSchoolClass);
                    DataBindEnglishExamSignUpInfo();
                }
            }

            if (fragmentFlag.Equals("4"))
            {
                DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();
                DataSet ds = doac.GetAllEnglishExamNotify();
                DataRowCollection drc = ds.Tables[0].Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    ddlEnglishExamNotifyExcel.Items.Add(new ListItem(drc[i]["englishExamNotifyTitle"].ToString().Trim(), drc[i]["englishExamNotifyId"].ToString().Trim()));
                }
                //绑定班级标识下拉列表
                DataBindSchoolClassList(ddlSchoolClassExcel);
            }
        }
        //绑定班级下拉列表
        public void DataBindSchoolClassList(DropDownList ddl)
        {
            DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();
            DataTable dt = doac.GetAllSchoolClass().Tables[0];

            schoolClassName = "{";

            List<string> schoolClassNameTemp = new List<string>();

            ddl.Items.Add(new ListItem("全部班级", "all"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl.Items.Add(new ListItem(dt.Rows[i]["className"].ToString().Trim(), dt.Rows[i]["SchoolClassID"].ToString().Trim()));
                schoolClassNameTemp.Add("\"" + dt.Rows[i]["className"].ToString().Trim() + "\":" + "\"" + dt.Rows[i]["SchoolClassID"].ToString().Trim()+"\"");
            }

            schoolClassName += String.Join(",", schoolClassNameTemp.ToArray());

            schoolClassName += "}";
        }
        //绑定搜索的四六级报名信息
        public void DataBindEnglishExamSignUpInfo()
        {
            DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();
            DataView dv = doac.GetEnglishExamSignUpInfoByTeacherNoAndLocale("admin", ddlSerachSchoolClass.SelectedValue == "all", ddlSerachSchoolClass.SelectedValue, txtKeyword.Text.Trim().Length > 0, txtKeyword.Text.Trim(), false, ddlSearchLocale.SelectedValue).Tables[0].DefaultView;

            this.AspNetPager2.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dv;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = AspNetPager2.CurrentPageIndex - 1;
            pds.PageSize = CommonUtility.pageSize; ;

            this.dlstEnglishExamSignUpInfo.DataSource = pds;
            this.dlstEnglishExamSignUpInfo.DataBind();

            if (pds.Count == 0)
            {
                this.dlstEnglishExamSignUpInfo.ShowFooter = true;
            }
            else
            {
                this.dlstEnglishExamSignUpInfo.ShowFooter = false;
            }
        }

        //第1个标签；开始
        protected void ddlEnglishExamNotifyLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataListBindEnglishExamNotify();
        }

        
        //第1个标签；开始
        protected void ddlSearchLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindEnglishExamSignUpInfo();
        }

        //搜索的学期列表分页
        protected void AspNetPager2_PageChanged(object sender, EventArgs e)
        {
            DataBindEnglishExamSignUpInfo();
        }
        //搜索课程列表
        protected void btnCommit_Click(object sender, EventArgs e)
        {
            //绑定搜索的课程信息
            DataBindEnglishExamSignUpInfo();
        }

        public void DataListBindEnglishExamNotify()
        {
            DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
            DataTable dt = dalOperationAboutEnglishExam.GetEnglishExamNotifyByLocale(ddlEnglishExamNotifyLocale.SelectedValue).Tables[0];
            DataView dv = dt.DefaultView;

            this.AspNetPager1.RecordCount = dv.Count;
            AspNetPager1.PageSize = CommonUtility.pageSize;

            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dv;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = pageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;

            this.dlstEnglishExamNotify.DataSource = pds;
            this.dlstEnglishExamNotify.DataBind();

            if (pds.Count == 0)
            {
                this.dlstEnglishExamNotify.ShowFooter = true;
            }
            else
            {
                this.dlstEnglishExamNotify.ShowFooter = false;
            }
        }
        //第1个标签：结束
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim().Length == 0 || Textarea1.Value.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "标题和内容不能为空，请输入！", Page);
                Javascript.ExcuteJavascriptCode("delBeforeUnloadEvent();", Page);
            }
            else
            {
                DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
                EnglishExamNotify englishExamNotify = new EnglishExamNotify();
                englishExamNotify.englishExamNotifyTitle = txtTitle.Text.Trim();
                englishExamNotify.englishExamNotifyContent = Textarea1.Value.Trim();
                englishExamNotify.deadLineTime = Convert.ToDateTime(datepicker.Value.Trim());
                englishExamNotify.locale = ddlLocale.SelectedValue;

                //以下提交附件的判断与相关操作
                if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
                {
                    englishExamNotify.attachmentIds = hidAttachmentId.Value;//保存了附件并且返回了attachmentId(自增长类型主键)
                }

                try
                {
                    dalOperationAboutEnglishExam.AddEnglishExamNotify(englishExamNotify);//保存通知
                    Javascript.ExcuteJavascriptCode("delBeforeUnloadEvent();", Page);
                    Javascript.AlertAndRedirect("添加成功！", "/Administrator/EnglishExamManage.aspx?fragment=1", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.ExcuteJavascriptCode("delBeforeUnloadEvent();", Page);
                    Javascript.GoHistory(-1, "添加失败,请检查格式是否有误！", Page);
                }

            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataListBindEnglishExamNotify();
        }
        //第1个标签；开始
        protected void ddlSerachSchoolClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKeyword.Text = string.Empty;
            DataBindEnglishExamSignUpInfo();
        }

        protected void dlstEnglishExamNotify_ItemCommand(object source, DataListCommandEventArgs e)
        {
            DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
            if (e.CommandName == "delete")
            {
                string englishExamNotifyInfoId = dlstEnglishExamNotify.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号  
                dalOperationAboutEnglishExam.DeleteEnglishExamNotifyById(int.Parse(englishExamNotifyInfoId));
                Javascript.AlertAndRedirect("删除成功！", "/Administrator/EnglishExamManage.aspx?fragment=1", Page);
            }
            else if (e.CommandName == "update")
            {
                string englishExamNotifyInfoId = dlstEnglishExamNotify.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号 
                Javascript.JavaScriptLocationHref("/Administrator/EditEnglishExamNotifyInfo.aspx?englishExamNotifyInfoId=" + englishExamNotifyInfoId, Page);
            }

        }
    }
}