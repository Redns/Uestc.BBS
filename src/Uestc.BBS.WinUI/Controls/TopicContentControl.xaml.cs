using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core.Services.Forum;
using Uestc.BBS.WinUI.Helpers;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class TopicContentControl : UserControl
    {
        private static readonly Regex _emojiRegex = EmojiRegex();

        public RichTextContent[] TopicContents
        {
            set => TopicContentBorder.Child = RenderTopicContents(value);
        }

        public TopicContentControl()
        {
            InitializeComponent();
        }

        private static RichTextBlock RenderTopicContents(RichTextContent[] contents)
        {
            var richTextBlock = new RichTextBlock { LineHeight = 26 };

            // TODO 优化渲染逻辑
            var paragraph = new Paragraph();
            foreach (var content in contents)
            {
                var inlines = RenderInlineContent(content);
                if (content.Type is not TopicContenType.Image)
                {
                    paragraph.AddRange(inlines);
                    continue;
                }

                if (paragraph.Inlines.Count > 0)
                {
                    richTextBlock.Blocks.Add(paragraph);
                }

                richTextBlock.Blocks.Add(new Paragraph { Inlines = { inlines.First() } });
                paragraph = new Paragraph();
            }

            // XXX 若内容为纯文本，则上述循环不会将 paragraph 加入到 richTextBlock.Blocks 中
            if (paragraph.Inlines.Count > 0)
            {
                richTextBlock.Blocks.Add(paragraph);
            }

            return richTextBlock;
        }

        private static List<Inline> RenderInlineContent(RichTextContent content)
        {
            // 【纯文本】Hello, world!
            // 【文本 + 表情包】Hello [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/22.gif], world!
            if (content.Type is TopicContenType.Text)
            {
                var emojis = _emojiRegex.Matches(content.Information).Where(m => m.Success);
                if (emojis.Any() is false)
                {
                    return [new Run { Text = content.Information }];
                }

                var lastEnd = 0;
                var inlineList = new List<Inline>();
                foreach (Match emoji in emojis)
                {
                    if (emoji.Index > lastEnd)
                    {
                        inlineList.Add(
                            new Run { Text = content.Information[lastEnd..emoji.Index] }
                        );
                    }

                    var emojiImage = new Image
                    {
                        Height = 26,
                        Stretch = Stretch.Uniform,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    ImageCacheHelper.SetSourceEx(emojiImage, new Uri(emoji.Groups["url"].Value));
                    inlineList.Add(new InlineUIContainer { Child = emojiImage });

                    lastEnd = emoji.Index + emoji.Length;
                }

                if (lastEnd < content.Information.Length)
                {
                    inlineList.Add(new Run { Text = content.Information[lastEnd..] });
                }

                return inlineList;
            }

            // 内联链接
            if (content.Type is TopicContenType.InlineLink)
            {
                return
                [
                    new Hyperlink
                    {
                        NavigateUri = new Uri(content.Url),
                        Inlines = { new Run { Text = content.Information } },
                    },
                ];
            }

            // 图片
            if (content.Type is TopicContenType.Image)
            {
                return
                [
                    new InlineUIContainer
                    {
                        Child = new Border
                        {
                            Child = new Image
                            {
                                MaxHeight = 600,
                                Stretch = Stretch.Uniform,
                                Source = new BitmapImage(new Uri(content.Information)),
                            },
                            CornerRadius = new CornerRadius(6),
                            Margin = new Thickness(0, 10, 0, 10),
                            HorizontalAlignment = HorizontalAlignment.Left,
                        },
                    },
                ];
            }

            return
            [
                new InlineUIContainer
                {
                    Child = new InfoBar
                    {
                        Severity = InfoBarSeverity.Warning,
                        Title = "解析失败",
                        Message = content.Information,
                    },
                },
            ];
        }

        [GeneratedRegex(@"\[mobcent_phiz=(?<url>[^\]]+)\]")]
        private static partial Regex EmojiRegex();
    }
}
