namespace BackupIt_daemon_v2
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class FileSystemNode : IEquatable<FileSystemNode>
    {
        [JsonProperty("FullPath", NullValueHandling = NullValueHandling.Ignore)]
        public string FullPath { get; set; }

        [JsonProperty("LastModified", NullValueHandling = NullValueHandling.Ignore)]
        public string LastModified { get; set; }

        [JsonProperty("Action", NullValueHandling = NullValueHandling.Ignore)]
        public object Action { get; set; }

        [JsonProperty("IsDirectory", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsDirectory { get; set; } //public bool? IsDirectory { get; set; }

        public bool Equals(FileSystemNode other)
        {
            //Check whether the compared object is null.
            if (FileSystemNode.ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (FileSystemNode.ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the objects’ properties are equal.

            try
            {
                bool fullPath = FullPath is null && other.FullPath is null || FullPath.Equals(other.FullPath);
                bool isDirectory = LastModified.Equals(other.LastModified);

                return fullPath && isDirectory;
            }
            catch (System.NullReferenceException)
            {
                return false;
            }

            
        }

        public override int GetHashCode()
        {
            int hashTextual = FullPath == null ? 0 : FullPath.GetHashCode();

            // Get the hash code for the Digital field.
            int hashDigital = LastModified.GetHashCode();

            // Calculate the hash code for the object.
            return hashDigital ^ hashTextual;
        }
    }
}
