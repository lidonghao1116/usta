using System;

namespace USTA.Model
{
    /// <summary>
    /// usta_GameType:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class GameType
    {
        public GameType()
        { }
        #region Model
        private int _gametypeid;
        private string _gametypetitle;
        private int? _allowsextype;
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
        public string gameTypeTitle
        {
            set { _gametypetitle = value; }
            get { return _gametypetitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string allowSexType
        {
            set;
            get;
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
        public int groupCapability
        {
            set;
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int gameCategoryId
        {
            set;
            get;
        }
        #endregion Model

    }
}

