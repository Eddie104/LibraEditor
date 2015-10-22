using LibraEditor.libra.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// MouseCursor.xaml 的交互逻辑
    /// </summary>
    public partial class MouseCursor : UserControl
    {

        public int CurRow { get; set; }

        public int CurCol { get; set; }

        public MouseCursor()
        {
            InitializeComponent();
        }

        public void DrawCursor()
        {
            this.mouseCursor.Children.Clear();

            List<LinePoint> points = new List<LinePoint>();
            if (Config.ViewType == ViewType.iso)
            {
                double endX = Config.CellHeight;
                double endY = endX / 2;
                Point p;
                for (int row = 0; row < 2; row++)
                {
                    p = new Point(0 - Config.CellWidth / 2 * row, Config.CellHeight / 2 * row);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = p + new Vector(endX, endY)
                    });
                }
                for (int col = 0; col < 2; col++)
                {
                    p = new Point(Config.CellWidth / 2 * col, Config.CellHeight / 2 * col);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = new Point(p.X - endX, p.Y + endY)
                    });
                }
            }
            else if (Config.ViewType == ViewType.tile)
            {

            }
            GraphicsHelper.Draw(mouseCursor, points, Brushes.Red);
        }

        public void SetRowAndCol(int row, int col)
        {
            if (this.CurRow != row || this.CurCol != col)
            {
                CurRow = row;
                CurCol = col;
                Point p = ISOHelper.GetItemPos(row, col);
                Canvas.SetLeft(mouseCursor, p.X);
                Canvas.SetTop(mouseCursor, p.Y);
            }
        }

    }
}
