using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class RegularPolygon : Shape {
    public RegularPolygon(int numVertices, Point startPoint, Point endPoint, Color color, float pointSize)
        : base(color, pointSize) {
        if (numVertices is <= 2 or > 60) {
            throw new ArgumentOutOfRangeException(
                nameof(numVertices), numVertices, "Number of vertices must be in range [2, 60]"
            );
        }

        // Crucial parameters
        var vertices = new Point[numVertices - 1];
        var radianBetweenVertex = 2 * MathF.PI / numVertices;
        int x = endPoint.X - startPoint.X, y = endPoint.Y - startPoint.Y;

        // Rotate vector
        for (var i = 0; i < vertices.Length; ++i) {
            var nextRadian = (i + 1) * radianBetweenVertex;
            var cos = MathF.Cos(nextRadian);
            var sin = MathF.Sin(nextRadian);
            vertices[i].X = (int)(x * cos - y * sin) + startPoint.X;
            vertices[i].Y = (int)(x * sin + y * cos) + startPoint.Y;
        }

        // Join line segments
        var sides = new List<Line> { new(endPoint, vertices[0], color, pointSize) };
        for (var i = 0; i < vertices.Length - 1; ++i) {
            sides.Add(new Line(vertices[i], vertices[i + 1], color, pointSize));
        }

        sides.Add(new Line(vertices[^1], endPoint, color, pointSize));

        Points = new List<Point>();
        foreach (var line in sides) {
            Points.AddRange(line.ReadOnlyPoints);
        }
    }
}
