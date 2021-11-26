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
        // Clear the color and depth buffer
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        _viewModel.Draw(gl);
        gl.End();
        //Execute a drawing immediately instead of waiting after a certain amount of time
        gl.Flush();
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
