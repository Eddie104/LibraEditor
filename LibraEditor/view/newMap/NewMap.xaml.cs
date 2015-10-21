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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using libra.util;
using libra.log4CSharp;

namespace LibraEditor.view.newMap
{
    /// <summary>
    /// NewMap.xaml 的交互逻辑
    /// </summary>
    public partial class NewMap : MetroWindow
    {

        public NewMap()
        {
            InitializeComponent();
        }

        private void OnOpenDir(object sender, RoutedEventArgs e)
        {
            mapFloderTextBlock.Text = FileHelper.FindFolder();
            mapFloderTextBlock.ToolTip = mapFloderTextBlock.Text;
        }
    }
}
