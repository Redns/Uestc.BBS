using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class AppSettingModel(AppSetting setting) : ObservableObject
    {
        /// <summary>
        /// 应用设置
        /// </summary>
        public AppSetting AppSetting => setting;

        /// <summary>
        /// 外观
        /// </summary>
        public AppearanceSettingModel Appearance { get; set; } = new(setting.Appearance);

        /// <summary>
        /// 浏览
        /// </summary>
        public BrowseSettingModel Browse { get; set; } = new(setting.Browse);

        /// <summary>
        /// 授权
        /// </summary>
        public AccountSettingModel Account { get; set; } = new(setting.Account);

        /// <summary>
        /// 通知
        /// </summary>
        public NotificationSettingModel Notification { get; set; } = new(setting.Notification);

        /// <summary>
        /// 服务
        /// </summary>
        public ServicesSettingModel Services { get; set; } = new(setting.Services);

        /// <summary>
        /// 数据与存储
        /// </summary>
        public StorageSettingModel Storage { get; set; } = new(setting.Storage);

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="path"></param>
        public void Save(string? path = null) => setting.Save(path);
    }

}
