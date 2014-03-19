using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class CourseResources
    {
        public CourseResources()
        {
        }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int courseResourceId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string courseResourceTitle
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
        public DateTime updateTime
        {
            set;
            get;
        }
        public string classID
        {
            set;
            get;
        }
        public string termTag
        {
            set;
            get;
        }
        #endregion Model

    }
}
