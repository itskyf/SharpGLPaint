using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Rectangle : Shape {
    // private readonly Line[] _sides;

    public Rectangle(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        int minX = Math.Min(startPoint.X, endPoint.X), minY = Math.Min(startPoint.Y, endPoint.Y);
        int maxX = Math.Max(startPoint.X, endPoint.X), maxY = Math.Max(startPoint.Y, endPoint.Y);
        Point topLeft = new(minX, minY),
            bottomLeft = new(minX, maxY),
            bottomRight = new(maxX, maxY),
            topRight = new(maxX, minY);

        var sides = new Line[] {
            new(topLeft, bottomLeft, color, pointSize),
            new(bottomLeft, bottomRight, color, pointSize),
            new(bottomRight, topRight, color, pointSize),
            new(topRight, topLeft, color, pointSize)
        };

        Points = new List<Point>();
        foreach (var line in sides) {
            Points.AddRange(line.ReadOnlyPoints);
        }
    }
}
