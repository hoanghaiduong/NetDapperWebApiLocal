
using NetDapperWebApi_local.Common.Middlewares;

namespace NetDapperWebApi_local.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(
            this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandlingMiddleware>();
            builder.UseMiddleware<LoggingMiddleware>();
            return builder;
        }
    }
}