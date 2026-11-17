using FluentValidation;
using Kentimce.Application.Responses;
using System.Net;
using System.Text.Json;

namespace Kentimce.WebApi.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new BaseResponse<string>(); 

                switch (error)
                {
                    case ValidationException e:
                        // Validation hatası ise 400 dön
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        // Hataları listele
                        var errors = e.Errors.Select(x => x.ErrorMessage).ToList();
                        responseModel.Fail(string.Join(", ", errors));
                        break;

                    default:
                        // Beklenmedik hata ise 500 dön
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Fail("Beklenmedik bir hata oluştu: " + error.Message);
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}