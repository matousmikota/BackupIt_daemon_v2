using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BackupIt_daemon_v2
{
    public class Snapshot
    {
        [JsonProperty("children")]
        public Snapshot[] Children { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isFolder")]
        public bool IsFolder { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("modified")]
        public string Modified { get; set; }

        [JsonProperty("fullPath")]
        public string FullPath { get; set; }
    }
}
