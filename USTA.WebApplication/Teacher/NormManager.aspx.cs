using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using System.Text;

namespace USTA.WebApplication.Teacher
{
    public partial class NormManager : System.Web.UI.Page
    {
        public int pageIndex = HttpContext.Current.Request["page"] == null ? 1 : int.Parse(HttpContext.Current.Request["page"]);
        private static string pageName = "Teacher_NormManager";
        DataSet normValue;
        public String teacherNo;
        public String term;
        DalOperationNorm dalOperationNorm = new DalOperationNorm();
        DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
        protected void Page_Load(object sender, EventArgs e)
        {
             UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
             UserAuth auth = dalua.GetUserAuth(pageName);
            List<string> authIds = new List<string>();
            if(auth.userIds !=null)
            {
                string[] _ids = auth.userIds.Split(',');
                for (int i = 0; i < _ids.Length; i++)
                {
                    authIds.Add(_ids[i]);
                }
            }
            if (!authIds.Contains(UserCookiesInfo.userNo))
            {
                Response.Redirect("/Common/NotifyList.aspx");
                return;
            }
            //控制Tab的显示
            string fragmentFlag = "1";

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, divFragment1, divFragment2);

            if (!IsPostBack)
            {
              
                if (fragmentFlag.Equals("1"))
                {
                    this.txtKeyword.Text = Request["key"];
                    if (Request["teacherType"]==null||Request["teacherType"] == "")
                    {
                        this.ddltTeacherType.SelectedValue = "本院";
                    }
                    else
                    {
                        this.ddltTeacherType.SelectedValue = Request["teacherType"];
                    }
                    DataListBind();
                }
                if (fragmentFlag.Equals("2"))
                {
                    liFragment2.Visible = true;
                    DataBindSearchTermNormTagList();
                    if (Request["term"] != null)
                    {
                        term = Request["term"].ToString().Trim();
                    }
                    else
                    {
                        term = ddltNormTerm.SelectedValue;
                    }
                    if (Request["teacherNo"] == null)
                    {
                        Response.Redirect("/Teacher/NormManager.aspx?fragment=2");
                        return;
                    }
                    teacherNo = Request["TeacherNo"].Trim();
                    hiddenteacherNo.Value = teacherNo;

                    updatelabel();
                    //DataSet confirm = dalOperationNorm.GetNormConfirm(term, teacherNo);
                   
                    //Boolean isConfirm = IsConfirm(confirm);
                    //if (!isConfirm)
                    //{
                    //    this.confirmStatus.Text = "否";
                    //}
                    //else
                    //{
                    //    this.confirmStatus.Text = "是";
                    //}

                   
                    this.labelTeacherName.Text = new DalOperationAboutTeacher().GetTeacherById(teacherNo).teacherName;
                    normValue = dalOperationNorm.GetNormValuesByTermOut(term);
                    dlstDetailFirstNorm.DataSource = dalOperationNorm.GetFirstNorms(term);
                    dlstDetailFirstNorm.DataBind();

                }
            }
        }
        protected void ddltTeacherType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnQuery_Click(sender, e);
        }
        private Boolean IsConfirm(DataSet ds)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if ("0".Equals(ds.Tables[0].Rows[i]["type"].ToString().Trim()))
                    return true;
            }
            return false;
        }
        
        public void DataBindSearchTermNormTagList()
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
            DataTable dt = dal.GetTermYear().Tables[0];

            string termTag = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                termTag = dt.Rows[i]["termYear"].ToString().Trim();
                ListItem li = new ListItem("20" + termTag, termTag);
                this.ddltNormTerm.Items.Add(li);
            }
        }
        public string GetNormValue(int normId)
        {
            Norm norm = dalOperationNorm.getNormById(normId);
            int type = 0;
            if (norm != null) type = norm.type;
            if (normValue != null)
            {
                DataRow[] drs = normValue.Tables[0].Select("teacherNo='" + teacherNo + "' AND normId ='" + normId + "'");
                if (drs.Length > 0)
                    if (drs[0]["value"] != null)
                    {
                        return type == 0 ? float.Parse(drs[0]["value"].ToString().Trim()).ToString() : (drs[0]["textValue"] != null ? drs[0]["textValue"].ToString().Trim() : "");
                    }
            }
            return "0";

        }
        protected void dlstFirstNorm_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("dlstSecondNorm");
                DataRowView rowv = (DataRowView)e.Item.DataItem;
                int mainID = Convert.ToInt32(rowv["normId"]);
                DalOperationNorm dalOperationNorm = new DalOperationNorm();
                DataSet ds = dalOperationNorm.GetChildNorms(mainID,term);
                dataList.DataSource = ds.Tables[0].DefaultView;
                dataList.DataBind();
            }
        }
        protected void dlstDetailFirstNorm_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("dlstDetailSecondNorm");
                DataRowView rowv = (DataRowView)e.Item.DataItem;
                int mainID = Convert.ToInt32(rowv["normId"]);
                DalOperationNorm dalOperationNorm = new DalOperationNorm();
                DataSet ds = dalOperationNorm.GetChildNorms(mainID, term);
                dataList.DataSource = ds.Tables[0].DefaultView;
                dataList.DataBind();
            }
        }

        public string getFormulaShow(int id)
        {
            string _termYear = this.ddltNormTerm.SelectedValue;
            NormFormula formula = dalOperationNorm.GetFormula(id, _termYear);
            if (formula != null) return formula.formulaShow;
            else return "";
        }
        //模糊查询
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string key = txtKeyword.Text;
            string teacherType = ddltTeacherType.SelectedValue;
            Javascript.JavaScriptLocationHref("/Teacher/NormManager.aspx?key=" + key + "&teacherType=" + teacherType + "&page=" + pageIndex, Page);

        }
        //绑定搜索到的教师数据
        protected void DataListBind()
        {
            DalOperationUsers dos = new DalOperationUsers();
            DataView dv = dos.SearchTeacher(txtKeyword.Text.Trim(), ddltTeacherType.SelectedValue).Tables[0].DefaultView;
            this.AspNetPager2.RecordCount = dv.Count;
            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dv;
            pds.AllowPaging = true;

            pds.CurrentPageIndex = pageIndex - 1;
            pds.PageSize = CommonUtility.pageSize;

            this.dlSearchTeacher.DataSource = pds;
            this.dlSearchTeacher.DataBind();

            if (pds.Count > 0)
            {
                dlSearchTeacher.ShowFooter = false;
            }
        }
        protected void AspNetPager2_PageChanged(object sender, EventArgs e)
        {
            DataListBind();
        }
        protected void ddltNormTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            term = ddltNormTerm.SelectedValue;
            teacherNo = hiddenteacherNo.Value;
            this.labelTeacherName.Text = new DalOperationAboutTeacher().GetTeacherById(teacherNo).teacherName;
            normValue = dalOperationNorm.GetNormValuesByTermOut(term);
            //this.confirmStatus.Text = dalOperationNorm.GetNormConfirm(term, teacherNo) == null ? "否" : "是";
            dlstDetailFirstNorm.DataSource = dalOperationNorm.GetFirstNorms(term);
            dlstDetailFirstNorm.DataBind();
            //绑定学期标识下拉列表

            ddltNormTerm.Items.Clear();
            DataBindSearchTermNormTagList();
            ddltNormTerm.SelectedValue = term;
            updatelabel();
        }
        private void updatelabel()
        {
          DataTable dt = dalOperationNorm.GetCourseStatistic(teacherNo, term);
          if (dt.Rows.Count > 0)
          {
                
                StringBuilder sb = new StringBuilder();
                sb.Append("<table class='datagrid2'><tr>");
                for (int i = 3; i < dt.Columns.Count; i++)
                {
                    sb.Append("<th>" + dt.Columns[i].ColumnName + "</th>");
                }
                sb.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sb.Append("<tr>");
                    for (int j = 3; j < dt.Columns.Count; j++)
                    {
                        string val = dt.Rows[i][dt.Columns[j]].ToString().Trim();
                        if (val.Length <= 0)
                        {
                            sb.Append("<td>无</td>");
                        }
                        else
                        {
                            if (j > 7 && !!System.Text.RegularExpressions.Regex.IsMatch(val, "^(-?\\d+)(\\.\\d+)?$"))
                            {
                                sb.Append("<td><div id='course_p_" + i + "_" + j + "' style='display:none'><p>" + val + "<p></div><span><a href='#TB_inline?height=150&width=500&inlineId=course_p_" + i + "_" + j + "' class='thickbox'>查看</a></span></td>");
                            }
                            else
                            {
                                sb.Append("<td>" + dt.Rows[i][dt.Columns[j]] + "</td>");
                            }
                        }
                    }
                    string termTag = dt.Rows[i]["termTag"].ToString().Trim();
                    string classID = dt.Rows[i]["classID"].ToString().Trim();
                    string courseNo = dt.Rows[i]["courseNo"].ToString().Trim();

                    sb.Append("</tr>");
                }

                sb.Append("</table>");
                this.shuoshijx.Text = sb.ToString();
            }
        }
      
    }
}