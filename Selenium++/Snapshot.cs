using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using computer_assisted_instruction;
using System.Threading;

namespace Selenium_Recorder
{
    public delegate void SnapBack(Bitmap snap);
    public partial class Snapshot : Form
    {
        Bitmap snap = null;
        Graphics g = null;
        bool keyDown = false;
        Pen p = new Pen(Color.OrangeRed, 2);
        SnapBack callBackMethod;
        public Snapshot()
        {
            InitializeComponent();
        }
        public void Snap(SnapBack _callBackMethod)
        {
            Bitmap screen = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                        Screen.PrimaryScreen.Bounds.Height);

            Graphics g = Graphics.FromImage(screen);
            g.CopyFromScreen(0, 0, 0, 0, screen.Size);
            snap = Jubbah_Image_Processing.JIP_AddGrayLayer(screen, 40);
            this.BackgroundImage = snap;
            callBackMethod = _callBackMethod;
            this.Show();
        }

        private void Snapshot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 && keyDown)
            {
                Graphics g = this.CreateGraphics();
                Rectangle r = new Rectangle(old_x, old_y, old_w, old_h);
                g.DrawImage(snap, r, r, GraphicsUnit.Pixel);
            }
            else if (e.KeyChar == 27)
                this.Close();
        }

        Point startP = Point.Empty, endP = Point.Empty;
        private void Snapshot_MouseDown(object sender, MouseEventArgs e)
        {
            startP = e.Location;
            keyDown = true;
        }
        int old_x, old_y, old_w, old_h;
        private void Snapshot_MouseMove(object sender, MouseEventArgs e)
        {
            if (!keyDown)
                return;

            int x = startP.X;
            int y = startP.Y;
            if (e.X > startP.X && e.Y > startP.Y)
            {
                x = startP.X;
                y = startP.Y;
            }
            else if (startP.X > e.X && startP.Y > e.Y)
            {
                x = e.X;
                y = e.Y;
            }
            else if (e.X > startP.X && startP.Y > e.Y)
            {
                x = startP.X;
                y = e.Y;
            }
            else if (startP.X > e.X && e.Y > startP.Y)
            {
                x = e.X;
                y = startP.Y;
            }
            int w = Math.Abs(e.X - startP.X);
            int h = Math.Abs(e.Y - startP.Y);
            if (w < 10 || h < 10)
                return;
            Graphics g = this.CreateGraphics();
            Rectangle r = new Rectangle(old_x, old_y, old_w, old_h);
            g.DrawImage(snap, r, r, GraphicsUnit.Pixel);
            g.DrawRectangle(p, x, y, w, h);
            old_x = x - 1; old_y = y - 1; old_w = w + 20; old_h = h + 20;
            Thread.Sleep(20);
        }

        private void Snapshot_MouseUp(object sender, MouseEventArgs e)
        {
            if (keyDown && old_w - 20 > 0 && old_h - 20 > 0)
            {
                Bitmap img = new Bitmap(old_w - 20, old_h - 20);
                Graphics g = Graphics.FromImage(img);
                Rectangle r = new Rectangle(old_x + 1, old_y + 1, old_w - 20, old_h - 20);
                g.DrawImage(snap, new Rectangle(0, 0, img.Width, img.Height), r, GraphicsUnit.Pixel);
                Invoke(callBackMethod, img);
                keyDown = false;
                endP = e.Location;
                this.Close();
            }
        }
    }
}
