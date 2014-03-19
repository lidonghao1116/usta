using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class FeedBack
    {
        public FeedBack()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int feedBackId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string feedBackTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string feedBackContent
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string feedBackContactTo
        {
            set;
            get;
        }
        public bool isRead
        {
            set;
            get;
        }
        public DateTime updateTime
        {
            set;
            get;
        }
        public string backInfo
        {
            set;
            get;
        }
        public DateTime backTime
        {
            set;
            get;
        }
        public string backUserNo
        {
            set;
            get;
        }
        public int backUserType
        {
            set;
            get;
        }
        public int type
        {
            set;
            get;
        }
        public string resolver
        {
            set;
            get;
        }
        #endregion Model
    }
}
