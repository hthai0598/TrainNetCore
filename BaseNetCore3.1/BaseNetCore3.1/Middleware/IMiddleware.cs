using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace BaseNetCore3._1.Middleware
{
    public interface IMiddleware
    {
        void RegisterMiddleware(IApplicationBuilder app, IWebHostEnvironment env);
    }
}
