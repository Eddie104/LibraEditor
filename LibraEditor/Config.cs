using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraEditor
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
        //2.5D
    }

    class Config
    {
        /// <summary>
        /// 项目所使用的游戏引擎
        /// </summary>
        public static ProjectType ProjectType { get; set; }

        /// <summary>
        /// 视角类型
        /// </summary>
        public static ViewType ViewType { get; set; }

        /// <summary>
        /// 格子宽度
        /// </summary>
        public static int CellWidth { get; set; }

        /// <summary>
        /// 格子高度
        /// </summary>
        public static int CellHeight { get; set; }

        /// <summary>
        /// 格子行数
        /// </summary>
        public static int CellRows { get; set; }

        /// <summary>
        /// 格子列数
        /// </summary>
        public static int CellCols { get; set; }
    }
}
