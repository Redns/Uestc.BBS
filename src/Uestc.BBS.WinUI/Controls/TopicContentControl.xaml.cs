using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core.Services.Api.Forum;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class TopicContentControl : UserControl
    {
        private static readonly Regex _emojiRegex = EmojiRegex();

        public TopicContent TopicContent
        {
            set => Content = RenderTopicContent(value);
        }

        public TopicContentControl()
        {
            InitializeComponent();
        }

        private UIElement RenderTopicContent(TopicContent content)
        {
            //
            // [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/22.gif][mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/22.gif][mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/22.gif]
            if (content.Type is TopicContenType.Text)
            {
                var emojis = _emojiRegex.Matches(content.Information).Where(m => m.Success);
                if (emojis.Any() is false)
                {
                    return new TextBlock
                    {
                        Text = content.Information,
                        TextWrapping = TextWrapping.WrapWholeWords
                    };
                }

                var lastEnd = 0;
                var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                foreach (Match emoji in emojis)
                {
                    if (emoji.Index > lastEnd)
                    {
                        stackPanel.Children.Add(
                            new TextBlock
                            {
                                Text = content.Information[lastEnd..emoji.Index],
                                TextWrapping = TextWrapping.WrapWholeWords,
                                TextAlignment = TextAlignment.Center,
                            }
                        );
                    }
                    stackPanel.Children.Add(
                        new Image
                        {
                            Stretch = Stretch.Uniform,
                            VerticalAlignment = VerticalAlignment.Center,
                            Source = new BitmapImage(new Uri(emoji.Groups["url"].Value)),
                        }
                    );
                    lastEnd = emoji.Index + emoji.Length;
                }

                if (lastEnd < content.Information.Length)
                {
                    stackPanel.Children.Add(
                        new TextBlock
                        {
                            Text = content.Information[lastEnd..],
                            TextWrapping = TextWrapping.WrapWholeWords
                        }
                    );
                }

                return stackPanel;
            }

            if (content.Type is TopicContenType.InlineLink)
            {
                return new RichTextBlock
                {
                    Blocks =
                    {
                        new Paragraph
                        {
                            Inlines =
                            {
                                new Hyperlink
                                {
                                    NavigateUri = new Uri(content.Url),
                                    Inlines = { new Run { Text = content.Information } },
                                },
                            },
                        },
                    },
                };
            }

            if (content.Type is TopicContenType.Image)
            {
                return new Border
                {
                    Child = new Image
                    {
                        MaxHeight = 400,
                        Stretch = Stretch.Uniform,
                        Source = new BitmapImage(new Uri(content.Url)),
                    },
                    CornerRadius = new CornerRadius(6),
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
            }

            return new TextBlock { Text = $"Unknown content type {content.Type}" };
        }

        [GeneratedRegex(@"\[mobcent_phiz=(?<url>[^\]]+)\]")]
        private static partial Regex EmojiRegex();
    }
}
