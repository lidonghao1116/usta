using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class CoursesTeachersCorrelation
    {
        public CoursesTeachersCorrelation()
        { }
        #region Model

        /// <summary>
        /// 
        /// </summary>
        public int coursesTeachersCorrelationId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string teacherNo
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

        /// <summary>
        /// 
        /// </summary>
        public int atCourseType
        {
            set;
            get;
        }
        #endregion Model
    }
}
