using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpGL;
using SharpGLPaint.Fill;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Ellipse : Shape {
    private readonly Point _center;

    public Ellipse(Color color, float pointSize, params object[] parameters) : base(color, pointSize) {
        var start = (Point)parameters[0];
        var end = (Point)parameters[1];

        int minX = Math.Min(start.X, end.X), minY = Math.Min(start.Y, end.Y);
        int rx = Math.Abs(start.X - end.X) / 2, ry = Math.Abs(start.Y - end.Y) / 2;
        _center = new Point(minX + rx, minY + ry);
        TopLeft = new Point(minX, minY);
        BottomRight = new Point(minX + 2 * rx, minY + 2 * ry);

        int x = 0, y = ry;
        var points = new List<Point> { new(x, y) };

        int ry2 = ry * ry, rx2 = rx * rx, rx2Twice = rx2 * 2, ry2Twice = ry2 * 2;

        // Region 1
        var p = ry2 - rx2 * ry + 0.25f * rx2;
        int dx = 0, dy = rx2Twice * y;
        while (dx < dy) {
            ++x;
            dx += ry2Twice;
            if (p < 0) {
                p += dx + ry2;
            } else {
                --y;
                dy -= rx2Twice;
                p += dx - dy + ry2;
            }

            points.Add(new Point(x, y));
        }

        // Region 2
        p = ry2 * MathF.Pow(x + 0.5f, 2f) + rx2 * MathF.Pow(y - 1f, 2f) - (long)rx2 * ry2;
        while (y > 0) {
            --y;
            dy -= rx2Twice;
            if (p >= 0) {
                p += -dy + rx2;
            } else {
                ++x;
                dx += ry2Twice;
                p += dx - dy + rx2;
            }

            points.Add(new Point(x, y));
        }

        // Reflect over y = 0
        var reflectPoints = points.Take(points.Count - 1).Select(point => new Point(point.X, -point.Y)).ToList();
        reflectPoints.Reverse();
        points.AddRange(reflectPoints);

        // Reflect over x = 0
        reflectPoints = points.Skip(1).Take(points.Count - 1).Select(point => new Point(-point.X, point.Y)).ToList();
        reflectPoints.Reverse();
        points.AddRange(reflectPoints);

        // Move to the true center
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
