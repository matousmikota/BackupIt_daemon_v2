using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.XmlDiffPatch;
using Newtonsoft.Json;
using System.Xml;
//using JsonDiffPatchDotNet;
using JsonDiffPatchDotNet.Formatters;
using Newtonsoft.Json.Linq;


namespace BackupIt_daemon_v2
{
    public class CompareJson
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

            List<FileSystemNode> added = rightNodes.Except(leftNodes).ToList();
            List<FileSystemNode> removed = leftNodes.Except(rightNodes).ToList(); //TODO: Could do this in the foreach. When rightNodes.Find returns null.

            List<FileSystemNode> modified = new();

            foreach (FileSystemNode leftNode in leftNodes)
            {
                FileSystemNode rightNode = rightNodes.Find(x => x.FullPath == leftNode.FullPath);
                DateTime rightDate = DateTime.Parse(rightNode.LastModified);
                DateTime leftDate = DateTime.Parse(leftNode.LastModified);

                if (DateTime.Compare(leftDate, rightDate) > 0)
                {
                    modified.Add(rightNode);
                }
            }

        }

        private List<FileSystemNode> JsonToList(string path)
        {
            string json;
            using (StreamReader sr = new(path))
            {
                json = sr.ReadToEnd();
            }

            JArray jArray = (JArray)JsonConvert.DeserializeObject(json);

            List<FileSystemNode> nodes = new();
            foreach (JToken item in jArray)
            {
                nodes.Add(item.ToObject<FileSystemNode>());
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
