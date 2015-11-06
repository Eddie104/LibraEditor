using Libra.helper;
using LibraEditor.mapEditor2.model.data;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// ResEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ResEditor : MetroWindow
    {

        /// <summary>
        /// 坐标和格子索引换算的工具
        /// </summary>
        private ICoordinateHelper coordinateHelper;

        private Prop prop = null;

        private Point oldPropPosition;

        public ResEditor()
        {
            InitializeComponent();

            MapData mapData = MapData.GetInstance();
            floorListBox.ItemsSource = mapData.FloorTypeList;
            buildingListBox.ItemsSource = mapData.BuildingTypeList;
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        private void DrawNet()
        {
            int canvasWidth = (int)canvas.ActualWidth;
            int canvasHeight = (int)canvas.ActualHeight;
            List<LinePoint> points = new List<LinePoint>();

            MapData mapData = MapData.GetInstance();
            GraphicsHelper.DrawNet(0, 0, 10, 10, mapData.CellWidth, mapData.CellHeight, canvasWidth, canvasHeight, canvas, mapData.ViewType == MapViewType.iso, out coordinateHelper);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawNet();
        }

        private void OnListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem;
            if (item != null)
            {
                if (prop != null)
                {
                    canvas.Children.Remove(prop);
                    prop.MouseLeftButtonDown -= Prop_MouseDown;
                    prop.MouseMove -= Prop_MouseMove;
                    prop.MouseLeftButtonUp -= Prop_MouseUp;
                    prop = null;
                }
                if (item is FloorTypeData)
                {
                    prop = new Floor(item as FloorTypeData);
                }
                else if (item is BuildingTypeData)
                {
                    prop = new Building(item as BuildingTypeData);
                }
                if (prop != null)
                {
                    canvas.Children.Add(prop);
                    prop.MouseLeftButtonDown += Prop_MouseDown;
                    prop.MouseMove += Prop_MouseMove;
                    prop.MouseLeftButtonUp += Prop_MouseUp;

                    offsetXNumericUpDown.Value = prop.GetData().OffsetX;
                    offsetYNumericUpDown.Value = prop.GetData().OffsetY;
                    prop.SetRowAndCol(0, 0, coordinateHelper);
                }
            }
        }

        private void Prop_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double t = (double)prop.GetValue(Canvas.LeftProperty);
            int propX = (int)t;
            t = (double)prop.GetValue(Canvas.TopProperty);
            int propY = (int)t;
            prop.GetData().OffsetX = (int)coordinateHelper.TopPoint.X - propX;
            prop.GetData().OffsetY = (int)coordinateHelper.TopPoint.Y - propY;
            MapData.GetInstance().NeedSave = true;
        }

        private void Prop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double xPos = e.GetPosition(canvas).X - oldPropPosition.X + (double)prop.GetValue(Canvas.LeftProperty);
                double yPos = e.GetPosition(canvas).Y - oldPropPosition.Y + (double)prop.GetValue(Canvas.TopProperty);
                prop.SetValue(Canvas.LeftProperty, xPos);
                prop.SetValue(Canvas.TopProperty, yPos);
                oldPropPosition = e.GetPosition(canvas);
            }
        }

        private void Prop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            oldPropPosition = e.GetPosition(canvas);
        }

        private void offsetXNumericUpDown_ValueChanged(object sender, object e)
        {
            if (prop != null)
            {
                if (offsetXNumericUpDown.Value != null && offsetYNumericUpDown.Value != null)
                {
                    prop.GetData().OffsetX = (int)offsetXNumericUpDown.Value;
                    prop.SetRowAndCol(0, 0, coordinateHelper);
                    MapData.GetInstance().NeedSave = true;
                }
            }
        }

        private void offsetYNumericUpDown_ValueChanged(object sender, object e)
        {
            if (prop != null)
            {
                if (offsetXNumericUpDown.Value != null && offsetYNumericUpDown.Value != null)
                {
                    prop.GetData().OffsetY = (int)offsetYNumericUpDown.Value;
                    prop.SetRowAndCol(0, 0, coordinateHelper);
                    MapData.GetInstance().NeedSave = true;
                }
            }
        }
    }
}
