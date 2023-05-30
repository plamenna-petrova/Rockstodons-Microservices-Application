using Catalog.API.Middlewares;

namespace Catalog.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rockstdons.CatalogAPI");
            });
        }

        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
