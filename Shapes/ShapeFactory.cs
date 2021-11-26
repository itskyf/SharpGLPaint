using System;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

/// <summary>
///     Factory for converting shape mode to an instance
/// </summary>
public static class ShapeFactory {
    public static Shape Create(ShapeMode key, Point startPoint, Point endPoint, Color color, float pointSize) {
        Shape shape = key switch {
            ShapeMode.Line => new Line(startPoint, endPoint, color, pointSize),
            ShapeMode.Circle => new Circle(startPoint, endPoint, color, pointSize),
            ShapeMode.Rectangle => new Rectangle(startPoint, endPoint, color, pointSize),
            ShapeMode.Ellipse => new Ellipse(startPoint, endPoint, color, pointSize),
            ShapeMode.Square => new Square(startPoint, endPoint, color, pointSize),
            ShapeMode.Pentagon => new RegularPolygon(5, startPoint, endPoint, color, pointSize),
            ShapeMode.Hexagon => new RegularPolygon(6, startPoint, endPoint, color, pointSize),
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };

        return shape;
    }
}
