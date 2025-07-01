using System;
using App.Common.Constants;

namespace App.Common.Models
{
    public class AppResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int? StatusCode { get; set; }

        public static AppResponseDto Response(bool isSuccess, string message, HttpStatusCodes statusCode = HttpStatusCodes.OK)
        {
            return new AppResponseDto
            {
                IsSuccess = isSuccess,
                Message = message,
                StatusCode = Convert.ToInt32(statusCode)
            };
        }

        public static AppResponseDto<TModel> Fail<TModel>(TModel data, string message = "", HttpStatusCodes statusCode = HttpStatusCodes.InternalServerError)
        {
            return new AppResponseDto<TModel>
            {
                Data = data,
                IsSuccess = false,
                Message = message,
                StatusCode = Convert.ToInt32(statusCode)
            };
        }

        public static AppResponseDto<TModel> Success<TModel>(TModel data, string message = "", HttpStatusCodes statusCode = HttpStatusCodes.OK)
        {
            return new AppResponseDto<TModel>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                StatusCode = Convert.ToInt32(statusCode)
            };
        }
    }

    public sealed class AppResponseDto<TModel>
    {
        public TModel Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int? StatusCode { get; set; }
    }
}
