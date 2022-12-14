using System;
using System.Diagnostics;
using PoniLCU;
using static PoniLCU.LeagueClient;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace ChampSelectNameFinder
{
    public class Player
    {
        public string Name { get; set; }
    }

    public class Players
    {
        public List<Player> Participants { get; set; }
    }
    public class GetSummonerNames
    {
        public static Dictionary<int, string> PlayerList { get; set; } = new Dictionary<int, string>();
        public static Dictionary<string, string> Riot { get; set; } = new Dictionary<string, string>();
        public static Dictionary<string, string> Client { get; set; } = new Dictionary<string, string>();

        public static string opggURL = "https://www.op.gg/multisearch/na?summoners=";
        static void Main(string[] args)
        {
            GetLCU();
            Console.WriteLine(GetOPGG(MakeRequest("GET", "/chat/v5/participants/champ-select", false)));
        }

        static void GetLCU()
        {
            Riot.Clear();
            Client.Clear();

            var commandline = Cmd("LeagueClientUx.exe");
            Riot.Add("port", Findstring(commandline, "--riotclient-app-port=", "\" \"--no-rads"));
            Riot.Add("token", Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("riot:" + Findstring(commandline, "--riotclient-auth-token=", "\" \"--riotclient"))));

            Client.Add("port", Findstring(commandline, "--app-port=", "\" \"--install"));
            Client.Add("token", Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("riot:" + Findstring(commandline, "--remoting-auth-token=", "\" \"--respawn-command=LeagueClient.exe"))));

        }

        static string MakeRequest(string type, string url, bool client)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += delegate
                {
                    const bool validationResult = true;
                    return validationResult;
                };

                int port;
                string token;

                if(client)
                {
                    port = Convert.ToInt32(Client["port"]);
                    token = Client["token"];
                }
                else
                {
                    port = Convert.ToInt32(Riot["port"]);
                    token = Riot["token"];
                }

                var request = (HttpWebRequest)WebRequest.Create("https://127.0.0.1:" + port + url);
                request.PreAuthenticate = true;
                request.ContentType = "application/json";
                request.Method = type;
                request.Headers.Add("Authorization", "Basic " + token);

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch
            {
                Console.WriteLine("Request failed - Connection Error");
                return "";
            }
        }

        static string GetOPGG(string req)
        {
            PlayerList.Clear();
 
            var deserialized = JsonConvert.DeserializeObject<Players>(req);
            var count = 0;

            foreach (var player in deserialized.Participants)
            {
                count++;
                PlayerList.Add(count, player.Name);
                Console.WriteLine(player.Name);
                opggURL += player.Name + "%2C";
            }
            return opggURL;
        }
        static string Findstring(string text, string from, string to)
        {
            var pFrom = text.IndexOf(from, StringComparison.Ordinal) + from.Length;
            var pTo = text.LastIndexOf(to, StringComparison.Ordinal);

            return text.Substring(pFrom, pTo - pFrom);
        }

        static string Cmd(string gamename)
        {
            var commandline = "";
            var mngmtClass = new ManagementClass("Win32_Process");
            foreach (var managementBaseObject in mngmtClass.GetInstances())
            {
                var o = (ManagementObject)managementBaseObject;
                if (o["Name"].Equals(gamename))
                {
                    commandline = "[" + o["CommandLine"] + "]";
                }
            }
            return commandline;
        }
    }  
}