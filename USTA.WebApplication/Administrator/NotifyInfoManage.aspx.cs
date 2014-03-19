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

public partial class Administrator_NotifyInfoManage : System.Web.UI.Page
{
    public int fileFolderType = (int)FileFolderType.adminNotify;
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]); 
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
                    ddlNotifyTypeManage.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
                }

                dt = dalNotifyType.FindAdminNotifyInfoByPid(int.Parse(ddlNotifyTypeManage.SelectedValue)).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlNotifyTypeManageChild.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
                }


                DataListBindAdminNotify(int.Parse(ddlNotifyTypeManageChild.SelectedValue.ToString().Trim()));//默认全部绑定
              
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
                    ddlNotifyType.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
                }

                dt = dalNotifyType.FindAdminNotifyInfoByPid(int.Parse(ddlNotifyType.SelectedValue)).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlNotifyTypeChild.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
                }

               txtTitle.Attributes.Add("class", "required");
            }

        }
        if (fragmentFlag.Equals("3"))
        {
            if (!IsPostBack)
            {
                DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();

                if (Request["del"] != null && Request["del"] == "true" && Request["notifyTypeId"] != null)
                {
                    dalNotifyType.DeleteAdminNotifyTypeById(int.Parse(Request["notifyTypeId"].ToString().Trim()));
                }

                if (ddlNotifyTypeLevel.SelectedValue == "0")
                {
                    ddlNotifyTypeModify.Visible = false;
                }
                else
                {
                    ddlNotifyTypeModify.Visible = true;

                    DataTable dt = dalNotifyType.FindAllParentAdminNotifyType().Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddlNotifyTypeModify.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
                    }
                }

                NotifyTypeDataBind();
            }
        }      
    }

    //第1个标签；开始
    protected void ddlNotifyTypeManage_SelectedIndexChanged(object sender, EventArgs e)
    {
        DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
        DataTable dt = dalNotifyType.FindAdminNotifyInfoByPid(int.Parse(ddlNotifyTypeManage.SelectedValue)).Tables[0];

        while (ddlNotifyTypeManageChild.Items.Count > 0)
        {
            ddlNotifyTypeManageChild.Items.RemoveAt(0);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ddlNotifyTypeManageChild.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
        }


        DataListBindAdminNotify(int.Parse(ddlNotifyTypeManageChild.SelectedValue.ToString().Trim()));//默认全部绑定
              
    }

    //第1个标签；开始
    protected void ddlNotifyTypeManageChild_SelectedIndexChanged(object sender, EventArgs e)
    {
         DataListBindAdminNotify(int.Parse(ddlNotifyTypeManageChild.SelectedValue.ToString().Trim()));
    }

    //第1个标签；开始
    protected void ddlNotifyTypeLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();

        if (ddlNotifyTypeLevel.SelectedValue == "0")
        {
            ddlNotifyTypeModify.Visible = false;
        }
        else
        {
            ddlNotifyTypeModify.Visible = true;

            DataTable dt = dalNotifyType.FindAllParentAdminNotifyType().Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlNotifyTypeModify.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
            }
        }

        NotifyTypeDataBind();
    }

    //第2个标签；开始
    protected void ddlNotifyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DalOperationAboutAdminNotifyType dalNotifyType = new DalOperationAboutAdminNotifyType();
        DataTable dt = dalNotifyType.FindAdminNotifyInfoByPid(int.Parse(ddlNotifyType.SelectedValue)).Tables[0];

        while (ddlNotifyTypeChild.Items.Count > 0)
        {
            ddlNotifyTypeChild.Items.RemoveAt(0);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ddlNotifyTypeChild.Items.Add(new ListItem(dt.Rows[i]["notifyTypeName"].ToString().Trim(), dt.Rows[i]["notifyTypeId"].ToString().Trim()));
        }
    }

    protected void ddlNotifyTypeModify_SelectedIndexChanged(object sender, EventArgs e)
    {
        NotifyTypeDataBind();
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
        DataView dv;

        if (ddlNotifyTypeLevel.SelectedValue == "0")
        {
             dv = dalNotifyType.FindAllParentAdminNotifyType().Tables[0].DefaultView;
        }
        else
        {
             dv = dalNotifyType.FindAdminNotifyInfoByPid(int.Parse(ddlNotifyTypeModify.SelectedValue.ToString().Trim())).Tables[0].DefaultView;
        }

        this.dlstNotifyType.DataSource = dv;
        this.dlstNotifyType.DataBind();
    }

    //****第3个标签：管理文章类型－－－－－－－－结束－－－－－－－－－－－

}