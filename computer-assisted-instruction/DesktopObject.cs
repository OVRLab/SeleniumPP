using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computer_assisted_instruction
{
    public class DesktopObject
    {
        public Bitmap Screen;
        private Bitmap obj=null,reserved=null;
        private Rectangle objPosition;
        public DesktopObject(Bitmap _Screen)
        {
            Screen = _Screen;
        }
        public DesktopObject(Bitmap _Screen, Bitmap _obj)
        {
            Screen = _Screen;
            obj = _obj;
        }
        public DesktopObject(Bitmap _Screen, Rectangle _objPosition)
        {
            Screen = _Screen;
            ObjPosition = _objPosition;
        }
        public Bitmap ObjBitmap
        {
            get 
            {
                if (obj == null && objPosition != null && objPosition.Width > 0 && objPosition.Height > 0)
                {
                    Bitmap taskbarBitmap = new Bitmap(objPosition.Width, objPosition.Height);
                    Graphics g = Graphics.FromImage(taskbarBitmap);
                    g.DrawImage(Screen, new Rectangle(0, 0, taskbarBitmap.Width, taskbarBitmap.Height),
                                        new Rectangle(objPosition.X, objPosition.Y, taskbarBitmap.Width, taskbarBitmap.Height),
                                        GraphicsUnit.Pixel);
                    obj = taskbarBitmap;
                }
                return obj;
            }
        }
        public Bitmap ReservedBitmap
        {
            get
            {
                if (reserved == null)
                {
                    Size reservedSize = new Size();
                    if (Screen.Width == objPosition.Width)
                        reservedSize.Width = Screen.Width;
                    else
                        reservedSize.Width = Screen.Width - objPosition.Width;
                    if (Screen.Height == objPosition.Height)
                        reservedSize.Height = Screen.Height;
                    else
                        reservedSize.Height = Screen.Height - objPosition.Height;

                    Bitmap reservedBitmap = new Bitmap(reservedSize.Width, reservedSize.Height);
                    Graphics g = Graphics.FromImage(reservedBitmap);
                    Point UpperLeft = new Point(0,0);
                    if (objPosition.X == 0 && objPosition.Y != 0)
                        UpperLeft = new Point(0, 0);
                    else if (objPosition.X == 0 && objPosition.Y == 0 && objPosition.Width != Screen.Width)
                        UpperLeft = new Point(objPosition.Width, 0);
                    else if (objPosition.X == 0 && objPosition.Y == 0 && objPosition.Width == Screen.Width)
                        UpperLeft = new Point(0,objPosition.Height);
                    else if (objPosition.X != 0 && objPosition.Y == 0)
                        UpperLeft = new Point(0, 0);
                    g.DrawImage(Screen, new Rectangle(0, 0, reservedBitmap.Width, reservedBitmap.Height),
                                        new Rectangle(UpperLeft.X, UpperLeft.Y, reservedBitmap.Width, reservedBitmap.Height),
                                        GraphicsUnit.Pixel);
                    reserved = reservedBitmap;
                }
                return reserved;
            }
        }

        public Rectangle ObjPosition
        {
            get { return objPosition; }
            set {
                    objPosition = value;
            }
        }
    }
}
