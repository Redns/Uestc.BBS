using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Uestc.BBS.Desktop.Controls;

public class Avatar : TemplatedControl
{
    /// <summary>
    /// ���
    /// </summary>
    public static new readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<
        Avatar,
        double
    >(nameof(Width));

    /// <summary>
    /// ��ʾ������
    /// </summary>
    public static readonly StyledProperty<bool> IsLoadingVisibleProperty =
        AvaloniaProperty.Register<Avatar, bool>(nameof(IsLoadingVisible), defaultValue: false);

    /// <summary>
    /// ����Դ
    /// </summary>
    public static readonly StyledProperty<string?> SourceProperty = AvaloniaProperty.Register<
        Avatar,
        string?
    >(nameof(Source));

    /// <summary>
    /// �ü�����·��
    /// </summary>
    public static readonly StyledProperty<StreamGeometry?> StreamGeometryProperty =
        AvaloniaProperty.Register<Avatar, StreamGeometry?>(nameof(StreamGeometry));

    public new double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
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

    public override void EndInit()
    {
        StreamGeometry ??= CreateCircleStreamGeometry(Width / 2);
        base.EndInit();
    }

    public static StreamGeometry CreateCircleStreamGeometry(double radius)
    {
        var diameter = radius * 2;
        var streamGeometry = new StreamGeometry();
        using var defaultStreamGeometryContext = streamGeometry.Open();

        // M 16 16���ƶ�����(16, 16)
        defaultStreamGeometryContext.BeginFigure(new Point(radius, radius), true);
        // m -16, 0������ƶ�����(0, 16)
        defaultStreamGeometryContext.LineTo(new Point(0, radius), true);
        // a 16,16 0 1,0 32,0������һ���뾶Ϊ16��Բ����x��뾶Ϊ16��y��뾶Ϊ16��0����ת������Ϊ0��ɨ��Ϊ0
        defaultStreamGeometryContext.ArcTo(
            new Point(diameter, radius),
            new Size(radius, radius),
            0,
            false,
            SweepDirection.Clockwise,
            true
        );
        // a 16,16 0 1,0 -32,0������һ���뾶Ϊ16��Բ����x��뾶Ϊ16��y��뾶Ϊ16��0����ת������Ϊ0��ɨ��Ϊ0
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
