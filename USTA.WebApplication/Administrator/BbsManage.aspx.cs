using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Common;
using USTA.Dal;
using USTA.Model;

public partial class Administrator_BbsManage : System.Web.UI.Page
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

            //删除版面
            if (Request["del"] == "true" && Request["forumId"] != null)
            {
                DalOperationAboutBbsManage dalOperationAboutBbsManage = new DalOperationAboutBbsManage();
                dalOperationAboutBbsManage.DeleteForumByForumId(Request["forumId"]);
            }

            if (fragmentFlag.Equals("1"))
            {
                //TODO
                DalOperationAboutBbsManage dalOperationAboutBbsManage = new DalOperationAboutBbsManage();
                DataSet ds = dalOperationAboutBbsManage.GetAllForums();
                dlstforums.DataSource = ds.Tables[0];
                dlstforums.DataBind();
            }

            if (fragmentFlag.Equals("2"))
            {
                DalOperationAboutTeacher dalt = new DalOperationAboutTeacher();
        
                dlstteachers.DataSource = dalt.GetTeachers().Tables[0];
 
                dlstteachers.DataBind();
   
                txtTitle.Attributes.Add("class", "required");
            }

            if (fragmentFlag.Equals("3"))
            {
                txtSearchString.Attributes.Add("class", "required");
            }
        }
    }

    protected void btsearch_Click(object sender, EventArgs e)
    {
        
        dlsttopicresult.DataSource = null;
        dlsttopicresult.DataBind();
        dlstpostresult.DataSource = null;
        dlstpostresult.DataBind();
        
        if (ddlttype.Text == "0")
        {
            dlsttopicresult.Visible = true;
            AspNetPager2.Visible = true;
            dlstpostresult.Visible = false;
            AspNetPager3.Visible = false;
            DataResultTopic();

        }
        else
        {
            AspNetPager2.Visible = false;
            AspNetPager3.Visible = true;
            dlsttopicresult.Visible = false;
            dlstpostresult.Visible = true;
            DataResultPost();
        }
    }

    protected void btnCommit_Click(object sender, EventArgs e)
    {
        BbsForum forum = new BbsForum
        {
            forumTitle = txtTitle.Text,
            userNo=this.userNo.Value,
            userType=int.Parse(this.userType.Value),
            forumType=int.Parse(this.ddltforunType.SelectedValue)
        };
        DalOperationAboutBbsManage DalOperationAboutBbsManage = new DalOperationAboutBbsManage();
        DalOperationAboutBbsManage.AddForumInfo(forum);
        Javascript.AlertAndRedirect("添加成功！","BbsManage.aspx",Page);
    }

    
    protected void DataResultTopic()
    {
        DalOperationAboutBbs DalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet ds = DalOperationAboutBbs.SearchTopic(txtSearchString.Text);
        DataView dv = ds.Tables[0].DefaultView;
        this.AspNetPager2.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = AspNetPager2.CurrentPageIndex - 1;
        pds.PageSize = AspNetPager2.PageSize;

        dlsttopicresult.DataSource = pds;
        dlsttopicresult.DataBind();
    }

    protected void DataResultPost()
    {
        DalOperationAboutBbs DalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet ds = DalOperationAboutBbs.SearchPost(txtSearchString.Text);

        DataView dv = ds.Tables[0].DefaultView;


        this.AspNetPager3.RecordCount = dv.Count;

        PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
        pds.DataSource = dv;
        pds.AllowPaging = true;

        pds.CurrentPageIndex = AspNetPager3.CurrentPageIndex - 1;
        pds.PageSize = AspNetPager3.PageSize;
        dlstpostresult.DataSource = pds;
        dlstpostresult.DataBind();
    }
    protected void AspNetPager2_PageChanged(object sender, EventArgs e)
    {
        DataResultTopic();

    }
    protected void AspNetPager3_PageChanged(object sender, EventArgs e)
    {
        DataResultPost();
    }
    
}
