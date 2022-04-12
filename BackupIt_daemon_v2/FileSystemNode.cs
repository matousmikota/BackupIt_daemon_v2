namespace BackupIt_daemon_v2
{
    public class FileSystemNode
    {
        public string FullPath { get; set; }
        public string LastModified { get; set; }
        public string Action { get; set; }
        public bool IsDirectory { get; set; }
    }
}
