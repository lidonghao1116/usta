using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using USTA.Model;

public partial class Teacher_CInfoCourseResource : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Master.ShowLiControl(this.Page, "liFragment5");
            //删除课程资源
            int resourseId = 0;
            if (CommonUtility.SafeCheckByParams<string>(Request["resourceId"], ref resourseId))
            {
                DalOperationAboutCourseNotifyInfo dalOperationAboutCourseNotifyInfo = new DalOperationAboutCourseNotifyInfo();
                if (Request["op"] == "delete")
                {
                    DalOperationAboutCourseResources dalOperationAboutCourseResources = new DalOperationAboutCourseResources();

                    dalOperationAboutCourseResources.DelCourseResourcesbyId(resourseId);
                }
            }
        }      
        UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
        DalOperationAboutCourses DalOperationAboutCourses = new DalOperationAboutCourses();
        DataSet coursesInfo = null;
        coursesInfo = DalOperationAboutCourses.GetCoursesInfo(Master.courseNo.ToString().Trim(), Master.classID, Master.termtag, "5");

        dlstCourseResource.DataSource = coursesInfo.Tables[0];
        dlstCourseResource.DataBind();
    }

    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false,string.Empty);
    }
    /// <summary>
    /// 判断是否为新
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool isNew(string date)
    {
        return DateTime.Now.AddDays(-CommonUtility.GetNewDays()).CompareTo(Convert.ToDateTime(date)) < 0;
    }

    //protected void dlstCourseResource_DeleteCommand(object source, DataListCommandEventArgs e)
    //{
    //    if (e.CommandName == "delete")
    //    {
    //        DalOperationAboutCourseResources dalOperationAboutCourseResources = new DalOperationAboutCourseResources();
    //        string resourceId = dlstCourseResource.DataKeys[e.Item.ItemIndex].ToString();
    //        dalOperationAboutCourseResources.DelCourseResourcesbyId(int.Parse(resourceId));
    //        Javascript.JavaScriptLocationHref("CourseInfo.aspx?courseNo=" + Request["courseNo"] + "&fragment=5", Page);
    //        //Response.Redirect("CourseInfo.aspx?courseNo=" + Request["courseNo"] + "&fragment=5" );
    //    }
    //}
}