using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpGL;
using SharpGLPaint.Fill;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Polygon : Shape {
    private readonly Point[] _vertices;

    public Polygon(Color color, float pointSize, params object[] parameters) : base(color, pointSize) {
        Points = new List<Point>();
        _vertices = ((List<Point>)parameters[0]).ToArray();
        Point? end = parameters.Length > 1 ? (Point)parameters[1] : null;

        for (var i = 0; i < _vertices.Length - 1; ++i) {
            var edge = new Line(color, pointSize, _vertices[i], _vertices[i + 1]);
            Points.AddRange(edge.ReadOnlyPoints);
        }

        int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
        foreach (var vertex in _vertices) {
            minX = Math.Min(minX, vertex.X);
            minY = Math.Min(minY, vertex.Y);
            maxX = Math.Max(maxX, vertex.X);
            maxY = Math.Max(maxY, vertex.Y);
        }

        if (end == null) {
            //Completed
            var edge = new Line(color, pointSize, _vertices[^1], _vertices[0]);
            Points.AddRange(edge.ReadOnlyPoints);
        } else {
            minX = Math.Min(minX, end.Value.X);
            minY = Math.Min(minY, end.Value.Y);
            maxX = Math.Max(maxX, end.Value.X);
            maxY = Math.Max(maxY, end.Value.Y);

            if (_vertices.Length > 1) {
                var edge = new Line(color, pointSize, _vertices[^1], end.Value);
                Points.AddRange(edge.ReadOnlyPoints);
            }

            var lastEdge = new Line(color, pointSize, end.Value, _vertices[0]);
            var pointsChunk = lastEdge.ReadOnlyPoints.Chunk(5).ToList();
            for (var i = 0; i < pointsChunk.Count; i += 2) {
                Points.AddRange(pointsChunk[i]);
            }
        }

        TopLeft = new Point(minX, minY);
        BottomRight = new Point(maxX, maxY);
    }

    protected override List<Point> GetFillPoints(OpenGL gl) {
        return Filling.Scanline(_vertices);
    }
}
