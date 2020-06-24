using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACoreApp.Middleware
{
    public class ModifyHtmlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ModifyHtmlMiddleware> _logger;
        public ModifyHtmlMiddleware(RequestDelegate next, ILogger<ModifyHtmlMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.ToString().ToLower().Contains("/amp/"))
            {
                _logger.LogInformation($"Modifying html tag");
                var newContent = string.Empty;
                var body = context.Response.Body;
                using(var newBody = new MemoryStream())
                {
                    context.Response.Body = newBody;
                    await _next(context);
                    context.Response.Body = body;
                    newBody.Seek(0, SeekOrigin.Begin);
                    var contentEncoding = context.Response.Headers["content-encoding"];
                    if (contentEncoding.Equals("gzip"))
                    {
                        // using gzip stream reader
                        using (var reader = new StreamReader(new GZipStream(newBody, CompressionMode.Decompress)))
                        {
                            newContent = reader.ReadToEnd();
                            var html = UpdateAmpImages(newContent);
                            html = ReplaceWithLink("script", html);
                            html = ReplaceWithLink("iframe", html);
                            await context.Response.WriteAsync(html);
                        }
                    }
                    else if (contentEncoding.Equals("deflate"))
                    {
                        using (var reader = new StreamReader(new DeflateStream(newBody, CompressionMode.Decompress)))
                        {
                            newContent = reader.ReadToEnd();
                            var html = UpdateAmpImages(newContent);
                            html = ReplaceWithLink("script", html);
                            html = ReplaceWithLink("iframe", html);
                            await context.Response.WriteAsync(html);
                        }
                    }
                    else
                    {
                        using (var reader = new StreamReader(newBody, Encoding.UTF8))
                        {
                            newContent = reader.ReadToEnd();
                            var html = UpdateAmpImages(newContent);
                            html = ReplaceWithLink("script", html);
                            html = ReplaceWithLink("iframe", html);
                            await context.Response.WriteAsync(html);
                        }
                    }
                    
                }
            }
            else
            {
                await _next(context);
            }

        }
        public static Stream GenerateStreamFromString(string value)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
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
