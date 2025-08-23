using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Uestc.BBS.WinUI.Controls;

public sealed partial class Avatar : UserControl
{
    /// <summary>
    /// Source
    /// </summary>
    private static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source),
        typeof(string),
        typeof(Avatar),
        new PropertyMetadata(null)
    );

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    /// ³ß´ç
    /// </summary>
    private static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(int),
        typeof(Avatar),
        new PropertyMetadata(36)
    );

    public int Size
    {
        get => (int)GetValue(SizeProperty);
        set
        {
            SetValue(SizeProperty, value);
            SetValue(CornerRadiusProperty, new CornerRadius(value / 2));
        }
    }

    public Avatar()
    {
        InitializeComponent();
    }
}
