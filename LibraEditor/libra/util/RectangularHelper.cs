using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraEditor.libra.util
{
    class RectangularHelper
    {
        public static int Width { get; set; }

        public static int Height { get; set; }

        public static Point TopPoint { get; set; }

        /// <summary>
        /// 获得屏幕上点的方块索引
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        //public static Point GetItemIndexByPosition(Point p)
        //{
        //    //p = Trans45To90(p);
        //    //return new Point((int)(p.X / Width), (int)(p.Y / Height));

        //}

        /// <summary>
        /// 获得鼠标位置所在方块的索引值
        /// </summary>
        /// <param name="mouseP"></param>
        /// <returns></returns>
        public static Point GetItemIndex(Point mouseP)
        {
            //mouseP = Point.Subtract(mouseP, new Vector(TopPoint.X, TopPoint.Y));
            //double row = mouseP.Y / Height - mouseP.X / Width;
            //double col = mouseP.X / Width + mouseP.Y / Height;
            //return new Point(Math.Floor(col), Math.Floor(row));

            mouseP = Point.Subtract(mouseP, new Vector(TopPoint.X, TopPoint.Y));
            double row = mouseP.Y / Height;
            double col = mouseP.X / Width;
            return new Point(Math.Floor(col), Math.Floor(row));
        }

        /// <summary>
        /// 根据方块的索引值获取方块的屏幕坐标
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Point GetItemPos(int row, int col)
        {
            //return new Point((col - row) * (Width * .5) + TopPoint.X, (col + row) * (Height * .5) + TopPoint.Y);
            return new Point(col * Width + TopPoint.X, row * Height + TopPoint.Y);
        }
    }
}
