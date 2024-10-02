using CleanArchitecture.Course.Project.Application.Users.RegisterUser;

namespace CleanArchitecture.Course.Project.Api.FunctionalTests.Users
{
    internal static class UserData
    {
        public static RegisterUserRequest RegisterUserReqTest = new()
        {
            Email = "test@mail.com",
            LastName = "test",
            Name = "test",
            Password = "test"
        };
    }
}