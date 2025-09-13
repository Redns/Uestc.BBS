using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Sdk.Services.System;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class GlobalStatusModel : ObservableObject
    {
        /// <summary>
        /// 今日发帖数
        /// </summary>
        [ObservableProperty]
        public partial uint TodayPostCount { get; set; }

        /// <summary>
        /// 昨日发帖数
        /// </summary>
        [ObservableProperty]
        public partial uint YesterdayPostCount { get; set; }

        /// <summary>
        /// 总发帖数
        /// </summary>
        [ObservableProperty]
        public partial uint TotalPostCount { get; set; }

        /// <summary>
        /// 总用户数
        /// </summary>
        [ObservableProperty]
        public partial uint TotalUserCount { get; set; }

        /// <summary>
        /// 新用户
        /// </summary>
        [ObservableProperty]
        public partial NewUser? NewUser { get; set; }
    }
}
