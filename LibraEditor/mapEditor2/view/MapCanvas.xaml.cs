using Libra.helper;
using LibraEditor.mapEditor2.model.data;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// MapCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class MapCanvas : Canvas
    {

        private Layer netLayer;

        private List<Layer> layerList = new List<Layer>();

        private Layer curLayer;

        private Prop curProp;

        /// <summary>
        /// 坐标和格子索引换算的工具
        /// </summary>
        private ICoordinateHelper coordinateHelper;

        private MapData mapData;

        public MapCanvas()
        {
            InitializeComponent();
            netLayer = new Layer();
            Children.Add(netLayer);
        }

        internal void UpdateMapData(MapData mapData)
        {
            this.mapData = mapData;
            DrawNet();

            curProp = null;
            foreach (var item in layerList)
            {
                Children.Remove(item);
            }
            layerList.Clear();

            Layer l;
            foreach (var item in mapData.LayerDataList)
            {
                l = new Layer() { Name = item.Name};
                layerList.Add(l);
                Children.Add(l);
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            //仅支持文件的拖放
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //获取拖拽的文件
                PropTypeData res = e.Data.GetData(DataFormats.FileDrop) as PropTypeData;
                if (res != null)
                {
                    if (curLayer != null)
                    {
                        Prop prop = null;
                        var nameList = res.Name.Split(new char[] { '.' });
                        switch(nameList[nameList.Length - 1].ToLower())
                        {
                            case "png":
                            case "jpg":
                                if (res is FloorTypeData)
                                {
                                    prop = new Floor(res as FloorTypeData);
                                    prop.SetRowAndCol(0, 0, coordinateHelper);
                                    curLayer.Children.Add(prop);
                                }
                                break;
                        }
                        //    Prop prop = null;
                        //    switch (res.ResType)
                        //    {
                        //        case ResType.jpg:
                        //        case ResType.png:
                        //            prop = new MapResView(res);
                        //            prop.ID = MapData.resID++;
                        //            prop.SetRowAndCol(0, 0);
                        //            curLayer.Children.Add(prop);
                        //            prop.MouseDown += ResView_MouseDown;
                        //            break;
                        //        default:
                        //            break;
                        //    }
                        //    if (prop != null)
                        //    {
                        //        MapData.GetInstance().AddMapRes(curLayer.Name, prop, prop.ID);
                        //    }
                    }
                    else
                    {
                        DialogManager.ShowMessageAsync(MapEditor.GetInstance(), "层错误", "请选择真确的层");
                    }
                }
            }
        }

        internal void SelectedLayerChanged(string name)
        {
            curLayer = GetLayer(name);
        }

        internal void AddLayer(string name)
        {
            Layer layer = new Layer() { Name = name };
            Children.Add(layer);
            layerList.Add(layer);
        }

        internal bool RemoveLayer(string name)
        {
            foreach (Layer item in layerList)
            {
                if (item.Name == name)
                {
                    Children.Remove(item);
                    layerList.Remove(item);
                    return true;
                }
            }
            return false;
        }

        internal Layer GetLayer(string name)
        {
            foreach(Layer item in layerList)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        private void DrawNet()
        {
            int canvasWidth = (int)ActualWidth;
            int canvasHeight = (int)ActualHeight;
            GameData gameData = GameData.GetInstance();
            GraphicsHelper.DrawNet(0, 0, mapData.CellRows, mapData.CellCols, gameData.CellWidth, gameData.CellHeight, canvasWidth, canvasHeight, netLayer, gameData.ViewType == MapViewType.iso, out coordinateHelper);
        }
    }

    class Layer : Canvas
    {

    }
}
