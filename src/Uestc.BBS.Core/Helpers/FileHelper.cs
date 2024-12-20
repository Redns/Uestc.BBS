namespace Uestc.BBS.Core.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="fileSizeBytes"></param>
        /// <returns></returns>
        public static string FormatFileSize(this long fileSizeBytes)
        {
            if (fileSizeBytes < 0)
            {
                return string.Empty;
            }

            if (fileSizeBytes < 1024)
            {
                return $"{fileSizeBytes} B"; 
            }

            if (fileSizeBytes < 1024 * 1024)
            {
                return $"{fileSizeBytes / 1024.0:F2} KB";
            }

            if (fileSizeBytes < 1024 * 1024 * 1024)
            {
                return $"{fileSizeBytes / (1024.0 * 1024):F2} MB";
            }

            return $"{fileSizeBytes / (1024.0 * 1024 * 1024):F2} GB";
        }

        public static long GetFileSize(this string path)
        {
            if (!File.Exists(path))
            {
                return 0;
            }

            return new FileInfo(path).Length;
        }

        public static void DeleteFiles(this string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            foreach (var file in Directory.GetFiles(path, searchPattern, searchOption))
            {
                File.Delete(file);
            }
        }

        public static long GetFileTotalSize(this string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (!Directory.Exists(path))
            {
                return 0;
            }

            return Directory.GetFiles(path, searchPattern, searchOption).Sum(f => new FileInfo(f).Length);
        }
    }
}
