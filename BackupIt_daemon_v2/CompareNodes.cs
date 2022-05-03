using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;
//using JsonDiffPatchDotNet;
using JsonDiffPatchDotNet.Formatters;
using Newtonsoft.Json.Linq;


namespace BackupIt_daemon_v2
{
    public class CompareNodes
    {

        //JsonDiffPatch jsonDiffPatch { get; set; } = new JsonDiffPatch();

        /*
        public void Compare(string oldJson, string newJson, string snapshopFilePath)
        {
            using (StreamWriter writer = new(snapshopFilePath))
            {
                writer.Write(jsonDiffPatch.Diff(oldJson, newJson));
            }
        }*/

        public void Compare(string leftPath, string rightPath)
        {
            List<FileSystemNode> leftNodes = JsonToList(leftPath);
            List<FileSystemNode> rightNodes = JsonToList(rightPath);

            List<FileSystemNode> added = CompareAdded(leftNodes, rightNodes);
            List<FileSystemNode> removed = CompareRemoved(leftNodes, rightNodes);
            List<FileSystemNode> modified = CompareModified(leftNodes, rightNodes);
        }

        public List<FileSystemNode> CompareAdded(List<FileSystemNode> leftNodes, List<FileSystemNode> rightNodes)
        {
            return rightNodes.Except(leftNodes).ToList();
        }

        public List<FileSystemNode> CompareRemoved(List<FileSystemNode> leftNodes, List<FileSystemNode> rightNodes)
        {
            return leftNodes.Except(rightNodes).ToList();
        }

        public List<FileSystemNode> CompareModified(List<FileSystemNode> leftNodes, List<FileSystemNode> rightNodes)
        {
            List<FileSystemNode> modified = new();

            foreach (FileSystemNode leftNode in leftNodes)
            {
                FileSystemNode rightNode = rightNodes.Find(x => x.FullPath == leftNode.FullPath); //TODO: What if finds nothing?

                DateTime rightDate = DateTime.ParseExact(rightNode.LastModified, "yyyyMMddHHmmss", null);
                DateTime leftDate = DateTime.ParseExact(leftNode.LastModified, "yyyyMMddHHmmss", null);
                
                if (DateTime.Compare(leftDate, rightDate) > 0)
                {
                    modified.Add(rightNode);
                }
            }

            return modified;
        }

        public List<FileSystemNode> JsonToList(string path)
        {
            string json;
            using (StreamReader sr = new(path))
            {
                json = sr.ReadToEnd();
            }

            JArray jArray = (JArray)JsonConvert.DeserializeObject(json);

            List<FileSystemNode> nodes = new();
            if (jArray is not null)
            {
                foreach (JToken item in jArray)
                {
                    nodes.Add(item.ToObject<FileSystemNode>());
                }
            }

            return nodes;
        }

        /*
        public bool Compare(string expected, string actual)
        {
            var expectedDoc = JsonConvert.DeserializeXmlNode(expected, "root");
            var actualDoc = JsonConvert.DeserializeXmlNode(actual, "root");
            var diff = new XmlDiff(XmlDiffOptions.IgnoreWhitespace |
                                   XmlDiffOptions.IgnoreChildOrder);
            using (var ms = new MemoryStream())
            using (var writer = new XmlTextWriter(ms, Encoding.UTF8))
            {
                var result = diff.Compare(expectedDoc, actualDoc, writer);
                if (!result)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Console.WriteLine(new StreamReader(ms).ReadToEnd());
                }
                return result;
            }
        }
        */
    }
}
