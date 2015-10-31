using LibraEditor.libra.util;
using LibraEditor.mapEditor.events;
using LibraEditor.mapEditor.model;
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

        private List<Canvas> layers = new List<Canvas>();

        private Canvas curLayer;

        public MapCanvas()
        {
            InitializeComponent();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point index = MapData.GetInstance().ViewType == ViewType.iso ? ISOHelper.GetItemIndex(e.GetPosition(netLayer)) : RectangularHelper.GetItemIndex(e.GetPosition(netLayer));
            mouseCursor.SetRowAndCol((int)index.Y, (int)index.X);
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
            if (curLayer != null)
            {
                //仅支持文件的拖放
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    //获取拖拽的文件
                    MapRes res = e.Data.GetData(DataFormats.FileDrop) as MapRes;
                    switch (res.ResType)
                    {
                        case ResType.jpg:
                        case ResType.png:
                            Image img = new Image();
                            BitmapImage bi = new BitmapImage(new Uri(res.Path, UriKind.Absolute));
                            img.Source = bi;
                            img.Width = bi.PixelWidth;
                            img.Height = bi.PixelHeight;
                            curLayer.Children.Add(img);
                            break;
                        default:
                            break;
                    }
                    //MapData.GetInstance().AddMapRes(curLayer.Name, res);
                }
            }
        }

        internal void ChangeLayerVisible(string name, bool isVisible)
        {
            var layer = GetLayer(name);
            layer.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        internal void AddLayer(string name)
        {
            Canvas layer = new Canvas();
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

        private Canvas GetLayer(string name)
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
