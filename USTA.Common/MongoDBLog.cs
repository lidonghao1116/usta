using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

using MongoDB;
using System.Configuration;
using System.Collections;

namespace USTA.Common
{
    /// <summary>
    /// MongoDB日志记录类，用于整个网站程序的日志记录
    /// </summary>
    public sealed class MongoDBLog
    {
        #region 构造函数，用于初始化
        /// <summary>
        ///Log4net的构造函数
        /// </summary>
        public MongoDBLog()
        {

        }
        #endregion

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ex">出错的信息</param>
        public static void LogRecord(Exception ex)
        {
            Mongo mongoDBLog = null;
            try
            {
                mongoDBLog = new Mongo(ConfigurationManager.AppSettings["mongoDBConfig"]);
                mongoDBLog.Connect();
                var dbLog = mongoDBLog.GetDatabase("USTALogs");

                var collection = dbLog.GetCollection<USTALogs>(DateTime.Now.Date.ToString("yyyy-MM-dd"));

                USTALogs ustaLogs = new USTALogs
                {
                    errorTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    errorURL = HttpContext.Current.Request.Url.ToString(),
                    accessIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                    errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(ex).ToString(),
                    errorObject = ex.Source,
                    errorStackTrace = ex.StackTrace,
                    errorMessage = ex.Message,
                    errorHelpLink = ex.HelpLink,
                    errorMethod = (ex.TargetSite == null ? string.Empty : ex.TargetSite.ToString())
                };

                collection.Insert(ustaLogs);
            }
            catch (Exception mongoDBLogException)
            {
                string mongoDBInfoError = HttpContext.Current.Server.MapPath("/LogFiles/WriteMongoDBInfoError_" + DateTime.Now.Date.ToString("yyyy-MM-dd"));

                if (!System.IO.File.Exists(mongoDBInfoError))
                {
                    System.IO.File.Create(mongoDBInfoError);
                }
            }
            finally
            {
                if (mongoDBLog != null)
                {
                    mongoDBLog.Disconnect();
                    mongoDBLog.Dispose();
                }
            }


            ////错误信息
            //string exceptionMessage = "\n出错页面地址为：" + HttpContext.Current.Request.Url.ToString() + "\n\n访问者IP为：" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + "\n" + "\n异常码为：" + System.Runtime.InteropServices.Marshal.GetHRForException(ex) + "\n出错对象为：" + ex.Source + "\n堆栈信息为：" + ex.StackTrace + "\n出错函数为：" + ex.TargetSite + "\n出错信息为：" + ex.Message + "\n参考帮助链接为：" + ex.HelpLink + "\n\n";
            ////记录错误日志
            //log.Error(exceptionMessage);
        }

        /// <summary>
        /// 按日期分页显示日志
        /// </summary>
        /// <param name="date"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public static ResultSet ShowLogs(string date, int pageSize, int pageNum)
        {
            long resultCount = 0;

            IList docResultSet = new List<USTALogs>();

            Mongo mongoDBLog = null;

            try
            {
                mongoDBLog = new Mongo(ConfigurationManager.AppSettings["mongoDBConfig"]);
                mongoDBLog.Connect();

                var dbLog = mongoDBLog.GetDatabase("USTALogs");

                var collection = dbLog.GetCollection<USTALogs>(date);
                resultCount = collection.Count();

                var queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                     orderby p.errorTime descending
                                     select p;
                docResultSet = queryResultSet.ToList<USTALogs>();

            }
            catch (Exception mongoDBLogException)
            {
                string mongoDBInfoError = HttpContext.Current.Server.MapPath("/LogFiles/GetMongoDBInfoError_" + DateTime.Now.Date.ToString("yyyy-MM-dd"));

                if (!System.IO.File.Exists(mongoDBInfoError))
                {
                    System.IO.File.Create(mongoDBInfoError);
                }
            }
            finally
            {
                if (mongoDBLog != null)
                {
                    mongoDBLog.Disconnect();
                    mongoDBLog.Dispose();
                }
            }


            return new ResultSet { resultCount = resultCount, result = docResultSet };
        }

    }

    #region 查询结果集实体类
    /// <summary>
    /// 查询结果集实体类
    /// </summary>
    public class ResultSet
    {
        public ResultSet()
        {

        }

        public long resultCount
        {
            get;
            set;
        }

        public IList result
        {
            get;
            set;
        }
    }
    #endregion

    #region 日志实体类
    /// <summary>
    /// 日志实体类
    /// </summary>
    public class USTALogs
    {

        public USTALogs()
        {


        }

        public string errorURL
        {
            get;
            set;
        }

        public string accessIP
        {
            get;
            set;
        }

        public string errorCode
        {
            get;
            set;
        }

        public string errorObject
        {
            get;
            set;
        }

        public string errorStackTrace
        {
            get;
            set;
        }

        public string errorMethod
        {
            get;
            set;
        }

        public string errorMessage
        {
            get;
            set;
        }

        public string errorHelpLink
        {
            get;
            set;
        }

        public string errorTime
        {
            get;
            set;
        }
    }
    #endregion
}