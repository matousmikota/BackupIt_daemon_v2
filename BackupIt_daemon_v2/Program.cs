using JsonDiffer;
//using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
//using System.Xml;

namespace BackupIt_daemon_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            Snapshoter snapshot = new();
            snapshot.Create(@"C:\Source", @"C:\Users\a\Documents\BackupIt-daemon-snapshot\right.json");
            
            
            
            string leftJson;
            string rightJson;

            JsonReader newSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\snapshot.json"));
            JsonReader oldSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\oldsnapshot.json"));

            /*
            using (newSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\snapshot.json")))
            using (oldSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\oldsnapshot.json")))
            {
                
                //newSnapshotJr.Read();
                //oldSnapshotJr.Read();

                leftJson = newSnapshotJr.
                rightJson = oldSnapshotJr.ToString();
            }

            //XmlDocument newXml = JsonConvert.DeserializeXmlNode(newSnapshot);
            //XmlDocument oldXml = JsonConvert.DeserializeXmlNode(oldSnapshot);

            //CompareJson compareJson = new CompareJson();
            //compareJson.Compare(oldXml.ToString(), newXml.ToString());

            //compareJson.Compare(oldSnapshot, newSnapshot, @"C:\Users\a\Documents\BackupIt-daemon-snapshot\comparedsnapshot.json");



            /*
            var jdp = new JsonDiffPatch();
            JToken diffResult = jdp.Diff(leftJson, rightJson);
            */


            /*
            JToken jLeft = JToken.Load(oldSnapshotJr);
            JToken jRight = JToken.Load(newSnapshotJr);

            JToken diff = JsonDifferentiator.Differentiate(jLeft, jRight);

            Console.WriteLine(diff);
            */

            JsonReader jrLeft = new JsonTextReader(new StreamReader(@"C:\left.json"));
            JsonReader jrRight = new JsonTextReader(new StreamReader(@"C:\right.json"));

            JToken jtLeft = JToken.Load(jrLeft);
            JToken jtRight = JToken.Load(jrRight);

            JToken diff = JsonDifferentiator.Differentiate(jtLeft, jtRight);

            using (StreamWriter sw = new(@"C:\diff.json"))
            {
                sw.Write(diff);
            }


        }
    }
}
