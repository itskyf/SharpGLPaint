using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SharpGL;
using SharpGL.WPF;
using SharpGLPaint.Fill;
using SharpGLPaint.Shapes;
using Color = System.Windows.Media.Color;

namespace SharpGLPaint;

public class MainViewModel : ObservableObject {
    private readonly List<Point> _polyPoints = new();
    private readonly ObservableCollection<Shape> _shapes = new();
    private readonly Stopwatch _timer = new();
    private bool _changed = true;
    private Color _color = Colors.MediumPurple;
    private string _drawTime = "-";
    private ShapeMode? _mode = ShapeMode.Line;

    private Shape? _preview;

    private Shape? _selectedShape;

    private Point? _startPoint;
    private int _thickness = 1;

    public MainViewModel() {
        ClearCommand = new RelayCommand(() => {
            DrawTime = "-";
            _shapes.Clear();
            SelectedShape = null;
            RemovePreview();
        });
        FillCommand = new RelayCommand(() => {
            if (SelectedShape != null) {
                SelectedShape.FillColor = new GlColor(_color);
            }
        });
        ModeCommand = new RelayCommand<ShapeMode?>(mode => {
            _mode = mode;
            SelectedShape = null;
            RemovePreview();
        });
        LClickCommand = new RelayCommand<OpenGLControl>(LClick);
        RClickCommand = new RelayCommand<OpenGLControl>(RClick);
        TrackMouseCommand = new RelayCommand<OpenGLControl>(TrackMouse);
        _shapes.CollectionChanged += (sender, e) => { _changed = true; };
    }

    private Shape? Preview {
        set {
            _changed = true;
            _preview = value;
        }
    }

    private Shape? SelectedShape {
        get {
            _changed = true;
            return _selectedShape;
        }
        set {
            _changed = true;
            _selectedShape = value;
        }
    }

    public int Thickness {
        get => _thickness;
        set => SetProperty(ref _thickness, value);
    }

    public Color ShapeColor {
        get => _color;
        set => SetProperty(ref _color, value);
    }

    public string DrawTime {
        get => _drawTime;
        set => SetProperty(ref _drawTime, value);
    }

    public ICommand ClearCommand { get; }
    public ICommand FillCommand { get; }
    public ICommand LClickCommand { get; }
    public ICommand RClickCommand { get; }
    public ICommand ModeCommand { get; }
    public ICommand TrackMouseCommand { get; }

    public void Draw(OpenGL gl) {
        if (!_changed) {
            return;
        }
        // Clear the color and depth buffer
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        foreach (var shape in _shapes) {
            shape.Draw(gl);
        }
        _preview?.Draw(gl);
        _selectedShape?.Highlight(gl);
        //Execute a drawing immediately instead of waiting after a certain amount of time
        gl.Flush();
        _changed = false;
    }

    private void LClick(OpenGLControl? board) {
        var position = Mouse.GetPosition(board);
        Point point = new((int)position.X, (int)position.Y);

        switch (_mode) {
            case null:
                SelectedShape = _shapes.MinBy(shape => shape.SquaredDistance(point));
                break;
            case ShapeMode.Polygon:
                if (_polyPoints.Count == 0 || _polyPoints[^1] != point) {
                    _polyPoints.Add(point);
                    _startPoint = point;
                }
                break;
            case ShapeMode.Line:
            case ShapeMode.Circle:
            case ShapeMode.Rectangle:
            case ShapeMode.Ellipse:
            case ShapeMode.Square:
            case ShapeMode.Pentagon:
            case ShapeMode.Hexagon:
                if (_startPoint == null) {
                    _startPoint = point;
                } else {
                    if (_preview != null) {
                        _shapes.Add(_preview);
                    }
                    RemovePreview();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_mode), _mode, "The inspector make me do it");
        }
    }

    private void RClick(OpenGLControl? board) {
        if (_mode is not ShapeMode.Polygon) {
            return;
        }
        var position = Mouse.GetPosition(board);
        Point endPoint = new((int)position.X, (int)position.Y);
        if (_polyPoints[^1] != endPoint) {
            _polyPoints.Add(endPoint);
        }
        _shapes.Add(DrawWithTimer(_polyPoints));
        RemovePreview();
    }

    private void TrackMouse(OpenGLControl? board) {
        if (_startPoint == null) {
            return;
        }
        var position = Mouse.GetPosition(board);
        Point endPoint = new((int)position.X, (int)position.Y);

        if (_mode is ShapeMode.Polygon && _polyPoints[^1] == endPoint) {
            return;
        }

        if (_mode is not ShapeMode.Polygon && _startPoint == endPoint) {
            Preview = null;
        }

        Preview = _mode is ShapeMode.Polygon
            ? DrawWithTimer(_polyPoints, endPoint)
            : DrawWithTimer(_startPoint, endPoint);
    }

    private Shape DrawWithTimer(params object[] parameters) {
        _timer.Start();
        var shape = ShapeFactory.Create(_mode!.Value, _color, _thickness, parameters);
        var milliseconds = _timer.Elapsed.TotalMilliseconds;
        // Use ms unit if elapsed time > 100 μs
        DrawTime = milliseconds > 0.1 ? $"{milliseconds:F} ms" : $"{milliseconds * 1000:00.00} μs";
        _timer.Reset();
        return shape;
    }

    private void RemovePreview() {
        _startPoint = null;
        Preview = null;
        _polyPoints.Clear();
    }
}
