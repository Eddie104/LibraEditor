using Libra.helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LibraEditor.mapEditor2.model.data
{
    abstract class Prop : Image
    {

        public int ID { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        private string path;
        [JsonIgnore]
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                BitmapImage bi = new BitmapImage(new Uri(path, UriKind.Absolute));
                this.Source = bi;
                this.Width = bi.PixelWidth;
                this.Height = bi.PixelHeight;
            }
        }

        private PropTypeData data;
        [JsonIgnore]        
        public virtual PropTypeData Data
        {
            get { return data; }
            protected set
            {
                data = value;
                Path = data.Path;
            }
        }

        public Prop(PropTypeData data)
        {
            Data = data;
        }

        public PropTypeData GetData()
        {
            return data;
        }

        public void SetRowAndCol(int row, int col, ICoordinateHelper coordinateHelper)
        {
            Row = row;
            Col = col;
            Point p = coordinateHelper.GetItemPos(row, col);
            Canvas.SetLeft(this, p.X - data.OffsetX);
            Canvas.SetTop(this, p.Y - data.OffsetY);
        }
    }

    class Floor : Prop
    {

        private FloorTypeData data;   
        public override PropTypeData Data
        {
            get
            {
                return base.Data;
            }

            protected set
            {
                base.Data = value;
                data = value as FloorTypeData;
            }
        }

        public Floor(FloorTypeData data) : base(data) { }

        public new FloorTypeData GetData()
        {
            return data;
        }
    }

    class Building : Prop
    {
        private BuildingTypeData data;
        public override PropTypeData Data
        {
            get
            {
                return base.Data;
            }

            protected set
            {
                base.Data = value;
                data = value as BuildingTypeData;
            }
        }

        public Building(BuildingTypeData data) : base(data) { }

        public new BuildingTypeData GetData()
        {
            return data;
        }
    }
}
