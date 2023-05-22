using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Catalog.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Rockstodons Catalog API",
                    Description = "Rockstodons Core API",
                    Contact = new OpenApiContact
                    {
                        Name = "Plamenna Petrova",
                        Email = "plamennavp@abv.bg",
                        Url = new Uri("https://github.com/plamenna-petrova")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - " +
                    "Bearer {your token here} to access this API"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        }, new List<string>()
                    }
                });
            });
        }

        public static void AddControllersExtension(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DictionaryKeyPolicy =
                        JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase;
                });
        }

        public static void AddCorsExtension(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }

        public static void AddJWTAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = configuration["STS:ServerUrl"];
                    options.Audience = "app.api.userprofile";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["STS:ServerUrl"],
                        ValidAudience = "app.api.userprofile"
                    };
                });
        }

        public static void AddAuthorizationPolicies(
            this IServiceCollection services, IConfiguration configuration)
        {
            string admin = configuration["APIRoles:AdminRole"];
            string editor = configuration["APIRoles:EditorRole"];
            string user = configuration["APIRoles:UserRole"];

            services.AddAuthorization(options =>
            {
                options.AddPolicy
                (
                    AuthorizationConstants.AdminPolicy,
                    policy => policy.RequireAssertion(context => 
                        HasRole(context.User, admin)
                    )
                );
                options.AddPolicy(
                    AuthorizationConstants.EditorPolicy,
                    policy => policy.RequireAssertion(context => 
                        HasRole(context.User, editor) || 
                        HasRole(context.User, admin)
                    )
                );
                options.AddPolicy(
                    AuthorizationConstants.UserPolicy, 
                    policy => policy.RequireAssertion(context => 
                        HasRole(context.User, user) ||
                        HasRole(context.User, editor) ||
                        HasRole(context.User, admin)
                    )
                );
            });
        }

        public static bool HasRole(ClaimsPrincipal claimsPrincipal, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return false;
            }

            return claimsPrincipal.HasClaim(
                c => (c.Type == JwtClaimTypes.Role || 
                c.Type == $"client_{JwtClaimTypes.Role}") &&
                Array.Exists(c.Value.Split(','), e => e == role)
            );
        }
    }
}
