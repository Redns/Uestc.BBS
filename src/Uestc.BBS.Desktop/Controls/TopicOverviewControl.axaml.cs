using System;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Uestc.BBS.Desktop.Controls;

public class TopicOverviewControl : TemplatedControl
{
    /// <summary>
    /// ͷ��
    /// </summary>
    public static readonly StyledProperty<string?> AvatarProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string?
    >(nameof(Avatar), defaultValue: null);

    public string? Avatar
    {
        get => GetValue(AvatarProperty);
        set => SetValue(AvatarProperty, value);
    }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public static readonly StyledProperty<string> UsernameProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(Username), defaultValue: string.Empty);

    public string Username
    {
        get => GetValue(UsernameProperty);
        set => SetValue(UsernameProperty, value);
    }

    /// <summary>
    /// ����/���»ظ�����
    /// </summary>
    public static readonly StyledProperty<DateTime> DateProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        DateTime
    >(nameof(Date), defaultValue: DateTime.Now);

    public DateTime Date
    {
        get => GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }

    /// <summary>
    /// �������
    /// </summary>
    public static readonly StyledProperty<string> BoardNameProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(BoardName), defaultValue: string.Empty);

    public string BoardName
    {
        get => GetValue(BoardNameProperty);
        set => SetValue(BoardNameProperty, value);
    }

    /// <summary>
    /// Ԥ��ͼ
    /// </summary>
    public static readonly StyledProperty<string?> PreviewSourceProperty =
        AvaloniaProperty.Register<TopicOverviewControl, string?>(
            nameof(PreviewSource),
            defaultValue: null
        );

    public string? PreviewSource
    {
        get => GetValue(PreviewSourceProperty);
        set => SetValue(PreviewSourceProperty, value);
    }

    /// <summary>
    /// ����
    /// </summary>
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(Title), defaultValue: string.Empty);

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// ժҪ
    /// </summary>
    public static readonly StyledProperty<string> SubjectProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        string
    >(nameof(Subject), defaultValue: string.Empty);

    public string Subject
    {
        get => GetValue(SubjectProperty);
        set => SetValue(SubjectProperty, value);
    }

    /// <summary>
    /// ������
    /// </summary>
    public static readonly StyledProperty<uint> LikesProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        uint
    >(nameof(Likes), defaultValue: 0);

    public uint Likes
    {
        get => GetValue(LikesProperty);
        set => SetValue(LikesProperty, value);
    }

    /// <summary>
    /// �ظ���
    /// </summary>
    public static readonly StyledProperty<uint> RepliesProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        uint
    >(nameof(Replies), defaultValue: 0);

    public uint Replies
    {
        get => GetValue(RepliesProperty);
        set => SetValue(RepliesProperty, value);
    }

    /// <summary>
    /// �����
    /// </summary>
    public static readonly StyledProperty<uint> ViewsProperty = AvaloniaProperty.Register<
        TopicOverviewControl,
        uint
    >(nameof(Views), defaultValue: 0);

    public uint Views
    {
        get => GetValue(ViewsProperty);
        set => SetValue(ViewsProperty, value);
    }

    public override void EndInit()
    {
        base.EndInit();
    }
}
