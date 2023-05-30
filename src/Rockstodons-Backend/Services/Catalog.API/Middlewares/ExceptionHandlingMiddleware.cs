using FluentValidation;
using FluentValidation.Results;
using SendGrid.Helpers.Errors.Model;
using System.Text.Json;

namespace Catalog.API.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);

            var responseModel = new Response<string>()
            {
                Title = "Server Errors",
                HasSuceeded = false,
                StatusCode = statusCode,
                ExceptionMessage = exception.Message,
                ValidationFailureErrors = GetValidationFailureErrors(exception)
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = responseModel.StatusCode;

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(responseModel));
        }

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };

        private static IEnumerable<ValidationFailure> GetValidationFailureErrors(Exception exception)
        {
            IEnumerable<ValidationFailure> validationFailureErrors = null!;

            if (exception is ValidationException validationException)
            {
                validationFailureErrors = validationException.Errors;
            }

            return validationFailureErrors;
        }
    }

    public class Response<T>
    {
        public string Title { get; set; } = default!;

        public bool HasSuceeded { get; set; }

        public int StatusCode { get; set; } 

        public string ExceptionMessage { get; set; } = default!;

        public IEnumerable<ValidationFailure> ValidationFailureErrors { get; init; } = default!;

        public T Data { get; init; } = default!;
    }
}
