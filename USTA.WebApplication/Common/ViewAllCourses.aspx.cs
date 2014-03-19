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

public partial class Common_ViewAllCourses : System.Web.UI.Page
{
    public string termTag;
    public string tag;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["termTag"] == null)
        {
            Javascript.GoHistory(-1, Page);
            return;
        }
        
        termTag = Request["termTag"].ToString();//获取Url参数,学期标识
        tag = CommonUtility.ChangeTermToString(Request["termTag"].ToString()); //学期标识字符串自定义转换
        if (!IsPostBack)
        {
            DataListBind();
        }
    }

    //绑定学期课程信息到DataList
    public void DataListBind()
    {
        DalOperationAboutCourses doac = new DalOperationAboutCourses();
        DataView dv = doac.FindCourseByTermTage(termTag).Tables[0].DefaultView;

        this.dlTermCourse.DataSource = dv;
        this.dlTermCourse.DataBind();
    }


    protected void dlTermCourse_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "view")
        {
            //执行页面跳转到，课程详细页

            Javascript.JavaScriptLocationHref("/Common/CInfoCourseIntro.aspx?courseNo=" + dlTermCourse.DataKeys[e.Item.ItemIndex].ToString(), Page);
             
        }
    }
}
