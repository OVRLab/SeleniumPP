using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace computer_assisted_instruction.Analysis.Desktop
{
    public class Basic
    {
        private static Color backgroundColor = Color.FromArgb(67, 149, 209);
        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;
        private const int SW_NORMAL = 1;
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public int lParam;
        }
        enum ABMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }
        enum ABNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }
        enum ABEdge : int
        {
            ABE_LEFT = 0,
            ABE_TOP,
            ABE_RIGHT,
            ABE_BOTTOM
        }
        ////////////////////////////////////////////////////////////
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);
        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();
        ////////////////////////////////////////////////////////////
        
        public static void SetDesktopBackgroundColor()
        {
            WallpaperColorChanger.SetColor(backgroundColor);
        }

        public static void MinimizeAllTrayWindows()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
            Thread.Sleep(500);//Should be replaced
        }

        public static void MaximizeAllTrayWindows()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
            Thread.Sleep(500);//Should be replaced
        }

        public static Rectangle GetTaskBar()
        {
            APPBARDATA pData = new APPBARDATA();
            SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS,ref pData);
            return new Rectangle(pData.rc.left,
                pData.rc.top,
                pData.rc.right - pData.rc.left,
                pData.rc.bottom - pData.rc.top);
        }

        public static Process[] GetTaskbarProcesses()
        {
            Process[] processes = Process.GetProcesses();
            List<Process> taskbarp = new List<Process>();
            foreach (var item in processes)
            {
                if (item.MainWindowTitle.Length > 0 && item.ProcessName.ToLower() != "taskmgr")
                {
                    taskbarp.Add(item);
                }
            }
            return taskbarp.ToArray();
        }
    
        public static void MaximizeProcess(Process wnd)
        {
            ShowWindow(wnd.MainWindowHandle, SW_NORMAL);
            Thread.Sleep(500);
        }

        public static Rectangle GetProcessRectangle(Process wnd)
        {
            RECT Rect = new RECT();
            GetWindowRect(wnd.MainWindowHandle, ref Rect);
            if (Rect.left >= 0 &&
                Rect.top >= 0 &&
                Rect.right >= 0 &&
                Rect.bottom >= 0)
                return new Rectangle(Rect.left,
                    Rect.top,
                    Rect.right - Rect.left,
                    Rect.bottom - Rect.top);
            else
                return new Rectangle(0, 0, 0, 0);
        }

        public static IDictionary<IntPtr, string> GetOpenWindows()
        {
            IntPtr shellWindow = GetShellWindow();
            Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();
            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;

            }, 0);
            return windows;
        }
    }
}
