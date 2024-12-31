using System.Net;
using System.Text.Json;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public class TopicService(HttpClient httpClient) : ITopicService
    {
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="boardId">板块 ID</param>
        /// <param name="sortby">排序方式</param>
        /// <param name="topOrder">置顶配置</param>
        /// <param name="getPreviewImages">获取预览图像</param>
        /// <param name="getPartialReply">获取部分回复</param>
        /// <returns></returns>
        public async Task<TopicResp?> GetTopicsAsync(
            uint page = 1,
            uint pageSize = 10,
            Board boardId = Board.Latest,
            TopicSortType sortby = TopicSortType.New,
            TopicTopOrder topOrder = TopicTopOrder.WithoutTop,
            bool getPreviewImages = false,
            bool getPartialReply = false
        )
        {
            using var resp = await _httpClient.PostAsync(
                string.Empty,
                new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { nameof(page), page.ToString() },
                        { nameof(pageSize), pageSize.ToString() },
                        { nameof(boardId), boardId.ToString() },
                        { nameof(sortby), sortby.ToString().ToLower() },
                        { nameof(topOrder), topOrder.ToString() },
                        { "circle", getPartialReply ? "1" : "0" },
                        { "isImageList", getPreviewImages ? "1" : "0" }
                    }
                )
            );

            if (resp.StatusCode is not HttpStatusCode.OK)
            {
                return null;
            }

            return JsonSerializer.Deserialize<TopicResp>(
                await resp.Content.ReadAsStreamAsync(),
                TopicResp.SerializerOptions
            );
        }
    }
}
