using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Middleware;
using Microsoft.AspNetCore.Builder;

namespace LACoreApp.Extensions
{
    public static class AmpImageMiddlewareExtensions
    {
        public static IApplicationBuilder UseAmpImageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AmpImageMiddleware>();
        }
    }
}
