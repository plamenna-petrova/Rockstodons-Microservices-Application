using Catalog.API.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Catalog.API.Middlewares
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

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                var responseModel = new Response<string>()
                {
                    Suceeded = false,
                    Message = exception.Message
                };

                switch (exception)
                {
                    case CatalogAPIException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException validationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = validationException.Errors.Select(err => err.ErrorMessage).ToList();
                        break;
                    case KeyNotFoundException keyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                _logger.LogError(exception.Message);

                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }

    public class Response<T>
    {
        public Response()
        {

        }

        public Response(T data, string message = null)
        {
            Suceeded = true;
            Message = message;
            Data = data;
        }

        public Response(string message)
        {
            Suceeded = false;
            Message = message;
        }

        public bool Suceeded { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public T Data { get; set; }
    }
}
