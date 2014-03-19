using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class CoursesNotifyInfo
    {
        public CoursesNotifyInfo(){
        }

        /// <summary>
        /// 
        /// </summary>
        public int courseNotifyInfoId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string courseNotifyInfoTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string courseNotifyInfoContent
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
        public string publishUserNo
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int notifyType
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
    }
}
