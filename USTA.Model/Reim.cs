using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class Reim
    {
        #region
        public Reim() { }
        #endregion

        #region
        public int id
        {
            set;
            get;
        }
        public string name
        {
            set;
            get;
        }
        public string comment
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
