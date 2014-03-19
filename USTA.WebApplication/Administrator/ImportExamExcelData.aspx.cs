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

public partial class Administrator_ImportExamExcelData : System.Web.UI.Page
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
                DataListBind();
            }
        }
    }

    //绑定考试安排信息到DataList
    public void DataListBind()
    {
        DalOperationUsers dos = new DalOperationUsers();
        DataView dv = dos.FindExamArrange().Tables[0].DefaultView;

        this.dlCourse.DataSource = dv;
        this.dlCourse.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.fileUploadUrl.FileName.Trim()=="")
        {
            Javascript.GoHistory(-1, "请选择本地Excel文件！", Page);
        }
        else
        {
            fileUploadUrl.SaveAs(Server.MapPath("/" + fileUploadUrl.FileName));//暂时保存要导入的Excel
            DalOperationUsers dos = new DalOperationUsers();

            //从绝对路径Excel文件中读取数据，保存在DataSet
            DataSet ds = dos.ExcelToDS(Server.MapPath("/" + fileUploadUrl.FileName), "考试安排");
            int count = 0;
            try
            {
                count = dos.DataTabletoDBTables(ds.Tables["sheetTable"], "考试安排");
            }
            catch (Exception ex)
            {
               MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "导入失败,错误提示：1．有空项　2．非Excel文件 3.重复插入．请按指定格式上传！", Page);
            }

            File.Delete(Server.MapPath("/" + fileUploadUrl.FileName));//导入完成后删除Excel
            Javascript.AlertAndRedirect("考试安排导入成功,共插入记录" + count.ToString() + "条！", "/Administrator/ImportExamExcelData.aspx", Page);


        }
    }
    protected void dlCourse_ItemCommand(object source, DataListCommandEventArgs e)
    {
        DalOperationUsers dos = new DalOperationUsers();

        if (e.CommandName == "delete")
        {
            int examArrangeListId =int.Parse( dlCourse.DataKeys[e.Item.ItemIndex].ToString());//取选中行编号  
            dos.DeleteExamArrangeById(examArrangeListId);
            Javascript.AlertAndRedirect("删除成功！", "/Administrator/ImportExamExcelData.aspx", Page);
        }
    }
}
