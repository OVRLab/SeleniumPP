using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace computer_assisted_instruction
{
    public static class Jubbah_Image_Processing
    {
        public static Bitmap JIP_AddGrayLayer(Bitmap sourceBitmap,byte GrayThreshold)
        {
            byte[] A = JIP_To1D(sourceBitmap);
            byte[] result = new byte[A.Length];
            for (int i = 0; i < A.Length; i += 4)
            {

                result[i] = A[i] + GrayThreshold >255 ? (byte)255 : (byte)(A[i] + GrayThreshold);
                result[i + 1] = A[i + 1] + GrayThreshold > 255 ? (byte)255 : (byte)(A[i + 1] + GrayThreshold);
                result[i + 2] = A[i + 2] + GrayThreshold > 255 ? (byte)255 : (byte)(A[i + 2] + GrayThreshold);
                result[i + 3] = 255;
            }
            return result.JIP_1DToBitmap(sourceBitmap.Width, sourceBitmap.Height);
        }
        public static Bitmap JIP_Colorization(this Bitmap sourceBitmap, bool DissolveSmallGroup=false)
        {
            byte[] pixelBuffer = sourceBitmap.JIP_To1D();
            byte[] resultBuffer = new byte[pixelBuffer.Length];
            bool[] grouped = new bool[pixelBuffer.Length];
            int Stride = sourceBitmap.Width * 4;
            int byteOffset = 0, pOffset = 0;
            int[] indexer = new int[3] { -1, 0, 1 };
            List<Color> colorize = new List<Color>();
            Queue<Point> queuedIndex = new Queue<Point>();
            Queue<Point> groupedIndex = new Queue<Point>();

            int threshold = (int)(sourceBitmap.Width * sourceBitmap.Height * 1) / 100;
            for (int offsetY = 0; offsetY < sourceBitmap.Height; offsetY++)
            {
                for (int offsetX = 0; offsetX < sourceBitmap.Width; offsetX++)
                {
                    byteOffset = offsetY * Stride + offsetX * 4;

                    if (grouped[byteOffset])
                        continue;
                    queuedIndex.Enqueue(new Point(offsetX, offsetY));
                    grouped[byteOffset] = true;
                    while (queuedIndex.Count > 0)
                    {
                        Point p = queuedIndex.Dequeue();
                        pOffset = p.Y * Stride + p.X * 4;
                        groupedIndex.Enqueue(p);
                        for (int i = 0; i < indexer.Length; i++)
                        {
                            for (int j = 0; j < indexer.Length; j++)
                            {
                                if (p.X + indexer[i] < 0 ||
                                    p.X + indexer[i] >= sourceBitmap.Width ||
                                    p.Y + indexer[j] < 0 ||
                                    p.Y + indexer[j] >= sourceBitmap.Height)
                                    continue;
                                byteOffset = (p.Y + indexer[j]) * Stride + (p.X + indexer[i]) * 4;
                                if (!grouped[byteOffset] &&
                                    pixelBuffer[pOffset] == pixelBuffer[byteOffset] &&
                                    pixelBuffer[pOffset + 1] == pixelBuffer[byteOffset + 1] &&
                                    pixelBuffer[pOffset + 2] == pixelBuffer[byteOffset + 2])
                                {
                                    queuedIndex.Enqueue(new Point(p.X + indexer[i], p.Y + indexer[j]));
                                    grouped[byteOffset] = true;
                                }
                            }
                        }
                    }
                    /////////////////////////////////
                    if (groupedIndex.Count > threshold)
                    {
                        Color c = generateRandomColor(Color.White);
                        while (colorize.Contains(c))
                            c = generateRandomColor(Color.White);
                        colorize.Add(c);
                        while (groupedIndex.Count > 0)
                        {
                            Point p = groupedIndex.Dequeue();
                            pOffset = p.Y * Stride + p.X * 4;
                            resultBuffer[pOffset] = c.R;
                            resultBuffer[pOffset + 1] = c.G;
                            resultBuffer[pOffset + 2] = c.B;
                            resultBuffer[pOffset + 3] = 255;
                        }
                    }
                    else
                    {
                        Color maxC = Color.Red;
                        if(DissolveSmallGroup)
                            maxC = findBoundaryColor(sourceBitmap, resultBuffer, groupedIndex.ToArray());
                        while (groupedIndex.Count > 0)
                        {
                            Point p = groupedIndex.Dequeue();
                            pOffset = p.Y * Stride + p.X * 4;
                            resultBuffer[pOffset] = maxC.B;
                            resultBuffer[pOffset + 1] = maxC.G;
                            resultBuffer[pOffset + 2] = maxC.R;
                            resultBuffer[pOffset + 3] = 255;
                        }
                    }
                }
            }
            return resultBuffer.JIP_1DToBitmap(sourceBitmap.Width,sourceBitmap.Height);
        }
        private static Color findBoundaryColor(Bitmap sourceBitmap,byte[] resultBuffer, Point[] pArray)
        {
            int[] indexer = new int[3] { -1, 0, 1 };
            int Stride = sourceBitmap.Width * 4;
            Dictionary<Color, int> BoundaryColors = new Dictionary<Color, int>();
            for (int index = 0; index < pArray.Length; index++)
            {
                Point p = pArray[index];
                int byteOffset;
                for (int i = 0; i < indexer.Length; i++)
                {
                    for (int j = 0; j < indexer.Length; j++)
                    {
                        if (p.X + indexer[i] < 0 ||
                            p.X + indexer[i] >= sourceBitmap.Width ||
                            p.Y + indexer[j] < 0 ||
                            p.Y + indexer[j] >= sourceBitmap.Height)
                            continue;
                        byteOffset = (p.Y + indexer[j]) * Stride + (p.X + indexer[i]) * 4;
                        if (!(resultBuffer[byteOffset] == 0 &&
                            resultBuffer[byteOffset + 1] == 0))
                        {
                            //not red or black
                            Color c = Color.FromArgb(resultBuffer[byteOffset + 2], resultBuffer[byteOffset + 1], resultBuffer[byteOffset]);
                            if (!BoundaryColors.ContainsKey(c))
                                BoundaryColors.Add(c, 1);
                            else
                                BoundaryColors[c] += 1;
                        }
                    }
                }
            }
            int max = 0;
            Color maxColor = Color.Black;
            foreach (var item in BoundaryColors)
	        {
                if(item.Value > max)
                {
                    max = item.Value;
                    maxColor = item.Key;
                }
	        }
            return maxColor;
        }
        private static Color generateRandomColor(Color mix)
        {
            Random random = new Random();
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            // mix the color
            if (mix != null)
            {
                red = (red + mix.R) / 2;
                green = (green + mix.G) / 2;
                blue = (blue + mix.B) / 2;
            }
            return Color.FromArgb(red, green, blue);
        }
        public static byte[] JIP_To1D(this Bitmap sourceBitmap)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            return pixelBuffer;
        }
        public static Image JIP_ResizeImage(this Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            int destWidth = (int)(sourceWidth * nPercentW);
            int destHeight = (int)(sourceHeight * nPercentH);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }
        public static Image JIP_ScaleImage(this Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercentW = 1;
            float nPercentH = 1;

            if (sourceWidth > size.Width)
                nPercentW = ((float)size.Width / (float)sourceWidth);
            if (sourceHeight > size.Height)
                nPercentH = ((float)size.Height / (float)sourceHeight);

            int destWidth = (int)(sourceWidth * nPercentW);
            int destHeight = (int)(sourceHeight * nPercentH);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }
        public static DesktopObject JIP_ExtractNewApp(Bitmap sourceBitmap, Bitmap anotherBitmap)
        {
            if (sourceBitmap.Size != anotherBitmap.Size)
                throw new Exception("Both bitmaps should have the same size");
            byte[] A = JIP_ToGray1D(sourceBitmap);
            byte[] B = JIP_ToGray1D(anotherBitmap);
            int Stride = sourceBitmap.Width * 4;
            int byteOffset = 0;
            //UpperLeft
            Point UpperLeft = Point.Empty;
            bool cont = true;
            for (int offsetY = 0; offsetY < sourceBitmap.Height && cont; offsetY++)
            {
                for (int offsetX = 0; offsetX < sourceBitmap.Width && cont; offsetX++)
                {
                    byteOffset = offsetY *
                                 Stride +
                                 offsetX * 4;
                    if (A[byteOffset] != B[byteOffset])
                    {
                        UpperLeft = new Point(offsetX, offsetY);
                        cont=false;
                    }
                }
            }
            //UpperRight
            Point UpperRight = Point.Empty;
            cont = true;
            for (int offsetY = 0; offsetY < sourceBitmap.Height && cont; offsetY++)
            {
                for (int offsetX = sourceBitmap.Width - 1; offsetX >= 0 && cont; offsetX--)
                {
                    byteOffset = offsetY *
                                 Stride +
                                 offsetX * 4;
                    if (A[byteOffset] != B[byteOffset])
                    {
                        UpperRight = new Point(offsetX, offsetY);
                        cont=false;
                    }
                }
            }
            //LowerLeft
            Point LowerLeft = Point.Empty;
            cont = true;
            for (int offsetY = sourceBitmap.Height - 1; offsetY >= 0 && cont; offsetY--)
            {
                for (int offsetX = 0; offsetX < sourceBitmap.Width && cont; offsetX++)
                {
                    byteOffset = offsetY *
                                 Stride +
                                 offsetX * 4;
                    if (A[byteOffset] != B[byteOffset])
                    {
                        LowerLeft = new Point(offsetX, offsetY);
                        cont = false;
                    }
                }
            }
            //LowerRight
            //Point LowerRight = Point.Empty;
            //for (int offsetY = sourceBitmap.Height - 1; offsetY >= 0; offsetY--)
            //{
            //    for (int offsetX = sourceBitmap.Width - 1; offsetX >= 0; offsetX--)
            //    {
            //        byteOffset = offsetY *
            //                     Stride +
            //                     offsetX * 4;
            //        if (A[byteOffset] != B[byteOffset])
            //        {
            //            LowerRight = new Point(offsetX, offsetY);
            //            break;
            //        }
            //    }
            //}
            if (UpperLeft != Point.Empty &&
                UpperRight != Point.Empty &&
                LowerLeft != Point.Empty) //&& LowerRight != Point.Empty)
            {
                return new DesktopObject(anotherBitmap, new Rectangle(UpperLeft, new Size(UpperRight.X - UpperLeft.X, LowerLeft.Y - UpperLeft.Y)));
            }
            return new DesktopObject(anotherBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height));
        }
        public static Bitmap JIP_Subtract(this Bitmap sourceBitmap, Bitmap anotherBitmap)
        {
            byte[] A = JIP_ToGray1D(sourceBitmap);
            byte[] B = JIP_ToGray1D(anotherBitmap);
            byte[] result = new byte[A.Length];
            for (int i = 0; i < A.Length; i += 4)
            {
                if (A[i] - B[i] == 0 &&
                   A[i + 1] - B[i + 1] == 0 &&
                    A[i + 2] - B[i + 2] == 0 &&
                    A[i + 3] - B[i + 3] == 0)
                {
                    result[i] = 0;
                    result[i + 1] = 0;
                    result[i + 2] = 0;
                    result[i + 3] = 255;
                }
                else
                {
                    result[i] = B[i];
                    result[i + 1] = B[i + 1];
                    result[i + 2] = B[i + 2];
                    result[i + 3] = 255;
                }
            }
            return result.JIP_1DToBitmap(sourceBitmap.Width, sourceBitmap.Height);
        }
        public static Point[] JIP_GetRunningAppPoints(this Bitmap sourceBitmap, double[,] filterMatrix, int threshold = 20)
        {
            List<Point> apps = new List<Point>();
            byte[] pixelBuffer = sourceBitmap.JIP_ToGray1D();
            byte[] resultBuffer = new byte[pixelBuffer.Length];
            int Stride = sourceBitmap.Width * 4;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffsetWidth = (filterWidth) / 2;
            int filterOffsetHeight = (filterHeight) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            for (int offsetY = filterOffsetHeight; offsetY <
                sourceBitmap.Height - filterOffsetHeight; offsetY++)
            {
                int prevoffsetX = 0;
                for (int offsetX = filterOffsetWidth; offsetX <
                    sourceBitmap.Width - filterOffsetWidth; offsetX++)
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
                        apps.Add(new Point(offsetX - (filterWidth / 2), offsetY));
                        prevoffsetX = offsetX;
                    }
                }
            }
            return apps.ToArray();
        }
        public static Bitmap[] JIP_GetRunningApp(this Bitmap sourceBitmap, double[,] filterMatrix, int threshold = 20)
        {
            List<Bitmap> apps = new List<Bitmap>();
            byte[] pixelBuffer = sourceBitmap.JIP_ToGray1D();
            byte[] resultBuffer = new byte[pixelBuffer.Length];
            int Stride = sourceBitmap.Width * 4;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffsetWidth = (filterWidth) / 2;
            int filterOffsetHeight = (filterHeight) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            for (int offsetY = filterOffsetHeight; offsetY <
                sourceBitmap.Height - filterOffsetHeight; offsetY++)
            {
                int prevoffsetX = 0;
                for (int offsetX = filterOffsetWidth; offsetX <
                    sourceBitmap.Width - filterOffsetWidth; offsetX++)
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
                        Bitmap app = new Bitmap(filterWidth, sourceBitmap.Height);
                        Graphics g = Graphics.FromImage(app);
                        g.DrawImage(sourceBitmap,
                            new Rectangle(0, 0, filterWidth, sourceBitmap.Height),
                            new Rectangle(offsetX - (filterWidth/2), offsetY, filterWidth, sourceBitmap.Height),
                            GraphicsUnit.Pixel);
                        apps.Add(app);
                        prevoffsetX = offsetX;
                    }
                }
            }
            return apps.ToArray();
        }
        public static Bitmap JIP_MatchPattern(this Bitmap sourceBitmap, double[,] filterMatrix, int threshold = 20)
        {
            byte[] pixelBuffer = sourceBitmap.JIP_ToGray1D();
            byte[] resultBuffer = new byte[pixelBuffer.Length];
            int Stride = sourceBitmap.Width * 4;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffsetWidth = (filterWidth) / 2;
            int filterOffsetHeight = (filterHeight) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            for (int offsetY = filterOffsetHeight; offsetY <
                sourceBitmap.Height - filterOffsetHeight; offsetY++)
            {
                for (int offsetX = filterOffsetWidth; offsetX <
                    sourceBitmap.Width - filterOffsetWidth; offsetX++)
                {
                    byteOffset = offsetY *
                                 Stride +
                                 offsetX * 4;
                    int percAvg=0;
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
                    percAvg /= filterWidth*filterHeight;
                    if (percAvg > 65)//65
                        percAvg = 100;
                    int AvgMatch = percAvg * 255 / 100;
                    
                    resultBuffer[byteOffset] = (byte)(AvgMatch);
                    resultBuffer[byteOffset + 1] = (byte)(AvgMatch);
                    resultBuffer[byteOffset + 2] = (byte)(AvgMatch);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }
            return resultBuffer.JIP_1DToBitmap(sourceBitmap.Width, sourceBitmap.Height);
        }
        public static Bitmap JIP_DrawVerticalLines(this Bitmap sourceBitmap, int lineLength = 100)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float rgb = 0;

            for (int k = 0; k < pixelBuffer.Length; k += 4)
            {
                rgb = pixelBuffer[k] * 0.11f;
                rgb += pixelBuffer[k + 1] * 0.59f;
                rgb += pixelBuffer[k + 2] * 0.3f;


                pixelBuffer[k] = (byte)rgb;
                pixelBuffer[k + 1] = pixelBuffer[k];
                pixelBuffer[k + 2] = pixelBuffer[k];
                pixelBuffer[k + 3] = 255;
            }
            int byteOffset = 0,index =0, prev = 0, next = 0, up = 0, down = 0;

            for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
            {
                int inLine = 0, inLineIndex = 0, inLineStartIndex = 0;
                int offsetY;
                for (offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
                {
                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    resultBuffer[byteOffset] = pixelBuffer[byteOffset];
                    resultBuffer[byteOffset + 1] = pixelBuffer[byteOffset + 1];
                    resultBuffer[byteOffset + 2] = pixelBuffer[byteOffset + 2];
                    resultBuffer[byteOffset + 3] = pixelBuffer[byteOffset + 3];

                    prev = offsetY * sourceData.Stride + (offsetX - 1) * 4;
                    next = offsetY * sourceData.Stride + (offsetX + 1) * 4;
                    up = (offsetY - 1) * sourceData.Stride + offsetX * 4;
                    down = (offsetY + 1) * sourceData.Stride + offsetX * 4;

                    if (pixelBuffer[byteOffset] == pixelBuffer[up] &&
                        pixelBuffer[byteOffset] == pixelBuffer[down] &&
                        (!(resultBuffer[prev + 2] == 255 && resultBuffer[prev] == 0 && resultBuffer[prev + 1] == 0)) &&
                        (pixelBuffer[byteOffset] != pixelBuffer[prev] || pixelBuffer[byteOffset] != pixelBuffer[next]))
                    {
                        if (inLine == 0)
                            inLineStartIndex = offsetY;
                        inLineIndex = byteOffset;
                        inLine++;
                    }
                    else
                    {
                        if (inLine >= lineLength)
                        {
                            //end of line
                            for (int i = inLineStartIndex; i < inLineStartIndex + inLine; i++)
                            {
                                index = i * sourceData.Stride + offsetX * 4;
                                resultBuffer[index] = 0;//Blue
                                resultBuffer[index + 1] = 0;//GREEN
                                resultBuffer[index + 2] = 255;//RED
                                resultBuffer[index + 3] = 255;
                            }
                        }
                        inLine = 0;
                    }
                }
                if (inLine >= lineLength)
                {
                    //end of line
                    for (int i = inLineStartIndex; i < inLineStartIndex + inLine; i++)
                    {
                        index = i * sourceData.Stride + offsetX * 4;
                        resultBuffer[index] = 0;//Blue
                        resultBuffer[index + 1] = 0;//GREEN
                        resultBuffer[index + 2] = 255;//RED
                        resultBuffer[index + 3] = 255;
                    }
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
        public static Bitmap JIP_1DToBitmap(this byte[] pixelBuffer,int width,int height)
        {
            Bitmap resultBitmap = new Bitmap(width, height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);
            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
        public static byte[] JIP_ToGray1D(this Bitmap sourceBitmap)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float rgb = 0;

            for (int k = 0; k < pixelBuffer.Length; k += 4)
            {
                rgb = pixelBuffer[k] * 0.11f;
                rgb += pixelBuffer[k + 1] * 0.59f;
                rgb += pixelBuffer[k + 2] * 0.3f;


                pixelBuffer[k] = (byte)rgb;
                pixelBuffer[k + 1] = pixelBuffer[k];
                pixelBuffer[k + 2] = pixelBuffer[k];
                pixelBuffer[k + 3] = 255;
            }
            return pixelBuffer;
        }
        public static Bitmap JIP_ToGrayBitmap(this Bitmap sourceBitmap)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float rgb = 0;

            for (int k = 0; k < pixelBuffer.Length; k += 4)
            {
                rgb = pixelBuffer[k] * 0.11f;
                rgb += pixelBuffer[k + 1] * 0.59f;
                rgb += pixelBuffer[k + 2] * 0.3f;


                pixelBuffer[k] = (byte)rgb;
                pixelBuffer[k + 1] = pixelBuffer[k];
                pixelBuffer[k + 2] = pixelBuffer[k];
                pixelBuffer[k + 3] = 255;
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
        public static JIP_Line[] JIP_GetHorizontalLines(this Bitmap sourceBitmap, int lineLength = 300)
        {
            byte[] pixelBuffer = sourceBitmap.JIP_ToGray1D();
            byte[] resultBuffer = new byte[pixelBuffer.Length];
            int Stride = sourceBitmap.Width * 4;
            
            int byteOffset = 0, index = 0, prev = 0, next = 0, up = 0, down = 0;
            List<JIP_Line> lines = new List<JIP_Line>();

            for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
            {
                int inLine = 0, inLineIndex = 0, inLineStartIndex = 0, inLineoffsetX = 0;
                for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
                {
                    byteOffset = offsetY *
                                 Stride +
                                 offsetX * 4;

                    resultBuffer[byteOffset] = pixelBuffer[byteOffset];
                    resultBuffer[byteOffset + 1] = pixelBuffer[byteOffset + 1];
                    resultBuffer[byteOffset + 2] = pixelBuffer[byteOffset + 2];
                    resultBuffer[byteOffset + 3] = pixelBuffer[byteOffset + 3];

                    prev = offsetY * Stride + (offsetX - 1) * 4;
                    next = offsetY * Stride + (offsetX + 1) * 4;
                    up = (offsetY - 1) * Stride + offsetX * 4;
                    down = (offsetY + 1) * Stride + offsetX * 4;

                    if (pixelBuffer[byteOffset] == pixelBuffer[prev] &&
                        pixelBuffer[byteOffset] == pixelBuffer[next] &&
                        //(!(resultBuffer[up + 2] == 255 && resultBuffer[up] == 0 && resultBuffer[up + 1] == 0)) &&
                        (pixelBuffer[byteOffset] != pixelBuffer[up] || pixelBuffer[byteOffset] != pixelBuffer[down]))
                    {
                        if (inLine == 0)
                        {
                            inLineStartIndex = byteOffset;
                            inLineoffsetX = offsetX;
                        }
                        inLineIndex = byteOffset;
                        inLine++;
                    }
                    else
                    {
                        if (inLine >= lineLength)
                        {
                            //end of line
                            for (int i = inLineStartIndex; i < inLineStartIndex + (inLine * 4); i += 4)
                            {
                                resultBuffer[i] = 0;//Blue
                                resultBuffer[i + 1] = 0;//GREEN
                                resultBuffer[i + 2] = 255;//RED
                                resultBuffer[i + 3] = 255;
                            }
                            lines.Add(new JIP_Line(inLineoffsetX-1, offsetY, inLineoffsetX + inLine+1, offsetY));
                        }
                        inLine = 0;
                    }
                }
                if (inLine >= lineLength)
                {
                    //end of line
                    for (int i = inLineStartIndex; i < inLineStartIndex + (inLine * 4); i += 4)
                    {
                        resultBuffer[i] = 0;//Draw Red Line
                        resultBuffer[i + 1] = 0;
                        resultBuffer[i + 2] = 255;
                        resultBuffer[i + 3] = 255;
                    }
                    lines.Add(new JIP_Line(inLineoffsetX-1, offsetY, inLineoffsetX + inLine+1, offsetY));
                }
            }
            return lines.ToArray();
        }
        public static Bitmap JIP_DrawJIPLines(this Bitmap sourceBitmap, JIP_Line[] lines)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            
            for (int i = 0; i < lines.Length; i++)
            {
                int byteOffset = lines[i].A.Y *
                                 sourceData.Stride +
                                 lines[i].A.X * 4;
                int inLine = lines[i].B.X - lines[i].A.X;
                for (int j = byteOffset; j < byteOffset+(inLine * 4); j+=4)
                {
                    pixelBuffer[j] = 0;//Blue
                    pixelBuffer[j + 1] = 0;//GREEN
                    pixelBuffer[j + 2] = 255;//RED
                    pixelBuffer[j + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
        public static Bitmap JIP_DrawHorizontalLines(this Bitmap sourceBitmap, int lineLength = 300)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float rgb = 0;

            for (int k = 0; k < pixelBuffer.Length; k += 4)
            {
                rgb = pixelBuffer[k] * 0.11f;
                rgb += pixelBuffer[k + 1] * 0.59f;
                rgb += pixelBuffer[k + 2] * 0.3f;


                pixelBuffer[k] = (byte)rgb;
                pixelBuffer[k + 1] = pixelBuffer[k];
                pixelBuffer[k + 2] = pixelBuffer[k];
                pixelBuffer[k + 3] = 255;
            }
            int byteOffset = 0,prev=0,next=0,up=0,down=0;

            for (int offsetY = 1; offsetY < sourceBitmap.Height-1; offsetY++)
            {
                int inLine = 0,inLineIndex=0,inLineStartIndex=0;
                for (int offsetX = 1; offsetX < sourceBitmap.Width-1; offsetX++)
                {
                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    resultBuffer[byteOffset] = pixelBuffer[byteOffset];
                    resultBuffer[byteOffset + 1] = pixelBuffer[byteOffset+1];
                    resultBuffer[byteOffset + 2] = pixelBuffer[byteOffset+2];
                    resultBuffer[byteOffset + 3] = pixelBuffer[byteOffset+3];

                    prev = offsetY * sourceData.Stride + (offsetX - 1) * 4;
                    next = offsetY * sourceData.Stride + (offsetX + 1) * 4;
                    up = (offsetY - 1) * sourceData.Stride + offsetX * 4;
                    down = (offsetY + 1) * sourceData.Stride + offsetX * 4;

                    if (pixelBuffer[byteOffset] == pixelBuffer[prev] &&
                        pixelBuffer[byteOffset] == pixelBuffer[next] &&
                        (!(resultBuffer[up + 2] == 255 && resultBuffer[up] == 0 && resultBuffer[up + 1] == 0)) &&
                        (pixelBuffer[byteOffset] != pixelBuffer[up] || pixelBuffer[byteOffset] != pixelBuffer[down]) )
                    {
                        if(inLine == 0)
                            inLineStartIndex = byteOffset;
                        inLineIndex = byteOffset;
                        inLine++;
                    }
                    else
                    {
                        if(inLine >= lineLength)
                        {
                            //end of line
                            for (int i = inLineStartIndex; i < inLineStartIndex+(inLine * 4); i += 4)
                            {
                                resultBuffer[i] = 0;//Blue
                                resultBuffer[i + 1] = 0;//GREEN
                                resultBuffer[i + 2] = 255;//RED
                                resultBuffer[i + 3] = 255;
                            }
                        }
                        inLine = 0;
                    }
                }
                if (inLine >= lineLength)
                {
                    //end of line
                    for (int i = inLineStartIndex; i < inLineStartIndex + (inLine * 4); i += 4)
                    {
                        resultBuffer[i] = 0;//Draw Red Line
                        resultBuffer[i + 1] = 0;
                        resultBuffer[i + 2] = 255;
                        resultBuffer[i + 3] = 255;
                    }
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
        public static Bitmap JIP_DrawHorizontalLinesX(this Bitmap sourceBitmap, double[,] filterMatrix, int filterThreshold = 30)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            float rgb = 0;

            for (int k = 0; k < pixelBuffer.Length; k += 4)
            {
                rgb = pixelBuffer[k] * 0.11f;
                rgb += pixelBuffer[k + 1] * 0.59f;
                rgb += pixelBuffer[k + 2] * 0.3f;


                pixelBuffer[k] = (byte)rgb;
                pixelBuffer[k + 1] = pixelBuffer[k];
                pixelBuffer[k + 2] = pixelBuffer[k];
                pixelBuffer[k + 3] = 255;
            }


            double gray = 0.0;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffsetWidth = (filterWidth) / 2;
            int filterOffsetHeight = (filterHeight) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            for (int offsetY = filterOffsetHeight; offsetY <
                sourceBitmap.Height - filterOffsetHeight; offsetY++)
            {
                for (int offsetX = filterOffsetWidth; offsetX <
                    sourceBitmap.Width - filterOffsetWidth; offsetX++)
                {
                    gray = 0;

                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;
                    int positives = 0, negatives = 0, zeros = 0;

                    for (int filterY = -filterOffsetHeight;
                        filterY < filterOffsetHeight; filterY++)
                    {
                        for (int filterX = -filterOffsetWidth;
                            filterX < filterOffsetWidth; filterX++)
                        {

                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);
                            if (filterMatrix[filterY + filterOffsetHeight, filterX + filterOffsetWidth] == 1)
                                positives += pixelBuffer[calcOffset];
                            else if(filterMatrix[filterY + filterOffsetHeight, filterX + filterOffsetWidth] == -1)
                                negatives += pixelBuffer[calcOffset];
                            else if(filterMatrix[filterY + filterOffsetHeight, filterX + filterOffsetWidth] == 0)
                                zeros += pixelBuffer[calcOffset];
                            /*gray += (double)(pixelBuffer[calcOffset]) *
                                   filterMatrix[filterY + filterOffsetHeight,
                                                       filterX + filterOffsetWidth];*/
                        }
                    }
                    positives /= filterWidth;
                    negatives /= filterWidth;
                    zeros /= 20;
                    bool isLine = true;
                    for (int filterY = -filterOffsetHeight;
                        filterY < filterOffsetHeight; filterY++)
                    {
                        for (int filterX = -filterOffsetWidth;
                            filterX < filterOffsetWidth; filterX++)
                        {

                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);
                            if (filterMatrix[filterY + filterOffsetHeight, filterX + filterOffsetWidth] == 1)
                            {
                                if (!(Math.Abs(positives - pixelBuffer[calcOffset]) < filterThreshold))
                                    isLine = false;
                            }
                            else if (filterMatrix[filterY + filterOffsetHeight, filterX + filterOffsetWidth] == -1)
                            {
                                if (!(Math.Abs(positives - pixelBuffer[calcOffset]) > filterThreshold))
                                    isLine = false;
                            }
                            //else if (filterMatrix[filterY + filterOffsetHeight, filterX + filterOffsetWidth] == 0)
                                //zeros += pixelBuffer[calcOffset];
                        }
                    }
                    if (isLine)
                        gray = 255;
                    else
                        gray = 0;
                    int threshold = 0;
                    if (gray > 255 - threshold)//255
                    { gray = 255; }
                    else if (gray <= 0 + threshold)//0
                    { gray = 0; }


                    resultBuffer[byteOffset] = (byte)(gray);
                    resultBuffer[byteOffset + 1] = (byte)(gray);
                    resultBuffer[byteOffset + 2] = (byte)(gray);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
    }
    public class JIP_Line
    {
        public Point A,B;
        public JIP_Line(int x1,int y1,int x2,int y2)
        {
            A = new Point(x1,y1);
            B = new Point(x2,y2);
        }
        public int Length
        {
            get 
            {
                if(A.X - B.X == 0)
                    return Math.Abs(A.Y - B.Y);
                else
                    return Math.Abs(A.X - B.X);
            }
        }
    }
    public static class JIP_Matrix
    {
        public static double[,] HorizontalLineA
        {
            get
            {
                return new double[,]  
                { { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },//20
                  {  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
            }
        }
        public static double[,] Laplacian3x3
        {
            get
            {
                return new double[,]  
                { { -1, -1, -1,  }, 
                  { -1,  8, -1,  }, 
                  { -1, -1, -1,  }, };
            }
        }

        public static double[,] RunningAppPattern
        {
            get
            {
                return new double[,]
                {
                    { 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60},
                    { 60,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160,160, 60},
                    { 60,160,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,105,160, 60}
                };
            }
        }
    }
}
