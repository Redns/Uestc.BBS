namespace Uestc.BBS.Core.Helpers
{
    public static class FileHelper
    {
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
                .GetFiles(searchPattern, searchOption)
                .AsParallel()
                .Sum(f => f.Length);
        }
    }
}
