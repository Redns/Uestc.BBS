using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Uestc.BBS.Desktop.Controls;

public class Avatar : TemplatedControl
{
    /// <summary>
    /// 宽度
    /// </summary>
    public static new readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<
        Avatar,
        double
    >(nameof(Width), defaultValue: 32);

    /// <summary>
    /// 显示进度条
    /// </summary>
    public static readonly StyledProperty<bool> IsLoadingVisibleProperty =
        AvaloniaProperty.Register<Avatar, bool>(nameof(IsLoadingVisible), defaultValue: false);

    /// <summary>
    /// 数据源
    /// </summary>
    public static readonly StyledProperty<string?> SourceProperty = AvaloniaProperty.Register<
        Avatar,
        string?
    >(nameof(Source), defaultValue: null);

    /// <summary>
    /// 裁剪几何路径
    /// </summary>
    public static readonly StyledProperty<StreamGeometry?> StreamGeometryProperty =
        AvaloniaProperty.Register<Avatar, StreamGeometry?>(
            nameof(StreamGeometry),
            defaultValue: null
        );

    public Avatar()
    {
        Width = double.IsNaN(Width) ? 32 : Width;
    }

    public new double Width
    {
        get => GetValue(WidthProperty);
        set
        {
            SetValue(WidthProperty, value);
            SetValue(HeightProperty, value);

            if ((Width != value) || (StreamGeometry == null))
            {
                StreamGeometry = CreateCircleStreamGeometry(Width);
            }
        }
    }

    public bool IsLoadingVisible
    {
        get => GetValue(IsLoadingVisibleProperty);
        set => SetValue(IsLoadingVisibleProperty, value);
    }

    public string? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public StreamGeometry? StreamGeometry
    {
        get => GetValue(StreamGeometryProperty);
        set => SetValue(StreamGeometryProperty, value);
    }

    public static StreamGeometry CreateCircleStreamGeometry(double width)
    {
        var diameter = width;
        var radius = width / 2;
        var streamGeometry = new StreamGeometry();
        using var defaultStreamGeometryContext = streamGeometry.Open();

        // M 16 16：移动到点(16, 16)
        defaultStreamGeometryContext.BeginFigure(new Point(radius, radius), true);
        // m -16, 0：相对移动到点(0, 16)
        defaultStreamGeometryContext.LineTo(new Point(0, radius), true);
        // a 16,16 0 1,0 32,0：绘制一个半径为16的圆弧，x轴半径为16，y轴半径为16，0度旋转，大弧线为0，扫过为0
        defaultStreamGeometryContext.ArcTo(
            new Point(diameter, radius),
            new Size(radius, radius),
            0,
            false,
            SweepDirection.Clockwise,
            true
        );
        // a 16,16 0 1,0 -32,0：绘制一个半径为16的圆弧，x轴半径为16，y轴半径为16，0度旋转，大弧线为0，扫过为0
        defaultStreamGeometryContext.ArcTo(
            new Point(0, radius),
            new Size(radius, radius),
            0,
            false,
            SweepDirection.Clockwise,
            true
        );

        return streamGeometry;
    }
}
