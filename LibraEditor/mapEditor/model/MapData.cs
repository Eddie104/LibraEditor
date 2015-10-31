using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Timers;

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
        public void AddMapRes(string[] path)
        {
            if (path != null)
            {
                foreach (string p in path)
                {
                    ResList.Add(new MapRes(p));
                }
            }
        }

        ///// <summary>
        ///// 往图层里添加资源
        ///// </summary>
        ///// <param name="name">图层名</param>
        ///// <param name="res">资源</param>
        //internal void AddMapRes(string layerName, MapRes res)
        //{
        //    foreach (var item in LayerList)
        //    {
        //        if (item.Name == layerName)
        //        {
                    
        //            break;
        //        }
        //    }
        //}

        public void Created()
        {
            //创建map的文件夹
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
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
