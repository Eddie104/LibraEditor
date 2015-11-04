using Libra.helper;
using LibraEditor.mapEditor.events;
using LibraEditor.mapEditor.model;
using LibraEditor.mapEditor.view.newMap;
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
    /// MapLayer.xaml 的交互逻辑
    /// </summary>
    public partial class MapLayer : UserControl
    {

        public MapLayer()
        {
            InitializeComponent();

            NewMap.CreateMapHandler += OnCreateMap;
        }

        //private void CreateResMenu(FrameworkElement element)
        //{
        //    ContextMenu cm = new ContextMenu();
        //    element.ContextMenu = cm;


        //    MenuItem item = new MenuItem();
        //    item.Header = "删除";
        //    item.Click += new RoutedEventHandler(OnResEditor);
        //    cm.Items.Add(item);
        //}

        //private void OnResEditor(object sender, RoutedEventArgs e)
        //{
        //    Console.WriteLine("aaaaaaaaa");
        //}

        /// <summary>
        /// 创建新地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateMap(object sender, EventArgs e)
        {
            mapCanvas.CreateMap();

            if (!layerListBox.Items.Contains(mapCanvas.NetLayerItem))
            {
                layerListBox.Items.Add(mapCanvas.NetLayerItem);
            }

            foreach (var item in MapData.GetInstance().ResList)
            {
                AddResToResLibListBox(item);
            }

            foreach (var item in MapData.GetInstance().LayerList)
            {
                AddLayerItem(new LayerItem(item));
            }

            mapCanvas.InitMapRes();

            IsEnabled = true;
        }

        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddLayerHandler(object sender, RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(MainWindow.GetInstance(), "新建图层", "请输入图层名");
            if (!string.IsNullOrEmpty(name))
            {
                LayerData layerData = new LayerData() { Name = name.Trim() };
                MapData.GetInstance().LayerList.Add(layerData);
                MapData.GetInstance().NeedSave = true;

                AddLayerItem(new LayerItem(layerData));
            }
        }

        private void AddLayerItem(LayerItem item)
        {
            item.VisibleChangedEvent += OnLayerItem_VisibleChangedEvent;
            layerListBox.Items.Add(item);

            mapCanvas.AddLayer(item.LayerData.Name);
        }

        private void OnLayerItem_VisibleChangedEvent(object sender, EventArgs e)
        {
            VisibleChangedEventArgs evt = e as VisibleChangedEventArgs;
            mapCanvas.ChangeLayerVisible(evt.Name, evt.IsVisible);
        }

        /// <summary>
        /// 删除选中的图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteLayerHandler(object sender, RoutedEventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                if (mapCanvas.NetLayerItem != layerListBox.SelectedItem)
                {
                    LayerItem item = layerListBox.SelectedItem as LayerItem;
                    MapData.GetInstance().LayerList.Remove(item.LayerData);
                    MapData.GetInstance().NeedSave = true;

                    layerListBox.Items.Remove(item);

                    mapCanvas.RemoveLayer(item.LayerData.Name);
                }
            }
        }

        /// <summary>
        /// 导入资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnImportRes(object sender, RoutedEventArgs e)
        {
            string[] resList = FileHelper.FindFile("*.png;*.jpg", true);
            if (resList != null)
            {
                MapData mapData = MapData.GetInstance();
                List<MapRes> newResList = mapData.AddMapRes(resList);
                mapData.Save();

                foreach (MapRes item in newResList)
                {
                    AddResToResLibListBox(item);
                }
            }
        }

        private void AddResToResLibListBox(MapRes res)
        {
            TextListBoxItem t = new TextListBoxItem(res);
            t.OnEdit += OnEditResFromLib;
            t.OnDel += OnDelResFromLib;
            resLibListBox.Items.Add(t);
        }

        private void OnEditResFromLib(object sender, EventArgs e)
        {
            ResEditor win = new ResEditor((sender as TextListBoxItem).Data as MapRes);
            win.ShowDialog();
        }

        private void OnDelResFromLib(object sender, EventArgs e)
        {
            TextListBoxItem t = sender as TextListBoxItem;
            resLibListBox.Items.Remove(t);

            MapData.GetInstance().RemoveMapRes(t.Data as MapRes);

            //TODO: 删除了库里的资源，那么地图上相关的资源也应该要删除掉
        }

        /// <summary>
        /// 选择资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResSelected(object sender, SelectionChangedEventArgs e)
        {
            if (resLibListBox.SelectedItem != null)
            {
                MapRes res = (resLibListBox.SelectedItem as TextListBoxItem).Data as MapRes;
                BitmapImage bi = new BitmapImage(new Uri(res.Path, UriKind.Absolute));
                previewImg.Source = bi;
                if (bi.PixelWidth > 200)
                {
                    previewImg.Width = 200;
                    previewImg.Height = bi.PixelHeight * 200 / bi.PixelWidth;
                }
                else if (bi.PixelHeight > 200)
                {
                    previewImg.Width = bi.PixelWidth * 200 / bi.PixelHeight;
                    previewImg.Height = 200;
                }
                else
                {
                    previewImg.Width = bi.PixelWidth;
                    previewImg.Height = bi.PixelHeight;
                }
            }
        }

        private void OnDragRes(object sender, MouseButtonEventArgs e)
        {
            if (resLibListBox.SelectedItem != null)
            {
                MapRes res = (resLibListBox.SelectedItem as TextListBoxItem).Data as MapRes;
                DragDrop.DoDragDrop(resLibListBox, new DataObject(DataFormats.FileDrop, res), DragDropEffects.Copy);
            }
        }

        private void OnLayerSelected(object sender, SelectionChangedEventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                if (layerListBox.SelectedItem != mapCanvas.NetLayerItem)
                {
                    mapCanvas.SetCurLayer((layerListBox.SelectedItem as LayerItem).LayerData.Name);
                }
            }
        }
    }
}
