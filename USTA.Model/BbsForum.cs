using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class BbsForum
    {
        public BbsForum()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int forumId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string forumTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime lastUpdateTime
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string lastTopicTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? lastTopicId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string bbsEmaiAddress
        {
            set;
            get;
        }
        public string userNo
        {
            set;
            get;
        }
        public int userType
        {
            set;
            get;
        }
        public int forumType
        {
            set;
            get;
        }
        #endregion Model

    }
}
