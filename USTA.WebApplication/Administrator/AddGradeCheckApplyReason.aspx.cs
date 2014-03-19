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
    public partial class AddGradeCheckApplyReason : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtGradeCheckApplyReasonTitle.Text.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "请输入重修重考原因标题：）", Page);
            }
            else
            {
                DalOperationAboutGradeCheckApplyReason dal = new DalOperationAboutGradeCheckApplyReason();
                StudentsGradeCheckApplyReason model = new StudentsGradeCheckApplyReason();
                model.gradeCheckApplyReasonTitle = txtGradeCheckApplyReasonTitle.Text.Trim();
                model.gradeCheckApplyReasonRemark = txtGradeCheckApplyReasonRemark.Text.Trim();

                try
                {

                    dal.Add(model);//添加
                    Javascript.RefreshParentWindow("添加重修重考原因成功！", "/Administrator/StudentManager.aspx?fragment=3", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "添加重修重考原因失败,请检查格式是否有误：(", Page);
                }

            }
        }
    }
}