using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WfGaming.Models
{
    class ShellInfo
    {
        public Int64 timestamp { get; }
        public Int64 player_ship_id { get; }
        public Int64 victim { get; }
        public Int64 shooter { get; }
        public Int64 damage { get; }
    }

    /*
    using System.IO;
    using System.Security.Permissions;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private static void Run()
    {
        string[] args = Environment.GetCommandLineArgs();

        // Create a new FileSystemWatcher and set its properties.
        using (FileSystemWatcher watcher = new FileSystemWatcher())
        {
            watcher.Path = "F:\\Games\\World_of_Warships_ASIA\\res_mods\\0.9.5.1\\PnFMods\\RLMod";

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            // Only watch text files.
            watcher.Filter = "shell_info.log";

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press 'q' to quit the sample.");
            while (Console.Read() != 'q') ;
        }
    }

    // Define the event handlers.
    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        // Specify what is done when a file is changed, created, or deleted.
        //Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        try
        {
            string text = System.IO.File.ReadAllText(e.FullPath);
            ShellInfo info = JsonSerializer.Deserialize<ShellInfo>(text);
            Console.WriteLine($"Time: {(Int64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds}, player_ship_id: {info.player_ship_id}, victim: {info.victim}, damage: {info.damage}");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception.Message);
        }
    }

    private static void OnRenamed(object source, RenamedEventArgs e) =>
        // Specify what is done when a file is renamed.
        Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
    */
}
