using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Thread.ThreadContent;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class Thread : UserControl
    {
        private readonly IThreadContentService _threadContentService =
            ServiceExtension.Services.GetRequiredKeyedService<IThreadContentService>(
                ServiceExtensions.MOBCENT_API
            );

        /// <summary>
        /// 主题 ID
        /// </summary>
        private static readonly DependencyProperty ThreadIdProperty = DependencyProperty.Register(
            nameof(ThreadId),
            typeof(uint),
            typeof(Thread),
            new PropertyMetadata(
                default(uint),
                static (obj, args) =>
                {
                    if (obj is not Thread thread || args.NewValue is not uint threadId)
                    {
                        return;
                    }

                    thread.ThreadReplies = new(
                        new ThreadReplySource(thread._threadContentService, threadId),
                        20
                    );
                }
            )
        );

        public uint ThreadId
        {
            get => (uint)GetValue(ThreadIdProperty);
            set => SetValue(ThreadIdProperty, value);
        }

        /// <summary>
        /// 主题回复列表
        /// </summary>
        private static readonly DependencyProperty ThreadRepliesProperty =
            DependencyProperty.Register(
                nameof(ThreadReplies),
                typeof(IncrementalLoadingCollection<ThreadReplySource, ThreadReplyModel>),
                typeof(Thread),
                new PropertyMetadata(
                    default(IncrementalLoadingCollection<ThreadReplySource, ThreadReplyModel>)
                )
            );

        public IncrementalLoadingCollection<ThreadReplySource, ThreadReplyModel> ThreadReplies
        {
            get =>
                (IncrementalLoadingCollection<ThreadReplySource, ThreadReplyModel>)
                    GetValue(ThreadRepliesProperty);
            set => SetValue(ThreadRepliesProperty, value);
        }

        public Thread()
        {
            InitializeComponent();
        }
    }

    public class ThreadReplySource(IThreadContentService threadContentService, uint threadId)
        : IIncrementalSource<ThreadReplyModel>
    {
        public async Task<IEnumerable<ThreadReplyModel>> GetPagedItemsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var threadContent = await threadContentService.GetThreadContentAsync(
                    threadId,
                    page: (uint)(pageIndex + 1),
                    pageSize: (uint)pageSize,
                    cancellationToken: cancellationToken
                );

                return threadContent.Replies.Select(r => new ThreadReplyModel(r));
            }
            catch
            {
                return [];
            }
        }
    }
}
