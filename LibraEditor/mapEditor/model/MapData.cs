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
        /// 保存地图的配置文件
        /// </summary>
        public void Save()
        {

        }

        public static MapData GetInstance()
        {
            if (instance == null)
            {
                instance = new MapData();
            }
            return instance;
        }
    }
}
