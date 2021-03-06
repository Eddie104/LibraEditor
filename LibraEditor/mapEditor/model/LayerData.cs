﻿using LibraEditor.mapEditor.view.mapLayer;
using Newtonsoft.Json;
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

        public void AddRes(MapResView mapResView, int id)
        {
            var res = new LayerRes() { ID = id, Name = mapResView.Res.Name };
            ResList.Add(res);
            UpdateRes(mapResView, res);
        }

        public bool UpdateRes(MapResView mapResView, LayerRes res = null)
        {
            if (res == null)
            {
                res = GetRes(mapResView.ID);
            }
            if (res != null)
            {
                res.Row = mapResView.Row;
                res.Col = mapResView.Col;
                res.IsRotate = mapResView.IsRotate;
                return true;
            }
            return false;
        }

        private LayerRes GetRes(int id)
        {
            foreach (var item in ResList)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 图层中的资源实例
    /// </summary>
    public class LayerRes
    {
        public int ID { get; set; }

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

        /// <summary>
        /// 是否旋转了
        /// </summary>
        public bool IsRotate { get; set; }
    }
}
