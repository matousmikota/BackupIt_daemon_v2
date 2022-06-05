using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupIt_daemon_v2.Models;
using Ionic.Zip;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;

namespace BackupIt_daemon_v2
{
    public class Backuper
    {
        private string DataFolder { get; set; }
        private string Type { get; set; }
        private List<string> Sources { get; set; }
        private List<string> Destinations { get; set; }
        private bool FullExists { get; set; }
        private string LeftJsonPath { get; set; }
        private string RightJsonPath { get; set; }
        private int RetentionCount { get; set; }
        private bool Compress { get; set; }
        private int BackupsAfterFull { get; set; }
        private DateTime StartDate { get; set; }
        public Config Config { get; set; }
        
        public Backuper(string type, List<string> sources, List<string> destinations, bool fullExists, int retentionCount, bool compress, Config config)
        {
            this.Type = type;
            this.Sources = sources;
            this.Destinations = destinations;
            this.FullExists = fullExists;
            this.RetentionCount = retentionCount;
            this.Compress = compress;
            this.Config = config;

            this.DataFolder = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), typeof(Backuper).Namespace, Config.Id.ToString());
            this.LeftJsonPath = Path.Combine(this.DataFolder, "left.json");
            this.RightJsonPath = Path.Combine(this.DataFolder, "right.json");

            try
            {
                Directory.Delete(this.DataFolder, true);
            }
            catch (System.IO.DirectoryNotFoundException) { }
            
            Directory.CreateDirectory(this.DataFolder);
            this.StartDate = DateTime.Now;
        }

        public void Run()
        {
            switch (this.Type)
            {
                case "full":;
                    this.Full();
                    break;

                case "differential":;
                    this.Differential();
                    break;

                case "incremental":;
                    this.Incremental();
                    break;

                default:
                    this.Full();
                    break;
            }
        }
        private void Full()
        {
            FileStream fs = File.Create(Path.Combine(this.DataFolder, "left.json")); //Do I have to close this?
            fs.Close();
            Snapshoter snapshoter = new(this.Sources, Path.Combine(this.DataFolder, "right.json"));

            this.Backup(this.LeftJsonPath, this.RightJsonPath, "full");
        }
        private void Differential()
        {
            RetentionCheck();

            if (this.FullExists)
            {
                Snapshoter snapshoter = new(this.Sources, Path.Combine(this.DataFolder, "right.json"));
                this.Backup(this.LeftJsonPath, this.RightJsonPath, "differential");
                this.BackupsAfterFull++;
            }
            else
            {
                this.Full();
                this.FullExists = true;
            }
        }
        private void Incremental()
        {
            RetentionCheck();

            if (this.FullExists)
            {
                Snapshoter snapshoter = new(this.Sources, Path.Combine(this.DataFolder, "right.json"));
                this.Backup(this.LeftJsonPath, this.RightJsonPath, "incremental");
                this.BackupsAfterFull++;
            }
            else
            {
                this.Full();
                this.FullExists = true;
            }
        }

        //If there are multiple sources they will already be inside the json so I should not be dealing with that in here, it shold be dealt with inside Snapshoter.cs
        private void Backup(string leftPath, string rightPath, string type)
        {
            string dateTime = this.StartDate.ToString("yyyy-MM-dd-HH-mm_ss");
            string packageFolder = Path.Combine(type, dateTime);

            //Cannot copy to one folder first and then copy to the other ones because it might not have read permission to the destination folder
            CompareNodes compare = new();
            Metadata metadata = new(Path.Combine(this.DataFolder, "metadata.json"));

            List<FileSystemNode> leftNodes = compare.JsonToList(leftPath);
            List<FileSystemNode> rightNodes = compare.JsonToList(rightPath);
            
            List<FileSystemNode> added  = compare.CompareAdded(leftNodes, rightNodes);
            List<FileSystemNode> removed = compare.CompareRemoved(leftNodes, rightNodes);
            List<FileSystemNode> modified = compare.CompareModified(leftNodes, rightNodes);

            if (this.Compress)
            {
                string temp = Path.Combine(this.DataFolder, "temp");
                Directory.CreateDirectory(temp);
                BackupUtility(added, removed, modified, packageFolder, new List<string> { temp }, type);

                using (ZipFile zip = new())
                {
                    zip.AddDirectory(temp);

                    //foreach save to every destination
                    foreach (string destination in this.Destinations)
                    {
                        string path = Path.Combine(destination, type);
                        Directory.CreateDirectory(path);
                        zip.Save(Path.Combine(path, $"{dateTime}.zip"));
                    }
                }

                Directory.Delete(temp, true);
            }
            else if (!this.Compress)
            {
                BackupUtility(added, removed, modified, packageFolder, this.Destinations, type);
            }

            metadata.AddMetadata(added, "added"); //TODO: Remove static input. Maybe get the name of the list.
            metadata.AddMetadata(modified, "modified"); //TODO: Remove static input.
            metadata.AddMetadata(removed, "removed"); //TODO: Remove static input.

            //After everything is compleated make a final snapshot and save it as left.json
            //Snapshoter snapshoter = new(this.Destinations.First(), Path.Combine(this.DataFolder, "left.json")); //What if daemon can only write to destination but not read? Then it would not work.
            /*using (StreamWriter sw = new(Path.Combine(this.DataFolder, "left.json")))
            {
                sw.Write(JsonConvert.SerializeObject(rightNodes, Formatting.None));
            }*/
            if (type == "differential" && !this.FullExists)
            {
                leftNodes = rightNodes;
            }
            //differential left as final
            //incremental rigth as final
            //full empty as final
            switch (type)
            {
                case "full":;
                    File.Delete(Path.Combine(this.DataFolder, "left.json"));
                    using(StreamWriter sw = new(Path.Combine(this.DataFolder, "left.json")))
                    {
                        sw.Write(string.Empty);
                    }
                    break;

                case "differential":;
                    using (StreamWriter sw = new(Path.Combine(this.DataFolder, "left.json")))
                    {
                        sw.Write(JsonConvert.SerializeObject(leftNodes, Formatting.None)); //TODO: Do I have to actually even do this? Isn't it already like this?
                    }
                    break;

                case "incremental":;
                    using (StreamWriter sw = new(Path.Combine(this.DataFolder, "left.json")))
                    {
                        sw.Write(JsonConvert.SerializeObject(rightNodes, Formatting.None));
                    }
                    break;

                default:
                    File.Delete(Path.Combine(this.DataFolder, "left.json"));
                    using (StreamWriter sw = new(Path.Combine(this.DataFolder, "left.json")))
                    {
                        sw.Write(string.Empty);
                    }
                    break;
            }

            CreateLog(true, this.StartDate, DateTime.Now, "");
        }
        private string ShortenPath(string path)
        {
            string noLetterPath = path.Substring(Path.GetPathRoot(path).Length);

            return noLetterPath;
        }

        private void RetentionCheck()
        {
            if (this.BackupsAfterFull >= this.RetentionCount)
            {
                this.FullExists = false;
                this.BackupsAfterFull = 0;

                foreach (string destination in this.Destinations)
                {
                    Directory.Delete(destination, true);
                    Directory.CreateDirectory(destination);
                }
            }
        }

        private void BackupUtility(List<FileSystemNode> added, List<FileSystemNode> removed, List<FileSystemNode> modified, string packageFolder, List<string> destinations, string type)
        {
            //First create all folders (all nodes with IsDirectory = true)
            //then create all files (all nodes with IsDirectory = false)

            /*foreach (FileSystemNode node in added) //Folders have to be created before files are copied. That is why there have to be two foreach cycles. Actually maybe no because the folders I guess create automatically if they are missing? //an empty directory could have been added
            {
                if (node.IsDirectory)
                {
                    foreach (string destination in destinations)
                    {
                        Directory.CreateDirectory(Path.Combine(destination, packageFolder, ShortenPath(node.FullPath)));
                    }
                }
            }*/

            foreach (FileSystemNode node in added)
            {
                if (!node.IsDirectory)
                {
                    foreach (string destination in destinations)
                    {
                        //Directory.CreateDirectory(new FileInfo(node.FullPath).Directory.FullName);
                        File.Copy(node.FullPath, Path.Combine(destination, packageFolder, ShortenPath(node.FullPath)), true);
                    }
                }
            }

            foreach (FileSystemNode node in modified)
            {
                if (!node.IsDirectory) //Directories do not matter. And if I were to copy a directory it might copy its contents with it.
                {
                    foreach (string destination in destinations)
                    {
                        File.Copy(node.FullPath, Path.Combine(destination, packageFolder, ShortenPath(node.FullPath)), true);
                    }
                }
            }

            if (type == "full") //if it is not full it should not be touching the first full backup
            {
                foreach (FileSystemNode node in removed)
                {
                    foreach (string destination in destinations)
                    {
                        File.Delete(Path.Combine(destination, packageFolder, ShortenPath(node.FullPath)));
                    }
                }
            }
        }

        private void CreateLog(bool wasSuccessful, DateTime start, DateTime end, string errorCode)
        {
            API api = new();
            api.PostLog(new Log(api.GetLocalClientID(), this.Config.Id, wasSuccessful, start, end, errorCode));
        }

    }
}
