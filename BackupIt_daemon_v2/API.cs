using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using BackupIt_daemon_v2.Models;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace BackupIt_daemon_v2
{
    public class API
    {
        public void Initialize()
        {
            string mac = GetLocalMAC();

            Client client = GetClient(mac);

            if (client is not null)
            {
                client.last_seen = DateTime.Now;
                PutClient(client);
            }
            else
            {
                PostClient(new Client(GetLocalDeviceName(), mac, GetLocalIP(), DateTime.Now));
            }

        }

        public Client GetLocalClient()
        {
            string mac = GetLocalMAC();

            return GetClient(mac);
        }

        public string GetLocalMAC()
        {
            string addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return addr;
        }

        public string GetLocalIP()
        {
            string IP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                IP = endPoint.Address.ToString();
            }
            return IP;
        }

        public string GetLocalDeviceName()
        {
            return Environment.MachineName.ToString();
        }

        public Client GetClient(string mac_address)
        {
            var url = $"http://localhost:5000/data/Client/mac/{mac_address}";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            //httpRequest.Accept = "application/json";
            httpRequest.Accept = "text/plain";

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                Client client = this.DeserializeClient(streamReader.ReadToEnd());
                return client;
            }
        }

        public void PostClient(Client client)
        {
            var url = "http://localhost:5000/data/Client";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "text/plain";
            httpRequest.ContentType = "application/json";

            string data = JsonConvert.SerializeObject(client);

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (System.Net.WebException)
            {
                throw new System.Net.WebException("Cannot post a client that has already been posted."); //should be written into metadata
            }
        }

        public void PutClient(Client client)
        {
            var url = $"http://localhost:5000/data/Client/{client.id}";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "PUT";

            httpRequest.ContentType = "application/json";

            string data = JsonConvert.SerializeObject(client);

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (System.Net.WebException)
            {
                throw new System.Net.WebException("A client with the set ID does not exist."); //should be written into metadata
            }
        }

        private Client DeserializeClient(string json)
        {
            return JsonConvert.DeserializeObject<Client>(json);
        }

    }
}
