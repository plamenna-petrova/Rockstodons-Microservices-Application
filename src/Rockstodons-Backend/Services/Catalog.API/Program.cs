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
using Catalog.API.Data.Data.Models;
using Catalog.API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Catalog.API.Services.Messaging;
using Catalog.API.Extensions;

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

            using (var serviceScope = app.Services.CreateScope())
            {
                var catalogDbContext = serviceScope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                if (catalogDbContext.Database.GetPendingMigrations().Any())
                {
                    catalogDbContext.Database.Migrate();
                }

                new CatalogDbContextSeeder()
                    .SeedAsync(catalogDbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }
        }

        AutomapperConfig.RegisterMappings(Assembly.GetExecutingAssembly());

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwaggerExtension();
        app.UseErrorHandlingMiddleware();

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
               .UseSqlServer("Server=LENOVOLEGION\\SQLEXPRESS;Initial Catalog=Rockstodons.CatalogDb;Integrated Security=true"));

        services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
            .AddRoles<ApplicationRole>().AddEntityFrameworkStores<CatalogDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorization();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(configuration);

        services.AddAutoMapper(typeof(Program));

        services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(BaseDeletableEntityRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IDbQueryRunner, DbQueryRunner>();

        services.AddTransient<IGenresService, GenresService>();
        services.AddTransient<IAlbumTypesService, AlbumTypesService>();
        services.AddTransient<IPerformersService, PerformersService>();
        services.AddTransient<IAlbumsService, AlbumsService>();
        services.AddTransient<ITracksService, TracksService>();
        services.AddTransient<IEmailSender, SendGridEmailSender>();
        services.AddTransient<IFileStorageService, FileStorageService>();

        services.AddSwaggerExtension();
        services.AddControllersExtension();
        services.AddCorsExtension();
        services.AddJWTAuthentication(configuration);
        services.AddAuthorizationPolicies(configuration);
        services.AddMvcCore().AddApiExplorer();
    }
}