using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentsGradeCheckConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public StudentsGradeCheckConfig()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public int gradeCheckConfigId
        {
            set;
            get;
        }


        /// <summary>
        /// 
        /// </summary>
        public string notifyTitle
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string notifyContent
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime startTime
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime endTime
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string attachmentIds
        {
            set;
            get;
        }
    }
}
