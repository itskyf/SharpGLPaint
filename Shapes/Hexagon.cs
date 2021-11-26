﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Hexagon : Shape {
    // private readonly Line[] _sides;

    public Hexagon(Point startPoint, Point endPoint, Color color, float pointSize) : base(color, pointSize) {
        float cos60 = MathF.Cos(MathF.PI / 3f), sin60 = MathF.Sin(MathF.PI / 3f);
        var vertices = new Point[] {
            new(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y), new(), new(), new(), new(), new()
        };

        for (var i = 1; i < 6; ++i) {
            vertices[i].X = (int)(vertices[i - 1].X * cos60 - vertices[i - 1].Y * sin60);
            vertices[i].Y = (int)(vertices[i - 1].X * sin60 + vertices[i - 1].Y * cos60);
            vertices[i - 1].X += startPoint.X;
            vertices[i - 1].Y += startPoint.Y;
        }

        vertices[5].X += startPoint.X;
        vertices[5].Y += startPoint.Y;

        var sides = new Line[] {
            new(vertices[0], vertices[1], color, pointSize),
            new(vertices[1], vertices[2], color, pointSize),
            new(vertices[2], vertices[3], color, pointSize),
            new(vertices[3], vertices[4], color, pointSize),
            new(vertices[4], vertices[5], color, pointSize),
            new(vertices[5], vertices[0], color, pointSize)
        };

        Points = new List<Point>();
        foreach (var line in sides) {
            Points.AddRange(line.ReadOnlyPoints);
        }
    }
}
