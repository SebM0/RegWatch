using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegUpdater
{
    public delegate void ConfigurationChange(ConfigurationHandler conf);

    public class ConfigurationHandler
    {
        public static readonly string PROCESSES = "PROCESSES";
        public static readonly string AWAKE = "AWAKE";

        public event ConfigurationChange ConfigurationChanged;
        public List<(string, string)> GetRegistrySettings()
        {
            var keys = new List<(string, string)>();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            foreach (string key in appSettings)
            {
                if (key.StartsWith("HKEY"))
                    keys.Add((key, appSettings.Get(key)));
            }
            return keys;
        }
        public List<string> GetProcessSettings()
        {
            var keys = new List<string>();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string processes = appSettings.Get(PROCESSES);
            if (processes != null)
            {
                foreach (string process in processes.Split(','))
                {
                    keys.Add(process);
                }
            }
            return keys;
        }

        public void SaveSettings(Dictionary<string, string> regKeys, List<string> procKeys)
        {
            // Validate
            List<string> badKeys = regKeys.Select(entry => entry.Key).Where(key => !key.StartsWith("HKEY")).ToList();
            if (badKeys.Count > 0)
                throw new InvalidOperationException("Unexpected registry keys " + string.Join(",", badKeys));
            var allProcesses = KeepProcess.ListFocusableProcesses();
            List<string> badProc = procKeys.Where(proc => !allProcesses.Contains(proc)).ToList();
            if (badProc.Count > 0)
                throw new InvalidOperationException("Unreachable process " + string.Join(",", badProc));

            // Save
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Clear();

            foreach (var entry in regKeys)
            {
                // Add an Application Setting.
                config.AppSettings.Settings.Add(entry.Key, entry.Value);
            }
            if (procKeys.Count > 0)
            {
                String processes = "";
                foreach (var entry in procKeys)
                {
                    if (processes.Length > 0)
                        processes += ",";
                    processes += entry;
                }
                config.AppSettings.Settings.Add(PROCESSES, processes);
            }
            // Save the changes in App.config file.
            config.Save(ConfigurationSaveMode.Modified);
            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");

            ConfigurationChanged(this);
        }
    }
}
