using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraEditor.mapEditor.model
{
    /// <summary>
    /// 资源类型
    /// </summary>
    enum ResType
    {
        png,
        jpg
    }

    /// <summary>
    /// 地图资源
    /// </summary>
    class MapRes
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        public ResType ResType{ get; set; }

        /// <summary>
        /// 资源名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 图片左上角到格子中心点的横坐标偏移值
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        /// 图片左上角到格子中心点的纵坐标偏移值
        /// </summary>
        public int OffsetY { get; set; }

        public MapRes(string path)
        {
            Path = path;
            //文件名  “Default.aspx”
            Name = System.IO.Path.GetFileName(Path);
            //扩展名 “.aspx”
            string extension = System.IO.Path.GetExtension(Path);
            //没有扩展名的文件名 “Default”
            //string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(Path);
            switch (extension)
            {
                case ".png":
                    ResType = ResType.png;
                    break;
                case ".jpg":
                    ResType = ResType.jpg;
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
