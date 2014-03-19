using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using USTA.Common;
using USTA.Model;
using USTA.Dal;
using System.Data;
using System.IO;
using USTA.Cache;
using System.Configuration;

using USTA.Bll;
using System.Collections;

public partial class Administrator_EmailManage : System.Web.UI.Page
{
    public int fileFolderType = (int)FileFolderType.emailAttachments;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, liFragment4, liFragment5, divFragment1, divFragment2, divFragment3, divFragment4,divFragment5);

            //绑定邮件列表信息
            if (fragmentFlag.Equals("1"))
            {
                string[] enums = Enum.GetNames(typeof(EmailType));
                foreach(string str in enums)
                {
                    string val = Enum.Format(typeof(EmailType),Enum.Parse(typeof(EmailType),str),"d");
                    ddlEmailType.Items.Add(new ListItem(CommonUtility.ReturnEmailTypeByVal(int.Parse(val)), val));
                }

                DataListBind();
            }
            if (fragmentFlag.Equals("2"))
            {
                upload1.InnerHtml = "<input id=\"Button3\" type=\"button\" value=\"添加一个附件\" onclick=\"addIframe(" + fileFolderType + ");\" />&nbsp;&nbsp;<b>上传文件大小不超过</b>" + ConfigurationManager.AppSettings["uploadFileLimit"] + "<div id=\"iframes\"></div>";
            }
            if (fragmentFlag.Equals("3"))
            {
                upload2.InnerHtml = "<input id=\"Button3\" type=\"button\" value=\"添加一个附件\" onclick=\"addIframe(" + fileFolderType + ");\" />&nbsp;&nbsp;<b>上传文件大小不超过</b>" + ConfigurationManager.AppSettings["uploadFileLimit"] + "<div id=\"iframes\"></div>";
                DataBindCourses();
                DataBindGroupUsers();
            }
            //绑定邮箱配置信息
            if (fragmentFlag.Equals("4"))
            {
                DataBindEmailConfig();
            }

            //绑定全部邮件模板类型信息
            if (fragmentFlag.Equals("5"))
            {
                Hashtable hashtableEmailTemplateType = CommonUtility.GetAllEmailTemplateTypeInfo();

                foreach(DictionaryEntry _emailTemplateType in hashtableEmailTemplateType)
                {
                    ddlEmailTemplateType.Items.Add(new ListItem(CommonUtility.ReturnEmailTypeByVal(int.Parse(_emailTemplateType.Key.ToString().Trim())), _emailTemplateType.Value.ToString().Trim()));
                }

                if (ddlEmailTemplateType.Items.Count > 0)
                {
                    DataBindEmailTemplate();
                }
            }
        }
    }


    protected void btnEmailTemplate_Click(object sender, EventArgs e)
    {
        CommonUtility.UpdateEmailTemplateInfo(ddlEmailTemplateType.SelectedValue, new EmailTemplate { title = txtEmailTemplateTitle.Text.Trim(), content = txtEmailTemplateContent.Text.Trim() });
        Javascript.RefreshParentWindow("修改邮件模板成功！", "/Administrator/EmailManage.aspx?fragment=5", Page);
    }

    protected void DataBindEmailTemplate()
    {
        EmailTemplate _emailTemplate = CommonUtility.GetDetailEmailConfigInfoByPath(ddlEmailTemplateType.SelectedValue);

        txtEmailTemplateTitle.Text = _emailTemplate.title;
        txtEmailTemplateContent.Text = _emailTemplate.content;

        if (_emailTemplate.isModify == 0)
        {
            txtEmailTemplateTitle.ReadOnly = true;
        }

        divEmailTemplate.Visible = true;
    }

    protected void ddlEmailTemplateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBindEmailTemplate();
    }

    //下拉列表事件
    protected void ddlEmailType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataListBind();
    }

    //绑定邮件列表信息到DataList
    public void DataListBind()
    {
        UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutEmail dou = new DalOperationAboutEmail();
        //DataView dv = dou.GetEmailSendingQueue(ConfigurationManager.AppSettings["briefSysName"]).Tables[0].DefaultView;
        DataView dv = dou.GetEmailSendingQueue(int.Parse(ddlEmailType.SelectedValue)).Tables[0].DefaultView;

        this.AspNetPager1.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;

        pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        pds.AllowPaging = true;

        pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        this.dlSendingEmailList.DataSource = pds;
        this.dlSendingEmailList.DataBind();

        if (pds.Count == 0)
        {
            this.dlSendingEmailList.ShowFooter = true;
            btnDeleteEmailList.Visible = false;
            ltlSelectAllEmail.Visible = false;
        }
        else
        {
            this.dlSendingEmailList.ShowFooter = false;
            btnDeleteEmailList.Visible = true;
            ltlSelectAllEmail.Visible = true;
        }


        if (pds.Count > 0)
        {
            ltlSelectAllEmail.Text = "<input id=\"dzxBtnSelectAll\" name=\"dzxBtnSelectAll\" type=\"button\" value=\"全选\" onclick=\"selectAll();\" /> ";
        }

    }
    //邮件发送列表分页
    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        DataListBind();
    }


    //--------------邮件发送-单用户发送---------开始-
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataListBind(ReturnUserType());
    }

    //邮件发送列表分页
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataListBind(ReturnUserType());
    }

    //绑定用户信息到DataList
    public void DataListBind(int type)
    {
        //if (txtKeyword.Text.Trim().Length > 0)
        //{
        DalOperationUsers dou = new DalOperationUsers();
        DataTable dt = dou.FindUserByTypeAndKeywod(type, txtKeyword.Text);
        if (dt.Rows.Count > 0)
        {
            maiContent.Visible = true;
        }
        else
        {
            maiContent.Visible = false;
        }

        DataView dv = dt.DefaultView;



        this.dlSearchUser.DataSource = dv;
        this.dlSearchUser.DataBind();

        int recordCount = dt.Rows.Count;

        if (recordCount == 0)
        {
            this.dlSearchUser.ShowFooter = true;
        }
        else
        {
            this.dlSearchUser.ShowFooter = false;
        }

        if (dt.Rows.Count > 0)
        {
            ltlSelectAllSearchUser.Text = "<input id=\"dzxBtnSelectAll\" name=\"dzxBtnSelectAll\" type=\"button\" value=\"全选\" onclick=\"selectAll();\" /> ";
        }

    }
    //取得用户类型
    public int ReturnUserType()
    {
        int userType = 0;
        switch (ddlUserType.SelectedItem.ToString())
        {
            case "教师":
                userType = 1;
                break;

            case "助教":
                userType = 2;
                break;
            case "学生":
                userType = 3;
                break;
            default:
                break;
        }
        return userType;
    }

    protected void btnDeleteEmailList_Click(object sender, EventArgs e)
    {
        DalOperationAboutEmail dalemail = new DalOperationAboutEmail();
        int count = 0;
        foreach (DataListItem item in this.dlSendingEmailList.Items)
        {
            CheckBox chkItem = (CheckBox)item.FindControl("ChkBox");
            if (chkItem.Checked)
            {
                count++;
                dalemail.DeleteSendEmail(int.Parse(((HiddenField)item.FindControl("mailId")).Value));

            }
        }
        Javascript.AlertAndRedirect("删除" + count + "封邮件！", "/Administrator/EmailManage.aspx", Page);
    }


    //向所选择的用户发送邮件
    protected void btnSendMails_Click(object sender, EventArgs e)
    {
        string userName, emailAddress;
        //事务处理发送邮件队列
        using (TransactionScope scope = new TransactionScope())
        {
            try
            {
                int count = 0;
                foreach (DataListItem item in this.dlSearchUser.Items)
                {
                    CheckBox chkItem = (CheckBox)item.FindControl("ChkBox");
                    TextBox txtName = (TextBox)item.FindControl("txtUserName");
                    TextBox txtEmail = (TextBox)item.FindControl("txtEmail");

                    if (chkItem.Checked)
                    {
                        count += 1;
                        emailAddress = txtEmail.Text;
                        userName = txtName.Text;
                        if (emailAddress.Length != 0 && CommonUtility.CheckStringIsEmail(emailAddress))
                            SendEmail(userName, emailAddress, 1);
                        else
                            throw new Exception("发现有不合法的邮件地址,添加到发送队列失败!");
                    }
                }
                scope.Complete();
                Javascript.AlertAndRedirect("添加邮件发送队列成功,共添加" + count.ToString() + "封待发邮件！", "/Administrator/EmailManage.aspx", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "邮件发送失败,错误提示：邮件地址不合法,请确认所有邮件地址合法有效！", Page);
            }

        }
        DataListBind(ReturnUserType());
    }
    //--------------邮件发送-少量用户发送---------结束-

    //--------------邮件发送-向用户组发送---------开始-

    //绑定课程下拉列表
    public void DataBindCourses()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataTable dt = doac.FindCurrentCourses().Tables[0];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ListItem li = new ListItem(dt.Rows[i]["courseName"].ToString().Trim() + "(" + dt.Rows[i]["classID"].ToString().Trim() + ")", dt.Rows[i]["termTag"].ToString().Trim() + dt.Rows[i]["courseNo"].ToString().Trim() + dt.Rows[i]["classID"].ToString().Trim());
            ddlCourses.Items.Add(li);
        }
    }
    //向用户组发送邮件
    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        if (txtEmailTitle.Text.Trim().Length == 0)
        {
            Javascript.GoHistory(-1, "标题不能为空，请输入！", Page);
        }
        else
        {
            string userName, emailAddress;
            //事务处理发送邮件队列
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    int count = 0;
                    foreach (DataListItem item in this.dlSearchUserGroup.Items)
                    {
                        CheckBox chkItem = (CheckBox)item.FindControl("ChkBox");
                        TextBox txtName = (TextBox)item.FindControl("txtUserName");
                        TextBox txtEmail = (TextBox)item.FindControl("txtEmail");

                        if (chkItem.Checked)
                        {
                            count += 1;
                            emailAddress = txtEmail.Text;
                            userName = txtName.Text;
                            if (emailAddress.Length != 0 && CommonUtility.CheckStringIsEmail(emailAddress))
                                SendEmail(userName, emailAddress, 2);
                            else
                                throw new Exception("发现有不合法的邮件地址,添加到发送队列失败!");
                        }
                    }
                    Response.Write(count);
                    scope.Complete();
                    Javascript.AlertAndRedirect("添加邮件发送队列成功,共添加" + count.ToString() + "封待发邮件！", "/Administrator/EmailManage.aspx", Page);
                }
                catch (Exception ex)
                {
                    MongoDBLog.LogRecord(ex);
                    Javascript.GoHistory(-1, "邮件发送失败,错误提示：邮件地址不合法,请确认所有邮件地址合法有效！", Page);
                }

            }
        }
    }
    //取得用户组类型
    public int ReturnUserGroup()
    {
        int userType = 0;
        switch (ddlUserGroup.SelectedItem.ToString())
        {
            case "教师":
                userType = 1;
                break;

            case "助教":
                userType = 2;
                break;
            case "学生":
                userType = 3;
                break;
            default:
                break;
        }
        return userType;
    }
    //向单个用户发送邮件
    public void SendEmail(string userName, string emailAddress, int type)
    {
        UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutEmail dou = new DalOperationAboutEmail();
        SendingEmailList sendingemail = new SendingEmailList();
        if (type == 1)//少量发送邮件
        {
            sendingemail.emailTitle = txtTitle.Text.Trim();
            sendingemail.emailContent = Textarea1.Value.Trim();

        }
        else //向用户组发送邮件
        {
            sendingemail.emailTitle = txtEmailTitle.Text.Trim();
            sendingemail.emailContent = Textarea2.Value.Trim();
        }
        sendingemail.emailAttachmentIds = hidAttachmentId.Value;
        sendingemail.userName = userName;
        sendingemail.emailAddress = emailAddress;
        sendingemail.sender = ConfigurationManager.AppSettings["briefSysName"];
        SendingEmailList[] sendingEmailList = { sendingemail };
        dou.AddEmailToSendingQueue(sendingEmailList);//插入邮件列表
    }
    //--------------邮件发送-向用户组发送---------结束-


    //绑定邮件配置信息
    public void DataBindEmailConfig()
    {
        UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutEmail doae = new DalOperationAboutEmail();
        EmailConfig emailconfig = doae.GetEmailConfig();
        if (emailconfig != null)
        {
            txtEmailAddress.Text = emailconfig.emailAddress;
            txtpasswd.Text = emailconfig.emailPassword;
            txtMailServer.Text = emailconfig.emailServerAddress;
            txtMailServerPort.Text = emailconfig.emailServerPort.ToString().Trim();
        }
    }
    //修改邮件配置信息
    protected void btnMailCommit_Click(object sender, EventArgs e)
    {
        if (txtEmailAddress.Text.Trim().Length == 0 || txtpasswd.Text.Trim().Length == 0 || txtMailServer.Text.Trim().Length == 0 || txtMailServerPort.Text.Trim().Length == 0)
        {

            Javascript.GoHistory(-1, "邮件配置信息不能为空，请输入！", Page);
        }
        else
        {

            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

            DalOperationAboutEmail doae = new DalOperationAboutEmail();
            EmailConfig emailconfig = new EmailConfig();
            emailconfig.emailAddress = txtEmailAddress.Text.Trim();
            emailconfig.emailPassword = txtpasswd.Text.Trim();
            emailconfig.emailServerAddress = txtMailServer.Text.Trim();
            emailconfig.emailServerPort = int.Parse(txtMailServerPort.Text.Trim());
            emailconfig.sender = user.userName;
            try
            {
                doae.UpdateEmailConfig(emailconfig);
                Javascript.AlertAndRedirect("更新邮件配置信息成功！", "/Administrator/EmailManage.aspx?fragment=4", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.AlertAndRedirect("更新邮件配置信息失败！", "/Administrator/EmailManage.aspx?fragment=4", Page);
            }

        }
    }

    protected void ddlUserGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUserGroup.SelectedValue == "3")
        {
            ddlCourses.Visible = true;
        }
        else
        {
            ddlCourses.Visible = false;
            ddlCourses.SelectedIndex = 0;
        }
        DataBindGroupUsers();
    }
    protected void ddlCourses_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBindGroupUsers();
    }

    //绑定用户组下拉列表
    public void DataBindGroupUsers()
    {
        DalOperationUsers dou = new DalOperationUsers();
        DataTable dt = null;
        if (ddlUserGroup.SelectedValue == "3")
        {
            dt = dou.SearchStudentByCourseNo(ddlCourses.SelectedValue.Trim()).Tables[0];
            dt.Columns["studentNo"].ColumnName = "userNo";
            dt.Columns["studentName"].ColumnName = "userName";
        }
        else
        {
            dt = dou.FindUserByTypeAndKeywod(ReturnUserGroup(), "");
        }

        if (dt.Rows.Count > 0)
        {
            EmailCounts.Visible = true;
        }
        else
        {
            EmailCounts.Visible = false;
        }

        this.dlSearchUserGroup.DataSource = dt.DefaultView;
        this.dlSearchUserGroup.DataBind();

        int recordCount = dt.Rows.Count;

        if (recordCount == 0)
        {
            this.dlSearchUserGroup.ShowFooter = true;
        }
        else
        {
            this.dlSearchUserGroup.ShowFooter = false;
        }

        if (dt.Rows.Count > 0)
        {
            ltlSelectAllUser.Text = "<input id=\"dzxBtnSelectAll\" name=\"dzxBtnSelectAll\" type=\"button\" value=\"全选\" onclick=\"selectAll();\" /> ";
        }

    }
}
