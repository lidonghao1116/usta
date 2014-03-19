using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// usta_EnglishExam:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class EnglishExam
    {
        public EnglishExam()
        { }
        #region Model
        private int _englishexamid;
        private string _examcertificatestate;
        private string _examcertificateremark;
        private int? _ispaid = 0;
        private string _studentno;
        private string _examplace;
        private string _examtype;
        private string _grade;
        private string _gradecertificatestate;
        private string _gradecertificateremark;
        private int _englishexamnotifyid;
        /// <summary>
        /// 
        /// </summary>
        public int englishExamId
        {
            set { _englishexamid = value; }
            get { return _englishexamid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string examCertificateState
        {
            set { _examcertificatestate = value; }
            get { return _examcertificatestate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string examCertificateRemark
        {
            set { _examcertificateremark = value; }
            get { return _examcertificateremark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isPaid
        {
            set { _ispaid = value; }
            get { return _ispaid; }
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
        public string examPlace
        {
            set { _examplace = value; }
            get { return _examplace; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string examType
        {
            set { _examtype = value; }
            get { return _examtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gradeCertificateState
        {
            set { _gradecertificatestate = value; }
            get { return _gradecertificatestate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gradeCertificateRemark
        {
            set { _gradecertificateremark = value; }
            get { return _gradecertificateremark; }
        }
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
        public int englishExamSignUpConfirm
        {
            get;
            set;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public DateTime englishExamSignUpConfirmTime
        //{
        //    get;
        //    set;
        //}
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
        public string isPaidRemark
        {
            get;
            set;
        }
        #endregion Model
    }
}
