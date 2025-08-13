using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uestc.BBS.Core;
using Uestc.BBS.Core.Services.FileCache;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.WinUI.Helpers;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class ThreadContentControl : UserControl
    {
        private static readonly Regex _emojiRegex = EmojiRegex();

        private static readonly IFileCache _fileCache =
            ServiceExtension.Services.GetRequiredService<IFileCache>();

        private static readonly DependencyProperty ContentsProperty = DependencyProperty.Register(
            nameof(Contents),
            typeof(RichTextContent[]),
            typeof(ThreadContentControl),
            new PropertyMetadata(null, OnContentsChanged)
        );

        public RichTextContent[] Contents
        {
            set => SetValue(ContentsProperty, value);
            get => (RichTextContent[])GetValue(ContentsProperty);
        }

        public ThreadContentControl()
        {
            InitializeComponent();
        }

        private static async void OnContentsChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (
                d is not ThreadContentControl threadContentControl
                || e.NewValue is not RichTextContent[] contents
            )
            {
                return;
            }

            threadContentControl.TopicContentBorder.Child = await RenderTopicContentsAsync(
                contents
            );
        }

        private static async Task<RichTextBlock> RenderTopicContentsAsync(
            RichTextContent[] contents
        )
        {
            var richTextBlock = new RichTextBlock { LineHeight = 26 };

            // TODO 优化渲染逻辑
            var paragraph = new Paragraph();
            foreach (var content in contents)
            {
                var inlines = await RenderInlineContentAsync(content);
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

        private static async Task<List<Inline>> RenderInlineContentAsync(RichTextContent content)
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
                    ImageCacheHelper.SetSourceEx(emojiImage, emoji.Groups["url"].Value);
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
                var imageUri = await _fileCache.GetFileUriAsync(new Uri(content.Information));
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
                                Source = new BitmapImage(imageUri),
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
