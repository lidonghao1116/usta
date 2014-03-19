using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;

using USTA.Bll;
using USTA.Dal;
using USTA.Common;
using System.IO;
using USTA.Model;

public partial class Administrator_ImportExcelData : System.Web.UI.Page
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
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            FileUpload1.SaveAs(Server.MapPath("/" + FileUpload1.FileName));
            DalOperationAboutUser DalOperationAboutUser = new DalOperationAboutUser();

            //按顺序设定每个工作薄的字段数目

            int[] sheetFiledsCount = { 5, 5, 7, 1, 6, 2, 2, 2 };

            //读取要导入的Excel文件的全部数据
            ExcelData excelData = BllOperationAboutExcel.BllImportExcelData(Server.MapPath("/" + FileUpload1.FileName), sheetFiledsCount,FileUpload1);
            //Response.Write(excelData.excelPasswordMapping.Count);
            //Response.End();
            //将获取的导入数据存入数据库
            DalOperationAboutUser.ImportExcelData(excelData);
        }
        else
        {
            Javascript.GoHistory(-1, "请先选择Excel文件！", Page);
        }
    }

}