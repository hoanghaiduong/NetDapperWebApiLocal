
using System.Globalization;
using System.Text.Json;

namespace NetDapperWebApi.Common.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Cho phép đọc lại body của Request
            context.Request.EnableBuffering();
            string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0; // reset lại vị trí stream

            // Log thông tin Request
            _logger.LogInformation("Request Information: Method={Method}, Path={Path}, Headers={Headers}, Body={Body}",
                context.Request.Method,
                context.Request.Path,
                context.Request.Headers,
                requestBody);

            // Thay thế Response.Body bằng MemoryStream để capture dữ liệu response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                // Gọi middleware kế tiếp
                await _next(context);

                // Sau khi middleware khác chạy xong, đọc nội dung Response
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Log thông tin Response
                _logger.LogInformation("Response Information: StatusCode={StatusCode}, Headers={Headers}, Body={Body}",
                    context.Response.StatusCode,
                    context.Response.Headers,
                    responseText);
            }
            catch (Exception ex)
            {
                // Bắt lỗi và ghi log lỗi
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");

                // Có thể tùy chỉnh response lỗi cho client (nếu muốn)
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var errorResponse = new { Message = "Có lỗi xảy ra trên server. Vui lòng thử lại sau." };
                string errorJson = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(errorJson);

                // Ghi log response lỗi
                _logger.LogInformation("Response Error Information: StatusCode={StatusCode}, Body={Body}",
                    context.Response.StatusCode,
                    errorJson);
            }
            finally
            {
                // Sao chép nội dung từ MemoryStream về lại stream gốc
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
        }
    }

}