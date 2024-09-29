using CleanArchitecture.Course.Project.Domain.Entities.Users;

namespace CleanArchitecture.Course.Project.Domain.UnitTests.Users
{
    internal class UserMock
    {
        public static readonly Name Name = new("John");
        public static readonly Email Email = new("mail@google.com");
        public static readonly LastName LastName = new("Doe");
        public static readonly PasswordHash Password = new("123");


    }
}