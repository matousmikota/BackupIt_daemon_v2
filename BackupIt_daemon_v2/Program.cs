using System;
using System.IO;

namespace BackupIt_daemon_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Snapshot snapshot = new();
            snapshot.Create(@"C:\Users\a\Music\source-testA", @"C:\Users\a\Documents\BackupIt-daemon-snapshot\snapshot.json");
        }
    }
}
