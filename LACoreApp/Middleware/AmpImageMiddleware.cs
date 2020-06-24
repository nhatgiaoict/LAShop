using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;

namespace LACoreApp.Middleware
{
    public class AmpImageMiddleware
    {

        private readonly RequestDelegate _next;

        public AmpImageMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            
            await _next(context);
        }

        
    }
}
