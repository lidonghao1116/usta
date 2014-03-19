using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using System.Data;
using System.IO;
using USTA.Cache;

public partial class Administrator_BaseConfig : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBindBaseConfig();
        }
    }
    //绑定初始系统配置信息
    public void DataBindBaseConfig()
    {
        DalOperationBaseConfig dobc = new DalOperationBaseConfig();
        BaseConfig basecofig = dobc.FindBaseConfig();
        if (basecofig != null)
        {
            txtSystemName.Text = basecofig.systemName;
            txtSystemVersion.Text = basecofig.systemVersion;
            txtSystemCopyright.Text = basecofig.systemCopyRight;
            txtServerAddress.Text = basecofig.fileServerAddress;
        }
    }
    //修改系统配置信息
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtSystemName.Text.Trim().Length == 0 || txtSystemVersion.Text.Trim().Length == 0 || txtSystemCopyright.Text.Trim().Length == 0 || txtServerAddress.Text.Trim().Length == 0)
        {

            Javascript.GoHistory(-1, "系统各项配置信息不能为空，请输入！", Page);
        }
        else
        {

            DalOperationBaseConfig dobc = new DalOperationBaseConfig();
            BaseConfig baseconfig = new BaseConfig();
            baseconfig.systemName = txtSystemName.Text.Trim();
            baseconfig.systemVersion = txtSystemVersion.Text.Trim();
            baseconfig.systemCopyRight = txtSystemCopyright.Text.Trim();
            baseconfig.fileServerAddress = txtServerAddress.Text.Trim();

            try
            {
                dobc.UpdateBaseConfig(baseconfig);
                CacheCollections.ClearCache("baseConfig");
                Javascript.AlertAndRedirect("更新系统配置信息成功！", "/Administrator/BaseConfig.aspx", Page);

            }
            catch (Exception ex)
            {
               MongoDBLog.LogRecord(ex);
                Javascript.AlertAndRedirect("更新系统配置信息失败！", "/Administrator/BaseConfig.aspx", Page);
            }

        }

    }
}
