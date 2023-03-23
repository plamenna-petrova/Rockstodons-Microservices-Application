using Catalog.API.Infrastructure.ActionResults;
using Catalog.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Catalog.API.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment webHostEnvironment, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            logger.LogError(
                new EventId(exceptionContext.Exception.HResult), 
                exceptionContext.Exception, 
                exceptionContext.Exception.Message
            );

            if (exceptionContext.Exception.GetType() == typeof(CatalogDomainException))
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Instance = exceptionContext.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                validationProblemDetails.Errors.Add(
                    "DomainValidations", 
                    new string[] { exceptionContext.Exception.Message.ToString() }
                );

                exceptionContext.Result = new BadRequestObjectResult(validationProblemDetails);
                exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                var jsonErrorResponse = new JsonErrorResponse
                {
                    Messages = new[] { "An error occurred." }
                };

                if (webHostEnvironment.IsDevelopment())
                {
                    jsonErrorResponse.DeveloperMessage = exceptionContext.Exception;
                }

                exceptionContext.Result = new InternalServerErrorObjectResult(jsonErrorResponse);
                exceptionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        private class JsonErrorResponse
        {
            public string[] Messages { get; set; } = default!;

            public object DeveloperMessage { get; set; } = default!;
        }
    }
}
