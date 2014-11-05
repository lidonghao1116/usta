using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using USTA.Bll;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;

namespace USTA.WebApplication.Student
{
    public partial class EditGradeCheckApply : CheckUserWithCommonPageBase
    {
        public DateTime updateTime = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            if (!dal.CheckGradeCheckAllowTime())
            {
                Javascript.ExcuteJavascriptCode("alert('当前暂未开放办理重修重考');parent.tb_remove();", Page);
                return;
            }

            if (!IsPostBack)
            {
                //绑定学期标识下拉列表
                DataBindTermTagList();
                //绑定课程列表--学期标识(termTag)
                DataBindSearchCourse();
                this.GetGradeCheckApplyInfo(updateTime);
            }
        }


        //判断是否是规定的办理时间范围
        protected bool isAllowTime()
        {
            DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
            return dalOperationAboutGradeCheck.CheckGradeCheckAllowTime();
        }


        protected void GetGradeCheckApplyInfo(DateTime updateTime)
        {
            if (!this.isAllowTime())
            {
                Javascript.ExcuteJavascriptCode("alert('当前暂未开放办理重修重考');parent.tb_remove();", Page);
                return;
            }

            string currentDocumentInfo = string.Empty;

            StringBuilder sb = new StringBuilder();
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
            DataTable dt = dal.GetStudentGradeCheckApplyByStudentNoAndUpdateTimeNotDeal(UserCookiesInfo.userNo, updateTime).Tables[0];


            if (dt.Rows.Count > 0)
            {
                sb.Append("<br /><b>当前已申请并且未处理的重修重考记录：</b><br />");
            }

            int rowsCount = dt.Rows.Count;

            for (int i = 0; i < rowsCount; i++)
            {
                sb.Append("<table class=\"datagrid2\"><th>详细信息</th>");
                sb.Append("<tr><td>所属学期：" + CommonUtility.ChangeTermToString(dt.Rows[i]["termTag"].ToString().Trim()) + "课程名称：" + dt.Rows[i]["courseName"].ToString().Trim() + "</td></tr><tr><td>原因：" + dt.Rows[i]["applyReason"].ToString().Trim() + "</td></tr><tr><td>审核结果：" + dt.Rows[i]["applyResult"].ToString().Trim() + "</td></tr><tr><td>审核意见：" + dt.Rows[i]["applyChecKSuggestion"].ToString().Trim() + "</td></tr>");
                sb.Append("</table>");
            }
            ltlApplyInfo.Text = sb.ToString();
        }

        //绑定学期标识下拉列表
        /// <summary>
        /// 
        /// </summary>
        public void DataBindTermTagList()
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            string termTag = DalCommon.GetTermTag(doac.conn);
            ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
            ddlTermTags.Items.Add(li);
        }

        //下拉列表事件
        protected void ddlTermTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            //绑定课程列表--学期标识(termTag)
            DataBindSearchCourse();
        }

        //绑定搜索的课程信息
        public void DataBindSearchCourse()
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataView dv = doac.SearchCourses(ddlTermTags.SelectedValue, txtGradeCheckCourses.Text.Trim()).Tables[0].DefaultView;


            this.dlstcourses.DataSource = dv;
            this.dlstcourses.DataBind();

            if (dv.Count > 0)
            {
                this.dlstcourses.ShowFooter = false;
            }
        }

        //搜索课程列表
        protected void btnCommit_Click(object sender, EventArgs e)
        {
            //绑定搜索的课程信息
            DataBindSearchCourse();
        }


        protected void Button3_Click(object sender, EventArgs e)
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();

            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

            List<StudentsGradeCheckApply> listStudentsGradeCheckApply = new List<StudentsGradeCheckApply>();

            bool isChecked = false;

            for (int i = 0; i < dlstcourses.Items.Count; i++)
            {

                System.Web.UI.WebControls.CheckBox ck = dlstcourses.Items[i].FindControl("select") as System.Web.UI.WebControls.CheckBox;
                System.Web.UI.WebControls.DropDownList ddl = dlstcourses.Items[i].FindControl("ddlGradeCheckApplyType") as System.Web.UI.WebControls.DropDownList;
                System.Web.UI.WebControls.DropDownList ddlApplyReason = dlstcourses.Items[i].FindControl("ddlApplyReason") as System.Web.UI.WebControls.DropDownList;
                System.Web.UI.HtmlControls.HtmlGenericControl ltl = dlstcourses.Items[i].FindControl("courseName") as System.Web.UI.HtmlControls.HtmlGenericControl;
                System.Web.UI.HtmlControls.HtmlGenericControl ltlCourseNo = dlstcourses.Items[i].FindControl("courseNo") as System.Web.UI.HtmlControls.HtmlGenericControl;
                System.Web.UI.HtmlControls.HtmlGenericControl ltlClassID = dlstcourses.Items[i].FindControl("ClassID") as System.Web.UI.HtmlControls.HtmlGenericControl;
                System.Web.UI.HtmlControls.HtmlGenericControl ltlTermTag = dlstcourses.Items[i].FindControl("termTag") as System.Web.UI.HtmlControls.HtmlGenericControl;

                if (ck != null && ck.Checked)
                {
                    isChecked = true;
                    listStudentsGradeCheckApply.Add(new StudentsGradeCheckApply { studentNo = UserCookiesInfo.userNo, updateTime = updateTime, applyUpdateTime = DateTime.Now, courseNo = ltlCourseNo.InnerText.Trim(), ClassID = ltlClassID.InnerText.Trim(), termTag = ltlTermTag.InnerText.Trim(), gradeCheckApplyType = ddl.SelectedValue, applyReason = ddlApplyReason.SelectedValue });
                }
            }

            if (!isChecked)
            {
                Javascript.GoHistory(-1, "请选择要进行重修重考的课程:)", Page);
                return;
            }

            //加申请记录唯一性验证

            for (int i = 0; i < listStudentsGradeCheckApply.Count; i++)
            {
                DataSet _temp = dal.GetStudentGradeCheckApplyByStudentNoAndTermTagCourseNoClassID(listStudentsGradeCheckApply[i].studentNo, listStudentsGradeCheckApply[i].termTag + listStudentsGradeCheckApply[i].courseNo + listStudentsGradeCheckApply[i].ClassID);
                
                if (_temp.Tables[0].Rows.Count > 0 && _temp.Tables[0].Rows[0]["applyResult"].ToString().Trim() == "符合")
                {
                    Javascript.GoHistory(-1, "申请重修重考失败，失败原因：\\n当前已经申请了“" + listStudentsGradeCheckApply[i].couseName + "”的重修重考记录，并且已经审核通过，无须重新申请：（", Page);
                    return;
                }
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    dal.DeleteStudentGradeCheckApplyByStudentNoAndUpdateTime(UserCookiesInfo.userNo, updateTime);

                    for (int i = 0; i < listStudentsGradeCheckApply.Count; i++)
                    {
                        dal.AddStudentGradeCheckApply(listStudentsGradeCheckApply[i]);
                    }
                    scope.Complete();
                    Javascript.RefreshParentWindow("修改申请重修重考成功:)", "/Student/MyGradeCheckManage.aspx?fragment=1", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "修改申请重修重考失败，请重试:(", Page);
                }
                finally
                {
                    dal.conn.Close();
                }
            }
        }



        //绑定重修重考原因
        protected void dlstcourses_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlApplyReason");

                DalOperationAboutGradeCheckApplyReason dal = new DalOperationAboutGradeCheckApplyReason();

                DataTable dt = dal.GetAll().Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["gradeCheckApplyReasonTitle"].ToString().Trim(), dt.Rows[i]["gradeCheckApplyReasonTitle"].ToString().Trim()));
                }
            }
        }
    }
}