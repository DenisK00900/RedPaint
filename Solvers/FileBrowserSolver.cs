using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedPaint
{
    public static class FileBrowserSolver
    {
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
