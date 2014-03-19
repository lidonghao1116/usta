using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace USTA.Model
{
    public class CheckUserLogin
    {
        public CheckUserLogin()
        {

        }

        public int userType
        {
            set;
            get;
        }

        public string spName
        {
            set;
            get;
        }

        public SqlParameter[] sqlParammeters
        {
            get;
            set;
        }

        public bool isAdmin
        {
            get;
            set;
        }
    }
}
