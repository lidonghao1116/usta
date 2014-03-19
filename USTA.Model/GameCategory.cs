using System;

namespace USTA.Model
{
    /// <summary>
    /// usta_GameCategory:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class GameCategory
    {
        public GameCategory()
        { }
        #region Model
        private int _gamecategoryid;
        private string _gametitle;
        private string _gamecontent;
        private string _attachmentids;
        private DateTime? _updatetime;
        private DateTime? _starttime;
        private DateTime? _endtime;
        /// <summary>
        /// 
        /// </summary>
        public int gameCategoryId
        {
            set { _gamecategoryid = value; }
            get { return _gamecategoryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gameTitle
        {
            set { _gametitle = value; }
            get { return _gametitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string gameContent
        {
            set { _gamecontent = value; }
            get { return _gamecontent; }
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
        public DateTime? updateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime startTime
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime endTime
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int isOpenDraw
        {
            set;
            get;
        }
        #endregion Model

    }
}

