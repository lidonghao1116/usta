using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USTA.Common;
using System.Collections;

namespace USTA.WebApplication.Administrator
{
    public partial class ShowUSTALogs : System.Web.UI.Page
    {
        public DateTime dateTime = DateTime.Now;
        
        public string page = (HttpContext.Current.Request.QueryString["page"] == null ? "1" : HttpContext.Current.Request.QueryString["page"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["datetime"] != null && !CommonUtility.SafeCheckByParams<string>(Request["datetime"], ref dateTime))
            {
                Response.Write("参数有误");
                return;
            }

            txtDateTime.Value = dateTime.ToString("yyyy-MM-dd");

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
                BindData();
            }
        }


        protected void BindData()
        {
            int pageNum = 1;

            if (txtDateTime.Value.Trim().Length == 0)
            {
                txtDateTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }

            CommonUtility.SafeCheckByParams<string>(page, ref pageNum);

            ResultSet resultSet = MongoDBLog.ShowLogs(txtDateTime.Value, CommonUtility.pageSize, pageNum);

            AspNetPager2.RecordCount = Convert.ToInt32(resultSet.resultCount);
            AspNetPager2.PageSize = CommonUtility.pageSize;

            AspNetPager2.UrlRewritePattern = "ShowUSTALogs.aspx?datetime=" + dateTime + "&page={0}";


            DataListUSTALogs.DataSource = resultSet.result;
            DataListUSTALogs.DataBind();

            if (resultSet.result.Count > 0)
            {
                DataListUSTALogs.ShowFooter = false;
            }
        }
    }
}