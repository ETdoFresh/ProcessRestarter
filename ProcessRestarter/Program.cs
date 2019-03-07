using System;
using System.Diagnostics;
using System.IO;

namespace ProcessRestarter
{
    class Program
    {
        private static string programCSV = "programs.csv";
        private static bool csvHasHeaderRow = true;

        static void Main(string[] args)
        {
            var csv = ReadCSV();
            var lines = GetLines(csv);
            var processes = GetProcesses(lines);
            foreach (var process in processes)
                Restart(process);
        }

        private static string ReadCSV()
        {
            return File.ReadAllText(programCSV);
        }

        private static string[] GetLines(string csv)
        {
            return csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static ProcessInfo[] GetProcesses(string[] lines)
        {
            var startingI = csvHasHeaderRow ? 1 : 0;
            ProcessInfo[] processes = new ProcessInfo[lines.Length - startingI];
            for (int i = startingI; i < lines.Length; i++)
            {
                var line = lines[i];
                var fields = line.Split(',');
                var process = new ProcessInfo();
                process.executable = fields[0];
                Enum.TryParse(fields[1], out process.action);
                processes[i - startingI] = process;
            }
            return processes;
        }

        private static void Restart(ProcessInfo process)
        {
            foreach (var result in Process.GetProcessesByName(process.executable))
            {
                var executable = result.MainModule.FileName;
                if (process.action == Action.Restart)
                {
                    result.CloseMainWindow();
                    result.WaitForExit();
                    Process.Start(executable);
                }
                else if (process.action == Action.KillRestart)
                {
                    result.Kill();
                    Process.Start(executable);
                }
                else if (process.action == Action.Kill)
                {
                    result.Kill();
                }
            }
        }

        private enum Action { Restart, KillRestart, Kill }
        private class ProcessInfo
        {
            internal string executable;
            internal Action action;
        }
    }
}
