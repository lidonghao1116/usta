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

public partial class Teacher_VExpRExperimentSubed : System.Web.UI.Page
{
    public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment2");
        if (CommonUtility.SafeCheckByParams<string>(Request["experimentResourceId"], ref Master.experimentResourceId))
        {
            //设置优秀实验操作--start
            if (Request["excellent"] != null && Request["studentNo"] != null)
            {
                DalOperationAboutExperiment doae = new DalOperationAboutExperiment();
                if (Request["excellent"].ToString().Trim().Equals("true"))
                {
                    doae.UpdateExperimentByexperimentResourceIdAndStudentNo(Master.experimentResourceId, Request["studentNo"].ToString().Trim(), 1);//设置为1：优秀实验
                }
                else if (Request["excellent"].ToString().Trim().Equals("false"))
                {
                    doae.UpdateExperimentByexperimentResourceIdAndStudentNo(Master.experimentResourceId, Request["studentNo"].ToString().Trim(), 0);//设置为0：非优秀实验
                }
            }
            //设置优秀实验操作--end
            
            //提交列表
            DataBindExperments(Master.experimentResourceId);

        }

        else
        {
            Javascript.GoHistory(-1, Page);
            return;
        }

    }

    //绑定提交实验列表
    public void DataBindExperments(int experimentResourceId)
    {
        DalOperationAboutExperiment dalOperationAboutExperiment = new DalOperationAboutExperiment();

        string studentName = txtNameSearch.Text;
        DataView dv = null;
        if ((txtlow.Text != null && txtlow.Text.Length > 0) || (txthigh.Text != null && txthigh.Text.Length > 0))
        {
            float low = (txtlow.Text == null || txtlow.Text.Equals("")) ? float.MinValue : float.Parse(txtlow.Text.Trim());
            float high = (txthigh.Text == null || txtlow.Text.Equals("")) ? float.MaxValue : float.Parse(txthigh.Text.Trim());
            dv = dalOperationAboutExperiment.GetExperimentsByResourcesId(experimentResourceId,studentName,low,high).Tables[0].DefaultView;
        }
        else
        {
            dv = dalOperationAboutExperiment.GetExperimentsByResourcesId(experimentResourceId,studentName).Tables[0].DefaultView;
        }
        this.AspNetPager2.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = pageIndex - 1;
        pds.PageSize = CommonUtility.pageSize; ;

        this.ddlstExp.DataSource = pds;
        this.ddlstExp.DataBind();

        if (pds.Count > 0) {
            this.ddlstExp.ShowFooter = false;
        }
    }
    //提交实验分页
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataBindExperments(Master.experimentResourceId);
    }

    //获取附件
    public string GetURL(string aids)
    {
        DalOperationAttachments dalOperationAttachments = new DalOperationAttachments();
        return dalOperationAttachments.GetAttachmentsList(aids, ref Master.iframeCount, false,string.Empty);
    }
    protected void btnSubmitSearch_Click(object sender, EventArgs e)
    {
    }
}