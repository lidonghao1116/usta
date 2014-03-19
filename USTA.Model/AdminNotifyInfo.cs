using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace USTA.Model
{
    public class AdminNotifyInfo
    {
        public AdminNotifyInfo()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public string adminNotifyInfoIds
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
        public int notifyTypeId
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
        public string attachmentIds
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int isTop
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int scanCount
        {
            set;
            get;
        }
    }
}
