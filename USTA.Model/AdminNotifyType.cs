using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class AdminNotifyType
    {
        public AdminNotifyType()
	    { }

		/// <summary>
		/// 
		/// </summary>
        public int notifyTypeId
		{
            set;
            get;
		}
		/// <summary>
		/// 
		/// </summary>
        public string notifyTypeName
		{
            set;
            get;
		}

        /// <summary>
        /// 
        /// </summary>
        public int parentId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int sequence
        {
            set;
            get;
        }
    }
}
