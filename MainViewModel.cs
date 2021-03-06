using System.Collections.Generic;
using System.Diagnostics;
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
    /// <value>
    ///     Represents all saved shapes
    /// </value>
    private readonly List<Shape> _shapes = new();

    private ShapeMode _currentMode = ShapeMode.Line;
    private string _drawTime = "-";
    private Shape? _preview;
    private Color _shapeColor = Colors.MediumPurple;
    private Point? _startPoint;
    private int _thickness = 1;

    public MainViewModel() {
        StartDrawCommand = new RelayCommand<OpenGLControl>(StartDraw);
        TrackMouseCommand = new RelayCommand<OpenGLControl>(TrackMouse);
        EndDrawCommand = new RelayCommand<OpenGLControl>(EndDraw);
        ClearCommand = new RelayCommand(() => {
            _shapes.Clear();
            DrawTime = "-";
        });
    }

    /// <value>
    ///     Selected shape in the ribbon
    /// </value>
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

    public string DrawTime {
        get => _drawTime;
        set => SetProperty(ref _drawTime, value);
    }

    public ICommand StartDrawCommand { get; }
    public ICommand TrackMouseCommand { get; }
    public ICommand EndDrawCommand { get; }
    public ICommand ClearCommand { get; }

    /// <summary>
    ///     Draw the preview and all saved shapes
    /// </summary>
    public void Draw(OpenGL gl) {
        _preview?.Draw(gl);
        foreach (var shape in _shapes) {
            shape.Draw(gl);
        }
    }

    /// <summary>
    ///     Binding for MouseDown action, record the start position
    /// </summary>
    private void StartDraw(OpenGLControl? board) {
        var position = Mouse.GetPosition(board);
        _startPoint = new Point((int)position.X, (int)position.Y);
        _preview = CreatePreview(_startPoint.Value);
    }

    /// <summary>
    ///     Binding for MouseMove action, record end point and create a preview shape
    /// </summary>
    private void TrackMouse(OpenGLControl? board) {
        if (_startPoint == null) {
            return;
        }

        var position = Mouse.GetPosition(board);
        _preview = CreatePreview(new Point((int)position.X, (int)position.Y));
    }

    /// <summary>
    ///     Create a shape preview and update drawing time
    /// </summary>
    private Shape CreatePreview(Point endPoint) {
        Stopwatch timer = new();
        timer.Start();
        var preview = ShapeFactory.Create(
            _currentMode, _startPoint!.Value, endPoint, _shapeColor, _thickness
        );
        timer.Stop();
        var milliseconds = timer.Elapsed.TotalMilliseconds;
        // Use ms unit if elapsed time > 100 μs
        DrawTime = milliseconds > 0.1 ? $"{milliseconds:F} ms" : $"{milliseconds * 1000:00.00} μs";
        return preview;
    }

    /// <summary>
    ///     Add the preview to saved shapes and reset start position
    /// </summary>
    private void EndDraw(OpenGLControl? board) {
        if (_preview != null) {
            _shapes.Add(_preview);
        }

        _preview = null;
        _startPoint = null;
    }
}
