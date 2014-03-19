using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class BbsTopics
    {
        public BbsTopics()
        { }
        #region Model

        /// <summary>
        /// 
        /// </summary>
        public int topicId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string topicUserName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string topicTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string topicContent
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int hits
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
        public string topicUserNo
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int topicUserType
        {
            set;
            get;
        }
        public int isbigTop
        {
            set;
            get;
        }
        #endregion Model

    }
}
