using System;
namespace USTA.Model
{
    /// <summary>
    /// usta_ArchivesItems:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ArchivesItems
    {
        public ArchivesItems()
        { }
        #region Model
        private int _archiveitemid;
        private string _archiveitemname;
        private string _remark;
        private string _teachertype;
        /// <summary>
        /// 
        /// </summary>
        public int archiveItemId
        {
            set { _archiveitemid = value; }
            get { return _archiveitemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string archiveItemName
        {
            set { _archiveitemname = value; }
            get { return _archiveitemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string teacherType
        {
            set { _teachertype = value; }
            get { return _teachertype; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string termTag
        {
            set;
            get;
        }
        #endregion Model

    }
}

