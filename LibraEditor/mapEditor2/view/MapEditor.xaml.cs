using Libra.helper;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// MapEditor.xaml 的交互逻辑
    /// </summary>
    public partial class MapEditor : MetroWindow
    {

        private static MapEditor instance;

        public ICoordinateHelper CoordinateHelper { get; set; }

        public MapEditor()
        {
            InitializeComponent();
            instance = this;
        }

        /// <summary>
        /// 创建新地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateMap(object sender, RoutedEventArgs e)
        {
            CreateMapWin win = new CreateMapWin();
            win.CreateMapHandler += OnCreateMap;
            win.ShowDialog();            
        }

        private void OnCreateMap(object sender, EventArgs e)
        {
            resLib.InitWithMapdata();
            layerLib.InitWithMapdata();
        }

        public static MapEditor GetInstance()
        {
            return instance;
        }
    }
}
