using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace BackupIt_daemon_v2.Models
{
    public class Client
    {
        public List<Config> configs { get; set; } //virtual ?
        public LazyLoader lazyLoader { get; set; }
        public int id { get; set; }
        public string device_name { get; set; }
        public string mac_address { get; set; }
        public string ipv4_address { get; set; }
        public DateTime last_backup { get; set; }
        public DateTime last_seen { get; set; }

        public Client(string device_name, string mac_address, string ipv4_address, DateTime last_seen)
        {
            this.device_name = device_name;
            this.mac_address = mac_address;
            this.ipv4_address = ipv4_address;
            this.last_seen = last_seen;
        }
    }
}
