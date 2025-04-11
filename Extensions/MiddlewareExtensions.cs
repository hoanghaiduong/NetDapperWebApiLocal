
using NetDapperWebApi.Common.Middlewares;

namespace NetDapperWebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(
            this IApplicationBuilder builder)
        {
            // builder.UseMiddleware<ErrorHandlingMiddleware>();
            builder.UseMiddleware<LoggingMiddleware>();
            return builder;
        }
    }
}