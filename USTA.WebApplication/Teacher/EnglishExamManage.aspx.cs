using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using USTA.Bll;

namespace USTA.WebApplication.Teacher
{
    public partial class EnglishExamManage : System.Web.UI.Page
    {
        string fragmentFlag = "1";
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //控制Tab的显示

                if (Request["fragment"] != null)
                {
                    fragmentFlag = Request["fragment"];
                }

                CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                    , divFragment1, divFragment2, divFragment3);

                if (fragmentFlag.Equals("1"))
                {
                    DataBindSchoolClassList(ddlSerachSchoolClass);
                    DataBindEnglishExamSignUpInfo();
                }
                else if (fragmentFlag.Equals("2"))
                {
                    DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();
                    DataSet ds = doac.GetAllEnglishExamNotify();
                    DataRowCollection drc = ds.Tables[0].Rows;

                    if (drc.Count == 0)
                    {
                        Javascript.AlertAndRedirect("当前暂无需要处理的四六级通知！", "/Teacher/EnglishExamManage.aspx?fragment=1", Page);
                    }

                    for (int i = 0; i < drc.Count; i++)
                    {
                        ddlEnglishExamNotify.Items.Add(new ListItem(drc[i]["englishExamNotifyTitle"].ToString().Trim(), drc[i]["englishExamNotifyId"].ToString().Trim()));
                    }
                    //绑定班级标识下拉列表
                    DataBindSchoolClassList(ddlSchoolClass);
                }
                else if (fragmentFlag.Equals("3"))
                {
                    DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();
                    DataSet ds = doac.GetAllEnglishExamNotify();
                    DataRowCollection drc = ds.Tables[0].Rows;


                    if (drc.Count == 0)
                    {
                        Javascript.AlertAndRedirect("当前暂无需要处理的四六级通知！", "/Teacher/EnglishExamManage.aspx?fragment=1", Page);
                    }

                    for (int i = 0; i < drc.Count; i++)
                    {
                        ddlEnglishExamNotifyExcel.Items.Add(new ListItem(drc[i]["englishExamNotifyTitle"].ToString().Trim(), drc[i]["englishExamNotifyId"].ToString().Trim()));
                    }
                    //绑定班级标识下拉列表
                    DataBindSchoolClassList(ddlSchoolClassExcel);
                }
            }
        }
        //第1个标签；开始
        protected void ddlSerachSchoolClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKeyword.Text = string.Empty;
            DataBindEnglishExamSignUpInfo();
        }

        //绑定班级下拉列表
        public void DataBindSchoolClassList(DropDownList ddl)
        {
            DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            DataTable dt = doac.GetSchoolClassByTeacherNo(user.userNo).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl.Items.Add(new ListItem(dt.Rows[i]["className"].ToString().Trim(), dt.Rows[i]["SchoolClassID"].ToString().Trim()));
            }
        }

        //搜索课程列表
        protected void btnCommit_Click(object sender, EventArgs e)
        {
            //绑定搜索的课程信息
            DataBindEnglishExamSignUpInfo();
        }

        //绑定搜索的四六级报名信息
        public void DataBindEnglishExamSignUpInfo()
        {
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();
            DataView dv = doac.GetEnglishExamSignUpInfoByTeacherNo(user.userNo, ddlSerachSchoolClass.SelectedValue == "all", ddlSerachSchoolClass.SelectedValue, txtKeyword.Text.Trim().Length > 0, txtKeyword.Text.Trim(),false).Tables[0].DefaultView;

            this.AspNetPager2.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dv;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = pageIndex - 1;
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

        //搜索的学期列表分页
        protected void AspNetPager2_PageChanged(object sender, EventArgs e)
        {
            DataBindEnglishExamSignUpInfo();
        }


        //搜索课程列表
        protected void btnCommitIspaid_Click(object sender, EventArgs e)
        {
            DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();

            try
            {
                doac.BatchUpdateSignUpInfoState("isPaid", ddlIspaid.SelectedValue, int.Parse(ddlEnglishExamNotify.SelectedValue), ddlSchoolClass.SelectedValue);
                Javascript.AlertAndRedirect("更改当前班级所有报名学生缴费状态成功！", "/Teacher/EnglishExamManage.aspx?fragment=1", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "更改当前班级所有报名学生缴费状态失败,请检查格式是否有误！", Page);
            }
        }


        //搜索课程列表
        protected void btnCommitExamCertificate_Click(object sender, EventArgs e)
        {
            DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();

            try
            {
                doac.BatchUpdateSignUpInfoState("examCertificateState", ddlExamCertificate.SelectedValue, int.Parse(ddlEnglishExamNotify.SelectedValue), ddlSchoolClass.SelectedValue);
                Javascript.AlertAndRedirect("更改当前班级所有报名学生准考证状态成功！", "/Teacher/EnglishExamManage.aspx?fragment=1", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "更改当前班级所有报名学生准考证状态失败,请检查格式是否有误！", Page);
            }

        }


        //搜索课程列表
        protected void btnCommitGradeCertificate_Click(object sender, EventArgs e)
        {
            DalOperationAboutEnglishExam doac = new DalOperationAboutEnglishExam();

            try
            {
                doac.BatchUpdateSignUpInfoState("gradeCertificateState", ddlGradeCertificate.SelectedValue, int.Parse(ddlEnglishExamNotify.SelectedValue), ddlSchoolClass.SelectedValue);
                Javascript.AlertAndRedirect("更改当前班级所有报名学生成绩单状态成功！", "/Teacher/EnglishExamManage.aspx?fragment=1", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "更改当前班级所有报名学生成绩单状态失败,请检查格式是否有误！", Page);
            }
            
        }
    }
}