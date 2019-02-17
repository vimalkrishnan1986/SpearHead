using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace SpearHead.Common.Helpers
{
    public static class FileHelper
    {
        public static void WriteToFile(byte[] file, string fileName)
        {
            File.WriteAllBytes(fileName, file);
        }

        public static string CurrentDirrectory(string location)
        {
            return Path.GetDirectoryName(location);
        }
        public static string GetFileName(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public static void CreateDirectory(String directoryName)
        {
            DirectoryInfo info = new DirectoryInfo(directoryName);
            if (!info.Exists)
            {
                info.Create();
            }
        }
        public static void Delete(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            File.Delete(path);
        }
    }
}
