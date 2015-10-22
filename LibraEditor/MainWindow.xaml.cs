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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using LibraEditor.mapEditor.view.newMap;

namespace LibraEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }

        private void NewMapButton_Click(object sender, RoutedEventArgs e)
        {
            NewMap newMap = new NewMap();
            newMap.ShowDialog();
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }
    }
}
