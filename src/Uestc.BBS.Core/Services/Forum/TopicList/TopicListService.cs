using System.Net;
using System.Text.Json;
using Uestc.BBS.Core.Helpers;

namespace Uestc.BBS.Core.Services.Forum.TopicList
{
    public class TopicListService(HttpClient httpClient) : ITopicListService
    {
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="boardId">板块 ID</param>
        /// <param name="moduleId"></param>
        /// <param name="sortby">排序方式</param>
        /// <param name="topOrder">置顶配置</param>
        /// <param name="getPreviewImages">获取预览图像</param>
        /// <param name="getPartialReply">获取部分回复</param>
        /// <returns></returns>
        public async Task<TopicListResp?> GetTopicsAsync(
            string? route = null,
            uint page = 1,
            uint pageSize = 10,
            uint moduleId = 2,
            Board boardId = Board.Latest,
            TopicSortType sortby = TopicSortType.All,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop,
            bool getPreviewImages = false,
            bool getPartialReply = false,
            CancellationToken cancellationToken = default
        )
        {
            using var resp = await _httpClient.PostAsync(
                string.Empty,
                new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "r", string.IsNullOrEmpty(route) ? "forum/topiclist" : route },
                        { nameof(page), page.ToString() },
                        { nameof(pageSize), pageSize.ToString() },
                        { nameof(boardId), boardId.ToInt32String() },
                        { nameof(moduleId), moduleId.ToString() },
                        { nameof(sortby), sortby.ToLowerString() },
                        { nameof(topOrder), topOrder.ToInt32String() },
                        { "circle", getPartialReply ? "1" : "0" },
                        { "isImageList", getPreviewImages ? "1" : "0" },
                    }
                ),
                cancellationToken
            );

            if (resp.StatusCode is not HttpStatusCode.OK)
            {
                return null;
            }

            return JsonSerializer.Deserialize(
                await resp.Content.ReadAsStreamAsync(cancellationToken),
                TopicRespContext.Default.TopicListResp
            );
        }
    }
}
