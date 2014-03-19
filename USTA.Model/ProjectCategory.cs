using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class ProjectCategory
    {
        public ProjectCategory()
        {
        }
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
        public int parentId
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

        public int categoryLevel
        {
            set;
            get;
        }
        #endregion
    }
}
