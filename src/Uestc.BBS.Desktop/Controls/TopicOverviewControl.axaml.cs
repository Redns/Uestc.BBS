using System;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Uestc.BBS.Desktop.Controls;

public class TopicOverviewControl : TemplatedControl
{
    public static readonly StyledProperty<string?> AvatarProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string?
    >(nameof(Avatar), defaultValue: null);

    public static readonly StyledProperty<string> UsernameProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(Username), defaultValue: string.Empty);

    public static readonly StyledProperty<DateTime> DateProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        DateTime
    >(nameof(Date), defaultValue: DateTime.Now);

    public static readonly StyledProperty<string> BoardNameProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(BoardName), defaultValue: string.Empty);

    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(Title), defaultValue: string.Empty);

    public static readonly StyledProperty<string> SubjectProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(Subject), defaultValue: string.Empty);

    /// <summary>
    /// ͷ��
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public string Username
    {
        set => SetValue(UsernameProperty, value);
    }

    /// <summary>
    /// ����
    /// </summary>
    public DateTime Date
    {
        get => GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }

    /// <summary>
    /// �������
    /// </summary>
    public string BoardName
    {
        get => GetValue(BoardNameProperty);
        set => SetValue(BoardNameProperty, value);
    }

    /// <summary>
    /// ����
    /// </summary>
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// ժҪ
    /// </summary>
    public string Subject
    {
        get => GetValue(SubjectProperty);
        set => SetValue(SubjectProperty, value);
    }
}
