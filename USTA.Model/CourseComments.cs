using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class CourseComments
    {
        public CourseComments()
        {
        }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int courseCommentId
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
        public string courseCommentContent
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string courseCommentUserName
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
        #endregion Model
    }
}
