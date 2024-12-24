using HtmlAgilityPack;

namespace Uestc.BBS.Core.Services
{
    public class DailySentenceService(HttpClient httpClient) : IDailySentenceService
    {
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// 获取每日一句
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDailySentenceAsync()
        {
            var content = await _httpClient.GetStringAsync(string.Empty);
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            var dom = new HtmlDocument();
            dom.LoadHtml(content);

            return dom
                .DocumentNode.SelectSingleNode("//div[@class='vanfon_geyan']")
                .SelectSingleNode(".//span")
                .InnerText.Trim();
        }
    }
}
