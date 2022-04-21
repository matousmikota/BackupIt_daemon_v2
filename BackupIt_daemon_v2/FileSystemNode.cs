namespace BackupIt_daemon_v2
{
    using Newtonsoft.Json;
    public class FileSystemNode
    {
        [JsonProperty("FullPath", NullValueHandling = NullValueHandling.Ignore)]
        public string FullPath { get; set; }

        [JsonProperty("LastModified", NullValueHandling = NullValueHandling.Ignore)]
        public string LastModified { get; set; }

        [JsonProperty("Action", NullValueHandling = NullValueHandling.Ignore)]
        public object Action { get; set; }

        [JsonProperty("IsDirectory", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsDirectory { get; set; } //public bool? IsDirectory { get; set; }
    }
}
