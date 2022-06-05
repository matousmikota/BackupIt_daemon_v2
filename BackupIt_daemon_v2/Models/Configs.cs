using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackupIt_daemon_v2.Models
{
    public class Config : IEquatable<Config>
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

        public bool Equals(Config other)
        {
            //Check whether the compared object is null.
            if (Config.ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (Config.ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the objects’ properties are equal.

            try
            {
                bool isIdEqual = Id.Equals(other.Id);

                //return isIdEqual && isPropertyXEqual && isPropertyYEqual && ...;
                return isIdEqual;
            }
            catch (System.NullReferenceException)
            {
                return false;
            }


        }

        public override int GetHashCode()
        {
            int hashTextual = name == null ? 0 : name.GetHashCode();

            // Get the hash code for the Digital field.
            int hashDigital = name.GetHashCode();

            // Calculate the hash code for the object.
            return hashDigital ^ hashTextual;
        }

    }
}
