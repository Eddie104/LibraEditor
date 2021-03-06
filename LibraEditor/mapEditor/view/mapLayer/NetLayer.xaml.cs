﻿using Libra.helper;
using LibraEditor.mapEditor.model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// NetLayer.xaml 的交互逻辑
    /// </summary>
    public partial class NetLayer : Canvas
    {
        public NetLayer()
        {
            InitializeComponent();
        }

        public void CreateMap(int canvasWidth, int canvasHeight)
        {
            this.Children.Clear();

            List<LinePoint> points = new List<LinePoint>();

            MapData mapData = MapData.GetInstance();
            int startX = 0;int startY = 0;
            ICoordinateHelper helper = MainWindow.GetInstance().CoordinateHelper;
            if (mapData.ViewType == ViewType.tile)
            {
                int totalWidth = mapData.CellWidth * mapData.CellCols;
                int totalHeight = mapData.CellHeight * mapData.CellRows;
                startX = (canvasWidth - totalWidth) / 2;
                startY = (canvasHeight - totalHeight) / 2;
                helper.TopPoint = new Point(startX, startY);

                Point index = helper.GetItemIndex(new Point(startX, startY));
                index = helper.GetItemPos((int)index.X, (int)index.Y);
                startX = (int)index.X;startY = (int)index.Y;
                for (int row = 0; row <= mapData.CellRows; row++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX, startY + row * mapData.CellHeight),
                        EndPoint = new Point(startX + mapData.CellCols * mapData.CellWidth, startY + row * mapData.CellHeight)
                    });
                }
                for (int col = 0; col <= mapData.CellCols; col++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX + col * mapData.CellWidth, startY),
                        EndPoint = new Point(startX + col * mapData.CellWidth, startY + mapData.CellRows * mapData.CellHeight)
                    });
                }
            }
            else if (mapData.ViewType == ViewType.iso)
            {
                int totalWidth = (mapData.CellRows + mapData.CellCols) * mapData.CellWidth / 2;
                int totalHeight = (mapData.CellRows + mapData.CellCols) * mapData.CellHeight / 2;
                helper.TopPoint = new Point(canvasWidth / 2 - (totalWidth - mapData.CellWidth * mapData.CellRows) / 2,
                    (int)Math.Floor((canvasHeight - totalHeight) * 0.5));

                double endX = mapData.CellCols * mapData.CellWidth / 2;
                double endY = endX / 2;
                Point p;
                for (int row = 0; row <= mapData.CellRows; row++)
                {
                    p = new Point(helper.TopPoint.X - mapData.CellWidth / 2 * row,
                            helper.TopPoint.Y + mapData.CellHeight / 2 * row);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = p + new Vector(endX, endY)
                    });
                }

                endX = mapData.CellRows * mapData.CellWidth / 2;
                endY = endX / 2;
                for (int col = 0; col <= mapData.CellCols; col++)
                {
                    p = new Point(helper.TopPoint.X + mapData.CellWidth / 2 * col,
                        helper.TopPoint.Y + mapData.CellHeight / 2 * col);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = new Point(p.X - endX, p.Y + endY)
                    });
                }
            }

            GraphicsHelper.Draw(this, points, Brushes.Black);
        }
    }
}
