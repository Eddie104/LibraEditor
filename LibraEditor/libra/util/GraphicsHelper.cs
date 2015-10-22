using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LibraEditor.libra.util
{
    struct LinePoint
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
    }

    class GraphicsHelper
    {

        public static void Draw(Canvas canvas, List<LinePoint> points, Brush stroke)
        {
            canvas.Children.Clear();

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            PathGeometry myPathGeometry = new PathGeometry();

            foreach (LinePoint p in points)
            {
                PathFigure myPathFigure = new PathFigure();
                myPathFigure.StartPoint = p.StartPoint;

                LineSegment myLineSegment = new LineSegment();
                myLineSegment.Point = p.EndPoint;

                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                myPathSegmentCollection.Add(myLineSegment);

                myPathFigure.Segments = myPathSegmentCollection;

                myPathFigureCollection.Add(myPathFigure);
            }

            myPathGeometry.Figures = myPathFigureCollection;
            Path myPath = new Path();
            myPath.Stroke = stroke == null ? Brushes.Black : stroke;
            myPath.StrokeThickness = 1;
            myPath.Data = myPathGeometry;

            canvas.Children.Add(myPath);
        }

    }
}
