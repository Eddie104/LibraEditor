using libra.util;
using LibraEditor.libra.util;
using LibraEditor.mapEditor.events;
using LibraEditor.mapEditor.model;
using LibraEditor.mapEditor.view.newMap;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// MapLayer.xaml 的交互逻辑
    /// </summary>
    public partial class MapLayer : UserControl
    {

        private NetLayerItem netLayerItem;

        public MapLayer()
        {
            InitializeComponent();

            NewMap.CreateMapHandler += OnCreateMap;
        }

        private void OnCreateMap(object sender, EventArgs e)
        {
            netLayer.CreateMap();
            mouseCursor.DrawCursor();

            if (netLayerItem == null)
            {
                netLayerItem = new NetLayerItem();
                netLayerItem.VisibleChanged += OnNetLayerItemVisibleChanged;
            }
            if (!layerListBox.Items.Contains(netLayerItem))
            {
                layerListBox.Items.Add(netLayerItem);
            }

            this.IsEnabled = true;
        }

        private void OnNetLayerItemVisibleChanged(object sender, EventArgs e)
        {
            netLayer.Visibility = (e as VisibleChangedEventArgs).IsVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private async void OnAddLayerHandler(object sender, RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(MainWindow.GetInstance(), "新建图层", "请输入图层名");
            if (!string.IsNullOrEmpty(name))
            {
                if (RegularHelper.IsLetterAndNumber(name))
                {
                    layerListBox.Items.Add(new LayerItem(name));
                }
                else
                {
                    await DialogManager.ShowMessageAsync(MainWindow.GetInstance(), "图层名错误", "图层名只能以字母开头并且只能包含字母、数字和下划线");
                }
            }
        }

        private void OnDeleteLayerHandler(object sender, RoutedEventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                layerListBox.Items.Remove(layerListBox.SelectedItem);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point index = MapData.GetInstance().ViewType == ViewType.iso ? ISOHelper.GetItemIndex(e.GetPosition(netLayer)) : RectangularHelper.GetItemIndex(e.GetPosition(netLayer));
            mouseCursor.SetRowAndCol((int)index.Y, (int)index.X);
        }

        private void OnImportRes(object sender, RoutedEventArgs e)
        {
            FileHelper.FindFile("*.png;*.jpg");
        }
    }
}
