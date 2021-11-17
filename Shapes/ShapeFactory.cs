using System;
using System.Collections.Generic;
using System.Drawing;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public static class ShapeFactory {
    private static readonly Dictionary<ShapeMode, Type> RegisteredShapes = new();

    public static void Register<T>(ShapeMode key) {
        if (RegisteredShapes.ContainsKey(key)) {
            throw new ArgumentException($"Key {key} has existed");
        }

        var type = typeof(T);
        if (type.IsAbstract || type.IsInterface) {
            throw new ArgumentException("Cannot create instance of interface or abstract class");
        }

        RegisteredShapes.Add(key, type);
    }

    public static Shape Create(ShapeMode key, Point startPoint, Point endPoint, Color color, float pointSize) {
        var createdShape =
            Activator.CreateInstance(RegisteredShapes[key], startPoint, endPoint, color, pointSize) ??
            throw new ArgumentException($"Cannot create {key}");
        return (Shape) createdShape;
    }
}