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

public partial class Common_ViewHistoryData : System.Web.UI.Page
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


            if (fragmentFlag.Equals("2"))
            {
                ViewData(Request["termTag"]);
            }

            DataListBind();
        }
    }

    protected void ViewData(string termTag)
    {
     
       DalOperationAboutCourses doac = new DalOperationAboutCourses();
       DataView dv = doac.FindCourseByTermTage(termTag).Tables[0].DefaultView;

       this.DataList1.DataSource = dv;
       this.DataList1.DataBind();
    }
    //绑定课程的学期标识信息到DataList
    public void DataListBind()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataView dv = doac.GetHistoryTags().Tables[0].DefaultView;

        this.dlTermCourse.DataSource = dv;
        this.dlTermCourse.DataBind();
    }
    //protected void dlTermCourse_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    DalOperationAboutCourses doac = new DalOperationAboutCourses();

    //    if (e.CommandName == "view")
    //    {
    //        string termTag = dlTermCourse.DataKeys[e.Item.ItemIndex].ToString();//取选中行课程标识
    //        Response.Redirect("/Common/ViewAllCourses.aspx?termTag=" + termTag);
    //    }
    //}
}
