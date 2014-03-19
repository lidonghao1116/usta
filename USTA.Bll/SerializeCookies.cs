using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace USTA.Bll
{
    using USTA.Model;

    /// <summary>
    /// 序列化与反序列化cookies类
    /// </summary>
    public sealed class SerializeCookies
    {
        #region 序列化cookies
        /// <summary>
        /// 序列化cookies
        /// </summary>
        /// <typeparam name="T">泛型形参</typeparam>
        /// <param name="userCookiesInfo">用户Cookies实体类</param>
        /// <returns>返回序列化后的字符串</returns>
        public static string SerializeCookiesMethod<T>(T userCookiesInfo)
        {

            IFormatter bf = new BinaryFormatter();

            string result = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, userCookiesInfo);

                byte[] byt = new byte[ms.Length];

                byt = ms.ToArray();

                result = Convert.ToBase64String(byt);
                ms.Flush();
            }
            return result;
        }
        #endregion

        #region 反序列化cookies
        /// <summary>
        /// 反序列化cookies
        /// </summary>
        /// <param name="cookiesValue">要反序列化的字符串</param>
        /// <returns>返回用户Cookies实体类</returns>
        public static UserCookiesInfo DeSerializeCookiesMethod(string cookiesValue)
        {
            IFormatter bf = new BinaryFormatter();

            UserCookiesInfo UserCookiesInfo;

            byte[] byt = Convert.FromBase64String(cookiesValue);

            using (MemoryStream ms = new MemoryStream(byt, 0, byt.Length))
            {
                UserCookiesInfo = (UserCookiesInfo)bf.Deserialize(ms);
            }
            return UserCookiesInfo;
        }
        #endregion
    }
}
