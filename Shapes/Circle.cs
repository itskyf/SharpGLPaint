using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Circle : Shape {
    // private readonly Point _center;
    // private readonly int _radius;

    public Circle(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        // Crucial parameter
        var radius = Math.Min(Math.Abs(startPoint.X - endPoint.X), Math.Abs(startPoint.Y - endPoint.Y)) / 2;
        var centerX = startPoint.X + (endPoint.X > startPoint.X ? radius : -radius);
        var centerY = startPoint.Y + (endPoint.Y > startPoint.Y ? radius : -radius);
        var center = new Point(centerX, centerY);

        // If start point is also end point => a dot
        if (radius == 0) {
            Points = new List<Point> { center };
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
            point.X += center.X;
            point.Y += center.Y;
            return point;
        });
    }
}
