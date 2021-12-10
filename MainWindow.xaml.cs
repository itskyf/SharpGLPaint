using SharpGL;
using SharpGL.WPF;

namespace SharpGLPaint;

public partial class MainWindow {
    private readonly MainViewModel _viewModel;

    public MainWindow() {
        InitializeComponent();
        ColorPicker.StandardColors.RemoveAt(0);
        // Link with ViewModel in XAML
        _viewModel = (MainViewModel)DataContext;
    }

    private void Board_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {
        var gl = Board.OpenGL;
        _viewModel.Draw(gl);
    }

    private void Board_Resized(object sender, OpenGLRoutedEventArgs args) {
        var gl = Board.OpenGL;
        int width = (int)Board.ActualWidth, height = (int)Board.ActualHeight;
        // Set PROJECTION as the current matrix mode
        gl.MatrixMode(OpenGL.GL_PROJECTION);
        gl.LoadIdentity();
        // Set background color to white
        gl.ClearColor(1, 1, 1, 1);
        // Transform perspective from OpenGL to WPF viewport
        gl.Viewport(0, 0, width, height);
        gl.Ortho2D(0, width, height, 0);
    }
}
