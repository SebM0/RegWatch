using System.Windows.Forms;

namespace RegUpdater
{
    partial class Configuration
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.save = new System.Windows.Forms.Button();
            this.dataGridRegistry = new System.Windows.Forms.DataGridView();
            this.keyHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridProcess = new System.Windows.Forms.DataGridView();
            this.processName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridRegistry)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.save);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(752, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(48, 450);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // save
            // 
            this.save.BackgroundImage = global::RegUpdater.Properties.Resources.Save;
            this.save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.save.Location = new System.Drawing.Point(3, 3);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(41, 40);
            this.save.TabIndex = 2;
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // dataGridRegistry
            // 
            this.dataGridRegistry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridRegistry.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyHeader,
            this.valueHeader});
            this.dataGridRegistry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridRegistry.Location = new System.Drawing.Point(3, 3);
            this.dataGridRegistry.Name = "dataGridRegistry";
            this.dataGridRegistry.Size = new System.Drawing.Size(738, 418);
            this.dataGridRegistry.TabIndex = 3;
            // 
            // keyHeader
            // 
            this.keyHeader.HeaderText = "Registry key";
            this.keyHeader.Name = "keyHeader";
            // 
            // valueHeader
            // 
            this.valueHeader.HeaderText = "Registry value";
            this.valueHeader.Name = "valueHeader";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(752, 450);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridRegistry);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(744, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Registry";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridProcess);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(744, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Process";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridProcess
            // 
            this.dataGridProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.processName});
            this.dataGridProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridProcess.Location = new System.Drawing.Point(3, 3);
            this.dataGridProcess.Name = "dataGridProcess";
            this.dataGridProcess.Size = new System.Drawing.Size(738, 418);
            this.dataGridProcess.TabIndex = 0;
            // 
            // processName
            // 
            this.processName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.processName.HeaderText = "Process name";
            this.processName.Name = "processName";
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Configuration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Configuration_FormClosing);
            this.Load += new System.EventHandler(this.Configuration_Load);
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridRegistry)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProcess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.DataGridView dataGridRegistry;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueHeader;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView dataGridProcess;
        private DataGridViewTextBoxColumn processName;
    }
}

