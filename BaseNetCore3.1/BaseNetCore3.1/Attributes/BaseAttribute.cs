using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseNetCore3._1.Attributes
{
    public class BaseAttribute : Attribute, IAsyncActionFilter
    {
        public BaseAttribute()
        {

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            return;
        }
    }
}
