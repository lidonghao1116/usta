using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.Bll;

public partial class Common_FeedBack : System.Web.UI.Page
{
    public string fragmentFlag = (HttpContext.Current.Request["fragmentFlag"] != null ? HttpContext.Current.Request["fragmentFlag"] : "1");
    protected void Page_Load(object sender, EventArgs e)
    {
        //控制Tab的显示

        if (Request["fragment"] != null)
        {
            fragmentFlag = Request["fragment"];
        }

        CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
            , divFragment1, divFragment2, divFragment3);

        if (fragmentFlag == "2")
        {
            DalOperationFeedBack dal = new DalOperationFeedBack();
            dlstfeeds.DataSource = dal.FindByUser(BllOperationAboutUser.GetUserCookiesInfo()).Tables[0];
            dlstfeeds.DataBind();
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Length == 0 || txtContent.Text.Trim().Length == 0)
        {

            Javascript.GoHistory(-1, "标题和内容不能为空，请输入！", Page);
        }
        else
        {
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationFeedBack dofb = new DalOperationFeedBack();
            DalOperationAboutStudent dals = new DalOperationAboutStudent();
            FeedBack feedback = new FeedBack();

            feedback.feedBackTitle = CommonUtility.JavascriptStringFilter(txtTitle.Text.Trim());
            feedback.feedBackContent =  CommonUtility.JavascriptStringFilter(txtContent.Text.Trim());
            feedback.feedBackContactTo =  CommonUtility.JavascriptStringFilter(txtContact.Text.Trim())+" 由"+user.userName+"反馈 ";
            feedback.backUserNo=user.userNo;
            feedback.backUserType = user.userType;
            feedback.type = Convert.ToInt32( this.ddltType.SelectedValue);
            if (user.userType == 3 &&feedback.type ==2)
            {
                feedback.resolver = dals.GetTeacherNoByStudent(user.userNo);
            }
            try
            {
                dofb.AddFeedBack(feedback); //保存反馈意见
                Javascript.AlertAndRedirect("意见反馈成功！", "/Common/FeedBack.aspx?fragment=2", Page);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "意见反馈失败,请检查格式是否有误！", Page);
            }

        }
    }
}
