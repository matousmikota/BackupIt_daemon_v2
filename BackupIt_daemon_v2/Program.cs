using JsonDiffer;
using Newtonsoft.Json;
//using JsonDiffPatchDotNet;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackupIt_daemon_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Backuper backuper = new("testZaloha", "incremental", new List<string> { @"C:\Users\a\Music\source-testA" }, new List<string> { @"C:\Users\a\Videos\destinationA" }, false);

            while (true)
            {
                backuper.Run();
            }
            
            
            

            /*
            API api = new API();
            api.Initialize();
            */
        }
    }
}
