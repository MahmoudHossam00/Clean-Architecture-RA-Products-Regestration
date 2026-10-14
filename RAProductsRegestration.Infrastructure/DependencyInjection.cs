using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RAProductsRegestration.Application.Common.Interfaces.Files;
using RAProductsRegestration.Application.Common.Interfaces.Integrations.CTD;
using RAProductsRegestration.Application.Common.Interfaces.Integrations.JDE.Suppliers;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Application.Common.Interfaces.Queries;
using RAProductsRegestration.Application.Features.Features.Appeals;
using RAProductsRegestration.Application.Features.Features.Variations;
using RAProductsRegestration.Infrastructure.Data;
using RAProductsRegestration.Infrastructure.Data.Interceptors;
using RAProductsRegestration.Infrastructure.Data.Queries;
using RAProductsRegestration.Infrastructure.Data.Repositories;
using RAProductsRegestration.Infrastructure.Files;
//using Microsoft.Extensions.Options.ConfigurationExtensions;


//using RAProductsRegestration.Infrastructure.Data;
using RAProductsRegestration.Infrastructure.Integrations.CTD;
using RAProductsRegestration.Infrastructure.Integrations.JDE.Suppliers;
using RAProductsRegestration.Infrastructure.Repositories;
using System.Configuration;
//using RAProductsRegestration.Infrastructure.Repositories;

namespace RAProductsRegestration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration).AddDatabase(configuration).AddCTDIntegration(configuration)
            .AddJdeIntegration( configuration);

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString(
                "DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string " +
                "'DefaultConnection' was not found.");

        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, options) =>
            {
                var auditableInterceptor =
                    serviceProvider
                        .GetRequiredService<
                            AuditableEntityInterceptor>();

                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(
                            typeof(ApplicationDbContext)
                                .Assembly
                                .FullName);

                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay:
                                TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    }).AddInterceptors(auditableInterceptor);
            });

        services.AddScoped<IUnitOfWork>(
            serviceProvider =>
                serviceProvider
                    .GetRequiredService<ApplicationDbContext>());
        return services;
    }

    private static IServiceCollection AddCTDIntegration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString(
                "CTDConnection")
            ?? throw new InvalidOperationException(
                "Connection string " +
                "'CTDConnection' was not found.");

        services.AddDbContext<CTDReadDbContext>(
            options =>
            {
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay:
                                TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    });
            });

        services.AddScoped<
            ICTDDataSource,
            CTDDataSource>();
        return services;

    }

    private static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped(
            typeof(IRepository<>),
            typeof(Repository<>));
        services.AddScoped<
    IProductBasicDataRepository,
    ProductBasicDataRepository>();
        services.AddScoped<
    IProductQueryRepository,
    ProductQueryRepository>();
        services.AddScoped<
    IActiveIngredientQueryRepository,
    ActiveIngredientQueryRepository>();

        services.AddScoped<
    ISupplierQueryRepository,
    SupplierQueryRepository>();

        services.AddScoped<
    IAttachmentStorage,
    LocalAttachmentStorage>();

        services.AddScoped<
    IProductLicenseRepository,
    ProductLicenseRepository>();

        services.AddScoped<
    IVariationRepository,
    VariationRepository>();
        services.AddScoped<
    IAppealRepository,
    AppealRepository>();

        services.AddScoped<
    IShelfLifeHistoryReadService,
    ShelfLifeHistoryReadService>();

        services.Configure<AttachmentStorageOptions>(
            configuration.GetSection(
        AttachmentStorageConstants.ConfigurationSection));

        services.AddScoped<
    IProductBasicDataReadService,
    ProductBasicDataReadService>();
        //services.AddScoped<AuditableEntityInterceptor>();
        services.AddHybridCache();

        services.AddSingleton(
            TimeProvider.System);
        return services;

    }
    private static IServiceCollection AddJdeIntegration(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString(
                "JDEConnection")
            ?? throw new InvalidOperationException(
                "JDEConnection was not configured.");

        services.AddDbContext<JdeReadDbContext>(
            options =>
            {
                options.UseOracle(
                    connectionString);
            });

        services.AddScoped<
            ISupplierDataSource,
            SupplierDataSource>();
        return services;
    }


    
}
public static class MigrationExtensions
{
    public static async Task MigrateDatabaseAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        await using var scope =
            serviceProvider.CreateAsyncScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync(
            cancellationToken);
    }
}