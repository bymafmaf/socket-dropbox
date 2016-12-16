using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class File
    {

        public string fileName;
        public long fileSize;
        public DateTime uploadTime;
        [JsonIgnore]
        public string filePath;

        public File() { }
        public File(string path)
        {
            filePath = path;
            FileInfo info = new FileInfo(filePath);
            fileName = info.Name;
            fileSize = info.Length;
            uploadTime = info.LastWriteTime;
        }

        public File(User theUser, string fileName)
        {
            string path = theUser.directoryToSave + "\\" + fileName;
            filePath = path;
            FileInfo info = new FileInfo(filePath);
            fileName = info.Name;
            fileSize = info.Length;
            uploadTime = info.LastWriteTime;
        }

        public static File getFromList(List<File> fileList, string filename)
        {
            foreach (File aFile in fileList)
            {
                if (filename == aFile.fileName)
                    return aFile;
            }
            return null;
        }
        public bool rename(string newName)
        {
            FileInfo info = new FileInfo(filePath);
            if (System.IO.File.Exists(info.Directory.FullName + "\\" + newName))
            {
                return false;
            }
            info.MoveTo(info.Directory.FullName + "\\" + newName);
            return true;
        }

        public void delete()
        {
            FileInfo info = new FileInfo(filePath);
            info.Delete();
        }
    }

    
}
