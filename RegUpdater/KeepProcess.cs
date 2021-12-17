using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegUpdater
{
    class KeepProcess : RecurrentTool
    {
        int focus = 0;
        private List<string> processes;

        public KeepProcess(ConfigurationHandler configurationHandler)
        {
            configurationHandler.ConfigurationChanged += OnConfigurationChanged;
            Init(configurationHandler);
        }

        #region Imports
        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        #endregion
        private void Init(ConfigurationHandler configurationHandler)
        {
            var allProcesses = ListFocusableProcesses();
            processes = configurationHandler.GetProcessSettings().Where(proc => allProcesses.Contains(proc)).ToList();
            if (processes.Count == 1)
            {
                processes.Add("explorer"); // to cycle focus loss
            }
        }
        private void OnConfigurationChanged(ConfigurationHandler configurationHandler)
        {
            bool active = IsActive();
            if (active)
                Stop();
            Init(configurationHandler);
            if (active)
                Start();
        }

        private IntPtr FocusWindow()
        {
            string pname = processes.ElementAt(focus);
            Console.WriteLine("Try to awake " + pname);
            focus++;
            if (focus >= processes.Count)
                focus = 0;

            IEnumerable <IntPtr> query =
              from process in Process.GetProcesses()
              where process.ProcessName == pname && process.MainWindowHandle != null && process.MainWindowHandle.ToInt64() != 0
              select process.MainWindowHandle;
            return query.First();
        }

        protected override void Run()
        {
            if (processes.Count == 0)
                return;

            SetForegroundWindow(FocusWindow());

            /*            foreach (Process process in Process.GetProcesses())
                        {
                            if (process.MainWindowHandle != null && process.MainWindowHandle.ToInt64() != 0)
                            {
                                Console.WriteLine("Process with hwnd: " + process.ProcessName);
                                //SendMessage(mainWindowHandle, WM_SETTEXT, 0, "\t");
                                //SendMessage(mainWindowHandle, (uint)WindowsMessage.WM_SETFOCUS, new IntPtr(1), IntPtr.Zero);
                                if (process.ProcessName == "Teams")
                                {
                                    SetForegroundWindow(process.MainWindowHandle);
                                    Console.WriteLine("Try to awake Teams");
                                }
                            }
                        }*/
        }

        public static ICollection<string> ListFocusableProcesses()
        {
            return Process.GetProcesses().Where(process => process.MainWindowHandle != null && process.MainWindowHandle.ToInt64() != 0 && process.ProcessName != null)
                .Select(process => process.ProcessName).ToHashSet();
        }
    }
}
