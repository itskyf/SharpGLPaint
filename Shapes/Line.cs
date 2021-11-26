using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Line : Shape {
    // private readonly Point _startPoint, _endPoint;

    public Line(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        // _startPoint = startPoint;
        // _endPoint = endPoint;

        int x = startPoint.X, y = startPoint.Y;
        int sx = x < endPoint.X ? 1 : -1, sy = y < endPoint.Y ? 1 : -1;
        int dx = Math.Abs(endPoint.X - x), dy = -Math.Abs(endPoint.Y - y), err = dx + dy;

        Points = new List<Point> { new(x, y) };
        while (x != endPoint.X || y != endPoint.Y) {
            var twiceErr = 2 * err;
            if (twiceErr >= dy) {
                err += dy;
                x += sx;
            }

            if (twiceErr <= dx) {
                err += dx;
                y += sy;
            }

            Points.Add(new Point(x, y));
        }

        Points.Add(new Point(endPoint.X, endPoint.Y));
    }
}
