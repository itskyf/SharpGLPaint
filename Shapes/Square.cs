using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Square : Shape {
    // private readonly Line[] _sides;

    public Square(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        // Find side's length and increment from the start point
        var length = Math.Min(Math.Abs(startPoint.X - endPoint.X), Math.Abs(startPoint.Y - endPoint.Y));
        var otherX = startPoint.X + (endPoint.X > startPoint.X ? length : -length);
        var otherY = startPoint.Y + (endPoint.Y > startPoint.Y ? length : -length);

        Point vertex2 = new(startPoint.X, otherY), vertex3 = new(otherX, otherY), vertex4 = new(otherX, startPoint.Y);

        // Join line segments
        var sides = new Line[] {
            new(startPoint, vertex2, color, pointSize),
            new(vertex2, vertex3, color, pointSize),
            new(vertex3, vertex4, color, pointSize),
            new(vertex4, startPoint, color, pointSize)
        };
        Points = new List<Point>();
        foreach (var line in sides) {
            Points.AddRange(line.ReadOnlyPoints);
        }
    }
}
