using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RAProductsRegestration.Application.Common.Behaviours;

namespace RAProductsRegestration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var applicationAssembly =
            typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(
            applicationAssembly);

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(
                applicationAssembly);

            configuration.AddOpenBehavior(
                typeof(UnhandledExceptionBehaviour<,>));

            configuration.AddOpenBehavior(
                typeof(ValidationBehaviour<,>));

            configuration.AddOpenBehavior(
                typeof(CachingBehaviour<,>));
        });

        return services;
    }
}