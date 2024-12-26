using System.Net;
using System.Text.Json;

namespace Uestc.BBS.Core.Services.Api.Forum
{
    public class TopicService(HttpClient httpClient) : ITopicService
    {
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="boardId"></param>
        /// <param name="sortby"></param>
        /// <param name="topOrder"></param>
        /// <returns></returns>
        public async Task<TopicResp?> GetTopicsAsync(int page = 1, int pageSize = 10, int boardId = 0, TopicSortType sortby = TopicSortType.New, TopicTopOrder topOrder = TopicTopOrder.WithoutTop)
        {
            using var resp = await _httpClient.PostAsync(string.Empty, new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { nameof(page), page.ToString() },
                { nameof(pageSize), pageSize.ToString() },
                { nameof(boardId), boardId.ToString() },
                { nameof(sortby), sortby.ToString().ToLower() },
                { nameof(topOrder), topOrder.ToString() },
            }));
            
            if (resp.StatusCode is not HttpStatusCode.OK)
            {
                return null;
            }

            var s = await resp.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TopicResp>(await resp.Content.ReadAsStreamAsync(), TopicResp.SerializerOptions);
        }
    }
}
