using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;

namespace VeeamTask.Testss
{
    [TestClass]
    public class ProcessMonitorTests
    {
        [TestMethod]
        public void TestProcessMonitor_KillsProcessAfterMaxLifetime()
        {
            // Arrange
            var processName = "notepad";
            var maxLifetime = 1;
            var monitoringFrequency = 1;
            Process process = Process.Start(processName);

            // Act
            var monitorThread = new Thread(() => StartMonitor(processName, maxLifetime, monitoringFrequency));
            monitorThread.Start();
            monitorThread.Join(TimeSpan.FromMinutes(2));

            // Assert
            Assert.IsTrue(process.HasExited);
        }

        [TestMethod]
        public void TestProcessMonitor_ContinuesMonitoringWhenNoProcessExceedsMaxLifetime()
        {
            // Arrange
            var processName = "notepad";
            var maxLifetime = 10;
            var monitoringFrequency = 1;

            // Act & Assert
            var monitorThread = new Thread(() => StartMonitor(processName, maxLifetime, monitoringFrequency));
            monitorThread.Start();
            Thread.Sleep(TimeSpan.FromMinutes(2));
        }

        [TestMethod]
        public void TestProcessMonitor_StopsWhenUserPressesSpecialKey()
        {
            // Arrange
            var processName = "notepad";
            var process = Process.Start(processName);
            var stopEvent = new ManualResetEvent(false);

            // Act
            var monitorThread = new Thread(() =>
            {
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
                    Thread.Sleep(100);
                }
                stopEvent.Set();
            });
            monitorThread.Start();

            Thread.Sleep(500);
            ConsoleSimulator.SendKeyPress('q');

            stopEvent.WaitOne(TimeSpan.FromSeconds(5));

            // Assert
            Assert.IsTrue(monitorThread.IsAlive);
        }

        private void StartMonitor(string processName, int maxLifetime, int monitoringFrequency)
        {
            string[] args = { processName, maxLifetime.ToString(), monitoringFrequency.ToString() };
            Program.Main(args);
        }
    }

    public static class ConsoleSimulator
    {
        public static void SendKeyPress(char key)
        {
            if (!Console.KeyAvailable)
            {
                Console.Write(key);
            }
        }
    }
}