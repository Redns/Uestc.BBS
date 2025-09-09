using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.Sdk;
using Uestc.BBS.Sdk.Services.Thread.ThreadContent;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;
using Uestc.BBS.Sdk.Services.Thread.ThreadReply;
using Uestc.BBS.WinUI.Controls;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel(
        ILogService logService,
        INotificationService notificationService,
        [FromKeyedServices(ServiceExtensions.MOBCENT_API)] IThreadListService threadListService,
        [FromKeyedServices(ServiceExtensions.MOBCENT_API)]
            IThreadContentService threadContentService,
        [FromKeyedServices(ServiceExtensions.WEB_API)] IThreadReplyService threadReplyService,
        Uri baseUri,
        AppSettingModel appSettingModel
    )
        : HomeViewModelBase<BoardTabItemListView>(
            logService,
            notificationService,
            threadListService,
            threadContentService,
            threadReplyService,
            model => new BoardTabItemListView() { BoardTabItem = model },
            view => view.BoardTabItem,
            baseUri,
            appSettingModel
        )
    {
        [ObservableProperty]
        public partial PageStatus PageStatus { get; set; } = PageStatus.Idle;
    }

    public enum PageStatus
    {
        Idle = 0,
        Loading,
        Success,
        Timeout,
        Error,
    }
}
