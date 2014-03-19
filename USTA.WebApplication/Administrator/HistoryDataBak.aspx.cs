using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Common;
using System.IO;
using System.Configuration;
using USTA.Dal;

public partial class Administrator_HistoryDataBak : System.Web.UI.Page
{
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

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
                , divFragment1, divFragment2, divFragment3);

            if (fragmentFlag.Equals("1"))
            {

                string fileFolder = Server.MapPath(ConfigurationManager.AppSettings["DataBaseBakPath"]);

                if (!Directory.Exists(fileFolder))
                {
                    Directory.CreateDirectory(fileFolder);
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(fileFolder);
                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.bak"))
                {
                    RadioButtonList1.Items.Add(new ListItem(fileInfo.Name, fileInfo.Name));
                }
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DalOperationAboutDataBaseBak dalOperationAboutDataBaseBak = new DalOperationAboutDataBaseBak();
        dalOperationAboutDataBaseBak.DataBaseBak();

        Javascript.AlertAndRedirect("数据库备份成功！", "HistoryDataBak.aspx?fragment=1", Page);
    }
}
