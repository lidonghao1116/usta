using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Model;
using USTA.Common;

public partial class Teacher_VSWorkNUnlineWork : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment4");

        if (Request["excellent"] != null || Request["studentNo"] != null)
        {
            DalOperationAboutSchoolWorks doas = new DalOperationAboutSchoolWorks();
            if (Request["excellent"].ToString().Trim().Equals("true"))
            {
                doas.UpdateSchoolWorkByschoolWorkNofityIdAndStudentNo(Master.schoolworkNotifyId, Request["studentNo"].ToString().Trim(), 1);//设置为1：优秀实验
            }
            else if (Request["excellent"].ToString().Trim().Equals("false"))
            {
                doas.UpdateSchoolWorkByschoolWorkNofityIdAndStudentNo(Master.schoolworkNotifyId, Request["studentNo"].ToString().Trim(), 0);//设置为0：非优秀实验
            }
        }

        if (!CommonUtility.SafeCheckByParams<string>(Request["schoolworkNotifyId"], ref Master.schoolworkNotifyId))
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        DataPaperWorkBind(Master.schoolworkNotifyId);
    }

    protected void DataPaperWorkBind(int schoolworkNotifyId)
    {
        DalOperationAboutSchoolWorks dalOperationAboutschoolwork = new DalOperationAboutSchoolWorks();
        DataView dv = dalOperationAboutschoolwork.FindAllSchoolWorksByschoolWorkNofityId(schoolworkNotifyId).Tables[0].DefaultView;

        this.AspNetPager3.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize;

        this.dlstpaperwork.DataSource = pds;
        this.dlstpaperwork.DataBind();
    }

    protected void AspNetPager3_PageChanged(object sender, EventArgs e)
    {
        DataPaperWorkBind(Master.schoolworkNotifyId);
    }
}