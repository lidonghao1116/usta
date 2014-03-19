using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class SalaryItemElement
    {
        public SalaryItemElement()
        { }
        #region Model
        public string salaryItemId
        {
            set;
            get;
        }

        public string salaryItemName
        {
            set;
            get;
        }

        public bool hasTax
        {
            set;
            get;
        }

        public float salaryStandard
        {
            set;
            get;
        }

        public float times
        {
            set;
            get;
        }

        public int MonthNum
        {
            set;
            get;
        }

        public string itemUnit
        {
            set;
            get;
        }

        public float adjustFactor
        {
            set;
            get;
        }

        public float itemCost
        {
            set;
            get;
        }

        #endregion Model
    }
}
