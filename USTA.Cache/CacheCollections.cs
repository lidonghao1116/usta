using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace USTA.Cache
{
    using USTA.Model;
    using USTA.Bll;
    using USTA.Dal;

    /// <summary>
    /// Cache存取类，用于提升系统性能，密封类，不能继承
    /// </summary>
    public sealed class CacheCollections
    {
        #region 全局变量及构造函数

        private static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;
        
        /// <summary>
        /// Cache存取类构造函数
        /// </summary>
        public CacheCollections()
        {
        }
        #endregion


        #region 获取基本设置Cache
        /// <summary>
        /// 获取系统基本设置Cache，如果为空则重新生成
        /// </summary>
        /// <returns>系统基本设置实体类</returns>
        public static BaseConfig GetBaseConfig()
        {
            if (webCache["baseConfig"] == null)
            {
                DalOperationBaseConfig dobc = new DalOperationBaseConfig();
                BaseConfig baseconfig = dobc.FindBaseConfig();

                webCache.Insert("baseConfig", baseconfig);

                return baseconfig;
            }
            else
            {
                return (BaseConfig)webCache["baseConfig"];
            }
        }
        #endregion

        #region 清理Cache
        /// <summary>
        /// 移除对应关键字的缓存
        /// </summary>
        /// <param name="key">缓存关键字</param>
        public static void ClearCache(string key)
        {
            webCache.Remove(key);
        }
        public static void putCache(string key, Object value)
        {
            webCache.Insert(key, value);
        }       
        
        #endregion
    }
}
