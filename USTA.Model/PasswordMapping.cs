using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class PasswordMapping
    {
        public PasswordMapping()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int passwordMappingId
        {
            set;
            get;
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
        public string initializePassword
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string termTag
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
        #endregion Model
    }
}
