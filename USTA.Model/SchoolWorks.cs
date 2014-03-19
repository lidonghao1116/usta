using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class SchoolWorks
    {
        public SchoolWorks()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int schoolWorkId
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
        public int schoolWorkNofityId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string studentNo
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool isCheck
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime checkTime
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string attachmentId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string score
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string remark
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? excellentTime
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool isExcellent
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool isSubmit
        {
            set;
            get;
        }
        #endregion Model

    }
}
