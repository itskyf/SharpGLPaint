using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SharpGLPaint.Fill;

public static class Filling {
    public static List<Point> FloodFill(List<Point> points, Point center, Point topLeft, Point bottomRight) {
        int height = bottomRight.Y - topLeft.Y + 1, width = bottomRight.X - topLeft.X + 1;

        // Create fill table
        var filled = new BitArray[height];
        for (var h = 0; h < height; ++h) {
            filled[h] = new BitArray(width, false);
        }
        // Mark boundary
        foreach (var point in points) {
            filled[point.Y - topLeft.Y][point.X - topLeft.X] = true;
        }
        // Fill
        Queue<Point> queue = new();
        int y = center.Y - topLeft.Y, x = center.X - topLeft.X;
        filled[y][x] = true;
        List<Point> fillPoints = new();
        queue.Enqueue(new Point(x, y));

        while (queue.Any()) {
            var point = queue.Dequeue();
            fillPoints.Add(new Point(point.X + topLeft.X, point.Y + topLeft.Y));
            y = point.Y - 1; // Left
            if (!filled[y][point.X]) {
                filled[y][point.X] = true;
                queue.Enqueue(new Point(point.X, y));
            }
            y = point.Y + 1; // Right
            if (!filled[y][point.X]) {
                filled[y][point.X] = true;
                queue.Enqueue(new Point(point.X, y));
            }
            x = point.X - 1; //Top
            if (!filled[point.Y][x]) {
                filled[point.Y][x] = true;
                queue.Enqueue(new Point(x, point.Y));
            }
            x = point.X + 1; //Bottom
            if (!filled[point.Y][x]) {
                filled[point.Y][x] = true;
                queue.Enqueue(new Point(x, point.Y));
            }
        }
        return fillPoints;
    }

    public static List<Point> Scanline(Point[] vertices) {
        Dictionary<int, List<ActiveEdge>> edgeTable = new();

        return new();
    }
}
