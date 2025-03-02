using System.Net;
using System.Text.Json;
using Uestc.BBS.Core.Models;

namespace Uestc.BBS.Core.Services.Forum
{
    public class TopicService(HttpClient httpClient, AuthCredential credential) : ITopicService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="authorId"></param>
        /// <param name="orderDesc"> </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TopicResp?> GetTopicAsync(
            uint topicId,
            uint? authorId = null,
            bool reverseReplyOrder = false,
            CancellationToken cancellationToken = default
        )
        {
            using var resp = await httpClient.PostAsync(
                string.Empty,
                new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        { "accessToken", credential.Token },
                        { "accessSecret", credential.Secret },
                        { nameof(topicId), topicId.ToString() },
                        { nameof(authorId), authorId?.ToString() ?? string.Empty },
                        { "order", reverseReplyOrder.ToString() },
                    }
                ),
                cancellationToken
            );

            return resp.StatusCode is HttpStatusCode.OK
                ? JsonSerializer.Deserialize(
                    await resp.Content.ReadAsStreamAsync(cancellationToken),
                    TopicRespContext.Default.TopicResp
                )
                : null;
        }
    }
}
