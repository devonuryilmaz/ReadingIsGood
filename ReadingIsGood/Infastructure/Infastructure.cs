using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ReadingIsGood.Data.Base;
using ReadingIsGood.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReadingIsGood.Core.Mapping;
using ReadingIsGood.Business.Configurations;

namespace ReadingIsGood.API.Infastructure
{
    public static class Infastructure
    {

        public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddDependencies()
                .AddDbConnections(config)
                .AddJwtOptions(config)
                .AddSwaggerGen()
                .AddMappingConfig()
                .AddLogging()
                .AddSignalR();

            return services;
        }

        public static IApplicationBuilder InstallServicesApp(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();
            builder.UseMiddleware<AuthMiddleware>();

            return builder;
        }
        private static IServiceCollection AddDbConnections(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

        private static IServiceCollection AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ReadingIsGood_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }

        private static IServiceCollection AddJwtOptions(this IServiceCollection services, IConfiguration _configuration)
        {
            services.Configure<CustomTokenOptions>(_configuration.GetSection("TokenOptions"));

            var tokenOptions = _configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();

            var key = Encoding.ASCII.GetBytes(tokenOptions.SecurityKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "readingisgood",
                    ValidAudience = "readingisgood",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }

        private static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            //services.AddScoped<IRolService, RolService>(); 
            services.AddHttpContextAccessor();
            services.AddScoped<ILogger, Logger>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            return services;
        }

        private static IServiceCollection AddMappingConfig(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
