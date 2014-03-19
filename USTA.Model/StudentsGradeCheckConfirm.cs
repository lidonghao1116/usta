using System;
namespace USTA.Model
{
    /// <summary>
    /// usta_StudentsGradeCheckConfirm:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class StudentsGradeCheckConfirm
    {
        public StudentsGradeCheckConfirm()
        { }
        #region Model
        private int _gradecheckconfirmid;
        private string _studentno;
        private DateTime _updatetime;
        private int _isaccord;
        /// <summary>
        /// 
        /// </summary>
        public int gradeCheckConfirmId
        {
            set { _gradecheckconfirmid = value; }
            get { return _gradecheckconfirmid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string studentNo
        {
            set { _studentno = value; }
            get { return _studentno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime updateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int isAccord
        {
            set { _isaccord = value; }
            get { return _isaccord; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string remark
        {
            get;
            set;
        }
        
        #endregion Model

    }
}

