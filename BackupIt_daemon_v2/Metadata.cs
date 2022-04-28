using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackupIt_daemon_v2
{
    public class Metadata
    {
        public string Path { get; set; }
        public Metadata(string path)
        {
            this.Path = path;
        }
        public void AddMetadata(FileSystemNode node, string action)
        {
            node.Action = action;

            using (StreamWriter sw = new(this.Path))
            {
                sw.Write(JsonConvert.SerializeObject(node, Formatting.None));
            }
        }
        public void AddMetadata(List<FileSystemNode> nodes, string action)
        {
            foreach (FileSystemNode node in nodes)
            {
                node.Action = action;
            }

            using (StreamWriter sw = new(this.Path))
            {
                sw.Write(JsonConvert.SerializeObject(nodes, Formatting.None));
            }
        }

    }
}
