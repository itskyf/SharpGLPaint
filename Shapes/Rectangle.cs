using System;
using System.Collections.Generic;
using System.Drawing;
using SharpGL;
using SharpGLPaint.Fill;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Rectangle : Shape {
    private readonly Point[] _vertices;

    public Rectangle(Color color, float pointSize, params object[] parameters) : base(color, pointSize) {
        Points = new List<Point>();
        Point startPoint = (Point)parameters[0], endPoint = (Point)parameters[1];

        // Find all vertices' coordinates
        int minX = Math.Min(startPoint.X, endPoint.X), minY = Math.Min(startPoint.Y, endPoint.Y);
        int maxX = Math.Max(startPoint.X, endPoint.X), maxY = Math.Max(startPoint.Y, endPoint.Y);
        TopLeft = new Point(minX, minY);
        BottomRight = new Point(maxX, maxY);
        _vertices = new Point[] { new(minX, minY), new(minX, maxY), new(maxX, maxY), new(maxX, minY) };

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
