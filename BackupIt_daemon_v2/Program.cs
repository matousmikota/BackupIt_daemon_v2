using JsonDiffer;
using Newtonsoft.Json;
//using JsonDiffPatchDotNet;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using BackupIt_daemon_v2.Models;

namespace BackupIt_daemon_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Planner planner = new Planner();
            planner.Run();


            /*
            Backuper backuper = new("incremental", new List<string> { @"C:\Users\a\Music\source-testA" }, new List<string> { @"C:\Users\a\Videos\destinationA" }, false, 20, true, new Config());

            while (true)
            {
                backuper.Run();
            }
            */

        }
    }
}
