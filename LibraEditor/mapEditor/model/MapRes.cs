using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LibraEditor.mapEditor.model
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        png,
        jpg
    }

    /// <summary>
    /// 地图资源
    /// </summary>
    public class MapRes
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

        /// <summary>
        /// 占地数据
        /// </summary>
        private string underside;
        public string Underside
        {
            get { return underside; }
            set
            {
                underside = value;

                undersideAry = new int[10, 10];
                if (!string.IsNullOrEmpty(underside))
                {
                    var t = underside.Split(new char[] { '&' });
                    for (int i = 0; i < t.Length; i++)
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

        private int[,] undersideAry = new int[10, 10];
        [JsonIgnore]
        public int[,] UndersideAry { get { return undersideAry; } }

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

        public void ChangeCover(int row, int col)
        {
            undersideAry[row, col] = undersideAry[row, col] == 0 ? 1 : 0;
            ResetUnderside();
        }

        public override string ToString()
        {
            return Name;
        }

        internal void ResetUnderside()
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
            MapData.GetInstance().NeedSave = true;
        }
    }
}
