using CommunityToolkit.Mvvm.ComponentModel;
using Uestc.BBS.Sdk.Services.Thread;
using Uestc.BBS.Sdk.Services.Thread.ThreadContent;

namespace Uestc.BBS.Mvvm.Models
{
    public partial class ThreadReplyModel(ThreadReply threadReply) : ObservableObject
    {
        /// <summary>
        /// ID
        /// </summary>
        [ObservableProperty]
        public partial uint Id { get; set; } = threadReply.Id;

        /// <summary>
        /// 创建时间
        /// </summary>
        [ObservableProperty]
        public partial DateTime CreateTime { get; set; } = threadReply.CreateTime;

        /// <summary>
        /// 点赞数
        /// </summary>
        [ObservableProperty]
        public partial uint LikeCount { get; set; } = threadReply.LikeCount;

        /// <summary>
        /// 点踩数
        /// </summary>
        [ObservableProperty]
        public partial uint DislikeCount { get; set; } = threadReply.DislikeCount;

        /// <summary>
        /// 楼层
        /// </summary>
        [ObservableProperty]
        public partial uint Position { get; set; } = threadReply.Position;

        /// <summary>
        /// 内容
        /// </summary>
        [ObservableProperty]
        public partial RichTextContent[] Contents { get; set; } = threadReply.Contents;

        /// <summary>
        /// 是否置顶
        /// </summary>
        [ObservableProperty]
        public partial bool IsPinned { get; set; } = threadReply.IsPinned;

        /// <summary>
        /// 是否是楼主
        /// </summary>
        [ObservableProperty]
        public partial bool IsFromThreadMaster { get; set; } = threadReply.IsFromThreadMaster;

        /// <summary>
        /// 用户 ID
        /// </summary>
        [ObservableProperty]
        public partial uint Uid { get; set; } = threadReply.Uid;

        /// <summary>
        /// 用户名
        /// </summary>
        [ObservableProperty]
        public partial string Username { get; set; } = threadReply.Username;

        /// <summary>
        /// 用户头像
        /// </summary>
        [ObservableProperty]
        public partial string UserAvatar { get; set; } = threadReply.UserAvatar;

        /// <summary>
        /// 用户等级
        /// </summary>
        [ObservableProperty]
        public partial uint UserLevel { get; set; } = threadReply.UserLevel;

        /// <summary>
        /// 用户组
        /// </summary>
        [ObservableProperty]
        public partial string UserGroup { get; set; } = threadReply.UserGroup;

        /// <summary>
        /// 是否有引用
        /// </summary>
        [ObservableProperty]
        public partial bool HasQuote { get; set; } = threadReply.HasQuote;

        /// <summary>
        /// 引用的主题 ID
        /// </summary>
        [ObservableProperty]
        public partial uint QuoteId { get; set; } = threadReply.QuoteId;

        /// <summary>
        /// 引用的主题用户名
        /// </summary>
        [ObservableProperty]
        public partial string QuoteUsername { get; set; } = threadReply.QuoteUsername;

        /// <summary>
        /// 引用的主题用户头像
        /// </summary>
        [ObservableProperty]
        public partial string QuoteUserAvatar { get; set; } = threadReply.QuoteUserAvatar;

        /// <summary>
        /// 引用的主题内容
        /// </summary>
        [ObservableProperty]
        public partial string QuoteContent { get; set; } = threadReply.QuoteContent;
    }
}
