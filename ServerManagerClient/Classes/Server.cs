using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerManagerClient;

namespace ServerManagerClient.Classes
{
    class Server
    {
        public string ExecFilePath { get; set; }
        public string ProcessName { get; set; }

        private bool _isRunning;
        public Process StartServer(string procname, string[] args0)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = procname;
            if (args0.Length > 0)
            {
                foreach (var arg in args0)
                {
                    proc.StartInfo.Arguments += arg;
                }
            }

            proc.Start();
            _isRunning = true;
            Task.Run(() => IsServerRunning(proc, args0));

            return proc;
        }

        private void IsServerRunning(Process proc, string[] args0)
        {
            Process[] processes;
            for (; ; )
            {
                processes = Process.GetProcessesByName(proc.ProcessName);
                if (processes.Length == 0 && _isRunning)
                {
                    _isRunning = false;
                    Process proc0 = StartServer(proc.ProcessName, args0);
                    
                    //Send new ProcessID to DB - Placeholder
                    MainWindow._SPID = proc0;
                    break;
                }
            }
        }

        public void StopServer(Process proc)
        {
            _isRunning = false;
            proc.Kill();
        }
    }
}
