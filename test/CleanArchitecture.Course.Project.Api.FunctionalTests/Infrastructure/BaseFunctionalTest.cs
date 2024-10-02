using System.Net.Http.Json;
using CleanArchitecture.Course.Project.Api.FunctionalTests.Users;
using CleanArchitecture.Course.Project.Application.Users.LoginUser;
using CleanArchitecture.Course.Project.Domain.Entities.Users;
using Xunit;

namespace CleanArchitecture.Course.Project.Api.FunctionalTests.Infrastructure
{
    public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
    {
        protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
        { 
            _client = factory.CreateClient();
        }

        protected readonly HttpClient _client;

        protected async Task<string> GetTokenAsync() {
            var response = await _client.PostAsJsonAsync("api/v1/users/login", new LoginUserRequest(
                UserData.RegisterUserReqTest.Email!,
                UserData.RegisterUserReqTest.Password!
            ));

            return await response.Content.ReadAsStringAsync();
        }
    }
}