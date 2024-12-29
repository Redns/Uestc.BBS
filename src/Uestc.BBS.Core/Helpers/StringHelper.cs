using System.Text.RegularExpressions;

namespace Uestc.BBS.Core.Helpers
{
    public static partial class StringHelper
    {
        #region Markdown

        /// <summary>
        /// https://github.com/stiang/remove-markdown/blob/master/index.js
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string StripMarkdownTags(this string content)
        {
            // Headers
            content = MarkdownHeaderRegex().Replace(content, "\n");
            // Strikethrough
            content = MarkdownStrikethroughRegex().Replace(content, string.Empty);
            // Codeblocks
            content = MarkdownCodeblocksRegex().Replace(content, string.Empty);
            // HTML Tags
            content = MarkdownHtmlRegex().Replace(content, string.Empty);
            // Remove setext-style headers
            content = MarkdownSetextStyleHeadersRegex().Replace(content, string.Empty);
            // Footnotes
            content = MarkdownFootnotesFirstRegex().Replace(content, string.Empty);
            content = MarkdownFootnotesSecondRegex().Replace(content, string.Empty);
            // Images
            content = MarkdownImagesRegex().Replace(content, string.Empty);
            // Links
            content = MarkdownLinksRegex().Replace(content, "$1");
            return content;
        }

        [GeneratedRegex("/\n={2,}/g")]
        private static partial Regex MarkdownHeaderRegex();

        [GeneratedRegex("/~~/g")]
        private static partial Regex MarkdownStrikethroughRegex();

        [GeneratedRegex("/`{3}.*\n/g")]
        private static partial Regex MarkdownCodeblocksRegex();

        [GeneratedRegex("/<[^>]*>/g")]
        private static partial Regex MarkdownHtmlRegex();

        [GeneratedRegex("/^[=\\-]{2,}\\s*$/g")]
        private static partial Regex MarkdownSetextStyleHeadersRegex();

        [GeneratedRegex("/\\[\\^.+?\\](\\: .*?$)?/g")]
        private static partial Regex MarkdownFootnotesFirstRegex();

        [GeneratedRegex("/\\s{0,2}\\[.*?\\]: .*?$/g")]
        private static partial Regex MarkdownFootnotesSecondRegex();

        [GeneratedRegex("/\\!\\[.*?\\][\\[\\(].*?[\\]\\)]/g")]
        private static partial Regex MarkdownImagesRegex();

        [GeneratedRegex("/\\[(.*?)\\][\\[\\(].*?[\\]\\)]/g")]
        private static partial Regex MarkdownLinksRegex();

        #endregion
    }
}
