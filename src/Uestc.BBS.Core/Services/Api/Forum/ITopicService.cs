namespace Uestc.BBS.Core.Services.Api.Forum
{
    public interface ITopicService
    {
        Task<TopicResp?> GetTopicsAsync(
            int page = 1,
            int pageSize = 10,
            Board boardId = 0,
            TopicSortType sortby = TopicSortType.New,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop
        );
    }

    /// <summary>
    /// 帖子排序方式
    /// </summary>
    public enum TopicSortType
    {
        New = 0, // 最新
        Essence, // 精华
        All // 全部
    }

    /// <summary>
    /// 帖子置顶配置
    /// </summary>
    public enum TopicTopOrder
    {
        WithoutTop = 0, // 不返回置顶帖
        WithCurrentSectionTop, // 返回本版置顶帖
        WithCategorySectionTop, // 返回分类置顶帖
        WithGlobalTop // 返回全局置顶帖
    }

    /// <summary>
    /// 板块
    /// </summary>
    public enum Board
    {
        Latest = 0, // 最新发表/回复
        WaterHome = 25, // 水手之家
        Transportation = 225, //交通出行
        Anonymous = 371, //密语
        ExamiHome = 382 //考试之家
    }

    public static class TopicExtension
    {
        public static string GetName(
            this Board board,
            TopicSortType sortType = TopicSortType.New
        ) =>
            board switch
            {
                Board.Latest => sortType is TopicSortType.New ? "最新发表" : "最新回复",
                Board.WaterHome => "水手之家",
                Board.Transportation => "交通出行",
                Board.Anonymous => "密语",
                Board.ExamiHome => "考试之家",
                _ => string.Empty
            };
    }
}
