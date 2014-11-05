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
using System.Text;

public partial class Administrator_NotifyInfoManage : System.Web.UI.Page
{
    public int fileFolderType = (int)FileFolderType.adminNotify;
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    public string strSelect = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //控制Tab的显示
        string fragmentFlag = "1";

        if (Request["fragment"] != null)
        {
            fragmentFlag = Request["fragment"];
        }

        CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
            , divFragment1, divFragment2, divFragment3);

        if (fragmentFlag.Equals("1"))
        {
            if (!IsPostBack)
            {
                DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();

                DataTable dt = dalNotifyType.FindAllParentAdminNotifyType().Tables[0];
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem _item = new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim());
                    ddlNotifyTypeManage.Items.Add(_item);
                }

                if (ddlNotifyTypeManage.Items.Count > 0)
                {
                    DataTable _dt = dalNotifyType.FindAllAdminNotifyTypeByParentId(int.Parse(ddlNotifyTypeManage.SelectedValue)).Tables[0];
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        ddlNotifyTypeManageChild.Items.Add(new ListItem(_dt.Rows[j]["notifyTypeName"].ToString().Trim(), _dt.Rows[j]["notifyTypeId"].ToString().Trim()));
                    }
                }

                
                if (ddlNotifyTypeManageChild.Items.Count > 0)
                {
                    DataListBindAdminNotify(int.Parse(ddlNotifyTypeManageChild.SelectedValue.ToString().Trim()));//默认全部绑定
                }
            }
        }
        if (fragmentFlag.Equals("2"))
        {
            if (!IsPostBack)
            {
                //Javascript.ExcuteJavascriptCode("initBeforeUnloadEvent('温馨提示：当前页面数据可能未保存哟~（此为提示，并不代表您真正未保存数据），确定离开吗？');", Page);
                DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
                DataTable dt = dalNotifyType.FindAllParentAdminNotifyType().Tables[0];

                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem _item = new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim());
                    ddlNotifyType.Items.Add(_item);                    
                }

                if (ddlNotifyType.Items.Count > 0)
                {
                    DataTable _dt = dalNotifyType.FindAllAdminNotifyTypeByParentId(int.Parse(ddlNotifyType.SelectedValue)).Tables[0];
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        ddlNotifyTypeChild.Items.Add(new ListItem(_dt.Rows[j]["notifyTypeName"].ToString().Trim(), _dt.Rows[j]["notifyTypeId"].ToString().Trim()));
                    }
                }
                txtTitle.Attributes.Add("class", "required");
            }
        }
        
        if (fragmentFlag.Equals("3"))
        {
            if (!IsPostBack)
            {
                DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
                DataTable dt = dalNotifyType.FindAllParentAdminNotifyType().Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem _item = new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim());
                    _item.Attributes.Add("parentId", "0");
                    _item.Attributes.Add("disabled", "true");
                    ddlNotifyType.Items.Add(_item);
                }

                if (Request["del"] != null && Request["del"] == "true" && Request["notifyTypeId"] != null)
                {
                    dalNotifyType.DeleteAdminNotifyTypeById(int.Parse(Request["notifyTypeId"].ToString().Trim()));                   
                }

                NotifyTypeDataBind();
            }
        }      
    }

    //依据文章类型，绑定第类的文章列表
    protected void dlstNotifyTypeParent_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList dataList = (DataList)e.Item.FindControl("dlstNotifyType");
            DataRowView rowv = (DataRowView)e.Item.DataItem;
            int mainID = Convert.ToInt32(rowv["notifyTypeId"]);
            DalOperationAboutAdminNotifyType dalOperationAboutAdminNotifyType = new DalOperationAboutAdminNotifyType();
            DataSet ds = dalOperationAboutAdminNotifyType.FindAllAdminNotifyTypeByParentId(mainID);
            dataList.DataSource = ds.Tables[0].DefaultView;
            dataList.DataBind();
        }
    }

    //第1个标签；开始
    protected void ddlNotifyTypeManage_SelectedIndexChanged(object sender, EventArgs e)
    {
        Javascript.ExcuteJavascriptCode("deleteBeforeUnloadEvent();", Page);

        while (ddlNotifyTypeManageChild.Items.Count > 0)
        {
            ddlNotifyTypeManageChild.Items.RemoveAt(0);
        }

        DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
        DataTable _dt = dalNotifyType.FindAllAdminNotifyTypeByParentId(int.Parse(ddlNotifyTypeManage.SelectedValue)).Tables[0];
        for (int j = 0; j < _dt.Rows.Count; j++)
        {
            ddlNotifyTypeManageChild.Items.Add(new ListItem(_dt.Rows[j]["notifyTypeName"].ToString().Trim(), _dt.Rows[j]["notifyTypeId"].ToString().Trim()));
        }

        DataListBindAdminNotify(int.Parse(ddlNotifyTypeManageChild.SelectedValue.ToString().Trim()));//默认全部绑定
    }

    //第1个标签；开始
    protected void ddlNotifyTypeManageChild_SelectedIndexChanged(object sender, EventArgs e)
    {
       DataListBindAdminNotify(int.Parse(ddlNotifyTypeManageChild.SelectedValue.ToString().Trim()));      
    }



    //第1个标签；开始
    protected void ddlNotifyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        while (ddlNotifyTypeChild.Items.Count > 0)
        {
            ddlNotifyTypeChild.Items.RemoveAt(0);
        }

        DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
        DataTable _dt = dalNotifyType.FindAllAdminNotifyTypeByParentId(int.Parse(ddlNotifyType.SelectedValue)).Tables[0];
        for (int j = 0; j < _dt.Rows.Count; j++)
        {
            ddlNotifyTypeChild.Items.Add(new ListItem(_dt.Rows[j]["notifyTypeName"].ToString().Trim(), _dt.Rows[j]["notifyTypeId"].ToString().Trim()));
        }
    }

    //绑定文章信息
    public void DataListBindAdminNotify(int type)
    {
        DalOperationAboutAdminNotify doan = new DalOperationAboutAdminNotify();
        DataView dv = null;
        if(type==0)
          dv = doan.GetAllNotifys().Tables[0].DefaultView;
        else
         dv=doan.FindNotifyByTypeId(type).Tables[0].DefaultView;

        this.AspNetPager1.RecordCount = dv.Count;
        AspNetPager1.PageSize = CommonUtility.pageSize;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = AspNetPager1.PageSize;

        this.dlNotify.DataSource = pds;
        this.dlNotify.DataBind();

        if (pds.Count > 0)
        {
            this.dlNotify.ShowFooter = false;
        }
    }
    //第1个标签：结束
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddlNotifyTypeChild.Items.Count == 0)
        {
            Javascript.GoHistory(-1, "请选择“" + ddlNotifyType.SelectedItem.Text + "”下的二级分类\n若无二级分类，请在文章类别管理页面添加相应地的二级分类", Page);
            return;
        }

        if (txtTitle.Text.Trim().Length == 0 || Textarea1.Value.Trim().Length == 0)
        {
            Javascript.GoHistory(-1, "标题和内容不能为空，请输入！", Page);
        }
        else
        {
            DalOperationAboutAdminNotify doan = new DalOperationAboutAdminNotify();
            AdminNotifyInfo notify = new AdminNotifyInfo();
            notify.notifyTitle = txtTitle.Text.Trim();
            notify.notifyContent = Textarea1.Value.Trim();
            notify.notifyTypeId = int.Parse(ddlNotifyTypeChild.SelectedValue);

            //以下提交附件的判断与相关操作
            if (hidAttachmentId.Value.CompareTo(string.Empty) != 0)
            {
                notify.attachmentIds = hidAttachmentId.Value;//保存了附件并且返回了attachmentId(自增长类型主键)
            }

            try
            {
                doan.AddNotifyInfo(notify);//保存通知
                //Javascript.ExcuteJavascriptCode("delBeforeUnloadEvent();", Page);
                Javascript.AlertAndRedirect("添加成功！", "/Common/NotifyList.aspx", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                //Javascript.ExcuteJavascriptCode("delBeforeUnloadEvent();", Page);
                Javascript.GoHistory(-1, "添加失败,请检查格式是否有误！", Page);
            }
        }
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        DataListBindAdminNotify(int.Parse(ddlNotifyTypeManage.SelectedValue.ToString().Trim()));
    }

    protected void dlNotify_ItemCommand(object source, DataListCommandEventArgs e)
    {
        DalOperationAboutAdminNotify doan = new DalOperationAboutAdminNotify();
        if (e.CommandName == "delete")
        {
            string adminNotifyInfoId = dlNotify.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号  
            doan.DeleteNotifyById(int.Parse(adminNotifyInfoId));
            Javascript.AlertAndRedirect("删除成功！", "/Administrator/NotifyInfoManage.aspx?page="+pageIndex, Page);
        }
        else if (e.CommandName == "update")
        {
            string adminNotifyInfoId = dlNotify.DataKeys[e.Item.ItemIndex].ToString();//取选中行公告编号 
            Javascript.JavaScriptLocationHref("/Administrator/EditNotifyInfo.aspx?adminNotifyInfoId=" + adminNotifyInfoId, Page);       
        }
        else if(e.CommandName == "editIstop")
        {
            string adminNotifyInfoId = dlNotify.DataKeys[e.Item.ItemIndex].ToString();
            AdminNotifyInfo adminNotify = doan.FindNotifyByNo(int.Parse(adminNotifyInfoId));
            if (adminNotify.isTop > 0)
                doan.Canceltop(int.Parse(adminNotifyInfoId));
            else
                doan.Addtop(int.Parse(adminNotifyInfoId));
            Javascript.JavaScriptLocationHref("/Administrator/NotifyInfoManage.aspx", Page);
        }

    }

    protected void NotifyTypeDataBind()
    {
        DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
        DataView dv = dalNotifyType.FindAllParentAdminNotifyType().Tables[0].DefaultView;

        this.dlstNotifyTypeParent.DataSource = dv;
        this.dlstNotifyTypeParent.DataBind();
    }

    //****第3个标签：管理文章类型－－－－－－－－结束－－－－－－－－－－－

}