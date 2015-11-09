
using LibraEditor.mapEditor2.model.data;
using System.Windows.Controls;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// LayerItem.xaml 的交互逻辑
    /// </summary>
    public partial class LayerListBoxItem : Grid
    {

        public LayerListBoxItem(string layerName)
        {
            InitializeComponent();

            Name = layerName;
            nameLabel.Content = layerName;
        }
    }
}
