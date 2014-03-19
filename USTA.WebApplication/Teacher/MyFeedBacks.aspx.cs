using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using USTA.Bll;
using USTA.Dal;
using USTA.Model;
using USTA.Common;

namespace USTA.WebApplication.Teacher
{
    public partial class MyFeedBacks : System.Web.UI.Page
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                TagView();
                DataListBind();
            }
        }
        //绑定用户信息到DataList
        public void DataListBind()
        {
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationFeedBack dou = new DalOperationFeedBack();
            DataView dv = dou.FindFeedBack(user.userNo).DefaultView;

            this.AspNetPager1.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dv;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = pageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;

            this.dlstCETFeedback.DataSource = pds;
            this.dlstCETFeedback.DataBind();

            if (this.dlstCETFeedback.Items.Count == 0)
            {
                btnDelete.Visible = false;
            }
            else
            {
                btnDelete.Visible = true;
            }

            if (pds.Count > 0)
            {
                this.dlstCETFeedback.ShowFooter = false;
            }
        }
        //显示反馈意见的已读与未读的条目数量
        public void TagView()
        {
            DalOperationFeedBack dou = new DalOperationFeedBack();
            DataTable dtNotRead = dou.FindFeedBackByIsRead(0);
            DataTable dtHaveRead = dou.FindFeedBackByIsRead(1);
            int countNotRead = dtNotRead.Rows.Count;
            int countHaveRead = dtHaveRead.Rows.Count;
            this.lblCount.Text = "您共有" + countNotRead.ToString() + "条未阅读";
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataListBind();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DalOperationFeedBack dou = new DalOperationFeedBack();
            int feedbackId;
            foreach (DataListItem item in this.dlstCETFeedback.Items)
            {

                CheckBox chkItem = (CheckBox)item.FindControl("ChkBox");
                Label lbl = (Label)item.FindControl("lblfeedBackId");
                if (chkItem.Checked)
                {
                    //被勾选的要删除
                    feedbackId = int.Parse(lbl.Text);
                    dou.DeleteFeedBackById(feedbackId);
                }
            }
            TagView();
            DataListBind();
        }
    }
}