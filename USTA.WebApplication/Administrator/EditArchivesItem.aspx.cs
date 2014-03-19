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
    public partial class EditArchivesItem : CheckUserWithCommonPageBase
    {
        int archiveItemId = (HttpContext.Current.Request["archiveItemId"] == null) ? 0 : int.Parse(HttpContext.Current.Request["archiveItemId"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tryParseInt = 0;
                if (CommonUtility.SafeCheckByParams<string>(Request["archiveItemId"], ref tryParseInt))
                {
                    //获取Url中的参数
                    archiveItemId = tryParseInt;
                }

                InitialNotifyEdit(archiveItemId);
            }
        }

        //初始化编辑页面
        public void InitialNotifyEdit(int archiveItemId)
        {
            DalOperationAboutArchivesItems dal = new DalOperationAboutArchivesItems();
            DataTable dt = dal.GetArchivesItemById(archiveItemId).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txtArchiveItemName.Text = dt.Rows[i]["archiveItemName"].ToString().Trim();

                txtRemark.Text = dt.Rows[i]["remark"].ToString().Trim();

                if(dt.Rows[i]["teacherType"].ToString().Trim() == "教师")
                {
                    ddlTeacherType.Items.Add(new ListItem("教师", "教师"));
                    ddlTeacherType.Items.Add(new ListItem("助教", "助教"));
                }
                else
                {
                    ddlTeacherType.Items.Add(new ListItem("助教", "助教"));
                    ddlTeacherType.Items.Add(new ListItem("教师", "教师"));
                }

                DalOperationAboutCourses doac = new DalOperationAboutCourses();
                DataTable _dt = doac.FindAllTermTags().Tables[0];

                string termTag = string.Empty;

                for (int j = 0; j < _dt.Rows.Count; j++)
                {
                    termTag = _dt.Rows[j]["termTag"].ToString().Trim();
                    ListItem li = new ListItem(CommonUtility.ChangeTermToString(termTag), termTag);
                    ddlTermTags.Items.Add(li);
                }

                for (int x = 0; x < ddlTermTags.Items.Count; x++)
                {
                    if (dt.Rows[i]["termTag"].ToString().Trim() == ddlTermTags.Items[x].Value)
                    {
                        ddlTermTags.SelectedIndex = x;
                    }
                }
            }

        }
        //修改
        protected void btnUpdate_Click(object sender, EventArgs e)
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
                model.archiveItemId = archiveItemId;
                model.termTag = ddlTermTags.SelectedValue;

                try
                {

                    dal.UpdateArchivesItemById(model);//修改
                    Javascript.RefreshParentWindow("修改结课资料规则成功！", "/Administrator/ArchivesManage.aspx?fragment=2", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "修改结课资料规则失败,请检查格式是否有误！", Page);
                }

            }
        }
    }
}