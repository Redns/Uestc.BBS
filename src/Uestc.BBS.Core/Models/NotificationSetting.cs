namespace Uestc.BBS.Core.Models
{
    public class NotificationSetting
    {
        /// <summary>
        /// 是否开启通知
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 系统
        /// </summary>
        public bool IsSystenMessageEnabled { get; set; } = true;

        /// <summary>
        /// 私信
        /// </summary>
        public bool IsPrivateMessageEnabled { get; set; } = true;

        /// <summary>
        /// 回复
        /// </summary>
        public bool IsNewReplyEnabled { get; set; } = true;

        /// <summary>
        /// @我
        /// </summary>
        public bool IsAtMeEnabled { get; set; } = true;

        /// <summary>
        /// 点评
        /// </summary>
        public bool IsReviewEnabled { get; set; } = true;

        /// <summary>
        /// 插眼
        /// </summary>
        public bool IsMarkEnabled { get; set; } = true;
    }
}
