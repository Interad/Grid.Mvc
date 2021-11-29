using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace GridMvc.Core.Utility
{
    internal static class HtmlContentHelper
    {
        public static string ToHtmlString(this IHtmlContent content)
        {
            if (content is HtmlString htmlString)
            {
                return htmlString.Value;
            }

            using (var writer = new StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
