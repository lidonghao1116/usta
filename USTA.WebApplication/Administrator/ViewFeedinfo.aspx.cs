using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Dal;
using USTA.Model;
using System.Configuration;
using USTA.PageBase;

public partial class Administrator_ViewFeedinfo : CheckUserWithCommonPageBase
{

    public FeedBack feedback;

    protected void Page_Load(object sender, EventArgs e)
    {
        int tryParseInt = 0;

        if (CommonUtility.SafeCheckByParams<string>(Request["feedBackId"], ref tryParseInt))
        {
            int feedbackId = tryParseInt;//取得URL参数
            DalOperationFeedBack dofb = new DalOperationFeedBack();
            feedback = dofb.FindFeedBackById(feedbackId); //绑定到对象实例
            if (feedback != null)
            {
                dofb.UpdateFeedBackIsReadById(feedbackId); //更新阅读的状态为已读
                if (!IsPostBack)
                {
                    txtbackinfo.Text = feedback.backInfo;
                }
            }
        }


    }

    protected void btnCommit_Click(object sender, EventArgs e)
    {
        DalOperationFeedBack dal = new DalOperationFeedBack();
        if (txtbackinfo.Text != null)
        {
            dal.Insertback(int.Parse(Request["feedBackId"]), CommonUtility.JavascriptStringFilter(txtbackinfo.Text.Trim()));
        }
        Javascript.RefreshParentWindow("ViewFeedBack.aspx", Page);
    }
}
