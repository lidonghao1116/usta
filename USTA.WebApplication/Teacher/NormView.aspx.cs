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
    public partial class NormView : System.Web.UI.Page
    {
        DataSet normValue;
        public Boolean _IsConfirm;
        DalOperationNorm dalOperationNorm = new DalOperationNorm();
        public string teacherNo;
        protected void Page_Load(object sender, EventArgs e)
        {
            DataBindSearchTermNormTagList();
            term = ddltNormTerm.SelectedValue;
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            normValue = dalOperationNorm.GetNormValuesByTermOut(term);
            teacherNo = UserCookiesInfo.userNo;
            DataSet normConfirm = dalOperationNorm.GetNormConfirm(term, teacherNo);
            this.dlstConfirm.DataSource = normConfirm;
            this.dlstConfirm.DataBind();
            _IsConfirm = IsConfirm(normConfirm);
            dlstDetailFirstNorm.DataSource = dalOperationNorm.GetFirstNorms(term);
            dlstDetailFirstNorm.DataBind();
            updatelabel();

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
        public string term;
        protected void ddltNormTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            term = ddltNormTerm.SelectedValue;
            normValue = dalOperationNorm.GetNormValuesByTermOut(term);
            dlstDetailFirstNorm.DataSource = dalOperationNorm.GetFirstNorms(term);
            dlstDetailFirstNorm.DataBind();
            //绑定学期标识下拉列表

            ddltNormTerm.Items.Clear();
            DataBindSearchTermNormTagList();
            ddltNormTerm.SelectedValue = term;
            updatelabel();
        }
        public void DataBindSearchTermNormTagList()
        {
            DalOperationAboutGradeCheck dal = new DalOperationAboutGradeCheck();
            DataTable dt = dal.GetTermYear().Tables[0];

            string termTag = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                termTag = dt.Rows[i]["termYear"].ToString().Trim();
                ListItem li = new ListItem("20"+termTag, termTag);
                this.ddltNormTerm.Items.Add(li);
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
                DataSet ds = dalOperationNorm.GetChildNorms(mainID,term);
                dataList.DataSource = ds.Tables[0].DefaultView;
                dataList.DataBind();
            }
        }
        public string GetNormValue(int normId)
        {
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            Norm norm = dalOperationNorm.getNormById(normId);
            int type = 0;
            if (norm != null) type = norm.type;
            if (normValue != null)
            {
                DataRow[] drs = normValue.Tables[0].Select("teacherNo='" + UserCookiesInfo.userNo + "' AND normId ='" + normId + "'");
                if (drs.Length > 0)
                    if (drs[0]["value"] != null)
                    {
                        return type == 0 ? float.Parse(drs[0]["value"].ToString().Trim()).ToString() : (drs[0]["textValue"] != null ? drs[0]["textValue"].ToString().Trim() : "");
                    }
            }
            return "0";

        }


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
             UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationNorm dalOperationNorm = new DalOperationNorm();
            NormConfirm nc = new NormConfirm{
                term=term,
                teacherNo = UserCookiesInfo.userNo,
                question="OK",
                answer="",
                value = GetNormValue(0),
                type = 0,
                
                
            };
            dalOperationNorm.AddNormConfirm(nc);
            Javascript.JavaScriptLocationHref("/Teacher/NormView.aspx", Page);
        }
        public string getFormulaShow(int id)
        {
            NormFormula formula = dalOperationNorm.GetFormula(id,term);
            if (formula != null) return formula.formulaShow;
            else return "";
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
                        sb.Append("<td>" + dt.Rows[i][dt.Columns[j]] + "</td>");
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