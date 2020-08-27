using System;
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

        private FileSystemWatcher watcher;

        private List<string> HandlingFiles = new List<string>();

        public DataSource(Game game)
        {
            this.game = game;
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
                        watcher.EnableRaisingEvents = false;
                        break;
                    }
                }
            }
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (HandlingFiles.Contains(e.FullPath)) { return; }

            Console.WriteLine($"[{DateTime.Now}] DataSource.OnChanged: {e.Name} {e.ChangeType}");
            Console.WriteLine($"[{DateTime.Now}] HandlingFiles.Count: {HandlingFiles.Count}");
            // Specify what is done when a file is changed, created, or deleted.
            if (e.Name.StartsWith("enemy"))
            {
                HandleEnemy(e.FullPath);
            }
        }

        private void HandleEnemy(string path)
        {
            HandlingFiles.Add(path);
            Console.WriteLine($"HandleEnemy: {path}");
            try
            {
                /*
                //List<byte> buffer = new List<byte>();
                byte[] buffer = new byte[1024];
                int i = 0;
                using (var stream = File.OpenRead(path))
                {
                    //while (true)
                    for (i = 0; i < buffer.Length; i++)
                    {
                        int bite = stream.ReadByte();
                        if (bite == -1)
                        {
                            //buffer[i] = 0;
                            break;
                        }
                        //buffer.Append((byte)bite);
                        //Console.WriteLine($"Bite: {bite} / {buffer.Count}");
                        buffer[i] = (byte)bite;
                    }
                    stream.Close();
                }

                string data = Encoding.Default.GetString(buffer, 0, i);
                */

                //Console.WriteLine($"buffer.Count: {buffer.Count}");

                byte[] buffer = new byte[1024];
                int i = 0;
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //while (true)
                    for (i = 0; i < buffer.Length; i++)
                    {
                        int bite = stream.ReadByte();
                        if (bite == -1)
                        {
                            //buffer[i] = 0;
                            break;
                        }
                        //buffer.Append((byte)bite);
                        //Console.WriteLine($"Bite: {bite} / {buffer.Count}");
                        buffer[i] = (byte)bite;
                    }
                }

                string data = Encoding.Default.GetString(buffer, 0, i);

                //string data = System.IO.File.ReadAllText(path);
                Console.WriteLine($"buffer.ToString(): {data}");
                //buffer.Clear();
                Enemy enemy = JsonSerializer.Deserialize<Enemy>(data);
                Console.WriteLine($"Enemy: {enemy.id}, {enemy.health} / {enemy.max_health}, {enemy.yaw}");
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
            }
            finally
            {
                HandlingFiles.Remove(path);
            }

            //File.Delete(path);
        }
    }
}
