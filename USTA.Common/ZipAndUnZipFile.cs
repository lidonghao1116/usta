using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Web;

namespace USTA.Common
{
    /// <summary>
    /// 压缩解压类
    /// </summary>
    public sealed class ZipAndUnZipFile
    {
        /// <summary>
        /// 压缩文件（单文件）
        /// </summary>
        /// <param name="FileToZip">要压缩的文件</param>
        /// <param name="ZipedFile">生成的压缩文件路径</param>
        /// <param name="BlockSize">缓存块大小</param>
        public static void ZipFile(string FileToZip, string ZipedFile, int BlockSize)
        {
            //如果文件没有找到，则报错  
            if (!File.Exists(FileToZip))
            {
                MongoDBLog.LogRecord(new Exception("压缩文件未找到！"));
            }

            FileStream StreamToZip = new FileStream
            (FileToZip, FileMode.Open, FileAccess.Read);
            FileStream ZipFile = File.Create(ZipedFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));
            ZipStream.PutNextEntry(ZipEntry);
            byte[] buffer = new byte[BlockSize];
            System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
            HttpContext.Current.Response.Write(StreamToZip.Length);
            ZipStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToZip.Length)
                {
                    int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
            }
            finally
            {
                ZipStream.Finish();
                ZipStream.Close();
                StreamToZip.Close();
            }
        }


        /// <summary>
        /// 压缩文件（多文件）
        /// </summary>
        /// <param name="FileToZipList">要压缩的文件名集合</param>
        /// <param name="ZipedFile">生成的压缩文件路径</param>
        /// <param name="BlockSize">缓存块大小</param>
        public static void MultiFilesZip(IList<string> FileToZipList, string ZipedFile, int BlockSize)
        {
            FileStream ZipFile = File.Create(ZipedFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);

            try
            {
                foreach (string FileToZip in FileToZipList)
                { 
                    //如果文件没有找到，则报错  
                    if (!File.Exists(HttpContext.Current.Server.MapPath("/" + FileToZip)))
                    {
                        MongoDBLog.LogRecord(new Exception("压缩文件未找到！"));
                        continue;
                    }
                    FileStream StreamToZip = new FileStream
                    (HttpContext.Current.Server.MapPath("/" + FileToZip), FileMode.Open, FileAccess.Read);

                    ZipEntry ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));
                    ZipStream.PutNextEntry(ZipEntry);
                    byte[] buffer = new byte[BlockSize];
                    System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.Write(StreamToZip.Length);
                    ZipStream.Write(buffer, 0, size);
                    while (size < StreamToZip.Length)
                    {
                        int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                        ZipStream.Write(buffer, 0, sizeRead);
                        size += sizeRead;
                    }
                    StreamToZip.Close();
                }
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
            }
            finally
            {
                ZipStream.Finish();
                ZipStream.Close();
            }
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="FileToZip">压缩后的文件名</param>
        /// <param name="FileFolderToZip">要压缩的文件夹</param>
        /// <returns>压缩文件是否成功</returns>
        public static Boolean ZipFileFolder(string FileToZip, string FileFolderToZip)
        {
            try
            {
                FastZip fastZip = new FastZip();
                fastZip.CreateZip(FileToZip, FileFolderToZip, true, "");
                return true;
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                return false;
            }

        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="FileToZip">要解压的文件路径</param>
        /// <param name="FileFolderToUnZip">解压到的文件夹路径</param>
        /// <returns>解压文件是否成功</returns>
        public static Boolean UNZipFile(string FileToZip, string FileFolderToUnZip)
        {
            try
            {
                FastZip fastZip = new FastZip();
                fastZip.ExtractZip(FileToZip, FileFolderToUnZip, "");
                return true;
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                return false;
            }

        }
    }
}
