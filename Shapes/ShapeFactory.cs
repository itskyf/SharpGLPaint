using System;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes {
public static class ShapeFactory {
    public static Shape Create(ShapeMode key, Point startPoint, Point endPoint, Color color, float pointSize) {
        Shape shape = key switch {
            ShapeMode.Line => new Line(startPoint, endPoint, color, pointSize),
            // ShapeMode.Circle => expr,
            ShapeMode.Rectangle => new Rectangle(startPoint, endPoint, color, pointSize),
            // ShapeMode.Ellipse => expr,
            ShapeMode.Square => new Square(startPoint, endPoint, color, pointSize),
            // ShapeMode.Pentagon => expr,
            // ShapeMode.Hexagon => expr,
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };

        return shape;
    }
}
}
