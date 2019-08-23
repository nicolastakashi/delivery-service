using DeliveryService.Infra.Api.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace DeliveryService.Infra.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services) 
            => services.AddSwaggerGen(
                options =>
                {
                    options.DescribeAllEnumsAsStrings();
                    options.DescribeAllParametersInCamelCase();
                    options.DescribeStringEnumsInCamelCase();
                    options.EnableAnnotations();
                    options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                    options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", Enumerable.Empty<string>() } });
                    options.OperationFilter<ApiVersionFilter>();
                    options.SchemaFilter<SwaggerExcludeFilter>();
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                    {
                        var info = new Info()
                        {
                            Title = "Delivery Service",
                            Description = apiVersionDescription.IsDeprecated ? "This API version has been deprecated." : string.Empty,
                            Version = apiVersionDescription.ApiVersion.ToString(),
                        };
                        options.SwaggerDoc(apiVersionDescription.GroupName, info);
                    }
                });

        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services) =>
           services.AddApiVersioning(
               options =>
               {
                   options.AssumeDefaultVersionWhenUnspecified = true;
                   options.ReportApiVersions = true;
               });

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var audienceConfig = configuration.GetSection("Audience");
            var keyByteArray = Convert.FromBase64String(audienceConfig["Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(keyByteArray)
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => { o.TokenValidationParameters = tokenValidationParameters; });

            return services;
        }

        public static IServiceCollection AddCustomResponseCompression(this IServiceCollection services) =>
            services
                .Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression(
                    options =>
                    {
                        var customMimeTypes = new string[] { "application/vnd.restful+json", "application/problem+json" };

                        options.MimeTypes = customMimeTypes.Concat(new[] { "image/svg+xml" });
                        options.Providers.Add<BrotliCompressionProvider>();
                        options.Providers.Add<GzipCompressionProvider>();
                    });

        public static IServiceCollection AddCustomMvcCore(this IServiceCollection services)
        {
            services.AddMvcCore(x =>
            {
                x.Filters.Add(typeof(ValidateModelStateFilter));
                x.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            }).AddJsonFormatters()
              .AddAuthorization()
              .AddDataAnnotations()
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }
    }
}
