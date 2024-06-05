using System.Globalization;

namespace Identity.Api.Middlewares
{
    public class RedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/Manage/Index");
                return;
            }
            await _next(context);
        }

    }
    public static class RedirectMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedirectMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RedirectMiddleware>();
        }
    }
}
