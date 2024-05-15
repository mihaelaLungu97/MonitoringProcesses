using System;
using System.Diagnostics;
using System.Threading;

namespace VeeamTask
{
    public class ProcessMonitor
    {
        public static void StopMonitor()
        {
            Console.WriteLine("Press 'q' to stop monitoring...");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);

                    if (key.KeyChar == 'q')
                    {
                        Console.WriteLine("Stopping the monitor...");
                        break;
                    }
                }
                Thread.Sleep(100); // Sleep for a short duration to avoid high CPU usage
            }

            Console.WriteLine("Monitoring stopped.");
        }

        public static void MonitorProcess(string processName, int maxLifetime, int monitoringFrequency)
        {
            while (true)
            {
                var processes = Process.GetProcessesByName(processName);

                foreach (Process process in processes)
                {
                    var processLifetime = DateTime.Now - process.StartTime;

                    if (processLifetime.TotalMinutes > maxLifetime)
                    {
                        Console.WriteLine($"Process {processName} has exceeded the maximum lifetime of {maxLifetime} minutes. Killing the process...");
                        process.Kill();
                    }
                }

                Thread.Sleep(monitoringFrequency * 60000); // Convert minutes to milliseconds
            }
        }
    }
}