using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using FluentIcons.Common;

namespace Uestc.BBS.Desktop.Controls;

public class IconText : TemplatedControl
{
    /// <summary>
    /// ����
    /// </summary>
    public static readonly StyledProperty<Symbol> SymbolProperty = AvaloniaProperty.Register<
        Avatar,
        Symbol
    >(nameof(Width));

    public Symbol Symbol
    {
        get => GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    /// <summary>
    /// ���Ŵ�С
    /// </summary>
    public static readonly StyledProperty<double> SymbolSizeProperty = AvaloniaProperty.Register<
        Avatar,
        double
    >(nameof(SymbolSize), defaultValue: 16);

    public double SymbolSize
    {
        get => GetValue(SymbolSizeProperty);
        set => SetValue(SymbolSizeProperty, value);
    }

    /// <summary>
    /// �ı�
    /// </summary>
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<
        Avatar,
        string
    >(nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// �ı���С
    /// </summary>
    public static readonly StyledProperty<double> TextSizeProperty = AvaloniaProperty.Register<
        Avatar,
        double
    >(nameof(TextSize), defaultValue: 14);

    public double TextSize
    {
        get => GetValue(TextSizeProperty);
        set => SetValue(TextSizeProperty, value);
    }
}
