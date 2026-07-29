using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationSeconds;

        public RedisCacheAttribute(int durationSeconds)
        {
            _durationSeconds = durationSeconds;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = CreateKey(context.HttpContext.Request);
            var cached = await cacheService.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached)) 
            {
                context.Result = new ContentResult()
                {
                    Content = cached,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
           var execute = await next.Invoke();
            if (execute.Result is OkObjectResult { Value: not null } okData) 
            {
                await cacheService.SetAsync(cacheKey, okData.Value, TimeSpan.FromSeconds(_durationSeconds));
            }

        }

        private static string CreateKey(HttpRequest httpRequest)
        {
            var key = new StringBuilder();
            key.Append(httpRequest.Path);
            key.Append('?');
            foreach(var (k, v) in httpRequest.Query.OrderBy(q => q.Key))
            {
                key.Append(k);
                key.Append('=');
                key.Append(v);
                key.Append('&');
            }
            return key.ToString();
            
        }
    }
}
