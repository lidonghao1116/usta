using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace USTA.Model
{
    public class ExcelSheetData
    {
        public ExcelSheetData()
        {

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

        /// <summary>
        /// 
        /// </summary>
        public string sheetName
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int sheetRowNo
        {
            set;
            get;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public int sheetColNo
        //{
        //    set;
        //    get;
        //}
    }
}