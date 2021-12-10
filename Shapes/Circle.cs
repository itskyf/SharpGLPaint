using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpGL;
using SharpGLPaint.Fill;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Circle : Shape {
    private readonly Point _center;

    public Circle(Color color, float pointSize, params object[] parameters) : base(color, pointSize) {
        var start = (Point)parameters[0];
        var end = (Point)parameters[1];

        // Crucial parameter
        var radius = Math.Min(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y)) / 2;
        var centerX = start.X + (end.X > start.X ? radius : -radius);
        var centerY = start.Y + (end.Y > start.Y ? radius : -radius);
        _center = new Point(centerX, centerY);
        TopLeft = new Point(centerX - radius, centerY - radius);
        BottomRight = new Point(centerX + radius, centerY + radius);

        // If start point == end point => a dot
        if (start == end) {
            Points = new List<Point> { _center };
            return;
        }

        int x = 0, y = radius;
        var points = new List<Point> { new(x, y) };

        var p = 1 - radius;
        while (x < y) {
            ++x;
            if (p < 0) {
                p += 2 * x + 1;
            } else {
                --y;
                p += 2 * (x - y) + 1;
            }

            points.Add(new Point(x, y));
        }

        // Reflect over y = x
        var reflectPoints = points.Take(points.Count - 1).Select(point => new Point(point.Y, point.X)).ToList();
        reflectPoints.Reverse();
        points.AddRange(reflectPoints);

        // Reflect over y = 0
        reflectPoints = points.Take(points.Count - 1).Select(point => new Point(point.X, -point.Y)).ToList();
        reflectPoints.Reverse();
        points.AddRange(reflectPoints);

        // Reflect over x = 0
        reflectPoints = points.Skip(1).Take(points.Count - 1).Select(point => new Point(-point.X, point.Y)).ToList();
        reflectPoints.Reverse();
        points.AddRange(reflectPoints);

        // Move to center
        Points = points.ConvertAll(point => {
            point.X += _center.X;
            point.Y += _center.Y;
            return point;
        });
    }

    protected override List<Point> GetFillPoints(OpenGL gl) {
        return Filling.FloodFill(Points, _center, TopLeft, BottomRight);
    }
}
