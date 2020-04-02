using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;
using LACoreApp.Utilities.Stream;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;

namespace LACoreApp.ActionFilters
{
    public class UseAmpImageAttribute : ActionFilterAttribute
    {
        private HtmlTextWriter _htmlTextWriter;
        private StringWriter _stringWriter;
        private StringBuilder _stringBuilder;
        private MemoryStream _output;
        private string newContent ;
        private MemoryStream originalFilter;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //_stringWriter = new StringWriter(_stringBuilder);
            //_htmlTextWriter = new HtmlTextWriter(_stringWriter);
            //_output = (MemoryStream)filterContext.HttpContext.Response.Body;
            //filterContext.HttpContext.Response.Body = _htmlTextWriter;


            
            //_output = new MemoryStream();
            //filterContext.HttpContext.Response.Body = _output;

            //_output.Seek(0, SeekOrigin.Begin);
            //newContent = new StreamReader(_output).ReadToEnd();
            //newContent += "andkjsnjfandsjknfakjndsjnfjas";
            //filterContext.HttpContext.Response.WriteAsync(newContent);

            originalFilter = (MemoryStream)filterContext.HttpContext.Response.Body;
            

            //newBody.Seek(0, SeekOrigin.Begin);

            //using (var newBody = new MemoryStream())
            //{
            //    // We set the response body to our stream so we can read after the chain of middlewares have been called.
            //    filterContext.HttpContext.Response.Body = newBody;

            //    // Reset the body so nothing from the latter middlewares goes to the output.
            //    filterContext.HttpContext.Response.Body = new MemoryStream();


            //    newBody.Seek(0, SeekOrigin.Begin);

            //    // newContent will be `Hello`.
            //    newContent = new StreamReader(newBody).ReadToEnd();

            //    newContent += ", World!";

            //    // Send our modified content to the response body.

            //    filterContext.HttpContext.Response.WriteAsync(newContent);
            //}
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Body = new HttpStream(originalFilter);
            //var response = _stringBuilder.ToString();
            //_output.Seek(0, SeekOrigin.Begin);
            //newContent = new StreamReader(_output).ReadToEnd();
            //var by = Encoding.ASCII.GetBytes(newContent);
            //_output.WriteAsync(by);
            //filterContext.HttpContext.Response.WriteAsync(newContent);
            //response = UpdateAmpImages(response);
            //_output.Write(response);
        }
        
    }
}
