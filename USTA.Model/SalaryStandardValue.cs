using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class SalaryStandardValue
    {
        #region
        /// <summary>
        /// 
        /// </summary>
        public int SalaryStandardValueId{
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int SalaryItemId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public float SalaryItemValue
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedTime
        {
            set;
            get;
        }


        #endregion
    }
}
