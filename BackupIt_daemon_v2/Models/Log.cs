using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupIt_daemon_v2.Models
{
    public class Log
    {
        public int Id { get; set; }

        public int client_id { get; set; }

        public int config_id { get; set; }

        public bool was_successful { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public string error_code { get; set; }

        public Log(int client_id, int config_id, bool was_successful, DateTime start, DateTime end, string error_code)
        {
            this.client_id = client_id;
            this.config_id = config_id;
            this.was_successful = was_successful;
            this.start = start;
            this.end = end;
            this.error_code = error_code;
        }
    }
}
