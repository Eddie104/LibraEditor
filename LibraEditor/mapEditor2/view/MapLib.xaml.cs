using LibraEditor.mapEditor2.model.data;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// LayerLib.xaml 的交互逻辑
    /// </summary>
    public partial class MapLib : Expander
    {

        public event EventHandler MapDataChangedHandler;

        public delegate void LayerDelegate(string name);
        public event LayerDelegate AddLayerHandler;

        public event LayerDelegate SelectedLayerChangedHandler;

        private MapData curMapData;

        public MapLib()
        {
            InitializeComponent();
        }

        internal void InitWithGamedata()
        {
            GameData gameData = GameData.GetInstance();
            foreach (MapData item in gameData.MapDataList)
            {
                AddMapItem(item.Name);
            }
            mapListBox.SelectedIndex = 0;
        }

        private void OnCreateMap(object sender, RoutedEventArgs e)
        {
            CreateMapWin win = new CreateMapWin();
            win.CreateMapHandler += Win_CreateMapHandler;
            win.ShowDialog();
        }

        private void Win_CreateMapHandler(string name, int rows, int cols)
        {
            var mapData = GameData.GetInstance().AddMapData(name, rows, cols);
            if (mapData != null)
            {
                AddMapItem(mapData.Name);
            }
        }

        private void OnDeleteMap(object sender, RoutedEventArgs e)
        {
            if (mapListBox.SelectedItem != null)
            {
                GameData.GetInstance().RemoveMapData((mapListBox.SelectedItem as MapBoxListItem).Name);
                mapListBox.Items.Remove(mapListBox.SelectedItem);
            }
        }

        private void AddMapItem(string name)
        {
            MapBoxListItem item = new MapBoxListItem(name);
            mapListBox.Items.Add(item);
        }

        private void OnMapDataSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mapListBox.SelectedItem != null)
            {
                curMapData = GameData.GetInstance().GetMapData((mapListBox.SelectedItem as MapBoxListItem).Name);
                layerListBox.Items.Clear();
                foreach (var item in curMapData.LayerDataList)
                {
                    layerListBox.Items.Add(item.Name);
                }

                MapDataChangedHandler(curMapData, null);
            }
        }

        private async void OnCreateLayer(object sender, RoutedEventArgs e)
        {
            if (curMapData == null)
            {
                await DialogManager.ShowMessageAsync(MapEditor.GetInstance(), "当前地图错误", "请选择一个地图先");
            }
            else
            {
                string name = await DialogManager.ShowInputAsync(MapEditor.GetInstance(), "新建图层", "请输入图层名:");
                if (!string.IsNullOrEmpty(name))
                {
                    curMapData.AddLayerData(name);
                    layerListBox.Items.Add(name);
                    AddLayerHandler(name);
                }
            }
        }

        private void OnDeleteLayer(object sender, RoutedEventArgs e)
        {
            if (curMapData == null)
            {
                DialogManager.ShowMessageAsync(MapEditor.GetInstance(), "当前地图错误", "请选择一个地图先");
            }
            else
            {
                if (layerListBox.SelectedItem != null)
                {
                    string name = layerListBox.SelectedItem.ToString();
                    curMapData.RemoveLayerData(name);
                    layerListBox.Items.Remove(layerListBox.SelectedItem);
                }
            }
        }

        private void OnLayerSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                string name = layerListBox.SelectedItem.ToString();
                SelectedLayerChangedHandler(name);
            }
        }
    }
}
