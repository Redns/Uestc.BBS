using System;
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
                return new TextBlock { Text = content.Information };
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
    }
}
