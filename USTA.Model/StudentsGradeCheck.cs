using System;
namespace USTA.Model
{
    /// <summary>
    /// usta_StudentsGradeCheck:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class StudentsGradeCheck
    {
        public StudentsGradeCheck()
        { }
        #region Model
        private int _gradecheckid;
        private string _gradecheckitemname;
        private string _gradecheckitemdefaultvalue = "无";
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
        public string gradeCheckItemName
        {
            set { _gradecheckitemname = value; }
            get { return _gradecheckitemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gradeCheckItemDefaultValue
        {
            set { _gradecheckitemdefaultvalue = value; }
            get { return _gradecheckitemdefaultvalue; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int displayOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string termYear
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string termYears
        {
            set;
            get;
        }
        #endregion Model

    }
}

