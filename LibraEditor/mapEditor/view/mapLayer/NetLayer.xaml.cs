using LibraEditor.libra.util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// NetLayer.xaml 的交互逻辑
    /// </summary>
    public partial class NetLayer : UserControl
    {
        public NetLayer()
        {
            InitializeComponent();
        }

        public void CreateMap()
        {
            List<LinePoint> points = new List<LinePoint>();

            int startX = 0;int startY = 0;
            if (Config.ViewType == ViewType.tile)
            {
                int totalWidth = Config.CellWidth * Config.CellCols;
                int totalHeight = Config.CellHeight * Config.CellRows;
                startX = (int)(this.canvas.ActualWidth - totalWidth) / 2;
                startY = (int)(this.canvas.ActualHeight - totalHeight) / 2;
                RectangularHelper.TopPoint = new Point(startX, startY);

                Point index = RectangularHelper.GetItemIndex(new Point(startX, startY));
                index = RectangularHelper.GetItemPos((int)index.X, (int)index.Y);
                startX = (int)index.X;startY = (int)index.Y;
                for (int row = 0; row <= Config.CellRows; row++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX, startY + row * Config.CellHeight),
                        EndPoint = new Point(startX + Config.CellCols * Config.CellWidth, startY + row * Config.CellHeight)
                    });
                }
                for (int col = 0; col <= Config.CellCols; col++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX + col * Config.CellWidth, startY),
                        EndPoint = new Point(startX + col * Config.CellWidth, startY + Config.CellRows * Config.CellHeight)
                    });
                }
            }
            else if (Config.ViewType == ViewType.iso)
            {
                int totalWidth = (Config.CellRows + Config.CellCols) * Config.CellWidth / 2;
                int totalHeight = (Config.CellRows + Config.CellCols) * Config.CellHeight / 2;
                ISOHelper.TopPoint = new Point(this.canvas.ActualWidth / 2 - (totalWidth -  Config.CellWidth * Config.CellRows) / 2,
                    (int)Math.Floor((this.canvas.ActualHeight - totalHeight) * 0.5));

                double endX = Config.CellCols * Config.CellWidth / 2;
                double endY = endX / 2;
                Point p;
                for (int row = 0; row <= Config.CellRows; row++)
                {
                    p = new Point(ISOHelper.TopPoint.X - Config.CellWidth / 2 * row,
                            ISOHelper.TopPoint.Y + Config.CellHeight / 2 * row);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = p + new Vector(endX, endY)
                    });
                }

                endX = Config.CellRows * Config.CellWidth / 2;
                endY = endX / 2;
                for (int col = 0; col <= Config.CellCols; col++)
                {
                    p = new Point(ISOHelper.TopPoint.X + Config.CellWidth / 2 * col,
                        ISOHelper.TopPoint.Y + Config.CellHeight / 2 * col);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = new Point(p.X - endX, p.Y + endY)
                    });
                }
            }

            GraphicsHelper.Draw(this.canvas, points, Brushes.Black);
        }
    }
}
