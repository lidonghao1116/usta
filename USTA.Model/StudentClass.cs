using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentClass
    {
        /// <summary>
        /// 
        /// </summary>
        public int classId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string className
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int special
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string remark
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string locale
        {
            set;
            get;
        }
    }
}