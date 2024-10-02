using System.Net.Http.Json;
using CleanArchitecture.Course.Project.Api.FunctionalTests.Users;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using CleanArchitecture.Course.Project.Infrastructure;
using CleanArchitecture.Course.Project.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Xunit;

namespace CleanArchitecture.Course.Project.Api.FunctionalTests.Infrastructure
{
    public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
            .WithImage("postgres:16.0")
            .WithDatabase("clean_architecture")
            .WithUsername("postgres")
            .WithPassword("postgres_1234")
            .Build();

        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            await CreateUserTestAsync();
        }

        public new async Task DisposeAsync()
        {
            await _container.StopAsync();
        }

        private async Task CreateUserTestAsync()
        {
            var httpClient = CreateClient();

            await httpClient.PostAsJsonAsync("api/v1/user/register", UserData.RegisterUserReqTest);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_container.GetConnectionString()).UseSnakeCaseNamingConvention();
                });

                services.RemoveAll(typeof(ISqlConnectionFactory));

                services.AddSingleton<ISqlConnectionFactory>(_ =>
                {
                    return new SqlConnectionFactory(_container.GetConnectionString());
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}