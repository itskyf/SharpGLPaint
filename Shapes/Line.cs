using System;
using System.Collections.Generic;
using System.Drawing;
using SharpGL;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

public class Line : Shape {
    private readonly Point _start, _end;

    public Line(Color color, float pointSize, params object[] parameters) : base(color, pointSize) {
        _start = (Point)parameters[0];
        _end = (Point)parameters[1];

        int x = _start.X, y = _start.Y;
        // Decide x and y increment at the next step
        int sx = x < _end.X ? 1 : -1, sy = y < _end.Y ? 1 : -1;
        int dx = Math.Abs(_end.X - x), dy = -Math.Abs(_end.Y - y);
        var err = dx + dy;

        Points = new List<Point> { new(x, y) };
        while (x != _end.X || y != _end.Y) {
            var twiceErr = 2 * err;
            if (twiceErr >= dy) {
                err += dy;
                x += sx;
            }

            if (twiceErr <= dx) {
                err += dx;
                y += sy;
            }

            Points.Add(new Point(x, y));
        }

        Points.Add(new Point(_end.X, _end.Y));
    }

    protected override List<Point> GetFillPoints(OpenGL gl) {
        return Points;
    }

    public override void Highlight(OpenGL gl) {
        gl.PointSize(7f);
        gl.Color(0, 0, 0);
        gl.Begin(OpenGL.GL_POINTS);
        gl.Vertex(_start.X, _start.Y);
        gl.Vertex(_end.X, _end.Y);
        gl.End();
    }
}
