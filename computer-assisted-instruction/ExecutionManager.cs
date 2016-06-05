using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/////////////////////////////////////////
using System.Windows.Automation;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
/////////////////////////////////////////
using computer_assisted_instruction;
using computer_assisted_instruction.Analysis.Desktop.win_8_1;
using System.Windows.Forms;

namespace computer_assisted_instruction
{
    public class ExecutionManager
    {
        Form Main;
        public ExecutionManager(Form _main)
        {
            Main = _main;
        }
        public enum JIP_Methods { Draw_Horizontal_Lines, Open_Running_App, Colorization, MinimizeAll };
        public delegate void callback_method(DesktopObject dObj);
        private ScreenAcquisition s_acq = new ScreenAcquisition();
        private DesktopObject[] dObjs;
        private object[] objs;
        private List<DesktopObject> dObjHistory = new List<DesktopObject>();
        int indexer = 0;
        private int steps = 0;
        public void m_execute(JIP_Methods method, DesktopObject dObj, callback_method callback)
        {
            if (dObj == null)
                steps = 0;
            Thread t = null;
            switch (method)
            {
                case JIP_Methods.Draw_Horizontal_Lines:
                    t = new Thread(Draw_Horizontal_Lines);
                    break;
                case JIP_Methods.Open_Running_App:
                    t = new Thread(Open_Running_App);
                    break;
                case JIP_Methods.Colorization:
                    t = new Thread(Colorization);
                    break;
                case JIP_Methods.MinimizeAll:
                    t = new Thread(MinimizeAll);
                    break;
                default:
                    break;
            }
            t.Start((object)(new object[2] { dObj, callback }));
        }
        private void Draw_Horizontal_Lines(object obj)
        {
            object[] param = (object[])obj;
            DesktopObject dObj = (DesktopObject)param[0];
            callback_method callback = (callback_method)param[1];
            switch (steps)
            {
                case 0:
                    Bitmap screen = ScreenAcquisition.m_capture();
                    dObj = new DesktopObject(screen, screen);
                    break;
                case 1:
                    JIP_Line[] lines = dObj.Screen.JIP_GetHorizontalLines();
                    Bitmap img = dObj.Screen.JIP_ToGrayBitmap().JIP_DrawJIPLines(lines);
                    dObj = new DesktopObject(dObj.Screen, img);
                    break;
                default:
                    dObj = null;
                    break;
            }
            steps++;
            Main.Invoke(callback, dObj);
        }
        public void Open_Running_App(object obj)
        {
            object[] param = (object[])obj;
            DesktopObject dObj = (DesktopObject)param[0];
            callback_method callback = (callback_method)param[1];
            System.Drawing.Point des;
            switch (steps)
            {
                case 0:
                    Bitmap screen = ScreenAcquisition.m_capture();
                    dObj = new DesktopObject(screen, screen);
                    break;
                case 1:
                    dObj = TaskBar.m_GetTaskBar(dObj.Screen);
                    break;
                case 2:
                    if (dObjs == null)
                        dObjs = TaskBar.m_GetRunningAppfromTaskBar(dObj);
                    if (indexer < dObjs.Length)
                    {
                        dObj = dObjs[indexer++];
                        des = new System.Drawing.Point(dObj.ObjPosition.Location.X + (dObj.ObjPosition.Width / 2),
                                        dObj.ObjPosition.Location.Y + (dObj.ObjPosition.Height / 2));

                        JIP_Mouse.m_MoveMouse(Cursor.Position, des);
                        JIP_Mouse.m_MouseClick(Cursor.Position);
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        dObj = null;
                        dObjs = null;
                    }
                    break;
                case 3:
                    Thread.Sleep(200);
                    Bitmap AppBitmap = ScreenAcquisition.m_capture();
                    DesktopObject x = new DesktopObject(AppBitmap, dObjHistory[1].ObjPosition);
                    dObjHistory[1].ReservedBitmap.Save("C:\\snap\\a.png");
                    x.ReservedBitmap.Save("C:\\snap\\b.png");
                    DesktopObject aObjApp = Jubbah_Image_Processing.JIP_ExtractNewApp(dObjHistory[1].ReservedBitmap, x.ReservedBitmap);
                    des = new System.Drawing.Point(aObjApp.ObjPosition.X + aObjApp.ObjPosition.Width - 75,
                                                   aObjApp.ObjPosition.Y + 7);
                    JIP_Mouse.m_MoveMouse(Cursor.Position, des);
                    Thread.Sleep(1000);
                    JIP_Mouse.m_MouseClick(Cursor.Position);
                    dObj = aObjApp;
                    steps = 1;
                    break;
                default:
                    dObj = null;
                    dObjs = null;
                    break;
            }
            dObjHistory.Add(dObj);
            steps++;
            Main.Invoke(callback, dObj);
        }
        public void Colorization(object obj)
        {
            object[] param = (object[])obj;
            DesktopObject dObj = (DesktopObject)param[0];
            callback_method callback = (callback_method)param[1];
            switch (steps)
            {
                case 0:
                    Bitmap screen = ScreenAcquisition.m_capture();
                    dObj = new DesktopObject(screen, screen);
                    break;
                case 1:
                    Bitmap colorized = dObj.Screen.JIP_Colorization();
                    dObj = new DesktopObject(dObj.Screen, colorized);
                    break;
                case 2:
                    Bitmap colorized2 = dObj.ObjBitmap.JIP_Colorization();
                    dObj = new DesktopObject(dObj.ObjBitmap, colorized2);
                    break;
                case 3:
                    Bitmap colorized3 = dObj.ObjBitmap.JIP_Colorization(true);
                    dObj = new DesktopObject(dObj.ObjBitmap, colorized3);
                    break;
                default:
                    dObj = null;
                    break;
            }
            steps++;
            Main.Invoke(callback, dObj);
        }
        public void MinimizeAll(object obj)
        {
            object[] param = (object[])obj;
            DesktopObject dObj = (DesktopObject)param[0];
            callback_method callback = (callback_method)param[1];
            switch (steps)
            {
                case 0:
                    //Analysis.Desktop.Basic.SetDesktopBackgroundColor();
                    Bitmap screen = ScreenAcquisition.m_capture();
                    dObj = new DesktopObject(screen, screen);
                    break;
                case 1:
                    Analysis.Desktop.Basic.MinimizeAllTrayWindows();
                    Bitmap screen2 = ScreenAcquisition.m_capture();
                    dObj = new DesktopObject(screen2, screen2);
                    break;
                case 2:
                    //Analysis.Desktop.Basic.MaximizeAllTrayWindows();
                    Rectangle taskbar = Analysis.Desktop.Basic.GetTaskBar();
                    Bitmap screen3 = ScreenAcquisition.m_capture();
                    dObj = new DesktopObject(screen3, taskbar);
                    break;
                case 3:
                    if (objs == null)
                    {
                        objs = Analysis.Desktop.Basic.GetTaskbarProcesses();
                        indexer = 0;
                    }
                    if(indexer < objs.Length)
                    {
                        Process[] ps = (Process[])objs;
                        Analysis.Desktop.Basic.MaximizeProcess(ps[indexer]);
                        Rectangle rc = Analysis.Desktop.Basic.GetProcessRectangle(ps[indexer++]);
                        Bitmap screen4 = ScreenAcquisition.m_capture();
                        dObj = new DesktopObject(screen4, rc);
                        steps = 2;
                    }
                    else
                    {
                        dObj = null;
                        objs = null;
                    }
                    break;
                default:
                    dObj = null;
                    break;
            }
            steps++;
            Main.Invoke(callback, dObj);
        }
        ///////////////////////////////////////////////////////////////////
    }
}
