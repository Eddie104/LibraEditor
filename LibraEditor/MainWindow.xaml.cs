using LibraEditor.egret.resourceTool;
using LibraEditor.mapEditor.view.newMap;
using LibraEditor.plistTool;
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
            Config.init();
            instance = this;
        }

        private void NewMapButton_Click(object sender, RoutedEventArgs e)
        {
            NewMap newMap = new NewMap();
            newMap.ShowDialog();
        }

        private void OnShowPlistTool(object sender, RoutedEventArgs e)
        {
            PlistTool plistTool = new PlistTool();
            plistTool.ShowDialog();
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }

        private void OnShowEgretResTool(object sender, RoutedEventArgs e)
        {
            ResourceTool win = new ResourceTool();
            win.ShowDialog();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.Save();
        }
    }
}
