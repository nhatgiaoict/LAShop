using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace LACoreApp.ActionFilters
{
    public class UseAmpImageAttribute : ActionFilterAttribute
    {
        private MemoryStream originalFilter;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            originalFilter = (MemoryStream)filterContext.HttpContext.Response.Body;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Body = new MemoryStream(ModifyStream(originalFilter));
            base.OnResultExecuted(filterContext);
        }
        public byte[] ModifyStream(MemoryStream stream)
        {
            var buffer = stream.GetBuffer();
            var html = Encoding.UTF8.GetString(buffer);
            html = UpdateAmpImages(html);
            html = ReplaceWithLink("script", html);
            html = ReplaceWithLink("iframe", html);
            return Encoding.UTF8.GetBytes(html);
        }
        private string UpdateAmpImages(string response)
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

        private string ReplaceWithLink(string tag, string response)
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

        private HtmlDocument GetHtmlDocument(string htmlContent)
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
    public class HttpStream : MemoryStream
    {
        private readonly MemoryStream _responseStream;

        public HttpStream(MemoryStream stream)
        {
            _responseStream = stream;
            //ModifyStream(stream);
        }

        //public void Run(MemoryStream stream)
        //{
        //    Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
        //}
        //public void ModifyStream(MemoryStream stream)
        //{
        //    var buffer = stream.GetBuffer();
        //    var html = Encoding.UTF8.GetString(buffer);
        //    html = UpdateAmpImages(html);
        //    html = ReplaceWithLink("script", html);
        //    html = ReplaceWithLink("iframe", html);
        //    buffer = Encoding.UTF8.GetBytes(html);
        //    _responseStream.Write(buffer, 0, buffer.Length);
        //}

        //public override void Write(byte[] buffer, int offset, int count)
        //{
        //    var html = Encoding.UTF8.GetString(buffer);
        //    html = UpdateAmpImages(html);
        //    html = ReplaceWithLink("script", html);
        //    html = ReplaceWithLink("iframe", html);
        //    buffer = Encoding.UTF8.GetBytes(html);
        //    _responseStream.Write(buffer, offset, count);
        //}

        
    }
}