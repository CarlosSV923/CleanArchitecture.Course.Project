using CleanArchitecture.Course.Project.Application.Abstractions.Behaviors;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Course.Project.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddValidatorsFromAssembly(
                typeof(DependencyInjection).Assembly
            );
            services.AddTransient<PrecioService>();
            return services;
        }
    }
}