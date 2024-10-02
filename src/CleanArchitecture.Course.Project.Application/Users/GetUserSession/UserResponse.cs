namespace CleanArchitecture.Course.Project.Application.Users.GetUserSession
{
    public sealed class UserResponse
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }
    }
}