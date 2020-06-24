using LACoreApp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACoreApp.Extensions
{
    public static class ModifyHtmlMiddlewareExtension
    {
        public static IApplicationBuilder UseModifyHtmlMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ModifyHtmlMiddleware>();
        }
    }
}
