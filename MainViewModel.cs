using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SharpGL.WPF;

namespace SharpGLPaint;

public class MainViewModel : ObservableObject {
    private ShapeMode _shape = ShapeMode.Line;

    private Color _shapeColor = Colors.MediumPurple;

    private Point? _startPoint;

    private int _thickness = 1;

    public MainViewModel() {
        StartDrawCommand = new RelayCommand<OpenGLControl>(StartDraw);
        TrackMouseCommand = new RelayCommand<OpenGLControl>(TrackMouse);
        EndDrawCommand = new RelayCommand<OpenGLControl>(EndDraw);
    }

    public ShapeMode Shape {
        get => _shape;
        set => SetProperty(ref _shape, value);
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

    private void StartDraw(OpenGLControl? board) {
        _startPoint = Mouse.GetPosition(board);
    }

    private void TrackMouse(OpenGLControl? board) {
        if (_startPoint != null) {
        }
    }

    private void EndDraw(OpenGLControl? board) {
        _startPoint = null;
    }
}