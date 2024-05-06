using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Chatapp.Core.Web.Extensions
{
    public static class HttpContextExtension
    {
        public static async ValueTask<string> ReadRequestBodyAsync(this HttpContext httpContext)
        {
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, false, 1024, true);

            var result = await reader.ReadToEndAsync();

            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            return result;
        }

        public static async ValueTask<string> ReadResponseBodyAsync(this HttpContext httpContext)
        {
            if (!httpContext.Response.Body.CanRead && !httpContext.Response.Body.CanSeek)
            {
                return string.Empty;
            }

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(httpContext.Response.Body, Encoding.UTF8, false, 1024, true);

            var result = await reader.ReadToEndAsync();

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            return result;
        }
    }
}
