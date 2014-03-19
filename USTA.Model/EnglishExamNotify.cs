using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// usta_EnglishExamNotify:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class EnglishExamNotify
    {
        public EnglishExamNotify()
        { }
        #region Model
        private int _englishexamnotifyid;
        private string _englishexamnotifytitle;
        private string _englishexamnotifycontent;
        private string _attachmentids;
        /// <summary>
        /// 
        /// </summary>
        public int englishExamNotifyId
        {
            set { _englishexamnotifyid = value; }
            get { return _englishexamnotifyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string englishExamNotifyTitle
        {
            set { _englishexamnotifytitle = value; }
            get { return _englishexamnotifytitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string englishExamNotifyContent
        {
            set { _englishexamnotifycontent = value; }
            get { return _englishexamnotifycontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string attachmentIds
        {
            set { _attachmentids = value; }
            get { return _attachmentids; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime updateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int hits
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime deadLineTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string locale
        {
            set;
            get;
        }
        #endregion Model

    }
}
