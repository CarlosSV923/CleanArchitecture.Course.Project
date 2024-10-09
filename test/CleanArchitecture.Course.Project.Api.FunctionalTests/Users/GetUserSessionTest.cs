using System.Net;
using System.Net.Http.Json;
using CleanArchitecture.Course.Project.Api.FunctionalTests.Infrastructure;
using CleanArchitecture.Course.Project.Application.Users.LoginUser;
using CleanArchitecture.Course.Project.Application.Users.RegisterUser;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Course.Project.Api.FunctionalTests.Users.GetUserSession
{
    public class GetUserSessionTest : BaseFunctionalTest
    {
        public GetUserSessionTest(FunctionalTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_Should_ReturnUnauthorized_When_TokenIsMissing()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/users/session");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Should_ReturnUserSession_When_TokenIsProvided()
        {
            // Arrange
            var token = await GetTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/users/session");
            request.Headers.Add("Authorization", $"Bearer {token}");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_Should_ReturnOk_When_UserExists()
        {
            // Arrange
            var requestData = new LoginUserRequest(
                UserData.RegisterUserReqTest.Email!,
                UserData.RegisterUserReqTest.Password!
            );

            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users/login")
            {
                Content = JsonContent.Create(requestData)
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Register_Should_ReturnOk_When_ReqIsValid()
        {
            // Arrange
            var requestData = new RegisterUserRequest()
            {
                Email = UserData.RegisterUserReqTest.Email!,
                Password = UserData.RegisterUserReqTest.Password!,
                Name = UserData.RegisterUserReqTest.Name!,
                LastName = UserData.RegisterUserReqTest.LastName!
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users/register")
            {
                Content = JsonContent.Create(requestData)
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}