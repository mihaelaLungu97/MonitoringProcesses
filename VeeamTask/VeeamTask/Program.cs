using System;
using System.Threading.Tasks;

namespace VeeamTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: monitor.exe [processName] [maxLifetimeInMinutes] [monitoringFrequencyInMinutes]");
                return;
            }

            var processName = args[0];
            var maxLifetime = int.Parse(args[1]);
            var monitoringFrequency = int.Parse(args[2]);

            Task.Run(() => ProcessMonitor.MonitorProcess(processName, maxLifetime, monitoringFrequency));
            ProcessMonitor.StopMonitor();

            Console.WriteLine("Monitoring stopped.");
        }
    }
}