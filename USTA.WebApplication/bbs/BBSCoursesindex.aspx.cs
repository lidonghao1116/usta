using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using USTA.Dal;
using USTA.Common;
using USTA.Model;
using USTA.Bll;
using System.Collections;


public partial class bbs_BBSCoursesindex : System.Web.UI.Page
{
    public string fragmentFlag = "1";
    public string tags;
    public string tagNameType0;
    public string tagNameType1;

    public Hashtable ht0 = new Hashtable();
    public Hashtable ht1 = new Hashtable();
    public Hashtable ht2 = new Hashtable();
    public Hashtable ht3 = new Hashtable();
    public Hashtable ht4 = new Hashtable();
    public Hashtable ht5 = new Hashtable();
    public Hashtable ht6 = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ShowLiControl(this.Page, "liFragment1");

        
        if (!IsPostBack)
        {
            DalOperationAboutCourses doac = new DalOperationAboutCourses();
            DataTable dt = doac.GetHistoryTags().Tables[0];
            ddltTerms.Items.Add(new ListItem("当前学期", "当前学期"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddltTerms.Items.Add(new ListItem(CommonUtility.ChangeTermToString((dt.Rows[i]["termTag"].ToString())), dt.Rows[i]["termTag"].ToString()));
            }
            DatalistBind("当前学期");

           
        }
    }
    protected void DatalistBind(string term)
    {
        

        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet dsCourses = new DataSet();

       
        if ("当前学期" == term)
        {
            tags = "当前学期";
            dsCourses = dalOperationAboutBbs.GetAllForumsList();
        }
        else
        {
            //Response.Write(term);
           // Response.End();
            tags = CommonUtility.ChangeTermToString(term);
            dsCourses = dalOperationAboutBbs.GetAllForumsList(term);
        }
        //if (fragmentFlag.Equals("1"))
        //{

            foreach (DataRow dr0 in dsCourses.Tables["3"].Rows)
            {

                ht0.Add(dr0["courseNo"].ToString().Trim(), dr0["topicsCount"].ToString());
            }

            foreach (DataRow dr1 in dsCourses.Tables["4"].Rows)
            {

                ht1.Add(dr1["courseNo"].ToString().Trim(), dr1["postIdCount"].ToString());
            }

            foreach (DataRow dr2 in dsCourses.Tables["5"].Rows)
            {
                ht2.Add(dr2["courseNo"].ToString().Trim(), dr2["postUserName"].ToString());
                ht3.Add(dr2["courseNo"].ToString().Trim(), dr2["updateTime"].ToString());
            }

            foreach (DataRow dr3 in dsCourses.Tables["6"].Rows)
            {
                ht4.Add(dr3["courseNo"].ToString().Trim(), dr3["todayTopicsCount"].ToString());
            }

            foreach (DataRow dr4 in dsCourses.Tables["7"].Rows)
            {
                ht5.Add(dr4["courseNo"].ToString().Trim(), dr4["todayPostsCount"].ToString());
            }


            foreach (DataRow dr6 in dsCourses.Tables["8"].Rows)
            {
                ht6.Add(dr6["courseNo"].ToString().Trim(), dr6["updateTime"].ToString().Trim());
            }

            this.dlstAboutCourses.DataSource = dsCourses.Tables[0];
            this.dlstAboutCourses.DataBind();

        //}
        /**
        if (fragmentFlag.Equals("2"))
        {
            this.dlstpic.DataSource = dsCourses.Tables["2"];

            this.dlstpic.DataBind();
        }
        if (fragmentFlag.Equals("3"))
        {
            this.dlstAboutOther.DataSource = dsCourses.Tables[1];
            this.dlstAboutOther.DataBind();
        }
        if (fragmentFlag.Equals("4"))
        {
            DalOperationPatch dalpa = new DalOperationPatch();

            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            string src = dalpa.GetAvatar(user.userNo, user.userType);
            ltavatar.Text = "<img src=\"" + src + "\">";
        }*/
    }
    protected void ddltTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddltTerms.SelectedValue == "当前学期")
        {
            DatalistBind("当前学期");
        }
        else
        {
            DatalistBind(ddltTerms.SelectedValue);
        }
        ddltTerms.SelectedValue = ddltTerms.SelectedValue;
    }
}