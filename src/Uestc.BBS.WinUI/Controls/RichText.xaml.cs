using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.WinUI.Helpers;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class RichText : UserControl
    {
        private static readonly Regex _emojiRegex = EmojiRegex();

        private static readonly DependencyProperty ContentsProperty = DependencyProperty.Register(
            nameof(Contents),
            typeof(RichTextContent[]),
            typeof(RichText),
            new PropertyMetadata(null, OnContentsChanged)
        );

        public RichTextContent[] Contents
        {
            set => SetValue(ContentsProperty, value);
            get => (RichTextContent[])GetValue(ContentsProperty);
        }

        public RichText()
        {
            InitializeComponent();
        }

        private static void OnContentsChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is not RichText richText || e.NewValue is not RichTextContent[] contents)
            {
                return;
            }

            if (contents.Length == 0)
            {
                return;
            }

            richText.Content = RenderThreadContents(contents);
        }

        // TODO 启用虚拟化
        public static RichTextBlock RenderThreadContents(RichTextContent[] contents)
        {
            var richTextBlock = new RichTextBlock
            {
                LineHeight = 28,
                // FIXME 鼠标选中文本时最后一行文本下半部分缺失
                TextLineBounds = TextLineBounds.TrimToBaseline,
            };

            // TODO 优化渲染逻辑
            var paragraph = new Paragraph()
            {
                LineStackingStrategy = LineStackingStrategy.BlockLineHeight,
            };
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
                paragraph = new Paragraph()
                {
                    LineStackingStrategy = LineStackingStrategy.BlockLineHeight,
                };
            }

            // XXX 若内容为纯文本，则上述循环不会将 paragraph 加入到 richTextBlock.Blocks 中
            if (paragraph.Inlines.Count > 0)
            {
                richTextBlock.Blocks.Add(paragraph);
            }

            return richTextBlock;
        }

        // TODO 统一不同内容的接口，加载时使用 Factory 方式创建对应控件
        public static List<Inline> RenderInlineContent(RichTextContent content)
        {
            // 【纯文本】Hello, world!
            // 【文本 + 表情包】Hello [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/22.gif], world!
            // 【代码块】您也可以下载数据以进行二次开发：\r\n\r\n```js\r\nconst CSV_URL = `https://file.range6.link/get/qshp/monitor/${dateStr}.csv?token=bbs.uestcer.org`;\r\nconst EXAMPLE = \"https://file.range6.link/get/qshp/monitor/2025-08-02.csv?token=bbs.uestcer.org\";\r\n```
            // 【标题】## 主要功能
            // 【无序列表】* 这是一个无序列表项
            // 【加粗】**实时监控**：自动检测新回复消息
            // 【斜体】*欢迎试用并反馈使用体验！*
            // 【分割线】---
            if (content.Type is TopicContenType.Text)
            {
                // 移除多余的 \r\n
                content.Information = RedundantCRLF().Replace(content.Information, "\r\n");

                // 获取表情包
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

                    var emojiImage = new AdvancedImage
                    {
                        Height = 26,
                        Stretch = Stretch.Uniform,
                        Source = emoji.Groups["url"].Value,
                        VerticalAlignment = VerticalAlignment.Center,
                        RenderTransform = new TranslateTransform { Y = 6 },
                    };
                    // TODO 使用本地 alu-face 代替，同时统一行高
                    inlineList.Add(new InlineUIContainer { Child = emojiImage });

                    lastEnd = emoji.Index + emoji.Length;
                }

                // 使用 TextDecorations 属性设置删除线
                if (lastEnd < content.Information.Length)
                {
                    inlineList.Add(new Run { Text = content.Information[lastEnd..] });
                }

                return inlineList;
            }

            // 内联链接
            if (content.Type is TopicContenType.InlineLink)
            {
                if (string.IsNullOrEmpty(content.Information))
                {
                    return [];
                }

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
                // TODO 优化图片渲染逻辑（缩放优化 + 懒加载 + 占位符）
                // 现有逻辑限制图像最大高度，当图片为长截图等情况时，显示效果不佳
                var image = new AdvancedImage
                {
                    MaxHeight = 600,
                    Stretch = Stretch.Uniform,
                    Source = content.Information,
                    IsCachedEnable = true,
                };

                return
                [
                    new InlineUIContainer
                    {
                        Child = new Border
                        {
                            Child = image,
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

        /// <summary>
        /// Emoji 正则表达式
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"\[mobcent_phiz=(?<url>[^\]]+)\]")]
        private static partial Regex EmojiRegex();

        /// <summary>
        /// 移除多余的 \r\n
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"(\r\n)+", RegexOptions.None)]
        private static partial Regex RedundantCRLF();
    }
}
