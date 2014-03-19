using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class SalaryItem
    {
       
        /// <summary>
        /// 
        /// </summary>
        public SalaryItem() 
        {
        
        }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int salaryItemId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string salaryItemName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string salaryItemUnit
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string salaryItemDesc
        {
            set;
            get;
        }

        public int useFor
        {
            set;
            get;
        }

        public int salaryItemStatus
        {
            set;
            get;
        }

        public bool hasTax
        {
            set;
            get;
        }

        public bool isDefaultChecked
        {

            set;
            get;
        }

        public DateTime createdTime
        {
            set;
            get;
        }

        public SalaryItemElement salaryItemElement
        {
            set;
            get;
        }

        #endregion Model 
    }
}
