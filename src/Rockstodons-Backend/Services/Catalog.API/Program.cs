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

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        app.UseSwagger();

        app.UseSwaggerUI();

        // Configure the HTTP request pipeline.
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

        app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
        );

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

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

        services.AddAuthentication(authenticationOptions =>
        {
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtBearerOptions =>
        {
            jwtBearerOptions.RequireHttpsMetadata = true;
            jwtBearerOptions.SaveToken = true;
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JWTConfiguration:Issuer"],
                ValidAudience = configuration["JWTConfiguration:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(configuration["JWTConfiguration:Secret"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });

        services.AddAuthorization();

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
        services.AddTransient<IAlbumsService, AlbumsService>();
        services.AddTransient<ITracksService, TracksService>();
        services.AddTransient<IRolesService, RolesService>();
        services.AddSingleton<IIdentityService, IdentityService>();
        services.AddTransient<IUsersService, UsersService>();
        services.AddTransient<IEmailSender, SendGridEmailSender>();
        services.AddTransient<IFileStorageService, FileStorageService>();

        services.AddHostedService<JWTRefreshTokenCache>();

        services.Configure<DataProtectionTokenProviderOptions>(
            dataProtectionTokenProviderOptions => 
            dataProtectionTokenProviderOptions.TokenLifespan = TimeSpan.FromHours(3));

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(swaggerGenerationOptions =>
        {
            swaggerGenerationOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Rockstodons Catalog API",
                Version = "v1"
            });
            swaggerGenerationOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer schemes"
            });
            swaggerGenerationOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
        });

        services.AddCors(corsOptions =>
        {
            corsOptions.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
    }
}