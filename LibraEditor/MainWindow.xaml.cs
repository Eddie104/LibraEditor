using LibraEditor.mapEditor.view.newMap;
using MahApps.Metro.Controls;
using System.Windows;

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
