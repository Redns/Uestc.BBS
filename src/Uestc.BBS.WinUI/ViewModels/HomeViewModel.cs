using System;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Uestc.BBS.Core.Services.Api.Forum;
using Uestc.BBS.Mvvm.Models;
using Uestc.BBS.Mvvm.ViewModels;

namespace Uestc.BBS.WinUI.ViewModels
{
    public partial class HomeViewModel(AppSettingModel appSettingModel, ITopicService topicService)
        : HomeViewModelBase(appSettingModel, topicService)
    {
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public override Task DispatcherAsync(Action action) =>
            _dispatcherQueue.EnqueueAsync(action);
    }
}
