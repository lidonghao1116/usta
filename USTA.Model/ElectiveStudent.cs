using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class ElectiveStudent
    {
        public ElectiveStudent()
        { }
        //[ID],[CourseID],[StudentNo],[Year],[ClassID]
        /// <summary>
        /// 
        /// </summary>
        public string ID
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string CourseID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string studentNo
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Year
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClassID
        {
            set;
            get;
        }
    }
}