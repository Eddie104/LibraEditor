using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Timers;
using LibraEditor.mapEditor.view.mapLayer;

namespace LibraEditor.mapEditor.model
{
    enum ProjectType
    {
        quick,
        egret
    }

    /// <summary>
    /// 视角类型
    /// </summary>
    enum ViewType
    {
        /// <summary>
        /// 正视角
        /// </summary>
        tile,
        /// <summary>
        /// 斜视角
        /// </summary>
        iso
    }

    class MapData
    {

        public static int resID = 36;

        private static MapData instance;

        /// <summary>
        /// 项目所使用的游戏引擎
        /// </summary>
        public ProjectType ProjectType { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 视角类型
        /// </summary>
        public ViewType ViewType { get; set; }

        /// <summary>
        /// 格子宽度
        /// </summary>
        public int CellWidth { get; set; }

        /// <summary>
        /// 格子高度
        /// </summary>
        public int CellHeight { get; set; }

        /// <summary>
        /// 格子行数
        /// </summary>
        public int CellRows { get; set; }

        /// <summary>
        /// 格子列数
        /// </summary>
        public int CellCols { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 资源列表
        /// </summary>
        public List<MapRes> ResList { get; set; }

        /// <summary>
        /// 图层列表
        /// </summary>
        public List<LayerData> LayerList { get; set; }

        [JsonIgnore]
        public bool NeedSave { get; set; }

        public MapData()
        {
            ResList = new List<MapRes>();
            LayerList = new List<LayerData>();

            //每10秒就自动保存一次
            MainWindow.GetInstance().Timer.Elapsed += Save;
        }

        /// <summary>
        /// 添加一个资源文件
        /// </summary>
        /// <param name="path">文件的路径</param>
        public List<MapRes> AddMapRes(string[] path)
        {
            List<MapRes> result = new List<MapRes>();
            if (path != null)
            {
                MapRes r;
                foreach (string p in path)
                {
                    r = new MapRes(p);
                    ResList.Add(r);
                    result.Add(r);
                }
                this.NeedSave = true;
            }
            return result;
        }

        internal void RemoveMapRes(MapRes mapRes)
        {
            ResList.Remove(mapRes);
            this.NeedSave = true;
        }

        internal MapRes GetMapRes(string name)
        {
            foreach (var item in ResList)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 往图层里添加资源
        /// </summary>
        /// <param name="name">图层名</param>
        /// <param name="res">资源</param>
        internal void AddMapRes(string layerName, MapResView mapResView, int id)
        {
            foreach (var item in LayerList)
            {
                if (item.Name == layerName)
                {
                    item.AddRes(mapResView, id);
                    NeedSave = true;
                    break;
                }
            }
        }

        internal void UpdateMapRes(MapResView mapResView)
        {
            foreach (var item in LayerList)
            {
                if (item.UpdateRes(mapResView))
                {
                    NeedSave = true;
                    break;
                }
            }
        }

        public void Created()
        {
            //创建map的文件夹
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            this.NeedSave = true;
            Save();
        }

        /// <summary>
        /// 保存地图的配置文件
        /// </summary>
        public void Save(object sender = null, ElapsedEventArgs e = null)
        {
            if (NeedSave)
            {
                string cfgPath = string.Format("{0}\\{1}.json", Path, Name);
                string cfgJson = JsonConvert.SerializeObject(this, Formatting.Indented);
                using (StreamWriter sr = new StreamWriter(cfgPath))
                {
                    sr.Write(cfgJson);
                    NeedSave = false;
                }
            }
        }

        public static MapData GetInstance()
        {
            if (instance == null)
            {
                instance = new MapData();
            }
            return instance;
        }

        internal static void CreateWithJson(string mapJsonPath)
        {
            using (StreamReader sr = new StreamReader(mapJsonPath))
            {
                string jsonTxt = sr.ReadToEnd();
                instance = JsonConvert.DeserializeObject(jsonTxt, typeof(MapData)) as MapData;
            }
        }
    }
}
