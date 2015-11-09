using Libra.helper;
using LibraEditor.mapEditor2.model.data;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// ResEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ResEditor : MetroWindow
    {

        enum EditorType
        {
            MOVE, UNDER_SIDE
        }

        /// <summary>
        /// 坐标和格子索引换算的工具
        /// </summary>
        private ICoordinateHelper coordinateHelper;

        private Prop prop = null;

        private Point oldPropPosition;

        private EditorType curEditorType = EditorType.MOVE;

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

                    DrawUndersideNet();
                }
            }
        }

        private void Prop_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (curEditorType == EditorType.MOVE)
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
            else if (curEditorType == EditorType.UNDER_SIDE)
            {
                Point p = e.GetPosition(canvas);
                p = coordinateHelper.GetItemIndex(p);
                if (p.X > -1 && p.Y > -1)
                {
                    prop.Data.ChangeUnderSide((int)p.Y, (int)p.X);
                    DrawUndersideNet();
                }
            }
        }

        private void DrawUndersideNet()
        {
            //绘制占地的格子
            underSideCanvas.Children.Clear();
            var ary = prop.Data.UndersideAry;
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
            List<LinePoint> points = new List<LinePoint>();
            GameData gameData = GameData.GetInstance();
            if (gameData.ViewType == MapViewType.tile)
            {
                int startX = 0; int startY = 0;
                Point index = coordinateHelper.GetItemPos(row, col);
                startX = (int)index.X; startY = (int)index.Y;
                points.Add(new LinePoint()
                {
                    StartPoint = new Point(startX, startY + row * gameData.CellHeight),
                    EndPoint = new Point(startX + gameData.CellWidth, startY + row * gameData.CellHeight)
                });
                points.Add(new LinePoint()
                {
                    StartPoint = new Point(startX + col * gameData.CellWidth, startY),
                    EndPoint = new Point(startX + col * gameData.CellWidth, startY + gameData.CellHeight)
                });
            }
            else if (gameData.ViewType == MapViewType.iso)
            {
                double endX = gameData.CellWidth / 2;
                double endY = endX / 2;
                Point p;
                for (int t = row; t <= row + 1; t++)
                {
                    p = coordinateHelper.GetItemPos(t, col);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = p + new Vector(endX, endY)
                    });
                }

                for (int t = col; t <= col + 1; t++)
                {
                    p = coordinateHelper.GetItemPos(row, t);
                    points.Add(new LinePoint()
                    {
                        StartPoint = p,
                        EndPoint = new Point(p.X - endX, p.Y + endY)
                    });
                }
            }
            GraphicsHelper.Draw(underSideCanvas, points, Brushes.Red, false);
        }

        private void Prop_MouseMove(object sender, MouseEventArgs e)
        {
            if (curEditorType == EditorType.MOVE)
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
        }

        private void Prop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (curEditorType == EditorType.MOVE)
            {
                oldPropPosition = e.GetPosition(canvas);
            }
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

        private void OnEditprTypeChanged(object sender, RoutedEventArgs e)
        {
            if (IsInitialized)
            {
                if (moveCheckBox.IsChecked == true)
                {
                    curEditorType = EditorType.MOVE;
                    underSideCanvas.Visibility = Visibility.Hidden;
                }
                else if (underSideCheckBox.IsChecked == true)
                {
                    curEditorType = EditorType.UNDER_SIDE;
                    underSideCanvas.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
