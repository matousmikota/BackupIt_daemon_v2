using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupIt_daemon_v2
{
    public class Snapshot
    {
        public void Create(string folderPath, string snapshopFilePath)
        {
            DynatreeItem di = new DynatreeItem(new DirectoryInfo(folderPath));
            string snapshotText = "[" + di.JsonToDynatree() + "]";

            using (StreamWriter writer = new(snapshopFilePath))
            {
                writer.Write(snapshotText);
            }
        }

    }
}
