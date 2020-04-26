using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace LACoreApp.ActionFilters
{
    public abstract class OutputProcessorActionFilterAttribute : ActionFilterAttribute
    {
        protected abstract string Process(string data);

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Body = new OutputProcessorStream(response.Body, Process);
        }
    }

    class OutputProcessorStream : Stream
    {
        private readonly StringBuilder _data = new StringBuilder();

        private readonly Stream _stream;
        private readonly Func<string, string> _processor;

        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public OutputProcessorStream(Stream stream, Func<string, string> processor)
        {
            _stream = stream;
            _processor = processor;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var htmlString = Encoding.UTF8.GetString(buffer, offset, count);
            htmlString = UpdateAmpImages(htmlString);
            htmlString = ReplaceWithLink("script", htmlString);
            htmlString = ReplaceWithLink("iframe", htmlString);
            _data.Append(htmlString);
        }

        public override void Close()
        {
            var output = Encoding.UTF8.GetBytes(_processor(_data.ToString()));
            _stream.Write(output, 0, output.Length);
            _stream.Flush();
            _data.Clear();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
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
}
