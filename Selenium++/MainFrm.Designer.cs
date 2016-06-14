namespace Selenium_Recorder
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.mouseKeyEventProvider1 = new MouseKeyboardActivityMonitor.Controls.MouseKeyEventProvider();
            this.metroCommandGrid = new MetroFramework.Controls.MetroGrid();
            this.Run = new System.Windows.Forms.DataGridViewImageColumn();
            this.Command = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pattern = new System.Windows.Forms.DataGridViewImageColumn();
            this.metroContextMenu1 = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.qToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.metroContextMenu2 = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.metroPanel4 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroPanelBrowser = new MetroFramework.Controls.MetroPanel();
            this.metroPanel5 = new MetroFramework.Controls.MetroPanel();
            this.metroPanelSetting = new MetroFramework.Controls.MetroPanel();
            ((System.ComponentModel.ISupportInitialize)(this.metroCommandGrid)).BeginInit();
            this.metroContextMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mouseKeyEventProvider1
            // 
            this.mouseKeyEventProvider1.Enabled = true;
            this.mouseKeyEventProvider1.HookType = MouseKeyboardActivityMonitor.Controls.HookType.Application;
            this.mouseKeyEventProvider1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mouseKeyEventProvider1_KeyDown);
            // 
            // metroCommandGrid
            // 
            this.metroCommandGrid.AllowUserToResizeRows = false;
            this.metroCommandGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroCommandGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroCommandGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.metroCommandGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroCommandGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.metroCommandGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.metroCommandGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Run,
            this.Command,
            this.Target,
            this.Value,
            this.Pattern});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metroCommandGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.metroCommandGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroCommandGrid.EnableHeadersVisualStyles = false;
            this.metroCommandGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroCommandGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroCommandGrid.Location = new System.Drawing.Point(0, 60);
            this.metroCommandGrid.Name = "metroCommandGrid";
            this.metroCommandGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroCommandGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.metroCommandGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.metroCommandGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metroCommandGrid.Size = new System.Drawing.Size(481, 644);
            this.metroCommandGrid.TabIndex = 0;
            this.metroCommandGrid.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroCommandGrid.UseCustomBackColor = true;
            this.metroCommandGrid.UseStyleColors = true;
            this.metroCommandGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.metroCommandGrid_CellDoubleClick);
            this.metroCommandGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.metroCommandGrid_CellMouseClick);
            this.metroCommandGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.metroCommandGrid_RowsAdded);
            this.metroCommandGrid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.metroCommandGrid_UserDeletingRow);
            // 
            // Run
            // 
            this.Run.HeaderText = "Run";
            this.Run.Name = "Run";
            this.Run.Width = 30;
            // 
            // Command
            // 
            this.Command.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Command.HeaderText = "Command";
            this.Command.Name = "Command";
            // 
            // Target
            // 
            this.Target.HeaderText = "Target";
            this.Target.Name = "Target";
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Pattern
            // 
            this.Pattern.HeaderText = "Pattern";
            this.Pattern.Name = "Pattern";
            // 
            // metroContextMenu1
            // 
            this.metroContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qToolStripMenuItem,
            this.wToolStripMenuItem,
            this.aToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.metroContextMenu1.Name = "metroContextMenu1";
            this.metroContextMenu1.Size = new System.Drawing.Size(103, 92);
            // 
            // qToolStripMenuItem
            // 
            this.qToolStripMenuItem.Image = global::Selenium_Recorder.Properties.Resources.save;
            this.qToolStripMenuItem.Name = "qToolStripMenuItem";
            this.qToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.qToolStripMenuItem.Text = "Save";
            // 
            // wToolStripMenuItem
            // 
            this.wToolStripMenuItem.Image = global::Selenium_Recorder.Properties.Resources.load;
            this.wToolStripMenuItem.Name = "wToolStripMenuItem";
            this.wToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.wToolStripMenuItem.Text = "Load";
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aToolStripMenuItem.Image")));
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.aToolStripMenuItem.Text = "Reset";
            this.aToolStripMenuItem.Click += new System.EventHandler(this.aToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Selenium_Recorder.Properties.Resources.exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroContextMenu2
            // 
            this.metroContextMenu2.BackColor = System.Drawing.SystemColors.Control;
            this.metroContextMenu2.Name = "metroContextMenu2";
            this.metroContextMenu2.ShowImageMargin = false;
            this.metroContextMenu2.Size = new System.Drawing.Size(36, 4);
            this.metroContextMenu2.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.metroContextMenu2_ItemClicked);
            // 
            // metroPanel4
            // 
            this.metroPanel4.BackgroundImage = global::Selenium_Recorder.Properties.Resources.opera;
            this.metroPanel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.metroPanel4.HorizontalScrollbarBarColor = true;
            this.metroPanel4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel4.HorizontalScrollbarSize = 10;
            this.metroPanel4.Location = new System.Drawing.Point(369, 27);
            this.metroPanel4.Name = "metroPanel4";
            this.metroPanel4.Size = new System.Drawing.Size(25, 25);
            this.metroPanel4.TabIndex = 2;
            this.metroPanel4.VerticalScrollbarBarColor = true;
            this.metroPanel4.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel4.VerticalScrollbarSize = 10;
            this.metroPanel4.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.metroPanelBrowser_MouseDoubleClick);
            // 
            // metroPanel3
            // 
            this.metroPanel3.BackgroundImage = global::Selenium_Recorder.Properties.Resources.safari;
            this.metroPanel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(338, 27);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(25, 25);
            this.metroPanel3.TabIndex = 2;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            this.metroPanel3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.metroPanelBrowser_MouseDoubleClick);
            // 
            // metroPanel2
            // 
            this.metroPanel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("metroPanel2.BackgroundImage")));
            this.metroPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(307, 27);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(25, 25);
            this.metroPanel2.TabIndex = 2;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            this.metroPanel2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.metroPanelBrowser_MouseDoubleClick);
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackgroundImage = global::Selenium_Recorder.Properties.Resources.chrome;
            this.metroPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(276, 27);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(25, 25);
            this.metroPanel1.TabIndex = 2;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            this.metroPanel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.metroPanel1_MouseDoubleClick);
            // 
            // metroPanelBrowser
            // 
            this.metroPanelBrowser.BackgroundImage = global::Selenium_Recorder.Properties.Resources.firefox;
            this.metroPanelBrowser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.metroPanelBrowser.HorizontalScrollbarBarColor = true;
            this.metroPanelBrowser.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelBrowser.HorizontalScrollbarSize = 10;
            this.metroPanelBrowser.Location = new System.Drawing.Point(245, 27);
            this.metroPanelBrowser.Name = "metroPanelBrowser";
            this.metroPanelBrowser.Size = new System.Drawing.Size(25, 25);
            this.metroPanelBrowser.TabIndex = 2;
            this.metroPanelBrowser.VerticalScrollbarBarColor = true;
            this.metroPanelBrowser.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelBrowser.VerticalScrollbarSize = 10;
            this.metroPanelBrowser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.metroPanelBrowser_MouseDoubleClick);
            // 
            // metroPanel5
            // 
            this.metroPanel5.BackgroundImage = global::Selenium_Recorder.Properties.Resources.layout;
            this.metroPanel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.metroPanel5.HorizontalScrollbarBarColor = true;
            this.metroPanel5.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel5.HorizontalScrollbarSize = 10;
            this.metroPanel5.Location = new System.Drawing.Point(179, 27);
            this.metroPanel5.Name = "metroPanel5";
            this.metroPanel5.Size = new System.Drawing.Size(25, 25);
            this.metroPanel5.TabIndex = 1;
            this.metroPanel5.VerticalScrollbarBarColor = true;
            this.metroPanel5.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel5.VerticalScrollbarSize = 10;
            this.metroPanel5.MouseClick += new System.Windows.Forms.MouseEventHandler(this.metroPanelSetting_MouseClick);
            // 
            // metroPanelSetting
            // 
            this.metroPanelSetting.BackgroundImage = global::Selenium_Recorder.Properties.Resources.setting;
            this.metroPanelSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.metroPanelSetting.HorizontalScrollbarBarColor = true;
            this.metroPanelSetting.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanelSetting.HorizontalScrollbarSize = 10;
            this.metroPanelSetting.Location = new System.Drawing.Point(210, 27);
            this.metroPanelSetting.Name = "metroPanelSetting";
            this.metroPanelSetting.Size = new System.Drawing.Size(25, 25);
            this.metroPanelSetting.TabIndex = 1;
            this.metroPanelSetting.VerticalScrollbarBarColor = true;
            this.metroPanelSetting.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanelSetting.VerticalScrollbarSize = 10;
            this.metroPanelSetting.MouseClick += new System.Windows.Forms.MouseEventHandler(this.metroPanelSetting_MouseClick);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 704);
            this.Controls.Add(this.metroPanel5);
            this.Controls.Add(this.metroPanel4);
            this.Controls.Add(this.metroPanel3);
            this.Controls.Add(this.metroPanel2);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.metroPanelBrowser);
            this.Controls.Add(this.metroPanelSetting);
            this.Controls.Add(this.metroCommandGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Style = MetroFramework.MetroColorStyle.Magenta;
            this.Text = "Selenium++";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroCommandGrid)).EndInit();
            this.metroContextMenu1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MouseKeyboardActivityMonitor.Controls.MouseKeyEventProvider mouseKeyEventProvider1;
        private MetroFramework.Controls.MetroGrid metroCommandGrid;
        private MetroFramework.Controls.MetroPanel metroPanelSetting;
        private MetroFramework.Controls.MetroContextMenu metroContextMenu1;
        private System.Windows.Forms.ToolStripMenuItem qToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridViewImageColumn Run;
        private System.Windows.Forms.DataGridViewComboBoxColumn Command;
        private System.Windows.Forms.DataGridViewTextBoxColumn Target;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewImageColumn Pattern;
        private MetroFramework.Controls.MetroPanel metroPanelBrowser;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private MetroFramework.Controls.MetroContextMenu metroContextMenu2;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private MetroFramework.Controls.MetroPanel metroPanel4;
        private MetroFramework.Controls.MetroPanel metroPanel5;

    }
}

