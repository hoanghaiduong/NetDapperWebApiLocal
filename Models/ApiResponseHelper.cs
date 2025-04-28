using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDapperWebApi_local.Models
{
    public class ApiResponseHelper
    {

        public static ApiResponse<T> Success<T>(T data, string message = null)
        {
            return new ApiResponse<T>(
                success: true,
                data: data,
                message: message ?? "Request successful.",
                errors: null
            );
        }


        public static ApiResponse<T> Fail<T>(string message, object errors = null)
        {
            return new ApiResponse<T>(
                success: false,
                data: default,
                message: message,
                errors: errors
            );
        }
    }
}