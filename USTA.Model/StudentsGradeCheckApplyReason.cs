using System;
namespace USTA.Model
{
    /// <summary>
    /// usta_StudentsGradeCheckApplyReason:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class StudentsGradeCheckApplyReason
    {
        public StudentsGradeCheckApplyReason()
        { }
        #region Model
        private int _gradecheckapplyreasonid;
        private string _gradecheckapplyreasontitle;
        private string _gradecheckapplyreasonremark;
        /// <summary>
        /// 
        /// </summary>
        public int gradeCheckApplyReasonId
        {
            set { _gradecheckapplyreasonid = value; }
            get { return _gradecheckapplyreasonid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gradeCheckApplyReasonTitle
        {
            set { _gradecheckapplyreasontitle = value; }
            get { return _gradecheckapplyreasontitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gradeCheckApplyReasonRemark
        {
            set { _gradecheckapplyreasonremark = value; }
            get { return _gradecheckapplyreasonremark; }
        }
        #endregion Model

    }
}

