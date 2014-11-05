using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections;

namespace USTA.Common
{
    using USTA.Model;
    using System.Configuration;
    using System.Text.RegularExpressions;
    using Microsoft.VisualBasic;
    using System.Web.UI.WebControls;
    using System.Xml;

    /// <summary>
    /// 公用函数类
    /// </summary>
    public sealed class CommonUtility
    {
        #region 全局变量及构造函数

        /// <summary>
        /// 分页大小
        /// </summary>
        public static int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);

        /// <summary>
        /// 随机数据类
        /// </summary>
        public static Random rdm = new Random();

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonUtility()
        {

        }

        #endregion

        #region MD5加密
        /// <summary>
        /// 获取要加密的字段，并转化为Byte[]数组 
        /// </summary>
        /// <param name="encodeValue">要加密的字段</param>
        /// <returns>加密后的字段</returns>
        public static string EncodeUsingMD5(string encodeValue)
        {
            ////获取要加密的字段，并转化为Byte[]数组 
            //byte[] data = System.Text.Encoding.Unicode.GetBytes(pwd.ToCharArray());
            ////建立加密服务 
            //System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            ////加密Byte[]数组 
            //byte[] result = md5.ComputeHash(data);
            ////将加密后的数组转化为字段 
            //string sResult = System.Text.Encoding.Unicode.GetString(result);
            //作为密码方式加密 并返回加密后的值
            return FormsAuthentication.HashPasswordForStoringInConfigFile(encodeValue, "MD5").ToLower();
        }
        #endregion

        #region 生成随机密码（4位数字）

        /// <summary>
        /// 声明4位随机的密码
        /// </summary>
        /// <returns>返回1000到9999的一个数作为随机密码</returns>
        public static string GenerateRandomPassword()
        {
            //随机数字类
            return rdm.Next(1000, 9999).ToString();
        }

        #endregion

        #region 检测传入的参数是否可以转换为数字，主要用于一些ID参数为数字的安全检测
        
        /// <summary>
        /// 检测传入的参数是否可以转换为float的安全检测
        /// </summary>
        /// <typeparam name="T">要转换参数的类型</typeparam>
        /// <param name="param">要转换的参数</param>
        /// <param name="tryParseInt">转换后的参数</param>
        /// <returns>转换是否成功</returns>
        public static bool SafeCheckByParams<T>(T param, ref float tryParseInt)
        {
            bool isSafe = false;

            if (param != null)
            {
                isSafe = float.TryParse(param.ToString().Trim(), out tryParseInt);
            }
            return isSafe;
        }

        /// <summary>
        /// 检测传入的参数是否可以转换为int的安全检测
        /// </summary>
        /// <typeparam name="T">要转换参数的类型</typeparam>
        /// <param name="param">要转换的参数</param>
        /// <param name="tryParseInt">转换后的参数</param>
        /// <returns>转换是否成功</returns>
        public static bool SafeCheckByParams<T>(T param, ref int tryParseInt)
        {
            bool isSafe = false;

            if (param != null)
            {
                isSafe = Int32.TryParse(param.ToString().Trim(), out tryParseInt);
            }
            return isSafe;
        }


        /// <summary>
        /// 检测传入的参数是否可以转换为DateTime的安全检测
        /// </summary>
        /// <typeparam name="T">要转换参数的类型</typeparam>
        /// <param name="param">要转换的参数</param>
        /// <param name="tryParseDateTime">转换后的参数</param>
        /// <returns>转换是否成功</returns>
        public static bool SafeCheckByParams<T>(T param, ref DateTime tryParseDateTime)
        {
            bool isSafe = false;

            if (param != null)
            {
                isSafe = DateTime.TryParse(param.ToString().Trim(), out tryParseDateTime);
            }
            return isSafe;
        }


        /// <summary>
        /// 检测传入的参数(2个)是否可以转换为int(2个)的安全检测
        /// </summary>
        /// <typeparam name="T">要转换参数的类型</typeparam>
        /// <param name="paramsOne">要转换的第一个参数</param>
        /// <param name="paramsTwo">要转换的第二个参数</param>
        /// <param name="tryParseIntFid">转换后的第一个参数</param>
        /// <param name="tryParseIntCid">转换后的第二个参数</param>
        /// <returns>转换是否成功</returns>
        public static bool SafeCheckByParams<T>(T paramsOne, T paramsTwo, ref int tryParseIntFid, ref int tryParseIntCid)
        {
            bool isSafe = false;
            if (paramsOne != null && paramsTwo != null)
            {
                isSafe = Int32.TryParse(paramsOne.ToString().Trim(), out tryParseIntFid) && Int32.TryParse(paramsTwo.ToString().Trim(), out tryParseIntCid);
            }
            return isSafe;
        }

        /// <summary>
        ///  检测传入的参数(3个)是否可以转换为int(3个)的安全检测
        /// </summary>
        /// <typeparam name="T">要转换参数的类型</typeparam>
        /// <param name="paramsOne">要转换的第一个参数</param>
        /// <param name="paramsTwo">要转换的第二个参数</param>
        /// <param name="paramsThree">要转换的第三个参数</param>
        /// <param name="tryParseIntFid">转换后的第一个参数</param>
        /// <param name="tryParseIntCid">转换后的第二个参数</param>
        /// <param name="tryParseIntId">转换后的第三个参数</param>
        /// <returns></returns>
        public static bool SafeCheckByParams<T>(T paramsOne, T paramsTwo, T paramsThree, ref int tryParseIntFid, ref int tryParseIntCid, ref int tryParseIntId)
        {
            bool isSafe = false;
            if (paramsOne != null && paramsTwo != null && paramsThree != null)
            {
                isSafe = Int32.TryParse(paramsOne.ToString().Trim(), out tryParseIntFid) && Int32.TryParse(paramsTwo.ToString().Trim(), out tryParseIntCid) && Int32.TryParse(paramsThree.ToString().Trim(), out tryParseIntId);
            }
            return isSafe;
        }
        #endregion

        #region 文件真实类型检查
        //public static bool IsAllowedExtension(FileUpload hifile)
        //{
        //    System.IO.FileStream fs = new System.IO.FileStream(HttpContext.Current.Server.MapPath(hifile.PostedFile.FileName), System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //    System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
        //    string fileclass = "";
        //    byte buffer;
        //    try
        //    {
        //        buffer = r.ReadByte();
        //        fileclass = buffer.ToString();
        //        buffer = r.ReadByte();
        //        fileclass += buffer.ToString();

        //    }
        //    catch
        //    {
        //        HttpContext.Current.Response.Write("<script>alert('您选择的图片文件有类型错误,请重新选择！');history.go(-1);</script>");
        //    }
        //    r.Close();
        //    fs.Close();
        //    //说明255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
        //    if (fileclass == "255216" || fileclass == "7173" || fileclass == "6677" || fileclass == "13780")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        #endregion


        #region 转向Url

        /// <summary>
        /// 跳转到出错页面
        /// </summary>
        public static void RedirectUrl()
        {
            HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["errorPage"], false);
        }

        #endregion


        #region 跳转到信息平台登录页

        /// <summary>
        /// 跳转到信息平台登录页
        /// </summary>
        public static void RedirectLoginUrl()
        {
            HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["sseweb"], true);
        }

        #endregion

        #region 控制Tab页面标签的显示

        /// <summary>
        /// 控制Tab页面标签的显示(2个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2
           , HtmlControl divControl1, HtmlControl divControl2)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(3个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
            , HtmlControl divControl1, HtmlControl divControl2, HtmlControl divControl3)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(4个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
            , HtmlControl liControl4, HtmlControl divControl1, HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(5个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="liControl5">liControl5</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        /// <param name="divControl5">divControl5</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3, HtmlControl liControl4, HtmlControl liControl5
            , HtmlControl divControl1, HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4, HtmlControl divControl5)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                case "5":
                    liControl5.Attributes.Add("class", "ui-tabs-selected");
                    divControl5.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(7个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="liControl5">liControl5</param>
        /// <param name="liControl6">liControl6</param>
        /// <param name="liControl7">liControl7</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        /// <param name="divControl5">divControl5</param>
        /// <param name="divControl6">divControl6</param>
        /// <param name="divControl7">divControl7</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
         , HtmlControl liControl4, HtmlControl liControl5, HtmlControl liControl6, HtmlControl liControl7, HtmlControl divControl1,
            HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4, HtmlControl divControl5, HtmlControl divControl6, HtmlControl divControl7)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                case "5":
                    liControl5.Attributes.Add("class", "ui-tabs-selected");
                    divControl5.Style.Add("display", "block");
                    break;
                case "6":
                    liControl6.Attributes.Add("class", "ui-tabs-selected");
                    divControl6.Style.Add("display", "block");
                    break;
                case "7":
                    liControl7.Attributes.Add("class", "ui-tabs-selected");
                    divControl7.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(8个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="liControl5">liControl5</param>
        /// <param name="liControl6">liControl6</param>
        /// <param name="liControl7">liControl7</param>
        /// <param name="liControl8">liControl8</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        /// <param name="divControl5">divControl5</param>
        /// <param name="divControl6">divControl6</param>
        /// <param name="divControl7">divControl7</param>
        /// <param name="divControl8">divControl8</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
         , HtmlControl liControl4, HtmlControl liControl5, HtmlControl liControl6, HtmlControl liControl7, HtmlControl liControl8,
            HtmlControl divControl1, HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4, HtmlControl divControl5,
            HtmlControl divControl6, HtmlControl divControl7, HtmlControl divControl8)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                case "5":
                    liControl5.Attributes.Add("class", "ui-tabs-selected");
                    divControl5.Style.Add("display", "block");
                    break;
                case "6":
                    liControl6.Attributes.Add("class", "ui-tabs-selected");
                    divControl6.Style.Add("display", "block");
                    break;
                case "7":
                    liControl7.Attributes.Add("class", "ui-tabs-selected");
                    divControl7.Style.Add("display", "block");
                    break;
                case "8":
                    liControl8.Attributes.Add("class", "ui-tabs-selected");
                    divControl8.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(6个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="liControl5">liControl5</param>
        /// <param name="liControl6">liControl6</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        /// <param name="divControl5">divControl5</param>
        /// <param name="divControl6">divControl6</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
         , HtmlControl liControl4, HtmlControl liControl5, HtmlControl liControl6, HtmlControl divControl1,
            HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4, HtmlControl divControl5, HtmlControl divControl6)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                case "5":
                    liControl5.Attributes.Add("class", "ui-tabs-selected");
                    divControl5.Style.Add("display", "block");
                    break;
                case "6":
                    liControl6.Attributes.Add("class", "ui-tabs-selected");
                    divControl6.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(9个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="liControl5">liControl5</param>
        /// <param name="liControl6">liControl6</param>
        /// <param name="liControl7">liControl7</param>
        /// <param name="liControl8">liControl8</param>
        /// <param name="liControl9">liControl9</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        /// <param name="divControl5">divControl5</param>
        /// <param name="divControl6">divControl6</param>
        /// <param name="divControl7">divControl7</param>
        /// <param name="divControl8">divControl8</param>
        /// <param name="divControl9">divControl9</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
         , HtmlControl liControl4, HtmlControl liControl5, HtmlControl liControl6, HtmlControl liControl7, HtmlControl liControl8, HtmlControl liControl9,
            HtmlControl divControl1, HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4,
            HtmlControl divControl5, HtmlControl divControl6, HtmlControl divControl7, HtmlControl divControl8, HtmlControl divControl9)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                case "5":
                    liControl5.Attributes.Add("class", "ui-tabs-selected");
                    divControl5.Style.Add("display", "block");
                    break;
                case "6":
                    liControl6.Attributes.Add("class", "ui-tabs-selected");
                    divControl6.Style.Add("display", "block");
                    break;
                case "7":
                    liControl7.Attributes.Add("class", "ui-tabs-selected");
                    divControl7.Style.Add("display", "block");
                    break;
                case "8":
                    liControl8.Attributes.Add("class", "ui-tabs-selected");
                    divControl8.Style.Add("display", "block");
                    break;
                case "9":
                    liControl9.Attributes.Add("class", "ui-tabs-selected");
                    divControl9.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }



        /// <summary>
        /// 控制Tab页面标签的显示(11个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="liControl5">liControl5</param>
        /// <param name="liControl6">liControl6</param>
        /// <param name="liControl7">liControl7</param>
        /// <param name="liControl8">liControl8</param>
        /// <param name="liControl9">liControl9</param>
        /// <param name="liControl10">liControl10</param>
        /// <param name="liControl11">liControl11</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        /// <param name="divControl5">divControl5</param>
        /// <param name="divControl6">divControl6</param>
        /// <param name="divControl7">divControl7</param>
        /// <param name="divControl8">divControl8</param>
        /// <param name="divControl9">divControl9</param>
        /// <param name="divControl10">divControl10</param>
        /// <param name="divControl11">divControl11</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
         , HtmlControl liControl4, HtmlControl liControl5, HtmlControl liControl6, HtmlControl liControl7, HtmlControl liControl8,
            HtmlControl liControl9, HtmlControl liControl10,
            HtmlControl divControl1, HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4,
            HtmlControl divControl5, HtmlControl divControl6, HtmlControl divControl7, HtmlControl divControl8,
            HtmlControl divControl9, HtmlControl divControl10)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                case "5":
                    liControl5.Attributes.Add("class", "ui-tabs-selected");
                    divControl5.Style.Add("display", "block");
                    break;
                case "6":
                    liControl6.Attributes.Add("class", "ui-tabs-selected");
                    divControl6.Style.Add("display", "block");
                    break;
                case "7":
                    liControl7.Attributes.Add("class", "ui-tabs-selected");
                    divControl7.Style.Add("display", "block");
                    break;
                case "8":
                    liControl8.Attributes.Add("class", "ui-tabs-selected");
                    divControl8.Style.Add("display", "block");
                    break;
                case "9":
                    liControl9.Attributes.Add("class", "ui-tabs-selected");
                    divControl9.Style.Add("display", "block");
                    break;
                case "10":
                    liControl10.Attributes.Add("class", "ui-tabs-selected");
                    divControl10.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }

        /// <summary>
        /// 控制Tab页面标签的显示(11个标签)
        /// </summary>
        /// <param name="fragmentFlag">标签号</param>
        /// <param name="liControl1">liControl1</param>
        /// <param name="liControl2">liControl2</param>
        /// <param name="liControl3">liControl3</param>
        /// <param name="liControl4">liControl4</param>
        /// <param name="liControl5">liControl5</param>
        /// <param name="liControl6">liControl6</param>
        /// <param name="liControl7">liControl7</param>
        /// <param name="liControl8">liControl8</param>
        /// <param name="liControl9">liControl9</param>
        /// <param name="liControl10">liControl10</param>
        /// <param name="liControl11">liControl11</param>
        /// <param name="divControl1">divControl1</param>
        /// <param name="divControl2">divControl2</param>
        /// <param name="divControl3">divControl3</param>
        /// <param name="divControl4">divControl4</param>
        /// <param name="divControl5">divControl5</param>
        /// <param name="divControl6">divControl6</param>
        /// <param name="divControl7">divControl7</param>
        /// <param name="divControl8">divControl8</param>
        /// <param name="divControl9">divControl9</param>
        /// <param name="divControl10">divControl10</param>
        /// <param name="divControl11">divControl11</param>
        public static void ShowLiControl(string fragmentFlag, HtmlControl liControl1, HtmlControl liControl2, HtmlControl liControl3
         , HtmlControl liControl4, HtmlControl liControl5, HtmlControl liControl6, HtmlControl liControl7, HtmlControl liControl8,
            HtmlControl liControl9, HtmlControl liControl10, HtmlControl liControl11,
            HtmlControl divControl1, HtmlControl divControl2, HtmlControl divControl3, HtmlControl divControl4,
            HtmlControl divControl5, HtmlControl divControl6, HtmlControl divControl7, HtmlControl divControl8,
            HtmlControl divControl9, HtmlControl divControl10, HtmlControl divControl11)
        {
            switch (fragmentFlag)
            {
                case "1":
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
                case "2":
                    liControl2.Attributes.Add("class", "ui-tabs-selected");
                    divControl2.Style.Add("display", "block");
                    break;
                case "3":
                    liControl3.Attributes.Add("class", "ui-tabs-selected");
                    divControl3.Style.Add("display", "block");
                    break;
                case "4":
                    liControl4.Attributes.Add("class", "ui-tabs-selected");
                    divControl4.Style.Add("display", "block");
                    break;
                case "5":
                    liControl5.Attributes.Add("class", "ui-tabs-selected");
                    divControl5.Style.Add("display", "block");
                    break;
                case "6":
                    liControl6.Attributes.Add("class", "ui-tabs-selected");
                    divControl6.Style.Add("display", "block");
                    break;
                case "7":
                    liControl7.Attributes.Add("class", "ui-tabs-selected");
                    divControl7.Style.Add("display", "block");
                    break;
                case "8":
                    liControl8.Attributes.Add("class", "ui-tabs-selected");
                    divControl8.Style.Add("display", "block");
                    break;
                case "9":
                    liControl9.Attributes.Add("class", "ui-tabs-selected");
                    divControl9.Style.Add("display", "block");
                    break;
                case "10":
                    liControl10.Attributes.Add("class", "ui-tabs-selected");
                    divControl10.Style.Add("display", "block");
                    break;
                case "11":
                    liControl11.Attributes.Add("class", "ui-tabs-selected");
                    divControl11.Style.Add("display", "block");
                    break;
                default:
                    liControl1.Attributes.Add("class", "ui-tabs-selected");
                    divControl1.Style.Add("display", "block");
                    break;
            }
        }
        #endregion

        #region 过滤js字符串
        /// <summary>
        /// 过滤JS字符串
        /// </summary>
        /// <param name="filteringString">要过滤得字符</param>
        /// <returns>过滤后的字符串</returns>
        public static string JavascriptStringFilter(string filteringString)
        {
            return filteringString.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// 过滤整个JS部分字符串
        /// </summary>
        /// <param name="filteringString">要过滤得字符</param>
        /// <returns>过滤后的字符</returns>
        public static string JavascriptStringFilterAll(string filteringString)
        {
            filteringString = Regex.Replace(filteringString, "<script", "&lt;script", RegexOptions.IgnoreCase);
            filteringString = Regex.Replace(filteringString, "/script>", "/script&gt;", RegexOptions.IgnoreCase);
            return filteringString;
        }
        #endregion

        #region 检测作业分数是否在0到100的范围内
        /// <summary>
        /// 检测作业分数是否在0到100的范围内
        /// </summary>
        /// <param name="score">要检测的分数</param>
        /// <returns>检测是否成功</returns>
        public static bool CheckScoreScope(ref float score)
        {
            bool isInScope = false;
            if (score >= 0 && score <= 100)
            {
                score = (float)Math.Round(score, 1);
                isInScope = true;
            }

            return isInScope;
        }
        #endregion

        #region 获取新闻显示为NEW的天数
        /// <summary>
        ///  获取新闻显示为NEW的天数
        /// </summary>
        /// <returns>返回新闻显示为NEW的天数</returns>
        public static int GetNewDays()
        {
            return int.Parse(ConfigurationManager.AppSettings["newDays"]);
        }

        /// <summary>
        /// 获得系统email使用者
        /// </summary>
        /// <returns>返回系统email使用者</returns>
        public static string GetSysMailAuthor()
        {
            return ConfigurationManager.AppSettings["mailauthor"];
        }

        /// <summary>
        /// 检测参数的短日期是否与当前短日期相等
        /// </summary>
        /// <param name="checkString"></param>
        /// <returns></returns>
        public static bool isInToday(string checkString)
        {
            string todayDate = DateTime.Now.ToShortDateString();
            DateTime  checkDate;
            bool inToday = false;

            if(DateTime.TryParse(checkString,out checkDate))
            {

                inToday = (todayDate == checkDate.ToShortDateString());
            }
            return inToday;
        }
        #endregion

        #region 动态添加JS和CSS

        /// <summary>
        /// 动态添加JS和CSS
        /// </summary>
        /// <param name="head">HTML的HEAD部分</param>
        /// <param name="cssAndJsPath">CSS和JS的路径信息</param>
        public static void AddCssAndJs(HtmlHead head, string[] cssAndJsPath)
        {
            if (cssAndJsPath.Length < 4)
            {
                return;
            }

            HtmlGenericControl scriptControl = new HtmlGenericControl("link");
            head.Controls.Add(scriptControl);
            scriptControl.Attributes["type"] = "text/css";
            scriptControl.Attributes["rel"] = "Stylesheet";
            scriptControl.Attributes["href"] = cssAndJsPath[0];

            HtmlGenericControl scriptControl1 = new HtmlGenericControl("script");
            head.Controls.Add(scriptControl1);
            scriptControl1.Attributes["type"] = "text/javascript";
            scriptControl1.Attributes["src"] = cssAndJsPath[1];

            HtmlGenericControl scriptControl2 = new HtmlGenericControl("script");
            head.Controls.Add(scriptControl2);
            scriptControl2.Attributes["type"] = "text/javascript";
            scriptControl2.Attributes["src"] = cssAndJsPath[2];

            HtmlGenericControl scriptControl3 = new HtmlGenericControl("script");
            head.Controls.Add(scriptControl3);
            scriptControl3.Attributes["type"] = "text/javascript";
            scriptControl3.Attributes["src"] = cssAndJsPath[3];
        }
        #endregion

        #region 计算扣除三险一金后应缴的个人所得税
        /// <summary>
        /// 计算应缴个人所得税，参考个税税率表http://114.xixik.com/table-of-rates/
        /// 级数	含税级距	不含税级距	税率(%)	速算扣除数	说明
        ///1	不超过1500元的	不超过1455元的	3	0	
        ///1、本表含税级距指以每月收入额减除费用三千五百元后的余额或者减除附加减除费用后的余额。

        ///2、含税级距适用于由纳税人负担税款的工资、薪金所得；不含税级距适用于由他人(单位)代付税款的工资、薪金所得。

        ///2	超过1500元至4,500元的部分	超过1455元至4,155元的部分	10	105
        ///3	超过4,500元至9,000元的部分	超过4,155元至7,755元的部分	20	555
        ///4	超过9,000元至35,000元的部分	超过7,755元至27,255元的部分	25	1,005
        ///5	超过35,000元至55,000元的部分	超过27,255元至41,255元的部分	30	2,755
        ///6	超过55,000元至80,000元的部分	超过31,375元至45,375元的部分	35	5,505
        ///7	超过80,000元的部分	超过57,505的部分	45	13,505
        /// </summary>
        /// <param name="income">扣除三险一金后应缴的个人所得税</param>
        /// <returns></returns>
        public static decimal CalculateTax(decimal income)
        {
            //税率
            decimal rate;
            //速算扣除数
            decimal fastDeduct;

            if (income <= 1500)
            {
                rate = 0.03M;
                fastDeduct = 0M;

                return income * rate - fastDeduct;
            }
            else if(income > 1500 && income <=4500)
            {
                rate = 0.1M;
                fastDeduct = 105M;
                return income * rate - fastDeduct;
            }
            else if (income > 4500 && income <= 9000)
            {
                rate = 0.2M;
                fastDeduct = 555M;
                return income * rate - fastDeduct;
            }
            else if (income > 9000 && income <= 35000)
            {
                rate = 0.25M;
                fastDeduct = 1005M;
                return income * rate - fastDeduct;
            }
            else if (income > 35000 && income <= 55000)
            {
                rate = 0.3M;
                fastDeduct = 2755M;
                return income * rate - fastDeduct;
            }
            else if (income > 55000 && income <= 80000)
            {
                rate = 0.35M;
                fastDeduct = 5505M;
                return income * rate - fastDeduct;
            }
            else if (income > 80000)
            {
                rate = 0.45M;
                fastDeduct = 13505M;
                return income * rate;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region 根据教师来源和任职身份，判断教师类型
        /// <summary>
        /// 判断教师的身份类型，这个身份分为三类：院内教师/助教, 院外教师, 院外助教
        /// 其中院内教师/助教 返回"1"; 院外教师 返回"2"; 院外助教 返回"3"
        /// </summary>
        /// <param name="teacherType">教师来源，从TeachersList表的teacherType中取值</param>
        /// <param name="atCourseType">教师身份：教师或助教，从TeachersList的type中取值</param>
        /// <returns></returns>
        public static int CheckTeacherType(string teacherType, int atCourseType) 
        {
            if ("本院" == teacherType)
            {
                return 1;                   // 院内教师/助教
            }
            else if (atCourseType == 1)
            {
                return 2;                   // 院外教师
            }
            else {
                return 3;                   // 院外助教
            }

        }

        /// <summary>
        /// 根据指定format对给定的formattingValue进行格式化转为float操作
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formattingValue"></param>
        /// <returns></returns>
        public static float ConvertFormatedFloat(string format, string formattingValue) {
            float formattedValue = 0.00f;
            if (!string.IsNullOrWhiteSpace(formattingValue)) { 
                formattedValue = float.Parse(string.Format(format, Convert.ToDouble(formattingValue.Trim())));
            }
            return formattedValue;
        }

        /// <summary>
        /// 根据指定format对给定的formattingValue进行格式化转为double操作
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formattingValue"></param>
        /// <returns></returns>
        public static double ConvertFormatedDouble(string format, string formattingValue) {
            double formattedValue = 0.00;
            if (!string.IsNullOrWhiteSpace(formattingValue)) {
                formattedValue = double.Parse(string.Format(format, Convert.ToDouble(formattingValue.Trim())));
            }
            return formattedValue;
        }


        #endregion

        #region 根据月发放薪酬状态，返回字符串状态值
        /// <summary>
        /// 根据月发放薪酬状态，返回字符串状态值
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ConvertSalaryEntryStatus(int status)
        {
            string salaryEntryStatus;
            if (status == 2)
            {
                salaryEntryStatus = "未确认";
            }
            else if (status == 3)
            {
                salaryEntryStatus = "已确认";
            }
            else
            {
                salaryEntryStatus = "未发放";
            }

            return salaryEntryStatus;
        }
        #endregion

        #region 根据由CheckTeacherType生成的整形类型的teacherType转换成对应的字符串
        /// <summary>
        /// 根据由CheckTeacherType生成的整形类型的teacherType转换成对应的字符串
        /// </summary>
        /// <param name="teacherType"></param>
        /// <returns></returns>
        public static string ConvertTeacherType2String(int teacherType) 
        {

            string result = "";
            switch (teacherType) {
                case 1: result = "院内教师/助教";
                    break;
                case 2: result = "院外教师";
                    break;
                case 3: result = "院外助教";
                    break;
            }
            return result;
        }
        #endregion



        #region 转换termTag为适当的文字表示
        /// <summary>
        /// 转换termTag为适当的文字表示
        /// </summary>
        /// <param name="termTag">学期标识</param>
        /// <returns>转换后的文字</returns>
        public static string ChangeTermToString(string termTag)
        {
            return termTag.Substring(0, 4) + "学年 第" + termTag.Substring(5, 1) + "学期";
        }
        #endregion

        #region 返回例如“2011至2012 学年 第一学期”格式的表示
        /// <summary>
        /// 返回例如“2011至2012 学年 第一学期”格式的表示
        /// </summary>
        /// <param name="termTag"></param>
        /// <returns></returns>
        public static string FormatTermTag(string termTag)
        {
            string _termYear = termTag.Substring(0, 4);

            return _termYear + "学年至" + (int.Parse(_termYear) + 1).ToString() + " 学年 第" + (termTag.Substring(5, 1) == "1" ? "一" : "二") + "学期";
        }
        #endregion

        #region 邮件相关操作

        #region Email正则检测
        /// <summary>
        ///  Email正则表达式检测
        /// </summary>
        /// <param name="emailString">相关的Email表达式</param>
        /// <returns>转化是否成功</returns>
        public static bool CheckStringIsEmail(string emailString)
        {
            Regex reg = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            return reg.IsMatch(emailString.Trim());
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="emailInfoList">要发送邮件的信息列表</param>
        public static void SendEmail(List<EmailInfo> emailInfoList)
        {
            List<string>[] fileList = new List<string>[2];
            List<string> fileNameList = new List<string>();
            List<string> fileUrlList = new List<string>();

            foreach (EmailInfo emailInfo in emailInfoList)
            {
                MailAddress mailFrom = new MailAddress(emailInfo.senderEmailAddress, ConfigurationManager.AppSettings["birefSysName"]);
                MailAddress mailTo = new MailAddress(emailInfo.receiverEmailAddress);

                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = emailInfo.emailTitle;

                StringBuilder sb = new StringBuilder();

                fileList = emailInfo.fileNameList;
                fileNameList = fileList[0];
                fileUrlList = fileList[1];

                //生成附件列表
                for (int i = 0; i < fileNameList.Count; i++)
                {
                    sb.Append("<table style=\"margin-bottom:5px;height:30px;text-indent:10px;line-height:30px;width:100%;text-align: left;background:#cae8ea;border: 1px #999 dashed;\"><tr><td><a href=\"");
                    sb.Append(fileUrlList[i]);
                    sb.Append("' target=\"_blank\">");
                    sb.Append(fileNameList[i]);

                    sb.Append("</a>");
                    sb.Append("</td></tr></table>");
                }

                message.Body = emailInfo.emailContent + "<br><br>" +
                    sb.ToString() +
                "<br><br>" + ConfigurationManager.AppSettings["sseaDomain"];
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;


                //Create  the file attachment for this e-mail message.
                //暂时不开放邮件附件功能，而是通过发送附件URL列表来提升性能，
                //没有必要直接发送文件

                //foreach (string fileName in emailInfo.fileNameList)
                //{
                //    if (File.Exists(serverFolder + fileName))
                //    {
                //        Attachment data = new Attachment(serverFolder + fileName, MediaTypeNames.Application.Octet);
                //        message.Attachments.Add(data);
                //    }
                //    //data.Dispose();
                //}
                //Send the message.

                SmtpClient client = new SmtpClient(emailInfo.senderEmailServer);
                // Add credentials if the SMTP server requires them.
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(emailInfo.senderEmailAddress, emailInfo.senderEmailPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.EnableSsl = true;
                client.Send(message);
            }
        }
        #endregion

        #region 根据数字值返回邮件类型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ReturnEmailTypeByVal(int val)
        {
            string emailType = string.Empty;
            switch (val)
            {
                case (int)EmailType.adminEmail:
                    emailType = "管理员发送的邮件";
                    break;
                case (int)EmailType.notifyEmail:
                    emailType = "通知相关邮件";
                    break;
                case (int)EmailType.gradeCheckNotifyEmail:
                    emailType = "成绩审核录入通知邮件";
                    break;
                case (int)EmailType.gradeCheckApplyEmail:
                    emailType = "重修重考通知邮件";
                    break;
                case (int)EmailType.englishExamEmail:
                    emailType = "四六级通知邮件";
                    break;
                case (int)EmailType.archivesEmail:
                    emailType = "结课资料上传通知邮件";
                    break;
                case (int)EmailType.salaryEmail:
                    emailType = "酬金通知邮件";
                    break;
                case (int)EmailType.normEmail:
                    emailType = "工作量通知邮件";
                    break;
                default:
                    break;
            }
            return emailType;
        }
        #endregion

        #region 获取指定路径的Email模板信息
        /// <summary>
        /// 获取指定路径的Email模板信息
        /// </summary>
        public static EmailTemplate GetDetailEmailConfigInfoByPath(string path)
        {
            if (!File.Exists(path))
            {
                return new EmailTemplate { title = string.Empty, content = string.Empty, isModify = -1, type = -1 };
            }

            XmlDocument doc = new XmlDocument();

            FileInfo _file = new FileInfo(path);
            doc.Load(_file.FullName);
            int _type = int.Parse(doc.SelectSingleNode("/EmailConfig/type").InnerText);
            string _title = doc.SelectSingleNode("/EmailConfig/title").InnerText;
            int _isModify = int.Parse(doc.SelectSingleNode("/EmailConfig/isModify").InnerText);
            string _content = doc.SelectSingleNode("/EmailConfig/content").InnerText;
            EmailTemplate _emailTemplate = new EmailTemplate { title = _title, isModify = _isModify, content = _content, type = _type };
            return _emailTemplate;
        }
        #endregion
        
        #region 获取全部Email模板类型信息
        /// <summary>
        /// 获取全部Email模板类型信息
        /// </summary>
        public static Hashtable GetAllEmailTemplateTypeInfo()
        {
            string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["EmailTemplate"]);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Hashtable hashtableEmailTemplateType = new Hashtable();

            DirectoryInfo _folder = new DirectoryInfo(path);
            XmlDocument doc = new XmlDocument();


            foreach (FileInfo _file in _folder.GetFiles())
            {
                if (_file.Extension.ToLower() == ".config")
                {
                    doc.Load(_file.FullName);
                    hashtableEmailTemplateType.Add(doc.SelectSingleNode("/EmailConfig/type").InnerText, _file.FullName);
                }
            }

            return hashtableEmailTemplateType;
        }
        #endregion

        #region 根据文件路径更新邮件模板信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="_emailTemplate"></param>
        /// <returns></returns>
        public static bool UpdateEmailTemplateInfo(string path,EmailTemplate _emailTemplate)
        {
            if (!File.Exists(path))
            {
                return false; ;
            }

            XmlDocument doc = new XmlDocument();


            int _type = _emailTemplate.type;
            XmlCDataSection _title = doc.CreateCDataSection(_emailTemplate.title);
            int _isModify = _emailTemplate.isModify;
            XmlCDataSection _content = doc.CreateCDataSection(_emailTemplate.content);

            FileInfo _file = new FileInfo(path);
            doc.Load(_file.FullName);

            //XmlNode _typeNode = doc.SelectSingleNode("/EmailConfig/type");
            XmlNode _titleNode = doc.SelectSingleNode("/EmailConfig/title");
            //XmlNode _isModifyNode = doc.SelectSingleNode("/EmailConfig/isModify");
            XmlNode _contentNode = doc.SelectSingleNode("/EmailConfig/content");

            //_typeNode.InnerText = _emailTemplate.type.ToString();

            _titleNode.InnerText=string.Empty;
            _titleNode.AppendChild(_title);

            //_isModifyNode.InnerText = _emailTemplate.isModify.ToString();

            _contentNode.InnerText = string.Empty;
            _contentNode.AppendChild(_content);
            
            doc.Save(path);

            return true;
        }
        #endregion

        #endregion


        #region 根据数字值返回反馈类型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ReturnFeedBackTypeByVal(int val)
        {
            string feedbackType = string.Empty;
            switch (val)
            {
                case (int)FeedBackType.SystemType:
                    feedbackType = "系统反馈";
                    break;
                case (int)FeedBackType.CET46Type:
                    feedbackType = "四六级报名反馈";
                    break;
                case (int)FeedBackType.EXAMType:
                    feedbackType = "成绩审核反馈";
                    break;
                default:
                    break;
            }
            return feedbackType;
        }
        #endregion


        #region 根据传入值返回活动类型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ReturnAllowSexTypeByVal(string val)
        {
            string _str = "暂无";

            List<string> _list = new List<string>();

            string[] _arr = val.Split(",".ToCharArray());

            for (int i = 0; i < _arr.Length; i++)
            {
                switch (_arr[i].Trim())
                {
                    case "1":
                        _list.Add("男教师");
                        break;
                    case "2":
                        _list.Add("女教师");
                        break;
                    case "12":
                        _list.Add("男教师和女教师");
                        break;
                    default:
                        break;
                }
            }

            return (_list.Count > 0 ? string.Join("，", _list.ToArray()) : _str);
        }
        #endregion

    }
}