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
using USTA.PageBase;


namespace USTA.WebApplication.Administrator
{
    public partial class EditNormFormula : CheckUserWithCommonPageBase
    {
        string termYear;
        public string normIds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["normId"] != null && Request["termYear"]!=null)
            {
                 normIds = Request["normId"].Trim();
                termYear = Request["termYear"].Trim();
                DalOperationNorm dal = new DalOperationNorm();
                this.dlstChildNorm.DataSource = dal.GetChildNorms(Convert.ToInt32(normIds),0,termYear);
                this.dlstChildNorm.DataBind();

               

            }
            
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            NormFormula normformula = new NormFormula
            {
                targetNormId = Convert.ToInt32(Request["normId"].ToString().Trim()),
                formula = this.formula.Value,
                formulaShow = this.TextNormFormula.Value,
                termYear = termYear
            };
          
            DalOperationNorm dal = new DalOperationNorm();
            dal.setFormula(normformula);
            NormFormula rootformula = dal.GetFormula(0,termYear);

            DalOperationAboutTeacher dalteacher = new DalOperationAboutTeacher();
            DataSet dsTeacher = dalteacher.GetTeachers();
            for (int i = 0; i < dsTeacher.Tables[0].Rows.Count; i++)
            {
                string teacherNo = dsTeacher.Tables[0].Rows[i]["teacherNo"].ToString().Trim();
                
                    if (normformula != null)
                    {
                        dal.Execute(normformula, teacherNo,termYear);

                    }
                    if (rootformula != null)
                    {
                        dal.Execute(rootformula, teacherNo, termYear);
                    }
                
               
            }
            string t = "template";
            if (normformula != null)
            {
                dal.Execute(normformula, t, termYear);

            }
            if (rootformula != null)
            {
                dal.Execute(rootformula, t, termYear);
            }
            Javascript.RefreshParentWindow("/Administrator/NormManager.aspx?fragment=2&&term=" + Request["termYear"], Page);
        }
    }
}