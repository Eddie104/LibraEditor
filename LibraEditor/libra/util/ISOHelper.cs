using System;
using System.Windows;

namespace LibraEditor.libra.util
{
    class ISOHelper
    {

        public static int Width { get; set; }

        public static int Height { get; set; }

        public static Point TopPoint { get; set; }

        private static int wh = 2;

        /// <summary>
        /// 获得屏幕上点的方块索引
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        //public static Point GetItemIndexByPosition(Point p)
        //{
        //    p = Trans45To90(p);
        //    return new Point((int)(p.X / Width), (int)(p.Y / Height));
        //}

        /// <summary>
        /// 获得鼠标位置所在方块的索引值
        /// </summary>
        /// <param name="mouseP"></param>
        /// <returns></returns>
        public static Point GetItemIndex(Point mouseP)
        {
            mouseP = Point.Subtract(mouseP, new Vector(TopPoint.X, TopPoint.Y));
            double row = mouseP.Y / Height - mouseP.X / Width;
            double col = mouseP.X / Width + mouseP.Y / Height;
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
            return new Point((col - row) * (Width * .5) + TopPoint.X, (col + row) * (Height * .5) + TopPoint.Y);
        }

        /// <summary>
        /// 从45度显示坐标换算为90度数据坐标
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point Trans45To90(Point p)
        {
            return new Point(p.X + p.Y * wh, p.Y - p.X / wh);
        }

        /// <summary>
        /// 从90度数据坐标换算为45度显示坐标
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point Trans90To45(Point p)
        {
            return new Point((p.X - p.Y * wh) * .5, (p.X / wh + p.Y) * .5);
        }
    }
}
