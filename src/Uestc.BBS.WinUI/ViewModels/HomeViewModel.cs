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
    public partial class HomeViewModel : HomeViewModelBase<BoardTabItemListView>
    {
        [ObservableProperty]
        public partial PageStatus PageStatus { get; set; } = PageStatus.Idle;

        public HomeViewModel(
            ILogService logService,
            INotificationService notificationService,
            [FromKeyedServices(ServiceExtensions.MOBCENT_API)] IThreadListService threadListService,
            [FromKeyedServices(ServiceExtensions.MOBCENT_API)]
                IThreadContentService threadContentService,
            [FromKeyedServices(ServiceExtensions.WEB_API)] IThreadReplyService threadReplyService,
            Uri baseUri,
            AppSettingModel appSettingModel
        )
            : base(
                logService,
                notificationService,
                threadListService,
                threadContentService,
                threadReplyService,
                baseUri,
                appSettingModel
            )
        {
            BoardTabItemModelToView = model =>
                new(appSettingModel, threadListService) { BoardTabItem = model };
            BoardTabItemModelFromView = view => view.BoardTabItem;
        }
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
