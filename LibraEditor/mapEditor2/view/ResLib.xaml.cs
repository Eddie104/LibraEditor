using Libra.helper;
using LibraEditor.mapEditor2.model.data;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// ResLib.xaml 的交互逻辑
    /// 资源库
    /// </summary>
    public partial class ResLib : Expander
    {
        public ResLib()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 导入资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnImportRes(object sender, RoutedEventArgs e)
        {
            MapData mapData = MapData.GetInstance();
            if (resTabControl.SelectedIndex == 0)
            {
                //导入地板资源
                string[] resList = FileHelper.FindFile("*.png", true);
                if (resList != null)
                {
                    mapData.AddFloorRes(resList);
                    foreach (FloorTypeData item in mapData.FloorTypeList)
                    {
                        AddResItem(item);
                    }
                }
            }
            else if (resTabControl.SelectedIndex == 1)
            {
                //导入建筑资源
                string[] resList = FileHelper.FindFile("*.png;*.jpg", true);
                if (resList != null)
                {
                    mapData.AddBuildingRes(resList);
                    foreach (BuildingTypeData item in mapData.BuildingTypeList)
                    {
                        AddResItem(item);
                    }
                }
            }            
        }

        internal void InitWithMapdata()
        {
            MapData mapData = MapData.GetInstance();
            foreach (FloorTypeData item in mapData.FloorTypeList)
            {
                AddResItem(item);
            }
            foreach (BuildingTypeData item in mapData.BuildingTypeList)
            {
                AddResItem(item);
            }

            floorResListBox.SelectedIndex = 0;
        }

        private void AddResItem(PropTypeData prop)
        {
            ListBox listBox = null;
            if (prop is FloorTypeData)
            {
                listBox = floorResListBox;
            }
            else if (prop is BuildingTypeData)
            {
                listBox = buildingResListBox;
            }

            foreach (ResListBoxItem item in listBox.Items)
            {
                if (item.PropData == prop)
                {
                    return;
                }
            }

            ResListBoxItem listBoxItem = new ResListBoxItem(prop);
            listBoxItem.OnDel += OnDelRes;
            listBoxItem.OnEdit += OnEditRes;
            listBox.Items.Add(listBoxItem);
        }

        private void OnEditRes(object sender, EventArgs e)
        {
            ResEditor win = new ResEditor();
            win.ShowDialog();
        }

        private void OnDelRes(object sender, EventArgs e)
        {
            MapData mapData = MapData.GetInstance();

            ResListBoxItem item = sender as ResListBoxItem;
            var propData = item.PropData;
            if (propData is FloorTypeData)
            {
                if (mapData.RemoveFloorRes(propData as FloorTypeData))
                {
                    item.OnDel -= OnDelRes;
                    item.OnEdit -= OnEditRes;
                    floorResListBox.Items.Remove(item);
                }
            }
        }

        private void OnResListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (sender as ListBox).SelectedItem;
            if (selectedItem != null)
            {
                var propData = (selectedItem as ResListBoxItem).PropData;
                if (File.Exists(propData.Path))
                {
                    BitmapImage bi = new BitmapImage(new Uri(propData.Path, UriKind.Absolute));
                    previewImg.Source = bi;
                    previewImg.Width = bi.PixelWidth;
                    previewImg.Height = bi.PixelHeight;
                }
                else
                {
                    DialogManager.ShowMessageAsync(MapEditor.GetInstance(), "资源不存在", string.Format("资源:{0}不存在啊", propData.Path));
                }
            }
        }
    }

    class ResListBoxItem : ListBoxItem
    {
        public event EventHandler OnEdit;

        public event EventHandler OnDel;

        public PropTypeData PropData { get; }

        public ResListBoxItem(PropTypeData prop) : base()
        {
            PropData = prop;

            this.Height = 28;
            this.Content = prop;

            ContextMenu menu = new ContextMenu();
            MenuItem editItem = new MenuItem();
            editItem.Header = "编辑";
            editItem.Click += EditItem_Click;
            menu.Items.Add(editItem);

            MenuItem delItem = new MenuItem();
            delItem.Header = "删除";
            delItem.Click += DelItem_Click;
            menu.Items.Add(delItem);

            ContextMenu = menu;
        }

        private void DelItem_Click(object sender, RoutedEventArgs e)
        {
            OnDel(this, null);
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            OnEdit(this, null);
        }
    }
}
