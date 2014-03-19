using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class CoursesStudentsCorrelation
    {
        public CoursesStudentsCorrelation()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int coursesStudentsCorrelationId
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
        public string courseNo
        {
            set;
            get;
        }
        #endregion Model
    }
}
