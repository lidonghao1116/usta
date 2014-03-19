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
    public partial class DrawManage : System.Web.UI.Page
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
        //控制Tab的显示
        string fragmentFlag = "1";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, liFragment4, liFragment5, divFragment1, divFragment2, divFragment3, divFragment4, divFragment5);

            if (fragmentFlag.Equals("1"))
            {
                if (!IsPostBack)
                {
                    DataListBindGameCategory();
                }
            }

            if (fragmentFlag.Equals("2"))
            {
                if (!IsPostBack)
                {
                    DalOperationAboutGameCategory dal = new DalOperationAboutGameCategory();
                    DataTable dt = dal.GetList().Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddlGameCategoryAboutGameType.Items.Add(new ListItem(dt.Rows[i]["gameTitle"].ToString().Trim(), dt.Rows[i]["gameCategoryId"].ToString().Trim()));
                    }

                    DataListBindGameType();
                }
            }

            if (fragmentFlag.Equals("3"))
            {
                if (!IsPostBack)
                {
                    DataBindGameCategoryAndType(ddlGameCategory, ddlGameType);
                    DataListBindEnrollList();
                }
            }

            if (fragmentFlag.Equals("4"))
            {
                if (!IsPostBack)
                {
                    DataBindGameCategoryAndType(ddlDrawGameCategory, ddlDrawGameType);
                    DataListBindGroupNum();
                }
            }
        }

        protected void dlGameCategory_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnOpenDraw = (LinkButton)e.Item.FindControl("lbtnOpenDraw");
                LinkButton lbtnCloseDraw = (LinkButton)e.Item.FindControl("lbtnCloseDraw");
                Literal ltlOpenDraw = (Literal)e.Item.FindControl("ltlOpenDraw");
                Literal ltlCloseDraw = (Literal)e.Item.FindControl("ltlCloseDraw");

                DataRowView rowv = (DataRowView)e.Item.DataItem;
                string isOpenDraw = rowv["isOpenDraw"].ToString().Trim();

                if (string.IsNullOrEmpty(isOpenDraw))
                {
                    lbtnOpenDraw.Visible = true;
                    ltlCloseDraw.Visible = true;
                }
                else if (isOpenDraw == "1")
                {
                    lbtnCloseDraw.Visible = true;
                    ltlOpenDraw.Visible = true;
                }
                else if (isOpenDraw == "0")
                {
                    lbtnOpenDraw.Visible = true;
                    ltlCloseDraw.Visible = true;
                }
            }
        }


        //依据组别绑定组号信息
        protected void dlstGroupNum_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("dlstGroupIndex");
                DataRowView rowv = (DataRowView)e.Item.DataItem;
                string groupNum = rowv["groupNum"].ToString().Trim();
                int gameCategoryId = int.Parse(rowv["gameCategoryId"].ToString().Trim());
                int gameTypeId = int.Parse(rowv["gameTypeId"].ToString().Trim());
                DalOperationAboutGameDrawList dal = new DalOperationAboutGameDrawList();
                DataSet ds = dal.GetGroupIndexList(groupNum, gameCategoryId, gameTypeId);
                dataList.DataSource = ds.Tables[0].DefaultView;
                dataList.DataBind();
            }
        }

        
        //第1个标签；开始
        protected void ddlDrawGameCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (ddlDrawGameType.Items.Count > 0)
            {
                ddlDrawGameType.Items.RemoveAt(0);
            }

            DalOperationAboutGameType dal = new DalOperationAboutGameType();

            DataTable dt = dal.GetGameTypeByGameCategoryId(int.Parse(ddlDrawGameCategory.SelectedValue)).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlDrawGameType.Items.Add(new ListItem(dt.Rows[i]["gameTypeTitle"].ToString().Trim(), dt.Rows[i]["gameTypeId"].ToString().Trim()));
            }

            DataListBindGroupNum();
        }

        
        //第1个标签；开始
        protected void ddlDrawGameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataListBindGroupNum();
        }

        //绑定组别
        protected void DataListBindGroupNum()
        {
            //检查是否有活动届次和活动类型数据

            if (!(ddlDrawGameCategory.Items.Count > 0 && ddlDrawGameType.Items.Count > 0))
            {
                Javascript.GoHistory(-1, "当前暂无活动届次和活动类型信息：（", Page);
                return;
            }

            DalOperationAboutGameDrawList dal = new DalOperationAboutGameDrawList();
            DataTable dt = dal.GetGroupNumList(int.Parse(ddlDrawGameCategory.SelectedValue), int.Parse(ddlDrawGameType.SelectedValue)).Tables[0];

            this.dlstGroupNum.DataSource = dt;
            this.dlstGroupNum.DataBind();
        }

        //绑定信息
        public void DataListBindGameCategory()
        {
            DalOperationAboutGameCategory doan = new DalOperationAboutGameCategory();
            DataTable dt = doan.GetList().Tables[0];

            this.AspNetPager1.RecordCount = dt.Rows.Count;
            AspNetPager1.PageSize = CommonUtility.pageSize;

            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dt.DefaultView;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = pageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;

            this.dlGameCategory.DataSource = pds;
            this.dlGameCategory.DataBind();

            if (pds.Count == 0)
            {
                this.dlGameCategory.ShowFooter = true;
            }
            else
            {
                this.dlGameCategory.ShowFooter = false;
            }
        }


        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataListBindGameCategory();
        }

        protected void dlGameCategory_ItemCommand(object source, DataListCommandEventArgs e)
        {
            DalOperationAboutGameCategory doan = new DalOperationAboutGameCategory();

            if (e.CommandName == "delete")
            {
                string gameCategoryId = dlGameCategory.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号  
                doan.Delete(int.Parse(gameCategoryId));
                Javascript.AlertAndRedirect("删除成功！", "/Administrator/DrawManage.aspx?fragment=1&page=" + pageIndex, Page);
            }
            else if (e.CommandName == "openDraw")
            {
                string gameCategoryId = dlGameCategory.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号  

                //首先判断是否已经过截止日期，如果已经未过截止日期则禁止修改抽签状态
                DataTable dt = doan.CheckGameCategoryIsOverTimeByGameCategoryId(int.Parse(gameCategoryId),DateTime.Now).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Javascript.GoHistory(-1, "当前报名未过截止日期，禁止修改抽签状态：（", Page);
                    return;
                }

                doan.UpdateDrawState(new GameCategory { gameCategoryId = int.Parse(gameCategoryId), isOpenDraw = 1 });
                Javascript.AlertAndRedirect("开放抽签成功！", "/Administrator/DrawManage.aspx?fragment=1&page=" + pageIndex, Page);
            }
            else if (e.CommandName == "closeDraw")
            {
                string gameCategoryId = dlGameCategory.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号  
                //首先判断是否已经过截止日期，如果已经未过截止日期则禁止修改抽签状态
                DataTable dt = doan.CheckGameCategoryIsOverTimeByGameCategoryId(int.Parse(gameCategoryId), DateTime.Now).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Javascript.GoHistory(-1, "当前报名未过截止日期，禁止修改抽签状态：（", Page);
                    return;
                }
                doan.UpdateDrawState(new GameCategory { gameCategoryId = int.Parse(gameCategoryId), isOpenDraw = 0 });
                Javascript.AlertAndRedirect("关闭抽签成功！", "/Administrator/DrawManage.aspx?fragment=1&page=" + pageIndex, Page);
            }
        }


        //绑定信息
        public void DataListBindGameType()
        {
            DalOperationAboutGameType doan = new DalOperationAboutGameType();
            DataTable dt = doan.GetGameTypeByGameCategoryId(int.Parse(ddlGameCategoryAboutGameType.SelectedValue)).Tables[0];

            this.dlGameType.DataSource = dt;
            this.dlGameType.DataBind();

            if (dt.Rows.Count == 0)
            {
                this.dlGameType.ShowFooter = true;
            }
            else
            {
                this.dlGameType.ShowFooter = false;
            }
        }

        public void DataBindGameCategoryAndType(DropDownList ddlGameCategory, DropDownList ddlGameType)
        {
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

            DalOperationAboutGameCategory dalGameCategory = new DalOperationAboutGameCategory();

            DataTable dtGameCategory =  dalGameCategory.GetList().Tables[0];

            for (int i = 0; i < dtGameCategory.Rows.Count; i++)
            {
                ddlGameCategory.Items.Add(new ListItem(dtGameCategory.Rows[i]["gameTitle"].ToString().Trim(), dtGameCategory.Rows[i]["gameCategoryId"].ToString().Trim()));
            }

            DalOperationAboutGameType dalGameType = new DalOperationAboutGameType();

            DataTable dtGameType = dalGameType.GetGameTypeByGameCategoryId(int.Parse(ddlGameCategory.SelectedValue)).Tables[0];

            for (int i = 0; i < dtGameType.Rows.Count; i++)
            {
                ddlGameType.Items.Add(new ListItem(dtGameType.Rows[i]["gameTypeTitle"].ToString().Trim(), dtGameType.Rows[i]["gameTypeId"].ToString().Trim()));
            }
        }

        //绑定信息
        public void DataListBindEnrollList()
        {
            //检查是否有活动届次和活动类型数据

            if (!(ddlGameCategory.Items.Count > 0 && ddlGameType.Items.Count > 0))
            {
                Javascript.GoHistory(-1, "当前暂无活动届次和活动类型信息：（", Page);
                return;
            }

            DalOperationAboutGameDrawList doan = new DalOperationAboutGameDrawList();
            DataTable dt = doan.GetList(int.Parse(ddlGameCategory.SelectedValue), int.Parse(ddlGameType.SelectedValue)).Tables[0];

            this.AspNetPager3.RecordCount = dt.Rows.Count;
            AspNetPager3.PageSize = CommonUtility.pageSize;

            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dt.DefaultView;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = AspNetPager3.CurrentPageIndex - 1;
            pds.PageSize = AspNetPager3.PageSize;

            this.dlEnroll.DataSource = pds;
            this.dlEnroll.DataBind();

            if (pds.Count == 0)
            {
                this.dlEnroll.ShowFooter = true;
            }
            else
            {
                this.dlEnroll.ShowFooter = false;
            }
        }

        protected void dlEnroll_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //DalOperationAboutGameCategory doan = new DalOperationAboutGameCategory();
            //if (e.CommandName == "delete")
            //{
            //    string gameCategoryId = dlGameCategory.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号  
            //    doan.Delete(int.Parse(gameCategoryId));
            //    Javascript.AlertAndRedirect("删除成功！", "/Administrator/DrawManage.aspx?fragment=1&page=" + pageIndex, Page);
            //}
        }

        protected void AspNetPager3_PageChanged(object sender, EventArgs e)
        {
            if (fragmentFlag == "3")
            {
                DataListBindEnrollList();
            }
        }

        //第1个标签；开始
        protected void ddlGameCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (ddlGameType.Items.Count > 0)
            {
                ddlGameType.Items.RemoveAt(0);
            }

            DalOperationAboutGameType dal = new DalOperationAboutGameType();

            DataTable dt = dal.GetGameTypeByGameCategoryId(int.Parse(ddlGameCategory.SelectedValue)).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlGameType.Items.Add(new ListItem(dt.Rows[i]["gameTypeTitle"].ToString().Trim(), dt.Rows[i]["gameTypeId"].ToString().Trim()));
            }

            DataListBindEnrollList();
        }


        //第1个标签；开始
        protected void ddlGameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataListBindEnrollList();
        }

        protected void ddlGameCategoryAboutGameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataListBindGameType();
        }

        protected void dlGameType_ItemCommand(object source, DataListCommandEventArgs e)
        {
            DalOperationAboutGameType doan = new DalOperationAboutGameType();
            if (e.CommandName == "delete")
            {
                string gameTypeId = dlGameType.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号  
                doan.Delete(int.Parse(gameTypeId));
                Javascript.AlertAndRedirect("删除成功！", "/Administrator/DrawManage.aspx?fragment=2&page=" + pageIndex, Page);
            }
        }
    }
}