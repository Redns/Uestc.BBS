namespace Uestc.BBS.Core.Services.Api.Forum
{
    public interface ITopicService
    {
        Task<TopicResp?> GetTopicsAsync(
            string? route = null,
            int page = 1,
            int pageSize = 10,
            int moduleId = 2,
            Board boardId = 0,
            TopicSortType sortby = TopicSortType.New,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop,
            bool getPreviewSources = false,
            bool getPartialReply = false
        );
    }
}
