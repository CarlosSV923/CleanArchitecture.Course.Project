using CleanArchitecture.Course.Project.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CleanArchitecture.Course.Project.Application.IntegrationTests.Infrastructure
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestsWebAppFactory>
    {
        private readonly IServiceScope _scope;

        protected readonly ApplicationDbContext _dbContext;

        protected readonly ISender _sender;

        protected BaseIntegrationTest(IntegrationTestsWebAppFactory factory){
            
            _scope = factory.Services.CreateScope();
            _sender = _scope.ServiceProvider.GetRequiredService<ISender>();
            _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }
}