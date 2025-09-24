using System;
using SqlSugar;

namespace Uestc.BBS.Entities
{
    [SugarTable("ThreadHistories")]
    public class ThreadHistoryEntity
    {
        /// <summary>
        /// 主题 ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public uint Id { get; set; }

        /// <summary>
        /// 板块名称
        /// </summary>
        [SugarColumn(Length = 8)]
        public string BoardName { get; set; } = string.Empty;

        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(Length = 64)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 摘要
        /// </summary>
        [SugarColumn(Length = 128)]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// 浏览时间
        /// </summary>
        [SugarColumn(DefaultValue = "(strftime('%Y-%m-%d %H:%M:%S', 'now', 'localtime'))")]
        public DateTime BrowserDateTime { get; set; }

        /// <summary>
        /// 用户 ID
        /// </summary>
        public uint Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(Length = 16)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 用户头像
        /// </summary>
        [SugarColumn(Length = 128)]
        public string UserAvatar { get; set; } = string.Empty;

        /// <summary>
        /// 是否包含投票
        /// </summary>
        public bool HasVote { get; set; }
    }
}
