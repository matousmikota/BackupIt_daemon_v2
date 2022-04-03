using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.XmlDiffPatch;
using Newtonsoft.Json;
using System.Xml;
using JsonDiffPatchDotNet;
using JsonDiffPatchDotNet.Formatters;

namespace BackupIt_daemon_v2
{
    public class CompareJson
    {

        JsonDiffPatch jsonDiffPatch { get; set; } = new JsonDiffPatch();

        public void Compare(string oldJson, string newJson, string snapshopFilePath)
        {
            using (StreamWriter writer = new(snapshopFilePath))
            {
                writer.Write(jsonDiffPatch.Diff(oldJson, newJson));
            }
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
