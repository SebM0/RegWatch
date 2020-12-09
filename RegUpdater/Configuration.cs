using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegUpdater
{
    public partial class Configuration : Form
    {
        private ConfigurationHandler configurationHandler;
        private Point SavedLocation = new Point(-1, -1);
        private Size SavedSize;

        public Configuration(ConfigurationHandler configurationHandler)
        {
            this.configurationHandler = configurationHandler;
            InitializeComponent();

            this.Load += ResizeHeaders;
            this.ResizeEnd += ResizeHeaders;
            foreach ((string key, string value) in configurationHandler.GetRegistrySettings())
            {
                dataGridRegistry.Rows.Add(new string[] { key, value });
            }
            foreach (string process in configurationHandler.GetProcessSettings())
            {
                dataGridProcess.Rows.Add(new string[] { process });
            }
        }

        private void ResizeHeaders(object sender, EventArgs e)
        {
            int colWidth = (dataGridRegistry.Width - dataGridRegistry.RowHeadersWidth - 2) / 2;
            keyHeader.Width = colWidth;
            valueHeader.Width = colWidth;
        }

        private void save_Click(object sender, EventArgs e)
        {
            var regKeys = new Dictionary<string, string>();
            foreach (DataGridViewRow item in dataGridRegistry.Rows)
            {
                if (item.Cells[0].Value != null)
                    regKeys.Add((String)item.Cells[0].Value, (String)item.Cells[1].Value);
            }
            var processKeys = new List<string>();
            if (dataGridProcess.Rows.Count > 0)
            {
                foreach (DataGridViewRow item in dataGridProcess.Rows)
                {
                    if (item.Cells[0].Value != null)
                        processKeys.Add((String)item.Cells[0].Value);
                }
            }
            try
            {
                configurationHandler.SaveSettings(regKeys, processKeys);
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            if (SavedLocation.X != -1)
            {
                this.Location = SavedLocation;
                this.Size = SavedSize;
            }
            else
            {
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            }
        }

        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavedLocation = this.Location;
            SavedSize = this.Size;
        }
    }
}
