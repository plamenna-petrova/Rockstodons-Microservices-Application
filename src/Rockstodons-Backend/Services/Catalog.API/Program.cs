
using Catalog.API;
using Catalog.API.Data.Data.Common;
using Catalog.API.Data.Data.Common.Repositories;
using Catalog.API.Infrastructure;
using Catalog.API.Infrastructure.Repositories;
using Catalog.API.Services.Mapping;
using Catalog.API.Services.Services.Data.Implementation;
using Catalog.API.Services.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerGen();

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // local database

            //services.AddDbContext<CatalogDbContext>(options =>
            //    options.UseSqlServer("Server=LENOVOLEGION\\SQLEXPRESS;Initial Catalog=Rockstodons.CatalogDb;Integrated Security=true"));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(configuration);

            services.AddAutoMapper(typeof(Program));

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(BaseDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            services.AddTransient<IGenresService, GenresService>();
            services.AddTransient<IAlbumTypesService, AlbumTypesService>();
            services.AddTransient<IPerformersService, PerformersService>();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var catalogContext = serviceScope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                if (catalogContext.Database.GetPendingMigrations().Any())
                {
                    catalogContext.Database.Migrate();
                }

                var webHostEnvironment = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();
                var settings = serviceScope.ServiceProvider.GetService<IOptions<CatalogSettings>>();
                var logger = serviceScope.ServiceProvider.GetService<ILogger<CatalogContextSeeder>>();

                new CatalogContextSeeder().SeedAsync(catalogContext, webHostEnvironment, settings, logger).GetAwaiter().GetResult();
            }
        }

        AutomapperConfig.RegisterMappings(Assembly.GetExecutingAssembly());

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}