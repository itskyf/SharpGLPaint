using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SharpGL;
using SharpGL.WPF;
using SharpGLPaint.Shapes;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint;

public class MainViewModel : ObservableObject {
    private readonly List<Shape> _shapes = new();
    private ShapeMode _currentMode = ShapeMode.Line;
    private Shape? _preview;

    private Color _shapeColor = Colors.MediumPurple;
    private Point? _startPoint;
    private int _thickness = 1;

    public MainViewModel() {
        StartDrawCommand = new RelayCommand<OpenGLControl>(StartDraw);
        TrackMouseCommand = new RelayCommand<OpenGLControl>(TrackMouse);
        EndDrawCommand = new RelayCommand<OpenGLControl>(EndDraw);
    }

    public ShapeMode CurrentMode {
        get => _currentMode;
        set => SetProperty(ref _currentMode, value);
    }

    public int Thickness {
        get => _thickness;
        set => SetProperty(ref _thickness, value);
    }

    public Color ShapeColor {
        get => _shapeColor;
        set => SetProperty(ref _shapeColor, value);
    }

    public ICommand StartDrawCommand { get; }
    public ICommand TrackMouseCommand { get; }
    public ICommand EndDrawCommand { get; }

    public void Draw(OpenGL gl) {
        _preview?.Draw(gl);
        foreach (var shape in _shapes) {
            shape.Draw(gl);
        }
    }

    private void StartDraw(OpenGLControl? board) {
        var position = Mouse.GetPosition(board);
        _startPoint = new Point((int)position.X, (int)position.Y);
        _preview = ShapeFactory.Create(
            _currentMode, _startPoint.Value, _startPoint.Value, _shapeColor, _thickness
        );
    }

    private void TrackMouse(OpenGLControl? board) {
        if (_startPoint == null) {
            return;
        }

        var position = Mouse.GetPosition(board);
        Point endPoint = new((int)position.X, (int)position.Y);
        _preview = ShapeFactory.Create(_currentMode, _startPoint.Value, endPoint, _shapeColor, _thickness);
    }

    private void EndDraw(OpenGLControl? board) {
        _shapes.Add(_preview!);
        _preview = null;
        _startPoint = null;
    }
}
