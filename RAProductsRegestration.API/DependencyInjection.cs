using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi;
using RAProductsRegestration.API.Authentication;
using RAProductsRegestration.API.Constants;
using RAProductsRegestration.API.ExceptionHandling;
using RAProductsRegestration.API.Filters;
using RAProductsRegestration.Application.Common.Interfaces.Authentication;
//using Microsoft.OpenApi;

namespace RAProductsRegestration.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ResultFilter>();

            services.AddControllers(options =>
            {
                options.Filters.AddService<ResultFilter>();
            });

            services.AddOpenApi();

            services.AddCorss(configuration);
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance =
                        $"{context.HttpContext.Request.Method} " +
                        $"{context.HttpContext.Request.Path}";

                    context.ProblemDetails.Extensions["requestId"] =
                        context.HttpContext.TraceIdentifier;
                };
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();


            services.ConfigureOpenAPI();


            services.AddHttpContextAccessor();

            services.AddScoped<
                ICurrentUser,
                CurrentUser>();
            return services;
        }
        private static IServiceCollection ConfigureOpenAPI(
            this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        document.Components ??= new OpenApiComponents();

                        document.Components.SecuritySchemes =
                            new Dictionary<string, IOpenApiSecurityScheme>
                            {
                                [JwtBearerDefaults.AuthenticationScheme] =
                                    new OpenApiSecurityScheme
                                    {
                                        Type = SecuritySchemeType.Http,
                                        Scheme = "bearer",
                                        BearerFormat = "JWT",
                                        In = ParameterLocation.Header,
                                        Description =
                                            "Enter the JWT access token."
                                    }
                            };

                        return Task.CompletedTask;
                    });
            });
            return services;
        }
        private static IServiceCollection AddCorss(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var frontUrls =
          configuration
              .GetSection("FrontURLs")
              .GetChildren()
              .Select(x => x.Value)
              .Where(x =>
                  !string.IsNullOrWhiteSpace(x))
              .Select(x => x!)
              .ToArray();

            if (frontUrls.Length == 0)
            {
                throw new InvalidOperationException(
                    "No FrontURLs were configured.");
            }

            services.AddCors(
            options =>
            {
                options.AddPolicy(
                    Frontend.FrontendCorsPolicy,
                    policy =>
                    {
                        policy
                            .WithOrigins(
                                frontUrls)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            return services;
        }
    }
}
