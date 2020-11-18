using Microsoft.AspNetCore.Builder;
using RewriteRouter.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RewriteRouter.AplicationBuilder
{
    /// <summary>
    /// đăng kí middleware
    /// </summary>
    ///B2
    ///B3 => trong startup  app.UseRequestCulture();
    public static class AplicationBuilder
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCulture>();
        }

        public static IApplicationBuilder UseRequestRewrite(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestRewrite>();

        }
    }


}
