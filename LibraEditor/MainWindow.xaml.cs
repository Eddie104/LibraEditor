using LibraEditor.egret.resourceTool;
using LibraEditor.mapEditor.model;
using LibraEditor.mapEditor.view.newMap;
using LibraEditor.plistTool;
using MahApps.Metro.Controls;
using System.Timers;
using System.Windows;

namespace LibraEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private static MainWindow instance;

        public Timer Timer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Config.GetInstance();
            instance = this;

            Timer = new Timer(10000);
            Timer.AutoReset = true;
            Timer.Start();
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
