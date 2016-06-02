using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////////////////////////////////
using System.Drawing;
using Sikuli4Net.sikuli_REST;
using Sikuli4Net.sikuli_UTIL;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Selenium_Interface
{
    public class IDesktop
    {
        APILauncher launcher;
        public IDesktop()
        {
            launcher = new APILauncher(false);
            launcher.Start();
            //OpenQA.Selenium.IWebDriver driver = new FirefoxDriver(new FirefoxProfile("aqf0u6pa.Dev"));
        }

        public void Close()
        {
            launcher.Stop();
        }
        public bool Find(string pattern_path, Point offset, double similar = 0.5, bool highlight = false)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
                scrn.Find(pattern, highlight);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Click(string pattern_path, Point offset, double similar = 0.5, bool highlight = false)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
                scrn.Click(pattern, highlight);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Wait(string pattern_path, Point offset, double similar = 0.5, int  timeout = 15)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
                scrn.Wait(pattern, timeout);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool WaitVanish(string pattern_path, Point offset, double similar = 0.5, int timeout = 15)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
               return scrn.WaitVanish(pattern, timeout);
            }
            catch
            {
                return false;
            }
        }

        public bool DoubleClick(string pattern_path, Point offset, double similar = 0.5, bool highlight = false)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
                scrn.DoubleClick(pattern, highlight);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool RightClick(string pattern_path, Point offset, double similar = 0.5, bool highlight = false)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
                scrn.RightClick(pattern, highlight);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Type(string pattern_path, Point offset, string text,double similar = 0.5)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
                scrn.Type(pattern, text);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Exists(string pattern_path, Point offset, double similar = 0.5, int timeout = 15)
        {
            Pattern pattern = new Pattern(pattern_path, offset, similar);
            Screen scrn = new Screen();
            try
            {
                return scrn.Exists(pattern, timeout);
            }
            catch
            {
                return false;
            }
        }

        public bool DragDrop(string clickPattern_path,string dropPattern_path,
                            Point clickPattern_offset,Point dropPattern_offset,
                            double clickPattern_similar = 0.5, double dropPattern_similar = 0.5)
        {
            Pattern clickPattern = new Pattern(clickPattern_path, clickPattern_offset, clickPattern_similar);
            Pattern dropPattern = new Pattern(dropPattern_path, dropPattern_offset, dropPattern_similar);
            Screen scrn = new Screen();
            try
            {
                scrn.DragDrop(clickPattern, dropPattern);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
