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

public partial class bbs_BBSOtherindex : System.Web.UI.Page
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
        Master.ShowLiControl(this.Page, "liFragment3");
        DalOperationAboutBbs dalOperationAboutBbs = new DalOperationAboutBbs();
        DataSet dsCourses = dalOperationAboutBbs.GetAllForumsList();
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
        this.dlstAboutOther.DataSource = dsCourses.Tables[1];
        this.dlstAboutOther.DataBind();
    }
}