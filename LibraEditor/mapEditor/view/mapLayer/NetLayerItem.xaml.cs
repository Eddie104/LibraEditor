using LibraEditor.mapEditor.events;
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

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// NetLayerItem.xaml 的交互逻辑
    /// </summary>
    public partial class NetLayerItem : UserControl
    {

        public event EventHandler VisibleChanged;

        private bool isCanVisible = true;

        public NetLayerItem()
        {
            InitializeComponent();
        }

        private void OnVisibleChanged(object sender, RoutedEventArgs e)
        {
            isCanVisible = !isCanVisible;
            VisibleChanged(this, new VisibleChangedEventArgs(isCanVisible));
        }
    }
}
