using Asp.Versioning;
using CleanArchitecture.Course.Project.Application.Abstractions.Clock;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Application.Abstractions.Email;
using CleanArchitecture.Course.Project.Application.Paginations;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using CleanArchitecture.Course.Project.Domain.Entities.Alquileres;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using CleanArchitecture.Course.Project.Domain.Entities.Vehiculos;
using CleanArchitecture.Course.Project.Infrastructure.Clock;
using CleanArchitecture.Course.Project.Infrastructure.Data;
using CleanArchitecture.Course.Project.Infrastructure.Email;
using CleanArchitecture.Course.Project.Infrastructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Course.Project.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {

            services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                }
            ).AddMvc()
            .AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                }
            );



            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaginationUserRepository, UserRepository>();
            
            services.AddScoped<IVehiculoRepository, VehiculoRepository>();
            services.AddScoped<IPaginationVehiculoRepository, VehiculoRepository>();
            
            services.AddScoped<IAlquilerRepository, AlquilerRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(
                _ => new SqlConnectionFactory(connectionString)
            );

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            return services;
        }
    }

}