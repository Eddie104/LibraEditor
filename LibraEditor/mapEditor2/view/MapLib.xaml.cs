using LibraEditor.mapEditor2.model.data;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// LayerLib.xaml 的交互逻辑
    /// </summary>
    public partial class MapLib : Expander
    {

        public event EventHandler MapDataChangedHandler;

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

        private async void OnCreateMap(object sender, RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(MapEditor.GetInstance(), "新建地图", "请输入地图名:");
            if (!string.IsNullOrEmpty(name))
            {
                var mapData = GameData.GetInstance().AddMapData(name);
                if (mapData != null)
                {
                    AddMapItem(mapData.Name);
                }
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
                    string name = (layerListBox.SelectedItem as LayerListBoxItem).Name;
                    curMapData.RemoveLayerData(name);
                    layerListBox.Items.Remove(layerListBox.SelectedItem);
                }
            }
        }
    }
}
