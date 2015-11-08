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
    public partial class LayerLib : Expander
    {
        public LayerLib()
        {
            InitializeComponent();
        }

        private void OnDeleteLayer(object sender, RoutedEventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                LayerListBoxItem item = layerListBox.SelectedItem as LayerListBoxItem;
                MapData.GetInstance().RemoveLayerData(item.LayerName);
                layerListBox.Items.Remove(item);
            }
        }

        private async void OnAddLayer(object sender, RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(MapEditor.GetInstance(), "新建层", "请输入层名:");
            if (!string.IsNullOrEmpty(name))
            {
                var layerData = MapData.GetInstance().AddLayerData(name);
                if (layerData != null)
                {
                    AddLayerItem(layerData.Name);
                }
            }
        }

        internal void InitWithMapdata()
        {
            MapData mapData = MapData.GetInstance();
            foreach (LayerData item in mapData.LayerDataList)
            {
                AddLayerItem(item.Name);
            }

            layerListBox.SelectedIndex = 0;
        }
        
        private void AddLayerItem(string name)
        {
            LayerListBoxItem item = new LayerListBoxItem(name);
            layerListBox.Items.Add(item);
        }
    }
}
