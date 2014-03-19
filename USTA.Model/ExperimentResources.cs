using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class ExperimentResources
    {
        public ExperimentResources()
        {
        }
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public int experimentResourceId
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
        public string attachmentIds
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string experimentResourceTitle
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string experimentResourceContent
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime deadLine
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
        public string classID
        {
            set;
            get;
            
        }
        public string termTag
        {
            set;
            get;
        }
        #endregion Model

    }
}
