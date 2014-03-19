using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Configuration;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class EditGradeCheckApplyReason : CheckUserWithCommonPageBase
    {
        int gradeCheckApplyReasonId = (HttpContext.Current.Request["gradeCheckApplyReasonId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["gradeCheckApplyReasonId"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tryParseInt = 0;
                if (CommonUtility.SafeCheckByParams<string>(Request["gradeCheckApplyReasonId"], ref tryParseInt))
                {
                    //获取Url中的参数
                    gradeCheckApplyReasonId = tryParseInt;
                }

                InitialNotifyEdit(gradeCheckApplyReasonId);
            }
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int gradeCheckApplyReasonId)
        {
            DalOperationAboutGradeCheckApplyReason dal = new DalOperationAboutGradeCheckApplyReason();
            DataTable dt = dal.Get(gradeCheckApplyReasonId).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txtGradeCheckApplyReasonTitle.Text = dt.Rows[i]["gradeCheckApplyReasonTitle"].ToString().Trim();

                txtGradeCheckApplyReasonRemark.Text = dt.Rows[i]["gradeCheckApplyReasonRemark"].ToString().Trim();
            }
        }
        //修改
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtGradeCheckApplyReasonTitle.Text.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "请输入重修重考原因标题：）", Page);
            }
            else
            {
                DalOperationAboutGradeCheckApplyReason dalOperationAboutGradeCheck = new DalOperationAboutGradeCheckApplyReason();
                StudentsGradeCheckApplyReason model = new StudentsGradeCheckApplyReason();
                model.gradeCheckApplyReasonTitle = txtGradeCheckApplyReasonTitle.Text.Trim();
                model.gradeCheckApplyReasonRemark = txtGradeCheckApplyReasonRemark.Text.Trim();
                model.gradeCheckApplyReasonId = gradeCheckApplyReasonId;

                try
                {

                    dalOperationAboutGradeCheck.Update(model);//修改
                    Javascript.RefreshParentWindow("修改重修重考原因成功！", "/Administrator/StudentManager.aspx?fragment=3", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "修改重修重考原因失败,请检查格式是否有误！", Page);
                }

            }
        }
    }
}