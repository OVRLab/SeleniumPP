using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;

namespace computer_assisted_instruction.Analysis.Desktop.win_8_1
{
    public class TaskBar
    {
        public static DesktopObject[] m_GetRunningAppfromTaskBar(DesktopObject TaskBar)
        {
            Bitmap TaskBarBitmap = TaskBar.ObjBitmap;
            double[,] filterMatrix = JIP_Matrix.RunningAppPattern;
            int threshold = 20;
            List<DesktopObject> apps = new List<DesktopObject>();
            byte[] pixelBuffer = TaskBarBitmap.JIP_ToGray1D();
            byte[] resultBuffer = new byte[pixelBuffer.Length];
            int Stride = TaskBarBitmap.Width * 4;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffsetWidth = (filterWidth) / 2;
            int filterOffsetHeight = (filterHeight) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            for (int offsetY = filterOffsetHeight; offsetY <
                TaskBarBitmap.Height - filterOffsetHeight; offsetY++)
            {
                int prevoffsetX = 0;
                for (int offsetX = filterOffsetWidth; offsetX <
                    TaskBarBitmap.Width - filterOffsetWidth; offsetX++)
                {
                    byteOffset = offsetY *
                                 Stride +
                                 offsetX * 4;
                    int percAvg = 0;
                    for (int filterY = -filterOffsetHeight;
                        filterY < filterOffsetHeight; filterY++)
                    {
                        for (int filterX = -filterOffsetWidth;
                            filterX < filterOffsetWidth; filterX++)
                        {

                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * Stride);

                            int diff = Math.Abs(pixelBuffer[calcOffset] - ((byte)filterMatrix[filterY + filterOffsetHeight, filterX + filterOffsetWidth]));
                            if (diff < threshold)
                                diff = 0;
                            int perc = ((255 - diff) * 100) / 255;
                            percAvg += perc;
                        }
                    }
                    percAvg /= filterWidth * filterHeight;
                    if (percAvg > 65 && Math.Abs(offsetX - prevoffsetX) > (filterWidth / 2))//65
                    {
                        percAvg = 100;
                        DesktopObject dObj = new DesktopObject(TaskBar.Screen);
                        dObj.ObjPosition =
                            new Rectangle(new Point(TaskBar.ObjPosition.X + offsetX - (filterWidth / 2),
                                                    TaskBar.ObjPosition.Y + offsetY),
                                new Size(filterWidth, TaskBarBitmap.Height)
                            );
                        apps.Add(dObj);
                        prevoffsetX = offsetX;
                    }
                }
            }
            return apps.ToArray();
        }
        public static DesktopObject m_GetTaskBar(Bitmap Screen)
        {
            JIP_Line[] lines = Screen.JIP_GetHorizontalLines();
            JIP_Line taskbarLine = lines[0]; //lines[lines.Length - 1];

            int w = Math.Abs(taskbarLine.B.X - taskbarLine.A.X);
            int h = Math.Abs(Screen.Height - taskbarLine.A.Y);
            DesktopObject dObj = new DesktopObject(Screen);
            dObj.ObjPosition = new Rectangle(taskbarLine.A.X, taskbarLine.A.Y, w, h);
            return dObj;
        }
        
        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            byte R = Convert.ToByte(r * 255.0f);
            byte G = Convert.ToByte(g * 255.0f);
            byte B = Convert.ToByte(b * 255.0f);
            return Color.FromArgb(R,G,B);
        }
    }
}