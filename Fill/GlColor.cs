using System.Windows.Media;

namespace SharpGLPaint.Fill;

public readonly struct GlColor {
    public GlColor(Color color) {
        R = color.R / 255f;
        G = color.G / 255f;
        B = color.B / 255f;
    }

    public readonly float R, G, B;
}
