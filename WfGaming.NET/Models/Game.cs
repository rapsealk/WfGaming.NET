using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WfGaming.Models
{
    class Game
    {
        DateTime LastBattleStartTime = DateTime.Now;
        DateTime LastBattleEndTime = DateTime.Now;

        string ModDirectory { get; set; } = string.Empty;
        private string BattleStartLog { get; } = "battle_start.log";
        private string BattleEndLog { get; } = "battle_end.log";

        public bool IsBattleStarted
        {
            get
            {
                string path = ModDirectory + $@"\{BattleStartLog}";
                DateTime fileLastWriteTime = File.GetLastWriteTime(path);
                if (fileLastWriteTime > LastBattleStartTime)
                {
                    LastBattleStartTime = fileLastWriteTime;
                    return true;
                }
                return false;
            }
        }

        public bool IsBattleEnded
        {
            get
            {
                string path = ModDirectory + $@"\{BattleEndLog}";
                DateTime fileLastWriteTime = File.GetLastWriteTime(path);
                if (fileLastWriteTime > LastBattleEndTime)
                {
                    LastBattleEndTime = fileLastWriteTime;
                    return true;
                }
                return false;
            }
        }

        public Game()
        {
            Initialize();
        }

        private void Initialize()
        {
            string clientPath = string.Empty;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                Console.WriteLine($"Drive: {drive.Name}, {drive.DriveType}");
                string path = drive.Name + @"Games\World_of_Warships_NA";
                if (Directory.Exists(path))
                {
                    clientPath = path;
                    break;
                }
            }

            if (clientPath.Equals(string.Empty))
            {
                Console.Error.WriteLine("Failed to find game path.");
                return;
            }
            Console.WriteLine($"Game path: {clientPath}");

            string buildPath = string.Empty;
            string[] buildDirectories = Directory.GetDirectories(clientPath + @"\bin");
            foreach (var buildDirectory in buildDirectories)
            {
                Console.WriteLine($"Build: {buildDirectory}");
                buildPath = buildDirectory;
            }

            string versionPath = string.Empty;
            string[] versionDirectories = Directory.GetDirectories(buildPath + @"\res_mods");
            foreach (var versionDirectory in versionDirectories)
            {
                Console.WriteLine($"Version: {versionDirectory}");
                versionPath = versionDirectory;
            }

            ModDirectory = versionPath + @"\PnFMods";

            string logPath = ModDirectory + @"\AutoMod\battle_start.txt";
            Console.WriteLine($"Log: {logPath}");
            DateTime datetime = File.GetLastWriteTime(logPath);
            Console.WriteLine($"DateTime: {datetime}");
        }
    }
}
