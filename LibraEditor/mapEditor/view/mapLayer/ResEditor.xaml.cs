﻿using Libra.helper;
using LibraEditor.mapEditor.model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// ResEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ResEditor : MetroWindow
    {
        enum EditType
        {
            Offset, UnderSider
        }

        private EditType curEditType = EditType.Offset;

        private MapRes mapRes;

        private MapResView mapResView;

        private Point oldTopPoint;

        public ResEditor()
        {
            InitializeComponent();
        }

        public ResEditor(MapRes mapRes)
        {
            InitializeComponent();
            this.mapRes = mapRes;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawNet();
            InitMapRes();
        }

        private void InitMapRes()
        {
            mapResView = new MapResView(mapRes, true);
            mapResView.SetRowAndCol(0, 0);
            resCanvas.Children.Add(mapResView);

            this.mapResView.MouseLeftButtonUp += OnMouseLeftButtonUp;

            this.offsetXNumericUpDown.Value = mapRes.OffsetX;
            this.offsetYNumericUpDown.Value = mapRes.OffsetY;

            DrawUndersideNet();
        }

        private void DrawNet(int rows = 10, int cols = 10)
        {
            List<LinePoint> points = new List<LinePoint>();
            MapData mapData = MapData.GetInstance();
            ICoordinateHelper helper = MainWindow.GetInstance().CoordinateHelper;
            if (mapData.ViewType == ViewType.tile)
            {
                int startX = 0; int startY = 0;
                int totalWidth = mapData.CellWidth * cols;
                int totalHeight = mapData.CellHeight * rows;
                startX = (int)(canvas.ActualWidth - totalWidth) / 2;
                startY = (int)(canvas.ActualHeight - totalHeight) / 2;
                oldTopPoint = helper.TopPoint;
                helper.TopPoint = new Point(startX, startY);

                Point index = helper.GetItemIndex(new Point(startX, startY));
                index = helper.GetItemPos((int)index.X, (int)index.Y);
                startX = (int)index.X; startY = (int)index.Y;
                for (int row = 0; row <= rows; row++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX, startY + row * mapData.CellHeight),
                        EndPoint = new Point(startX + cols * mapData.CellWidth, startY + row * mapData.CellHeight)
                    });
                }
                for (int col = 0; col <= cols; col++)
                {
                    points.Add(new LinePoint()
                    {
                        StartPoint = new Point(startX + col * mapData.CellWidth, startY),
                        EndPoint = new Point(startX + col * mapData.CellWidth, startY + rows * mapData.CellHeight)
                    });
                }
            }
            else if (mapData.ViewType == ViewType.iso)
            {
                int totalWidth = (rows + cols) * mapData.CellWidth / 2;
                int totalHeight = (rows + cols) * mapData.CellHeight / 2;
                oldTopPoint = helper.TopPoint;
                helper.TopPoint = new Point(canvas.ActualWidth / 2 - (totalWidth - mapData.CellWidth * rows) / 2,
                    (int)Math.Floor((canvas.ActualHeight - totalHeight) * 0.5));

                double endX = cols * mapData.CellWidth / 2;
                double endY = endX / 2;
                Point p;
                for (int row = 0; row <= rows; row++)
                {
                    p = new Point(helper.TopPoint.X - mapData.CellWidth / 2 * row,
                            helper.TopPoint.Y + mapData.CellHeight / 2 * row);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = p + new Vector(endX, endY)
                    });
                }

                endX = rows * mapData.CellWidth / 2;
                endY = endX / 2;
                for (int col = 0; col <= cols; col++)
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
            GraphicsHelper.Draw(canvas, points, Brushes.Black);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ICoordinateHelper helper = MainWindow.GetInstance().CoordinateHelper;
            if (curEditType == EditType.Offset)
            {
                var t = mapResView.GetValue(Canvas.LeftProperty);
                int x = 0, y = 0;
                int.TryParse(mapResView.GetValue(Canvas.LeftProperty).ToString(), out x);
                int.TryParse(mapResView.GetValue(Canvas.TopProperty).ToString(), out y);

                Point topPoint = helper.TopPoint;
                int offsetX = (int)(topPoint.X - x);
                int offsetY = (int)(topPoint.Y - y);

                offsetXNumericUpDown.Value = offsetX;
                offsetYNumericUpDown.Value = offsetY;
            }
            else if (curEditType == EditType.UnderSider)
            {
                Point p = e.GetPosition(canvas);
                p = helper.GetItemIndex(p);
                if (p.X > -1 && p.Y > -1)
                {
                    mapRes.ChangeCover((int)p.Y, (int)p.X);
                    DrawUndersideNet();
                }
            }
        }

        private void DrawUndersideNet()
        {
            //绘制占地的格子
            undersideCanvas.Children.Clear();
            var ary = mapRes.UndersideAry;
            for (int row = 0; row < ary.GetLength(0); row++)
            {
                for (int col = 0; col < ary.GetLength(1); col++)
                {
                    if (ary[row, col] == 1)
                    {
                        DrawUndersideNet(row, col);
                    }
                }
            }
        }

        private void DrawUndersideNet(int row, int col)
        {
            ICoordinateHelper helper = MainWindow.GetInstance().CoordinateHelper;
            List<LinePoint> points = new List<LinePoint>();
            MapData mapData = MapData.GetInstance();
            if (mapData.ViewType == ViewType.tile)
            {
                int startX = 0; int startY = 0;
                Point index = helper.GetItemPos(row, col);
                startX = (int)index.X; startY = (int)index.Y;
                points.Add(new LinePoint()
                {
                    StartPoint = new Point(startX, startY + row * mapData.CellHeight),
                    EndPoint = new Point(startX + mapData.CellWidth, startY + row * mapData.CellHeight)
                });
                points.Add(new LinePoint()
                {
                    StartPoint = new Point(startX + col * mapData.CellWidth, startY),
                    EndPoint = new Point(startX + col * mapData.CellWidth, startY + mapData.CellHeight)
                });
            }
            else if (mapData.ViewType == ViewType.iso)
            {
                double endX = mapData.CellWidth / 2;
                double endY = endX / 2;
                Point p;
                for (int t = row; t <= row + 1; t++)
                {
                    p = helper.GetItemPos(t, col);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = p + new Vector(endX, endY)
                    });
                }

                for (int t = col; t <= col + 1; t++)
                {
                    p = helper.GetItemPos(row, t);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = new Point(p.X - endX, p.Y + endY)
                    });
                }
            }
            GraphicsHelper.Draw(undersideCanvas, points, Brushes.Red, false);
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.GetInstance().CoordinateHelper.TopPoint = oldTopPoint;                
            mapRes.ResetUnderside();
        }

        private void offsetXNumericUpDown_ValueChanged(object sender, object e)
        {
            if (offsetXNumericUpDown.Value != null)
            {
                mapRes.OffsetX = (int)offsetXNumericUpDown.Value;
                MapData.GetInstance().NeedSave = true;
                mapResView.SetRowAndCol(mapResView.Row, mapResView.Col, true);
            }
        }

        private void offsetYNumericUpDown_ValueChanged(object sender, object e)
        {
            if (offsetYNumericUpDown.Value != null)
            {
                mapRes.OffsetY = (int)offsetYNumericUpDown.Value;
                MapData.GetInstance().NeedSave = true;
                mapResView.SetRowAndCol(mapResView.Row, mapResView.Col, true);
            }
        }

        private void OnStartEditOffset(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                curEditType = EditType.Offset;
                mapResView.IsCanDrag = true;
            }
        }

        private void OnStartEditUnderSider(object sender, RoutedEventArgs e)
        {
            curEditType = EditType.UnderSider;
            mapResView.IsCanDrag = false;
        }
    }
}
