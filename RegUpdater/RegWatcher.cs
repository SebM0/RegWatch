using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Management;

namespace RegUpdater
{
    public delegate void Notify();

    public class RegWatcher
	{
        public event Notify ChangeDetected;
        private ManagementEventWatcher watcher;
        private List<(string, string)> registryKeys;

        public RegWatcher(ConfigurationHandler configurationHandler)
		{
            watcher = new ManagementEventWatcher();
            // Set up the delegate that will handle the change event.
            watcher.EventArrived += new EventArrivedEventHandler(HandleEvent);
            configurationHandler.ConfigurationChanged += OnConfigurationChanged;
            Init(configurationHandler);
            RestoreKeys();
        }

        private void Init(ConfigurationHandler configurationHandler)
        {
            registryKeys = configurationHandler.GetRegistrySettings();

            String queryWhere = "";
            foreach ((string key, string value) in registryKeys)
            {
                (string hive, string path, string variable)  = DecodeKey(key);
                if (hive == null)
                {
                    Console.WriteLine($"Unsupported key format {key}");
                    continue;
                }
                if (queryWhere.Length > 0)
                {
                    queryWhere += " OR ";
                }
                queryWhere += $"(Hive = '{hive}' AND KeyPath = '{path.Replace("\\", "\\\\")}' AND ValueName='{variable}')";
            }
            // Your query goes below; "KeyPath" is the key in the registry that you
            // want to monitor for changes. Make sure you escape the \ character.
            watcher.Query = new WqlEventQuery("SELECT * FROM RegistryValueChangeEvent WHERE " + queryWhere);
        }

        private (string hive, string path, string variable) DecodeKey(string key)
        {
            int hivePos = key.IndexOf('\\');
            if (hivePos < 0)
            {
                Console.WriteLine($"Unsupported key format {key}");
                return (null, null, null);
            }
            string hive = key.Substring(0, hivePos);
            string path = key.Substring(hivePos + 1);
            int valuePos = path.LastIndexOf('\\');
            if (valuePos < 0)
            {
                Console.WriteLine($"Unsupported key format {key}");
                return (null, null, null);
            }
            string value = path.Substring(valuePos + 1);
            path = path.Substring(0, valuePos);
            return (hive, path, value);
        }

        public void Start()
        {
            // Start listening for events.
            watcher.Start();
        }

        public void Stop()
        {
            // Stop listening for events.
            watcher.Stop();
        }

        private void OnConfigurationChanged(ConfigurationHandler configurationHandler)
        {
            Stop();
            Init(configurationHandler);
            RestoreKeys();
            Start();
        }

        private void HandleEvent(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("Received an event.");
            // RegistryKeyChangeEvent occurs here; do something.
            //ChangeDetected?.Invoke();
            ChangeDetected();
            Stop();
            RestoreKeys();
            Start();
        }

        internal void RestoreKeys()
        {
            foreach ((string key, string value) in registryKeys)
            {
                (string hive, string path, string variable) = DecodeKey(key);
                if (hive == null)
                {
                    Console.WriteLine($"Unsupported key format {key}");
                    continue;
                }
                using (RegistryKey myKey = Registry.LocalMachine.OpenSubKey(path, true)) { 
                    if (myKey != null)
                    {
                        RegistryValueKind valueKind = myKey.GetValueKind(variable);
                        switch (valueKind)
                        {
                            case RegistryValueKind.DWord:
                                myKey.SetValue(variable, UInt32.Parse(value), myKey.GetValueKind(variable));
                                break;
                            default:
                                myKey.SetValue(variable, value, myKey.GetValueKind(variable));
                                break;
                        }
                    }
                }
            }
        }
    }
}
