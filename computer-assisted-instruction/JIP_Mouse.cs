using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace computer_assisted_instruction
{
    public class JIP_Mouse
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void m_MouseClick(Point source)
        {
            int xpos = source.X;
            int ypos = source.Y;
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        public static void m_MoveMouse(Point source,Point destination)
        {
            Point A = source;
            Point B = destination;

            int startX = A.X;
            int startY = A.Y;

            int endX = B.X;
            int endY = B.Y;

            int bezierX = (startX + endX) / 2+100;
            int bezierY = (startY + endY) / 2;

            for (double t = 0.0; t <= 1; t += 0.01)
            {
                int x = (int)((1 - t) * (1 - t) * startX + 2 * (1 - t) * t * bezierX + t * t * endX);
                int y = (int)((1 - t) * (1 - t) * startY + 2 * (1 - t) * t * bezierY + t * t * endY);
                Cursor.Position = new Point(x, y);
                Thread.Sleep(10);
            }
        }
    }
}
