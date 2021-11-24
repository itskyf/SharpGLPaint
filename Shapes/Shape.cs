using System.Collections.Generic;
using System.Drawing;
using SharpGL;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public abstract class Shape {
    private readonly float _pointSize;
    private readonly float _r, _g, _b;

    private List<Point>? _points;

    protected Shape(Color color, float pointSize) {
        _r = color.R / 255f;
        _g = color.G / 255f;
        _b = color.B / 255f;
        _pointSize = pointSize;
    }

    public List<Point> Points {
        get {
            _points ??= InitPoints();
            return _points;
        }
    }

    protected abstract List<Point> InitPoints();

    public void Draw(OpenGL gl) {
        gl.PointSize(_pointSize);
        gl.Color(_r, _g, _b);
        gl.Begin(OpenGL.GL_POINTS);

        foreach (var point in Points) {
            gl.Vertex(point.X, point.Y);
        }

        gl.End();
    }
}
