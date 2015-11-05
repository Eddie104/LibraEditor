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
                    prop.SetRowAndCol(0, 0, coordinateHelper);
                    canvas.Children.Add(prop);
                    prop.MouseLeftButtonDown += Prop_MouseDown;
                    prop.MouseMove += Prop_MouseMove;
                    prop.MouseLeftButtonUp += Prop_MouseUp;
                }
            }
        }

        private void Prop_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
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

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var t = coordinateHelper.GetItemIndex(e.GetPosition(canvas));
            Console.WriteLine(string.Format("row = {0}, col = {1}", t.Y, t.X));
        }
    }
}
