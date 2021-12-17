using RegUpdater.Properties;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace RegUpdater
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }
    }

    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private MenuItem awakeMenu;
        private ConfigurationHandler configurationHandler;
        private Configuration configuration;
        private RegWatcher regWatcher;
        private KeepAlive keepAlive;
        private KeepProcess keepProcess;
        private NewsChecker newsChecker;
        private int changes = 0;
        private String lastChangeTime = "";

        public MyCustomApplicationContext()
        {
            bool awake;
            try
            {
                awake = Boolean.Parse(ConfigurationManager.AppSettings.Get(ConfigurationHandler.AWAKE));
            }
            catch (ArgumentNullException)
            {
                awake = false;
            }
            awakeMenu = new MenuItem("Awake process", AwakeProcess);
            awakeMenu.Checked = awake;
            awakeMenu.Enabled = false;
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Configure", Configure),
                    new MenuItem("Apply registry now", Apply),
                    new MenuItem("Keep alive", KeepAlive),
                    awakeMenu,
                    new MenuItem("Check News", CheckNews),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true,
                Text = "Registry watcher"
            };
            trayIcon.DoubleClick += Configure;

            configurationHandler = new ConfigurationHandler();
            regWatcher = new RegWatcher(configurationHandler);
            regWatcher.ChangeDetected += OnChanged;
            configuration = new Configuration(configurationHandler);
            regWatcher.Start();
            keepAlive = new KeepAlive();
            keepProcess = new KeepProcess(configurationHandler);
            newsChecker = new NewsChecker();
            newsChecker.ChangeDetected += OnNews;
        }

        void Exit(object sender, EventArgs e)
        {
            regWatcher.Stop();
            keepAlive.Dispose();

            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
        void Configure(object sender, EventArgs e)
        {
            // If we are already showing the window, merely focus it.
            if (configuration.Visible)
            {
                configuration.Activate();
            }
            else
            {
                configuration.ShowDialog();
            }
        }

        void Apply(object sender, EventArgs e)
        {
            // Apply configuration.
            regWatcher.RestoreKeys();
        }

        void KeepAlive(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Checked)
            {
                keepAlive.Stop();
                awakeMenu.Enabled = false;
            } 
            else
            {
                keepAlive.Start();
                awakeMenu.Enabled = true;
            }
            menuItem.Checked = !menuItem.Checked;
        }

        void AwakeProcess(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Checked)
            {
                keepProcess.Stop();
                ConfigurationManager.AppSettings.Set(ConfigurationHandler.AWAKE, Boolean.FalseString);
            }
            else
            {
                keepProcess.Start();
                ConfigurationManager.AppSettings.Set(ConfigurationHandler.AWAKE, Boolean.TrueString);
            }
            menuItem.Checked = !menuItem.Checked;
        }

        void OnChanged()
        {
            changes++;
            lastChangeTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            String text = $"Changes: {changes}\r\nLast change: {lastChangeTime}";
            trayIcon.Text = $"Registry watcher\r\n{text}";
            trayIcon.ShowBalloonTip(5_000, "Registry Watcher", text, ToolTipIcon.Info);
        }

        void CheckNews(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Checked)
            {
                newsChecker.Stop();
            }
            else
            {
                newsChecker.Start();
            }
            menuItem.Checked = !menuItem.Checked;
        }

        void OnNews()
        {
            String newsTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            String text = $"News updated at: {newsTime}";
            trayIcon.Text = $"Registry watcher\r\n{text}";
            text += $"\r\n{newsChecker.visited}";
            trayIcon.ShowBalloonTip(5_000, "Registry Watcher", text, ToolTipIcon.Info);
        }
    }
}
