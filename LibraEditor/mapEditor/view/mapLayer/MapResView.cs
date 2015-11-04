using Libra.helper;
using LibraEditor.mapEditor.model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LibraEditor.mapEditor.view.mapLayer
{
    public class MapResView : Image
    {

        public int ID { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        public bool IsRotate { get; set; }

        public MapRes Res { get; set; }

        public bool IsCanDrag { get; set; }

        private Point oldPoint = new Point();

        private bool isMoving = false;

        public MapResView(MapRes res, bool isCanDrag = false)
        {
            Res = res;
            IsCanDrag = isCanDrag;
            BitmapImage bi = new BitmapImage(new Uri(res.Path, UriKind.Absolute));
            Source = bi;
            Width = bi.PixelWidth;
            Height = bi.PixelHeight;

            Row = -1;Col = -1;

            this.MouseMove += OnMouseMove;
            this.MouseLeftButtonDown += OnMouseDown;
            this.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsCanDrag)
            {
                isMoving = true;
                oldPoint = e.GetPosition(null);
            }
        }

        private void OnMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsCanDrag)
            {
                isMoving = false;
            }
        }

        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsCanDrag)
            {
                if (isMoving)
                {
                    double xPos = e.GetPosition(null).X - oldPoint.X + (double)this.GetValue(Canvas.LeftProperty);
                    double yPos = e.GetPosition(null).Y - oldPoint.Y + (double)this.GetValue(Canvas.TopProperty);
                    this.SetValue(Canvas.LeftProperty, xPos);
                    this.SetValue(Canvas.TopProperty, yPos);
                    oldPoint = e.GetPosition(null);
                }
            }            
        }

        public void SetRowAndCol(int row, int col, bool force = false)
        {
            if (Row != row || Col != col || force)
            {
                Row = row;
                Col = col;

                Point p;
                if (MapData.GetInstance().ViewType == ViewType.iso)
                {
                    p = ISOHelper.GetItemPos(row, col);

                }
                else
                {
                    p = RectangularHelper.GetItemPos(row, col);
                }
                Canvas.SetLeft(this, p.X - Res.OffsetX);
                Canvas.SetTop(this, p.Y - Res.OffsetY);
            }
        }
    }
}
