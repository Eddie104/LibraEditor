using System.Collections.Generic;

namespace LibraEditor.mapEditor.model
{
    /// <summary>
    /// 图层数据
    /// </summary>
    public class LayerData
    {

        public string Name { get; set; }

        public List<LayerRes> ResList{ get; set; }

        public LayerData()
        {
            ResList = new List<LayerRes>();
        }
    }

    /// <summary>
    /// 图层中的资源实例
    /// </summary>
    public class LayerRes
    {

        /// <summary>
        /// 资源实例名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所在格子的行索引
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 所在格子的列索引
        /// </summary>
        public int Col { get; set; }
    }
}
