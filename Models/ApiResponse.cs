namespace NetDapperWebApi_local.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }  
        public ApiResponse(bool success, T data = default, string message = null, object errors = null)
        {
            Success = success;
            Data = data;
            Message = message;
             Errors = errors;
        }
    }
}
