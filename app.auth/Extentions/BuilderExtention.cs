using System.Reflection;
using app.auth.Application.DB;
using app.auth.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using app.auth.Application.Utils;
using Microsoft.OpenApi.Models;

namespace app.auth.Extentions
{
    public static class BuilderExtention
    {
        public static void AddArchtectures(this WebApplicationBuilder builder)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            // builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minimal API", Version = "v1" });

                // Configura o esquema de segurança para o Bearer Token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato 'Bearer {token}'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });
        }
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21))));

            #region Added for Services
            var serviceTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "app.auth.Application.Services");

            foreach (var type in serviceTypes)
            {
                builder.Services.AddScoped(type);
            }
            #endregion

        }
        public static void AddSecurity(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();
            builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
        }
    }
}