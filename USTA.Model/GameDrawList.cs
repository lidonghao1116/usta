using System;

namespace USTA.Model
{
    /// <summary>
    /// usta_GameDrawList:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class GameDrawList
    {
        public GameDrawList()
        { }
        #region Model
        private int _gamedrawlistid;
        private string _teacherno;
        private int _gamecategoryid;
        private int _gametypeid;
        private string _groupnum;
        private int? _groupindex;
        /// <summary>
        /// 
        /// </summary>
        public int gameDrawListId
        {
            set { _gamedrawlistid = value; }
            get { return _gamedrawlistid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string teacherNo
        {
            set { _teacherno = value; }
            get { return _teacherno; }
        }
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
        public int gameTypeId
        {
            set { _gametypeid = value; }
            get { return _gametypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string groupNum
        {
            set { _groupnum = value; }
            get { return _groupnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? groupIndex
        {
            set { _groupindex = value; }
            get { return _groupindex; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime updateTime
        {
            set;
            get;
        }
        #endregion Model

    }
}

