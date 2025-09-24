using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uestc.BBS.Mvvm.Messages;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;
using Uestc.BBS.WinUI.Common;

namespace Uestc.BBS.WinUI.Controls
{
    public sealed partial class BoardTabItemListView : UserControl
    {
        /// <summary>
        /// Ӧ������
        /// </summary>
        public AppSettingModel AppSetting { get; private set; }

        /// <summary>
        /// �����б����
        /// </summary>
        public IThreadListService ThreadListService { get; private set; }

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

                        view.Threads = new ThreadOverviewSource(view.ThreadListService, model)
                        {
                            PageSize = 50,
                            KeySelector = t => t.Id,
                            Filter = t =>
                            {
                                if (!view.AppSetting.Browse.Filter.IsFilterEnable)
                                {
                                    return true;
                                }

                                // ����ͶƱ
                                if (view.AppSetting.Browse.Filter.BlockVote && t.HasVote)
                                {
                                    return false;
                                }

                                // ��������
                                if (
                                    view.AppSetting.Browse.Filter.BlockAnonymousUser
                                    && t.IsAnonymous
                                )
                                {
                                    return false;
                                }

                                // ��ͼ����
                                // ������鿪����Ԥ��ͼ�����ʱ��Ч
                                if (
                                    model.RequirePreviewSources
                                    && view.AppSetting.Browse.Filter.BlockNoImage
                                    && t.PreviewImageSources.Length is 0
                                )
                                {
                                    return false;
                                }

                                // ���ιؼ���
                                if (
                                    view.AppSetting.Browse.Filter.BlockedKeywords.Any(k =>
                                        t.Title.Contains(k) || t.Subject.Contains(k)
                                    )
                                )
                                {
                                    return false;
                                }

                                // ���κ�����
                                if (
                                    view.AppSetting.Account.DefaultCredential?.BlacklistUsers.Any(
                                        u => u.Uid == t.Uid
                                    )
                                    is true
                                )
                                {
                                    return false;
                                }

                                // �Զ�����ʽ
                                if (view.AppSetting.Browse.Filter.CustomizedFilter(t))
                                {
                                    return false;
                                }

                                return true;
                            },
                        };
                        view.Threads.OnRefresh += () =>
                        {
                            view.Status = BoardStatus.Normal;
                        };
                        view.Threads.OnException += exception =>
                        {
                            view.Status = BoardStatus.Error;
                            view.AlertMessage = exception.Message;
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

        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        private static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            nameof(Status),
            typeof(BoardStatus),
            typeof(BoardTabItemListView),
            new PropertyMetadata(BoardStatus.Normal)
        );

        public BoardStatus Status
        {
            get => (BoardStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private static readonly DependencyProperty AlertMessageProperty =
            DependencyProperty.Register(
                nameof(AlertMessage),
                typeof(string),
                typeof(BoardTabItemListView),
                new PropertyMetadata(string.Empty)
            );

        public string? AlertMessage
        {
            get => (string)GetValue(AlertMessageProperty);
            set => SetValue(AlertMessageProperty, value);
        }

        public BoardTabItemListView(
            AppSettingModel appSetting,
            IThreadListService threadListService
        )
        {
            AppSetting = appSetting;
            ThreadListService = threadListService;

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
            StrongReferenceMessenger.Default.Send(new ThreadHistoryChangedMessage(threadOverview));
        }
    }

    public partial class ThreadOverviewSource(
        IThreadListService threadListService,
        BoardTabItemModel boardTabItem
    ) : IncrementalLoadingCollection<uint, ThreadOverview>
    {
        public override async Task<IEnumerable<ThreadOverview>> GetPagedItemsAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            var threads = await threadListService.GetThreadListAsync(
                route: boardTabItem.Route,
                page: (uint)page + 1,
                pageSize: (uint)pageSize,
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

    public enum BoardStatus
    {
        Normal,
        Error,
    }
}
