using System;
using System.Diagnostics;
using PoniLCU;
using static PoniLCU.LeagueClient;

namespace champSelectNameFinder
{
    class findTheNames
    {
        static LeagueClient leagueClient = new LeagueClient(credentials.lockfile);
        public static void Main(string[] args)
        {
            getData();
            Console.Read();
        }

        async static void getData()
        {
            var data = await leagueClient.Request(requestMethod.GET, "/lol-summoner/v1/current-summoner");
            Console.WriteLine(data);
        }
    }  
}