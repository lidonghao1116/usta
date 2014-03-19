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
using USTA.PageBase;
using System.Configuration;

namespace USTA.WebApplication.Administrator
{
    public partial class AddArchivesItem : CheckUserWithCommonPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindTermTagList();
            }
        }

        /// <summary>
        /// 绑定学期标识下拉列表
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
        }


        //添加
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtArchiveItemName.Text.Trim().Length == 0)
            {
                Javascript.GoHistory(-1, "请输入要上传的资料名称！", Page);
            }
            else
            {
                DalOperationAboutArchivesItems dal = new DalOperationAboutArchivesItems();
                ArchivesItems model = new ArchivesItems();
                model.archiveItemName = txtArchiveItemName.Text.Trim();
                model.remark = txtRemark.Text.Trim();
                model.teacherType = ddlTeacherType.SelectedValue;
                model.termTag = ddlTermTags.SelectedValue;

                try
                {
                    dal.AddArchivesItems(model);//添加
                    Javascript.RefreshParentWindow("添加结课资料规则成功！", "/Administrator/ArchivesManage.aspx?fragment=2", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "添加结课资料规则失败,请检查格式是否有误！", Page);
                }

            }
        }
    }
}