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
using System.Diagnostics;

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
            try
            {
                var url = $"http://localhost:18788/data/Client/mac/{mac_address}";

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
            catch (System.Net.WebException)
            {
                //TODO: PostLog()
            }

            Client clientnull = null;
            return clientnull;
        }

        public int GetLocalClientID()
        {
            return GetClient(GetLocalMAC()).id;
        }

        public void PostClient(Client client)
        {
            var url = "http://localhost:18788/data/Client";

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
                Debug.WriteLine("Cannot post a client that has already been posted."); //should be written into metadata
            }
        }

        public void PutClient(Client client)
        {
            var url = $"http://localhost:18788/data/Client/{client.id}";

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

        private List<DConfigs> DeserializeDConfigs(string json)
        {
            return JsonConvert.DeserializeObject<List<DConfigs>>(json);
        }

        private List<Destinations> DeserializeDestinations(string json)
        {
            return JsonConvert.DeserializeObject<List<Destinations>>(json);
        }

        private List<Source> DeserializeSource(string json)
        {
            return JsonConvert.DeserializeObject<List<Source>>(json);
        }

        public void PostLog(Log log)
        {
            var url = "http://localhost:18788/data/Log";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "text/plain";
            httpRequest.ContentType = "application/json";

            string data = JsonConvert.SerializeObject(log);

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
                Debug.WriteLine("Cannot post a log that has already been posted."); //should be written into metadata
            }
        }
        
        public List<Config> GetConfigs(string client_mac)
        {
            Client client = GetClient(client_mac);
            return client.configs;
        }

        private List<DConfigs> GetDConfigs()
        {
            try
            {
                var url = $"http://localhost:18788/data/DConfigs";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                //httpRequest.Accept = "application/json";
                httpRequest.Accept = "text/plain";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    List<DConfigs> dConfigs = this.DeserializeDConfigs(streamReader.ReadToEnd());
                    return dConfigs;
                }
            }
            catch (System.Net.WebException)
            {
                //TODO: PostLog()
            }

            List<DConfigs> dConfigsnull = null;
            return dConfigsnull;
        }

        private List<Destinations> GetDestinations()
        {
            try
            {
                var url = $"http://localhost:18788/data/Destinations";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                //httpRequest.Accept = "application/json";
                httpRequest.Accept = "text/plain";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    List<Destinations> destinations = this.DeserializeDestinations(streamReader.ReadToEnd());
                    return destinations;
                }
            }
            catch (System.Net.WebException)
            {
                //TODO: PostLog()
            }

            List<Destinations> destinationsnull = null;
            return destinationsnull;
        }

        private List<Source> GetSource()
        {
            try
            {
                var url = $"http://localhost:18788/data/Source";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);

                //httpRequest.Accept = "application/json";
                httpRequest.Accept = "text/plain";

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    List<Source> source = this.DeserializeSource(streamReader.ReadToEnd());
                    return source;
                }
            }
            catch (System.Net.WebException)
            {
                //TODO: PostLog()
            }

            List<Source> sourcenull = null;
            return sourcenull;
        }

        public List<Source> GetSource(int config_id)
        {
            return GetSource().Where(x => x.config_id == config_id).ToList();
        }

        public List<Destinations> GetDestinations(int config_id)
        {
            List<Destinations> destinations = GetDestinations();
            List<DConfigs> dConfigs = GetDConfigs();
            List<Destinations> result = new();

            List<DConfigs> sortedDConfigs = dConfigs.Where(x => x.configid == config_id).ToList();

            foreach (DConfigs dConfig in sortedDConfigs)
            {
                result.Add(destinations.Where(x => x.id == dConfig.destinationid).FirstOrDefault());
            }

            return result;
        }
    }
}
