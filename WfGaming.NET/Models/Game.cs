using System;
using System.Linq;
using System.IO;

namespace WfGaming.Models
{
    class Game
    {
        public string Build { get; private set; }
        public string Version { get; private set; }

        DateTime LastBattleStartTime = DateTime.Now;
        DateTime LastBattleEndTime = DateTime.Now;

        public readonly string ModName = "AutoMod";
        public string ModRootDirectory { get; set; } = string.Empty;
        public string ModDirectory { get; set; } = string.Empty;
        private string BattleStartLog { get; } = "battle_start.log";
        private string BattleEndLog { get; } = "battle_end.log";

        public bool IsBattleStarted
        {
            get
            {
                string path = $@"{ModDirectory}\{BattleStartLog}";
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
                string path = $@"{ModDirectory}\{BattleEndLog}";
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
                string path = $@"{drive.Name}Games\World_of_Warships_NA";
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

                try
                {
                    Build = buildDirectory.Split('\\').Last();
                }
                catch (FormatException e)
                {
                    Console.Error.WriteLine(e.ToString());
                }
            }

            if (Build.StartsWith("27"))
            {
                ProcessBuild27(buildPath);
            }
            else if (Build.StartsWith("28"))
            {
                ProcessBuild28(buildPath);
            }
        }

        private void ProcessBuild27(string buildPath)
        {
            string[] versionDirectories = Directory.GetDirectories(buildPath + @"\res_mods");
            foreach (var versionDirectory in versionDirectories)
            {
                Console.WriteLine($"Version: {versionDirectory}");
                ModRootDirectory = versionDirectory;

                Version = versionDirectory.Split('\\').Last();
            }

            ModDirectory = $@"{ModRootDirectory}\PnFMods\{ModName}";
        }

        private void ProcessBuild28(string buildPath)
        {
            ModRootDirectory = $@"{buildPath}\res_mods";
            ModDirectory = $@"{ModRootDirectory}\PnFMods\{ModName}";
        }

        public void InstallMod(string modScriptPath)
        {
            File.Create($@"{ModRootDirectory}\PnFModsLoader.py");

            string pnfMods = $@"{ModRootDirectory}\PnFMods";
            if (!Directory.Exists(pnfMods))
            {
                Directory.CreateDirectory(pnfMods);
            }

            ModDirectory = $@"{pnfMods}\{ModName}";
            if (!Directory.Exists(ModDirectory))
            {
                Directory.CreateDirectory(ModDirectory);
            }

            string targetModPath = $@"{ModDirectory}\Main.py";

            File.Copy(modScriptPath, targetModPath, overwrite: true);
        }
    }
}
