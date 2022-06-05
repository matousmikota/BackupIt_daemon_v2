using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupIt_daemon_v2.Models
{
    public class Source
    {
        public int id { get; set; }

        public int config_id { get; set; }

        public string path { get; set; }
    }
}
