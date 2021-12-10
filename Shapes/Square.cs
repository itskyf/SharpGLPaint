using System;
using System.Collections.Generic;
using System.Drawing;
using SharpGL;
using SharpGLPaint.Fill;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Square : Shape {
    private readonly Point[] _vertices;

    public Square(Color color, float pointSize, params object[] parameters) : base(color, pointSize) {
        Points = new List<Point>();
        Point start = (Point)parameters[0], end = (Point)parameters[1];

        // Find side's length and increment from the start point
        var length = Math.Min(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
        var otherX = start.X + (end.X > start.X ? length : -length);
        var otherY = start.Y + (end.Y > start.Y ? length : -length);
        int minX = Math.Min(start.X, otherX), minY = Math.Min(start.Y, otherY);

        TopLeft = new Point(minX, minY);
        BottomRight = new Point(minX + length, minY + length);
        _vertices = new[] {
            start, new(start.X, otherY), new(otherX, otherY), new(otherX, start.Y)
        };

        // Join edges
        for (var i = 0; i < _vertices.Length - 1; ++i) {
            var edge = new Line(color, pointSize, _vertices[i], _vertices[i + 1]);
            Points.AddRange(edge.ReadOnlyPoints);
        }
        var lastEdge = new Line(color, pointSize, _vertices[^1], _vertices[0]);
        Points.AddRange(lastEdge.ReadOnlyPoints);
    }

    protected override List<Point> GetFillPoints(OpenGL gl) {
        return Filling.Scanline(_vertices);
    }
}
