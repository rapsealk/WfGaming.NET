using System;
using System.Diagnostics;
using System.Linq;

namespace WfGaming.Utils
{
    class ProcessManager
    {
        public static string TargetProcessName = "World of Warships";   // wgc

        public static bool IsGameProcessRunning()
        {
            Process[] processes = Process.GetProcesses();
            /*
            foreach (var p in processes)
            {
                if (p.ProcessName.StartsWith("W"))
                {
                    Console.WriteLine($"Process: {p.ProcessName}");
                }
            }
            */
            //Console.WriteLine($"{processes.Length} processes are running.");
            Console.WriteLine($"Target Process: {TargetProcessName}");
            Process process = processes.FirstOrDefault(p => p.ProcessName.Equals(TargetProcessName));
            Console.WriteLine($"Found process: {process?.ProcessName ?? "Null"}");
            return process != null;
        }
    }
}
