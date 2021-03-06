﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;
using System.Text.Json;
using System.Text.Json.Serialization;
using WfGaming.Models;
using System.Threading;

namespace WfGaming
{
    class DataSource
    {
        private Game game;

        public Player Player { get; set; }
        public Player Enemy { get; set; }

        public Mouse Mouse { get; set; }

        private FileSystemWatcher watcher;

        private readonly List<string> HandlingFiles = new List<string>();

        public static readonly string[] Headers = {
            "image", "health", "max_health", "yaw", "speed",
            "visible", "ship_visible", "burning", "flooding",
            "health2", "max_health2", "yaw2", "speed2",
            "visible2", "ship_visible2", "burning2", "flooding2",
            "forwarding", "turning", "firing", "zooming", "repairing", "x", "y",
            "\n"
        };

        public DataSource(Game game)
        {
            this.game = game;

            Reset();
        }

        public void Reset()
        {
            this.Player = new Player();
            this.Enemy = new Player();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run()
        {
            // Create a new FileSystemWatcher and set its properties.
            using (watcher = new FileSystemWatcher())
            {
                watcher.Path = game.ModDirectory;
                Console.WriteLine($"Watcher.Path: {watcher.Path}");

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.log";

                // Add event handlers.
                watcher.Changed += OnChanged;
                //watcher.Created += OnChanged;
                //watcher.Deleted += OnChanged;
                //watcher.Renamed += OnChanged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                while (true)
                {
                    try
                    {
                        Thread.Sleep(10);
                    }
                    catch (System.Threading.ThreadInterruptedException e)
                    {
                        Console.WriteLine("DataSource.Run: ThreadInterruptedException");
                        break;
                    }
                }
                watcher.EnableRaisingEvents = false;
            }
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (HandlingFiles.Contains(e.FullPath)) { return; }

            // Specify what is done when a file is changed, created, or deleted.
            if (e.Name.StartsWith("player"))
            {
                HandlePlayer(e.FullPath);
            }
            else if (e.Name.StartsWith("enemy"))
            {
                HandlePlayer(e.FullPath, true);
            }
            else if (e.Name.StartsWith("mouse"))
            {
                HandleMouse(e.FullPath);
            }
        }

        private void HandlePlayer(string path, bool isEnemy = false)
        {
            HandlingFiles.Add(path);

            try
            {
                byte[] buffer = new byte[1024];
                int i = 0;
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    for (i = 0; i < buffer.Length; i++)
                    {
                        int bite = stream.ReadByte();
                        if (bite == -1)
                        {
                            break;
                        }
                        buffer[i] = (byte)bite;
                    }
                }

                string data = Encoding.Default.GetString(buffer, 0, i);

                try
                {
                    if (isEnemy)
                    {
                        Enemy = JsonSerializer.Deserialize<Player>(data);
                        Console.WriteLine($"[{DateTime.Now}] Enemy: {Enemy.Health}");
                    }
                    else
                    {
                        Player = JsonSerializer.Deserialize<Player>(data);
                        Console.WriteLine($"[{DateTime.Now}] Player: {Player.Health}");
                    }
                }
                catch (System.Text.Json.JsonException e)
                {

                }
            }
            catch (Exception exception)
            {
                //Console.Error.WriteLine(exception.Message);
            }
            finally
            {
                HandlingFiles.Remove(path);
            }
        }

        private void HandleMouse(string path)
        {
            try
            {
                byte[] buffer = new byte[128];
                int i = 0;
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    for (i = 0; i < buffer.Length; i++)
                    {
                        int bite = stream.ReadByte();
                        if (bite == -1)
                        {
                            break;
                        }
                        buffer[i] = (byte)bite;
                    }
                }

                string data = Encoding.Default.GetString(buffer, 0, i);

                try
                {
                    Mouse = JsonSerializer.Deserialize<Mouse>(data);
                }
                catch (System.Text.Json.JsonException e)
                {

                }
                Console.WriteLine($"[{DateTime.Now}] Mouse: {Mouse.X},{Mouse.Y}");
            }
            catch (Exception exception)
            {
                
            }
            finally
            {

            }
        }

        public string GetPlayerCSV()
        {
            Player p = this.Player;
            string[] args = {
                p.Health.ToString(), p.MaxHealth.ToString(), p.Yaw.ToString(), p.Speed.ToString(),
                Convert.ToUInt16(p.IsVisible).ToString(), Convert.ToUInt16(p.IsShipVisible).ToString(),
                Convert.ToUInt16(p.IsVehicleBurning).ToString(), Convert.ToUInt16(p.IsVehicleFlooding).ToString()
            };
            return string.Join(",", args);
        }

        public string GetEnemyCSV()
        {
            Player e = this.Enemy;
            string[] args = {
                e.Health.ToString(), e.MaxHealth.ToString(), e.Yaw.ToString(), e.Speed.ToString(),
                Convert.ToUInt16(e.IsVisible).ToString(), Convert.ToUInt16(e.IsShipVisible).ToString(),
                Convert.ToUInt16(e.IsVehicleBurning).ToString(), Convert.ToUInt16(e.IsVehicleFlooding).ToString()
            };
            return string.Join(",", args);
        }
    }
}
