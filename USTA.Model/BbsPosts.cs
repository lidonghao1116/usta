using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class BbsPosts
    {
        public BbsPosts()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int postId
        {
            set;
            get;
        }
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
        public string postContent
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string postUserName
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
        public string postUserNo
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int postUserType
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
        #endregion Model

    }
}
