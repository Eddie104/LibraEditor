using LibraEditor.mapEditor2.view;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace LibraEditor.mapEditor2.model.data
{

    /// <summary>
    /// 视角类型
    /// </summary>
    enum MapViewType
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

    class GameData
    {

        private static GameData instance;

        public MapViewType ViewType { get; set; }

        /// <summary>
        /// 格子宽度
        /// </summary>
        public double CellWidth { get; set; }

        /// <summary>
        /// 格子高度
        /// </summary>
        public double CellHeight { get; set; }

        ///// <summary>
        ///// 格子行数
        ///// </summary>
        //public int CellRows { get; set; }

        ///// <summary>
        ///// 格子列数
        ///// </summary>
        //public int CellCols { get; set; }

        /// <summary>
        /// 地图路径
        /// </summary>
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                //创建文件夹
                string floorPath = path + "\\floor";
                if (!Directory.Exists(floorPath))
                {
                    Directory.CreateDirectory(floorPath);
                }
                string buildingPath = path + "\\building";
                if (!Directory.Exists(buildingPath))
                {
                    Directory.CreateDirectory(buildingPath);
                }
            }
        }

        /// <summary>
        /// 地图名
        /// </summary>
        public string Name { get; set; }

        public List<FloorTypeData> FloorTypeList { get; set; }

        public List<BuildingTypeData> BuildingTypeList { get; set; }

        public List<MapData> MapDataList { get; set; }

        [JsonIgnore]
        public bool NeedSave { get; set; }

        public GameData()
        {
            FloorTypeList = new List<FloorTypeData>();
            BuildingTypeList = new List<BuildingTypeData>();
            MapDataList = new List<MapData>();

            //每10秒就自动保存一次
            MainWindow.GetInstance().Timer.Elapsed += Save;
        }

        public void AddFloorRes(string[] resPathList)
        {
            string floorPath = Path + "\\floor";
            string name;
            foreach (string path in resPathList)
            {
                name = System.IO.Path.GetFileName(path);
                if (GetFloorRes(name) == null)
                {
                    File.Copy(path, floorPath + "\\" + name, true);
                    FloorTypeList.Add(new FloorTypeData() { Name = name });
                    NeedSave = true;
                }
            }            
        }

        internal bool RemoveFloorRes(FloorTypeData floorTypeData)
        {
            foreach (var item in FloorTypeList)
            {
                if (item == floorTypeData)
                {
                    try
                    {
                        File.Delete(item.Path);
                    }
                    catch (Exception e)
                    {

                        DialogManager.ShowMessageAsync(MapEditor.GetInstance(), "删除文件错误", e.Message);
                    }
                    FloorTypeList.Remove(item);
                    NeedSave = true;
                    return true;
                }
            }
            return false;
        }

        private FloorTypeData GetFloorRes(string name)
        {
            foreach (var item in FloorTypeList)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        public void AddBuildingRes(string[] resPathList)
        {
            string buildingPath = Path + "\\building";
            string name;
            foreach (string path in resPathList)
            {
                name = System.IO.Path.GetFileName(path);
                if (GetBuildingRes(name) == null)
                {
                    File.Copy(path, buildingPath + "\\" + name, true);
                    BuildingTypeList.Add(new BuildingTypeData() { Name = name });
                    NeedSave = true;
                }
            }
        }

        internal bool RemoveBuildingRes(BuildingTypeData buildingTypeData)
        {
            foreach (var item in BuildingTypeList)
            {
                if (item == buildingTypeData)
                {
                    try
                    {
                        File.Delete(item.Path);
                    }
                    catch (Exception e)
                    {

                        DialogManager.ShowMessageAsync(MapEditor.GetInstance(), "删除文件错误", e.Message);
                    }
                    
                    BuildingTypeList.Remove(item);
                    NeedSave = true;
                    return true;
                }
            }
            return false;
        }

        private BuildingTypeData GetBuildingRes(string name)
        {
            foreach (var item in BuildingTypeList)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        internal MapData AddMapData(string name, int rows, int cols)
        {
            if (GetMapData(name) == null)
            {
                MapData i = new MapData() { Name = name, CellRows = rows, CellCols = cols };
                MapDataList.Add(i);
                NeedSave = true;
                return i;
            }
            return null;
        }

        internal bool RemoveMapData(string name)
        {
            foreach (var item in MapDataList)
            {
                if (item.Name == name)
                {
                    MapDataList.Remove(item);
                    NeedSave = true;
                    return true;
                }
            }
            return false;
        }

        internal MapData GetMapData(string name)
        {
            foreach (var item in MapDataList)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
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

        internal static GameData GetInstance()
        {
            if (instance == null)
            {
                instance = new GameData();
            }
            return instance;
        }

        internal static void CreateWithJson(string mapJsonPath)
        {
            using (StreamReader sr = new StreamReader(mapJsonPath))
            {
                string jsonTxt = sr.ReadToEnd();
                instance = JsonConvert.DeserializeObject(jsonTxt, typeof(GameData)) as GameData;
            }
        }
    }

    abstract class PropTypeData
    {
        public string Name { get; set; }

        public string Des { get; set; }

        public int OffsetX { get; set; }

        public int OffsetY { get; set; }

        protected string underside = "";
        public virtual string Underside
        {
            get { return underside; }
            set
            {
                underside = value;
                var t = underside.Split(new char[] { '&' });
                rows = t.Length;
                cols = t[0].Split(new char[] { '|' }).Length;

                undersideAry = new int[10, 10];
                if (!string.IsNullOrEmpty(underside))
                {
                    for (int i = 0; i < rows; i++)
                    {
                        var tt = t[i].Split(new char[] { '|' });
                        for (int j = 0; j < tt.Length; j++)
                        {
                            undersideAry[i, j] = int.Parse(tt[j]);
                        }
                    }
                }
            }
        }

        protected int[,] undersideAry = new int[10, 10];
        [JsonIgnore]
        public int[,] UndersideAry { get { return undersideAry; } }

        protected int rows;

        protected int cols;

        [JsonIgnore]
        public virtual string Path
        {
            get { return null; }
        }

        internal virtual void ChangeUnderSide(int row, int col)
        {
            undersideAry[row, col] = undersideAry[row, col] == 0 ? 1 : 0;
            ResetUnderside();
        }

        internal virtual void ResetUnderside()
        {
            int maxRow = 0, maxCol = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (undersideAry[i, j] == 1)
                    {
                        maxRow = Math.Max(maxRow, i);
                        maxCol = Math.Max(maxCol, j);
                    }
                }
            }
            underside = "";
            for (int i = 0; i <= maxRow; i++)
            {
                for (int j = 0; j <= maxCol; j++)
                {
                    underside += j == maxCol ? undersideAry[i, j].ToString() : undersideAry[i, j].ToString() + "|";
                }
                if (i < maxRow)
                {
                    underside += "&";
                }
            }
            GameData.GetInstance().NeedSave = true;
        }

        public override string ToString()
        {
            return Name;
        }

    }

    class FloorTypeData : PropTypeData
    {
        public override string Underside
        {
            get { return "1"; }
            set { underside = "1"; }
        }

        protected new int[,] undersideAry = new int[1, 1] { { 1 } };
        [JsonIgnore]
        public new int[,] UndersideAry { get { return undersideAry; } }

        internal override void ChangeUnderSide(int row, int col)
        {
            //do nothing
        }

        internal override void ResetUnderside()
        {
            //do nothing
        }

        public override string Path
        {
            get { return string.Format("{0}\\floor\\{1}", GameData.GetInstance().Path, Name); }
        }
    }

    class BuildingTypeData : PropTypeData
    {
        public override string Path
        {
            get { return string.Format("{0}\\building\\{1}", GameData.GetInstance().Path, Name); }
        }
    }

    class MapData
    {
        public string Name { get; set; }

        /// <summary>
        /// 格子行数
        /// </summary>
        public int CellRows { get; set; }

        /// <summary>
        /// 格子列数
        /// </summary>
        public int CellCols { get; set; }

        public List<LayerData> LayerDataList { get; set; }

        public MapData()
        {
            LayerDataList = new List<LayerData>();
        }

        public bool AddLayerData(string name)
        {
            LayerData layerData = GetLayerData(name);
            if (layerData == null)
            {
                layerData = new LayerData() { Name = name };
                LayerDataList.Add(layerData);
                GameData.GetInstance().NeedSave = true;
                return true;
            }
            return false;
        }

        public bool RemoveLayerData(string name)
        {
            foreach (var item in LayerDataList)
            {
                if (item.Name == name)
                {
                    LayerDataList.Remove(item);
                    GameData.GetInstance().NeedSave = true;
                    return true;
                }
            }
            return false;
        }

        internal LayerData GetLayerData(string name)
        {
            foreach (var item in LayerDataList)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }
    }

    class LayerData
    {
        public string Name { get; set; }
    }
}
