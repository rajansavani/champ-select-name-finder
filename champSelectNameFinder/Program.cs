using System;
using System.Diagnostics;

namespace champSelectNameFinder
{
    class findTheNames
    {
        public static void Main(string[] args)
        {
            findTheNames find = new findTheNames();
        }

        public findTheNames()
        {
            Console.WriteLine(processList());
        }

        /*
         * sends argument to command line to return the process list for the UX process of the LCU
         * it then uses a regex search to find the password and port
         */
        public string processList() 
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("wmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            return (cmd.StandardOutput.ReadToEnd());
        }
    }  
}