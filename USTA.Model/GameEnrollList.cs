using System;

namespace USTA.Model
{
    /// <summary>
    /// usta_GameEnrollList:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class GameEnrollList
    {
        public GameEnrollList()
        { }
        #region Model
        private int _gameenrolllistid;
        private string _teacherno;
        private int _gamecategoryid;
        private int _gametypeid;
        /// <summary>
        /// 
        /// </summary>
        public int gameEnrollListId
        {
            set { _gameenrolllistid = value; }
            get { return _gameenrolllistid; }
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
        public DateTime updateTime
        {
            set;
            get;
        }
        #endregion Model

    }
}

