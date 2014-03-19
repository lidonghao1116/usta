using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class Attachments
    {
        public Attachments()
        {

        }

        public int? attachmentId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string attachmentTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string attachmentUrl
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
    }
}
