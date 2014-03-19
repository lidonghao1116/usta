using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web;
using USTA.Model;
using System.IO;
using System.Configuration;

namespace USTA.Common
{
    /// <summary>
    /// 压缩图片类
    /// </summary>
    public class Thumbnails
    {
        /// <summary>
        /// A better alternative to Image.GetThumbnail. Higher quality but slightly slower
        /// </summary>
        /// <param name="source">源图片流</param>
        /// <param name="thumbWi">图片宽度</param>
        /// <param name="thumbHi">图片高度</param>
        /// <param name="maintainAspect">maintain the aspect ratio despite the thumbnail size parameters</param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(Bitmap source, int thumbWi, int thumbHi, bool maintainAspect)
        {
            // return the source image if it's smaller than the designated thumbnail
            if (source.Width < thumbWi && source.Height < thumbHi) return source;

            System.Drawing.Bitmap ret = null;
            try
            {
                int wi, hi;

                wi = thumbWi;
                hi = thumbHi;

                if (maintainAspect)
                {
                    // maintain the aspect ratio despite the thumbnail size parameters
                    if (source.Width > source.Height)
                    {
                        wi = thumbWi;
                        hi = (int)(source.Height * ((decimal)thumbWi / source.Width));
                    }
                    else
                    {
                        hi = thumbHi;
                        wi = (int)(source.Width * ((decimal)thumbHi / source.Height));
                    }
                }

                // original code that creates lousy thumbnails
                // System.Drawing.Image ret = source.GetThumbnailImage(wi,hi,null,IntPtr.Zero);
                ret = new Bitmap(wi, hi);
                using (Graphics g = Graphics.FromImage(ret))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, wi, hi);
                    g.DrawImage(source, 0, 0, wi, hi);
                }
            }
            catch
            {
                ret = null;
            }

            return ret;
        }

        /// <summary>
        /// 创建图片文件
        /// </summary>
        /// <param name="fileName">图片文件名</param>
        public static void CreateImage(string fileName)
        {
            int width = 1;
            int height = 0;
            int constWidth = 100;

            FileInfo fi = new FileInfo(fileName);
            
            File.Copy(fileName, HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FCKeditor:TempUploadPicPath"] + fi.Name), true);

            Bitmap myBitmap = new Bitmap(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FCKeditor:TempUploadPicPath"] + fi.Name));


            if (myBitmap.Width > constWidth)
            {
                width = constWidth;
                height = (int)(myBitmap.Height * width / myBitmap.Width);
            }
            else
            {
                width = myBitmap.Width;
                height = myBitmap.Height;
            }


            //Configure JPEG Compression Engine
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 75;
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    break;
                }
            }
            System.Drawing.Image myThumbnail = CreateThumbnail(myBitmap, width, height, false);

            myThumbnail.Save(fileName, jpegICI, encoderParams);
            myThumbnail.Dispose();
            myBitmap.Dispose();

            File.Delete(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FCKeditor:TempUploadPicPath"] + fi.Name));
        }
    }
}