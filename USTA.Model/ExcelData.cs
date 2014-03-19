using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class ExcelData
    {
        public ExcelData()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public List<ExcelSheetData> excelSheetData
        {
            set;
            get;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public List<ExcelPasswordMapping> excelPasswordMapping
        //{
        //    set;
        //    get;
        //}
        /// <summary>
        /// 
        /// </summary>
        public List<PasswordMapping> excelPasswordMapping
        {
            set;
            get;
        }

        //public List<ExcelSheetInfo> excelSheetInfo
        //{
        //    set;
        //    get;
        //}
    }
}
