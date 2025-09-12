using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Core;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;
using Uestc.BBS.WinUI.Common;

namespace Uestc.BBS.WinUI.Controls
{
    /// <summary>
    /// TODO �ع�����ʵ��
    /// 1. BoardTabItemListView ʵ�� IBoardTabItemListView �ӿڣ��ṩ IsLoading ���ԡ�RefreshThreads ����
    /// 2. �� HashSet �����ھ���ʵ�����У�����˫�б�ɸѡ�ظ�/��������
    /// 3. ViewModel ֱ��ע�����������ʹ�� ServiceExtension.Services������ʹ�� IServiceProvider ע��
    /// </summary>
    public sealed partial class BoardTabItemListView : UserControl
    {
        private readonly IThreadListService _threaListService =
            ServiceExtension.Services.GetRequiredKeyedService<IThreadListService>(
                ServiceExtensions.MOBCENT_API
            );

        private readonly AppSettingModel _appSetting =
            ServiceExtension.Services.GetRequiredService<AppSettingModel>();

        /// <summary>
        /// ��ǰ���
        /// </summary>
        private static readonly DependencyProperty BoardTabItemProperty =
            DependencyProperty.Register(
                nameof(BoardTabItem),
                typeof(BoardTabItemModel),
                typeof(BoardTabItemListView),
                new PropertyMetadata(
                    null,
                    static (obj, args) =>
                    {
                        if (
                            obj is not BoardTabItemListView view
                            || args.NewValue is not BoardTabItemModel model
                        )
                        {
                            return;
                        }

                        view.Threads = new ThreadOverviewSource(view._threaListService, model)
                        {
                            PageSize = 30,

                            KeySelector = t => t.Id,
                            Filter = t =>
                            {
                                if (!view._appSetting.Browse.Filter.IsFilterEnable)
                                {
                                    return true;
                                }

                                // ����ͶƱ
                                if (view._appSetting.Browse.Filter.BlockVote && t.HasVote)
                                {
                                    return false;
                                }

                                // ��������
                                if (
                                    view._appSetting.Browse.Filter.BlockAnonymousUser
                                    && t.IsAnonymous
                                )
                                {
                                    return false;
                                }

                                // ��ͼ����
                                // ������鿪����Ԥ��ͼ�����ʱ��Ч
                                if (
                                    model.RequirePreviewSources
                                    && view._appSetting.Browse.Filter.BlockNoImage
                                    && t.PreviewImageSources.Length is 0
                                )
                                {
                                    return false;
                                }

                                // ���ιؼ���
                                if (
                                    view._appSetting.Browse.Filter.BlockedKeywords.Any(k =>
                                        t.Title.Contains(k) || t.Subject.Contains(k)
                                    )
                                )
                                {
                                    return false;
                                }

                                // ���κ�����
                                if (
                                    view._appSetting.Account.DefaultCredential?.BlacklistUsers.Any(
                                        u => u.Uid == t.Uid
                                    )
                                    is true
                                )
                                {
                                    return false;
                                }

                                // �Զ�����ʽ
                                if (view._appSetting.Browse.Filter.CustomizedFilter(t))
                                {
                                    return false;
                                }

                                return true;
                            },
                        };
                    }
                )
            );

        public BoardTabItemModel BoardTabItem
        {
            get => (BoardTabItemModel)GetValue(BoardTabItemProperty);
            set => SetValue(BoardTabItemProperty, value);
        }

        /// <summary>
        /// �����б�
        /// </summary>
        private static readonly DependencyProperty ThreadsProperty = DependencyProperty.Register(
            nameof(Threads),
            typeof(ThreadOverviewSource),
            typeof(BoardTabItemListView),
            new PropertyMetadata(null)
        );

        public ThreadOverviewSource? Threads
        {
            get => (ThreadOverviewSource)GetValue(ThreadsProperty);
            private set => SetValue(ThreadsProperty, value);
        }

        public BoardTabItemListView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ѡ������
        /// </summary>
        /// <param name="_"></param>
        /// <param name="e"></param>
        private void SelectedThreadChanged(object _, ItemClickEventArgs e)
        {
            if (e.ClickedItem is not ThreadOverview threadOverview)
            {
                return;
            }

            StrongReferenceMessenger.Default.Send(new ThreadChangedMessage(threadOverview.Id));
        }
    }

    public partial class ThreadOverviewSource(
        IThreadListService threadListService,
        BoardTabItemModel boardTabItem
    ) : IncrementalLoadingCollection<uint, ThreadOverview>
    {
        public override async Task<IEnumerable<ThreadOverview>> GetPagedItemsAsync(
            uint page,
            uint pageSize,
            CancellationToken cancellationToken = default
        )
        {
            var threads = await threadListService.GetThreadListAsync(
                route: boardTabItem.Route,
                page: page + 1,
                pageSize: pageSize,
                moduleId: boardTabItem.ModuleId,
                boardId: boardTabItem.Board,
                sortby: boardTabItem.SortType,
                getPreviewSources: boardTabItem.RequirePreviewSources,
                cancellationToken: cancellationToken
            );

            return
            [
                .. threads.Select(o =>
                {
                    // XXX ���Ű���ʱ��Ϊ����ʱ�䣬����Ϊ���»ظ�ʱ��
                    // �������Ű��ʵ�������ɸ����������ɣ���� Board �ֶβ��� Hot �������Ӧ�İ������
                    // ���������Ҫ�жϰ���Ƿ�Ϊ���Ű�飬������ IsHot ����
                    o.IsHot = boardTabItem.Board is Board.Hot;
                    return o;
                }),
            ];
        }
    }
}
