using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetDapperWebApi.Common.Middlewares
{
   public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        int statusCode;
        string title;
        object errors;
        
        // Nếu là ValidationException thì trả về 400 với dữ liệu lỗi chi tiết
        if(ex is ValidationException validationEx)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            title = "One or more validation errors occurred.";
            
            // Giả sử ValidationException chứa thông tin lỗi dưới dạng dictionary
            // Nếu không, bạn có thể tự định dạng lại cho phù hợp
            errors = new Dictionary<string, string[]>
            {
                { "ValidationError", new [] { validationEx.Message } }
            };
        }
        else
        {
            // Xử lý lỗi chung (500 Internal Server Error)
            statusCode = (int)HttpStatusCode.InternalServerError;
            title = "An unexpected error occurred.";
            errors = new Dictionary<string, string[]>
            {
                { "Error", new [] { ex.Message } }
            };
        }
        
        var errorResponse = new
        {
            title,
            status = statusCode,
            errors
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        string jsonResponse = JsonSerializer.Serialize(errorResponse, options);
        
        return context.Response.WriteAsync(jsonResponse);
    }
}
}