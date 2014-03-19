using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Common;
using USTA.Dal;
using USTA.Model;
using USTA.PageBase;
using System.Configuration;

namespace USTA.WebApplication.Administrator
{
    public partial class ViewNormAuth : CheckUserWithCommonPageBase
    {
        private static string pageName = "Teacher_NormManager";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DalOperationNorm daln = new DalOperationNorm();
                DalOperationAboutTeacher dalt = new DalOperationAboutTeacher();
                DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
                UserAuth auth = dalua.GetUserAuth(pageName);
                DataSet ds = dalt.GetTeachers();
                DataTable dt = new DataTable();
                dt.Columns.Add("teacherNo");
                dt.Columns.Add("teacherName");
                if (auth != null)
                {
                    if (auth.userIds != null)
                    {
                        foreach (string s in auth.userIds.Split(','))
                        {
                            DataRow[] drs = ds.Tables[0].Select("teacherNo='" + s + "'");
                            if (drs.Length > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr.SetField("teacherNo", drs[0]["teacherNo"].ToString());
                                dr.SetField("teacherName", drs[0]["teacherName"].ToString());

                                dt.Rows.Add(dr);
                            }
                        }
                        dsltAuthTeacher.DataSource = dt;
                    }
                }
                dsltAuthTeacher.DataBind();
            }

        }

        protected void dsltAuthTeacher_OnItemCommand(object source, DataListCommandEventArgs e)
        {
            string teacherNoSelect = this.dsltAuthTeacher.DataKeys[e.Item.ItemIndex].ToString();//取选中行教师编号   
            if (e.CommandName.Equals("remove"))
            {
                DalOperationNorm dalOperationNorm = new DalOperationNorm();
                DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
                UserAuth userAuth = dalua.GetUserAuth(pageName);
                string[] ids = userAuth.userIds.Split(',');

                List<string> list = new List<string>();

                for (int i = 0; i < ids.Length; i++)
                {
                    if (!ids[i].Equals(teacherNoSelect))
                    {
                        list.Add(ids[i]);
                    }
                }
                userAuth.userIds = string.Join(",", list.ToArray());
                dalua.setUserAuth(userAuth);
                Javascript.JavaScriptLocationHref("/Administrator/ViewNormAuth.aspx", Page);
            }
        }

    }
}