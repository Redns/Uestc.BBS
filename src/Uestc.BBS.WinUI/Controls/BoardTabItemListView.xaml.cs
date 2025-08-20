using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class BoardTabItemListView : UserControl
    {
        private readonly IThreadListService _threaListService =
            ServiceExtension.Services.GetRequiredService<IThreadListService>();

        /// <summary>
        /// ��ǰ���
        /// </summary>
        private static readonly DependencyProperty BoardTabItemProperty =
            DependencyProperty.Register(
                nameof(BoardTabItem),
                typeof(BoardTabItemModel),
                typeof(BoardTabItemListView),
                new PropertyMetadata(null)
            );

        public BoardTabItemModel BoardTabItem
        {
            get => (BoardTabItemModel)GetValue(BoardTabItemProperty);
            set
            {
                SetValue(BoardTabItemProperty, value);
                Topics = new IncrementalLoadingCollection<ThreadOverviewSource, ThreadOverview>(
                    new ThreadOverviewSource(_threaListService, value),
                    itemsPerPage: 30
                );
            }
        }

        /// <summary>
        /// �����б�
        /// </summary>
        public IncrementalLoadingCollection<ThreadOverviewSource, ThreadOverview>? Topics
        {
            get;
            private set;
        }

        public BoardTabItemListView()
        {
            InitializeComponent();
        }

        private void SelectedThreadChanged(object _, ItemClickEventArgs e)
        {
            if (e.ClickedItem is not ThreadOverview threadOverview)
            {
                return;
            }

            StrongReferenceMessenger.Default.Send(new ThreadChangedMessage(threadOverview.Id));
        }
    }

    public class ThreadOverviewSource(
        IThreadListService threadListService,
        BoardTabItemModel boardTabItem
    ) : IIncrementalSource<ThreadOverview>
    {
        // XXX IIncrementalSource �����쳣ʱ�����������������쳣������ [] ��������������ȡ����
        // TODO ʹ�� ScrollViewer �ع�
        public async Task<IEnumerable<ThreadOverview>> GetPagedItemsAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            var threads = await threadListService.GetThreadListAsync(
                route: boardTabItem.Route,
                page: (uint)pageIndex + 1,
                pageSize: (uint)pageSize,
                moduleId: boardTabItem.ModuleId,
                boardId: boardTabItem.Board,
                sortby: boardTabItem.SortType,
                getPreviewSources: boardTabItem.RequirePreviewSources,
                cancellationToken: cancellationToken
            );

            return threads
                .DistinctBy(o => o.Id)
                .Select(o =>
                {
                    // XXX ���Ű���ʱ��Ϊ����ʱ�䣬����Ϊ���»ظ�ʱ��
                    // �������Ű��ʵ�������ɸ����������ɣ���� Board �ֶβ��� Hot �������Ӧ�İ������
                    // ���������Ҫ�жϰ���Ƿ�Ϊ���Ű�飬������ IsHot ����
                    o.IsHot = boardTabItem.Board is Board.Hot;
                    // ���������û�ͷ��
                    o.UserAvatar = o.IsAnonymous
                        ? "ms-appx:///Assets/Icons/anonymous.png"
                        : o.UserAvatar;
                    return o;
                });
        }
    }
}
