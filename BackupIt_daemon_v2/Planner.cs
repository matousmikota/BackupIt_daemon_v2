using BackupIt_daemon_v2.Models;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BackupIt_daemon_v2
{
    public class Planner
    {
        const int Interval = 60 * 1000; // in miliseconds
        private List<Backuper> Backupers { get; set; }
        private List<Config> Configs { get; set; }
        private Timer Timer { get; set; }
        private DateTime BackupBegan { get; set; }

        public Planner()
        {
            this.Backupers = new();
            this.Configs = new();
            this.BackupBegan = DateTime.Now;
        }

        public void Run()
        {
            //timer that pauses everything for 60 seconds and then everything bellow is ran
            // this.Timer = new Timer(Start, null, 0, Interval); //should this be defined just once? In the constructor?

            
            // Start();

            this.Timer = new Timer(Start, null, 0, Interval); //should this be defined just once? In the constructor?
            Console.ReadKey();
            SaveState();
        }

        private void SaveState()
        {
            throw new NotImplementedException();
        }

        private void CheckForNewConfigs()
        {
            API api = new();
            Client updatedClient = api.GetClient(api.GetLocalMAC());
            List<Config> updatedConfigs = updatedClient.configs;

            // if a new Config is found then create a new Backuper instance and add it to this.Backupers and this.Configs

            //rightNodes.Except(leftNodes).ToList()

            List<Config> added = updatedConfigs.Except(this.Configs).ToList();
            //this foreach is the problem. The Backuper gets added again and overwrites the previous instance which had fullExists=true
            if (added is not null && added.Any())
            {
                foreach (Config config in added) //TODO: Should not it be the other way around (the List.Except)
                {
                    this.Configs.Add(config);
                    this.Backupers.Add(CreateBackuper(config));
                }
            }


            // if a Config is removed then remove it from this.Backupers and this.Configs


            List<Config> removed = this.Configs.Except(updatedConfigs).ToList();
            if (removed is not null && removed.Any())
            {
                foreach (Config config in removed) //TODO: Should not it be the other way around (the List.Except)
                {
                    this.Configs.Remove(this.Configs.Where(i => i.Id == config.Id).FirstOrDefault());
                    this.Backupers.Remove(this.Backupers.Where(i => i.Config.Id == config.Id).FirstOrDefault());
                }
            }

            try
            {
                
            }
            catch (System.ArgumentNullException)
            {

                // If there are no changes do nothing
            }

        }

        private void Start(object state)
        {
            this.BackupBegan = DateTime.Now;
            Debug.WriteLine("Planner.Start() Ran.");
            API api = new();
            api.Initialize();
            /*API api = new();
            Client client = api.GetClient(api.GetLocalMAC());
            List<Config> configs = client.configs;*/

            CheckForNewConfigs();
            DateTime time = SetSecondsZero(DateTime.Now);
            
            // List<GetJobDto> configs = await _repository.GetJobsAsync(client);
            if (this.Backupers is null || !this.Backupers.Any())
            {
                //api.PostLog(new Log(client.id, 0, false, DateTime.Now, DateTime.Now, "No configs assigned to this client. Please assign configs."));
                return;
            }

            // configs = configs.Where(x => x.Active).ToList();

            foreach (Backuper backuper in this.Backupers)
            {
                if (backuper.Config is null)
                {
                    // api.PostLog(new Log(client.id, 0, false, DateTime.Now, DateTime.Now, "No configs assigned to this client. Please assign configs."));
                    return;
                }

                DateTime nextOccurence = NextOccurence(time, backuper.Config.backup_cron);
                Debug.WriteLine(backuper.Config.backup_cron);
                Debug.WriteLine($"{time} - {nextOccurence}");

                if (CompareNextOccurence(time, nextOccurence))
                {
                    backuper.Run();
                }
            }
        }

        private bool CompareNextOccurence(DateTime time, DateTime nextOccurence)
        {
            return time.Year == nextOccurence.Year && time.Month == nextOccurence.Month && time.Day == nextOccurence.Day && time.Hour == nextOccurence.Hour && time.Minute == nextOccurence.Minute;
        }
        private DateTime NextOccurence(DateTime time, string cron)
        {
            return CrontabSchedule.TryParse(cron).GetNextOccurrence(new DateTime(time.Year, time.Month, time.Day, (time.Minute != 0) ? time.Hour : time.Hour - 1, (time.Minute != 0) ? time.Minute - 1 : 59, 0, 0));
        }

        private void Backup(Backuper backuper)
        {
            API api = new();

            // Backuper backuper = new(config.type, new List<string> { @"C:\Users\a\Music\source-testA" }, new List<string> { @"C:\Users\a\Videos\destinationA" }, false, 20, true);

            backuper.Run();
        }
        private DateTime SetSecondsZero(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
        }

        private Backuper CreateBackuper(Config config)
        {
            API api = new();
            List<Source> sources = api.GetSource(config.Id);
            List<Destinations> destinations = api.GetDestinations(config.Id);
            List<string> sourcesString = new();
            List<string> destinationsString = new();

            try
            {
                sources.ForEach(x => sourcesString.Add(x.path));
                destinations.ForEach(x => destinationsString.Add(x.path));
            }
            catch (System.NullReferenceException)
            {
                api.PostLog(new Log(api.GetLocalClientID(), config.Id, false, this.BackupBegan, DateTime.Now, "Current configs has either no sources or destinations assigned."));
            }


            Backuper backuper = new(config.type, sourcesString, destinationsString, false, config.max_count, config.compress_into_archive, config);

            return backuper;
        }
    }
}
