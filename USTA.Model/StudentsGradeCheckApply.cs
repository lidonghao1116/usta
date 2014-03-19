using System;
namespace USTA.Model
{
    /// <summary>
    /// usta_StudentsGradeCheckApply:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class StudentsGradeCheckApply
    {
        public StudentsGradeCheckApply()
        { }
        #region Model
        private int _gradecheckapplyid;
        private string _studentno;
        private DateTime _updatetime;
        private string _courseno;
        private string _classid;
        private string _termtag;
        private string _gradecheckapplytype;
        /// <summary>
        /// 
        /// </summary>
        public int gradeCheckApplyId
        {
            set { _gradecheckapplyid = value; }
            get { return _gradecheckapplyid; }
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
        public string courseNo
        {
            set { _courseno = value; }
            get { return _courseno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClassID
        {
            set { _classid = value; }
            get { return _classid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string termTag
        {
            set { _termtag = value; }
            get { return _termtag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gradeCheckApplyType
        {
            set { _gradecheckapplytype = value; }
            get { return _gradecheckapplytype; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string applyResult
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string applyChecKSuggestion
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string applyReason
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime applyUpdateTime
        {
            get;
            set;
        }

        #endregion Model

    }
}

