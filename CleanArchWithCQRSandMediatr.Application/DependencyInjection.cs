using System.Reflection;

using CleanArchWithCQRSandMediatr.Application.Common.Behaviours;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace CleanArchWithCQRSandMediatr.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Scan the current assembly and register all FluentValidation validators (IValidator<T>).
        // This allows validators like CreateBlogCommandValidator to be resolved by DI.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(configuration: ctg =>
        {
            ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // Register the MediatR pipeline behavior that runs validators before handlers.
            // ValidationBehaviour<TRequest,TResponse> will be invoked for every MediatR request.
            ctg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }

}
