using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedPaint
{
    public static class FileBrowserSolver
    {
        public static string GetTypeOfPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "Неопр.";

            if (path.EndsWith("/"))
                return "Папка";

            if (path.EndsWith("\\"))
                return "Диск";

            string lowerPath = path.ToLowerInvariant();

            if (lowerPath.EndsWith(".png") || lowerPath.EndsWith(".jpg"))
                return "Изобр.";

            int lastSlashIndex = path.LastIndexOf('/');
            string fileName = (lastSlashIndex >= 0 && lastSlashIndex < path.Length - 1)
                ? path.Substring(lastSlashIndex + 1)
                : path;

            if (fileName.Contains("."))
                return "Файл";

            return "Неопр.";
        }

        public static string ShortenString(string path, int maxSize = 32)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            if (path.Length > maxSize)
            {
                int safeSize = Math.Max(0, maxSize);
                return path.Substring(0, safeSize) + "...";
            }

            return path;
        }

        public static string CanOpenPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "Путь не указан";

            bool isDirectory = path.EndsWith("/") || path.EndsWith("\\");
            string lowerPath = path.ToLowerInvariant();

            if (isDirectory)
            {
                try
                {
                    var _ = Directory.GetFileSystemEntries(path);
                    return string.Empty;
                }
                catch (UnauthorizedAccessException)
                {
                    return "Нет доступа";
                }
                catch (IOException)
                {
                    return $"Ошибка доступа";
                }
            }
            else
            {
                if (!lowerPath.EndsWith(".png") && !lowerPath.EndsWith(".jpg"))
                    return "Неподд. тип";

                try
                {
                    using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                    }
                    return string.Empty;
                }
                catch (UnauthorizedAccessException)
                {
                    return "Нет доступа";
                }
                catch (IOException)
                {
                    return $"Ошибка чтения";
                }
            }
        }

        public static List<string> GetDiskRoots()
        {
            var roots = new List<string>();

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    roots.Add(drive.RootDirectory.FullName);
                }
            }

            return roots;
        }
        public static List<string> GetDirectoryContents(string directoryPath)
        {
            if (directoryPath == "") return GetDiskRoots();

            var result = new List<string>();

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException(
                    $"Директория не найдена: {directoryPath}"
                );
            }

            try
            {
                var entries = Directory.GetFileSystemEntries(directoryPath);

                foreach (var entry in entries)
                {
                    string name = Path.GetFileName(entry);

                    if (Directory.Exists(entry))
                    {
                        result.Add(name + "/");
                    }
                    else if (File.Exists(entry))
                    {
                        result.Add(name);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException(
                    $"Нет доступа к директории: {directoryPath}"
                );
            }

            return result;
        }
    }
}
