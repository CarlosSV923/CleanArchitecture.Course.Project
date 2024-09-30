using CleanArchitecture.Course.Project.Domain.Entities.Users;

namespace CleanArchitecture.Course.Project.Application.UnitTests.Users
{
    internal static class UserMock
    {
        public static readonly Name Name = new("John");

        public static readonly Email Email = new("test@email.com");

        public static readonly PasswordHash Password = new("123456");

        public static readonly LastName LastName = new("Doe");

        public static User Create()
        {
            return User.Create(Name, LastName, Email, Password);
        }
    }
}