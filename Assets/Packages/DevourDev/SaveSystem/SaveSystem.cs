using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;

namespace DevourDev.ProgressSaving
{
    public static class SaveSystem
    {
        private const string _saveFolderName = "Saves";
        private const string _saveFileName = "Save";
        private const string _saveFileExtention = ".dat";


        public static BinaryWriter GetWriter()
        {
            string path = GenerateNewSaveFilePath();
            FileInfo finfo = new(path);
            var stream = finfo.Create();
            return new BinaryWriter(stream);
        }


        public static bool TryGetLastReader(out BinaryReader br)
        {
            if (!TryGetSaveFiles(out var files))
            {
                br = null;
                return false;
            }

            var lastSaveFile = files[0];
            DateTime lastDateTime = lastSaveFile.LastWriteTime;

            for (int i = 1; i < files.Length; i++)
            {
                var f = files[i];
                var d = f.LastWriteTime;

                if (d > lastDateTime)
                {
                    lastDateTime = d;
                    lastSaveFile = f;
                }
            }

            br = new(lastSaveFile.OpenRead());
            return true;
        }

        public static bool TryGetReader(string saveFileName, out BinaryReader br)
        {

            if (TryGetSaveFiles(out var files))
            {
                foreach (var f in files)
                {
                    if (f.Name == saveFileName)
                    {
                        br = new(f.OpenRead());
                        return true;
                    }
                }
            }

            br = null;
            return false;
        }


        public static BinaryReader GetReader(FileInfo saveFileInfo)
        {
            return new BinaryReader(saveFileInfo.OpenRead());
        }

        public static bool TryGetSaveFiles(out FileInfo[] files)
        {
            var dir = GetSaveDirectory();
            files = dir.GetFiles();

            return files != null && files.Length != 0;
        }

        private static string FixFileName(string raw)
        {
            var arr = Path.GetInvalidFileNameChars();
            var hs = new HashSet<char>(arr);
            return raw.ReplaceMultiple(hs, '_');
        }

        private static string GenerateNewSaveFilePath()
        {
            DirectoryInfo dir = GetSaveDirectory();

            string dirStr = dir.ToString();
            string dateStr = DateTime.Now.ToString();
            string fullPath;

            for (int i = 0; ; ++i)
            {
                string fileName = $"{_saveFileName}_{dateStr}{(i > 0 ? '_' + i.ToString() : string.Empty)}{_saveFileExtention}";
                fileName = FixFileName(fileName);


                fullPath = Path.Combine(dirStr, fileName);

                if (!File.Exists(fullPath))
                    break;
            }

            if (fullPath == null)
            {
                throw new NullReferenceException(nameof(fullPath));
            }

            return fullPath;
        }

        private static DirectoryInfo GetSaveDirectory()
        {
            var dir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), _saveFolderName));

            if (!dir.Exists)
                dir.Create();

            return dir;
        }
    }
}
