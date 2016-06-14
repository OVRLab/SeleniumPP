using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//////////////////////////////
using MetroFramework;
using MetroFramework.Forms;
using MetroFramework.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using computer_assisted_instruction;
//////////////////////////////
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace Selenium_Recorder
{
    public partial class MainFrm :  MetroForm
    {
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;
        const int SWP_FRAMECHANGED = 0x0020;
        const int SW_SHOWMAXIMIZED = 3;
        
        string default_browser = "firefox";//chrome
        Selenium_Interface.IWebDriver inter;
        CommandTable command_table = new CommandTable();
        
        int rowIndex = 0, ColumnIndex = 0;
        
        ////////////////////////////////////////////////
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public MainFrm()
        {
            InitializeComponent();
            InitUI();
            InitValues();
        }

        private void InitUI()
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width / 4;
            this.Height = Screen.PrimaryScreen.Bounds.Height - 45;
            metroCommandGrid.Columns[0].Width = 30;
            metroCommandGrid.Columns[0].HeaderText = "";
            metroCommandGrid.Rows[0].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.play;
            metroCommandGrid.Rows[0].Cells[4].Value = global::Selenium_Recorder.Properties.Resources.snapshot;
            this.Command.Items.Clear();
            this.Command.Items.Add("WD.Run");
            this.Command.Items.AddRange(Command_SK.sk_type);
            this.Run.ImageLayout = DataGridViewImageCellLayout.Zoom;
            this.Pattern.ImageLayout = DataGridViewImageCellLayout.Zoom;
            metroPanelSetting.Location = new Point(this.Width - 40, metroPanelSetting.Location.Y);
            metroPanelBrowser.Location = new Point(metroPanelSetting.Location.X - 40, metroPanelSetting.Location.Y);
            metroCommandGrid.Columns[1].Width = this.Width / 5;
            metroCommandGrid.Columns[2].Width = (int)(this.Width / 5);
            metroCommandGrid.Columns[3].Width = (int)(this.Width / 5);
            metroCommandGrid.Columns[4].Width = (int)(this.Width / 5);
        }

        private void InitValues()
        {
            metroCommandGrid.Rows.Clear();
            command_table.Clear();
            //object[] row = new object[5] { null, "WD.Run", default_browser, "", null };
            Command_WD c = new Command_WD();
            c.TextCommand = "WD.Run";
            target_command_pair pair = new target_command_pair();
            pair.Target = default_browser;
            c.baseURL = default_browser;
            pair.CSharpCommand = "";
            c.target_command_list.Add(pair);
            c.Value = "";
            command_table.SetCommand(c,0);
            command_table.UpdateLastRow2Grid(metroCommandGrid);
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            inter = new Selenium_Interface.IWebDriver(this, AppendCommand_WD, 7777);
            inter.Start();
        }

        public void AppendCommand_WD(string data)
        {
            string[] commands = data.Split(new string[1] { "#$#" }, StringSplitOptions.None);
            //baseURL + command + target + value + lastURL + csharp_command
            string baseURL = commands[0];
            string TextCommand = "WD."+commands[1];
            string Target = commands[2];
            string Value = commands[3];
            string lastURL = commands[4];
            string CSharpCommands = commands[5];

            if (!this.Command.Items.Contains(TextCommand))
                this.Command.Items.Add(TextCommand);
            Command_WD c = new Command_WD();
            c.TextCommand = TextCommand;
            c.baseURL = baseURL;
            c.lastURL = lastURL;
            c.Value = Value;
            string[] commands_splited = CSharpCommands.Split(new string[1] { "#@#" }, StringSplitOptions.RemoveEmptyEntries);
            string[] target_splited = Target.Split(new string[1] { "#@#" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < commands_splited.Length; i++)
            {
                target_command_pair tc = new target_command_pair();
                tc.Target = target_splited[i];
                tc.CSharpCommand = commands_splited[i];
                c.target_command_list.Add(tc);
            }
            c.selectedTargetIndex = commands_splited.Length - 1;
            command_table.SetCommand(c);
            command_table.UpdateLastRow2Grid(metroCommandGrid);
        }

        
        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //iDesktop.Close();
            inter.Stop();
        }

        private void metroCommandGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //metroCommandGrid.Rows[e.RowIndex].Cells[0].ToolTipText = (metroCommandGrid.Rows.Count - 1).ToString();
            metroCommandGrid.Rows[e.RowIndex].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.play;
            metroCommandGrid.Rows[e.RowIndex].Cells[4].Value = global::Selenium_Recorder.Properties.Resources.snapshot;
        }

        private void metroCommandGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //Execute Command
                ((Command_WD)command_table.commands[0]).baseURL = metroCommandGrid.Rows[0].Cells[2].Value.ToString();
                metroCommandGrid.Rows[e.RowIndex].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.pause;
                metroCommandGrid.Refresh();
                if(command_table.ExecuteCommand(e.RowIndex))
                    metroCommandGrid.Rows[e.RowIndex].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.success;
                else
                    metroCommandGrid.Rows[e.RowIndex].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.fail;
                metroCommandGrid.Refresh();
            }
            else if (e.ColumnIndex == 4)
            {
                //Take a snapshot as pattern
                //iDesktop.Find(@"C:\patterns\CloseChrome.png", new Point(5, 5), 0.9, true);
                if (e.RowIndex < metroCommandGrid.Rows.Count &&
                    metroCommandGrid.Rows[e.RowIndex].Cells[1].Value != null &&
                    metroCommandGrid.Rows[e.RowIndex].Cells[1].Value.ToString().StartsWith("IP."))
                {
                    rowIndex = e.RowIndex;
                    ColumnIndex = e.ColumnIndex;
                    new Snapshot().Snap(SnapBack);
                }
                else if (e.RowIndex < metroCommandGrid.Rows.Count)
                {
                    metroCommandGrid.Rows[e.RowIndex].Cells[1].Selected = true;
                }
            }
        }

        public void SnapBack(Bitmap snap)
        {
            //snap.Save(patterns_path + rowIndex + ".png");
            //Resize
            //Set to Col & Index
            metroCommandGrid.Rows[rowIndex].Cells[4].Value = snap;
            Pattern p = new Pattern(snap);
            command_table.SetCommand(new Command_SK(metroCommandGrid.Rows[rowIndex].Cells[1].Value.ToString(), p), rowIndex);
        }
             
        private void metroPanelSetting_MouseClick(object sender, MouseEventArgs e)
        {
            //Menu
            metroContextMenu1.Show(new Point(metroPanelSetting.Location.X + (metroPanelSetting.Size.Width/2),
                                        metroPanelSetting.Location.Y + (metroPanelSetting.Size.Height / 2)));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Close
            Application.Exit();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Clear Var
            InitValues();
        }

        private void metroCommandGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //Delete
            if (e.Row.Index == 0)
                e.Cancel = true;
            else
            {
                command_table.Delete(e.Row.Index);
            }
        }

        private void metroPanelBrowser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            metroCommandGrid.Rows[0].Cells[2].Value = "firefox";
            //Process[] p = Process.GetProcessesByName("firefox");
            //if (p.Length == 0)
            //{
            //    Process.Start(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe");
            //    Thread.Sleep(2000);
            //    p = Process.GetProcessesByName("firefox");
            //}
            //for (int i = 0; i < p.Length; i++)
            //{
            //    IntPtr handle = p[i].MainWindowHandle;
            //    int Width = (Screen.PrimaryScreen.Bounds.Width / 4) * 3;
            //    int Height = Screen.PrimaryScreen.Bounds.Height - 45;
            //    if (handle != IntPtr.Zero)
            //    {
            //        ShowWindow(handle, SW_SHOWMAXIMIZED);
            //        Thread.Sleep(200);
            //        SetWindowPos(handle, 0, this.Width + 1, 0, Width, Height, 0);
            //        metroPanelBrowser.BackgroundImage = global::Selenium_Recorder.Properties.Resources.firefox_blue;
            //    }
            //}
        }
        private void metroPanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            metroCommandGrid.Rows[0].Cells[2].Value = "chrome";
        }

        private void metroCommandGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 &&
                e.Button == System.Windows.Forms.MouseButtons.Right &&
                e.RowIndex >= 0 && e.RowIndex < command_table.Count())
            {
                //Show Result Msg
                metroContextMenu2.Items.Clear();
                metroContextMenu2.Items.Add(command_table.GetCommandbyIndex(e.RowIndex).Result);
                metroContextMenu2.Show((Control)sender, e.Location);
            }
            else if (e.ColumnIndex == 2 && e.Button == System.Windows.Forms.MouseButtons.Right &&
                e.RowIndex >= 0 && e.RowIndex < command_table.Count())
            {
                //Select diffrent Target
                if(metroCommandGrid.Rows[e.RowIndex].Cells[1].Value.ToString().StartsWith("WD."))
                {
                    metroContextMenu2.Items.Clear();
                    List<target_command_pair> target_sum =  ((Command_WD)command_table.GetCommandbyIndex(e.RowIndex)).target_command_list;
                    for (int i = 0; i < target_sum.Count; i++)
                        metroContextMenu2.Items.Add(target_sum[i].Target);
                    rowIndex = e.RowIndex;
                    ColumnIndex = e.ColumnIndex;
                    metroContextMenu2.Show((Control)sender,e.Location);
                }
            }
        }

        private void metroContextMenu2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (metroCommandGrid.Rows[rowIndex].Cells[ColumnIndex].Value.ToString() != e.ClickedItem.Text)
            {
                metroCommandGrid.Rows[rowIndex].Cells[ColumnIndex].Value = e.ClickedItem.Text;
                for (int i = 0; i < metroContextMenu2.Items.Count; i++)
                {
                    if (e.ClickedItem.Text == metroContextMenu2.Items[i].Text)
                    {
                        Command_WD wd = ((Command_WD)command_table.GetCommandbyIndex(rowIndex));
                        wd.selectedTargetIndex = i;
                        command_table.SetCommand(wd, rowIndex);
                        break;
                    }
                }
            }
        }

        private void mouseKeyEventProvider1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 114)//F3
            {
                int e_RowIndex = metroCommandGrid.SelectedRows[0].Index;
                ((Command_WD)command_table.commands[0]).baseURL = metroCommandGrid.Rows[0].Cells[2].Value.ToString();
                metroCommandGrid.Rows[e_RowIndex].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.pause;
                metroCommandGrid.Refresh();
                if (command_table.ExecuteCommand(e_RowIndex))
                    metroCommandGrid.Rows[e_RowIndex].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.success;
                else
                    metroCommandGrid.Rows[e_RowIndex].Cells[0].Value = global::Selenium_Recorder.Properties.Resources.fail;
                metroCommandGrid.Refresh();
            }

        }

       
    }
}