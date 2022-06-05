using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupIt_daemon_v2.Models
{
    public class Destinations
    {
        public int id { get; set; }

        public string name { get; set; }

        public string path { get; set; }

        public string type { get; set; }

        public string login { get; set; }

        public string password { get; set; }
    }
}
