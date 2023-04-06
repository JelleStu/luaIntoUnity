using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LuaBridge.Core.Extensions
{
    public static class DirectoryInfoExtension
    {
        public static async Task<long> GetSizeAsync(this DirectoryInfo directoryInfo, string path)
        {
            return await Task.Run(() => directoryInfo.GetSize(path, out _));
        }

        public static long GetSize(this DirectoryInfo directoryInfo, string path, out DirectoryInfo directory)
        {
            if (directoryInfo == null)
                directoryInfo = string.IsNullOrEmpty(path) ? null : new DirectoryInfo(path);
            else
                directoryInfo.Refresh();
            directory = directoryInfo;
            return directoryInfo?.GetSize() ?? 0;
        }

        private static long GetSize(this DirectoryInfo directoryInfo)
        {
            try
            {
                if (!directoryInfo.Exists)
                    return GetFileInfoLength(directoryInfo);
                return GetSizeOffAllFilesInDirectory(directoryInfo) + GetSizeOfSubDirectories(directoryInfo);
            }
            catch (DirectoryNotFoundException)
            {
                return 0;
            }
            catch (FileNotFoundException)
            {
                return 0;
            }
        }

        private static long GetSizeOfSubDirectories(DirectoryInfo directoryInfo)
        {
            try
            {
                return directoryInfo.GetDirectories().Sum(info => info.GetSize());
            }
            catch (DirectoryNotFoundException)
            {
                return 0;
            }
        }

        private static long GetSizeOffAllFilesInDirectory(DirectoryInfo directoryInfo)
        {
            try
            {
                return directoryInfo.GetFiles().Sum(info => !info.Exists || info.Name.EndsWith(".tmp") ? 0 : info.Length);
            }
            catch (FileNotFoundException)
            {
                return 0;
            }
        }

        private static long GetFileInfoLength(DirectoryInfo directoryInfo)
        {
            try
            {
                var fileInfo = new FileInfo(directoryInfo.FullName);
                return fileInfo.Exists ? fileInfo.Length : 0;
            }
            catch (FileNotFoundException)
            {
                return 0;
            }
        }
    }
}