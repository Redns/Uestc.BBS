using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Services.System;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;
using Uestc.BBS.Sdk.Services.Thread.ThreadContent;
using Uestc.BBS.Sdk.Services.Thread.ThreadList;
using Uestc.BBS.WinUI.Controls;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel(
        ILogService logService,
        IThreadListService threadListService,
        IThreadContentService threadContentService,
        AppSettingModel appSettingModel
    )
        : HomeViewModelBase<BoardTabItemListView>(
            logService,
            threadListService,
            threadContentService,
            model => new BoardTabItemListView() { BoardTabItem = model },
            view => view.BoardTabItem,
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
