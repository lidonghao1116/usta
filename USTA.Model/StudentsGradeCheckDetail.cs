using System;
namespace USTA.Model
{
    /// <summary>
    /// usta_StudentsGradeCheckDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class StudentsGradeCheckDetail
    {
        public StudentsGradeCheckDetail()
        { }
        #region Model
        private int _gradecheckdetailid;
        private int _gradecheckid;
        private string _gradecheckdetailvalue;
        private string _studentno;
        /// <summary>
        /// 
        /// </summary>
        public int gradeCheckDetailId
        {
            set { _gradecheckdetailid = value; }
            get { return _gradecheckdetailid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int gradeCheckId
        {
            set { _gradecheckid = value; }
            get { return _gradecheckid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gradeCheckDetailValue
        {
            set { _gradecheckdetailvalue = value; }
            get { return _gradecheckdetailvalue; }
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
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string gradeCheckItemName
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string gradeCheckItemDefaultValue
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string termYear
        {
            set;
            get;
        }
        #endregion Model

    }
}

