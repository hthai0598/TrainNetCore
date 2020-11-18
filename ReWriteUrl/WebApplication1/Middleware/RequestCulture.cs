using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Cách tạo mới 1 middleware
/// </summary>
namespace RewriteRouter.Middleware
{
    //B1
    //B2 => folder Application Builder
    public class RequestCulture
    {
        private readonly RequestDelegate _next;
        public RequestCulture(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            return this._next(context);
        }
    }

    public class RequestRewrite
    {
        private readonly RequestDelegate _next;
        public RequestRewrite(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke( HttpContext context) 
        {
            var url = context.Request.Path.Value;

            // Rewrite privacy URL to index
            if (url.Contains("/photo"))
            {
                // rewrite to index
                context.Request.Path = "/Gallery";
            }
    

            return this._next(context);
        }
    }


}
