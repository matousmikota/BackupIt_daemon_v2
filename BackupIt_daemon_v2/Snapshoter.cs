using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackupIt_daemon_v2
{
    public class Snapshoter
    {
        public List<FileSystemNode> Nodes { get; set; } = new();

        public Snapshoter(string sourcePath, string outputPath)
        {
            this.Create(new DirectoryInfo(sourcePath));

            using (StreamWriter sw = new(outputPath))
            {
                sw.Write(this.NodesToJson());
            }
        }

        private void Create(FileSystemInfo fsi)
        {
            FileSystemNode fsn = new();
            fsn.FullPath = fsi.FullName;
            fsn.LastModified = fsi.LastWriteTime.ToString("yyyyMMddHHmmss");

            if (fsi.Attributes == FileAttributes.Directory)
            {
                fsn.IsDirectory = true;
                this.Nodes.Add(fsn);

                foreach (FileSystemInfo f in (fsi as DirectoryInfo).GetFileSystemInfos())
                {
                    this.Create(f);
                }
            }
            else
            {
                fsn.IsDirectory = false;
                this.Nodes.Add(fsn);
            }
        }

        private string NodesToJson()
        {
            return JsonConvert.SerializeObject(this.Nodes, Formatting.None);
        }
    }
}
