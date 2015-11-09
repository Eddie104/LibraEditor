using Libra.helper;
using LibraEditor.mapEditor2.model.data;
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
        private void OnCreateProject(object sender, RoutedEventArgs e)
        {
            CreateProjectWin win = new CreateProjectWin();
            win.CreateMapHandler += OnCreateMap;
            win.ShowDialog();            
        }

        private void OnCreateMap(object sender, EventArgs e)
        {
            resLib.InitWithGamedata();
            mapLib.InitWithGamedata();
        }

        public static MapEditor GetInstance()
        {
            return instance;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mapLib.MapDataChangedHandler += MapLib_MapDataChangedHandler;
        }

        private void MapLib_MapDataChangedHandler(object sender, EventArgs e)
        {
            mapCanvas.UpdateMapData(sender as MapData);
        }
    }
}
