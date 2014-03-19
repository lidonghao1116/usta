using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Archives
    {
        /// <summary>
        /// 
        /// </summary>
        public Archives()
        { }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int archiveId
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
        public string courseNo
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string classID
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
        public int archiveItemId
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
        #endregion Model

    }
}
