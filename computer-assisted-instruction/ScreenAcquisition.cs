using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace computer_assisted_instruction
{
    public class ScreenAcquisition : IDisposable
    {
        string MainFolder;
        public Queue<Bitmap> ScreenQueue = new Queue<Bitmap>();
        Thread ImageCollector = null;
        bool Running = true;
        int SleepTime = 2000;
        public static Bitmap m_capture()
        {
            Bitmap screen = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                        Screen.PrimaryScreen.Bounds.Height);

            Graphics g = Graphics.FromImage(screen);
            g.CopyFromScreen(0, 0, 0, 0, screen.Size);
            return screen;
        }
        public void m_capture(string path)
        {
            try
            {
                MainFolder = path;
                new Thread(thrd_capture).Start();
                if (path != "" && ImageCollector == null)
                {
                    ImageCollector = new Thread(thrd_ScreenQueue2Files);
                    ImageCollector.Start();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true); 
            //GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Running = false;
                disposed = true;
            }
        }
        ~ScreenAcquisition()
        {
            Dispose(false);
        }

        private void thrd_capture()
        {
            try
            {
                Bitmap screen = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                        Screen.PrimaryScreen.Bounds.Height);

                Graphics g = Graphics.FromImage(screen);
                g.CopyFromScreen(0, 0, 0, 0, screen.Size);
                ScreenQueue.Enqueue(screen);
            }
            catch(Exception ex)
            {

            }
        }

        private void thrd_ScreenQueue2Files()
        {
            try
            {
                while (Running)
                {
                    Thread.Sleep(SleepTime);
                    while (ScreenQueue.Count != 0)
                    {
                        string filename = DateTime.Now.Year.ToString("D4") + "-" +
                            DateTime.Now.Month.ToString("D2") + "-" +
                            DateTime.Now.Day.ToString("D2") + "-" +
                            DateTime.Now.Hour.ToString("D2") + "-" +
                            DateTime.Now.Minute.ToString("D2") + "-" +
                            DateTime.Now.Second.ToString("D2") + "-" +
                            DateTime.Now.Millisecond.ToString("D3");
                        ScreenQueue.Dequeue().Save
                            (MainFolder + filename.ToString() + ".png");
                            //System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public static class WallpaperColorChanger
    {
        public static void SetColor(Color color)
        {
            // Remove the current wallpaper
            NativeMethods.SystemParametersInfo(
                NativeMethods.SPI_SETDESKWALLPAPER,
                0,
                "",
                NativeMethods.SPIF_UPDATEINIFILE | NativeMethods.SPIF_SENDWININICHANGE);

            // Set the new desktop solid color for the current session
            int[] elements = { NativeMethods.COLOR_DESKTOP };
            int[] colors = { System.Drawing.ColorTranslator.ToWin32(color) };
            NativeMethods.SetSysColors(elements.Length, elements, colors);

            // Save value in registry so that it will persist
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Colors", true);
            key.SetValue(@"Background", string.Format("{0} {1} {2}", color.R, color.G, color.B));
        }

        private static class NativeMethods
        {
            public const int COLOR_DESKTOP = 1;
            public const int SPI_SETDESKWALLPAPER = 20;
            public const int SPIF_UPDATEINIFILE = 0x01;
            public const int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll")]
            public static extern bool SetSysColors(int cElements, int[] lpaElements, int[] lpaRgbValues);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        }
    }
}