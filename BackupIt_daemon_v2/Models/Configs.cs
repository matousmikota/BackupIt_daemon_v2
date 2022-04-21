using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackupIt_daemon_v2.Models
{
    public class Config
    {
        public int Id { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string backup_cron { get; set; }

        public int max_size { get; set; }

        public int max_count { get; set; }

        public bool compress_into_archive { get; set; }

        [JsonIgnore()]
        public virtual List<Client> Clients { get; set; }

    }
}
