using System.Collections.Generic;
using Microsoft.UI.Xaml.Documents;

namespace Uestc.BBS.WinUI.Helpers
{
    public static class RichTextBlockHelper
    {
        public static Paragraph AddRange(this Paragraph paragraph, IEnumerable<Inline> inlines)
        {
            foreach (var inline in inlines)
            {
                paragraph.Inlines.Add(inline);
            }
            return paragraph;
        }
    }
}
