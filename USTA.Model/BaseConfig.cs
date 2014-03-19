using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class BaseConfig
    {
        public BaseConfig()
        {

        }

        public string systemName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string systemVersion
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string systemCopyRight
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string fileServerAddress
        {
            set;
            get;
        }
    }
}
