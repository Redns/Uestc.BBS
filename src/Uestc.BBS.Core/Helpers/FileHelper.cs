namespace Uestc.BBS.Core.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// 删除指定目录下的文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        public static void DeleteFiles(
            this string path,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.AllDirectories
        )
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            Directory.GetFiles(path, searchPattern, searchOption).AsParallel().ForAll(File.Delete);
        }

        /// <summary>
        /// 获取指定目录下的文件总大小
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static long GetFileTotalSize(
            this string path,
            string searchPattern = "*",
            SearchOption searchOption = SearchOption.AllDirectories
        )
        {
            if (!Directory.Exists(path))
            {
                return 0;
            }

            return new DirectoryInfo(path)
                .EnumerateFiles(searchPattern, searchOption)
                .AsParallel()
                .Sum(f => f.Length);
        }
    }
}
