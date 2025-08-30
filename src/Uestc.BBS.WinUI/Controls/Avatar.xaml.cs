using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

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
        set
        {
            if (value.Split('?').LastOrDefault() is "uid=0&size=middle")
            {
                value = "ms-appx:///Assets/Icons/anonymous.png";
            }
            SetValue(SourceProperty, value);
        }
    }

    /// <summary>
    /// 尺寸
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

    /// <summary>
    /// 拉伸状态
    /// </summary>
    private static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
        nameof(Stretch),
        typeof(Stretch),
        typeof(Avatar),
        new PropertyMetadata(Stretch.UniformToFill)
    );

    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    /// <summary>
    /// 是否启用延迟加载（默认关闭）
    /// XXX 延迟加载和虚拟化冲突，只能启用其中一个
    /// </summary>
    private static readonly DependencyProperty IsLazyLoadEnableProperty =
        DependencyProperty.Register(
            nameof(IsLazyLoadEnable),
            typeof(bool),
            typeof(Avatar),
            new PropertyMetadata(false)
        );

    public bool IsLazyLoadEnable
    {
        get => (bool)GetValue(IsLazyLoadEnableProperty);
        set => SetValue(IsLazyLoadEnableProperty, value);
    }

    /// <summary>
    /// 是否启用缓存（默认开启）
    /// </summary>
    private static readonly DependencyProperty IsCachedEnableProperty = DependencyProperty.Register(
        nameof(IsCachedEnable),
        typeof(bool),
        typeof(Avatar),
        new PropertyMetadata(true)
    );

    public bool IsCachedEnable
    {
        get => (bool)GetValue(IsCachedEnableProperty);
        set => SetValue(IsCachedEnableProperty, value);
    }

    public Avatar()
    {
        InitializeComponent();
    }
}
