using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes {
public class Line : Shape {
    private readonly Point _startPoint, _endPoint;

    public Line(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        _startPoint = startPoint;
        _endPoint = endPoint;
    }

    protected override List<Point> InitPoints() {
        int x0 = _startPoint.X, y0 = _startPoint.Y, x1 = _endPoint.X, y1 = _endPoint.Y;
        int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
        int dx = Math.Abs(x1 - x0), dy = -Math.Abs(y1 - y0), err = dx + dy;

        var points = new List<Point> {new(x0, y0)};
        while (x0 != x1 || y0 != y1) {
            var err2 = err * 2;
            if (err2 >= dy) {
                err += dy;
                x0 += sx;
            }

            if (err2 <= dx) {
                err += dx;
                y0 += sy;
            }

            points.Add(new Point(x0, y0));
        }

        points.Add(new Point(x1, y1));
        return points;
    }
}
}
