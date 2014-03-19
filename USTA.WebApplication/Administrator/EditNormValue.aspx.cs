using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.PageBase;

namespace USTA.WebApplication.Administrator
{
    public partial class EditNormValue : CheckUserWithCommonPageBase
    {
        string teacherNo = HttpContext.Current.Request["teacherNo"].Trim();
        string term = HttpContext.Current.Request["term"].Trim();
        int normId = Convert.ToInt32(HttpContext.Current.Request["normId"].Trim());

        string termtag = HttpContext.Current.Request["termtag"] == null ? "" : HttpContext.Current.Request["termtag"].Trim();
        string classID = HttpContext.Current.Request["classID"] == null ? "" : HttpContext.Current.Request["classID"].Trim();
        string courseNo = HttpContext.Current.Request["courseNo"] == null ? "" : HttpContext.Current.Request["courseNo"].Trim();
        string atType = HttpContext.Current.Request["atType"] == null ? "" : HttpContext.Current.Request["atType"].Trim();
        DalOperationNorm dal = new DalOperationNorm();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               
                
                    this.TextNormId.Value = normId.ToString();
               
                this.TextTeacherNo.Value = teacherNo;
                this.TextTerm.Value = term;
              
                Norm norm = dal.getNormById(normId);
                if (norm == null)
                {
                    if (normId == -1) {
                        this.lblnormTitle.Text = "硕士教学";
                    }
                }
                else
                {
                    this.lblnormTitle.Text = norm.name;
                }
                DataSet ds = dal.GetChildNorms(normId,term);
                this.dsltchildNorm.DataSource = ds;
                this.dsltchildNorm.DataBind();
                for (int i = 0; i < dsltchildNorm.Items.Count; i++)
                {
                    
                }
                //if (norm.type == 0)
                //{
                //    this.TextNormValue.CssClass = "required number";
                //    this.TextNormValue.Text = normValue != null?normValue.value.ToString():"0";
                //}
                //else
                //{
                //    this.TextNormValue.CssClass = "required";
                //     this.TextNormValue.Text = normValue != null?normValue.textValue:"";
                //}
            }
        }
        protected void dsltchildNorm_OnItemDataBound(object sender, DataListItemEventArgs e)
        {
            TextBox tbox = (TextBox)e.Item.FindControl("itemValue");
            HiddenField hidf = (HiddenField)e.Item.FindControl("hidnormId");
            DataRowView rowv = (DataRowView)e.Item.DataItem;
          
            int mainId = Convert.ToInt32(rowv["normId"].ToString());

            NormValue normValue=null;
            if (courseNo != "")
            {
                int atTypeInt = 0;
                if (atType.Equals("教师")) atTypeInt = 1;
                else if (atType.Equals("助教"))
                {
                    atTypeInt = 2;
                }
                normValue = dal.getNormValue(mainId, teacherNo, term, courseNo, classID, termtag, atTypeInt);
            }
            else
            {
                normValue = dal.getNormValue(mainId, teacherNo, term);
            }
            Norm normp = dal.getNormById(mainId);
            hidf.Value = mainId.ToString();
            if (normp.type == 0)
            {
                tbox.CssClass = "required number";
                tbox.Text = normValue==null?"0":normValue.value.ToString();
            }
            else
           {
               tbox.TextMode = TextBoxMode.MultiLine;
               tbox.Rows = 5;
               tbox.Width = Unit.Percentage(100);
                tbox.CssClass = "required";
                tbox.Text = normValue == null ? "" : normValue.textValue==null?"":normValue.textValue.ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
        //    DalOperationNorm dal = new DalOperationNorm();
        //    int normId = Convert.ToInt32(this.TextNormId.Value);
        //    string teacherNo = this.TextTeacherNo.Value;
        //    string term = this.TextTerm.Value;
        //    string value =this.TextNormValue.Text.Trim() ;
            Norm norm = dal.getNormById(normId);
            int type = 0;
            if (norm != null)
                type = norm.type;
            //if (norm != null)
            //{
            for (int i = 0; i < this.dsltchildNorm.Items.Count; i++)
            {
                TextBox tbox = this.dsltchildNorm.Items[i].FindControl("itemValue") as TextBox;
                HiddenField hidf = this.dsltchildNorm.Items[i].FindControl("hidnormId") as HiddenField;
                DataRowView rowv = (DataRowView)this.dsltchildNorm.Items[i].DataItem;

                if (termtag.Equals("") && classID.Equals("") && courseNo.Equals(""))
                {
                    dal.setNormValue(int.Parse(hidf.Value), teacherNo, term, tbox.Text);
                }
                else
                {
                    int atTypeInt = 0;
                    if (atType.Equals("教师")) atTypeInt = 1;
                    else
                    {
                        atTypeInt = 2;
                    }
                    dal.setNormValue(int.Parse(hidf.Value), teacherNo, term, tbox.Text, courseNo, classID, termtag, atTypeInt);
                }

            }

                if (type == 0)
                {
                    NormFormula fomula = dal.GetFormula(normId, term);
                    if (fomula == null)
                    {
                        Javascript.Alert("执行错误!请查看是否设置规则！", Page);
                        return;
                    }
                    dal.Execute(fomula, teacherNo, term);
                    NormFormula fomularoot = dal.GetFormula(0, term);
                    if (fomularoot == null)
                    {
                        Javascript.Alert("执行错误!请查看是否设置总规则！", Page);
                        return;
                    }
                    dal.Execute(fomularoot, teacherNo, term);
                }
            //}
            if ("template".Equals(teacherNo))
            {
                Javascript.RefreshParentWindow("/Administrator/NormManager.aspx?fragment=5&term=" + term, Page);

            }
            else
            {
                Javascript.RefreshParentWindow("/Administrator/NormManager.aspx?fragment=3&teacherNo=" + teacherNo + "&term=" + term, Page);
            }
        }
    }
}