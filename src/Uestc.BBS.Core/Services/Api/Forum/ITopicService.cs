namespace Uestc.BBS.Core.Services.Api.Forum
{
    public interface ITopicService
    {
        Task<TopicResp?> GetTopicsAsync(
            string? route = null,
            uint page = 1,
            uint pageSize = 10,
            uint moduleId = 2,
            Board boardId = 0,
            TopicSortType sortby = TopicSortType.New,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop,
            bool getPreviewSources = false,
            bool getPartialReply = false,
            CancellationToken cancellationToken = default
        );
    }
}
