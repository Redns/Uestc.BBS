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

        // TODO �������⻯
        public static RichTextBlock RenderThreadContents(RichTextContent[] contents)
        {
            var richTextBlock = new RichTextBlock
            {
                LineHeight = 28,
                // FIXME ���ѡ���ı�ʱ���һ���ı��°벿��ȱʧ
                TextLineBounds = TextLineBounds.TrimToBaseline,
            };

            // TODO �Ż���Ⱦ�߼�
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

            // XXX ������Ϊ���ı���������ѭ�����Ὣ paragraph ���뵽 richTextBlock.Blocks ��
            if (paragraph.Inlines.Count > 0)
            {
                richTextBlock.Blocks.Add(paragraph);
            }

            return richTextBlock;
        }

        // TODO ͳһ��ͬ���ݵĽӿڣ�����ʱʹ�� Factory ��ʽ������Ӧ�ؼ�
        public static List<Inline> RenderInlineContent(RichTextContent content)
        {
            // �����ı���Hello, world!
            // ���ı� + �������Hello [mobcent_phiz=https://bbs.uestc.edu.cn/static/image/smiley/alu/22.gif], world!
            // ������顿��Ҳ�������������Խ��ж��ο�����\r\n\r\n```js\r\nconst CSV_URL = `https://file.range6.link/get/qshp/monitor/${dateStr}.csv?token=bbs.uestcer.org`;\r\nconst EXAMPLE = \"https://file.range6.link/get/qshp/monitor/2025-08-02.csv?token=bbs.uestcer.org\";\r\n```
            // �����⡿## ��Ҫ����
            // �������б�* ����һ�������б���
            // ���Ӵ֡�**ʵʱ���**���Զ�����»ظ���Ϣ
            // ��б�塿*��ӭ���ò�����ʹ�����飡*
            // ���ָ��ߡ�---
            if (content.Type is TopicContenType.Text)
            {
                // �Ƴ������ \r\n
                content.Information = RedundantCRLF().Replace(content.Information, "\r\n");

                // ��ȡ�����
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
                    // TODO ʹ�ñ��� alu-face ���棬ͬʱͳһ�и�
                    inlineList.Add(new InlineUIContainer { Child = emojiImage });

                    lastEnd = emoji.Index + emoji.Length;
                }

                // ʹ�� TextDecorations ��������ɾ����
                if (lastEnd < content.Information.Length)
                {
                    inlineList.Add(new Run { Text = content.Information[lastEnd..] });
                }

                return inlineList;
            }

            // ��������
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

            // ͼƬ
            if (content.Type is TopicContenType.Image)
            {
                // TODO �Ż�ͼƬ��Ⱦ�߼��������Ż� + ������ + ռλ����
                // �����߼�����ͼ�����߶ȣ���ͼƬΪ����ͼ�����ʱ����ʾЧ������
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
                        Title = "����ʧ��",
                        Message = content.Information,
                    },
                },
            ];
        }

        /// <summary>
        /// Emoji ������ʽ
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"\[mobcent_phiz=(?<url>[^\]]+)\]")]
        private static partial Regex EmojiRegex();

        /// <summary>
        /// �Ƴ������ \r\n
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"(\r\n)+", RegexOptions.None)]
        private static partial Regex RedundantCRLF();
    }
}
