using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using USTA.Model;
using USTA.Common;
using System.Web;

namespace USTA.Dal
{
    public class DalBase
    {
         #region
        /// <summary>
        /// SqlConnection变量  
        /// </summary>
        public SqlConnection conn
        {
            set;
            get;
        }
        #endregion

        #region
        /// <summary>
        /// 构造函数
        /// </summary>
        public DalBase()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion
    }
}
