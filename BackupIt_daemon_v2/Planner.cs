﻿using BackupIt_daemon_v2.Models;
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
            //timer that pauses everything for 60 seconds and then Start is ran
            this.Timer = new Timer(Start, null, 0, Interval);
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
            List<Config> added = updatedConfigs.Except(this.Configs).ToList();
            if (added is not null && added.Any())
            {
                foreach (Config config in added)
                {
                    this.Configs.Add(config);
                    this.Backupers.Add(CreateBackuper(config));
                }
            }

            // if a Config is removed then remove it from this.Backupers and this.Configs
            List<Config> removed = this.Configs.Except(updatedConfigs).ToList();
            if (removed is not null && removed.Any())
            {
                foreach (Config config in removed)
                {
                    this.Configs.Remove(this.Configs.Where(i => i.Id == config.Id).FirstOrDefault());
                    this.Backupers.Remove(this.Backupers.Where(i => i.Config.Id == config.Id).FirstOrDefault());
                }
            }
        }

        private void Start(object state)
        {
            this.BackupBegan = DateTime.Now;
            API api = new();
            api.Initialize();

            CheckForNewConfigs();
            DateTime time = SetSecondsZero(DateTime.Now);
            
            if (this.Backupers is null || !this.Backupers.Any())
            {
                api.PostLog(new Log(api.GetLocalClientID(), 0, false, DateTime.Now, DateTime.Now, "No configs assigned to this client. Please assign configs."));
                return;
            }

            foreach (Backuper backuper in this.Backupers)
            {
                if (backuper.Config is null)
                {
                    api.PostLog(new Log(api.GetLocalClientID(), 0, false, DateTime.Now, DateTime.Now, "No configs assigned to this client. Please assign configs."));
                    return;
                }

                DateTime nextOccurence = NextOccurence(time, backuper.Config.backup_cron);

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
