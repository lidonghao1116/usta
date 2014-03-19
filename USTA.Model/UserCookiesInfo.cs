using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    [Serializable]
    public class UserCookiesInfo
    {
        public UserCookiesInfo()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string userNo
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
        public int userType
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string teacherType
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Sex
        {
            set;
            get;
        }
    }
}
