using System.ComponentModel;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public interface ITopicService
    {
        Task<TopicResp?> GetTopicsAsync(
            int page = 1, 
            int pageSize = 10, 
            int boardId = 0, 
            TopicSortType sortby = TopicSortType.New,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop);
    }

    public enum TopicSortType
    {
        New = 0,    // 最新
        Essence,    // 精华
        All         // 全部
    }

    public enum TopicTopOrder
    {
        WithoutTop = 0,             // 不返回置顶帖
        WithCurrentSectionTop,      // 返回本版置顶帖
        WithCategorySectionTop,     // 返回分类置顶帖
        WithGlobalTop               // 返回全局置顶帖
    }
}
