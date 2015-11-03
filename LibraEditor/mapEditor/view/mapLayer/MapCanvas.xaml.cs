using Libra.helper;
using LibraEditor.mapEditor.events;
using LibraEditor.mapEditor.model;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// MapCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class MapCanvas : Canvas
    {

        public NetLayerItem NetLayerItem { get; set; }

        private List<MapLayerView> layers = new List<MapLayerView>();

        private MapLayerView curLayer;

        private MapResView selectedMapResView;

        public MapCanvas()
        {
            InitializeComponent();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point index = MapData.GetInstance().ViewType == ViewType.iso ? ISOHelper.GetItemIndex(e.GetPosition(netLayer)) : RectangularHelper.GetItemIndex(e.GetPosition(netLayer));
            mouseCursor.SetRowAndCol((int)index.Y, (int)index.X);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (selectedMapResView != null)
                {
                    selectedMapResView.SetRowAndCol((int)index.Y, (int)index.X);
                }
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            selectedMapResView = null;
        }

        internal void CreateMap()
        {
            netLayer.CreateMap((int)ActualWidth, (int)ActualHeight);
            mouseCursor.DrawCursor();

            if (NetLayerItem == null)
            {
                NetLayerItem = new NetLayerItem();
                NetLayerItem.VisibleChanged += OnNetLayerItemVisibleChanged;
            }
        }

        /// <summary>
        /// 网格层的是否可视事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNetLayerItemVisibleChanged(object sender, EventArgs e)
        {
            netLayer.Visibility = (e as VisibleChangedEventArgs).IsVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            //仅支持文件的拖放
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //获取拖拽的文件
                MapRes res = e.Data.GetData(DataFormats.FileDrop) as MapRes;
                if (res != null)
                {
                    if (curLayer != null)
                    {
                        switch (res.ResType)
                        {
                            case ResType.jpg:
                            case ResType.png:
                                MapResView resView = new MapResView(res);
                                resView.SetRowAndCol(0, 0);
                                curLayer.Children.Add(resView);

                                resView.MouseDown += ResView_MouseDown;
                                break;
                            default:
                                break;
                        }
                        //MapData.GetInstance().AddMapRes(curLayer.Name, res);
                    }
                    else
                    {
                        DialogManager.ShowMessageAsync(MainWindow.GetInstance(), "层错误", "请选择真确的层");
                    }
                }
            }
        }

        private void ResView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetSelectdMapResView(sender as MapResView);
        }

        private void SetSelectdMapResView(MapResView resView)
        {
            selectedMapResView = resView;
        }

        internal void ChangeLayerVisible(string name, bool isVisible)
        {
            var layer = GetLayer(name);
            layer.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        internal void AddLayer(string name)
        {
            MapLayerView layer = new MapLayerView();
            layer.Name = name;
            Children.Add(layer);
            layers.Add(layer);
        }

        internal void RemoveLayer(string name)
        {
            foreach (var item in layers)
            {
                if (item.Name == name)
                {
                    layers.Remove(item);
                    Children.Remove(item);
                    return;
                }
            }
        }

        private MapLayerView GetLayer(string name)
        {
            foreach (var item in layers)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        internal void SetCurLayer(string name)
        {
            curLayer = GetLayer(name);
        }
    }
}
