using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class SchoolWorkNotify
    {
        public SchoolWorkNotify()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int schoolWorkNotifyId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string schoolWorkNotifyTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string schoolWorkNotifyContent
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
        /// <summary>
        /// 
        /// </summary>
        public DateTime deadline
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
        public bool isOnline
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
