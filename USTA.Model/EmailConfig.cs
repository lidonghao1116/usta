using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class EmailConfig
    {
        public EmailConfig()
        { }
        #region Model
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
        public string emailPassword
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string emailServerAddress
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int emailServerPort
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
        #endregion Model

    }
}
