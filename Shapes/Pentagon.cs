using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Pentagon : Shape {
    private readonly Line[] _sides;

    public Pentagon(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        float cos72 = MathF.Cos(0.4f * MathF.PI), sin72 = MathF.Sin(0.4f * MathF.PI);
        var vertices = new Point[] {
            new(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y), new(), new(), new(), new()
        };

        for (var i = 1; i < 5; ++i) {
            vertices[i].X = (int)(vertices[i - 1].X * cos72 - vertices[i - 1].Y * sin72);
            vertices[i].Y = (int)(vertices[i - 1].X * sin72 + vertices[i - 1].Y * cos72);
        }

        for (var i = 0; i < 5; ++i) {
            vertices[i].X += startPoint.X;
            vertices[i].Y += startPoint.Y;
        }

        _sides = new Line[] {
            new(vertices[0], vertices[1], color, pointSize),
            new(vertices[1], vertices[2], color, pointSize),
            new(vertices[2], vertices[3], color, pointSize),
            new(vertices[3], vertices[4], color, pointSize),
            new(vertices[4], vertices[0], color, pointSize)
        };
    }

    protected override List<Point> InitPoints() {
        List<Point> points = new();
        foreach (var line in _sides) {
            points.AddRange(line.Points);
        }

        return points;
    }
}
