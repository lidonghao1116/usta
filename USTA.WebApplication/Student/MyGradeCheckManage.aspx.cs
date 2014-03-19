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

namespace USTA.WebApplication.Student
{
    public partial class MyGradeCheckManage : System.Web.UI.Page
    {
        public string fragmentFlag = "1";
        public string studentNo = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            studentNo = UserCookiesInfo.userNo;

            if (!IsPostBack)
            {
                //控制Tab的显示

                if (Request["fragment"] != null)
                {
                    fragmentFlag = Request["fragment"];
                }

                CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, divFragment1, divFragment2, divFragment3);


                if (fragmentFlag.Equals("1"))
                {
                    DlstStudentSchoolClassNameDataBind();
                    DlstStudentGradeCheckDataBind();
                }

                deleteGradeCheckApplyById();
            }
        }

        protected void deleteGradeCheckApplyById()
        {
            DalOperationAboutGradeCheck doan = new DalOperationAboutGradeCheck();

            int _gradeCheckApplyId = -1;
            if (Request["action"] != null && Request["action"].Trim() == "delete" && CommonUtility.SafeCheckByParams<string>(Request["gradeCheckApplyId"],ref _gradeCheckApplyId))
            {
                if (!this.isAllowTime())
                {
                    Javascript.GoHistory(-1, "当前暂未开放办理重修重考", Page);
                    return;
                }
                doan.DeleteStudentGradeCheckApplyById(_gradeCheckApplyId, studentNo);
                Javascript.AlertAndRedirect("删除成功！", "/Student/MyGradeCheckManage.aspx", Page);
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

            DataTable dt = dal.GetGradeCheckNotify().Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["notifyTitle"].ToString().Trim().Length > 0)
                {

                    notifyTitle.NavigateUrl = "~/Common/ViewGradeCheckNotify.aspx?keepThis=true&TB_iframe=true&height=500&width=800";
                    notifyTitle.CssClass = "thickbox";
                    notifyTitle.Text = dt.Rows[i]["notifyTitle"].ToString().Trim() + "(点击查看重修重考详细通知)";

                    notifyTitle.ToolTip = dt.Rows[i]["notifyTitle"].ToString().Trim();
                }
            }
        }



        protected void dlstStudentGradeCheck_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    DataList dataList = (DataList)e.Item.FindControl("dlstStudentGradeCheckItem");
            //    DataRowView rowv = (DataRowView)e.Item.DataItem;
            //    string updateTime = rowv["updateTime"].ToString().Trim();
            //    DalOperationAboutGradeCheck dalOperationAboutGradeCheck = new DalOperationAboutGradeCheck();
            //    DataSet ds = dalOperationAboutGradeCheck.GetGradeCheckDetailByStudentNo(studentNo, updateTime);
            //    dataList.DataSource = ds.Tables[0].DefaultView;
            //    dataList.DataBind();

            //    if (dataList.Items.Count == 0)
            //    {
            //        dataList.ShowFooter = true;
            //    }
            //    else
            //    {
            //        dataList.ShowFooter = false;
            //    }
            //}
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


        protected string GetGradeCheckApplyInfo(int gradeCheckApplyId)
        {
            StringBuilder sb = new StringBuilder();
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
            DataTable dt = dal.GetStudentGradeCheckApplyByStudentNoAndUpdateTime(UserCookiesInfo.userNo, gradeCheckApplyId).Tables[0];


            if (dt.Rows.Count > 0)
            {
                sb.Append("<br /><b>当前已申请的重修重考记录：</b><br />");
            }

            //是否有审核未通过记录
            bool isHasNotAccord = false;
            //检测是否有未审核过的记录
            bool isHasNotCheck = false;

            int rowsCount = dt.Rows.Count;

            for (int i = 0; i < rowsCount; i++)
            {
                sb.Append("<table class=\"datagrid2\"><th>详细信息</th>");
                sb.Append("<tr><td>所属学期：" + CommonUtility.ChangeTermToString(dt.Rows[i]["termTag"].ToString().Trim()) + "课程名称：" + dt.Rows[i]["courseName"].ToString().Trim() + "</td></tr><tr><td>审核结果：" + dt.Rows[i]["applyResult"].ToString().Trim() + "</td></tr><tr><td>原因：" + dt.Rows[i]["applyReason"].ToString().Trim() + "</td></tr><tr><td>审核意见：" + dt.Rows[i]["applyChecKSuggestion"].ToString().Trim() + "</td></tr>" + (string.IsNullOrEmpty(dt.Rows[i]["applyResult"].ToString().Trim()) ? "<tr><td><a href=\"?action=delete&gradeCheckApplyId=" + dt.Rows[i]["gradeCheckApplyId"].ToString().Trim() + "\"  onclick=\"return deleteTip();\">删除</a></td></tr>" : string.Empty));

                if (dt.Rows[i]["applyResult"].ToString().Trim() == "不符合")
                {
                    isHasNotAccord = true;
                }

                if (string.IsNullOrEmpty(dt.Rows[i]["applyResult"].ToString().Trim()))
                {
                    isHasNotCheck = true;
                }

                sb.Append("</table>");
            }

            if (this.isAllowTime())
            {
                //if (rowsCount == 0)
                //{
                //    sb.Append("<a href=\"GradeCheckApply.aspx?updateTime=" + updateTime + "&keepThis=true&TB_iframe=true&height=500&width=800\" title=\"申请办理重修重考\" class=\"thickbox\">申请办理重修重考</a>&nbsp;&nbsp;&nbsp;&nbsp;");
                //}
                if (rowsCount > 0 && (isHasNotAccord || isHasNotCheck))
                {
                    sb.Append("<a href=\"EditGradeCheckApply.aspx?gradeCheckApplyId=" + gradeCheckApplyId + "&keepThis=true&TB_iframe=true&height=500&width=800\" title=\"修改重修重考申请\" class=\"thickbox\">修改重修重考申请</a>&nbsp;&nbsp;&nbsp;&nbsp;");
                }
            }

            return sb.ToString();
        }       
    }
}