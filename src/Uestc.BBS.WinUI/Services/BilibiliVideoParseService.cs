using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Uestc.BBS.Core.Services.VideoParse;

namespace Uestc.BBS.WinUI.Services
{
    public class BilibiliVideoParseService(HttpClient httpClient) : IVideoParseService
    {
        public Task<Uri> GetSourceUriAsync(string url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输入 BV 号，返回可直接播放的 DASH 视频/音频直链
        /// </summary>
        public async Task<BiliPlayResponse> GetDashUrlsAsync(string bvid, int page = 1)
        {
            // 1. 取 cid
            var cid = await GetCidAsync(bvid, page);
            if (cid <= 0)
                return new() { Code = 1, Message = "cid not found" };

            // 2. 调 playurl
            var url =
                $"https://api.bilibili.com/x/player/playurl"
                + $"?bvid={bvid}&cid={cid}&fnval=4048&fourk=1";
            using var resp = await httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            var root = doc.RootElement.GetProperty("data");

            // 3. 取最高清晰度
            var video = root.GetProperty("dash")
                .GetProperty("video")
                .EnumerateArray()
                .OrderByDescending(v => v.GetProperty("id").GetInt32())
                .First();
            var audio = root.GetProperty("dash")
                .GetProperty("audio")
                .EnumerateArray()
                .OrderByDescending(a => a.GetProperty("id").GetInt32())
                .First();

            return new()
            {
                Code = 0,
                Quality = video.GetProperty("id").GetInt32(),
                AcceptQuality = root.GetProperty("accept_quality").Deserialize<List<int>>(),
                Video = video.GetProperty("baseUrl").GetString(),
                Audio = audio.GetProperty("baseUrl").GetString(),
            };
        }

        private async Task<int> GetCidAsync(string bvid, int page)
        {
            var url = $"https://api.bilibili.com/x/web-interface/view?bvid={bvid}";
            using var resp = await httpClient.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
                return 0;
            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            var pages = doc
                .RootElement.GetProperty("data")
                .GetProperty("pages")
                .EnumerateArray()
                .ToList();
            if (pages.Count < page)
                return 0;
            return pages[page - 1].GetProperty("cid").GetInt32();
        }
    }

    public class BiliPlayResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public int Quality { get; set; }
        public List<int> AcceptQuality { get; set; }
        public string Video { get; set; }
        public string Audio { get; set; }
    }
}
