using System;
using System.Windows.Media;

namespace SharpGLPaint.Shapes;

/// <summary>
///     Factory for converting shape mode to an instance
/// </summary>
public static class ShapeFactory {
    public static Shape Create(ShapeMode key, Color color, float pointSize, params object[] parameters) {
        return key switch {
            ShapeMode.Line => new Line(color, pointSize, parameters),
            ShapeMode.Circle => new Circle(color, pointSize, parameters),
            ShapeMode.Rectangle => new Rectangle(color, pointSize, parameters),
            ShapeMode.Ellipse => new Ellipse(color, pointSize, parameters),
            ShapeMode.Square => new Square(color, pointSize, parameters),
            ShapeMode.Pentagon => new RegularPolygon(5, color, pointSize, parameters),
            ShapeMode.Hexagon => new RegularPolygon(6, color, pointSize, parameters),
            ShapeMode.Polygon => new Polygon(color, pointSize, parameters),
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };
    }
}
