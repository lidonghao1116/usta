using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class SendingEmailList
    {
        public SendingEmailList()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int sendingEmailListId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string emailTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string emailContent
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string emailAttachmentIds
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
        public bool isSendSuccess
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string userName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string emailAddress
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string sender
        {
            set;
            get;
        }
        public int sendType
        {
            set;
            get;
        }
        #endregion Model

    }
}
