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

        /// <summary>
        /// 是否显示放大镜
        /// </summary>
        private bool isMagnifierShowing = false;

        public ResEditor()
        {
            InitializeComponent();

            GameData gameData = GameData.GetInstance();
            floorListBox.ItemsSource = gameData.FloorTypeList;
            buildingListBox.ItemsSource = gameData.BuildingTypeList;
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        private void DrawNet()
        {
            int canvasWidth = (int)canvas.ActualWidth;
            int canvasHeight = (int)canvas.ActualHeight;
            List<LinePoint> points = new List<LinePoint>();

            GameData gameData = GameData.GetInstance();
            GraphicsHelper.DrawNet(0, 0, 10, 10, gameData.CellWidth, gameData.CellHeight, canvasWidth, canvasHeight, canvas, gameData.ViewType == MapViewType.iso, out coordinateHelper);
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
            offsetXNumericUpDown.Value = prop.GetData().OffsetX;
            offsetYNumericUpDown.Value = prop.GetData().OffsetY;
            GameData.GetInstance().NeedSave = true;
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
                    GameData.GetInstance().NeedSave = true;
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
                    GameData.GetInstance().NeedSave = true;
                }
            }
        }

        private void canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isMagnifierShowing)
            {
                Point rate = new Point(2, 2);
                //相对于outsideGrid 获取鼠标的坐标
                Point pos = e.MouseDevice.GetPosition(canvas);
                //这里的Viewbox和前台的一样   这里就是获取前台Viewbox的值
                Rect viewBox = vb.Viewbox;
                //因为鼠标要让它在矩形(放大镜)的中间  那么我们就要让矩形的左上角重新移动位置
                double xoffset = 0;
                double yoffset = 0;

                xoffset = magnifierEllipse.ActualWidth / 2;
                yoffset = magnifierEllipse.ActualHeight / 2;

                viewBox.X = pos.X - xoffset + (magnifierEllipse.ActualWidth - magnifierEllipse.ActualWidth / rate.X) / 2;
                viewBox.Y = pos.Y - yoffset + (magnifierEllipse.ActualHeight - magnifierEllipse.ActualHeight / rate.Y) / 2;
                vb.Viewbox = viewBox;
                //同理重新定位Canvas magnifierCanvas的坐标
                Canvas.SetLeft(magnifierCanvas, pos.X - xoffset);
                Canvas.SetTop(magnifierCanvas, pos.Y - yoffset);
            }
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            isMagnifierShowing = true;
            magnifierCanvas.Visibility = Visibility.Visible;
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            isMagnifierShowing = false;
            magnifierCanvas.Visibility = Visibility.Hidden;
        }
    }
}
