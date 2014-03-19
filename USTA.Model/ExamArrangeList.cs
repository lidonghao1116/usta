using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class ExamArrangeList
    {
        public ExamArrangeList()
        {
        }
        #region model
        /// <summary>
        /// 
        /// </summary>
        public int examArrangeListId
        {
            set;
            get;
        }
        public string courseName
        {
            set;
            get;
        }
        public DateTime examArrangeTime
        {
            set;
            get;
        }
        public string examArrageAddress
        {
            set;
            get;
        }
        public string remark
        {
            set;
            get;
        }
        public string teacherName
        {
            set;
            get;
        }
        public string courseNo
        {
            set;
            get;
        }
        #endregion model
    }
}
