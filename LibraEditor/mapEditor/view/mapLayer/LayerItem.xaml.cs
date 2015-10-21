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
    /// LayerItem.xaml 的交互逻辑
    /// </summary>
    public partial class LayerItem : UserControl
    {

        public bool IsCanVisible { get; set; }

        public LayerItem(string name)
        {
            InitializeComponent();

            IsCanVisible = true;
            this.nameLabel.Content = name;
        }

        private void OnVisibleChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                IsCanVisible = (bool)(sender as CheckBox).IsChecked;
            }
        }
    }
}
