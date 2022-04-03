using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackupIt_daemon_v2
{
    public class DynatreeItem
    {
        public string name { get; set; }
        public bool isFolder { get; set; }
        public string key { get; set; }
        public string modified { get; set; }
        public string fullPath { get; set; }
        public List<DynatreeItem> children;
        public DynatreeItem(FileSystemInfo fsi)
        {
            name = fsi.Name;
            modified = fsi.LastWriteTime.ToString("yyyyMMddHHmmss");
            fullPath = fsi.FullName;
            children = new List<DynatreeItem>();

            if (fsi.Attributes == FileAttributes.Directory)
            {
                isFolder = true;
                foreach (FileSystemInfo f in (fsi as DirectoryInfo).GetFileSystemInfos())
                {
                    children.Add(new DynatreeItem(f));
                }
            }
            else
            {
                isFolder = false;
            }
            key = name.Replace(" ", "").ToLower();
        }
        
        public string JsonToDynatree()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
