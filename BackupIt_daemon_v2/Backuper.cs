using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackupIt_daemon_v2
{
    public class Backuper
    {
        private string DataFolder { get; set; }
        private string Name { get; set; }
        private string Type { get; set; }
        private List<string> Sources { get; set; }
        private List<string> Destinations { get; set; }
        private bool FullExists { get; set; }
        private string LeftJsonPath { get; set; }
        private string RightJsonPath { get; set; }
        public Backuper(string name, string type, List<string> sources, List<string> destinations, bool fullExists)
        {
            this.Name = name;
            this.Type = type;
            this.Sources = sources;
            this.Destinations = destinations;
            this.FullExists = fullExists;

            this.DataFolder = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), typeof(Backuper).Namespace);
            this.LeftJsonPath = Path.Combine(this.DataFolder, "left.json");
            this.RightJsonPath = Path.Combine(this.DataFolder, "right.json");

            Directory.CreateDirectory(this.DataFolder);
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

            this.Backup(this.Destinations, this.LeftJsonPath, this.RightJsonPath, "Full");
        }
        private void Differential()
        {
            if (this.FullExists)
            {
                Snapshoter snapshoter = new(this.Sources, Path.Combine(this.DataFolder, "right.json"));
                this.Backup(this.Destinations, this.LeftJsonPath, this.RightJsonPath, Path.Combine("Differential", DateTime.Now.ToString("yyyy-MM-dd-HH-mm_ss")));
            }
            else
            {
                this.Full();
                this.FullExists = true;
            }
        }
        private void Incremental()
        {
            if (this.FullExists)
            {
                Snapshoter snapshoter = new(this.Sources, Path.Combine(this.DataFolder, "right.json"));
                this.Backup(this.Destinations, this.LeftJsonPath, this.RightJsonPath, Path.Combine("Incremental", DateTime.Now.ToString("yyyy-MM-dd-HH-mm_ss")));
            }
            else
            {
                this.Full();
                this.FullExists = true;
            }
        }

        //If there are multiple sources they will already be inside the json so I should not be dealing with that in here, it shold be dealt with inside Snapshoter.cs
        private void Backup(List<string> destinations, string leftPath, string rightPath, string packageFolder)
        {
            //Cannot copy to one folder first and then copy to the other ones because it might not have read permission to the destination folder
            CompareNodes compare = new();
            Metadata metadata = new(Path.Combine(this.DataFolder, "metadata.json"));

            List<FileSystemNode> leftNodes = compare.JsonToList(leftPath);
            List<FileSystemNode> rightNodes = compare.JsonToList(rightPath);
            
            List<FileSystemNode> added  = compare.CompareAdded(leftNodes, rightNodes);
            List<FileSystemNode> removed = compare.CompareRemoved(leftNodes, rightNodes);
            List<FileSystemNode> modified = compare.CompareModified(leftNodes, rightNodes);

            //First create all folders (all nodes with IsDirectory = true)
            //then create all files (all nodes with IsDirectory = false)

            foreach (FileSystemNode node in added) //Folders have to be created before files are copied. That is why there have to be two foreach cycles. Actually maybe no because the folders I guess create automatically if they are missing? //an empty directory could have been added
            {
                if (node.IsDirectory)
                {
                    foreach (string destination in destinations)
                    {
                        Directory.CreateDirectory(Path.Combine(destination, packageFolder, ShortenPath(node.FullPath)));
                    }
                }
            }

            foreach (FileSystemNode node in added)
            {
                if (!node.IsDirectory)
                {
                    foreach (string destination in destinations)
                    {
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

            if (this.Type=="full") //if it is not full it should not be touching the first full backup
            {
                foreach (FileSystemNode node in removed)
                {
                    foreach (string destination in destinations)
                    {
                        File.Delete(Path.Combine(destination, packageFolder, ShortenPath(node.FullPath)));
                    }
                }
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
            if (this.Type == "differential" && !this.FullExists)
            {
                leftNodes = rightNodes;
            }
            //differential left as final
            //incremental rigth as final
            //full empty as final
            switch (this.Type)
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

        }

        private string ShortenPath(string path)
        {
            string noLetterPath = path.Substring(Path.GetPathRoot(path).Length);

            return noLetterPath;
        }

    }
}
