namespace Uestc.BBS.Core.Services.Api.Forum
{
    public interface ITopicService
    {
        Task<TopicResp?> GetTopicsAsync(
            uint page = 1,
            uint pageSize = 10,
            Board boardId = 0,
            TopicSortType sortby = TopicSortType.New,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop,
            bool getPreviewImages = false,
            bool getPartialReply = false
        );
    }
}
