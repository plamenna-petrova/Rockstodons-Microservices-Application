using Catalog.API.Data.Data.Common;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Infrastructure;
using Catalog.API.Infrastructure.Repositories;
using Catalog.API.Services.Mapping;
using Catalog.API.Services.Data.Implementation;
using Catalog.API.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Catalog.API.Infrastructure.Seeding;
using Catalog.API.Services.Messaging;
using Catalog.API.Extensions;
using Newtonsoft.Json;
using MediatR;
using Catalog.API.Application.Behaviors;
using FluentValidation;
using Catalog.API.Middlewares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        app.UseSwagger();

        app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            using var serviceScope = app.Services.CreateScope();
            var catalogDbContext = serviceScope.ServiceProvider.GetRequiredService<CatalogDbContext>();

            if (catalogDbContext.Database.GetPendingMigrations().Any())
            {
                catalogDbContext.Database.Migrate();
            }

            new CatalogDbContextSeeder()
                .SeedAsync(catalogDbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
        }

        AutomapperConfig.RegisterMappings(Assembly.GetExecutingAssembly());

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwaggerExtension();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); 
        });

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(
            options => options.UseLazyLoadingProxies()
               .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddAuthorization();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(configuration);

        services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(BaseDeletableEntityRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IDbQueryRunner, DbQueryRunner>();

        services.AddTransient<IPerformersService, PerformersService>();
        services.AddTransient<IAlbumsService, AlbumsService>();
        services.AddTransient<ITracksService, TracksService>();
        services.AddTransient<IStreamsService, StreamsService>();
        services.AddTransient<ICommentsService, CommentsService>();
        services.AddTransient<ISubcommentsService, SubcommentsService>();
        services.AddTransient<IFileStorageService, FileStorageService>();
        services.AddTransient<IEmailSender, SendGridEmailSender>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Program).GetTypeInfo().Assembly));
        AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly)
            .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CustomValidationBehavior<,>));

        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddSwaggerExtension();

        services.AddMvc().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        services.AddControllersExtension();
        services.AddCorsExtension();
        services.AddJWTAuthentication(configuration);
        services.AddAuthorizationPolicies(configuration);
        services.AddMvcCore().AddApiExplorer();
    }
}