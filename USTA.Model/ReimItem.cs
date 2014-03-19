using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class ReimItem
    {
        public ReimItem()
        {
        }

        #region
        public int id
        {
            set;
            get;
        }
        public Project project
        {
            set;
            get;
        }
        public Reim reim
        {
            set;
            get;
        }
        public float value
        {
            set;
            get;
        }
        public string memo
        {
            set;
            get;
        }
        public DateTime createdTime
        {
            set;
            get;
        }
        #endregion
    }
}
