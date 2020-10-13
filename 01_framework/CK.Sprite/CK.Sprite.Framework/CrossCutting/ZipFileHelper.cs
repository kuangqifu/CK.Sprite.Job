using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace CK.Sprite.Framework
{
    public class ZipFileHelper
    {
        public static byte[] CreateZipStream(List<ZipFileInfo> zipFileInfos)
        {
            List<InMemoryFile> memoryFiles = new List<InMemoryFile>();
            foreach (var zipFileInfo in zipFileInfos)
            {
                using (WebClient web = new WebClient())
                {
                    var downLoadStream = web.OpenRead(zipFileInfo.DownloadUrl);
                    memoryFiles.Add(new InMemoryFile()
                    {
                        FileName = zipFileInfo.FileName,
                        DownloadStream = downLoadStream
                    });
                }
            }

            var zipResult = GetZipArchive(memoryFiles);
            return zipResult;
        }

        private static byte[] GetZipArchive(List<InMemoryFile> files)
        {
            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                        {
                            file.DownloadStream.CopyTo(zipStream);
                        }
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }
    }

    /// <summary>
    /// 压缩实体
    /// </summary>
    public class ZipFileInfo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 下载路径
        /// </summary>
        public string DownloadUrl { get; set; }
    }

    /// <summary>
    /// 压缩实体
    /// </summary>
    internal class InMemoryFile
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 下载到内存的对象
        /// </summary>
        public Stream DownloadStream { get; set; }
    }
}
