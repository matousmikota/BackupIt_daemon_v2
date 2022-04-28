using JsonDiffer;
using Newtonsoft.Json;
//using JsonDiffPatchDotNet;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;

namespace BackupIt_daemon_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CompareJson compareJson = new();
            compareJson.Compare(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\left.json", @"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\right.json");


            //API api = new API();

            //Console.WriteLine(api.LocalMAC());
            //api.Initialize();
            //api.GetClient("xcv");
            //api.PutClient(new Models.Client(444, "device444", "44", "4.4.4.4", DateTime.Now));


            /*
            Snapshoter snapshoter = new(@"C:\test-source", @"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\rightright.json");

            foreach (FileSystemNode item in snapshoter.Nodes)
            {
                Console.WriteLine(item.FullPath);
            }
            /*


            /*
            string jsonLeft = @"[{""FullPath"":""C:\\test-source"",""LastModified"":""20220419180343"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\100.docx"",""LastModified"":""20220323192311"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000"",""LastModified"":""20220324212925"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\added"",""LastModified"":""20220324212054"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\added\\added.pub"",""LastModified"":""20220324212052"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\added.accdb"",""LastModified"":""20220324212113"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\added2"",""LastModified"":""20220324212942"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\added2\\added2.accdb"",""LastModified"":""20220324212938"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\ahoj"",""LastModified"":""20220323192109"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\ahoj\\wefwef.pptx"",""LastModified"":""20220323192109"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\New Microsoft Publisher Document.pub"",""LastModified"":""20220323192446"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\we.bmp"",""LastModified"":""20220323192435"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\werwer.accdb"",""LastModified"":""20220323192113"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\www.rtf"",""LastModified"":""20220323192440"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\bruh"",""LastModified"":""20220328153814"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\bruh\\wejfoiweoifweoijf.docx"",""LastModified"":""20220328153813"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\New Bitmap image.bmp"",""LastModified"":""20220323192127"",""Action"":null,""IsDirectory"":false}]";
            string jsonRight = @"[{""FullPath"":""C:\\test-source"",""LastModified"":""20220419180624"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000"",""LastModified"":""20220324212925"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\added"",""LastModified"":""20220324212054"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\added\\added.pub"",""LastModified"":""20220324212052"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\added.accdb"",""LastModified"":""20220324212113"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\added2"",""LastModified"":""20220324212942"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\added2\\added2.accdb"",""LastModified"":""20220324212938"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\ahoj"",""LastModified"":""20220323192109"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\1000\\ahoj\\wefwef.pptx"",""LastModified"":""20220323192109"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\New Microsoft Publisher Document.pub"",""LastModified"":""20220323192446"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\we.bmp"",""LastModified"":""20220323192435"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\werwer.accdb"",""LastModified"":""20220323192113"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\1000\\www.rtf"",""LastModified"":""20220323192440"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\added"",""LastModified"":""20220419180605"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\added\\added"",""LastModified"":""20220419180603"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\added\\added.accdb"",""LastModified"":""20220419180553"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\added\\added.rtf"",""LastModified"":""20220419180559"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\added.pub"",""LastModified"":""20220419180620"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\bruh"",""LastModified"":""20220328153814"",""Action"":null,""IsDirectory"":true},{""FullPath"":""C:\\test-source\\bruh\\wejfoiweoifweoijf.docx"",""LastModified"":""20220328153813"",""Action"":null,""IsDirectory"":false},{""FullPath"":""C:\\test-source\\New Bitmap image.bmp"",""LastModified"":""20220323192127"",""Action"":null,""IsDirectory"":false}]";

            CompareJson compareJson = new CompareJson();

           

            compareJson.Compare(jsonLeft, jsonRight, @"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\diffOld.json");

            */





            /*
            JsonReader jrLeft = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\right.json"));
            JsonReader jrRight = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\rightright.json"));

            JToken jtLeft = JToken.Load(jrLeft);
            JToken jtRight = JToken.Load(jrRight);

            JToken diff = JsonDifferentiator.Differentiate(jtLeft, jtRight, OutputMode.Detailed, true);

            using (StreamWriter sw = new(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\newSnap\diffDetailed.json"))
            {
                sw.Write(diff);
            }

            
            string jsonString = diff.ToString();
            */



















            /*
            string leftJson;
            string rightJson;

            JsonReader newSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\snapshot.json"));
            JsonReader oldSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\oldsnapshot.json"));

            
            using (newSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\snapshot.json")))
            using (oldSnapshotJr = new JsonTextReader(new StreamReader(@"C:\Users\a\Documents\BackupIt-daemon-snapshot\oldsnapshot.json")))
            {
                
                //newSnapshotJr.Read();
                //oldSnapshotJr.Read();

                leftJson = newSnapshotJr.
                rightJson = oldSnapshotJr.ToString();
            }

            XmlDocument newXml = JsonConvert.DeserializeXmlNode(newSnapshot);
            XmlDocument oldXml = JsonConvert.DeserializeXmlNode(oldSnapshot);
            
            CompareJson compareJson = new CompareJson();
            compareJson.Compare(oldXml.ToString(), newXml.ToString());

            compareJson.Compare(oldSnapshot, newSnapshot, @"C:\Users\a\Documents\BackupIt-daemon-snapshot\comparedsnapshot.json");
            */


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














            /*

            JsonReader jrLeft = new JsonTextReader(new StreamReader(@"C:\right.json"));
            JsonReader jrRight = new JsonTextReader(new StreamReader(@"C:\rightright.json"));

            JToken jtLeft = JToken.Load(jrLeft);
            JToken jtRight = JToken.Load(jrRight);

            JToken diff = JsonDifferentiator.Differentiate(jtLeft, jtRight, OutputMode.Symbol, true);

            using (StreamWriter sw = new(@"C:\symboldiffwithoriginalrightright.json"))
            {
                sw.Write(diff);
            }

            */
            //string jsonString = diff.ToString();


















            /*
            string jsonLeft = @"{
  ""children"": [],
  ""name"": ""Source"",
  ""isFolder"": true,
  ""key"": ""source"",
  ""modified"": ""20220402221355"",
  ""fullPath"": ""C:\\Source""
}";
            string jsonRight = @"{
  ""children"": [
    {
      ""children"": [
        {
          ""children"": [],
          ""name"": ""text.txt"",
          ""isFolder"": false,
          ""key"": ""text.txt"",
          ""modified"": ""20220402221457"",
          ""fullPath"": ""C:\\Source\\Texts\\text.txt""
        }
      ],
      ""name"": ""Texts"",
      ""isFolder"": true,
      ""key"": ""texts"",
      ""modified"": ""20220402221501"",
      ""fullPath"": ""C:\\Source\\Texts""
    }
  ],
  ""name"": ""Source"",
  ""isFolder"": true,
  ""key"": ""source"",
  ""modified"": ""20220402221452"",
  ""fullPath"": ""C:\\Source""
}";

            string jsonDiff = "";

            string jsonNewdiff = @"{
  ""children"": {
    ""_t"": ""a"",
    ""0"": [
      {
        ""children"": [
          {
            ""children"": [],
            ""name"": ""text.txt"",
            ""isFolder"": false,
            ""key"": ""text.txt"",
            ""modified"": ""20220402221457"",
            ""fullPath"": ""C:\\Source\\Texts\\text.txt""
          }
        ],
        ""name"": ""Texts"",
        ""isFolder"": true,
        ""key"": ""texts"",
        ""modified"": ""20220402221501"",
        ""fullPath"": ""C:\\Source\\Texts""
      }
    ]
  },
  ""modified"": [
    ""20220402221355"",
    ""20220402221452""
  ]
}";

            Snapshot snapshot = System.Text.Json.JsonSerializer.Deserialize<Snapshot>(jsonNewdiff);

            



            CompareJson compareJson = new CompareJson();
            //compareJson.Compare(oldXml.ToString(), newXml.ToString());
            
            //compareJson.Compare(jsonLeft, jsonRight, @"C:\newdiff.json");

            */
























            /*
            string jsonsymboldiff = @"{
    ""*children"": [
      {
        ""*children"": [
          {
            ""*children"": [
              {
                ""+children"": [],
                ""+name"": ""New Microsoft Excel Worksheet.xlsx"",
                ""+isFolder"": false,
                ""+key"": ""newmicrosoftexcelworksheet.xlsx"",
                ""+modified"": ""20220403175942"",
                ""+fullPath"": ""C:\\Source\\jfweoif\\ertertert\\New Microsoft Excel Worksheet.xlsx""
              }
            ],
            ""*name"": ""ertertert"",
            ""*isFolder"": true,
            ""*key"": ""ertertert"",
            ""*modified"": ""20220403175942"",
            ""*fullPath"": ""C:\\Source\\jfweoif\\ertertert""
          },
          {
            ""+children"": [
              {
                ""children"": [
                  {
                    ""children"": [],
                    ""name"": ""New Microsoft Access Database.accdb"",
                    ""isFolder"": false,
                    ""key"": ""newmicrosoftaccessdatabase.accdb"",
                    ""modified"": ""20220403175916"",
                    ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\ertetertert\\New Microsoft Access Database.accdb""
                  },
                  {
                    ""children"": [],
                    ""name"": ""New Text Document.txt"",
                    ""isFolder"": false,
                    ""key"": ""newtextdocument.txt"",
                    ""modified"": ""20220403175919"",
                    ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\ertetertert\\New Text Document.txt""
                  },
                  {
                    ""children"": [
                      {
                        ""children"": [],
                        ""name"": ""New Microsoft Access Database.accdb"",
                        ""isFolder"": false,
                        ""key"": ""newmicrosoftaccessdatabase.accdb"",
                        ""modified"": ""20220403175922"",
                        ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\ertetertert\\wertwertwet\\New Microsoft Access Database.accdb""
                      }
                    ],
                    ""name"": ""wertwertwet"",
                    ""isFolder"": true,
                    ""key"": ""wertwertwet"",
                    ""modified"": ""20220403175922"",
                    ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\ertetertert\\wertwertwet""
                  }
                ],
                ""name"": ""ertetertert"",
                ""isFolder"": true,
                ""key"": ""ertetertert"",
                ""modified"": ""20220403175919"",
                ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\ertetertert""
              },
              {
                ""children"": [],
                ""name"": ""New Bitmap image.bmp"",
                ""isFolder"": false,
                ""key"": ""newbitmapimage.bmp"",
                ""modified"": ""20220403175837"",
                ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\New Bitmap image.bmp""
              },
              {
                ""children"": [],
                ""name"": ""New Microsoft Excel Worksheet.xlsx"",
                ""isFolder"": false,
                ""key"": ""newmicrosoftexcelworksheet.xlsx"",
                ""modified"": ""20220403175852"",
                ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\New Microsoft Excel Worksheet.xlsx""
              },
              {
                ""children"": [],
                ""name"": ""New Microsoft Word Document.docx"",
                ""isFolder"": false,
                ""key"": ""newmicrosoftworddocument.docx"",
                ""modified"": ""20220403175848"",
                ""fullPath"": ""C:\\Source\\jfweoif\\wefwef\\New Microsoft Word Document.docx""
              }
            ],
            ""+name"": ""wefwef"",
            ""+isFolder"": true,
            ""+key"": ""wefwef"",
            ""+modified"": ""20220403175908"",
            ""+fullPath"": ""C:\\Source\\jfweoif\\wefwef""
          }
        ],
        ""*name"": ""jfweoif"",
        ""*key"": ""jfweoif"",
        ""*modified"": ""20220403175901"",
        ""*fullPath"": ""C:\\Source\\jfweoif""
      },
      {
        ""+children"": [],
        ""+name"": ""New Bitmap image.bmp"",
        ""+isFolder"": false,
        ""+key"": ""newbitmapimage.bmp"",
        ""+modified"": ""20220403175946"",
        ""+fullPath"": ""C:\\Source\\New Bitmap image.bmp""
      },
      {
        ""+children"": [
          {
            ""children"": [
              {
                ""children"": [],
                ""name"": ""New folder"",
                ""isFolder"": true,
                ""key"": ""newfolder"",
                ""modified"": ""20220403180009"",
                ""fullPath"": ""C:\\Source\\New folder\\New folder\\New folder""
              }
            ],
            ""name"": ""New folder"",
            ""isFolder"": true,
            ""key"": ""newfolder"",
            ""modified"": ""20220403180009"",
            ""fullPath"": ""C:\\Source\\New folder\\New folder""
          }
        ],
        ""+name"": ""New folder"",
        ""+isFolder"": true,
        ""+key"": ""newfolder"",
        ""+modified"": ""20220403180005"",
        ""+fullPath"": ""C:\\Source\\New folder""
      },
      {
        ""+children"": [],
        ""+name"": ""New Microsoft Excel Worksheet.xlsx"",
        ""+isFolder"": false,
        ""+key"": ""newmicrosoftexcelworksheet.xlsx"",
        ""+modified"": ""20220403175957"",
        ""+fullPath"": ""C:\\Source\\New Microsoft Excel Worksheet.xlsx""
      }
    ],
    ""*modified"": ""20220403180012""
  }";


            Snapshot snapshot = System.Text.Json.JsonSerializer.Deserialize<Snapshot>(jsonsymboldiff);
            */





        }
    }
}
