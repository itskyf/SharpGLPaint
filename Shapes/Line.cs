using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Line : Shape {
    private readonly Point _startPoint, _endPoint;

    public Line(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        _startPoint = startPoint;
        _endPoint = endPoint;
    }

    protected override List<Point> InitPoints() {
        int x = _startPoint.X, y = _startPoint.Y;
        int sx = x < _endPoint.X ? 1 : -1, sy = y < _endPoint.Y ? 1 : -1;
        int dx = Math.Abs(_endPoint.X - x), dy = -Math.Abs(_endPoint.Y - y), err = dx + dy;

        var points = new List<Point> { new(x, y) };
        while (x != _endPoint.X || y != _endPoint.Y) {
            var twiceErr = 2 * err;
            if (twiceErr >= dy) {
                err += dy;
                x += sx;
            }

            if (twiceErr <= dx) {
                err += dx;
                y += sy;
            }

            points.Add(new Point(x, y));
        }

        points.Add(new Point(_endPoint.X, _endPoint.Y));
        return points;
    }
}
