using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpGL;
using SharpGLPaint.Fill;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint.Shapes;

/// <summary>
///     Base class for a save; contains color, thickness and points list
/// </summary>
public abstract class Shape {
    private readonly GlColor _color;
    private readonly float _pointSize;
    private List<Point>? _fillPoints;
    protected List<Point> Points = null!;

    // Center != null => could perform flood fill
    protected Point TopLeft, BottomRight;

    protected Shape(Color color, float pointSize) {
        _color = new GlColor(color);
        _pointSize = pointSize;
    }

    public GlColor? FillColor { get; set; }

    public IEnumerable<Point> ReadOnlyPoints => Points.AsReadOnly();

    public void Draw(OpenGL gl) {
        // Draw
        gl.PointSize(_pointSize);
        gl.Color(_color.R, _color.G, _color.B);
        gl.Begin(OpenGL.GL_POINTS);
        foreach (var point in Points) {
            gl.Vertex(point.X, point.Y);
        }
        gl.End();

        // Fill
        if (FillColor == null) {
            return;
        }
        gl.PointSize(1f);
        gl.Color(FillColor.Value.R, FillColor.Value.G, FillColor.Value.B);
        gl.Begin(OpenGL.GL_POINTS);
        _fillPoints ??= GetFillPoints(gl);
        foreach (var point in _fillPoints) {
            gl.Vertex(point.X, point.Y);
        }
        gl.End();
    }

    protected abstract List<Point> GetFillPoints(OpenGL gl);

    public virtual void Highlight(OpenGL gl) {
        gl.PointSize(7f);
        gl.Color(0, 0, 0);
        gl.Begin(OpenGL.GL_POINTS);
        gl.Vertex(TopLeft.X, TopLeft.Y);
        gl.Vertex(BottomRight.X, BottomRight.Y);
        gl.Vertex(TopLeft.X, BottomRight.Y);
        gl.Vertex(BottomRight.X, TopLeft.Y);
        int halfX = TopLeft.X + (BottomRight.X - TopLeft.X) / 2, halfY = TopLeft.Y + (BottomRight.Y - TopLeft.Y) / 2;
        gl.Vertex(TopLeft.X, halfY);
        gl.Vertex(BottomRight.X, halfY);
        gl.Vertex(halfX, TopLeft.Y);
        gl.Vertex(halfX, BottomRight.Y);
        gl.End();
    }

    public int SquaredDistance(Point b) {
        return Points.Min(a => {
            int dx = a.X - b.X, dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        });
    }
}
