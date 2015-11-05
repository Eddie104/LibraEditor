using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    class MapData
    {

        private static MapData instance;

        public MapViewType ViewType { get; set; }

        /// <summary>
        /// 格子宽度
        /// </summary>
        public double CellWidth { get; set; }

        /// <summary>
        /// 格子高度
        /// </summary>
        public double CellHeight { get; set; }

        /// <summary>
        /// 格子行数
        /// </summary>
        public int CellRows { get; set; }

        /// <summary>
        /// 格子列数
        /// </summary>
        public int CellCols { get; set; }

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

        public List<LayerData> LayerDataList { get; set; }

        [JsonIgnore]
        public bool NeedSave { get; set; }

        public MapData()
        {
            FloorTypeList = new List<FloorTypeData>();
            BuildingTypeList = new List<BuildingTypeData>();
            LayerDataList = new List<LayerData>();

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
                    File.Delete(item.Path);
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
                    File.Delete(item.Path);
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

        internal LayerData AddLayerData(string name)
        {
            if (GetLayerData(name) == null)
            {
                LayerData i = new LayerData() { Name = name };
                LayerDataList.Add(i);
                NeedSave = true;
                return i;
            }
            return null;
        }

        internal bool RemoveLayerData(string name)
        {
            foreach (var item in LayerDataList)
            {
                if (item.Name == name)
                {
                    LayerDataList.Remove(item);
                    NeedSave = true;
                    return true;
                }
            }
            return false;
        }

        private LayerData GetLayerData(string name)
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

        internal static MapData GetInstance()
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

    class PropTypeData
    {
        public string Name { get; set; }

        public string Des { get; set; }

        [JsonIgnore]
        public virtual string Path
        {
            get { return null; }
        }

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
            }
        }

        protected int rows;

        protected int cols;

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
        }

        public override string Path
        {
            get { return string.Format("{0}\\floor\\{1}", MapData.GetInstance().Path, Name); }
        }
    }

    class BuildingTypeData : PropTypeData
    {
        public override string Path
        {
            get { return string.Format("{0}\\building\\{1}", MapData.GetInstance().Path, Name); }
        }
    }

    class LayerData
    {
        public string Name { get; set; }
    }
}
