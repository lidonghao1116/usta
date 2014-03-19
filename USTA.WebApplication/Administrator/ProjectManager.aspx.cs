using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Common;
using USTA.Dal;
using USTA.Model;
using System.Data;

namespace USTA.WebApplication.Administrator
{
    public partial class ProjectManager : System.Web.UI.Page
    {
        private static string pageName = "ProjectManager";
        public List<string> authIds = new List<string>();
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
               , divFragment1, divFragment2, divFragment3);

            if (fragmentFlag.Equals("1"))
            {
                if (!IsPostBack)
                {

                    TeacherDataListBind();
                }
            }
        }

        //模糊查询
        protected void SearchTeacher_Click(object sender, EventArgs e)
        {
            string teacherName = this.txtKeyword.Text;
            Response.Redirect("/Administrator/ProjectManager.aspx?teacherName=" + teacherName);
        }

        //绑定搜索到的教师数据
        protected void TeacherDataListBind()
        {
            DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
            UserAuth auth = dalua.GetUserAuth(pageName);
            if (auth != null)
            {
                string ids = auth.userIds;
                string[] _ids = ids.Split(',');
                for (int i = 0; i < _ids.Length; i++)
                {
                    authIds.Add(_ids[i]);
                }
            }

            DalOperationUsers dal = new DalOperationUsers();
            
            string keyWord = "";
            
           

            if (Request["teacherName"] != null)
            {
                keyWord = Request["teacherName"].Trim();
                this.txtKeyword.Text = keyWord;
            }

            DataView dv = dal.SearchTeacherByTermTagAndKeyword(null, keyWord, 0).DefaultView;

            this.TeacherListPager.RecordCount = dv.Count;
            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dv;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = pageIndex - 1;
            pds.PageSize = CommonUtility.pageSize;

            this.dlSearchTeacher.DataSource = pds;
            this.dlSearchTeacher.DataBind();

            if (pds.Count == 0)
            {
                dlSearchTeacher.ShowFooter = true;
            }
            else
            {
                dlSearchTeacher.ShowFooter = false;
            }


        }
        //搜索到的教师数据分页
        protected void TeacherSearch_PageChanged(object sender, EventArgs e)
        {
            TeacherDataListBind();
        }

        protected void dlSearchTeacher_ItemCommand(object source, DataListCommandEventArgs e)
        {
            string teacherNowSelected = this.dlSearchTeacher.DataKeys[e.Item.ItemIndex].ToString(); //取选中行教师的编号
            DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
            UserAuth userAuth = dalua.GetUserAuth(pageName);
            if (e.CommandName == "addAuth")
            {
                if (userAuth == null)
                {
                    userAuth = new UserAuth();
                    userAuth.pageName = pageName;
                    userAuth.userIds = teacherNowSelected;
                }
                else
                {
                    if (userAuth.userIds == null || userAuth.userIds.Equals(""))
                    {
                        userAuth.userIds = teacherNowSelected;
                    }
                    else
                    {
                        userAuth.userIds = userAuth.userIds + "," + teacherNowSelected;
                    }
                }
            }
            else if (e.CommandName == "removeAuth")
            {
                if (userAuth == null) return;
                string[] ids = userAuth.userIds.Split(',');

                List<string> list = new List<string>();

                for (int i = 0; i < ids.Length; i++)
                {
                    if (!ids[i].Equals(teacherNowSelected))
                    {
                        list.Add(ids[i]);
                    }
                }
                userAuth.userIds = string.Join(",", list.ToArray());
            }

            dalua.setUserAuth(userAuth);
            Javascript.JavaScriptLocationHref("ProjectManager.aspx?fragment=1&teacherName=" + this.txtKeyword.Text.Trim(), Page);
        }

        protected void dlSearchTeacher_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("dlstSecondNorm");
                DataRowView rowv = (DataRowView)e.Item.DataItem;
                string teacherNo = rowv["teacherNo"].ToString().Trim();

                if (this.isAuth(teacherNo))
                {
                    e.Item.FindControl("LinkButton_RemoveProjectManagerPermission").Visible = true;
                    e.Item.FindControl("LinkButton_AddProjectManagerPermission").Visible = false;
                }
                else
                {
                    e.Item.FindControl("LinkButton_RemoveProjectManagerPermission").Visible = false;
                    e.Item.FindControl("LinkButton_AddProjectManagerPermission").Visible = true;
                }
            }
        }

        public Boolean isAuth(string teacherNo)
        {
            return authIds.Contains(teacherNo);
        }

        

        private void DataBindSalaryStatus(ListItemCollection listItemCollection, string salaryEntryStatus)
        {
            if (salaryEntryStatus != null)
            {
                salaryEntryStatus = salaryEntryStatus.Trim();
                foreach (ListItem li in listItemCollection)
                {
                    if (li.Value == salaryEntryStatus)
                    {
                        li.Selected = true;
                        break;
                    }
                }

            }
        }

        private string SearchTeacherIds(string keyWord)
        {

            DalOperationUsers dos = new DalOperationUsers();
            DataView dv = dos.SearchUserByTypeAndKeywod(1, keyWord).DefaultView;
            DataRowCollection drCollection = dv.Table.Rows;
            List<string> tidList = new List<string>();

            for (var i = 0; i < drCollection.Count; i++)
            {
                string s = drCollection[i]["teacherNo"].ToString();
                tidList.Add("'" + s + "'");
            }

            string tids = string.Join(",", tidList.ToArray());
            return tids;
        }
    }
}