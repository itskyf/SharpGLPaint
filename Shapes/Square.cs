using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Square : Shape {
    private readonly Line[] _lines;

    public Square(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        var length = Math.Min(Math.Abs(startPoint.X - endPoint.X), Math.Abs(startPoint.Y - endPoint.Y));
        var otherX = startPoint.X + (endPoint.X > startPoint.X ? length : -length);
        var otherY = startPoint.Y + (endPoint.Y > startPoint.Y ? length : -length);

        Point apex2 = new(startPoint.X, otherY), apex3 = new(otherX, otherY), apex4 = new(otherX, startPoint.Y);

        _lines = new Line[] {
            new(startPoint, apex2, color, pointSize),
            new(apex2, apex3, color, pointSize),
            new(apex3, apex4, color, pointSize),
            new(apex4, startPoint, color, pointSize)
        };
    }

    protected override List<Point> InitPoints() {
        List<Point> points = new();
        foreach (var line in _lines) {
            points.AddRange(line.Points);
        }

        return points;
    }
}
