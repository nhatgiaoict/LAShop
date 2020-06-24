using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LACoreApp.Utilities.Stream
{
    public static class ProcessRequest
    {

        public static string ModifyRequest(HttpContext context)
        {
            var buffer = ((MemoryStream)context.Response.Body).GetBuffer();
            var html = Encoding.UTF8.GetString(buffer);
            html = UpdateAmpImages(html);
            html = ReplaceWithLink("script", html);
            html = ReplaceWithLink("iframe", html);
            return html;
        }
        private static string UpdateAmpImages(string response)
        {
            // Use HtmlAgilityPack (install-package HtmlAgilityPack)
            var doc = GetHtmlDocument(response);
            var imageList = doc.DocumentNode.Descendants("img");
            const string ampImage = "amp-img";
            var htmlNodes = imageList as HtmlNode[] ?? imageList.ToArray();
            if (!htmlNodes.Any()) return response;
            if (!HtmlNode.ElementsFlags.ContainsKey(ampImage))
            {
                HtmlNode.ElementsFlags.Add(ampImage, HtmlElementFlag.Closed);
            }
            foreach (var imgTag in htmlNodes)
            {
                var original = imgTag.OuterHtml;
                var replacement = imgTag.Clone();
                replacement.Name = ampImage;
                response = response.Replace(original, replacement.OuterHtml);
            }
            return response;
        }

        private static string ReplaceWithLink(string tag, string response)
        {
            var doc = GetHtmlDocument(response);
            var elements = doc.DocumentNode.Descendants(tag);
            foreach (var htmlNode in elements)
            {
                if (htmlNode.Attributes["data-link"] == null) continue;

                var dataLink = htmlNode.Attributes["data-link"].Value;
                var paragraph = doc.CreateElement("p");

                var text = $"[Embedded Link] {dataLink}";

                var anchor = doc.CreateElement("a");
                anchor.InnerHtml = text;
                anchor.Attributes.Add("href", dataLink);
                anchor.Attributes.Add("title", text);
                paragraph.InnerHtml = anchor.OuterHtml;

                var original = htmlNode.OuterHtml;
                var replacement = paragraph.OuterHtml;

                response = response.Replace(original, replacement);
            }

            return response;
        }

        private static HtmlDocument GetHtmlDocument(string htmlContent)
        {
            var doc = new HtmlDocument
            {
                OptionOutputAsXml = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            doc.LoadHtml(htmlContent);
            return doc;
        }
    }
}