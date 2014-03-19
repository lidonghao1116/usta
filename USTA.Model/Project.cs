using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class Project
    {
        public Project()
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
        public ProjectCategory category
        {
            set;
            get;
        }
        public string userName
        {
            set;
            get;
        }
        public string userNo
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
