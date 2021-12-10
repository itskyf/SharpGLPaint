using System;
using System.Collections.Generic;
using System.Drawing;
using SharpGL;
using SharpGLPaint.Fill;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class RegularPolygon : Shape {
    private readonly Point[] _vertices;

    public RegularPolygon(
        int numVertices, Color color, float pointSize, params object[] parameters
    ) : base(color, pointSize) {
        if (numVertices is <= 2 or > 60) {
            throw new ArgumentOutOfRangeException(
                nameof(numVertices), numVertices, "Number of vertices must be in range [2, 60]"
            );
        }
        Points = new List<Point>();
        Point center = (Point)parameters[0], end = (Point)parameters[1];

        // Crucial parameters
        _vertices = new Point[numVertices];
        _vertices[0] = end;
        var radianBetweenVertex = 2 * MathF.PI / numVertices;
        int x = end.X - center.X, y = end.Y - center.Y;

        // Rotate vector
        for (var i = 1; i < _vertices.Length; ++i) {
            var nextRadian = i * radianBetweenVertex;
            var cos = MathF.Cos(nextRadian);
            var sin = MathF.Sin(nextRadian);
            _vertices[i].X = (int)(x * cos - y * sin) + center.X;
            _vertices[i].Y = (int)(x * sin + y * cos) + center.Y;
        }

        int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
        foreach (var vertex in _vertices) {
            minX = Math.Min(minX, vertex.X);
            minY = Math.Min(minY, vertex.Y);
            maxX = Math.Max(maxX, vertex.X);
            maxY = Math.Max(maxY, vertex.Y);
        }
        TopLeft = new Point(minX, minY);
        BottomRight = new Point(maxX, maxY);

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
