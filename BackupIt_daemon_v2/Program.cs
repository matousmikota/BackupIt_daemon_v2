using System;
using System.IO;

namespace BackupIt_daemon_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DynatreeItem di = new DynatreeItem(new DirectoryInfo(@"C:\Users\a\Music\source-testA"));
            string result = "[" + di.JsonToDynatree() + "]";
            Console.WriteLine(result);
        }
    }
}
