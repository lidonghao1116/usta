using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace USTA.Model
{
    class AboutStudent
    {
        public AboutStudent()
        {
        }
        public string StudentNo
        {
        get;
        set;
     }
        public SqlParameter[] sqlParammeters
        {
            get;
            set;
        }
    }
}
